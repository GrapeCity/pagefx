using System;

namespace DataDynamics.PageFX.CodeModel
{
    public class LabelResolver
    {
        private readonly IStatementCollection _body;
        private ILabeledStatement _labelBegin;
        private int _label;

        public LabelResolver(IStatementCollection body, int lastLabelNumber)
        {
            _body = body;
            _label = lastLabelNumber;
        }

        public void Run()
        {
            if (_body.Count > 0)
            {
                //foreach (IStatement st in new List<IStatement>(_body))
                //{
                //    Resolve(st);
                //}
            }
        }

        private void Resolve(IStatement st)
        {
            var go = st as IGotoStatement;
            if (go != null)
            {
                Resolve(go);
            }

            var kids = st.ChildNodes;
            if (kids != null)
            {
                foreach (var node in kids)
                {
                    st = node as IStatement;
                    if (st != null)
                    {
                        Resolve(st);
                    }
                }
            }
        }

        private void Resolve(IGotoStatement go)
        {
            var label = go.Label;
            if (label == null)
                throw new InvalidOperationException();

            if (IsVisible(go, label))
                return;

            EnshureBeginLabel();

            var go2 = new GotoStatement(go.Label);
            var l = Label(go2);
            go.Label = l;
            _body.Insert(1, l);
        }

        private void EnshureBeginLabel()
        {
            if (_labelBegin != null) return;
            _labelBegin = _body[0] as ILabeledStatement;
            if (_labelBegin != null) return;
            _labelBegin = Label(_body[0]);
            _body[0] = _labelBegin;
            var go = new GotoStatement(_labelBegin);
            _body.Insert(0, go);
        }

        private ILabeledStatement Label(IStatement st)
        {
            if (st == null)
                throw new ArgumentNullException();
            ++_label;
            return new LabeledStatement("L" + _label, st);
        }

        private static bool IsVisible(IGotoStatement go, ILabeledStatement label)
        {
            var parent = go.ParentStatement;
            
            //if label is descendant of goto parent
            if (HasParent(label, parent))
                return true;

            //label is child of method body
            parent = label.ParentStatement;
            if (parent.ParentStatement == null) 
                return true;

            //goto case
            var sc = parent.ParentStatement as ISwitchCase;
            if (sc != null && sc.Body.Count > 0 && sc.Body[0] == label)
            {
                //first parent is collection of switch cases
                var sw = sc.ParentStatement.ParentStatement;
                if (HasParent(go, sw))
                    return true;
            }

            return false;
        }

        public static ISwitchCase IsGotoCase(IGotoStatement go)
        {
            var label = go.Label;
            //body of switch case
            var parent = label.ParentStatement;
            if (parent == null) return null;
            var sc = parent.ParentStatement as ISwitchCase;
            if (sc == null) return null;
            if (sc.Body[0] != label) return null;

            //first parent is collection of switch cases
            var sw = sc.ParentStatement.ParentStatement;

            if (HasParent(go, sw))
                return sc;

            return null;
        }

        public static bool HasParent(IStatement st, IStatement parent)
        {
            var p = st.ParentStatement;
            while (p != null)
            {
                if (p == parent)
                    return true;
                p = p.ParentStatement;
            }
            return false;
        }
    }
}