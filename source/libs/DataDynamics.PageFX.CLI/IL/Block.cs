using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
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

	/// <summary>
	/// Defines SEH block.
	/// </summary>
    internal abstract class Block : ISehBlock
    {
		/// <summary>
		/// Gets or sets the entry point.
		/// </summary>
		public Instruction EntryPoint { get; set; }

		/// <summary>
		/// Gets or sets the exit point.
		/// </summary>
    	public Instruction ExitPoint { get; set; }

		/// <summary>
		/// Gets the index of the entry point.
		/// </summary>
    	public int EntryIndex
        {
            get
            {
                if (EntryPoint != null)
                    return EntryPoint.Index;
                return -1;
            }
        }

		/// <summary>
		/// Gets the index of the exit point.
		/// </summary>
        public int ExitIndex
        {
            get
            {
                if (ExitPoint != null)
                    return ExitPoint.Index;
                return -1;
            }
        }

		/// <summary>
		/// Gets or sets the parent block.
		/// </summary>
		public Block Parent { get; set; }

		/// <summary>
		/// Gets the child blocks.
		/// </summary>
        public IEnumerable<Block> Kids
        {
            get { return _kids; }
        }
        private readonly BlockList _kids = new BlockList();

		/// <summary>
		/// Adds the child block.
		/// </summary>
		/// <param name="kid">The block to add as child.</param>
        public void Add(Block kid)
        {
            kid.Parent = this;
            _kids.Add(kid);
        }

        public abstract BlockType Type
        {
            get;
        }

		/// <summary>
		/// Gets the reference to whole CIL code of the current method.
		/// </summary>
    	public ILStream Code { get; private set; }

		/// <summary>
		/// Gets the instructions.
		/// </summary>
		/// <returns></returns>
    	public IEnumerable<Instruction> GetInstructions()
        {
            for (int i = EntryIndex; i <= ExitIndex; ++i)
                yield return Code[i];
        }

		/// <summary>
		/// Gets the number of inner basic blocks. Used for diag purposes.
		/// </summary>
		public int BasicBlockCount
		{
			get { return GetInstructions().Select(x => x.BasicBlock).Distinct().Count(); }
		}

        public void SetupInstructions(ILStream code)
        {
            Code = code;

        	for (int i = EntryIndex; i <= ExitIndex; ++i)
            {
                var instr = code[i];
                if (instr.SehBlock == null)
                {
                    instr.SehBlock = this;
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

		public IInstruction TranslatedEntryPoint
		{
			get
			{
				if (EntryPoint == null) return null;

				var bb = EntryPoint.BasicBlock;
				return bb != null && bb.TranslatedCode.Count > 0 ? bb.TranslatedCode[0] : null;
			}
		}

		public IInstruction TranslatedExitPoint
		{
			get
			{
				if (ExitPoint == null) return null;

				var bb = ExitPoint.BasicBlock;
				return bb != null && bb.TranslatedCode.Count > 0 ? bb.TranslatedCode[bb.TranslatedCode.Count - 1] : null;
			}
		}

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

		public override string ToString()
		{
			return string.Format("{0}({1})", Type, CodeSpan);
		}
    }

	internal sealed class BlockList : List<Block>
    {
    }

	/// <summary>
	/// Represents try block.
	/// </summary>
    internal sealed class TryCatchBlock : Block, ISehTryBlock
    {
		public TryCatchBlock()
		{
			_handlers.Owner = this;
		}

        public override BlockType Type
        {
            get { return BlockType.Protected; }
        }

        public bool IsTryFinally
        {
            get { return _handlers.Count == 1 && ((HandlerBlock)_handlers[0]).Type == BlockType.Finally; }
        }

        public HandlerCollection Handlers
        {
            get { return _handlers; }
        }
        private readonly HandlerCollection _handlers = new HandlerCollection();

        ISehHandlerCollection ISehTryBlock.Handlers
        {
            get { return _handlers; }
        }

		public override string ToString()
		{
			if (IsTryFinally)
			{
				return string.Format("TryFinally[{0}-{1}]", EntryIndex, ExitIndex);
			}
			return base.ToString();
		}
    }

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

    	public TryCatchBlock Owner { get; set; }

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
            if (instruction.Code == InstructionCode.Endfinally && instruction.Value == null)
            {
                var p = Owner.ExitPoint;
                while (true)
                {
                    var hb = p.SehBlock as HandlerBlock;
                    if (hb == null) break;
                    p = hb.Owner.ExitPoint;
                }

				if (p.IsLeave)
				{
					instruction.Value = p.Value;
				}
				else
				{
					bool isLast = instruction.Index == Code.Count - 1;
					instruction.Value = isLast ? instruction.Index : instruction.Index + 1;
				}
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

	internal sealed class HandlerCollection : ISehHandlerCollection
    {
		private readonly List<HandlerBlock> _list = new List<HandlerBlock>();

    	public TryCatchBlock Owner { get; set; }

    	public void Add(HandlerBlock block)
		{
			block.Owner = Owner;
			_list.Add(block);
		}

    	public int Count
    	{
			get { return _list.Count; }
    	}

    	public ISehHandlerBlock this[int index]
        {
            get { return _list[index]; }
        }

    	public IEnumerator<ISehHandlerBlock> GetEnumerator()
    	{
    		return _list.Cast<ISehHandlerBlock>().GetEnumerator();
    	}

    	IEnumerator IEnumerable.GetEnumerator()
        {
            return _list.GetEnumerator();
        }

    	public int IndexOf(HandlerBlock handlerBlock)
    	{
    		return _list.IndexOf(handlerBlock);
    	}
    }
}