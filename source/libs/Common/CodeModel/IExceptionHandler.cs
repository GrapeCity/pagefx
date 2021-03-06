using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common.CodeModel
{
    public interface IExceptionHandler
    {
        ExceptionHandlerType Type { get; set; }
        IType CatchType { get; set; }

        int FilterOffset { get; set; }

        int HandlerLength { get; set; }
        int HandlerOffset { get; set; }

        int TryLength { get; set; }
        int TryOffset { get; set; }
    }

    public interface IExceptionHandlerCollection : IReadOnlyList<IExceptionHandler>
    {
    }

	public enum ExceptionHandlerType
	{
		Finally,
		Catch,
		Filter,
		Fault
	}
}