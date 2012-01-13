using DataDynamics.Collections;

namespace DataDynamics.PageFX.CodeModel
{
    public interface ISehBlock
    {
        bool IsHandler { get; }

        IInstruction EntryPoint { get; }

        IInstruction ExitPoint { get; }
    }

    public interface ISehTryBlock : ISehBlock
    {
        ISehHandlerCollection Handlers { get; }
        int Depth { get; }
    }

    public interface ISehHandlerBlock : ISehBlock
    {
        object Tag { get; set; }

        ISehTryBlock Owner { get; }

        ISehHandlerBlock PrevHandler { get; }

        ISehHandlerBlock NextHandler { get; }

        IType ExceptionType { get; }

        int ExceptionVariable { get; }
    }

    public interface ISehHandlerCollection : ISimpleList<ISehHandlerBlock>
    {
    }
}