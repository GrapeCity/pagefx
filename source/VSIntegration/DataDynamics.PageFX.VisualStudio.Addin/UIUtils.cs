using System;
using Microsoft.VisualStudio.CommandBars;

namespace DataDynamics.PageFX.VisualStudio
{
    static class UIUtils
    {
        public static void RemoveControls(CommandBar commandBar, string text)
        {
            while (true)
            {
                try
                {
                    var ctl = commandBar.Controls[text];
                    ctl.Delete(false);
                }
                catch (ArgumentException)
                {
                    break;
                }
            }
        }

        public static void RemoveControls(CommandBarPopup container, string text)
        {
            while (true)
            {
                try
                {
                    var ctl = container.Controls[text];
                    ctl.Delete(false);
                }
                catch (ArgumentException)
                {
                    break;
                }
            }
        }

        public static CommandBarControl FindControl(CommandBar parent, string name)
        {
            try
            {
                var child = parent.Controls[name];
                return child;
            }
            catch
            {
                return null;
            }
        }

        public static CommandBarPopup CreatePopup(CommandBar parent, string name, int pos)
        {
            try
            {
                var m = parent.Controls[name];
                return (CommandBarPopup)m;
            }
            catch
            {
            }

            var p = parent.Controls.Add(MsoControlType.msoControlPopup, 1, "", pos, false);
            p.Caption = name;
            p.Visible = true;
            return (CommandBarPopup)p;
        }

        public static CommandBarButton CreateButton(CommandBarPopup menu, string caption, string icon, int pos)
        {
            var button = (CommandBarButton)menu.Controls.Add(MsoControlType.msoControlButton, 1, "", pos, false);
            button.Caption = caption;
            IconUtils.SetPicture(button, icon);
            return button;
        }
    }
}