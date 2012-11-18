using System.Linq;

namespace System.Collections.Generic
{
	/// <summary>
	/// Provides memoize extension for enumerables.
	/// </summary>
	public static class MemoizeEnumerable
	{
		/// <summary>
		/// Returns memoizable enumerable wrapper for specified enumerale.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="source">The enumerable to memoize.</param>
		/// <returns></returns>
		public static IReadOnlyList<T> Memoize<T>(this IEnumerable<T> source)
		{
			return new Memoizable<T>(source);
		}

		/// <summary>
		/// Implements <see cref="IEnumerable"/> with caching of items from input <see cref="IEnumerable"/>.
		/// </summary>
		/// <typeparam name="T">The type of sequence element.</typeparam>
		private sealed class Memoizable<T> : IReadOnlyList<T>
		{
			private readonly List<T> _list = new List<T>();
			private readonly IEnumerable<T> _source;
			private bool _cached;
			private IEnumerator<T> _enumerator;
			private static readonly object Sync = new object();

			public Memoizable(IEnumerable<T> source)
			{
				if (source == null) throw new ArgumentNullException("source");

				_source = source;
			}

			public IEnumerator<T> GetEnumerator()
			{
				foreach (var item in _list)
				{
					yield return item;
				}

				int count = _list.Count;

				if (_cached)
				{
					yield break;
				}

				lock (Sync)
				{
					if (_enumerator == null)
					{
						_enumerator = _source.GetEnumerator();
					}	
				}

				while (true)
				{
					while (count < _list.Count)
						yield return _list[count++];

					
					if (_enumerator == null || !_enumerator.MoveNext()) break;

					lock (Sync)
					{
						_list.Add(_enumerator.Current);
						count++;
					}

					yield return _enumerator.Current;
				}

				lock (Sync)
				{
					_cached = true;

					if (_enumerator != null)
					{
						_enumerator.Dispose();
						_enumerator = null;	
					}
				}
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public int Count
			{
				get { return _cached ? _list.Count : this.Count(); }
			}

			public T this[int index]
			{
				get
				{
					if (_cached || index < _list.Count)
						return _list[index];

					if (this.Any(item => _cached || index < _list.Count))
					{
						return _list[index];
					}

					throw new ArgumentOutOfRangeException("index");
				}
			}
		}
	}
}
