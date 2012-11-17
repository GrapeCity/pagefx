using System;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Tables
{
	internal sealed class MethodTable : MetadataTable<IMethod>
	{
		public MethodTable(AssemblyLoader loader)
			: base(loader)
		{
		}

		public override MdbTableId Id
		{
			get { return MdbTableId.MethodDef; }
		}

		protected override IMethod ParseRow(MdbRow row, int index)
		{
			MdbIndex entryPoint = Mdb.CLIHeader.EntryPointToken;
			bool checkEntryPoint = entryPoint.Table == MdbTableId.MethodDef;

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

			// for ref from types
			row.Object = method;

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

			var sigBlob = row[MDB.MethodDef.Signature].Blob;
			var sig = MdbSignature.DecodeMethodSignature(sigBlob);

			SetParams(method, row, index, sig);

			uint rva = row[MDB.MethodDef.RVA].Value;
			if (rva != 0) //abstract or extern
			{
				method.Body = new LateMethodBody(Loader, method, rva);
			}

			method.CustomAttributes = new CustomAttributes(Loader, method, token);

			method.TypeResolver = () => ResolveReturnType(method, sig);
			
			return method;
		}

		private void SetParams(Method method, MdbRow row, int index, MdbMethodSignature signature)
		{
			int from = row[MDB.MethodDef.ParamList].Index - 1;
			
			method.Parameters = new ParamList(Loader, method, from, signature, () => ResolveDeclType(method));
		}

		private IType ResolveReturnType(IMethod method, MdbMethodSignature sig)
		{
			var declType = method.DeclaringType ?? ResolveDeclType(method);
			if (declType == null)
			{
				throw new InvalidOperationException();
			}
			return ResolveReturnType(declType, method, sig);
		}

		private IType ResolveReturnType(IType declType, IMethod method, MdbMethodSignature sig)
		{
			var context = new Context(declType, method);

			var type = Loader.ResolveType(sig.Type, context);
			if (type == null)
				throw new InvalidOperationException();

			return type;
		}

		private IType ResolveDeclType(IMethod method)
		{
			return Loader.ResolveDeclType(method);
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
