//CHANGED
//
// System.Array.cs
//
// Authors:
//   Joe Shaw (joe@ximian.com)
//   Martin Baulig (martin@gnome.org)
//   Dietmar Maurer (dietmar@ximian.com)
//   Gonzalo Paniagua Javier (gonzalo@ximian.com)
//
// (C) 2001-2003 Ximian, Inc.  http://www.ximian.com
// Copyright (C) 2004-2005 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

#if NET_2_0
using System.Collections.ObjectModel;
#endif

namespace System
{
    [Serializable]
    // FIXME: We are doing way to many double/triple exception checks for the overloaded functions"
    // FIXME: Sort overloads parameter checks are VERY inconsistent"
    public class Array : ICloneable, IList
    {
        #region Fields
        internal Avm.Array m_value;
        private int m_rank;
        private int m_type;
        private Avm.Function m_box;
        private Avm.Function m_unbox;
        
        // only used for multidimensional arrays
        private Avm.Array m_lbounds;
        private Avm.Array m_lengths;
        private Avm.Array m_dims;
        #endregion

        public static implicit operator Avm.Array(Array arr)
        {
            if (arr == null) return null;
            return arr.m_value;
        }

        internal void Push(object value)
        {
            m_value.push(value);
        }

        public override int GetTypeId()
        {
            return m_type;
        }

        public override Type GetType()
        {
            return Assembly.GetType(m_type);
        }

        public Type GetElementType()
        {
            return GetType().ElementType;
        }

        #region Properties
        public int Length
        {
            get { return (int)m_value.length; }
        }

#if NET_1_1
        public long LongLength
        {
            get { return Length; }
        }
#endif

        public int GetLength(int dimension)
        {
            if (dimension < 0 || dimension >= m_rank)
                throw new IndexOutOfRangeException();
            if (m_lengths == null)
                return (int)m_value.length;
            return m_lengths.GetInt32(dimension);
        }

#if NET_1_1
        [ComVisible(false)]
        public long GetLongLength(int dimension)
        {
            return GetLength(dimension);
        }
#endif

        public int Rank
        {
            get { return m_rank; }
        }

        public int GetLowerBound(int dimension)
        {
            if (dimension < 0 || dimension >= m_rank)
                throw new IndexOutOfRangeException("dimension < 0 || dimension >= Rank");
            if (m_lbounds == null)
                return 0;
            return m_lbounds.GetInt32(dimension);
        }

        public int GetUpperBound(int dimension)
        {
            return GetLowerBound(dimension) + GetLength(dimension) - 1;
        }
        #endregion

        #region GetElem, SetElem
        //NOTE: cane be use for value type arrays
        //object v = m_value[index];
        //if (avm.IsUndefined(v))
        //    throw new IndexOutOfRangeException();
        //return v;

        internal object GetElem(int index)
        {
            if (unchecked((uint)index) >= m_value.length)
                throw new IndexOutOfRangeException();
            return m_value[index];
        }

        internal object GetElemFast(int index)
        {
            return m_value[index];
        }

        internal object GetItem(int index)
        {
            if (unchecked((uint)index) >= m_value.length)
                throw new ArgumentOutOfRangeException("index");
            return m_value[index];
        }

        internal T GetElemT<T>(int index)
        {
            //NOTE: it is used only in Enumerator<T>
            //if (unchecked((uint)index) >= m_value.length)
            //    throw new IndexOutOfRangeException();
            return (T)m_value[index];
        }

        internal void SetElem(int index, object value)
        {
            if (unchecked((uint)index) >= m_value.length)
                throw new IndexOutOfRangeException();
            m_value[index] = value;
        }

        internal void SetElemFast(int index, object value)
        {
            m_value[index] = value;
        }

        internal void SetItem(int index, object value)
        {
            if (unchecked((uint)index) >= m_value.length)
                throw new ArgumentOutOfRangeException();
            m_value[index] = value;
        }
        #endregion

        #region Generic Members
#if NET_2_0
		// FIXME: they should not be exposed, but there are some
		// dependent code in the runtime.
		protected int InternalArray__ICollection_get_Count<T> ()
		{
			return Length;
		}

		protected IEnumerator<T> InternalArray__IEnumerable_GetEnumerator<T> ()
		{
			return new InternalEnumerator<T> (this);
		}

		protected void InternalArray__ICollection_Clear<T> ()
		{
			throw new NotSupportedException ("Collection is read-only");
		}

		protected void InternalArray__ICollection_Add<T> (T item)
		{
			throw new NotSupportedException ("Collection is read-only");
		}

		protected bool InternalArray__ICollection_Remove<T> (T item)
		{
			throw new NotSupportedException ("Collection is read-only");
		}

		protected bool InternalArray__ICollection_Contains<T> (T item)
		{
			if (this.Rank > 1)
				throw new RankException (Locale.GetText ("Only single dimension arrays are supported."));

			int length = this.Length;
			for (int i = 0; i < length; i++) {
				T value;
				GetGenericValueImpl (i, out value);
				if (item == null){
					if (value == null)
						return true;
					else
						return false;
				}
				
				if (item.Equals (value))
					return true;
			}

			return false;
		}

		protected void InternalArray__ICollection_CopyTo<T> (T[] array, int index)
		{
			if (array == null)
				throw new ArgumentNullException ("array");

			// The order of these exception checks may look strange,
			// but that's how the microsoft runtime does it.
			if (this.Rank > 1)
				throw new RankException (Locale.GetText ("Only single dimension arrays are supported."));
			if (index + this.GetLength (0) > array.GetLowerBound (0) + array.GetLength (0))
				throw new ArgumentException ();
			if (array.Rank > 1)
				throw new RankException (Locale.GetText ("Only single dimension arrays are supported."));
			if (index < 0)
				throw new ArgumentOutOfRangeException (
					"index", Locale.GetText ("Value has to be >= 0."));

			Copy (this, this.GetLowerBound (0), array, index, this.GetLength (0));
		}

		protected void InternalArray__Insert<T> (int index, T item)
		{
			throw new NotSupportedException ("Collection is read-only");
		}

		protected void InternalArray__RemoveAt<T> (int index)
		{
			throw new NotSupportedException ("Collection is read-only");
		}

		protected int InternalArray__IndexOf<T> (T item)
		{
			if (this.Rank > 1)
				throw new RankException (Locale.GetText ("Only single dimension arrays are supported."));

			int length = this.Length;
			for (int i = 0; i < length; i++) {
				T value;
				GetGenericValueImpl (i, out value);
				if (item == null){
					if (value == null)
						return i + this.GetLowerBound (0);
					else {
						unchecked {
							return this.GetLowerBound (0) - 1;
						}
					}
				}
				if (item.Equals (value))
					// array index may not be zero-based.
					// use lower bound
					return i + this.GetLowerBound (0);
			}

			int retVal;
			unchecked {
				// lower bound may be MinValue
				retVal = this.GetLowerBound (0) - 1;
			}

			return retVal;
		}

		protected T InternalArray__get_Item<T> (int index)
		{
			if (unchecked ((uint) index) >= unchecked ((uint) Length))
				throw new ArgumentOutOfRangeException ("index");

			T value;
			GetGenericValueImpl (index, out value);
			return value;
		}

		protected void InternalArray__set_Item<T> (int index, T item)
		{
			throw new NotSupportedException ("Collection is read-only");
		}

		// CAUTION! No bounds checking!
		[MethodImplAttribute (MethodImplOptions.InternalCall)]
		internal extern void GetGenericValueImpl<T> (int pos, out T value);

		internal struct InternalEnumerator<T> : IEnumerator<T>
		{
			const int NOT_STARTED = -2;
			
			// this MUST be -1, because we depend on it in move next.
			// we just decr the size, so, 0 - 1 == FINISHED
			const int FINISHED = -1;
			
			Array array;
			int idx;

			internal InternalEnumerator (Array array)
			{
				this.array = array;
				idx = NOT_STARTED;
			}

			public void Dispose ()
			{
				idx = NOT_STARTED;
			}

			public bool MoveNext ()
			{
				if (idx == NOT_STARTED)
					idx = array.Length;

				return idx != FINISHED && -- idx != FINISHED;
			}

			public T Current {
				get {
					if (idx < 0)
						throw new InvalidOperationException ();

					return array.InternalArray__get_Item<T> (array.Length - 1 - idx);
				}
			}

			void IEnumerator.Reset ()
			{
				throw new NotImplementedException ();
			}

			object IEnumerator.Current {
				get {
					return Current;
				}
			}
		}
#endif
        #endregion

        #region IList interface
        object IList.this[int index]
        {
            get
            {
                if (index < 0 || index >= m_value.length)
                    throw new ArgumentOutOfRangeException("index");
                return GetValueImpl(index);
            }
            set
            {
                if (index < 0 || index >= m_value.length)
                    throw new ArgumentOutOfRangeException("index");
                SetValueImpl(value, index);
            }
        }

        int IList.Add(object value)
        {
            throw new NotSupportedException();
        }

        internal void Clear()
        {
            Clear(this, GetLowerBound(0), Length);
        }

        void IList.Clear()
        {
            Clear();
        }

        internal bool Contains(object value)
        {
            if (m_rank > 1)
                throw new RankException(Locale.GetText("Only single dimension arrays are supported."));

            int n = Length;
            for (int i = 0; i < n; i++)
            {
                if (Equals(value, GetValueImpl(i)))
                    return true;
            }
            return false;
        }

        bool IList.Contains(object value)
        {
            return Contains(value);
        }

        internal int IndexOf(object value)
        {
            if (m_rank > 1)
                throw new RankException(Locale.GetText("Only single dimension arrays are supported."));

            int n = Length;
            for (int i = 0; i < n; i++)
            {
                if (Equals(value, GetValueImpl(i)))
                    // array index may not be zero-based.
                    // use lower bound
                    return i + GetLowerBound(0);
            }

            int retVal;
            unchecked
            {
                // lower bound may be MinValue
                retVal = this.GetLowerBound(0) - 1;
            }

            return retVal;
        }

        int IList.IndexOf(object value)
        {
            return IndexOf(value);
        }

        void IList.Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        void IList.Remove(object value)
        {
            throw new NotSupportedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }
        #endregion

        #region ICollection Properties
        int ICollection.Count
        {
            get { return Length; }
        }

        public
#if !NET_2_0
 virtual
#endif
 bool IsSynchronized
        {
            get
            {
                return false;
            }
        }

        public
#if !NET_2_0
 virtual
#endif
 object SyncRoot
        {
            get
            {
                return this;
            }
        }

        public
#if !NET_2_0
 virtual
#endif
 bool IsFixedSize
        {
            get
            {
                return true;
            }
        }

        public
#if !NET_2_0
 virtual
#endif
 bool IsReadOnly
        {
            get
            {
                return false;
            }
        }
        #endregion

        #region IEnumerable Members
        public IEnumerator GetEnumerator()
        {
            return new SimpleEnumerator(this);
        }
        #endregion

        #region GetValue, SetValue
        private int ToFlatIndex(Avm.Array indices)
        {
            if (indices == null)
                throw new ArgumentNullException("indices");
            if (m_rank != indices.Length)
                throw new RankException();
            int flatIndex = 0;
            int i, index, lb;
            for (i = 0; i < m_rank - 1; ++i)
            {
                index = indices.GetInt32(i);
                lb = GetLowerBound(i);
                if (index < lb || index > GetUpperBound(i))
                    throw new IndexOutOfRangeException(
                        Locale.GetText("Index has to be between upper and lower bound of the array."));
                flatIndex += (index - lb) * m_dims.GetInt32(i);
            }

            i = m_rank - 1;
            index = indices.GetInt32(i);
            lb = GetLowerBound(i);
            if (index < lb || index > GetUpperBound(i))
                throw new IndexOutOfRangeException(
                    Locale.GetText("Index has to be between upper and lower bound of the array."));

            flatIndex += (index - lb);
            return flatIndex;
        }

        private int ToFlatIndex(params int[] indices)
        {
            if (indices == null)
                throw new ArgumentNullException("indices");
            return ToFlatIndex(indices.m_value);
        }

        private int ToFlatIndex(int i0, int i1)
        {
            if (m_rank != 2)
                throw new RankException(Locale.GetText("Array was not a 2-dimensional array."));

            int lb0 = GetLowerBound(0);
            if (i0 < lb0 || i0 > GetUpperBound(0))
                throw new IndexOutOfRangeException(
                    Locale.GetText("Index has to be between upper and lower bound of the array."));

            int lb1 = GetLowerBound(1);
            if (i1 < lb1 || i1 > GetUpperBound(1))
                throw new IndexOutOfRangeException(
                    Locale.GetText("Index has to be between upper and lower bound of the array."));

            return (i0 - lb0) * GetLength(1) + (i1 - lb1);
        }

        private int ToFlatIndex(int i0, int i1, int i2)
        {
            if (m_rank != 3)
                throw new RankException(Locale.GetText("Array was not a 3-dimensional array."));

            int lb0 = GetLowerBound(0);
            if (i0 < lb0 || i0 > GetUpperBound(0))
                throw new IndexOutOfRangeException(
                    Locale.GetText("Index has to be between upper and lower bound of the array."));

            int lb1 = GetLowerBound(1);
            if (i1 < lb1 || i1 > GetUpperBound(1))
                throw new IndexOutOfRangeException(
                    Locale.GetText("Index has to be between upper and lower bound of the array."));

            int lb2 = GetLowerBound(2);
            if (i2 < lb2 || i2 > GetUpperBound(2))
                throw new IndexOutOfRangeException(
                    Locale.GetText("Index has to be between upper and lower bound of the array."));

            return (i0 - lb0) * m_dims.GetInt32(0)
                   + (i1 - lb1) * m_dims.GetInt32(1)
                   + (i2 - lb2);
        }

        public object GetValue(params int[] indices)
        {
            int i = ToFlatIndex(indices);
            return GetValueImpl(i);
        }

        public void SetValue(object value, params int[] indices)
        {
            int i = ToFlatIndex(indices);
            SetValueImpl(value, i);
        }

        // CAUTION! No bounds checking!
        internal object GetValueImpl(int pos)
        {
            object v = m_value[pos];
            if (m_box != null)
            {
                return m_box.call(null, v);
            }
            return v;
        }

        // CAUTION! No bounds checking!
        internal void SetValueImpl(object value, int pos)
        {
            try
            {
                if (m_unbox != null)
                {
                    if (value == null)
                        throw new ArgumentNullException("value");
                    value = m_unbox.call(null, value);
                }
                m_value[pos] = value;
            }
            catch (Avm.Error)
            {
                throw new InvalidCastException();
            }
        }

        private void CheckIndex(int index)
        {
            if (m_rank != 1)
                throw new RankException(Locale.GetText("Array was not a one-dimensional array."));

            if (index < GetLowerBound(0) || index > GetUpperBound(0))
                throw new IndexOutOfRangeException(
                    Locale.GetText("Index has to be between upper and lower bound of the array."));
        }

        public object GetValue(int index)
        {
            CheckIndex(index);
            return GetValueImpl(index - GetLowerBound(0));
        }

        public object GetValue(int index1, int index2)
        {
            int i = ToFlatIndex(index1, index2);
            return GetValueImpl(i);
        }

        public object GetValue(int index1, int index2, int index3)
        {
            int i = ToFlatIndex(index1, index2, index3);
            return GetValueImpl(i);
        }

#if NET_1_1
        [ComVisible(false)]
        public object GetValue(long index)
        {
            if (index < 0 || index > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("index", Locale.GetText(
                    "Value must be >= 0 and <= Int32.MaxValue."));

            return GetValue((int)index);
        }

        [ComVisible(false)]
        public object GetValue(long index1, long index2)
        {
            if (index1 < 0 || index1 > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("index1", Locale.GetText(
                    "Value must be >= 0 and <= Int32.MaxValue."));

            if (index2 < 0 || index2 > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("index2", Locale.GetText(
                    "Value must be >= 0 and <= Int32.MaxValue."));

            return GetValue((int)index1, (int)index2);
        }

        [ComVisible(false)]
        public object GetValue(long index1, long index2, long index3)
        {
            if (index1 < 0 || index1 > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("index1", Locale.GetText(
                    "Value must be >= 0 and <= Int32.MaxValue."));

            if (index2 < 0 || index2 > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("index2", Locale.GetText(
                    "Value must be >= 0 and <= Int32.MaxValue."));

            if (index3 < 0 || index3 > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("index3", Locale.GetText(
                    "Value must be >= 0 and <= Int32.MaxValue."));

            return GetValue((int)index1, (int)index2, (int)index3);
        }

        [ComVisible(false)]
        public void SetValue(object value, long index)
        {
            if (index < 0 || index > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("index", Locale.GetText(
                    "Value must be >= 0 and <= Int32.MaxValue."));

            SetValue(value, (int)index);
        }

        [ComVisible(false)]
        public void SetValue(object value, long index1, long index2)
        {
            if (index1 < 0 || index1 > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("index1", Locale.GetText(
                    "Value must be >= 0 and <= Int32.MaxValue."));

            if (index2 < 0 || index2 > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("index2", Locale.GetText(
                    "Value must be >= 0 and <= Int32.MaxValue."));

            int[] ind = { (int)index1, (int)index2 };
            SetValue(value, ind);
        }

        [ComVisible(false)]
        public void SetValue(object value, long index1, long index2, long index3)
        {
            if (index1 < 0 || index1 > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("index1", Locale.GetText(
                    "Value must be >= 0 and <= Int32.MaxValue."));

            if (index2 < 0 || index2 > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("index2", Locale.GetText(
                    "Value must be >= 0 and <= Int32.MaxValue."));

            if (index3 < 0 || index3 > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("index3", Locale.GetText(
                    "Value must be >= 0 and <= Int32.MaxValue."));

            int[] ind = { (int)index1, (int)index2, (int)index3 };
            SetValue(value, ind);
        }
#endif

        public void SetValue(object value, int index)
        {
            CheckIndex(index);
            SetValueImpl(value, index - GetLowerBound(0));
        }

        public void SetValue(object value, int index1, int index2)
        {
            int i = ToFlatIndex(index1, index2);
            SetValueImpl(value, i);
        }

        public void SetValue(object value, int index1, int index2, int index3)
        {
            int i = ToFlatIndex(index1, index2, index3);
            SetValueImpl(value, i);
        }
        #endregion

        #region CreateInstance
        internal static Array CreateInstanceImpl(Type elementType, int[] lengths, int[] bounds)
        {
            Array arr = new Array();
            int n = lengths.Length;
            arr.m_rank = n;

            arr.m_lengths = avm.CopyArray(lengths.m_value);
            if (bounds != null)
                arr.m_lbounds = avm.CopyArray(bounds.m_value);

            int len = 1;
            for (int i = 0; i < n; ++i)
                len *= lengths[i];

            arr.m_value = avm.NewArray(len);

            arr.m_box = elementType.m_box;
            arr.m_unbox = elementType.m_unbox;
            if (arr.m_box == null || arr.m_unbox == null)
            {
                arr.m_box = elementType.m_copy;
                arr.m_unbox = elementType.m_copy;
            }

            Type arrType = Assembly.GetArrayType(elementType, lengths, bounds);
            arr.m_type = arrType.index;
            
            return arr;
        }

        public static Array CreateInstance(Type elementType, int length)
        {
            int[] lengths = { length };

            return CreateInstance(elementType, lengths);
        }

        public static Array CreateInstance(Type elementType, int length1, int length2)
        {
            int[] lengths = { length1, length2 };

            return CreateInstance(elementType, lengths);
        }

        public static Array CreateInstance(Type elementType, int length1, int length2, int length3)
        {
            int[] lengths = { length1, length2, length3 };

            return CreateInstance(elementType, lengths);
        }

        public static Array CreateInstance(Type elementType, params int[] lengths)
        {
            if (elementType == null)
                throw new ArgumentNullException("elementType");
            if (lengths == null)
                throw new ArgumentNullException("lengths");

            //elementType = elementType.UnderlyingSystemType;
            //if (!elementType.IsSystemType)
            //    throw new ArgumentException("Type must be a type provided by the runtime.", "elementType");

            int n = lengths.Length;
            if (n < 1)
                throw new ArgumentException(Locale.GetText("Arrays must contain >= 1 elements."));

            for (int i = 0; i < n; i++)
            {
                if (lengths[i] < 0)
                    throw new ArgumentOutOfRangeException("lengths",
                                                          Locale.GetText("Each value has to be >= 0."));
            }

            if (n > 255)
                throw new TypeLoadException();

            int[] bounds = null;

            return CreateInstanceImpl(elementType, lengths, bounds);
        }

        public static Array CreateInstance(Type elementType, int[] lengths, int[] bounds)
        {
            if (elementType == null)
                throw new ArgumentNullException("elementType");
            if (lengths == null)
                throw new ArgumentNullException("lengths");
            if (bounds == null)
                throw new ArgumentNullException("bounds");

            //elementType = elementType.UnderlyingSystemType;
            //if (!elementType.IsSystemType)
            //    throw new ArgumentException("Type must be a type provided by the runtime.", "elementType");

            int n = lengths.Length;
            if (n < 1)
                throw new ArgumentException(Locale.GetText("Arrays must contain >= 1 elements."));

            if (n != bounds.Length)
                throw new ArgumentException(Locale.GetText("Arrays must be of same size."));

            for (int i = 0; i < n; i++)
            {
                if (lengths[i] < 0)
                    throw new ArgumentOutOfRangeException("lengths",
                                                          Locale.GetText("Each value has to be >= 0."));
                if (bounds[i] + lengths[i] > Int32.MaxValue)
                    throw new ArgumentOutOfRangeException("lengths",
                                                          Locale.GetText(
                                                              "Length + bound must not exceed Int32.MaxValue."));
            }

            if (n > 255)
                throw new TypeLoadException();

            return CreateInstanceImpl(elementType, lengths, bounds);
        }
        #endregion

        #region NET_1_1 (CreateInstance, GetValue, SetValue)
#if NET_1_1
        static int[] GetIntArray(long[] values)
        {
            int len = values.Length;
            int[] ints = new int[len];
            for (int i = 0; i < len; i++)
            {
                long current = values[i];
                if (current < 0 || current > Int32.MaxValue)
                    throw new ArgumentOutOfRangeException(
                        "values", Locale.GetText("Each value has to be >= 0 and <= Int32.MaxValue."));

                ints[i] = (int)current;
            }
            return ints;
        }

        public static Array CreateInstance(Type elementType, params long[] lengths)
        {
#if NET_2_0
			if (lengths == null)
				throw new ArgumentNullException ("lengths");
#endif
            return CreateInstance(elementType, GetIntArray(lengths));
        }

        [ComVisible(false)]
        public object GetValue(params long[] indices)
        {
#if NET_2_0
			if (indices == null)
				throw new ArgumentNullException ("indices");
#endif
            return GetValue(GetIntArray(indices));
        }

        [ComVisible(false)]
        public void SetValue(object value, params long[] indices)
        {
#if NET_2_0
			if (indices == null)
				throw new ArgumentNullException ("indices");
#endif
            SetValue(value, GetIntArray(indices));
        }
#endif
        #endregion

        #region BinarySearch
        public static int BinarySearch(Array array, object value)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if (value == null)
                return -1;

            if (array.m_rank > 1)
                throw new RankException(Locale.GetText("Only single dimension arrays are supported."));

            if (array.Length == 0)
                return -1;

            if (!(value is IComparable))
                throw new ArgumentException(Locale.GetText("value does not support IComparable."));

            return DoBinarySearch(array, array.GetLowerBound(0), array.GetLength(0), value, null);
        }

        public static int BinarySearch(Array array, object value, IComparer comparer)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if (array.m_rank > 1)
                throw new RankException(Locale.GetText("Only single dimension arrays are supported."));

            if (array.Length == 0)
                return -1;

            if ((comparer == null) && (value != null) && !(value is IComparable))
                throw new ArgumentException(Locale.GetText(
                    "comparer is null and value does not support IComparable."));

            return DoBinarySearch(array, array.GetLowerBound(0), array.GetLength(0), value, comparer);
        }

        public static int BinarySearch(Array array, int index, int length, object value)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if (array.m_rank > 1)
                throw new RankException(Locale.GetText("Only single dimension arrays are supported."));

            if (index < array.GetLowerBound(0))
                throw new ArgumentOutOfRangeException("index", Locale.GetText(
                    "index is less than the lower bound of array."));
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", Locale.GetText(
                    "Value has to be >= 0."));
            // re-ordered to avoid possible integer overflow
            if (index > array.GetLowerBound(0) + array.GetLength(0) - length)
                throw new ArgumentException(Locale.GetText(
                    "index and length do not specify a valid range in array."));

            if (array.Length == 0)
                return -1;

            if ((value != null) && (!(value is IComparable)))
                throw new ArgumentException(Locale.GetText(
                    "value does not support IComparable"));

            return DoBinarySearch(array, index, length, value, null);
        }

        public static int BinarySearch(Array array, int index, int length, object value, IComparer comparer)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if (array.m_rank > 1)
                throw new RankException(Locale.GetText("Only single dimension arrays are supported."));

            if (index < array.GetLowerBound(0))
                throw new ArgumentOutOfRangeException("index", Locale.GetText(
                    "index is less than the lower bound of array."));
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", Locale.GetText(
                    "Value has to be >= 0."));
            // re-ordered to avoid possible integer overflow
            if (index > array.GetLowerBound(0) + array.GetLength(0) - length)
                throw new ArgumentException(Locale.GetText(
                    "index and length do not specify a valid range in array."));

            if (array.Length == 0)
                return -1;

            if ((comparer == null) && (value != null) && !(value is IComparable))
                throw new ArgumentException(Locale.GetText(
                    "comparer is null and value does not support IComparable."));

            return DoBinarySearch(array, index, length, value, comparer);
        }

        static int DoBinarySearch(Array array, int index, int length, object value, IComparer comparer)
        {
            // cache this in case we need it
            if (comparer == null)
                comparer = Comparer.Default;

            int iMin = index;
            // Comment from Tum (tum@veridicus.com):
            // *Must* start at index + length - 1 to pass rotor test co2460binarysearch_iioi
            int iMax = index + length - 1;
            int iCmp = 0;
            try
            {
                while (iMin <= iMax)
                {
                    int iMid = (iMin + iMax) / 2;
                    object midItem = array.GetValueImpl(iMid);

                    iCmp = comparer.Compare(midItem, value);
                    
                    if (iCmp == 0)
                        return iMid;
                    else if (iCmp > 0)
                        iMax = iMid - 1;
                    else
                        iMin = iMid + 1; // compensate for the rounding down
                }
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(Locale.GetText("Comparer threw an exception."), e);
            }

            return ~iMin;
        }
        #endregion

        #region Clear
        public static void Clear(Array array, int index, int length)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (length < 0)
                throw new ArgumentOutOfRangeException("length", "length < 0");

            int low = array.GetLowerBound(0);
            if (index < low)
                throw new IndexOutOfRangeException("index < lower bound");
            index = index - low;

            // re-ordered to avoid possible integer overflow
            if (index > array.Length - length)
                throw new IndexOutOfRangeException("index + length > size");

            Type elemType = array.GetElementType();
            object z = elemType.GetZero();
            while (length-- > 0)
            {
                array.SetValueImpl(z, index);
                ++index;
            }
        }
        #endregion

        #region ICloneable Impl
        public object Clone()
        {
            Array arr = new Array();
            arr.m_value = avm.CopyArray(m_value);
            arr.m_rank = m_rank;
            arr.m_type = m_type;
            arr.m_lbounds = m_lbounds;
            arr.m_lengths = m_lengths;
            arr.m_dims = m_dims;
            arr.m_box = m_box;
            arr.m_unbox = m_unbox;
            return arr;
        }
        #endregion

        #region CastTo
        private static bool CanCastPrimitiveElemTypes(Type from, Type to)
        {
            TypeCode a = Type.GetTypeCode(from);
            TypeCode b = Type.GetTypeCode(to);
            switch (a)
            {
                case TypeCode.Byte:
                case TypeCode.SByte:
                    return b == TypeCode.Byte || b == TypeCode.SByte;
                case TypeCode.Int16:
                case TypeCode.UInt16:
                    return b == TypeCode.Int16 || b == TypeCode.UInt16;
                case TypeCode.Int32:
                case TypeCode.UInt32:
                    return b == TypeCode.Int32 || b == TypeCode.UInt32;
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return b == TypeCode.Int64 || b == TypeCode.UInt64;
                default:
                    return b == a;
            }
        }

        internal bool IsCharArray()
        {
            Type myElemType = GetElementType();
            return Type.GetTypeCode(myElemType) == TypeCode.Char;
        }

        private static bool CanCast(Type myElemType, Type elemType)
        {
            if (myElemType.IsPrimitive)
            {
                return CanCastPrimitiveElemTypes(myElemType, elemType);
            }
            if (myElemType.IsEnum)
            {
                return CanCastPrimitiveElemTypes(myElemType.UnderlyingSystemType, elemType);
            }
            return elemType.IsAssignableFrom(myElemType);
        }

        internal bool HasElemType(int elemTypeId, bool covariance)
        {
            Type elemType = Assembly.GetType(elemTypeId);
            if (elemType == null)
                return false;
            Type myElemType = GetElementType();
            if (covariance)
            {
                return CanCast(myElemType, elemType);
            }
            return elemType == myElemType;
        }

        internal Array CastTo(int arrTypeId)
        {
            if (arrTypeId == m_type)
                return this;

            Type arrType = Assembly.GetType(arrTypeId);
            if (arrType == null)
            {
                //throw new InvalidCastException("invalid type id " + arrTypeId);
                return null;
            }

            if (!arrType.IsArray)
            {
                //throw new InvalidCastException(
                //    string.Format("given type {0} is not array", arrType.FullName));
                return null;
            }

            if (arrType.rank != m_rank)
            {
                //throw new InvalidCastException("invalid rank: " + m_rank + " != " + arrType.rank);
                return null;
            }

            Type elemType = arrType.ElementType;
            Type myElemType = GetElementType();
            if (!CanCast(myElemType, elemType))
            {
                //throw new InvalidCastException(
                //    string.Format("Unable to cast array of type {0} to array of type {1}",
                //                  myElemType, elemType));
                return null;
            }

            return this;
        }
        #endregion

        public void Initialize()
        {
            //FIXME: We would like to find a compiler that uses
            // this method. It looks like this method do nothing
            // in C# so no exception is trown by the moment.
        }

        #region Copy
        public static void Copy(Array sourceArray, Array destinationArray, int length)
        {
            // need these checks here because we are going to use
            // GetLowerBound() on source and dest.
            if (sourceArray == null)
                throw new ArgumentNullException("sourceArray");

            if (destinationArray == null)
                throw new ArgumentNullException("destinationArray");

            Copy(sourceArray, sourceArray.GetLowerBound(0),
                 destinationArray, destinationArray.GetLowerBound(0), length);
        }

        public static void Copy(Array sourceArray, int sourceIndex, Array destinationArray, int destinationIndex, int length)
        {
            if (sourceArray == null)
                throw new ArgumentNullException("sourceArray");

            if (destinationArray == null)
                throw new ArgumentNullException("destinationArray");

            if (sourceArray.Rank != destinationArray.Rank)
                throw new RankException(Locale.GetText("Arrays must be of same size."));

            if (length < 0)
                throw new ArgumentOutOfRangeException("length", "Value has to be >= 0.");

            if (sourceIndex < 0)
                throw new ArgumentOutOfRangeException("sourceIndex", "Value has to be >= 0.");

            if (destinationIndex < 0)
                throw new ArgumentOutOfRangeException("destinationIndex", "Value has to be >= 0.");

            int source_pos = sourceIndex - sourceArray.GetLowerBound(0);
            int dest_pos = destinationIndex - destinationArray.GetLowerBound(0);

            // re-ordered to avoid possible integer overflow
            if (source_pos > sourceArray.Length - length || dest_pos > destinationArray.Length - length)
                throw new ArgumentException("length");

            Type src_type = sourceArray.GetElementType();
            Type dst_type = destinationArray.GetElementType();

            if (!ReferenceEquals(sourceArray, destinationArray) || source_pos > dest_pos)
            {
                for (int i = 0; i < length; i++)
                {
                    Object srcval = sourceArray.GetValueImpl(source_pos + i);

                    try
                    {
                        destinationArray.SetValueImpl(srcval, dest_pos + i);
                    }
                    catch
                    {
                        if ((dst_type.IsValueType || dst_type.Equals(typeof(String))) &&
                            (src_type.Equals(typeof(Object))))
                            throw new InvalidCastException();
                        else
                            throw new ArrayTypeMismatchException(String.Format(Locale.GetText(
                                "(Types: source={0};  target={1})"), src_type.FullName, dst_type.FullName));
                    }
                }
            }
            else
            {
                for (int i = length - 1; i >= 0; i--)
                {
                    Object srcval = sourceArray.GetValueImpl(source_pos + i);

                    try
                    {
                        destinationArray.SetValueImpl(srcval, dest_pos + i);
                    }
                    catch
                    {
                        if ((dst_type.IsValueType || dst_type.Equals(typeof(String))) &&
                            (src_type.Equals(typeof(Object))))
                            throw new InvalidCastException();
                        else
                            throw new ArrayTypeMismatchException(String.Format(Locale.GetText(
                                "(Types: source={0};  target={1})"), src_type.FullName, dst_type.FullName));
                    }
                }
            }
        }

#if NET_1_1
        public static void Copy(Array sourceArray, long sourceIndex, Array destinationArray,
                                 long destinationIndex, long length)
        {
            if (sourceArray == null)
                throw new ArgumentNullException("sourceArray");

            if (destinationArray == null)
                throw new ArgumentNullException("destinationArray");

            if (sourceIndex < Int32.MinValue || sourceIndex > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("sourceIndex",
                    Locale.GetText("Must be in the Int32 range."));

            if (destinationIndex < Int32.MinValue || destinationIndex > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("destinationIndex",
                    Locale.GetText("Must be in the Int32 range."));

            if (length < 0 || length > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("length", Locale.GetText(
                    "Value must be >= 0 and <= Int32.MaxValue."));

            Copy(sourceArray, (int)sourceIndex, destinationArray, (int)destinationIndex, (int)length);
        }

        public static void Copy(Array sourceArray, Array destinationArray, long length)
        {
            if (length < 0 || length > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("length", Locale.GetText(
                    "Value must be >= 0 and <= Int32.MaxValue."));

            Copy(sourceArray, destinationArray, (int)length);
        }
#endif
        #endregion

        #region IndexOf
        public static int IndexOf(Array array, object value)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            return IndexOf(array, value, 0, array.Length);
        }

        public static int IndexOf(Array array, object value, int startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            return IndexOf(array, value, startIndex, array.Length - startIndex);
        }

        public static int IndexOf(Array array, object value, int startIndex, int count)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if (array.Rank > 1)
                throw new RankException(Locale.GetText("Only single dimension arrays are supported."));

            // re-ordered to avoid possible integer overflow
            if (count < 0 || startIndex < array.GetLowerBound(0) || startIndex - 1 > array.GetUpperBound(0) - count)
                throw new ArgumentOutOfRangeException();

            int max = startIndex + count;
            for (int i = startIndex; i < max; i++)
            {
                if (Equals(value, array.GetValueImpl(i)))
                    return i;
            }

            return array.GetLowerBound(0) - 1;
        }
        #endregion

        #region LastIndexOf
        public static int LastIndexOf(Array array, object value)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            return LastIndexOf(array, value, array.Length - 1);
        }

        public static int LastIndexOf(Array array, object value, int startIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            return LastIndexOf(array, value, startIndex, startIndex - array.GetLowerBound(0) + 1);
        }

        public static int LastIndexOf(Array array, object value, int startIndex, int count)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if (array.m_rank > 1)
                throw new RankException(Locale.GetText("Only single dimension arrays are supported."));

            if (count < 0 || startIndex < array.GetLowerBound(0) ||
                startIndex > array.GetUpperBound(0) || startIndex - count + 1 < array.GetLowerBound(0))
                throw new ArgumentOutOfRangeException();

            for (int i = startIndex; i >= startIndex - count + 1; i--)
            {
                if (Object.Equals(value, array.GetValueImpl(i)))
                    return i;
            }

            return array.GetLowerBound(0) - 1;
        }
        #endregion

#if NOT_PFX
#if NET_2_0
		static Swapper get_swapper<T> (T [] array)
		{
			if (array is int[])
				return new Swapper (array.int_swapper);
			if (array is double[])
				return new Swapper (array.double_swapper);

			// gmcs refuses to compile this
			//return new Swapper (array.generic_swapper<T>);
			return new Swapper (array.slow_swapper);
		}
#endif
#endif
        #region Reverse
        public static void Reverse(Array array)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (array.Rank > 1)
                throw new RankException(Locale.GetText("Only single dimension arrays are supported."));
            array.m_value = array.m_value.reverse();
        }

        public static void Reverse(Array array, int index, int length)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            if (array.Rank > 1)
                throw new RankException(Locale.GetText("Only single dimension arrays are supported."));

            if (index < array.GetLowerBound(0) || length < 0)
                throw new ArgumentOutOfRangeException();

            // re-ordered to avoid possible integer overflow
            if (index > array.GetUpperBound(0) + 1 - length)
                throw new ArgumentOutOfRangeException();

            array.InternalReverse(index, length);
        }

        private void InternalReverse(int index, int length)
        {
            int endPoint = index + length - 1;
            length >>= 1;
            for (int i = 0; i < length; ++i)
            {
                SwapElems(index, endPoint);
                --endPoint;
                ++index;
            }
        }
        #endregion

        #region Sort
        public static void Sort(Array array)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            Sort(array, null, array.GetLowerBound(0), array.GetLength(0), null);
        }

        public static void Sort(Array keys, Array items)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");

            Sort(keys, items, keys.GetLowerBound(0), keys.GetLength(0), null);
        }

        public static void Sort(Array array, IComparer comparer)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            Sort(array, null, array.GetLowerBound(0), array.GetLength(0), comparer);
        }

        public static void Sort(Array array, int index, int length)
        {
            Sort(array, null, index, length, null);
        }

        public static void Sort(Array keys, Array items, IComparer comparer)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");

            Sort(keys, items, keys.GetLowerBound(0), keys.GetLength(0), comparer);
        }

        public static void Sort(Array keys, Array items, int index, int length)
        {
            Sort(keys, items, index, length, null);
        }

        public static void Sort(Array array, int index, int length, IComparer comparer)
        {
            Sort(array, null, index, length, comparer);
        }

        public static void Sort(Array keys, Array items, int index, int length, IComparer comparer)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");

            if (keys.Rank > 1 || (items != null && items.Rank > 1))
                throw new RankException();

            if (items != null && keys.GetLowerBound(0) != items.GetLowerBound(0))
                throw new ArgumentException();

            if (index < keys.GetLowerBound(0))
                throw new ArgumentOutOfRangeException("index");

            if (length < 0)
                throw new ArgumentOutOfRangeException("length", Locale.GetText(
                    "Value has to be >= 0."));

            if (keys.Length - (index + keys.GetLowerBound(0)) < length || (items != null && index > items.Length - length))
                throw new ArgumentException();

            if (length <= 1)
                return;

            try
            {
                int low0 = index;
                int high0 = index + length - 1;
                qsort(keys, items, low0, high0, comparer);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(Locale.GetText("The comparer threw an exception."), e);
            }
        }

        static int new_gap(int gap)
        {
            gap = (gap * 10) / 13;
            if (gap == 9 || gap == 10)
                return 11;
            if (gap < 1)
                return 1;
            return gap;
        }

        private static void qsort(Array keys, Array items, int low0, int high0, IComparer comparer)
        {
            if (low0 >= high0)
                return;

            int low = low0;
            int high = high0;

            object objPivot = keys.GetValueImpl((low + high) / 2);

            while (low <= high)
            {
                // Move the walls in
                while (low < high0 && compare(keys.GetValueImpl(low), objPivot, comparer) < 0)
                    ++low;
                while (high > low0 && compare(objPivot, keys.GetValueImpl(high), comparer) < 0)
                    --high;

                if (low <= high)
                {
                    keys.SwapElems(low, high);
                    if (items != null)
                    {
                        items.SwapElems(low, high);
                    }
                    ++low;
                    --high;
                }
            }

            if (low0 < high)
                qsort(keys, items, low0, high, comparer);
            if (low < high0)
                qsort(keys, items, low, high0, comparer);
        }

        [AnyVars]
        private void SwapElems(int index1, int index2)
        {
            //object temp = GetValueImpl(index1);
            //SetValueImpl(GetValueImpl(index2), index1);
            //SetValueImpl(temp, index2);
            Avm.Array arr = m_value;
            object temp = m_value[index1];
            m_value[index1] = m_value[index2];
            m_value[index2] = temp;
        }

        private static int compare(object value1, object value2, IComparer comparer)
        {
            if (value1 == null)
                return value2 == null ? 0 : -1;
            else if (value2 == null)
                return 1;
            else if (comparer == null)
                return ((IComparable)value1).CompareTo(value2);
            else
                return comparer.Compare(value1, value2);
        }

#if NET_2_0
		public static void Sort<T> (T [] array)
		{
			if (array == null)
				throw new ArgumentNullException ("array");

			Sort<T, T> (array, null, 0, array.Length, null);
		}

		public static void Sort<TKey, TValue> (TKey [] keys, TValue [] items)
		{
			if (keys == null)
				throw new ArgumentNullException ("keys");
			
			Sort<TKey, TValue> (keys, items, 0, keys.Length, null);
		}

		public static void Sort<T> (T [] array, IComparer<T> comparer)
		{
			if (array == null)
				throw new ArgumentNullException ("array");

			Sort<T, T> (array, null, 0, array.Length, comparer);
		}

		public static void Sort<TKey, TValue> (TKey [] keys, TValue [] items, IComparer<TKey> comparer)
		{
			if (keys == null)
				throw new ArgumentNullException ("keys");
			
			Sort<TKey, TValue> (keys, items, 0, keys.Length, comparer);
		}

		public static void Sort<T> (T [] array, int index, int length)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			
			Sort<T, T> (array, null, index, length, null);
		}

		public static void Sort<TKey, TValue> (TKey [] keys, TValue [] items, int index, int length)
		{
			Sort<TKey, TValue> (keys, items, index, length, null);
		}

		public static void Sort<T> (T [] array, int index, int length, IComparer<T> comparer)
		{
			if (array == null)
				throw new ArgumentNullException ("array");

			Sort<T, T> (array, null, index, length, comparer);
		}

        public static void Sort<TKey, TValue>(TKey[] keys, TValue[] items, int index, int length, IComparer<TKey> comparer)
        {
            if (keys == null)
                throw new ArgumentNullException("keys");

            if (index < 0)
                throw new ArgumentOutOfRangeException("index");

            if (length < 0)
                throw new ArgumentOutOfRangeException("length");

            if (keys.Length - index < length
                || (items != null && index > items.Length - length))
                throw new ArgumentException();

            if (length <= 1)
                return;

            //
            // Check for value types which can be sorted without Compare () method
            //
            if (comparer == null)
            {
                //    Swapper iswapper;
                //    if (items == null)
                //        iswapper = null;
                //    else 
                //        iswapper = get_swapper<TValue> (items);
                //    if (keys is double[]) {
                //        combsort (keys as double[], index, length, iswapper);
                //        return;
                //    }
                //    if (keys is int[]) {
                //        combsort (keys as int[], index, length, iswapper);
                //        return;
                //    }
                //    if (keys is char[]) {
                //        combsort (keys as char[], index, length, iswapper);
                //        return;
                //    }

                //Use Comparer<T>.Default instead
                comparer = Comparer<TKey>.Default;
            }

            try
            {
                int low0 = index;
                int high0 = index + length - 1;
                qsort<TKey, TValue>(keys, items, low0, high0, comparer);
            }
            catch (Exception e)
            {
                throw new InvalidOperationException(Locale.GetText("The comparer threw an exception."), e);
            }
        }

        public static void Sort<T> (T [] array, Comparison<T> comparison)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			Sort<T> (array, array.Length, comparison);
		}

		internal static void Sort<T> (T [] array, int length, Comparison<T> comparison)
		{
			if (comparison == null)
				throw new ArgumentNullException ("comparison");

			if (length <= 1 || array.Length <= 1)
				return;
			
			try {
				int low0 = 0;
				int high0 = length - 1;
				qsort<T> (array, low0, high0, comparison);
			}
			catch (Exception e) {
				throw new InvalidOperationException (Locale.GetText ("Comparison threw an exception."), e);
			}
		}

		private static void qsort<K, V> (K [] keys, V [] items, int low0, int high0, IComparer<K> comparer)
		{
			if (low0 >= high0)
				return;

			int low = low0;
			int high = high0;

			K keyPivot = keys [(low + high) / 2];

			while (low <= high) {
				// Move the walls in
				//while (low < high0 && comparer.Compare (keys [low], keyPivot) < 0)
				while (low < high0 && compare (keys [low], keyPivot, comparer) < 0)
					++low;
				//while (high > low0 && comparer.Compare (keyPivot, keys [high]) < 0)
				while (high > low0 && compare (keyPivot, keys [high], comparer) < 0)
					--high;

				if (low <= high) {
					swap<K, V> (keys, items, low, high);
					++low;
					--high;
				}
			}

			if (low0 < high)
				qsort<K, V> (keys, items, low0, high, comparer);
			if (low < high0)
				qsort<K, V> (keys, items, low, high0, comparer);
		}

		private static int compare<T> (T value1, T value2, IComparer<T> comparer)
		{
			if (comparer != null)
				return comparer.Compare (value1, value2);
			else if (value1 == null)
				return value2 == null ? 0 : -1;
			else if (value2 == null)
				return 1;
			else if (value1 is IComparable<T>)
				return ((IComparable<T>) value1).CompareTo (value2);
			else if (value1 is IComparable)
				return ((IComparable) value1).CompareTo (value2);

			string msg = Locale.GetText ("No IComparable or IComparable<T> interface found for type '{0}'.");
			throw new InvalidOperationException (String.Format (msg, typeof (T)));
		}

		private static void qsort<T> (T [] array, int low0, int high0, Comparison<T> comparison)
		{
			if (low0 >= high0)
				return;

			int low = low0;
			int high = high0;

			T keyPivot = array [(low + high) / 2];

			while (low <= high) {
				// Move the walls in
				while (low < high0 && comparison (array [low], keyPivot) < 0)
					++low;
				while (high > low0 && comparison (keyPivot, array [high]) < 0)
					--high;

				if (low <= high) {
					swap<T> (array, low, high);
					++low;
					--high;
				}
			}

			if (low0 < high)
				qsort<T> (array, low0, high, comparison);
			if (low < high0)
				qsort<T> (array, low, high0, comparison);
		}

		private static void swap<K, V> (K [] keys, V [] items, int i, int j)
		{
			K tmp;

			tmp = keys [i];
			keys [i] = keys [j];
			keys [j] = tmp;

			if (items != null) {
				V itmp;
				itmp = items [i];
				items [i] = items [j];
				items [j] = itmp;
			}
		}

		private static void swap<T> (T [] array, int i, int j)
		{
			T tmp = array [i];
			array [i] = array [j];
			array [j] = tmp;
		}
#endif
        #endregion

        #region CopyTo
        public
#if !NET_2_0
 virtual
#endif
 void CopyTo(Array array, int index)
        {
            if (array == null)
                throw new ArgumentNullException("array");

            // The order of these exception checks may look strange,
            // but that's how the microsoft runtime does it.
            if (Rank > 1)
                throw new RankException(Locale.GetText("Only single dimension arrays are supported."));
            if (array.Rank > 1)
                throw new RankException(Locale.GetText("Only single dimension arrays are supported."));
            int n = Length;
            if (index + n > array.GetLowerBound(0) + array.Length)
                throw new ArgumentException();
            if (index < 0)
                throw new ArgumentOutOfRangeException("index", "Value has to be >= 0.");

            Copy(this, GetLowerBound(0), array, index, n);
        }

#if NET_1_1
        [ComVisible(false)]
        public
#if !NET_2_0
 virtual
#endif
 void CopyTo(Array array, long index)
        {
            if (index < 0 || index > Int32.MaxValue)
                throw new ArgumentOutOfRangeException("index", Locale.GetText(
                    "Value must be >= 0 and <= Int32.MaxValue."));

            CopyTo(array, (int)index);
        }
#endif
        #endregion

        #region class SimpleEnumerator
        internal class SimpleEnumerator : IEnumerator, ICloneable
        {
            Array array;
            int currentpos;
            int length;

            private SimpleEnumerator()
            {
            }

            public SimpleEnumerator(Array arrayToEnumerate)
            {
                array = arrayToEnumerate;
                currentpos = -1;
                length = arrayToEnumerate.Length;
            }

            public object Current
            {
                get
                {
                    // Exception messages based on MS implementation
                    if (currentpos < 0)
                        throw new InvalidOperationException("Enumeration has not started.");
                    if (currentpos >= length)
                        throw new InvalidOperationException("Enumeration has already ended");
                    // Current should not increase the position. So no ++ over here.
                    return array.GetValueImpl(currentpos);
                }
            }

            public bool MoveNext()
            {
                //The docs say Current should throw an exception if last
                //call to MoveNext returned false. This means currentpos
                //should be set to length when returning false.
                if (currentpos < length)
                    currentpos++;
                return currentpos < length;
            }

            public void Reset()
            {
                currentpos = -1;
            }

            public object Clone()
            {
                SimpleEnumerator e = new SimpleEnumerator();
                e.array = array;
                e.length = length;
                e.currentpos = currentpos;
                return e;
            }
        }
        #endregion

        #region Enumerator
        internal class Enumerator<T> : IEnumerator<T>, ICloneable
        {
            Array array;
            int currentpos;
            int length;

            private Enumerator()
            {
            }

            public Enumerator(Array arr)
            {
                array = arr;
                currentpos = -1;
                length = arr.Length;
            }

            public void Dispose()
            {
            }

            object IEnumerator.Current
            {
                get { return Current; }
            }

            public T Current
            {
                get 
                {
                    // Exception messages based on MS implementation
                    if (currentpos < 0)
                        throw new InvalidOperationException("Enumeration has not started.");
                    if (currentpos >= length)
                        throw new InvalidOperationException("Enumeration has already ended");
                    return array.GetElemT<T>(currentpos);
                }
            }

            public bool MoveNext()
            {
                //The docs say Current should throw an exception if last
                //call to MoveNext returned false. This means currentpos
                //should be set to length when returning false.
                if (currentpos < length)
                    currentpos++;
                return currentpos < length;
            }

            public void Reset()
            {
                currentpos = -1;
            }

            #region ICloneable Members
            public object Clone()
            {
                Enumerator<T> e = new Enumerator<T>();
                e.array = array;
                e.length = length;
                e.currentpos = currentpos;
                return e;
            }
            #endregion
        }
        #endregion

#if NET_2_0
		public static void Resize<T> (ref T [] array, int newSize)
		{
			Resize<T> (ref array, array == null ? 0 : array.Length, newSize);
		}

		internal static void Resize<T> (ref T[] array, int length, int newSize)
		{
			if (newSize < 0)
				throw new ArgumentOutOfRangeException ();
			
			if (array == null) {
				array = new T [newSize];
				return;
			}
			
			if (array.Length == newSize)
				return;
			
			T [] a = new T [newSize];
			Copy (array, a, Math.Min (newSize, length));
			array = a;
		}
		
		public static bool TrueForAll <T> (T [] array, Predicate <T> match)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			if (match == null)
				throw new ArgumentNullException ("match");
			
			foreach (T t in array)
				if (! match (t))
					return false;
				
			return true;
		}
		
		public static void ForEach<T> (T [] array, Action <T> action)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			if (action == null)
				throw new ArgumentNullException ("action");
			
			foreach (T t in array)
				action (t);
		}
		
		public static TOutput[] ConvertAll<TInput, TOutput> (TInput [] array, Converter<TInput, TOutput> converter)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			if (converter == null)
				throw new ArgumentNullException ("converter");
			
			TOutput [] output = new TOutput [array.Length];
			for (int i = 0; i < array.Length; i ++)
				output [i] = converter (array [i]);
			
			return output;
		}
		
		public static int FindLastIndex<T> (T [] array, Predicate <T> match)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			
			return FindLastIndex<T> (array, 0, array.Length, match);
		}
		
		public static int FindLastIndex<T> (T [] array, int startIndex, Predicate<T> match)
		{
			if (array == null)
				throw new ArgumentNullException ();
			
			return FindLastIndex<T> (array, startIndex, array.Length - startIndex, match);
		}
		
		public static int FindLastIndex<T> (T [] array, int startIndex, int count, Predicate<T> match)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			if (match == null)
				throw new ArgumentNullException ("match");
			
			if (startIndex > array.Length || startIndex + count > array.Length)
				throw new ArgumentOutOfRangeException ();
			
			for (int i = startIndex + count - 1; i >= startIndex; i--)
				if (match (array [i]))
					return i;
				
			return -1;
		}
		
		public static int FindIndex<T> (T [] array, Predicate<T> match)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			
			return FindIndex<T> (array, 0, array.Length, match);
		}
		
		public static int FindIndex<T> (T [] array, int startIndex, Predicate<T> match)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			
			return FindIndex<T> (array, startIndex, array.Length - startIndex, match);
		}
		
		public static int FindIndex<T> (T [] array, int startIndex, int count, Predicate<T> match)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			
			if (match == null)
				throw new ArgumentNullException ("match");
			
			if (startIndex > array.Length || startIndex + count > array.Length)
				throw new ArgumentOutOfRangeException ();
			
			for (int i = startIndex; i < startIndex + count; i ++)
				if (match (array [i]))
					return i;
				
			return -1;
		}
		
		public static int BinarySearch<T> (T [] array, T value)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			
			return BinarySearch<T> (array, 0, array.Length, value, null);
		}
		
		public static int BinarySearch<T> (T [] array, T value, IComparer<T> comparer)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			
			return BinarySearch<T> (array, 0, array.Length, value, comparer);
		}
		
		public static int BinarySearch<T> (T [] array, int offset, int length, T value)
		{
			return BinarySearch<T> (array, offset, length, value, null);
		}
		
		public static int BinarySearch<T> (T [] array, int index, int length, T value, IComparer<T> comparer)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			if (index < 0)
				throw new ArgumentOutOfRangeException ("index", Locale.GetText (
					"index is less than the lower bound of array."));
			if (length < 0)
				throw new ArgumentOutOfRangeException ("length", Locale.GetText (
					"Value has to be >= 0."));
			// re-ordered to avoid possible integer overflow
			if (index > array.Length - length)
				throw new ArgumentException (Locale.GetText (
					"index and length do not specify a valid range in array."));
			if (comparer == null)
				comparer = Comparer <T>.Default;
			
			int iMin = index;
			int iMax = index + length - 1;
			int iCmp = 0;
			try {
				while (iMin <= iMax) {
					int iMid = (iMin + iMax) / 2;
					iCmp = comparer.Compare (value, array [iMid]);

					if (iCmp == 0)
						return iMid;
					else if (iCmp < 0)
						iMax = iMid - 1;
					else
						iMin = iMid + 1; // compensate for the rounding down
				}
			} catch (Exception e) {
				throw new InvalidOperationException (Locale.GetText ("Comparer threw an exception."), e);
			}

			return ~iMin;
		}
		
		public static int IndexOf<T> (T [] array, T value)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
	
			return IndexOf<T> (array, value, 0, array.Length);
		}

		public static int IndexOf<T> (T [] array, T value, int startIndex)
		{
			if (array == null)
				throw new ArgumentNullException ("array");

			return IndexOf<T> (array, value, startIndex, array.Length - startIndex);
		}

		public static int IndexOf<T> (T [] array, T value, int startIndex, int count)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			
			// re-ordered to avoid possible integer overflow
			if (count < 0 || startIndex < array.GetLowerBound (0) || startIndex - 1 > array.GetUpperBound (0) - count)
				throw new ArgumentOutOfRangeException ();

			int max = startIndex + count;
			EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
			for (int i = startIndex; i < max; i++) {
				if (equalityComparer.Equals (value, array [i]))
					return i;
			}

			return -1;
		}
		
		public static int LastIndexOf<T> (T [] array, T value)
		{
			if (array == null)
				throw new ArgumentNullException ("array");

			return LastIndexOf<T> (array, value, array.Length - 1);
		}

		public static int LastIndexOf<T> (T [] array, T value, int startIndex)
		{
			if (array == null)
				throw new ArgumentNullException ("array");

			return LastIndexOf<T> (array, value, startIndex, startIndex + 1);
		}

		public static int LastIndexOf<T> (T [] array, T value, int startIndex, int count)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			
			if (count < 0 || startIndex > array.Length || startIndex - count + 1 < 0)
				throw new ArgumentOutOfRangeException ();

			EqualityComparer<T> equalityComparer = EqualityComparer<T>.Default;
			for (int i = startIndex; i >= startIndex - count + 1; i--) {
				if (equalityComparer.Equals (value, array [i]))
					return i;
			}

			return -1;
		}
		
		public static T [] FindAll<T> (T [] array, Predicate <T> match)
		{
			if (array == null)
				throw new ArgumentNullException ("array");

			if (match == null)
				throw new ArgumentNullException ("match");
			
			int pos = 0;
			T [] d = new T [array.Length];
			foreach (T t in array)
				if (match (t))
					d [pos++] = t;
			
			Resize <T> (ref d, pos);
			return d;
		}

		public static bool Exists<T> (T [] array, Predicate <T> match)
		{
			if (array == null)
				throw new ArgumentNullException ("array");

			if (match == null)
				throw new ArgumentNullException ("match");
			
			foreach (T t in array)
				if (match (t))
					return true;
			return false;
		}

		public static ReadOnlyCollection<T> AsReadOnly<T> (T[] array)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			return new ReadOnlyCollection<T> (new ArrayReadOnlyList<T> (array));
		}

		public static T Find<T> (T [] array, Predicate<T> match)
		{
			if (array == null)
				throw new ArgumentNullException ("array");

			if (match == null)
				throw new ArgumentNullException ("match");
			
			foreach (T t in array)
				if (match (t))
					return t;
				
			return default (T);
		}
		
		public static T FindLast<T> (T [] array, Predicate <T> match)
		{
			if (array == null)
				throw new ArgumentNullException ("array");

			if (match == null)
				throw new ArgumentNullException ("match");
			
			for (int i = array.Length - 1; i >= 0; i--)
				if (match (array [i]))
					return array [i];
				
			return default (T);
		}

		//
		// The constrained copy should guarantee that if there is an exception thrown
		// during the copy, the destination array remains unchanged.
		// This is related to System.Runtime.Reliability.CER
		public static void ConstrainedCopy (Array s, int s_i, Array d, int d_i, int c)
		{
			Copy (s, s_i, d, d_i, c);
		}
#endif

#if NET_2_0
		class ArrayReadOnlyList<T> : IList<T>
		{
			T [] array;
			bool is_value_type;

			public ArrayReadOnlyList (T [] array)
			{
				this.array = array;
				is_value_type = typeof (T).IsValueType;
			}

			public T this [int index] {
				get {
					if (unchecked ((uint) index) >= unchecked ((uint) array.Length))
						throw new ArgumentOutOfRangeException ("index");
					return array [index];
				}
				set { throw ReadOnlyError (); }
			}

			public int Count {
				get { return array.Length; }
			}

			public bool IsReadOnly {
				get { return true; }
			}

			public void Add (T item)
			{
				throw ReadOnlyError ();
			}

			public void Clear ()
			{
				throw ReadOnlyError ();
			}

			public bool Contains (T item)
			{
				return Array.IndexOf<T> (array, item) >= 0;
			}

			public void CopyTo (T [] array, int index)
			{
				array.CopyTo (array, index);
			}

			IEnumerator IEnumerable.GetEnumerator ()
			{
				return GetEnumerator ();
			}

			public IEnumerator<T> GetEnumerator ()
			{
				for (int i = 0; i < array.Length; i++)
					yield return array [i];
			}

			public int IndexOf (T item)
			{
				return Array.IndexOf<T> (array, item);
			}

			public void Insert (int index, T item)
			{
				throw ReadOnlyError ();
			}

			public bool Remove (T item)
			{
				throw ReadOnlyError ();
			}

			public void RemoveAt (int index)
			{
				throw ReadOnlyError ();
			}

			Exception ReadOnlyError ()
			{
				return new NotSupportedException ("This collection is read-only.");
			}
		}
#endif
    }
}
