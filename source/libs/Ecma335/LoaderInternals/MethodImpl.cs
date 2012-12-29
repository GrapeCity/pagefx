using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Metadata;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Ecma335.Metadata;

namespace DataDynamics.PageFX.Ecma335.LoaderInternals
{
	internal sealed class MethodImpl : Method
	{
		private readonly AssemblyLoader _loader;
		private readonly MethodSignature _signature;
		private bool _associationResolved;

		public MethodImpl(AssemblyLoader loader, MethodSignature signature)
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
			switch (assoc.Table)
			{
				case TableId.Property:
					var property = _loader.Properties[assoc.Index - 1];

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
					var e = _loader.Events[assoc.Index - 1];

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
				return EmptyReadOnlyList.Create<IMethod>();

			var list = new List<IMethod>();
			Implements = list.AsReadOnlyList();

			PopulateImpls(list);

			return Implements;
		}

		private static bool NoImpls(IMethod method)
		{
			if (method.IsStatic || method.IsConstructor)
				return true;

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

		private void PopulateImpls(List<IMethod> list)
		{
			var declType = DeclaringType;
				
			var explicitImpl = FindExplicitImpl();
			if (explicitImpl != null)
			{
				list.Add(explicitImpl);
				return;
			}

			//TODO: Add impls of base method

			var typeMethods =
				declType.Methods
				        .Where(x => x != this && x != ProxyOf && x != InstanceOf && !NoImpls(x))
				        .ToList();

			var ifaces = declType.Interfaces.SelectMany(x => x.Methods);
			var impls = ifaces
				.Where(x => Signature.Equals(this, x, true) && !HasExplicitImpl(typeMethods, x));

			list.AddRange(impls);
		}

		private static bool HasExplicitImpl(IEnumerable<IMethod> typeMethods, IMethod ifaceMethod)
		{
			return typeMethods.Any(x => x.IsExplicitImplementation && x.Implements[0] == ifaceMethod);
		}

		private IMethod FindExplicitImpl()
		{
			var declType = DeclaringType;
			var typeIndex = declType.RowIndex();
			var rows = _loader.Metadata.LookupRows(TableId.MethodImpl, Schema.MethodImpl.Class, typeIndex, true);

			foreach (var row in rows)
			{
				SimpleIndex bodyIdx = row[Schema.MethodImpl.MethodBody].Value;
				var body = _loader.GetMethodDefOrRef(bodyIdx, new Context(declType));
				if (body == this)
				{
					SimpleIndex declIdx = row[Schema.MethodImpl.MethodDeclaration].Value;
					var decl = _loader.GetMethodDefOrRef(declIdx, new Context(declType, body));
					body.IsExplicitImplementation = true;
					return decl;
				}
			}

			return null;
		}
	}
}