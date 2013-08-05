using System;
using System.Collections.Generic;
using DataDynamics.PageFX.Common.CodeModel;

namespace DataDynamics.PageFX.Flash.Core
{
    internal interface IInstructionSubject
    {
        void Apply(IInstruction instruction);
    }

    internal sealed class AvmResolver
    {
		private struct Point
		{
			public IInstruction Instruction;
			public IInstructionSubject Subject;
		}

		private struct Block
		{
			public ISehTryBlock Try;
			public IInstructionSubject From;
			public IInstructionSubject To;
		}

		private readonly List<Point> _points = new List<Point>();
		private readonly List<Block> _blocks = new List<Block>();

		public void Add(ISehTryBlock tryBlock, IInstructionSubject from, IInstructionSubject to)
		{
			if (tryBlock == null) throw new ArgumentNullException("tryBlock");
			if (from == null && to == null) throw new ArgumentNullException("from");
			
			_blocks.Add(new Block
			            	{
			            		Try = tryBlock,
			            		From = from,
			            		To = to
			            	});
		}

    	public void Add(IInstruction instruction, IInstructionSubject subject)
        {
        	if (instruction == null) throw new ArgumentNullException("instruction");
        	if (subject == null) throw new ArgumentNullException("subject");

        	_points.Add(new Point
        	          	{
        	          		Instruction = instruction,
        	          		Subject = subject
        	          	});
        }

    	public void Resolve()
        {
            foreach (var p in _points)
            {
                p.Subject.Apply(p.Instruction);
            }

    		foreach (var block in _blocks)
    		{
				if (block.From != null)
				{
					block.From.Apply(block.Try.EntryPoint);
				}
				if (block.To != null)
				{
					block.To.Apply(block.Try.ExitPoint);
				}
    		}
        }
    }
}