namespace DataDynamics.PageFX.CodeModel
{
    public class GotoStatement : Statement, IGotoStatement
    {
        #region Constructors
        public GotoStatement()
        {
        }

        public GotoStatement(ILabeledStatement label)
        {
            Label = label;
        }
        #endregion

        #region IGotoStatement Members

    	public ILabeledStatement Label { get; set; }

    	#endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var go = obj as IGotoStatement;
            if (go == null) return false;
            if (!Equals(go.Label, Label)) return false;
            return true;
        }

        private static readonly int _hs = typeof(IGotoStatement).GetHashCode();

        public override int GetHashCode()
        {
            int h = _hs;
            if (Label != null)
                h ^= Label.GetHashCode();
            return h;
        }
        #endregion
    }

    public class BreakStatement : Statement, IBreakStatement
    {
        #region IGotoStatement Members

    	public ILabeledStatement Label { get; set; }

    	#endregion
    }

    public class ContinueStatement : Statement, IContinueStatement
    {
        #region IGotoStatement Members

    	public ILabeledStatement Label { get; set; }

    	#endregion
    }
}