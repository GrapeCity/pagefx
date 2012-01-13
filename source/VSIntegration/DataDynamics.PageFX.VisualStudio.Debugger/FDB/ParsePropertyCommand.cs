using System.Collections.Generic;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    class ParsePropertyCommand : Command
    {
        protected readonly StackFrame m_frame;

        public ParsePropertyCommand(StackFrame frame, string text)
            : base(text)
        {
            m_frame = frame;
        }

        public ParsePropertyCommand(StackFrame frame, string format, params object[] args)
            : base(format, args)
        {
            m_frame = frame;
        }

        public override bool IsMulti
        {
            get { return true; }
        }

        Property m_prop;

        //public readonly List<Property> ComplexProperties = new List<Property>();
        public readonly List<Property> SpecProperties = new List<Property>();
        public readonly List<Property> Properties = new List<Property>();

        void Add()
        {
            var prop = m_prop;
            m_prop = null;
            prop.Frame = m_frame;
            //if (prop.IsComplex)
            //    ComplexProperties.Add(prop);
            if (prop.IsSpecial)
                SpecProperties.Add(prop);
            else
                Properties.Add(prop);
        }

        public override void Handler(string s)
        {
            if (m_prop != null && m_prop.IsMultiLine)
            {
                if (string.IsNullOrEmpty(s))
                {
                    m_prop.Value += "\n";
                    return;
                }

                if (s[s.Length - 1] == '\"')
                {
                    m_prop.Value += "\n";
                    m_prop.Value += s.Trim('\"');
                    Add();
                    return;
                }

                m_prop.Value += "\n";
                m_prop.Value += s;
                return;
            }

            m_prop = Property.Parse(s);
            if (m_prop == null) return;
            if (!m_prop.IsMultiLine)
                Add();
        }
    }

    class PrintCommand : ParsePropertyCommand
    {
        public PrintCommand(StackFrame frame, string exp)
            : base(frame, "print {0}", exp)
        {
        }
    }
}