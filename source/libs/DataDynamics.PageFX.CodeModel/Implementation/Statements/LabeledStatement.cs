using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class LabeledStatement : Statement, ILabeledStatement
    {
        #region Constructors
        public LabeledStatement(string name, IStatement statement)
        {
            _name = name;
            _statement = statement;
        }
        #endregion

        #region ILabeledStatement Members
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _name;

        public IStatement Statement
        {
            get { return _statement; }
            set { _statement = value; }
        }
        private IStatement _statement;
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return CMHelper.Enumerate(_statement); }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as ILabeledStatement;
            if (s == null) return false;
            if (!Equals(s.Statement, _statement)) return false;
            if (s.Name != _name) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int h = 0;
            if (_name != null)
                h ^= _name.GetHashCode();
            if (_statement != null)
                h ^= _statement.GetHashCode();
            return h;
        }
        #endregion
    }
}