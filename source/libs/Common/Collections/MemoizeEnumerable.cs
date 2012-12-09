using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.Common.Collections
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
			private readonly IEnumerator<T> _enumerator;
			private bool _cached;
			private static readonly object Sync = new object();

			public Memoizable(IEnumerable<T> source)
			{
				if (source == null)
					throw new ArgumentNullException("source");

				_enumerator = source.GetEnumerator();
			}

			public IEnumerator<T> GetEnumerator()
			{
				if (_cached)
				{
					foreach (var item in _list)
					{
						yield return item;
					}
					yield break;
				}

				var index = 0;

				while (true)
				{
					lock (Sync)
					{
						if (index < _list.Count)
						{
							yield return _list[index++];
						}
						else if (_enumerator.MoveNext())
						{
							var item = _enumerator.Current;
							_list.Add(item);
							index++;
							yield return item;
						}
						else
						{
							_cached = true;
							yield break;
						}
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
