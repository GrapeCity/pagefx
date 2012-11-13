namespace System.Collections.Generic
{
	public interface ITuple<T> : IReadOnlyList<T>
	{
		T[] ToArray();
	}

	public static class Pair
	{
		public static Pair<T> New<T>(T first, T second)
		{
			return new Pair<T>(first, second);
		}
	}

	public struct Pair<T> :
		ITuple<T>, IComparable,
		IComparable<Pair<T>>, IEquatable<Pair<T>>,
		IComparable<ITuple<T>>, IEquatable<ITuple<T>>
	{
		public readonly T First;
		public readonly T Second;

		public Pair(T first, T second)
		{
			First = first;
			Second = second;
		}

		public int Count
		{
			get { return 2; }
		}

		public T this[int index]
		{
			get
			{
				switch (index)
				{
					case 0:
						return First;
					case 1:
						return Second;
					default:
						throw new ArgumentOutOfRangeException("index");
				}
			}
		}

		public T[] ToArray()
		{
			return new[] {First, Second};
		}

		public int CompareTo(Pair<T> other)
		{
			int result = Comparer<T>.Default.Compare(First, other.First);
			if (result != 0) return result;

			result = Comparer<T>.Default.Compare(Second, other.Second);

			return result;
		}

		public int CompareTo(ITuple<T> other)
		{
			if (other == null)
				throw new ArgumentNullException("other");
			if (other.Count != 2)
				throw new InvalidOperationException();

			int result = Comparer<T>.Default.Compare(First, other[0]);
			if (result != 0) return result;

			result = Comparer<T>.Default.Compare(Second, other[1]);

			return result;
		}

		public int CompareTo(object obj)
		{
			return CompareTo((ITuple<T>)obj);
		}

		public bool Equals(Pair<T> other)
		{
			return Equals(First, other.First) && Equals(Second, other.Second);
		}

		public bool Equals(ITuple<T> other)
		{
			return other != null && other.Count == 2
			       && Equals(First, other[0])
			       && Equals(Second, other[1]);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ITuple<T>);
		}

		public override int GetHashCode()
		{
			return First.GetHashCode() ^ Second.GetHashCode();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			yield return First;
			yield return Second;
		}

		public override string ToString()
		{
			return String.Format("({0}, {1})", First, Second);
		}
	}

	public static class Triplet
	{
		public static Triplet<T> New<T>(T first, T second, T third)
		{
			return new Triplet<T>(first, second, third);
		}
	}

	public struct Triplet<T> :
		ITuple<T>, IComparable,
		IComparable<Triplet<T>>, IEquatable<Triplet<T>>,
		IComparable<ITuple<T>>, IEquatable<ITuple<T>>
	{
		public readonly T First;
		public readonly T Second;
		public readonly T Third;

		public Triplet(T first, T second, T third)
		{
			First = first;
			Second = second;
			Third = third;
		}

		public int Count
		{
			get { return 3; }
		}

		public T this[int index]
		{
			get
			{
				switch (index)
				{
					case 0:
						return First;
					case 1:
						return Second;
					case 2:
						return Third;
					default:
						throw new ArgumentOutOfRangeException("index");
				}
			}
		}

		public T[] ToArray()
		{
			return new[] {First, Second, Third};
		}

		public int CompareTo(Triplet<T> other)
		{
			int result = Comparer<T>.Default.Compare(First, other.First);
			if (result != 0) return result;

			result = Comparer<T>.Default.Compare(Second, other.Second);
			if (result != 0) return result;

			result = Comparer<T>.Default.Compare(Third, other.Third);

			return result;
		}

		public int CompareTo(ITuple<T> other)
		{
			if (other == null)
				return 1;
			if (other.Count != 3)
				throw new InvalidOperationException();

			int result = Comparer<T>.Default.Compare(First, other[0]);
			if (result != 0) return result;

			result = Comparer<T>.Default.Compare(Second, other[1]);
			if (result != 0) return result;

			result = Comparer<T>.Default.Compare(Third, other[2]);

			return result;
		}

		public int CompareTo(object obj)
		{
			return CompareTo((ITuple<T>)obj);
		}

		public bool Equals(Triplet<T> other)
		{
			return Equals(First, other.First)
			       && Equals(Second, other.Second)
			       && Equals(Third, other.Third);
		}

		public bool Equals(ITuple<T> other)
		{
			return other != null && other.Count == 3
			       && Equals(First, other[0])
			       && Equals(Second, other[1])
			       && Equals(Third, other[2]);
		}

		public override int GetHashCode()
		{
			return First.GetHashCode() ^ Second.GetHashCode() ^ Third.GetHashCode();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public IEnumerator<T> GetEnumerator()
		{
			yield return First;
			yield return Second;
			yield return Third;
		}

		public override string ToString()
		{
			return String.Format("({0}, {1}, {2})", First, Second, Third);
		}
	}

	public sealed class Tuple<T> :
		ITuple<T>, IComparable,
		IComparable<Tuple<T>>, IEquatable<Tuple<T>>,
		IComparable<ITuple<T>>, IEquatable<ITuple<T>>
	{
		public readonly T Item1;

		public Tuple(T value)
		{
			Item1 = value;
		}

		public T[] ToArray()
		{
			return new[] {Item1};
		}

		public IEnumerator<T> GetEnumerator()
		{
			yield return Item1;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count
		{
			get { return 1; }
		}

		public T this[int index]
		{
			get
			{
				switch (index)
				{
					case 0:
						return Item1;
					default:
						throw new ArgumentOutOfRangeException("index");
				}
			}
		}

		public int CompareTo(Tuple<T> other)
		{
			return other == null ? 1 : Comparer<T>.Default.Compare(Item1, other.Item1);
		}

		public int CompareTo(ITuple<T> other)
		{
			if (other == null)
				return 1;
			if (other.Count != 1)
				throw new InvalidOperationException();
			return Comparer<T>.Default.Compare(Item1, other[0]);
		}

		public int CompareTo(object obj)
		{
			return CompareTo((ITuple<T>)obj);
		}

		public bool Equals(Tuple<T> other)
		{
			return other != null && Equals(Item1, other.Item1);
		}

		public bool Equals(ITuple<T> other)
		{
			return other != null && other.Count == 1
			       && Equals(Item1, other[0]);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as ITuple<T>);
		}

		public override int GetHashCode()
		{
			return Item1.GetHashCode();
		}

		public override string ToString()
		{
			return String.Format("({0})", Item1);
		}
	}

	public sealed class Tuple<T1, T2> :
		IComparable,
		IComparable<Tuple<T1, T2>>,
		IEquatable<Tuple<T1, T2>>,
		IEnumerable
	{
		public readonly T1 Item1;
		public readonly T2 Item2;

		public Tuple(T1 item1, T2 item2)
		{
			Item1 = item1;
			Item2 = item2;
		}

		public IEnumerator GetEnumerator()
		{
			yield return Item1;
			yield return Item2;
		}

		public int CompareTo(Tuple<T1, T2> other)
		{
			if (other == null)
				return 1;

			int result = Comparer<T1>.Default.Compare(Item1, other.Item1);
			if (result != 0) return result;

			result = Comparer<T2>.Default.Compare(Item2, other.Item2);

			return result;
		}

		public int CompareTo(object obj)
		{
			return CompareTo((Tuple<T1, T2>)obj);
		}

		public bool Equals(Tuple<T1, T2> other)
		{
			return other != null
			       && Equals(Item1, other.Item1)
			       && Equals(Item2, other.Item2);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Tuple<T1, T2>);
		}

		public override int GetHashCode()
		{
			return Item1.GetHashCode() ^ Item2.GetHashCode();
		}

		public override string ToString()
		{
			return String.Format("({0}, {1})", Item1, Item2);
		}
	}

	public sealed class Tuple<T1, T2, T3> :
		IComparable,
		IComparable<Tuple<T1, T2, T3>>,
		IEquatable<Tuple<T1, T2, T3>>,
		IEnumerable
	{
		public readonly T1 Item1;
		public readonly T2 Item2;
		public readonly T3 Item3;

		public Tuple(T1 item1, T2 item2, T3 item3)
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
		}

		public IEnumerator GetEnumerator()
		{
			yield return Item1;
			yield return Item2;
			yield return Item3;
		}

		public int CompareTo(Tuple<T1, T2, T3> other)
		{
			if (other == null)
				return 1;

			int result = Comparer<T1>.Default.Compare(Item1, other.Item1);
			if (result != 0) return result;

			result = Comparer<T2>.Default.Compare(Item2, other.Item2);
			if (result != 0) return result;

			result = Comparer<T3>.Default.Compare(Item3, other.Item3);

			return result;
		}

		public int CompareTo(object obj)
		{
			return CompareTo((Tuple<T1, T2, T3>)obj);
		}

		public bool Equals(Tuple<T1, T2, T3> other)
		{
			return other != null
			       && Equals(Item1, other.Item1)
			       && Equals(Item2, other.Item2)
			       && Equals(Item3, other.Item3);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Tuple<T1, T2, T3>);
		}

		public override int GetHashCode()
		{
			return Item1.GetHashCode() ^ Item2.GetHashCode() ^ Item3.GetHashCode();
		}

		public override string ToString()
		{
			return String.Format("({0}, {1}, {2})", Item1, Item2, Item3);
		}
	}

	public sealed class Tuple<T1, T2, T3, T4> :
		IComparable,
		IComparable<Tuple<T1, T2, T3, T4>>,
		IEquatable<Tuple<T1, T2, T3, T4>>,
		IEnumerable
	{
		public readonly T1 Item1;
		public readonly T2 Item2;
		public readonly T3 Item3;
		public readonly T4 Item4;

		public Tuple(T1 item1, T2 item2, T3 item3, T4 item4)
		{
			Item1 = item1;
			Item2 = item2;
			Item3 = item3;
			Item4 = item4;
		}

		public IEnumerator GetEnumerator()
		{
			yield return Item1;
			yield return Item2;
			yield return Item3;
			yield return Item4;
		}

		public int CompareTo(Tuple<T1, T2, T3, T4> other)
		{
			if (other == null)
				return 1;

			int result = Comparer<T1>.Default.Compare(Item1, other.Item1);
			if (result != 0) return result;

			result = Comparer<T2>.Default.Compare(Item2, other.Item2);
			if (result != 0) return result;

			result = Comparer<T3>.Default.Compare(Item3, other.Item3);
			if (result != 0) return result;

			result = Comparer<T4>.Default.Compare(Item4, other.Item4);

			return result;
		}

		public int CompareTo(object obj)
		{
			return CompareTo((Tuple<T1, T2, T3, T4>)obj);
		}

		public bool Equals(Tuple<T1, T2, T3, T4> other)
		{
			return other != null
			       && Equals(Item1, other.Item1)
			       && Equals(Item2, other.Item2)
			       && Equals(Item3, other.Item3)
			       && Equals(Item4, other.Item4);
		}

		public override bool Equals(object obj)
		{
			return Equals(obj as Tuple<T1, T2, T3, T4>);
		}

		public override int GetHashCode()
		{
			return Item1.GetHashCode() ^ Item2.GetHashCode()
			       ^ Item3.GetHashCode() ^ Item4.GetHashCode();
		}

		public override string ToString()
		{
			return String.Format("({0}, {1}, {2}, {3})", Item1, Item2, Item3, Item4);
		}
	}
}