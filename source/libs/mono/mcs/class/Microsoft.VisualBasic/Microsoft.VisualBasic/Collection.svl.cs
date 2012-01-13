namespace Microsoft.VisualBasic
{
    using Microsoft.VisualBasic.CompilerServices;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.Reflection;
    using System.Runtime.InteropServices;
    using System.Threading;

    [DebuggerDisplay("Count = {Count}")]
    public sealed class Collection : IEnumerable
    {
        private FastList m_ItemsList;
        private List<WeakReference> m_Iterators;
        private Dictionary<string, Node> m_KeyedNodesHash;

        public Collection()
        {
            this.Initialize(Utils.GetCultureInfo(), 0);
        }

        public void Add(object Item, [Optional, DefaultParameterValue(null)] string Key, [Optional, DefaultParameterValue(null)] object Before, [Optional, DefaultParameterValue(null)] object After)
        {
            if ((Before != null) && (After != null))
            {
                throw new ArgumentException(Utils.GetResourceString("Collection_BeforeAfterExclusive"));
            }
            Node node = new Node(Key, Item);
            if (Key != null)
            {
                try
                {
                    this.m_KeyedNodesHash.Add(Key, node);
                }
                catch (ArgumentException)
                {
                    throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Collection_DuplicateKey")), 0x1c9);
                }
            }
            try
            {
                if ((Before == null) && (After == null))
                {
                    this.m_ItemsList.Add(node);
                }
                else if (Before != null)
                {
                    string key = Before as string;
                    if (key != null)
                    {
                        Node node2 = null;
                        if (!this.m_KeyedNodesHash.TryGetValue(key, out node2))
                        {
                            throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Before" }));
                        }
                        this.m_ItemsList.InsertBefore(node, node2);
                    }
                    else
                    {
                        this.m_ItemsList.Insert(Conversions.ToInteger(Before) - 1, node);
                    }
                }
                else
                {
                    string str2 = After as string;
                    if (str2 != null)
                    {
                        Node node3 = null;
                        if (!this.m_KeyedNodesHash.TryGetValue(str2, out node3))
                        {
                            throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "After" }));
                        }
                        this.m_ItemsList.InsertAfter(node, node3);
                    }
                    else
                    {
                        this.m_ItemsList.Insert(Conversions.ToInteger(After), node);
                    }
                }
            }
            catch (OutOfMemoryException)
            {
                throw;
            }
            catch (ThreadAbortException)
            {
                throw;
            }
            catch (StackOverflowException)
            {
                throw;
            }
            catch (Exception)
            {
                if (Key != null)
                {
                    this.m_KeyedNodesHash.Remove(Key);
                }
                throw;
            }
            this.AdjustEnumeratorsOnNodeInserted(node);
        }

        internal void AddIterator(WeakReference weakref)
        {
            this.m_Iterators.Add(weakref);
        }

        private void AdjustEnumeratorsHelper(Node NewOrRemovedNode, ForEachEnum.AdjustIndexType Type)
        {
            for (int i = this.m_Iterators.Count - 1; i >= 0; i--)
            {
                WeakReference reference = this.m_Iterators[i];
                if (reference.IsAlive)
                {
                    ForEachEnum target = (ForEachEnum) reference.Target;
                    if (target != null)
                    {
                        target.Adjust(NewOrRemovedNode, Type);
                    }
                }
                else
                {
                    this.m_Iterators.RemoveAt(i);
                }
            }
        }

        private void AdjustEnumeratorsOnNodeInserted(Node NewNode)
        {
            this.AdjustEnumeratorsHelper(NewNode, ForEachEnum.AdjustIndexType.Insert);
        }

        private void AdjustEnumeratorsOnNodeRemoved(Node RemovedNode)
        {
            this.AdjustEnumeratorsHelper(RemovedNode, ForEachEnum.AdjustIndexType.Remove);
        }

        public void Clear()
        {
            this.m_KeyedNodesHash.Clear();
            this.m_ItemsList.Clear();
            for (int i = this.m_Iterators.Count - 1; i >= 0; i--)
            {
                WeakReference reference = this.m_Iterators[i];
                if (reference.IsAlive)
                {
                    ForEachEnum target = (ForEachEnum) reference.Target;
                    if (target != null)
                    {
                        target.AdjustOnListCleared();
                    }
                }
                else
                {
                    this.m_Iterators.RemoveAt(i);
                }
            }
        }

        public bool Contains(string Key)
        {
            if (Key == null)
            {
                throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Key" }));
            }
            return this.m_KeyedNodesHash.ContainsKey(Key);
        }

        public IEnumerator GetEnumerator()
        {
            for (int i = this.m_Iterators.Count - 1; i >= 0; i--)
            {
                WeakReference reference = this.m_Iterators[i];
                if (!reference.IsAlive)
                {
                    this.m_Iterators.RemoveAt(i);
                }
            }
            ForEachEnum target = new ForEachEnum(this);
            WeakReference item = new WeakReference(target);
            target.WeakRef = item;
            this.m_Iterators.Add(item);
            return target;
        }

        internal Node GetFirstListNode()
        {
            return this.m_ItemsList.GetFirstListNode();
        }

        private void IndexCheck(int Index)
        {
            if ((Index < 1) || (Index > this.m_ItemsList.Count()))
            {
                throw new IndexOutOfRangeException(Utils.GetResourceString("Argument_CollectionIndex"));
            }
        }

        private void Initialize(CultureInfo CultureInfo, [Optional, DefaultParameterValue(0)] int StartingHashCapacity)
        {
            if (StartingHashCapacity > 0)
            {
                this.m_KeyedNodesHash = new Dictionary<string, Node>(StartingHashCapacity, StringComparer.Create(CultureInfo, true));
            }
            else
            {
                this.m_KeyedNodesHash = new Dictionary<string, Node>(StringComparer.Create(CultureInfo, true));
            }
            this.m_ItemsList = new FastList();
            this.m_Iterators = new List<WeakReference>();
        }

        private FastList InternalItemsList()
        {
            return this.m_ItemsList;
        }

        public void Remove(int Index)
        {
            this.IndexCheck(Index);
            Node removedNode = this.m_ItemsList.RemoveAt(Index - 1);
            this.AdjustEnumeratorsOnNodeRemoved(removedNode);
            if (removedNode.m_Key != null)
            {
                this.m_KeyedNodesHash.Remove(removedNode.m_Key);
            }
            removedNode.m_Prev = null;
            removedNode.m_Next = null;
        }

        public void Remove(string Key)
        {
            Node node = null;
            if (!this.m_KeyedNodesHash.TryGetValue(Key, out node))
            {
                throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Key" }));
            }
            this.AdjustEnumeratorsOnNodeRemoved(node);
            this.m_KeyedNodesHash.Remove(Key);
            this.m_ItemsList.RemoveNode(node);
            node.m_Prev = null;
            node.m_Next = null;
        }

        internal void RemoveIterator(WeakReference weakref)
        {
            this.m_Iterators.Remove(weakref);
        }

        public int Count
        {
            get
            {
                return this.m_ItemsList.Count();
            }
        }

        public object this[int Index]
        {
            get
            {
                this.IndexCheck(Index);
                return this.m_ItemsList.get_Item(Index - 1).m_Value;
            }
        }

        public object this[object Index]
        {
            get
            {
                int num;
                if (((Index is string) || (Index is char)) || (Index is char[]))
                {
                    string str = Conversions.ToString(Index);
                    return this[str];
                }
                try
                {
                    num = Conversions.ToInteger(Index);
                }
                catch (StackOverflowException exception)
                {
                    throw exception;
                }
                catch (OutOfMemoryException exception2)
                {
                    throw exception2;
                }
                catch (ThreadAbortException exception3)
                {
                    throw exception3;
                }
                catch (Exception)
                {
                    throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Index" }));
                }
                return this[num];
            }
        }

        public object this[string Key]
        {
            get
            {
                if (Key == null)
                {
                    throw new IndexOutOfRangeException(Utils.GetResourceString("Argument_CollectionIndex"));
                }
                Node node = null;
                if (!this.m_KeyedNodesHash.TryGetValue(Key, out node))
                {
                    throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValue1", new string[] { "Index" }));
                }
                return node.m_Value;
            }
        }

        internal sealed class CollectionDebugView
        {
            [DebuggerBrowsable(DebuggerBrowsableState.Never)]
            private Collection m_InstanceBeingWatched;

            public CollectionDebugView(Collection RealClass)
            {
                this.m_InstanceBeingWatched = RealClass;
            }

            [DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
            public object[] Items
            {
                get
                {
                    int count = this.m_InstanceBeingWatched.Count;
                    if (count == 0)
                    {
                        return null;
                    }
                    object[] objArray2 = new object[count + 1];
                    objArray2[0] = Utils.GetResourceString("EmptyPlaceHolderMessage");
                    int num3 = count;
                    for (int i = 1; i <= num3; i++)
                    {
                        Collection.Node node = this.m_InstanceBeingWatched.InternalItemsList().get_Item(i - 1);
                        objArray2[i] = new Collection.KeyValuePair(node.m_Key, node.m_Value);
                    }
                    return objArray2;
                }
            }
        }

        private sealed class FastList
        {
            private int m_Count = 0;
            private Collection.Node m_EndOfList;
            private Collection.Node m_StartOfList;

            internal FastList()
            {
            }

            internal void Add(Collection.Node Node)
            {
                if (this.m_StartOfList == null)
                {
                    this.m_StartOfList = Node;
                }
                else
                {
                    this.m_EndOfList.m_Next = Node;
                    Node.m_Prev = this.m_EndOfList;
                }
                this.m_EndOfList = Node;
                this.m_Count++;
            }

            internal void Clear()
            {
                this.m_StartOfList = null;
                this.m_EndOfList = null;
                this.m_Count = 0;
            }

            internal int Count()
            {
                return this.m_Count;
            }

            private void DeleteNode(Collection.Node NodeToBeDeleted, Collection.Node PrevNode)
            {
                if (PrevNode == null)
                {
                    this.m_StartOfList = this.m_StartOfList.m_Next;
                    if (this.m_StartOfList == null)
                    {
                        this.m_EndOfList = null;
                    }
                    else
                    {
                        this.m_StartOfList.m_Prev = null;
                    }
                }
                else
                {
                    PrevNode.m_Next = NodeToBeDeleted.m_Next;
                    if (PrevNode.m_Next == null)
                    {
                        this.m_EndOfList = PrevNode;
                    }
                    else
                    {
                        PrevNode.m_Next.m_Prev = PrevNode;
                    }
                }
                this.m_Count--;
            }

            internal Collection.Node GetFirstListNode()
            {
                return this.m_StartOfList;
            }

            private Collection.Node GetNodeAtIndex(int Index, [Optional, DefaultParameterValue(null)] ref Collection.Node PrevNode)
            {
                Collection.Node startOfList = this.m_StartOfList;
                int num = 0;
                PrevNode = null;
                while ((num < Index) && (startOfList != null))
                {
                    PrevNode = startOfList;
                    startOfList = startOfList.m_Next;
                    num++;
                }
                return startOfList;
            }

            internal void Insert(int Index, Collection.Node Node)
            {
                Collection.Node prevNode = null;
                if ((Index < 0) || (Index > this.m_Count))
                {
                    throw new ArgumentOutOfRangeException("Index");
                }
                Collection.Node nodeAtIndex = this.GetNodeAtIndex(Index, ref prevNode);
                this.Insert(Node, prevNode, nodeAtIndex);
            }

            private void Insert(Collection.Node Node, Collection.Node PrevNode, Collection.Node CurrentNode)
            {
                Node.m_Next = CurrentNode;
                if (CurrentNode != null)
                {
                    CurrentNode.m_Prev = Node;
                }
                if (PrevNode == null)
                {
                    this.m_StartOfList = Node;
                }
                else
                {
                    PrevNode.m_Next = Node;
                    Node.m_Prev = PrevNode;
                }
                if (Node.m_Next == null)
                {
                    this.m_EndOfList = Node;
                }
                this.m_Count++;
            }

            internal void InsertAfter(Collection.Node Node, Collection.Node NodeToInsertAfter)
            {
                this.Insert(Node, NodeToInsertAfter, NodeToInsertAfter.m_Next);
            }

            internal void InsertBefore(Collection.Node Node, Collection.Node NodeToInsertBefore)
            {
                this.Insert(Node, NodeToInsertBefore.m_Prev, NodeToInsertBefore);
            }

            internal Collection.Node RemoveAt(int Index)
            {
                Collection.Node startOfList = this.m_StartOfList;
                int num = 0;
                Collection.Node prevNode = null;
                while ((num < Index) && (startOfList != null))
                {
                    prevNode = startOfList;
                    startOfList = startOfList.m_Next;
                    num++;
                }
                if (startOfList == null)
                {
                    throw new ArgumentOutOfRangeException("Index");
                }
                this.DeleteNode(startOfList, prevNode);
                return startOfList;
            }

            internal void RemoveNode(Collection.Node NodeToBeDeleted)
            {
                this.DeleteNode(NodeToBeDeleted, NodeToBeDeleted.m_Prev);
            }

            internal Collection.Node this[int Index]
            {
                get
                {
                    Collection.Node prevNode = null;
                    Collection.Node nodeAtIndex = this.GetNodeAtIndex(Index, ref prevNode);
                    if (nodeAtIndex == null)
                    {
                        throw new ArgumentOutOfRangeException("Index");
                    }
                    return nodeAtIndex;
                }
            }
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct KeyValuePair
        {
            private object m_Key;
            private object m_Value;
            internal KeyValuePair(object NewKey, object NewValue)
            {
                this = new Collection.KeyValuePair();
                this.m_Key = NewKey;
                this.m_Value = NewValue;
            }

            public object Key
            {
                get
                {
                    return this.m_Key;
                }
            }
            public object Value
            {
                get
                {
                    return this.m_Value;
                }
            }
        }

        internal sealed class Node
        {
            internal string m_Key;
            internal Collection.Node m_Next;
            internal Collection.Node m_Prev;
            internal object m_Value;

            internal Node(string Key, object Value)
            {
                this.m_Value = Value;
                this.m_Key = Key;
            }
        }
    }
}

