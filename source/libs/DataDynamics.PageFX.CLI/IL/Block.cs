using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using DataDynamics.Collections;
using DataDynamics.PageFX.CLI.CFG;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    internal enum BlockType
    {
        Protected,
        Catch,
        Finally,
        Fault,
        Filter,
    }

    #region Block
    internal abstract class Block : ISehBlock
    {
        public Node Node { get; set; }

        public Instruction EntryPoint
        {
            get { return _entryPoint; }
            set { _entryPoint = value; }
        }
        private Instruction _entryPoint;

        public Instruction ExitPoint
        {
            get { return _exitPoint; }
            set { _exitPoint = value; }
        }
        private Instruction _exitPoint;

        public int EntryIndex
        {
            get
            {
                if (_entryPoint != null)
                    return _entryPoint.Index;
                return -1;
            }
        }

        public int ExitIndex
        {
            get
            {
                if (_exitPoint != null)
                    return _exitPoint.Index;
                return -1;
            }
        }

        public Block Parent { get; set; }

        public BlockList Kids
        {
            get { return _kids; }
        }
        private readonly BlockList _kids = new BlockList();

        public void Add(Block kid)
        {
            kid.Parent = this;
            _kids.Add(kid);
        }

        public abstract BlockType Type
        {
            get;
        }

        public bool IsHandler
        {
            get
            {
                switch(Type)
                {
                    case BlockType.Catch:
                    case BlockType.Filter:
                    case BlockType.Finally:
                    case BlockType.Fault:
                        return true;
                }
                return false;
            }
        }

        public ILStream Code
        {
            get { return _code; }
        }
        private ILStream _code;

        public IEnumerable<Instruction> GetInstructions()
        {
            for (int i = EntryIndex; i <= ExitIndex; ++i)
                yield return _code[i];
        }

        public void SetupInstructions(ILStream code)
        {
            _code = code;
            int exit = ExitIndex;
            for (int i = EntryIndex; i <= exit; ++i)
            {
                var instr = code[i];
                if (instr.Block == null)
                {
                    instr.Block = this;
                    VisitInstruction(instr);
                }
            }
        }

        protected virtual void VisitInstruction(Instruction instruction)
        {
        }

        public string CodeSpan
        {
            get
            {
                if (EntryIndex == ExitIndex)
                {
                    return EntryIndex.ToString();
                }
                return string.Format("{0} : {1}", EntryIndex, ExitIndex);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}({1})", Type, CodeSpan);
        }

        //static Node GetNode(Instruction instruction)
        //{
        //    Node node;
        //    var block = instruction.Block;
        //    if (block != null && block.Node != null)
        //    {
        //        node = block.Node;
        //    }
        //    else
        //    {
        //        node = instruction.OwnerNode;
        //    }
        //    if (node != null)
        //    {
        //        while (node.Parent != null)
        //            node = node.Parent;
        //    }
        //    return node;
        //}

        //public Node GetEntryNode()
        //{
        //    foreach (var instruction in GetInstructions())
        //    {
        //        var first = _code.GetTarget(instruction.Index);
        //        var node = GetNode(first);
        //        if (node != null)
        //        {
        //            return node;
        //        }
        //    }
        //    return null;
        //}

        //public virtual Node GetExitNode()
        //{
        //    for (int i = ExitIndex; i >= EntryIndex; --i)
        //    {
        //        var instruction = Code[i];
        //        if (instruction.Block == this)
        //        {
        //            var c = instruction.Code;
        //            if (c == InstructionCode.Leave_S || c == InstructionCode.Leave)
        //            {
        //                instruction = Code.GetTarget(i);
        //                return GetNode(instruction);
        //            }
        //        }
        //    }
        //    return null;
        //}
        
        //public NodeList GetNodes()
        //{
        //    var list = new NodeList();
        //    var inList = new Hashtable();
        //    Node prev = null;
        //    foreach (var instruction in GetInstructions())
        //    {
        //        var node = GetNode(instruction);
        //        if (node != null)
        //        {
        //            if (node != prev)
        //            {
        //                prev = node;
        //                if (inList[node] == null)
        //                {
        //                    inList[node] = node;
        //                    list.Add(node);
        //                }
        //            }
        //        }
        //    }
        //    return list;
        //}

        public IInstruction TranslatedEntryPoint;
        public IInstruction TranslatedExitPoint;

        #region ISehBlock Members
        IInstruction ISehBlock.EntryPoint
        {
            get { return TranslatedEntryPoint; }
        }

        IInstruction ISehBlock.ExitPoint
        {
            get { return TranslatedExitPoint; }
        }
        #endregion

        internal bool Dumped;
    }
    #endregion

    internal class BlockList : List<Block>
    {
    }

    #region ProtectedBlock
    internal class ProtectedBlock : Block, ISehTryBlock
    {
        public override BlockType Type
        {
            get { return BlockType.Protected; }
        }

        public bool IsTryFinally
        {
            get
            {
                return _handlers.Count == 1
                       && _handlers[0].Type == BlockType.Finally;
            }
        }

        public new TryNode Node
        {
            get { return (TryNode)base.Node; }
            set { base.Node = value; }
        }

        public HandlerBlockList Handlers
        {
            get { return _handlers; }
        }
        private readonly HandlerBlockList _handlers = new HandlerBlockList();

        public void AddHandler(HandlerBlock h)
        {
            h.Owner = this;
            _handlers.Add(h);
        }

        #region ISehTryBlock Members
        ISehHandlerCollection ISehTryBlock.Handlers
        {
            get { return _handlers; }
        }

        public int Depth
        {
            get
            {
                if (_depth < 0)
                {
                    _depth = 0;
                    var parent = Parent;
                    while (parent != null)
                    {
                        ++_depth;
                        parent = parent.Parent;
                    }
                }
                return _depth;
            }
        }
        private int _depth = -1;
        #endregion
    }
    #endregion

    #region HandlerBlock
    internal class HandlerBlock : Block, ISehHandlerBlock
    {
        public HandlerBlock(BlockType type)
        {
            _type = type;
        }

        public override BlockType Type
        {
            get { return _type; }
        }
        readonly BlockType _type;

        public ProtectedBlock Owner
        {
            get { return _owner; }
            set { _owner = value; }
        }
        private ProtectedBlock _owner;

        public int Index
        {
            get
            {
                if (_owner == null) return -1;
                if (_index < 0)
                    _index = Algorithms.IndexOf<HandlerBlock>(Owner.Handlers, this);
                return _index;
            }
        }
        int _index = -1;

        protected override void VisitInstruction(Instruction instruction)
        {
            //FIX:
            //Fix for endfinally, endfilter instructions,
            //in order to avoid promblems with their translation
            if (instruction.Code == InstructionCode.Endfinally)
            {
                var p = _owner.ExitPoint;
                while (true)
                {
                    var hb = p.Block as HandlerBlock;
                    if (hb == null) break;
                    p = hb.Owner.ExitPoint;
                }
                instruction.Value = p.Value;
            }
        }

        //public override Node GetExitNode()
        //{
        //    return Owner.GetExitNode();
        //}

        public int FilterIndex;

        #region ISehHandlerBlock Members
        public object Tag { get; set; }

        ISehTryBlock ISehHandlerBlock.Owner
        {
            get { return _owner; }
        }

        ISehHandlerBlock ISehHandlerBlock.PrevHandler
        {
            get
            {
                if (_owner == null) return null;
                int index = Index;
                if (index > 0)
                    return _owner.Handlers[index - 1];
                return null;
            }
        }

        ISehHandlerBlock ISehHandlerBlock.NextHandler
        {
            get
            {
                if (_owner == null) return null;
                int index = Index + 1;
                if (index < _owner.Handlers.Count)
                    return _owner.Handlers[index];
                return null;
            }
        }

        public IType ExceptionType { get; set; }

        public int ExceptionVariable { get; set; }

        public IType GenericExceptionType { get; set; }
        #endregion
    }
    #endregion

    internal class HandlerBlockList : List<HandlerBlock>, ISehHandlerCollection
    {
        #region ISimpleList<ISehHandlerBlock> Members
        ISehHandlerBlock ISimpleList<ISehHandlerBlock>.this[int index]
        {
            get { return this[index]; }
        }
        #endregion

        #region IEnumerable<ISehHandlerBlock> Members
        IEnumerator<ISehHandlerBlock> IEnumerable<ISehHandlerBlock>.GetEnumerator()
        {
            return new BaseTypeEnumerator<HandlerBlock, ISehHandlerBlock>(this);
        }
        #endregion

        #region IEnumerable Members
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion
    }
}