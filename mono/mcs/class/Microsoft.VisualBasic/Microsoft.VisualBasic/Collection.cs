//
// Collection.vb
//
// Author:
// Chris J Breisch (cjbreisch@altavista.net)
// Mizrahi Rafael (rafim@mainsoft.com)
// Boris Kirzner (borisk@mainsoft.com)
//
//
// Copyright (C) 2002-2006 Mainsoft Corporation.
// Copyright (C) 2004-2006 Novell, Inc (http://www.novell.com)
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
using System;
using System.Collections;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Microsoft.VisualBasic
{
    //[DebuggerTypeProxy(typeof(Collection.CollectionDebugView))]
    [Serializable]
    //[DebuggerDisplay("Count = {Count}")]
    public sealed class Collection : IList, ISerializable
    {
        // Declarations
        readonly Hashtable m_Hashtable = new Hashtable();
        readonly ArrayList m_HashIndexers = new ArrayList();
        int m_KeysCount = int.MinValue;
        internal bool Modified;

        bool m_Broken;

        class Enumerator : IEnumerator
        {
            object currentKey;
            bool afterLast;
            Collection m_col;

            private object m_Current;

            public Enumerator(Collection coll)
            {
                m_col = coll;
                currentKey = null;
            }

            public void Reset()
            {
                if ((m_col.Modified))
                {
                }
                //LAMESPEC: spec says throw exception, MS doesn't
                //throw new InvalidOperationException();
                currentKey = null;
                afterLast = false;
            }

            public bool MoveNext()
            {
                if ((m_col.Modified))
                {
                }
                //LAMESPEC: spec says throw exception, MS doesn't
                //throw new InvalidOperationException();

                if (currentKey == null & m_col.Count > 0)
                {
                    currentKey = m_col.m_HashIndexers[0];
                    m_Current = CurrentInternal;
                    return true;
                }

                if (afterLast)
                {
                    m_Current = null;
                    return false;
                }

                int index = m_col.m_HashIndexers.IndexOf(currentKey);
                if (index >= m_col.Count - 1)
                {
                    afterLast = true;
                    m_Current = null;
                    return false;
                }

                currentKey = m_col.m_HashIndexers[index + 1];
                afterLast = false;

                m_Current = CurrentInternal;
                return true;
            }

            public object Current
            {
                get { return m_Current; }
            }

            private object CurrentInternal
            {
                get
                {
                    int index = m_col.m_HashIndexers.IndexOf(currentKey);
                    if (index > m_col.Count - 1)
                        return null;
                    if (afterLast)
                    {
                        if (MoveNext())
                        {
                            Enumerator tmo = this;
                            return tmo.Current;
                        }
                        return null;
                    }
                    return m_col[index + 1];
                }
            }
        }

        // Constructors
        // Properties
        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool IsSynchronized
        {
            get { return m_Hashtable.IsSynchronized; }
        }

        public object SyncRoot
        {
            get { return m_Hashtable.SyncRoot; }
        }

        public bool IsFixedSize
        {
            get { return false; }
        }

        public int Count
        {
            get { return m_HashIndexers.Count; }
        }

        [EditorBrowsable(EditorBrowsableState.Advanced)]
        public object this[object index]
        {
            get
            {
                if (index == null) throw new IndexOutOfRangeException("Argument 'Index' is not a valid index.");

                if (index is int)
                    return this[(int)index];

                int idx = m_HashIndexers.IndexOf(index);
                if (idx == -1)
                {
                    throw new ArgumentException("Argument 'Index' is not a valid value.");
                }
                return this[idx + 1];
            }
        }

        public object this[int index]
        {
            get
            {
                //The behaviour of Collection.Item is NOT the same as the IList.Item interface implementation.
                index = index - 1;

                if (index > Count - 1 | index < 0)
                {
                    throw new IndexOutOfRangeException("Collection1 index must be in the range 1 to the size of the collection.");
                }

                return m_Hashtable[m_HashIndexers[index]];
            }
        }

        object IList.this[int index]
        {
            get
            {
                if (m_Broken) throw new InvalidCastException();

                if (index < 0 && Count > 0)
                {
                    //Oh man this behaviour is weird...
                    index = 0;
                }

                if (index > Count - 1 | index < 0)
                {
                    throw new ArgumentOutOfRangeException("Collection1 index must be in the range 1 to the size of the collection.");
                }

                return m_Hashtable[m_HashIndexers[index]];
            }

            set
            {
                m_Broken = true;
                if (index < 0 && Count > 0)
                {
                    //Oh man this behaviour is weird...
                    index = 0;
                }

                if (index > Count | index < 0)
                {
                    throw new ArgumentOutOfRangeException("Index");
                }

                if (index == -1)
                {
                    m_Hashtable[m_HashIndexers[0]] = value;
                }
                else
                {
                    m_Hashtable[m_HashIndexers[index]] = value;
                }
            }
        }

        public int IndexOf(object value)
        {

            int index = -1;

            foreach (DictionaryEntry enTry in m_Hashtable)
            {
                if (object.ReferenceEquals(enTry.Value, value))
                {
                    index = m_HashIndexers.IndexOf(enTry.Key);
                    break; // TODO: might not be correct. Was : Exit For
                }

                // also allow value comparison to work for types that do not
                // override equality operator
                if ((enTry.Value.GetType().Name == value.GetType().Name))
                {
                    if (object.Equals(enTry.Value, value))
                    {
                        index = m_HashIndexers.IndexOf(enTry.Key);
                        break; // TODO: might not be correct. Was : Exit For
                    }
                }
            }

            return index;

        }

        public bool Contains(string key)
        {
            return m_Hashtable.ContainsKey(key);
        }

        bool IList.Contains(object value)
        {
            return IndexOf(value) >= 0;
        }

        public void Clear()
        {
            m_Hashtable.Clear();
            m_HashIndexers.Clear();
            m_KeysCount = int.MinValue;
        }

        void IList.Clear()
        {
            Clear();
        }

        public void Remove(string Key)
        {

            if (m_Hashtable.ContainsKey(Key))
            {
                m_Hashtable.Remove(Key);
                m_HashIndexers.Remove(Key);
                Modified = true;
            }
            else
            {
                throw new ArgumentException("Argument 'Key' is not a valid value.");
            }

        }

        public void Remove(int Index)
        {

            try
            {
                // Collections are 1-based
                m_Hashtable.Remove(m_HashIndexers[Index - 1]);
                m_HashIndexers.RemoveAt(Index - 1);
                Modified = true;
            }
            catch (ArgumentOutOfRangeException e)
            {
                throw new IndexOutOfRangeException("Collection1 index must be in the range 1 to the size of the collection.");
            }
        }

        public void Remove(object value)
        {
            //FIXME: .Net behaviour is unstable
            int index = IndexOf(value);
            if (index != -1)
            {
                Remove(index + 1);
            }
        }

        public void RemoveAt(int index)
        {
            if (index + 1 > Count | (index == -1 & Count == 0))
            {
                throw new ArgumentOutOfRangeException("Index");
            }

            if (index == -1)
            {
                Remove(1);
            }
            else
            {
                Remove(index + 1);
            }
        }

        public void Insert(int index, object value)
        {
            if (index < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (index + 2 > Count + 1)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (index + 2 >= Count)
            {
                Add(value, null, null, null);
            }
            else
            {
                Insert(index + 2, value, GetNextKey(value));
            }

        }

        private void Insert(int index, object value, string Key)
        {
            m_HashIndexers.Insert(index - 1, Key);
            m_Hashtable.Add(Key, value);
            Modified = true;
        }

        int IList.Add(object value)
        {
            return AddByKey(value, GetNextKey(value));
        }

        private int AddByKey(object Item, string Key)
        {
            m_Hashtable.Add(Key, Item);
            Modified = true;

            return m_HashIndexers.Add(Key);
        }

        public void Add(object Item, string Key, object Before, object After)
        {
            int Position = int.MinValue;

            // check for valid args
            if (((Before != null)) & ((After != null)))
            {
                throw new ArgumentException("'Before' and 'After' arguments cannot be combined.");
            }
            if (Key != null & m_HashIndexers.IndexOf(Key) != -1)
            {
                throw new ArgumentException();
            }
            if ((Before != null))
            {
                // Looks like its an implementation bug in .NET
                // Not very satisfied with the fix, but did it
                // just to bring the similar behaviour on mono
                // as well.
                if (Before is int)
                {
                    Position = Convert.ToInt32(Before);
                    if (Position != (m_HashIndexers.Count + 1))
                    {
                        Position = GetIndexPosition(Before);
                    }
                }
                else
                {
                    Position = GetIndexPosition(Before);
                }
            }
            if ((After != null))
            {
                Position = GetIndexPosition(After) + 1;
            }
            if (Key == null)
            {
                Key = GetNextKey(Item);
            }

            if (Position > (m_HashIndexers.Count + 1) | Position == int.MinValue)
            {
                AddByKey(Item, Key);
            }
            else
            {
                Insert(Position, Item, Key);
            }
        }

        private string GetNextKey(object value)
        {
            m_KeysCount = m_KeysCount + 1;
            string key = null;
            if (value == null)
            {
                key = "Nothing";
            }
            else
            {
                key = value.ToString();
            }
            return (key + m_KeysCount);
        }

        private int GetIndexPosition(object Item)
        {
            int Position = int.MinValue;

            if (Item is string)
            {
                Position = m_HashIndexers.IndexOf(Item) + 1;
            }
            else if (Item is int)
            {
                Position = Convert.ToInt32(Item);
            }
            else
            {
                throw new InvalidCastException();
            }
            if (Position < 0)
            {
                throw new ArgumentOutOfRangeException("Specified argument was out of the range of valid values.");
            }

            //Position must be from 1 to value of collections Count
            if (Position > m_HashIndexers.Count)
            {
                throw new ArgumentOutOfRangeException("Specified argument was out of the range of valid values.");
            }

            return Position;

        }

        public void CopyTo(Array array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException();
            }

            if (index < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (array.Rank > 1 | index >= array.Length | Count > (array.Length - index))
            {
                throw new ArgumentException();
            }

            //Dim NewArray As System.Array = array.CreateInstance(Type.GetType("System.Object"), m_HashIndexers.Count - index)

            // Collections are 1-based
            for (int i = 0; i <= m_HashIndexers.Count - 1; i++)
            {
                array.SetValue(m_Hashtable[m_HashIndexers[i]], i + index);
            }
        }

        public IEnumerator GetEnumerator()
        {
            return new Enumerator(this);
        }
    }
}

