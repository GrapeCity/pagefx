using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CLI
{
	internal sealed class InterfaceImpl : ITypeCollection
	{
		private readonly AssemblyLoader _loader;
		private readonly IType _owner;
		private readonly int _ownerIndex;
		private readonly List<IType> _list = new List<IType>();
		private bool _loaded;

		public InterfaceImpl(AssemblyLoader loader, IType owner, int ownerIndex)
		{
			_loader = loader;
			_owner = owner;
			_ownerIndex = ownerIndex;
		}

		public int Count
		{
			get 
			{
				Load();
				return _list.Count;
			}
		}

		public IType this[int index]
		{
			get 
			{ 
				Load();
				return _list[index];
			}
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
			return type != null && this.Any(x => x == type);
		}

		public IEnumerator<IType> GetEnumerator()
		{
			for (int i = 0; i < Count; i++)
			{
				yield return this[i];
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private void Load()
		{
			if (_loaded) return;

			_loaded = true;

			var rows = _loader.Mdb.LookupRows(MdbTableId.InterfaceImpl, MDB.InterfaceImpl.Class, _ownerIndex, true);
			var ifaces = rows.Select(row =>
				{
					MdbIndex ifaceIndex = row[MDB.InterfaceImpl.Interface].Value;
					var iface = _loader.GetTypeDefOrRef(ifaceIndex, new Context(_owner));
					if (iface == null)
						throw new BadMetadataException();
					return iface;
				});

			_list.AddRange(ifaces);

			for (int i = 0; i < _list.Count; ++i)
			{
				var iface = _list[i];

				foreach (var ifaceMethod in iface.Methods)
					AddImplementedMethod(_owner, ifaceMethod);
			}
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
