using System;

namespace DataDynamics.PageFX.CodeModel
{
    public class CommentStatement : Statement, ICommentStatement
    {
        #region Constructors
        public CommentStatement(string comment)
        {
            _comment = comment;
        }
        #endregion

        #region ICommentStatement Members
        public string Comment
        {
            get { return _comment; }
            set { _comment = value; }
        }
        private string _comment;
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as ICommentStatement;
            if (s == null) return false;
            if (s.Comment != _comment) return false;
            return true;
        }

        public override int GetHashCode()
        {
            if (_comment != null)
                return _comment.GetHashCode();
            return base.GetHashCode();
        }
        #endregion
    }

    public class ErrorStatement : CommentStatement
    {
        public ErrorStatement(Exception e)
            : base(e.ToString())
        {
            _exception = e;
        }

        public Exception Exception
        {
            get { return _exception; }
        }
        private readonly Exception _exception;
    }
}