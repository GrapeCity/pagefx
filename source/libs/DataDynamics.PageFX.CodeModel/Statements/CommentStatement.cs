using System;

namespace DataDynamics.PageFX.Common.Statements
{
    public class CommentStatement : Statement, ICommentStatement
    {
    	public CommentStatement(string comment)
        {
            Comment = comment;
        }

    	public string Comment { get; set; }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as ICommentStatement;
            if (s == null) return false;
            return s.Comment == Comment;
        }

    	private static readonly int HashSalt = typeof(CommentStatement).GetHashCode();

        public override int GetHashCode()
        {
        	int h = HashSalt;
            if (Comment != null)
                h ^= Comment.GetHashCode();
            return h;
        }
    }

    public sealed class ErrorStatement : CommentStatement
    {
        public ErrorStatement(Exception e)
            : base(e.ToString())
        {
            Exception = e;
        }

    	public Exception Exception { get; private set; }
    }
}