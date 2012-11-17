using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal sealed class TypeTable : MetadataTable<IType>
	{
		public TypeTable(AssemblyLoader loader) 
			: base(loader)
		{
		}

		public override MdbTableId Id
		{
			get { return MdbTableId.TypeDef; }
		}

		protected override IType ParseRow(MdbRow row, int index)
		{
			var flags = (TypeAttributes)row[MDB.TypeDef.Flags].Value;
			string name = row[MDB.TypeDef.TypeName].String;
			string ns = row[MDB.TypeDef.TypeNamespace].String;

			var token = MdbIndex.MakeToken(MdbTableId.TypeDef, index + 1);
			var genericParams = Loader.GenericParameters.Find(token);

			var type = CreateType(ns, name, flags, genericParams);

			// to avoid problems with self refs in fields/methods,etc
			Rows[index] = type;

			type.MetadataToken = token;

			//TODO: lazy resolving of class layout
			type.Layout = Loader.ClassLayout.Find(Mdb, index);

			//TODO: lazy resolving of declaring type
			var declType = ResolveDeclaringType(index);
			if (declType != null)
			{
				type.DeclaringType = declType;
				declType.Types.Add(type);
			}

			RegisterType(type);

			//TODO: lazy fields/methods collections
			SetFieldsAndMethods(row, index, type);

			//TODO: lazy resolving of base type
			SetBaseType(row, type);

			return type;
		}

		private void RegisterType(IType type)
		{
			var mod = Loader.MainModule;
			type.Module = mod;
			mod.Types.Add(type);
		}

		private void SetBaseType(MdbRow row, IType type)
		{
			if (type == SystemTypes.Object) return;

			MdbIndex baseIndex = row[MDB.TypeDef.Extends].Value;

			var baseType = Loader.GetTypeDefOrRef(baseIndex, new Context(type));
			type.BaseType = baseType;

			var thisType = type as UserDefinedType;
			if (thisType != null && thisType.TypeKind != TypeKind.Primitive)
			{
				if (baseType == SystemTypes.Enum)
					thisType.TypeKind = TypeKind.Enum;
				else if (baseType == SystemTypes.ValueType)
					thisType.TypeKind = TypeKind.Struct;
				else if (baseType == SystemTypes.Delegate || baseType == SystemTypes.MulticastDelegate)
					thisType.TypeKind = TypeKind.Delegate;
			}
		}

		private IType ResolveDeclaringType(int index)
		{
			//TODO: subject for PERF
			foreach (var row in Mdb.GetRows(MdbTableId.NestedClass))
			{
				int nestedIndex = row[MDB.NestedClass.Class].Index - 1;
				if (nestedIndex != index) continue;

				int enclosingIndex = row[MDB.NestedClass.EnclosingClass].Index - 1;
				return this[enclosingIndex];
			}
			return null;
		}

		private void SetFieldsAndMethods(MdbRow row, int index, IType type)
		{
			SetFields(row, index, type);
			SetMethods(row, index, type);
		}

		private void SetFields(MdbRow row, int index, IType type)
		{
			int from = row[MDB.TypeDef.FieldList].Index - 1;
			if (from < 0) return;

			var fields = Loader.Fields;
			int n = fields.Count;
			int to = n;

			if (index + 1 < Count)
			{
				var nextRow = Mdb.GetRow(MdbTableId.TypeDef, index + 1);
				to = nextRow[MDB.TypeDef.FieldList].Index - 1;
			}

			for (int i = from; i < n && i < to; ++i)
			{
				var f = fields[i];
				type.Members.Add(f);
			}
		}

		private void SetMethods(MdbRow row, int index, IType type)
		{
			int from = row[MDB.TypeDef.MethodList].Index - 1;
			if (from < 0) return;

			var methods = Loader.Methods;
			int n = methods.Count;
			int to = n;

			if (index + 1 < Count)
			{
				var nextRow = Mdb.GetRow(MdbTableId.TypeDef, index + 1);
				to = nextRow[MDB.TypeDef.MethodList].Index - 1;
			}

			for (int i = from; i < n && i < to; ++i)
			{
				var m = methods[i];
				type.Members.Add(m);
			}
		}

		private static UserDefinedType CreateType(string ns, string name, TypeAttributes flags, IList<IGenericParameter> genericParameters)
		{
			UserDefinedType type;
			bool isIface = IsInterface(flags);
			if (genericParameters != null && genericParameters.Any())
			{
				type = new GenericType
					{
						TypeKind = (isIface ? TypeKind.Interface : TypeKind.Class),
						Namespace = ns,
						Name = name
					};
				foreach (var parameter in genericParameters)
				{
					((GenericType)type).GenericParameters.Add(parameter);
					parameter.DeclaringType = type;
				}
			}
			else if (isIface)
			{
				type = new UserDefinedType(TypeKind.Interface)
					{
						Namespace = ns,
						Name = name
					};
			}
			else
			{
				type = CreateSystemType(ns, name)
				       ?? new UserDefinedType(TypeKind.Class)
					       {
						       Namespace = ns,
						       Name = name
					       };
			}

			SetTypeFlags(type, flags);

			return type;
		}

		private static UserDefinedType CreateSystemType(string ns, string name)
		{
			if (ns != SystemTypes.Namespace) return null;

			//TODO: PERF use dictionary
			foreach (var sysType in SystemTypes.Types)
			{
				if (name == sysType.Name)
				{
					var type = new UserDefinedType(sysType.Kind)
						{
							Namespace = ns,
							Name = name
						};
					sysType.Value = type;
					type.SystemType = sysType;
					return type;
				}
			}

			return null;
		}

		private static void SetTypeFlags(IType type, TypeAttributes flags)
		{
			type.Visibility = ToVisibility(flags);
			type.IsAbstract = (flags & TypeAttributes.Abstract) != 0;
			type.IsSealed = (flags & TypeAttributes.Sealed) != 0;
			type.IsBeforeFieldInit = (flags & TypeAttributes.BeforeFieldInit) != 0;
			type.IsSpecialName = (flags & TypeAttributes.SpecialName) != 0;
			type.IsRuntimeSpecialName = (flags & TypeAttributes.RTSpecialName) != 0;
		}

		private static bool IsInterface(TypeAttributes f)
		{
			return (f & TypeAttributes.ClassSemanticsMask) == TypeAttributes.Interface;
		}

		private static Visibility ToVisibility(TypeAttributes f)
		{
			var v = f & TypeAttributes.VisibilityMask;
			switch (v)
			{
				case TypeAttributes.Public:
					return Visibility.Public;
				case TypeAttributes.NestedFamily:
					return Visibility.NestedProtected;
				case TypeAttributes.NestedAssembly:
					return Visibility.NestedInternal;
				case TypeAttributes.NestedFamORAssem:
				case TypeAttributes.NestedFamANDAssem:
					return Visibility.NestedProtectedInternal;
				case TypeAttributes.NestedPrivate:
					return Visibility.NestedPrivate;
			}
			return Visibility.Internal;
		}
	}
}
