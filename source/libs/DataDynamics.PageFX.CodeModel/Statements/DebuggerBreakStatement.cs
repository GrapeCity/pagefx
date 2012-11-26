namespace DataDynamics.PageFX.CodeModel
{
    public class DebuggerBreakStatement : Statement, IDebuggerBreakStatement
    {
    	public override bool Equals(object obj)
        {
            return obj is IDebuggerBreakStatement;
        }

        private static readonly int HashSalt = "DebuggerBreakStatement".GetHashCode();

        public override int GetHashCode()
        {
            return HashSalt;
        }
    }
}