using System;

namespace DataDynamics.PageFX.VisualStudio
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        public string Name;
        public string Caption;
        public string Icon;
        public int Position = 1;

        //TODO: Format: 
        public string OwnerMenu;
        
        public CommandAttribute(string name, string caption)
        {
            Name = name;
            Caption = caption;
        }

        public string Tooltip
        {
            get 
            {
                if (_tooltip == null)
                    return Caption;
                return _tooltip;
            }
            set { _tooltip = value; }
        }
        string _tooltip;

        public string HotKey;
    }
}