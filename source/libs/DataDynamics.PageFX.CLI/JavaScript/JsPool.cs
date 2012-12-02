using System.Collections;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CLI.JavaScript
{
	internal sealed class JsPool<T> : IEnumerable<T> where T:class,IJsNameValuePair,new()
	{
		private readonly List<T> _list = new List<T>();
		private readonly Dictionary<object, T> _cache = new Dictionary<object, T>();
		private readonly Dictionary<string, int> _names = new Dictionary<string, int>();

		public T this[object key]
		{
			get
			{
				T value;
				if (_cache.TryGetValue(key, out value))
					return value;
				return null;
			}
		}

		public T Add(object key, JsNode value)
		{
			var name = key as string;
			if (name == null)
			{
				var prefix = GetPrefix(key);
				int count;
				if (_names.TryGetValue(prefix, out count))
				{
					count++;
				}
				else
				{
					count = 1;
				}
				_names[prefix] = count;
				name = prefix + count;
			}

			var item = new T {Name = name, Value = value};

			_cache.Add(key, item);
			_list.Add(item);

			return item;
		}

		private static string GetPrefix(object key)
		{
			if (key is IMethod) return "m";
			if (key is IField) return "f";
			return "v";
		}

		public IEnumerator<T> GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
}