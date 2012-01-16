using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.CFG
{
    //http://en.wikipedia.org/wiki/Basic_block

    /// <summary>
    /// Graph node
    /// </summary>
    internal class Node
    {
        #region Constructors
        public Node()
        {
        }

        public Node(string name)
        {
            Name = name;
        }
        #endregion

        #region Public Properties
        public virtual NodeType NodeType
        {
            get { return NodeType.BasicBlock; }
        }

        public bool IsBasicBlock
        {
            get { return NodeType == NodeType.BasicBlock; }
        }

        public Block OwnerBlock { get; set; }

        public bool IsOneWay
        {
            get { return HasOneOut; }
        }

        public bool IsTwoWay
        {
            get
            {
                var e = _firstOut;
                if (e == null) return false;
                e = e.NextOut;
                if (e == null) return false;
                e = e.NextOut;
                return e == null;
            }
        }

        public bool IsNWay
        {
            get
            {
                var e = _firstOut;
                if (e == null) return false;
                return e.NextOut != null;
            }
        }

        public bool IsSwitch
        {
            get
            {
                if (NodeType != NodeType.BasicBlock) return false;
                int n = CodeLength;
                if (n == 0) return false;
                return Code[n - 1].IsSwitch;
            }
        }

        public bool IsSwitchCase
        {
            get { return InEdges.Any(i => i.From.IsSwitch); }
        }

        public bool HasZeroIn
        {
            get { return _firstIn == null; }
        }

        public bool HasOneIn
        {
            get { return _firstIn != null && _firstIn.NextIn == null; }
        }

        public bool HasZeroOrOneIn
        {
            get
            {
                if (_firstIn == null) return true;
                return _firstIn.NextIn == null;
            }
        }

        public bool HasZeroOut
        {
            get { return _firstOut == null; }
        }

        public bool HasOneOut
        {
            get { return _firstOut != null && _firstOut.NextOut == null; }
        }

        /// <summary>
        /// Gets or sets index of the node in owner graph node set.
        /// </summary>
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        int _index = -1;

        public string Name { get; set; }

        public virtual string GetSourceName(bool subgraph)
        {
            return Name;
        }

        public virtual string GetTargetName(bool subgraph)
        {
            return Name;
        }

        public Node Goto;
        public IGotoStatement GotoStatement;
        public ILabeledStatement Label;

        public int InCount
        {
            get
            {
                int n = 0;
                for (var e = _firstIn; e != null; e = e.NextIn)
                    ++n;
                return n;
            }
        }

        public int OutCount
        {
            get
            {
                int n = 0;
                for (var e = _firstOut; e != null; e = e.NextOut)
                    ++n;
                return n;
            }
        }

#if DEBUG
        public int Phase;
#endif
        #endregion

        #region Flags
        public NodeFlags Flags
        {
            get { return _flags; }
            set { _flags = value; }
        }
        NodeFlags _flags;

        bool IsFlag(NodeFlags f)
        {
            return (_flags & f) != 0;
        }

        void SetFlag(NodeFlags f, bool value)
        {
            if (value) _flags |= f;
            else _flags &= ~f;
        }

        public bool IsVisited
        {
            get { return IsFlag(NodeFlags.Visited); }
            set { SetFlag(NodeFlags.Visited, value); }
        }

        public bool IsLabeled
        {
            get { return IsFlag(NodeFlags.Labeled); }
            set { SetFlag(NodeFlags.Labeled, value); }
        }

        public bool IsEntry
        {
            get { return IsFlag(NodeFlags.Entry); }
            set { SetFlag(NodeFlags.Entry, value); }
        }

        public bool IsNew
        {
            get { return IsFlag(NodeFlags.New); }
            set { SetFlag(NodeFlags.New, value); }
        }

        public bool InStack
        {
            get { return IsFlag(NodeFlags.InStack); }
            set { SetFlag(NodeFlags.InStack, value); }
        }

        public bool PreventDetach
        {
            get { return IsFlag(NodeFlags.PreventDetach); }
            set { SetFlag(NodeFlags.PreventDetach, value); }
        }

        public int BackInCount
        {
            get { return _backInCount; }
            set { _backInCount = value; }
        }
        int _backInCount;

        public int BackOutCount
        {
            get { return _backOutCount; }
            set { _backOutCount = value; }
        }
        int _backOutCount;

        public bool HasOutBackEdges
        {
            get { return _backOutCount > 0; }
        }

        public bool HasInBackEdges
        {
            get { return _backInCount > 0; }
        }

        public Block BeginSehBlock
        {
            get
            {
                int n = Code.Count;
                if (n <= 0) return null;
                var first = Code[0];
                var block = first.Block;
                if (block == null) return null;
                if (block.EntryIndex != first.Index) return null;
                return block;
            }
        }

        public Block EndSehBlock
        {
            get
            {
                int n = Code.Count;
                if (n <= 0) return null;
                var last = Code[n - 1];
                var block = last.Block;
                if (block == null) return null;
                if (block.ExitIndex != last.Index) return null;
                return block;
            }
        }

        public bool IsBeginOfSehBlock
        {
            get { return BeginSehBlock != null; }
        }

        public bool IsEndOfSehBlock
        {
            get { return EndSehBlock != null; }
        }
        #endregion

        #region Edges
        public Edge FirstIn
        {
            get { return _firstIn; }
        }
        Edge _firstIn;

        public Edge LastIn
        {
            get { return _lastIn; }
        }
        Edge _lastIn;

        public Edge FirstOut
        {
            get { return _firstOut; }
        }
        Edge _firstOut;

        public Edge LastOut
        {
            get { return _lastOut; }
        }
        Edge _lastOut;

        public Node FirstPredecessor
        {
            get
            {
                if (_firstIn != null)
                    return _firstIn.From;
                return null;
            }
        }

        public Node FirstSuccessor
        {
            get
            {
                if (_firstOut != null)
                    return _firstOut.To;
                return null;
            }
        }

        public Edge FalseEdge
        {
            get
            {
                if (IsTwoWay)
                {
                    var e = _firstOut;
                    if (e.Label == 0) return e;
                    return e.NextOut;
                }
                return null;
            }
        }

        public Edge TrueEdge
        {
            get
            {
                if (IsTwoWay)
                {
                    var e = _firstOut;
                    if (e.Label == 1) return e;
                    return e.NextOut;
                }
                return null;
            }
        }
        #endregion

        #region Sibling Linked List

    	/// <summary>
    	/// Gets or sets the parent of this node.
    	/// </summary>
    	public Node Parent { get; set; }

    	/// <summary>
        /// Gets the previous node
        /// </summary>
        public Node Prev
        {
            get { return _prev; }
        }
        Node _prev;

        /// <summary>
        /// Gets the next node
        /// </summary>
        public Node Next
        {
            get { return _next; }
        }
        Node _next;

        public Node First
        {
            get
            {
                var node = this;
                while (node.Prev != null)
                    node = node.Prev;
                return node;
            }
        }

        public Node Last
        {
            get
            {
                var node = this;
                while (node.Next != null)
                    node = node.Next;
                return node;
            }
        }

        /// <summary>
        /// Prepends a given node before this node.
        /// </summary>
        /// <param name="node">the node to prepend</param>
        public void Prepend(Node node)
        {
            node.Remove();
            if (_prev != null)
                _prev._next = node;
            node._prev = _prev;
            node._next = this;
            _prev = node;
        }

        /// <summary>
        /// Appends a given node after this node.
        /// </summary>
        /// <param name="node">the node to append</param>
        public void Append(Node node)
        {
            node.Remove();
            if (_next != null)
                _next._prev = node;
            node._prev = this;
            node._next = _next;
            _next = node;
        }

        /// <summary>
        /// Remove this node from the list.
        /// </summary>
        /// <returns>the next node in the list</returns>
        public Node Remove()
        {
            var prev = _prev;
            var next = _next;

            _prev = null;
            _next = null;

            if (prev != null)
                prev._next = next;
            if (next != null)
                next._prev = prev;

            return next;
        }
        #endregion

        #region Entry Linked List
        public Node PrevEntry
        {
            get { return _prevEntry; }
        }
        Node _prevEntry;

        public Node NextEntry
        {
            get { return _nextEntry; }
        }
        Node _nextEntry;

        public Node FirstEntry
        {
            get
            {
                if (IsEntry)
                {
                    var node = this;
                    while (node.PrevEntry != null)
                        node = node.PrevEntry;
                    return node;
                }
                else
                {
                    var node = this;
                    while (node.Prev != null)
                        node = node.Prev;
                    if (node.IsEntry)
                        return node.FirstEntry;
                    return null;
                }
            }
        }

        public Node LastEntry
        {
            get
            {
                if (IsEntry)
                {
                    var node = this;
                    while (node.NextEntry != null)
                        node = node.NextEntry;
                    return node;
                }
                else
                {
                    var node = this;
                    while (node.Prev != null)
                        node = node.Prev;
                    return node.LastEntry;
                }
            }
        }

        public void PrependEntry(Node node)
        {
            if (!node.IsEntry)
                throw new InvalidOperationException();
            node.RemoveEntry();
            if (_prevEntry != null)
                _prevEntry._nextEntry = node;
            node._prevEntry = _prevEntry;
            node._nextEntry = this;
            _prevEntry = node;
        }

        public void AppendEntry(Node node)
        {
            if (!node.IsEntry)
                throw new InvalidOperationException();
            node.RemoveEntry();
            if (_nextEntry != null)
                _nextEntry._prevEntry = node;
            node._prevEntry = this;
            node._nextEntry = _nextEntry;
            _nextEntry = node;
        }

        public Node RemoveEntry()
        {
            var prev = _prevEntry;
            var next = _nextEntry;

            _prevEntry = null;
            _nextEntry = null;

            if (prev != null)
                prev._nextEntry = next;
            if (next != null)
                next._prevEntry = prev;

            return next;
        }
        #endregion

        #region Iterators
        public IEnumerable<Edge> InEdges
        {
            get
            {
                for (var e = _firstIn; e != null; e = e.NextIn)
                    yield return e;
            }
        }

        public List<Edge> InEdgeList
        {
            get { return new List<Edge>(InEdges); }
        }

        public IEnumerable<Edge> OutEdges
        {
            get
            {
                for (var e = _firstOut; e != null; e = e.NextOut)
                    yield return e;
            }
        }

        public List<Edge> OutEdgeList
        {
            get { return new List<Edge>(OutEdges); }
        }

        public IEnumerable<Node> InNodes
        {
            get
            {
                for (var e = _firstIn; e != null; e = e.NextIn)
                    yield return e.From;
            }
        }

        public List<Node> InNodeList
        {
            get { return new List<Node>(InNodes); }
        }

        public IEnumerable<Node> OutNodes
        {
            get
            {
                for (var e = _firstOut; e != null; e = e.NextOut)
                    yield return e.To;
            }
        }

        public List<Node> OutNodeList
        {
            get { return new List<Node>(OutNodes); }
        }

        public IEnumerable<Node> Predecessors
        {
            get { return InNodes; }
        }

        public IEnumerable<Node> Successors
        {
            get { return OutNodes; }
        }

        public IEnumerable<Node> Nodes
        {
            get
            {
                for (var node = this; node != null; node = node.Next)
                    yield return node;
            }
        }

        public IEnumerable<Node> NextNodes
        {
            get
            {
                for (var node = Next; node != null; node = node.Next)
                    yield return node;
            }
        }

        public IEnumerable<Node> Entries
        {
            get
            {
                for (var node = FirstEntry; node != null; node = node.NextEntry)
                    yield return node;
            }
        }

        public NodeList GetGraphNodes()
        {
            var list = new NodeList();
            var inList = new Hashtable();
            //Stack<Node> stack = new Stack<Node>();
            for (var node = FirstEntry; node != null; node = node.NextEntry)
            {
                list.Add(node);
                inList[node] = node;
                for (var node2 = node.First; node2 != null; node2 = node2.Next)
                {
                    if (inList[node2] == null)
                    {
                        inList[node2] = node2;
                        list.Add(node2);
                    }
                }

                //inList[node] = node;
                //stack.Push(node);
                //while (stack.Count > 0)
                //{
                //    Node top = stack.Pop();
                //    foreach (Node suc in top.Successors)
                //    {
                //        if (inList[suc] == null)
                //        {
                //            list.Add(suc);
                //            inList[suc] = true;
                //            stack.Push(suc);
                //        }
                //    }
                //}
            }
            return list;
        }
        #endregion

        #region Methods
        #region Find
        public Edge FindIn(Node node)
        {
            for (var e = _firstIn; e != null; e = e.NextIn)
                if (e.From == node)
                    return e;
            return null;
        }

        public Edge FindOut(Node node)
        {
            for (var e = _firstOut; e != null; e = e.NextOut)
                if (e.To == node)
                    return e;
            return null;
        }

        public bool HasIn(Node node)
        {
            return FindIn(node) != null;
        }

        public bool HasOut(Node node)
        {
            return FindOut(node) != null;
        }

        public bool HasIn(int min)
        {
            int n = 0;
            for (var e = _firstIn; e != null; e = e.NextIn)
            {
                ++n;
                if (n >= min) return true;
            }
            return false;
        }

        public bool HasOut(int min)
        {
            int n = 0;
            for (var e = _firstOut; e != null; e = e.NextOut)
            {
                ++n;
                if (n >= min) return true;
            }
            return false;
        }
        #endregion

        #region Append
        public void AppendIn(Edge e)
        {
            if (_lastIn == null) _firstIn = e;
            else _lastIn.NextIn = e;
            _lastIn = e;
            if (e.IsBack) ++_backInCount;
        }

        public void AppendOut(Edge e)
        {
            if (_lastOut == null) _firstOut = e;
            else _lastOut.NextOut = e;
            _lastOut = e;
            if (e.IsBack) ++_backOutCount;
        }

        public void AppendOut(Node to)
        {
            var e = new Edge(this, to);
            AppendOut(e);
            to.AppendIn(e);
        }
        #endregion

        #region Remove
        /// <summary>
        /// Removes all in edges
        /// </summary>
        public void RemoveInEdges()
        {
            while (_firstIn != null)
            {
                RemoveIn(_firstIn);
            }
        }

        /// <summary>
        /// Removes all out edges
        /// </summary>
        public void RemoveOutEdges()
        {
            while (_firstOut != null)
            {
                RemoveOut(_firstOut);
            }
        }

        public bool RemoveIn(Edge e)
        {
            if (e.To == this)
            {
                var prev = e.PrevIn;
                var next = e.NextIn;

                e.PrevIn = null;
                e.NextIn = null;
                
                if (prev != null)
                    prev.NextIn = next;
                if (next != null)
                    next.PrevIn = prev;

                if (prev == null)
                    _firstIn = next;
                if (next == null)
                    _lastIn = prev;

                if (e.IsBack) --_backInCount;

                return true;
            }
            return false;
        }

        public bool RemoveOut(Edge e)
        {
            if (e.From == this)
            {
                var prev = e.PrevOut;
                var next = e.NextOut;

                e.PrevOut = null;
                e.NextOut = null;

                if (prev != null)
                    prev.NextOut = next;
                if (next != null)
                    next.PrevOut = prev;

                if (prev == null)
                    _firstOut = next;
                if (next == null)
                    _lastOut = prev;

                if (e.IsBack) --_backOutCount;

                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes in edge from given node
        /// </summary>
        /// <param name="node"></param>
        public bool RemoveIn(Node node)
        {
            var e = FindIn(node);
            if (e != null)
            {
                RemoveIn(e);
                node.RemoveOut(e);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Removes outgoing edge to given node
        /// </summary>
        /// <param name="node"></param>
        public bool RemoveOut(Node node)
        {
            var e = FindOut(node);
            if (e != null)
            {
                RemoveOut(e);
                node.RemoveIn(e);
                return true;
            }
            return false;
        }
        #endregion

        #region Replace
        /// <summary>
        /// Replaces an old node with a new node
        /// </summary>
        /// <param name="oldOut"></param>
        /// <param name="newOut"></param>
        public void ReplaceOut(Node oldOut, Node newOut)
        {
            RemoveOut(oldOut);
            AppendOut(newOut);
        }
        #endregion

        /// <summary>
        /// Revert the sequence of out edges
        /// </summary>
        public void RevertOuts()
        {
            Edge last = null;
            var e = _firstOut;
            _firstOut = _lastOut;
            _lastOut = e;
            while (e != null)
            {
                var next = e.NextOut;
                e.NextOut = last;
                last = e;
                e = next;
            }
        }
        
        public IEnumerable<Node> ReachableNodes
        {
            get { return _reachableNodes ?? (_reachableNodes = new List<Node>(EvalReachableNodes())); }
        }
        private List<Node> _reachableNodes;

        private IEnumerable<Node> EvalReachableNodes()
        {
            var stack = new Stack<Node>();
            var marked = new Hashtable();
            stack.Push(this);
            marked[this] = this;
            while (stack.Count > 0)
            {
                var from = stack.Pop();
                foreach (var e in from.OutEdges)
                {
                    if (e.IsBack) continue;
                    var rn = e.To;
                    if (!marked.Contains(rn))
                    {
                        yield return rn;
                        marked[rn] = rn;
                        stack.Push(rn);
                    }
                }
            }
        }

        public bool IsReachable(Node from)
        {
        	return from.ReachableNodes.Any(node => node == this);
        }

    	public void Detach()
        {
            if (PreventDetach) return;

            var list = new List<Edge>(InEdges);
            list.AddRange(OutEdges);
            foreach (var e in list)
                e.Remove();

//#if DEBUG
//            if (_firstIn != null)
//                throw new InvalidOperationException();
//            if (_lastIn != null)
//                throw new InvalidOperationException();
//            if (_firstOut != null)
//                throw new InvalidOperationException();
//            if (_lastOut != null)
//                throw new InvalidOperationException();
//#endif
            
            _index = -1;
            Remove();

            if (IsEntry)
            {
                RemoveEntry();
                IsEntry = false;
            }
        }

        public virtual void DetachKids()
        {
        }
        #endregion

        #region Code
        /// <summary>
        /// Instruction for this node
        /// </summary>
        public ILStream Code
        {
            get { return _code ?? (_code = new ILStream()); }
        }
        private ILStream _code;

        public int CodeLength
        {
            get { return _code == null ? 0 : _code.Count; }
        }

        /// <summary>
        /// Gets entry point of the Basic Block
        /// </summary>
        public Instruction EntryPoint
        {
            get { return _code.Count > 0 ? _code[0] : null; }
        }

        /// <summary>
        /// Gets exit point of the Basic Block
        /// </summary>
        public Instruction ExitPoint
        {
            get
            {
                int index = _code.Count - 1;
                return index >= 0 ? _code[index] : null;
            }
        }

        public int EntryIndex
        {
            get
            {
            	var p = EntryPoint;
            	return p != null ? p.Index : -1;
            }
        }

        public int ExitIndex
        {
            get
            {
            	var p = ExitPoint;
            	return p != null ? p.Index : -1;
            }
        }

        public void AddInstruction(Instruction i)
        {
            Code.Add(i);
            i.OwnerNode = this;
        }

#if DEBUG
        public string CodeSpan
        {
            get
            {
                if (NodeType == NodeType.BasicBlock || NodeType == NodeType.Sequence)
                    return string.Format("{0}, {1}", EntryPoint, ExitPoint);
                return "";
            }
        }
#endif
        #endregion

        #region Object Override Methods
        public override string ToString()
        {
            return ToString(true);
        }

        public virtual string ToString(bool full)
        {
            return FormatService.ToString(this, FormatOptions.Default);
        }
        #endregion

        #region State
        public NodeState State
        {
            get { return _stateStack.Peek(); }
        }

        public bool IsAnalysed
        {
            get { return State.IsAnalysed; }
            set { State.IsAnalysed = value; }
        }

        public bool IsTranslated
        {
            get { return State.IsTranslated; }
            set { State.IsTranslated = value; }
        }

        public int TranslationIndex
        {
            get { return State.TranslationIndex; }
            set { State.TranslationIndex = value; }
        }

        public List<IInstruction> TranslatedCode
        {
            get { return State.TranslatedCode; }
        }

        public int TranslatedEntryIndex
        {
            get { return State.TranslatedEntryIndex; }
            set { State.TranslatedEntryIndex = value; }
        }

        public EvalStack Stack
        {
            get { return State.Stack; }
            set { State.Stack = value; }
        }

        public EvalStack StackBefore
        {
            get { return State.StackBefore; }
            set { State.StackBefore = value; }
        }

        public Instruction LastInstruction
        {
            get { return State.LastInstruction; }
            set { State.LastInstruction = value; }
        }

        public bool IsFirstAssignment
        {
            get { return State.IsFirstAssignment; }
            set { State.IsFirstAssignment = value; }
        }

        public IParameter PartOfTernaryParam
        {
            get { return State.PartOfTernaryParam; }
            set { State.PartOfTernaryParam = value; }
        }

        public IParameter Parameter
        {
            get { return State.Parameter; }
            set { State.Parameter = value; }
        }

        public void PushState()
        {
            _stateStack.Push(new NodeState());
        }

        public void PopState()
        {
            _stateStack.Pop();
        }
        readonly Stack<NodeState> _stateStack = new Stack<NodeState>();
        #endregion
    }

    #region class NodeState
    internal class NodeState
    {
        public bool IsAnalysed
        {
            get;
            set;
        }

        public bool IsTranslated
        {
            get;
            set;
        }

        public int TranslationIndex
        {
            get { return _translationIndex; }
            set { _translationIndex = value; }
        }
        int _translationIndex = -1;

        public List<IInstruction> TranslatedCode
        {
            get { return _translatedCode ?? (_translatedCode = new List<IInstruction>()); }
        }
        private List<IInstruction> _translatedCode;

        public int TranslatedEntryIndex { get; set; }

        public EvalStack Stack { get; set; }

        public EvalStack StackBefore { get; set; }

        public Instruction LastInstruction;

        public bool IsFirstAssignment = true;

        public IParameter PartOfTernaryParam;

        public IParameter Parameter { get; set; }
    }
    #endregion

    #region enum NodeFlags
    [Flags]
    internal enum NodeFlags
    {
        None = 0x00,
        Visited = 0x01,
        Labeled = 0x02,
        InStack = 0x04,
        Entry = 0x08,
        New = 0x10,
        PreventDetach = 0x20,
        Analysed = 0x40,
        Translated = 0x80,
    }
    #endregion
}