using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;

namespace DataDynamics.PageFX.Ecma335.IL
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
}