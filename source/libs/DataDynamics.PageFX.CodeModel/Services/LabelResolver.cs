using DataDynamics.PageFX.Common.Statements;

namespace DataDynamics.PageFX.Common.Services
{
    public class LabelResolver
    {
        public static ISwitchCase IsGotoCase(IGotoStatement go)
        {
            var label = go.Label;
            //body of switch case
            var parent = label.ParentStatement;
            if (parent == null) return null;
            var sc = parent.ParentStatement as ISwitchCase;
            if (sc == null) return null;
            if (!ReferenceEquals(sc.Body[0], label)) return null;

            //first parent is collection of switch cases
            var sw = sc.ParentStatement.ParentStatement;

            return HasParent(go, sw) ? sc : null;
        }

        private static bool HasParent(IStatement st, IStatement parent)
        {
            var p = st.ParentStatement;
            while (p != null)
            {
                if (ReferenceEquals(p, parent)) return true;
                p = p.ParentStatement;
            }
            return false;
        }
    }
}