using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public class VariableDeclarationStatement : Statement, IVariableDeclarationStatement, ITypeReferenceProvider
    {
        #region Constructors
        public VariableDeclarationStatement(IVariable v)
        {
            _var = v;
        }
        #endregion

        #region IVariableDeclarationStatement Members
        public IVariable Variable
        {
            get { return _var; }
            set { _var = value; }
        }
        private IVariable _var;
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] {_var}; }
        }
        #endregion

        #region Object Override Members
        public override string ToString()
        {
            return ToString(null, null);
        }

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as IVariableDeclarationStatement;
            if (s == null) return false;
            if (!Equals(s.Variable, _var)) return false;
            return true;
        }

        private static readonly int _hs = typeof(IVariableDeclarationStatement).GetHashCode();

        public override int GetHashCode()
        {
            if (_var != null)
                return _var.GetHashCode() ^ _hs;
            return base.GetHashCode();
        }
        #endregion

        #region ITypeReferenceProvider Members
        public IEnumerable<IType> GetTypeReferences()
        {
            if (_var != null)
                return new[] { _var.Type };
            return null;
        }
        #endregion
    }
}