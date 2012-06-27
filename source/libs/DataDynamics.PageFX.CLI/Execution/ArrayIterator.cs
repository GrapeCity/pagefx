using System;
using System.Collections;

namespace DataDynamics.PageFX.CLI.Execution
{
	internal sealed class ArrayIterator : IEnumerator
	{
		private readonly Array _array;
		private int _index;
		private int[] _indices;
		private bool _complete;

		public ArrayIterator(Array array)
		{
			_array = array;

			Reset();
		}

		public int[] Indices
		{
			get { return _indices; }
		}

		public bool MoveNext()
		{
			if (_complete)
			{
				_index = _array.Length;
				return false;
			}
			_index++;
			NextIndex();
			return !_complete;
		}

		private void NextIndex()
		{
			int rank = _array.Rank;
			_indices[rank - 1]++;
			for (int i = rank - 1; i >= 0; i--)
			{
				if (_indices[i] > _array.GetUpperBound(i))
				{
					if (i == 0)
					{
						_complete = true;
						return;
					}
					for (int j = i; j < rank; j++)
					{
						_indices[j] = _array.GetLowerBound(j);
					}
					_indices[i - 1]++;
				}
			}
		}

		public void Reset()
		{
			_index = - 1;
			_indices = new int[_array.Rank];
			int num = 1;
			for (int i = 0; i < _array.Rank; i++)
			{
				_indices[i] = _array.GetLowerBound(i);
				num *= _array.GetLength(i);
			}
			_indices[_indices.Length - 1]--;
			_complete = num == 0;

		}

		public object Current
		{
			get
			{
				if (_index < 0)
				{
					throw new InvalidOperationException();
				}
				if (_complete)
				{
					throw new InvalidOperationException();
				}
				return _array.GetValue(_indices);
			}
		}
	}
}
