using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    internal class ILStream : InstructionList<Instruction>
    {
        public int GetTargetIndex(int index)
        {
            if (index < Count)
            {
                var instruction = this[index];
                if (instruction.IsHandlerEnd)
                {
                    for (int i = index - 1; i >= 0; --i)
                    {
                        if (this[i].IsLeave)
                            return GetTargetIndex(i);
                    }
                    return GetTargetIndex(index + 1);
                }
                while (instruction.IsUnconditionalBranch)
                {
                    index = (int)instruction.Value;
                    instruction = this[index];
                }
                return index;
            }
            return -1;
        }

        public Instruction GetTarget(int index)
        {
            index = GetTargetIndex(index);
            return this[index];
        }
    }
}