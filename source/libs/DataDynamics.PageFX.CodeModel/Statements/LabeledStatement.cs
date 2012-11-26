using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class LabeledStatement : Statement, ILabeledStatement
    {
    	public LabeledStatement(string name, IStatement statement)
        {
            Name = name;
            Statement = statement;
        }

    	public string Name { get; set; }

    	public IStatement Statement { get; set; }

    	public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] {Statement}; }
        }

    	public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var s = obj as ILabeledStatement;
            if (s == null) return false;
            if (!Equals(s.Statement, Statement)) return false;
            if (s.Name != Name) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int h = 0;
            if (Name != null)
                h ^= Name.GetHashCode();
            if (Statement != null)
                h ^= Statement.GetHashCode();
            return h;
        }
    }
}