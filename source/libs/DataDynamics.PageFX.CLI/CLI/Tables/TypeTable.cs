using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.CLI.Tables
{
	internal sealed class TypeTable : MetadataTable<IType>
	{
		public TypeTable(AssemblyLoader loader) 
			: base(loader, MdbTableId.TypeDef)
		{
		}

		public override MdbTableId Id
		{
			get { return MdbTableId.TypeDef; }
		}

		protected override IType ParseRow(int index)
		{
			var row = Mdb.GetRow(MdbTableId.TypeDef, index);

			var flags = (TypeAttributes)row[MDB.TypeDef.Flags].Value;
			string name = row[MDB.TypeDef.TypeName].String;
			string ns = row[MDB.TypeDef.TypeNamespace].String;

			var token = MdbIndex.MakeToken(MdbTableId.TypeDef, index + 1);
			var genericParams = Loader.GenericParameters.Find(token);

			var type = CreateType(ns, name, flags, genericParams);

			type.MetadataToken = token;
			type.Name = name;
			type.Namespace = ns;
			type.Layout = Loader.ClassLayout.Find(Mdb, index);

			SetFieldsAndMethods(row, index, type);

			return type;
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
					TypeKind = (isIface ? TypeKind.Interface : TypeKind.Class)
				};
				foreach (var parameter in genericParameters)
				{
					((GenericType)type).GenericParameters.Add(parameter);
					parameter.DeclaringType = type;
				}
			}
			else if (isIface)
			{
				type = new UserDefinedType(TypeKind.Interface);
			}
			else
			{
				type = CreateSystemType(ns, name) ?? new UserDefinedType(TypeKind.Class);
			}
			SetTypeFlags(type, flags);
			return type;
		}

		private static UserDefinedType CreateSystemType(string ns, string name)
		{
			if (ns == SystemTypes.Namespace)
			{
				foreach (var sysType in SystemTypes.Types)
				{
					if (name == sysType.Name)
					{
						var type = new UserDefinedType(sysType.Kind);
						sysType.Value = type;
						type.SystemType = sysType;
						return type;
					}
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

		static Visibility ToVisibility(TypeAttributes f)
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
