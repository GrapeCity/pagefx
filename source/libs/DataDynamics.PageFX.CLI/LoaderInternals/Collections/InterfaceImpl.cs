using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.Common;
using DataDynamics.PageFX.Common.Syntax;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.CLI.LoaderInternals.Collections
{
	internal sealed class InterfaceImpl : ITypeCollection
	{
		private readonly AssemblyLoader _loader;
		private readonly IType _owner;
		private IReadOnlyList<IType> _list;

		public InterfaceImpl(AssemblyLoader loader, IType owner)
		{
			_loader = loader;
			_owner = owner;
		}

		private int OwnerIndex
		{
			get { return _owner.RowIndex(); }
		}

		public int Count
		{
			get { return List.Count; }
		}

		public IType this[int index]
		{
			get { return List[index]; }
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		public CodeNodeType NodeType
		{
			get { return CodeNodeType.Types; }
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return this.Cast<ICodeNode>(); }
		}

		public object Tag { get; set; }

		public IType this[string fullname]
		{
			get { return this.FirstOrDefault(t => t.FullName == fullname); }
		}

		public void Add(IType type)
		{
			throw new NotSupportedException();
		}

		public bool Contains(IType type)
		{
			return type != null && this.Any(x => ReferenceEquals(x, type));
		}

		public IEnumerator<IType> GetEnumerator()
		{
			return List.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private IReadOnlyList<IType> List
		{
			get { return _list ?? (_list = Populate().Memoize()); }
		}

		private IEnumerable<IType> Populate()
		{
			var rows = _loader.Metadata.LookupRows(TableId.InterfaceImpl, Schema.InterfaceImpl.Class, OwnerIndex, true);
			return rows.Select(row =>
				{
					SimpleIndex ifaceIndex = row[Schema.InterfaceImpl.Interface].Value;
					var iface = _loader.GetTypeDefOrRef(ifaceIndex, new Context(_owner));
					if (iface == null)
						throw new BadMetadataException();
					foreach (var ifaceMethod in iface.Methods)
						AddImplementedMethod(_owner, ifaceMethod);
					return iface;
				});
		}

		private static void AddImplementedMethod(IType type, IMethod ifaceMethod)
		{
			if (HasExplicitImplementation(type, ifaceMethod))
				return;

			var method = FindImpl(type, ifaceMethod);
			if (method == null) return;

			if (method.ImplementedMethods == null)
			{
				method.ImplementedMethods = new[] { ifaceMethod };
				return;
			}

			if (method.ImplementedMethods.Contains(ifaceMethod))
				return;

			int n = method.ImplementedMethods.Length;
			var newArr = new IMethod[n + 1];
			Array.Copy(method.ImplementedMethods, newArr, n);
			newArr[n] = ifaceMethod;
			method.ImplementedMethods = newArr;
		}

		private static IMethod FindImpl(IType type, IMethod ifaceMethod)
		{
			string mname = ifaceMethod.Name;
			while (type != null)
			{
				var candidates = type.Methods.Find(mname);
				foreach (var method in candidates)
				{
					if (method.IsExplicitImplementation) continue;
					if (Signature.Equals(method, ifaceMethod, true, false))
						return method;
				}
				type = type.BaseType;
			}
			return null;
		}

		private static bool HasExplicitImplementation(IType type, IMethod ifaceMethod)
		{
			return (from method in type.Methods
			        where method.IsExplicitImplementation
			        select method.ImplementedMethods).Any(impl => impl != null && impl.Length == 1 && impl[0] == ifaceMethod);
		}
	}
}
