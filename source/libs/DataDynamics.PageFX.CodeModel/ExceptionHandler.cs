using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class ExceptionHandler : IExceptionHandler
    {
        #region IExceptionHandler Members

    	public ExceptionHandlerType Type { get; set; }

    	public IType CatchType { get; set; }

    	public int FilterOffset { get; set; }

    	public int HandlerLength { get; set; }

    	public int HandlerOffset { get; set; }

    	public int TryLength { get; set; }

    	public int TryOffset { get; set; }

    	#endregion
    }

    public class ExceptionHandlerCollection : List<IExceptionHandler>, IExceptionHandlerCollection
    {
    }
}