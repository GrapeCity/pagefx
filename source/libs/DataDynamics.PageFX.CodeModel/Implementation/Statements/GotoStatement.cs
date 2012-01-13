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
            _label = label;
        }
        #endregion

        #region IGotoStatement Members
        public ILabeledStatement Label
        {
            get { return _label; }
            set { _label = value; }
        }
        private ILabeledStatement _label;
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var go = obj as IGotoStatement;
            if (go == null) return false;
            if (!Equals(go.Label, _label)) return false;
            return true;
        }

        private static readonly int _hs = typeof(IGotoStatement).GetHashCode();

        public override int GetHashCode()
        {
            int h = _hs;
            if (_label != null)
                h ^= _label.GetHashCode();
            return h;
        }
        #endregion
    }

    public class BreakStatement : Statement, IBreakStatement
    {
        #region IGotoStatement Members
        public ILabeledStatement Label
        {
            get { return _label; }
            set { _label = value; }
        }
        private ILabeledStatement _label;
        #endregion
    }

    public class ContinueStatement : Statement, IContinueStatement
    {
        #region IGotoStatement Members
        public ILabeledStatement Label
        {
            get { return _label; }
            set { _label = value; }
        }
        private ILabeledStatement _label;
        #endregion
    }
}