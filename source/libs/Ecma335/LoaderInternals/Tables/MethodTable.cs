using System;
using DataDynamics.PageFX.Common.Metadata;
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

			var method = new Method
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
			row.Object = method;

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

			var sigBlob = row[Schema.MethodDef.Signature].Blob;
			var signature = MethodSignature.Decode(sigBlob);

			method.Meta = new MetaMethod(Loader, method, signature);

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

		private sealed class MetaMethod : IMetaMethod
		{
			private readonly AssemblyLoader _loader;
			private readonly Method _method;
			private readonly MethodSignature _signature;
			private IType _type;
			private IType _declType;
			private ITypeMember _association;
			private bool _associationResolved;

			public MetaMethod(AssemblyLoader loader, Method method, MethodSignature signature)
			{
				_loader = loader;
				_method = method;
				_signature = signature;
			}

			public IType Type
			{
				get { return _type ?? (_type = ResolveType()); }
			}

			public IType DeclaringType
			{
				get { return _declType ?? (_declType = ResolveDeclType()); }
			}

			public ITypeMember Association
			{
				get
				{
					if (_associationResolved) return _association;
					_associationResolved = true;
					return (_association = ResolveAssociation());
				}
			}

			private IType ResolveDeclType()
			{
				var type = _loader.ResolveDeclType(_method);
				if (type == null)
					throw new InvalidOperationException();
				return type;
			}

			private IType ResolveType()
			{
				var context = new Context(DeclaringType, _method);

				var type = _loader.ResolveType(_signature.Type, context);
				if (type == null)
					throw new InvalidOperationException();

				return type;
			}

			private ITypeMember ResolveAssociation()
			{
				SimpleIndex token = _method.MetadataToken;

				var row = _loader.Metadata.LookupRow(TableId.MethodSemantics, Schema.MethodSemantics.Method, token.Index - 1, true);
				if (row == null) return null;
				
				var sem = (MethodSemanticsAttributes)row[Schema.MethodSemantics.Semantics].Value;

				SimpleIndex assoc = row[Schema.MethodSemantics.Association].Value;
				switch (assoc.Table)
				{
					case TableId.Property:
						var property = _loader.Properties[assoc.Index - 1];

						_method.Association = property;
						switch (sem)
						{
							case MethodSemanticsAttributes.Getter:
								property.Getter = _method;
								break;
							case MethodSemanticsAttributes.Setter:
								property.Setter = _method;
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}

						property.ResolveTypeAndParameters();
						
						return property;

					case TableId.Event:
						var e = _loader.Events[assoc.Index - 1];

						_method.Association = e;
						switch (sem)
						{
							case MethodSemanticsAttributes.AddOn:
								e.Adder = _method;
								break;
							case MethodSemanticsAttributes.RemoveOn:
								e.Remover = _method;
								break;
							case MethodSemanticsAttributes.Fire:
								e.Raiser = _method;
								break;
							default:
								throw new ArgumentOutOfRangeException();
						}

						e.ResolveType();

						return e;

					default:
						throw new ArgumentOutOfRangeException();
				}
			}
		}
	}
}
