using System.Collections.Generic;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common
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