using System.Collections;
using System.Collections.Generic;
using DataDynamics.Collections;
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
    	public Instruction EntryPoint { get; set; }

    	public Instruction ExitPoint { get; set; }

    	public int EntryIndex
        {
            get
            {
                if (EntryPoint != null)
                    return EntryPoint.Index;
                return -1;
            }
        }

        public int ExitIndex
        {
            get
            {
                if (ExitPoint != null)
                    return ExitPoint.Index;
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

    	public ILStream Code { get; private set; }

    	public IEnumerable<Instruction> GetInstructions()
        {
            for (int i = EntryIndex; i <= ExitIndex; ++i)
                yield return Code[i];
        }

        public void SetupInstructions(ILStream code)
        {
            Code = code;
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

    	public IInstruction TranslatedEntryPoint { get; set; }

		public IInstruction TranslatedExitPoint { get; set; }

		public object Tag { get; set; }

    	IInstruction ISehBlock.EntryPoint
        {
            get { return TranslatedEntryPoint; }
        }

        IInstruction ISehBlock.ExitPoint
        {
            get { return TranslatedExitPoint; }
        }

    	internal bool Dumped;
    }
    #endregion

    internal sealed class BlockList : List<Block>
    {
    }

    #region ProtectedBlock
	/// <summary>
	/// Represents try block.
	/// </summary>
    internal sealed class ProtectedBlock : Block, ISehTryBlock
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

		ISehHandlerCollection ISehTryBlock.Handlers
        {
            get { return _handlers; }
        }
    }
    #endregion

    #region HandlerBlock
    internal sealed class HandlerBlock : Block, ISehHandlerBlock
    {
        public HandlerBlock(BlockType type)
        {
            _type = type;
        }

        public override BlockType Type
        {
            get { return _type; }
        }
        private readonly BlockType _type;

    	public ProtectedBlock Owner { get; set; }

    	public int Index
        {
            get
            {
                if (Owner == null) return -1;
                if (_index < 0)
                    _index = Owner.Handlers.IndexOf(this);
                return _index;
            }
        }
        private int _index = -1;

        protected override void VisitInstruction(Instruction instruction)
        {
            //FIX:
            //Fix for endfinally, endfilter instructions,
            //in order to avoid promblems with their translation
            if (instruction.Code == InstructionCode.Endfinally)
            {
                var p = Owner.ExitPoint;
                while (true)
                {
                    var hb = p.Block as HandlerBlock;
                    if (hb == null) break;
                    p = hb.Owner.ExitPoint;
                }
                instruction.Value = p.Value;
            }
        }

        public int FilterIndex;

    	ISehTryBlock ISehHandlerBlock.Owner
        {
            get { return Owner; }
        }

        ISehHandlerBlock ISehHandlerBlock.PrevHandler
        {
            get
            {
                if (Owner == null) return null;
                int index = Index;
                if (index > 0)
                    return Owner.Handlers[index - 1];
                return null;
            }
        }

        ISehHandlerBlock ISehHandlerBlock.NextHandler
        {
            get
            {
                if (Owner == null) return null;
                int index = Index + 1;
                if (index < Owner.Handlers.Count)
                    return Owner.Handlers[index];
                return null;
            }
        }

        public IType ExceptionType { get; set; }

        public int ExceptionVariable { get; set; }

        public IType GenericExceptionType { get; set; }
    }
    #endregion

    internal sealed class HandlerBlockList : List<HandlerBlock>, ISehHandlerCollection
    {
    	ISehHandlerBlock ISimpleList<ISehHandlerBlock>.this[int index]
        {
            get { return this[index]; }
        }

    	IEnumerator<ISehHandlerBlock> IEnumerable<ISehHandlerBlock>.GetEnumerator()
        {
        	foreach (var block in this)
        	{
        		yield return block;
        	}
        }

    	IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}