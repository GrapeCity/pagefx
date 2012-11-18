using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CLI
{
	internal sealed class MethodList : IMethodCollection
	{
		private readonly AssemblyLoader _loader;
		private readonly IType _owner;
		private readonly int _from;
		private readonly int _to;
		private IDictionary<string, IMethod[]> _lookup;
		private IReadOnlyList<IMethod> _list;
		private IList<IMethod> _ctors;
		private IMethod _cctor;
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
				_lookup = this.GroupBy(x => x.Name).ToDictionary(x => x.Key, x => x.ToArray());
			}
			IMethod[] list;
			return _lookup.TryGetValue(name, out list) ? list : Enumerable.Empty<IMethod>();
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
		}

		public IEnumerable<IMethod> Constructors
		{
			get
			{
				Load();
				return _ctors;
			}
		}

		public IMethod StaticConstructor
		{
			get
			{
				Load();
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
			_ctors = new List<IMethod>();

			int n = _loader.Methods.Count;
			for (int i = _from; i < n && i < _to; ++i)
			{
				var method = _loader.Methods[i];

				if (method.IsConstructor)
				{
					_ctors.Add(method);
					if (method.IsStatic)
						_cctor = method;
				}

				method.DeclaringType = _owner;

				yield return method;
			}
		}

		private void Load()
		{
			foreach (var method in List){}
		}
	}
}
