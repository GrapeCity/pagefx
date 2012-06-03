using System;
using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.FLI
{
    internal interface IInstructionSubject
    {
        void Apply(IInstruction instruction);
    }

    internal sealed class AvmResolver
    {
		private readonly List<KeyValuePair<IInstruction, IInstructionSubject>> _list = new List<KeyValuePair<IInstruction, IInstructionSubject>>();

        public void Add(IInstruction instruction, IInstructionSubject subject)
        {
        	if (instruction == null) throw new ArgumentNullException("instruction");
        	if (subject == null) throw new ArgumentNullException("subject");

			_list.Add(new KeyValuePair<IInstruction, IInstructionSubject>(instruction, subject));
        }

    	public void Resolve()
        {
            foreach (var p in _list)
            {
                p.Value.Apply(p.Key);
            }
        }
    }
}