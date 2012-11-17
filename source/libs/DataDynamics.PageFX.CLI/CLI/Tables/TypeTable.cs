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

			bool isIface = IsInterface(flags);
			var kind = isIface ? TypeKind.Interface : TypeKind.Class;
			var sysType = !isIface && ns == SystemTypes.Namespace ? SystemTypes.Find(name) : null;
			if (sysType != null) kind = sysType.Kind;

			var type = CreateType(kind, genericParams);

			type.Namespace = ns;
			type.Name = name;
			SetTypeFlags(type, flags);

			if (sysType != null)
			{
				sysType.Value = type;
				type.SystemType = sysType;
			}

			// to avoid problems with self refs in fields/methods,etc
			row.Object = type;

			type.MetadataToken = token;
			type.CustomAttributes = new CustomAttributes(Loader, type, token);

			//TODO: lazy resolving of class layout
			type.Layout = ResolveLayout(Mdb, index);

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

			type.Interfaces = new InterfaceImpl(Loader, type, index);

			LoadMethodImpl(type, index);

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
			if (type.FullName == "System.Object") return;

			MdbIndex baseIndex = row[MDB.TypeDef.Extends].Value;

			var baseType = Loader.GetTypeDefOrRef(baseIndex, new Context(type));
			type.BaseType = baseType;

			var thisType = type as UserDefinedType;
			if (thisType != null && thisType.TypeKind != TypeKind.Primitive)
			{
				if (baseType.FullName == "System.Enum")
					thisType.TypeKind = TypeKind.Enum;
				else if (baseType.FullName == "System.ValueType")
					thisType.TypeKind = TypeKind.Struct;
				else if (baseType.FullName == "System.Delegate" || baseType.FullName == "System.MulticastDelegate")
					thisType.TypeKind = TypeKind.Delegate;
			}
		}

		private static ClassLayout ResolveLayout(MdbReader mdb, int typeIndex)
		{
			var row = mdb.LookupRow(MdbTableId.ClassLayout, MDB.ClassLayout.Parent, typeIndex, true);
			if (row == null)
				return null;

			var size = (int)row[MDB.ClassLayout.ClassSize].Value;
			var pack = (int)row[MDB.ClassLayout.PackingSize].Value;
			return new ClassLayout(size, pack);
		}

		private IType ResolveDeclaringType(int index)
		{
			var row = Mdb.LookupRow(MdbTableId.NestedClass, MDB.NestedClass.Class, index, true);
			if (row == null) return null;

			int enclosingIndex = row[MDB.NestedClass.EnclosingClass].Index - 1;
			return this[enclosingIndex];
		}

		private void SetFieldsAndMethods(MdbRow row, int index, IType type)
		{
			var fields = GetFields(row, index, type);
			var methods = GetMethods(row, index, type);

			//TODO: remove, lazy loading
			foreach (var field in fields)
			{
			}

			foreach (var method in methods)
			{
			}

			var members = (TypeMemberCollection)type.Members;
			members.Fields = fields;			
			members.Methods = methods;
		}

		private IFieldCollection GetFields(MdbRow row, int index, IType type)
		{
			int from = row[MDB.TypeDef.FieldList].Index - 1;
			if (from < 0) return FieldCollection.Empty;

			var fields = Loader.Fields;
			int n = fields.Count;
			int to = n;

			if (index + 1 < Count)
			{
				var nextRow = Mdb.GetRow(MdbTableId.TypeDef, index + 1);
				to = nextRow[MDB.TypeDef.FieldList].Index - 1;
			}

			return new FieldList(Loader, type, from, to);
		}

		private IMethodCollection GetMethods(MdbRow row, int index, IType type)
		{
			int from = row[MDB.TypeDef.MethodList].Index - 1;
			if (from < 0) return MethodCollection.Empty;

			var methods = Loader.Methods;
			int n = methods.Count;
			int to = n;

			if (index + 1 < Count)
			{
				var nextRow = Mdb.GetRow(MdbTableId.TypeDef, index + 1);
				to = nextRow[MDB.TypeDef.MethodList].Index - 1;
			}

			return new MethodList(Loader, type, from, to);
		}

		private void LoadMethodImpl(IType type, int typeIndex)
		{
			var rows = Mdb.LookupRows(MdbTableId.MethodImpl, MDB.MethodImpl.Class, typeIndex, true);
			foreach (var row in rows)
			{
				MdbIndex bodyIdx = row[MDB.MethodImpl.MethodBody].Value;
				MdbIndex declIdx = row[MDB.MethodImpl.MethodDeclaration].Value;

				var body = Loader.GetMethodDefOrRef(bodyIdx, new Context(type));

				var decl = Loader.GetMethodDefOrRef(declIdx, new Context(type, body));

				body.ImplementedMethods = new[] { decl };
				body.IsExplicitImplementation = true;
			}
		}

		private static UserDefinedType CreateType(TypeKind kind, IList<IGenericParameter> genericParameters)
		{
			if (genericParameters != null && genericParameters.Any())
			{
				var type = new GenericType {TypeKind = kind};
				foreach (var parameter in genericParameters)
				{
					type.GenericParameters.Add(parameter);
					parameter.DeclaringType = type;
				}
				return type;
			}
			return new UserDefinedType(kind);
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
