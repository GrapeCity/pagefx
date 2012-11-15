using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.CLI.Tables
{
	internal sealed class MethodTable : MetadataTable<IMethod>
	{
		public MethodTable(AssemblyLoader loader)
			: base(loader, MdbTableId.MethodDef)
		{
		}

		public override MdbTableId Id
		{
			get { return MdbTableId.MethodDef; }
		}

		protected override IMethod ParseRow(int index)
		{
			MdbIndex entryPoint = Mdb.CLIHeader.EntryPointToken;
			bool checkEntryPoint = entryPoint.Table == MdbTableId.MethodDef;

			var row = Mdb.GetRow(MdbTableId.MethodDef, index);

			var implFlags = (MethodImplAttributes)row[MDB.MethodDef.ImplFlags].Value;
			var flags = (MethodAttributes)row[MDB.MethodDef.Flags].Value;

			bool isStatic = (flags & MethodAttributes.Static) != 0;
			var token = MdbIndex.MakeToken(MdbTableId.MethodDef, index + 1);

			var method = new Method
				{
					MetadataToken = token,
					Name = row[MDB.MethodDef.Name].String,
					ImplFlags = implFlags,
					Visibility = ToVisibility(flags),
					IsStatic = isStatic
				};

			var genericParams = Loader.GenericParameters.Find(token);
			foreach (var genericParameter in genericParams)
			{
				method.GenericParameters.Add(genericParameter);
				genericParameter.DeclaringMethod = method;
			}

			if (!isStatic)
			{
				method.IsAbstract = (flags & MethodAttributes.Abstract) != 0;
				method.IsFinal = (flags & MethodAttributes.Final) != 0;
				method.IsNewSlot = (flags & MethodAttributes.NewSlot) != 0;
				method.IsVirtual = (flags & MethodAttributes.Virtual) != 0;
			}

			method.IsSpecialName = (flags & MethodAttributes.SpecialName) != 0;
			method.IsRuntimeSpecialName = (flags & MethodAttributes.RTSpecialName) != 0;

			if (checkEntryPoint && entryPoint.Index - 1 == index)
			{
				method.IsEntryPoint = true;
				Loader.Assembly.EntryPoint = method;
			}

			LoadParams(method, row, index);

			uint rva = row[MDB.MethodDef.RVA].Value;
			if (rva != 0) //abstract or extern
			{
				method.Body = new LateMethodBody(Loader, method, rva);
			}

			return method;
		}

		private void LoadParams(IMethod method, MdbRow row, int index)
		{
			var parameters = Loader.Parameters;
			int paramNum = parameters.Count;
			int paramList = row[MDB.MethodDef.ParamList].Index - 1;
			int nextParamList;
			if (index + 1 < Count)
			{
				var nm = Mdb.GetRow(MdbTableId.MethodDef, index + 1);
				nextParamList = nm[MDB.MethodDef.ParamList].Index - 1;
			}
			else nextParamList = paramNum;

			for (int pi = paramList; pi < paramNum && pi < nextParamList; ++pi)
			{
				var param = parameters[pi];
				if (param.Index == 0)
				{
					//0 refers to the owner method's return type;
					continue;
				}
				method.Parameters.Add(param);
			}
		}

		private static Visibility ToVisibility(MethodAttributes f)
		{
			var v = f & MethodAttributes.MemberAccessMask;
			switch (v)
			{
				case MethodAttributes.PrivateScope:
					return Visibility.PrivateScope;
				case MethodAttributes.Private:
					return Visibility.Private;
				case MethodAttributes.FamANDAssem:
				case MethodAttributes.FamORAssem:
					return Visibility.ProtectedInternal;
				case MethodAttributes.Assembly:
					return Visibility.Internal;
				case MethodAttributes.Family:
					return Visibility.Protected;
			}
			return Visibility.Public;
		}
	}
}
