namespace System.Collections.Generic
{
    public class ReadOnlyList<T> : IReadOnlyList<T>
    {
        protected readonly List<T> List = new List<T>();

        public T this[int index]
        {
            get { return List[index]; }
        }

    	public int Count
        {
            get { return List.Count; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return List.GetEnumerator();
        }

    	IEnumerator IEnumerable.GetEnumerator()
        {
            return List.GetEnumerator();
        }

		public int IndexOf(T item)
		{
			return List.IndexOf(item);
		}

		public bool Contains(T item)
		{
			return List.Contains(item);
		}
    }
}