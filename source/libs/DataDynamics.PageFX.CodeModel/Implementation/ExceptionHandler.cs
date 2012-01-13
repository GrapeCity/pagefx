using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class ExceptionHandler : IExceptionHandler
    {
        #region IExceptionHandler Members
        public ExceptionHandlerType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        private ExceptionHandlerType _type;

        public IType CatchType
        {
            get { return _catchType; }
            set { _catchType = value; }
        }
        private IType _catchType;

        public int FilterOffset
        {
            get { return _filterOffset; }
            set { _filterOffset = value; }
        }
        private int _filterOffset;

        public int HandlerLength
        {
            get { return _handlerLength; }
            set { _handlerLength = value; }
        }
        private int _handlerLength;

        public int HandlerOffset
        {
            get { return _handlerOffset; }
            set { _handlerOffset = value; }
        }
        private int _handlerOffset;

        public int TryLength
        {
            get { return _tryLength; }
            set { _tryLength = value; }
        }
        private int _tryLength;

        public int TryOffset
        {
            get { return _tryOffset; }
            set { _tryOffset = value; }
        }
        private int _tryOffset;
        #endregion
    }

    public class ExceptionHandlerCollection : List<IExceptionHandler>, IExceptionHandlerCollection
    {
    }
}