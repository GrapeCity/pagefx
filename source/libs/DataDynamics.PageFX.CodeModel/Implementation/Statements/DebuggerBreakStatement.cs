namespace DataDynamics.PageFX.CodeModel
{
    public class DebuggerBreakStatement : Statement, IDebuggerBreakStatement
    {
        #region Object Override Members
        public override bool Equals(object obj)
        {
            return obj is IDebuggerBreakStatement;
        }

        private static readonly int _hs = "DebuggerBreakStatement".GetHashCode();

        public override int GetHashCode()
        {
            return _hs;
        }
        #endregion
    }
}