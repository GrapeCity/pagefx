using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.FLI
{
    internal interface IInstructionSubject
    {
        void Apply(IInstruction instruction);
    }

    internal class AvmResolver
    {
        private readonly List<Pair<IInstruction, IInstructionSubject>> _list = new List<Pair<IInstruction, IInstructionSubject>>();

        public void Add(IInstruction instruction, IInstructionSubject subject)
        {
            _list.Add(new Pair<IInstruction, IInstructionSubject>(instruction, subject));
        }

        public void Resolve()
        {
            foreach (var p in _list)
            {
                p.Second.Apply(p.First);
            }
        }
    }
}