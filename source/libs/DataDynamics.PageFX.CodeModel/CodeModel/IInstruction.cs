using System;
using System.Collections.Generic;
using System.Linq;

namespace DataDynamics.PageFX.Common.CodeModel
{
    //Instruction Format String
    //I     - index
    //IX    - hex index
    //O     - offset
    //OX    - hex offset
    //C     - decimal code
    //CX    - hex code
    //N     - name
    //V     - value

    public interface IInstruction : IFormattable
    {
        /// <summary>
        /// Gets instruction code
        /// </summary>
        int Code { get; }

        /// <summary>
        /// Gets or sets instruction index whithin instruction stream.
        /// </summary>
        int Index { get; set; }

        /// <summary>
        /// Gets or sets instruction offset from the method begin
        /// </summary>
        int Offset { get; set; }

        /// <summary>
        /// Gets instruction operand.
        /// </summary>
        object Value { get; }

        /// <summary>
        /// Gets or sets flag indicating whether there are branches to this instruction.
        /// </summary>
        bool IsBranchTarget { get; set; }

        /// <summary>
        /// Gets the boolean value indicating whether this instruction is used to exit from method.
        /// </summary>
        bool IsReturn { get; }

        /// <summary>
        /// Gets the boolean value indicating whether this instruction is used to throw exception.
        /// </summary>
        bool IsThrow { get; }

        /// <summary>
        /// Gets the boolean value indicating whether this instruction is used to perform conditional branch.
        /// </summary>
        bool IsConditionalBranch { get; }

        /// <summary>
        /// Gets the boolean value indicating whether this instruction is used to perform unconditional branch.
        /// </summary>
        bool IsUnconditionalBranch { get; }

        /// <summary>
        /// Gets the boolean value indicating whether this instruction is used to perform conditional/unconditional branch.
        /// </summary>
        bool IsBranch { get; }

        /// <summary>
        /// Gets the boolean value indicating whether this instruction is used to perform switch.
        /// </summary>
        bool IsSwitch { get; }

        void TranslateOffsets(IInstructionList list);

        int MetadataToken { get; set; }

	    IInstruction Clone();
    }

    public interface IInstructionList : IList<IInstruction>
    {
        int GetOffsetIndex(int offset);

        IInstruction FindByOffset(int offset);

        void TranslateOffsets();
    }

	public static class InstructionExtensions
	{
		public static IEnumerable<IInstruction> Clone(this IEnumerable<IInstruction> seq)
		{
			return seq.Select(i => i.Clone());
		}
	}
}