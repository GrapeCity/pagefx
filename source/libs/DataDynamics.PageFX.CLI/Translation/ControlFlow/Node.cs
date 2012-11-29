using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CLI.Translation.ControlFlow.Services;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.CodeModel.Statements;

namespace DataDynamics.PageFX.CLI.Translation.ControlFlow
{
    //http://en.wikipedia.org/wiki/Basic_block

    /// <summary>
    /// Graph node
    /// </summary>
    internal sealed class Node
    {
    	public Node()
    	{
    		Index = -1;
    	}

    	public Node(string name) : this()
    	{
    		Name = name;
    	}

    	#region Public Properties

    	public Block OwnerBlock { get; set; }

        public bool IsOneWay
        {
            get { return HasOneOut; }
        }

        public bool IsTwoWay
        {
            get
            {
                var e = FirstOut;
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
                var e = FirstOut;
                if (e == null) return false;
                return e.NextOut != null;
            }
        }

    	public bool HasOneIn
        {
            get { return FirstIn != null && FirstIn.NextIn == null; }
        }

    	public bool HasOneOut
        {
            get { return FirstOut != null && FirstOut.NextOut == null; }
        }

    	/// <summary>
    	/// Gets or sets index of the node in owner graph node set.
    	/// </summary>
    	public int Index { get; set; }

    	public string Name { get; set; }

    	public Node Goto;
        public IGotoStatement GotoStatement;
        public ILabeledStatement Label;

        public int InCount
        {
            get { return InEdges.Count(); }
        }

        public int OutCount
        {
            get { return OutEdges.Count(); }
        }

#if DEBUG
        public int Phase;
#endif
        #endregion

        #region Flags

    	public NodeFlags Flags { get; set; }

    	bool IsFlag(NodeFlags f)
        {
            return (Flags & f) != 0;
        }

        void SetFlag(NodeFlags f, bool value)
        {
            if (value) Flags |= f;
            else Flags &= ~f;
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

    	public int BackInCount { get; set; }

    	public int BackOutCount { get; set; }

    	public bool HasOutBackEdges
        {
            get { return BackOutCount > 0; }
        }

        public bool HasInBackEdges
        {
            get { return BackInCount > 0; }
        }

        public Block SehBegin
        {
            get
            {
                int n = Code.Count;
                if (n <= 0) return null;
                var first = Code[0];
                var block = first.SehBlock;
                if (block == null) return null;
                if (block.EntryIndex != first.Index) return null;
                return block;
            }
        }

        public Block SehEnd
        {
            get
            {
                int n = Code.Count;
                if (n <= 0) return null;
                var last = Code[n - 1];
                var block = last.SehBlock;
                if (block == null) return null;
                if (block.ExitIndex != last.Index) return null;
                return block;
            }
        }

        public bool IsSehBegin
        {
            get { return SehBegin != null; }
        }

        public bool IsSehEnd
        {
            get { return SehEnd != null; }
        }
        #endregion

        #region Edges

    	public Edge FirstIn { get; private set; }

    	public Edge LastIn { get; private set; }

    	public Edge FirstOut { get; private set; }

    	public Edge LastOut { get; private set; }

    	public Node FirstPredecessor
        {
            get { return FirstIn != null ? FirstIn.From : null; }
        }

        public Node FirstSuccessor
        {
            get { return FirstOut != null ? FirstOut.To : null; }
        }

        public Edge FalseEdge
        {
            get
            {
                if (IsTwoWay)
                {
                    var e = FirstOut;
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
                    var e = FirstOut;
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
    	public Node Prev { get; private set; }

    	/// <summary>
    	/// Gets the next node
    	/// </summary>
    	public Node Next { get; private set; }

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
            if (Prev != null)
                Prev.Next = node;
            node.Prev = Prev;
            node.Next = this;
            Prev = node;
        }

        /// <summary>
        /// Appends a given node after this node.
        /// </summary>
        /// <param name="node">the node to append</param>
        public void Append(Node node)
        {
            node.Remove();
            if (Next != null)
                Next.Prev = node;
            node.Prev = this;
            node.Next = Next;
            Next = node;
        }

        /// <summary>
        /// Remove this node from the list.
        /// </summary>
        /// <returns>the next node in the list</returns>
        public Node Remove()
        {
            var prev = Prev;
            var next = Next;

            Prev = null;
            Next = null;

            if (prev != null)
                prev.Next = next;
            if (next != null)
                next.Prev = prev;

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
                for (var e = FirstIn; e != null; e = e.NextIn)
                    yield return e;
            }
        }

    	public IEnumerable<Edge> OutEdges
        {
            get
            {
                for (var e = FirstOut; e != null; e = e.NextOut)
                    yield return e;
            }
        }

    	public IEnumerable<Node> InNodes
        {
            get
            {
                for (var e = FirstIn; e != null; e = e.NextIn)
                    yield return e.From;
            }
        }

    	public IEnumerable<Node> OutNodes
        {
            get
            {
                for (var e = FirstOut; e != null; e = e.NextOut)
                    yield return e.To;
            }
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
            for (var e = FirstIn; e != null; e = e.NextIn)
                if (e.From == node)
                    return e;
            return null;
        }

        public Edge FindOut(Node node)
        {
            for (var e = FirstOut; e != null; e = e.NextOut)
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
            for (var e = FirstIn; e != null; e = e.NextIn)
            {
                ++n;
                if (n >= min) return true;
            }
            return false;
        }

        public bool HasOut(int min)
        {
            int n = 0;
            for (var e = FirstOut; e != null; e = e.NextOut)
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
            if (LastIn == null) FirstIn = e;
            else LastIn.NextIn = e;
            LastIn = e;
            if (e.IsBack) ++BackInCount;
        }

        public void AppendOut(Edge e)
        {
            if (LastOut == null) FirstOut = e;
            else LastOut.NextOut = e;
            LastOut = e;
            if (e.IsBack) ++BackOutCount;
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
            while (FirstIn != null)
            {
                RemoveIn(FirstIn);
            }
        }

        /// <summary>
        /// Removes all out edges
        /// </summary>
        public void RemoveOutEdges()
        {
            while (FirstOut != null)
            {
                RemoveOut(FirstOut);
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
                    FirstIn = next;
                if (next == null)
                    LastIn = prev;

                if (e.IsBack) --BackInCount;

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
                    FirstOut = next;
                if (next == null)
                    LastOut = prev;

                if (e.IsBack) --BackOutCount;

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
            var e = FirstOut;
            FirstOut = LastOut;
            LastOut = e;
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
            get { return _reachableNodes ?? (_reachableNodes = new List<Node>(GetReachableNodesCore())); }
        }
        private List<Node> _reachableNodes;

        IEnumerable<Node> GetReachableNodesCore()
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
            
            Index = -1;
            Remove();

            if (IsEntry)
            {
                RemoveEntry();
                IsEntry = false;
            }
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
            get { return _code.Count > 0 ? _code[_code.Count - 1] : null; }
        }

        public int EntryIndex
        {
            get
            {
                var p = EntryPoint;
                if (p != null)
                    return p.Index;
                return -1;
            }
        }

        public int ExitIndex
        {
            get
            {
                var p = ExitPoint;
                if (p != null)
                    return p.Index;
                return -1;
            }
        }

        public void AddInstruction(Instruction i)
        {
            Code.Add(i);
            i.BasicBlock = this;
        }

#if DEBUG
        public string CodeSpan
        {
            get
            {
				if (_code != null) return _code.ToString();
            	return string.Format("{0}, {1}", EntryPoint, ExitPoint);
            }
        }
#endif

        #endregion

    	public override string ToString()
        {
            return ToString(true);
        }

        public string ToString(bool full)
        {
            return FormatService.ToString(this, FormatOptions.Default);
        }

    	#region State
		private readonly Stack<NodeState> _stateStack = new Stack<NodeState>();

		public void PushState()
		{
			_stateStack.Push(new NodeState());
		}

		public void PopState()
		{
			_stateStack.Pop();
		}
		
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

		public int TranslatedExitIndex
		{
			get { return State.TranslatedExitIndex; }
			set { State.TranslatedExitIndex = value; }
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

        
        #endregion
    }

    #region class NodeState
    internal sealed class NodeState
    {
		public NodeState()
		{
			TranslationIndex = -1;
			TranslatedCode = new List<IInstruction>();
		}

        public bool IsAnalysed { get; set; }

        public bool IsTranslated { get; set; }

    	public int TranslationIndex { get; set; }

    	public List<IInstruction> TranslatedCode { get; private set; }

    	public int TranslatedEntryIndex { get; set; }

    	public int TranslatedExitIndex { get; set; }

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