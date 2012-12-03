namespace DataDynamics.PageFX.Common.CodeModel.Statements
{
    public class GotoStatement : Statement, IGotoStatement
    {
    	public GotoStatement()
        {
        }

        public GotoStatement(ILabeledStatement label)
        {
            Label = label;
        }

    	public ILabeledStatement Label { get; set; }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var go = obj as IGotoStatement;
            if (go == null) return false;
            if (!Equals(go.Label, Label)) return false;
            return true;
        }

        private static readonly int HashSalt = typeof(IGotoStatement).GetHashCode();

        public override int GetHashCode()
        {
            int h = HashSalt;
            if (Label != null)
                h ^= Label.GetHashCode();
            return h;
        }
    }

    public class BreakStatement : Statement, IBreakStatement
    {
    	public ILabeledStatement Label { get; set; }
    }

    public class ContinueStatement : Statement, IContinueStatement
    {
    	public ILabeledStatement Label { get; set; }
    }
}