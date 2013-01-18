using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Core.Metadata;

namespace DataDynamics.PageFX.Core.LoaderInternals
{
	internal sealed class InternalMethod : Method
	{
		private readonly AssemblyLoader _loader;
		private readonly MethodSignature _signature;
		private bool _associationResolved;

		public InternalMethod(AssemblyLoader loader, MethodSignature signature)
		{
			_loader = loader;
			_signature = signature;
		}

		protected override IType ResolveDeclaringType()
		{
			var type = _loader.ResolveDeclType(this);
			if (type == null)
				throw new InvalidOperationException();
			return type;
		}

		protected override IType ResolveType()
		{
			var context = new Context(DeclaringType, this);

			var type = _loader.ResolveType(_signature.Type, context);
			if (type == null)
				throw new InvalidOperationException();

			return type;
		}

		protected override ITypeMember ResolveAssociation()
		{
			if (_associationResolved) return null;

			_associationResolved = true;

			SimpleIndex token = MetadataToken;

			var row = _loader.Metadata.LookupRow(TableId.MethodSemantics, Schema.MethodSemantics.Method, token.Index - 1, true);
			if (row == null) return null;
				
			var sem = (MethodSemanticsAttributes)row[Schema.MethodSemantics.Semantics].Value;

			SimpleIndex assoc = row[Schema.MethodSemantics.Association].Value;
			var index = assoc.Index - 1;
			switch (assoc.Table)
			{
				case TableId.Property:
					var property = _loader.Properties[index];

					Association = property;
					switch (sem)
					{
						case MethodSemanticsAttributes.Getter:
							property.Getter = this;
							break;
						case MethodSemanticsAttributes.Setter:
							property.Setter = this;
							break;
						default:
							throw new ArgumentOutOfRangeException();
					}

					property.ResolveTypeAndParameters();
						
					return property;

				case TableId.Event:
					var e = _loader.Events[index];

					Association = e;
					switch (sem)
					{
						case MethodSemanticsAttributes.AddOn:
							e.Adder = this;
							break;
						case MethodSemanticsAttributes.RemoveOn:
							e.Remover = this;
							break;
						case MethodSemanticsAttributes.Fire:
							e.Raiser = this;
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

		protected override IReadOnlyList<IMethod> ResolveImpls()
		{
			if (NoImpls(this))
			{
				return EmptyReadOnlyList.Create<IMethod>();
			}

			var explicitImpl = FindExplicitImpl();
			if (explicitImpl != null)
			{
				return new[] {explicitImpl}.AsReadOnlyList();
			}

			var list = new List<IMethod>();
			Implements = list.AsReadOnlyList();

			PopulateImplicitImpls(list);

			return Implements;
		}

		private static bool NoImpls(IMethod method)
		{
			if (method.IsStatic || method.IsConstructor)
				return true;

			// exclude protected, internal
			switch (method.Visibility)
			{
				case Visibility.NestedProtected:
				case Visibility.NestedProtectedInternal:
				case Visibility.NestedInternal:
				case Visibility.Protected:
				case Visibility.ProtectedInternal:
				case Visibility.Internal:
					return true;
			}

			return method.DeclaringType.IsInterface;
		}

		private void PopulateImplicitImpls(List<IMethod> list)
		{
			var declType = DeclaringType;
			var explictImpls = declType.Methods.Where(x => !NoImpls(x) && x.IsExplicitImplementation).Select(x => x.Implements[0]).ToList();

			// get iface methods that has no explicit impls
			var ifaces = declType.Interfaces.SelectMany(x => x.Methods).Where(x => explictImpls.All(t => t != x));

			var impls = ifaces.Where(x => Signature.Equals(this, x, true));
			list.AddRange(impls);

			// get impls of base method
			if (IsOverride && BaseMethod != null && Signature.TypeEquals(BaseMethod.Type, Type))
			{
				list.AddRange(BaseMethod.Implements);
			}
		}

		private IMethod FindExplicitImpl()
		{
			var declType = DeclaringType;
			var typeIndex = declType.RowIndex();
			var rows = _loader.Metadata.LookupRows(TableId.MethodImpl, Schema.MethodImpl.Class, typeIndex, true);
			var context = new Context(declType);

			foreach (var row in rows)
			{
				SimpleIndex bodyIdx = row[Schema.MethodImpl.MethodBody].Value;
				SimpleIndex declIdx = row[Schema.MethodImpl.MethodDeclaration].Value;

				var impl = _loader.GetMethodDefOrRef(bodyIdx, context);
				if (impl == null)
					throw new InvalidOperationException();

				var iface = _loader.GetMethodDefOrRef(declIdx, new Context(declType, impl));
				if (iface == null)
					throw new InvalidOperationException();

				impl.IsExplicitImplementation = true;

				if (impl == this)
				{
					return iface;
				}

				impl.Implements = new[] {iface}.AsReadOnlyList();
			}

			return null;
		}
	}
}