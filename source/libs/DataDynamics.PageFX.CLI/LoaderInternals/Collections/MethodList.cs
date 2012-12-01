using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CLI.LoaderInternals.Collections
{
	internal sealed class MethodList : IMethodCollection
	{
		private readonly AssemblyLoader _loader;
		private readonly IType _owner;
		private readonly int _from;
		private readonly int _to;
		private IDictionary<string, List<IMethod>> _lookup;
		private IReadOnlyList<IMethod> _list;
		private IReadOnlyList<IMethod> _ctors;
		private IMethod _cctor;
		private bool _resolveStaticCtor = true;
		private readonly List<IMethod> _instances = new List<IMethod>();

		public MethodList(AssemblyLoader loader, IType owner, int from, int to)
		{
			_loader = loader;
			_owner = owner;
			_from = from;
			_to = to;
		}

		public IEnumerable<IMethod> Find(string name)
		{
			if (_lookup == null)
			{
				_lookup = this.GroupBy(x => x.Name).ToDictionary(x => x.Key, x => x.ToList());
			}
			List<IMethod> list;
			return _lookup.TryGetValue(name, out list) ? list.AsReadOnly() : Enumerable.Empty<IMethod>();
		}

		public int Count
		{
			get { return List.Count + _instances.Count; }
		}

		public IMethod this[int index]
		{
			get
			{
				if (_instances.Count == 0)
					return List[index];
				if (index < 0 || index >= Count)
					throw new ArgumentOutOfRangeException("index");
				return index < List.Count ? List[index] : _instances[index - List.Count];
			}
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		public CodeNodeType NodeType
		{
			get { return CodeNodeType.Methods; }
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return this.Cast<ICodeNode>(); }
		}

		public object Tag { get; set; }

		public void Add(IMethod method)
		{
			if (method == null)
				throw new ArgumentNullException("method");

			if (!method.IsGenericInstance)
				throw new InvalidOperationException();

			_instances.Add(method);

			// update lookup
			if (_lookup != null)
			{
				List<IMethod> list;
				if (!_lookup.TryGetValue(method.Name, out list))
				{
					list = new List<IMethod>();
					_lookup.Add(method.Name, list);
				}

				list.Add(method);
			}
		}

		public IEnumerable<IMethod> Constructors
		{
			get { return _ctors ?? (_ctors = List.Where(x => !x.IsStatic && x.IsConstructor).AsReadOnlyList()); }
		}

		public IMethod StaticConstructor
		{
			get
			{
				if (_resolveStaticCtor)
				{
					_resolveStaticCtor = false;
					_cctor = List.FirstOrDefault(x => x.IsStatic && x.IsConstructor);
				}
				return _cctor;
			}
		}

		public IEnumerator<IMethod> GetEnumerator()
		{
			foreach (var method in List)
			{
				yield return method;
			}
			foreach (var method in _instances)
			{
				yield return method;
			}
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		private IReadOnlyList<IMethod> List
		{
			get { return _list ?? (_list = Populate().Memoize()); }
		}

		private IEnumerable<IMethod> Populate()
		{
			int n = _loader.Methods.Count;

			for (int i = _from; i < n && i < _to; ++i)
			{
				var method = _loader.Methods[i];

				method.DeclaringType = _owner;

				yield return method;
			}
		}
	}
}
