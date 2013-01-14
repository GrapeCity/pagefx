using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Ecma335.IL;
using DataDynamics.PageFX.Ecma335.LoaderInternals.Collections;
using DataDynamics.PageFX.Ecma335.Metadata;

namespace DataDynamics.PageFX.Ecma335.LoaderInternals.Tables
{
	internal sealed class MethodTable : MetadataTable<IMethod>
	{
		public MethodTable(AssemblyLoader loader)
			: base(loader)
		{
		}

		public override TableId Id
		{
			get { return TableId.MethodDef; }
		}

		protected override IMethod ParseRow(MetadataRow row, int index)
		{
			var implFlags = (MethodImplAttributes)row[Schema.MethodDef.ImplFlags].Value;
			var flags = (MethodAttributes)row[Schema.MethodDef.Flags].Value;

			bool isStatic = (flags & MethodAttributes.Static) != 0;
			var token = SimpleIndex.MakeToken(TableId.MethodDef, index + 1);
			var sigBlob = row[Schema.MethodDef.Signature].Blob;
			var signature = MethodSignature.Decode(sigBlob);

			var method = new MethodImpl(Loader, signature)
				{
					MetadataToken = token,
					Name = row[Schema.MethodDef.Name].String,
					ImplFlags = implFlags,
					Visibility = ToVisibility(flags),
					IsStatic = isStatic,
					IsAbstract = !isStatic && (flags & MethodAttributes.Abstract) != 0,
					IsFinal = !isStatic && (flags & MethodAttributes.Final) != 0,
					IsNewSlot = !isStatic && (flags & MethodAttributes.NewSlot) != 0,
					IsVirtual = !isStatic && (flags & MethodAttributes.Virtual) != 0,
					IsSpecialName = (flags & MethodAttributes.SpecialName) != 0,
					IsRuntimeSpecialName = (flags & MethodAttributes.RTSpecialName) != 0
				};

			// for ref from types
			this[index] = method;

			var genericParams = Loader.GenericParameters.Find(token);
			foreach (var genericParameter in genericParams)
			{
				method.GenericParameters.Add(genericParameter);
				genericParameter.DeclaringMethod = method;
			}

			SimpleIndex entryPoint = Metadata.EntryPointToken;
			if (entryPoint.Table == TableId.MethodDef && entryPoint.Index - 1 == index)
			{
				method.IsEntryPoint = true;
			}

			method.Parameters = GetParams(method, row, signature);

			uint rva = row[Schema.MethodDef.RVA].Value;
			if (rva != 0) //abstract or extern
			{
				method.Body = new LateMethodBody(Loader, method, rva);
			}

			method.CustomAttributes = new CustomAttributes(Loader, method);

			return method;
		}

		private IParameterCollection GetParams(Method method, MetadataRow row, MethodSignature signature)
		{
			if (signature.Params.Length == 0)
				return ParameterCollection.Empty;

			int from = row[Schema.MethodDef.ParamList].Index - 1;
			if (from < 0) return ParameterCollection.Empty;

			return new ParamList(Loader, method, from, signature);
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

		public string GetFullName(int index)
		{
			var row = Metadata.GetRow(TableId.MethodDef, index);

			var name = row[Schema.MethodDef.Name].String;

			var typeIndex = Loader.Types.ResolveDeclTypeIndex(index, true);
			if (typeIndex < 0)
				throw new InvalidOperationException();

			return Loader.Types.GetFullName(typeIndex) + "." + name;
		}
	}
}
