using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel.TypeSystem;

namespace DataDynamics.PageFX.CodeModel
{
    public interface ISehBlock
    {
    	object Tag { get; set; }

        IInstruction EntryPoint { get; }

        IInstruction ExitPoint { get; }
    }

    public interface ISehTryBlock : ISehBlock
    {
        ISehHandlerCollection Handlers { get; }
    }

    public interface ISehHandlerBlock : ISehBlock
    {
        ISehTryBlock Owner { get; }

        ISehHandlerBlock PrevHandler { get; }

        ISehHandlerBlock NextHandler { get; }

        IType ExceptionType { get; }

        int ExceptionVariable { get; }
    }

    public interface ISehHandlerCollection : IReadOnlyList<ISehHandlerBlock>
    {
    }
}