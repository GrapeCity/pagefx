using System.Collections.Generic;

namespace DataDynamics.PageFX.Common.Collections
{
    public interface IReadOnlyList<T> : IEnumerable<T>
    {
        int Count { get; }

        T this[int index] { get; }
    }

	public class ListEx<T> : List<T>, IReadOnlyList<T>
	{
	}
}