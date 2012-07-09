using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Linq;
using DataDynamics;

namespace abc
{
    static class Utils
    {
        public static XmlWriter CreateXmlWriter(string path)
        {
            var xws = new XmlWriterSettings
                      	{
                      		Indent = true,
                      		IndentChars = "  ",
                      		Encoding = Encoding.UTF8
                      	};
            return XmlWriter.Create(path, xws);
        }

        public static string GetExt(string path, string defext)
        {
            if (string.IsNullOrEmpty(path)) return defext;
            string ext = Path.GetExtension(path);
            if (string.IsNullOrEmpty(ext)) return defext;
            if (ext[0] == '.')
                ext = ext.Substring(1);
            return ext.ToLower();
        }

        public static string GetExt(string path)
        {
            return GetExt(path, "");
        }

        public static string ResolvePath(string path)
        {
            if (Path.IsPathRooted(path))
                return path;

            string curdir = Environment.CurrentDirectory;
            string fullpath = Path.Combine(curdir, path);
            if (File.Exists(fullpath))
                return fullpath;

            return path;
        }

        public static void BrowseFile(Control tb, string filter)
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = filter;
                if (dlg.ShowDialog() == DialogResult.OK)
                    tb.Text = dlg.FileName;
            }
        }

        #region Form State
        #region Save
        public static void SaveState(Control root)
        {
            SaveState(root, GetFileName(root));
        }

        public static void SaveState(Control root, string fname)
        {
            string path = GetStatePath(fname, true);

        	var xws = new XmlWriterSettings {Indent = true, IndentChars = "  "};
        	using (var writer = XmlWriter.Create(path))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("root");
                SaveKids(writer, root);
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        private static void SaveKids(XmlWriter writer, Control parent)
        {
            foreach (Control kid in parent.Controls)
            {
                SaveState(writer, kid);
            }
        }

        private static void SaveData(XmlWriter writer, Control ctrl)
        {
            var tb = ctrl as TextBox;
            if (tb != null)
            {
                writer.WriteAttributeString("value", tb.Text);
                return;
            }

            var cb = ctrl as CheckBox;
            if (cb != null)
            {
                writer.WriteAttributeString("value", cb.Checked ? "true" : "false");
                return;
            }
        }

        private static void SaveState(XmlWriter writer, Control ctrl)
        {
            if (ctrl is Label) return;
            writer.WriteStartElement(ctrl.GetType().Name);
            writer.WriteAttributeString("name", ctrl.Name);
            SaveData(writer, ctrl);
            SaveKids(writer, ctrl);
            writer.WriteEndElement();
        }

        private static string GetStatePath(string fname, bool mkdir)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            path = Path.Combine(path, "DataDynamics");
            path = Path.Combine(path, "Temp");
            if (mkdir) Directory.CreateDirectory(path);
            return Path.Combine(path, fname);
        }
        #endregion

        #region Load
        private static string GetFileName(Control root)
        {
            return root.GetType().Name + ".xml";
        }

        public static void LoadState(Control root)
        {
            LoadState(root, GetFileName(root));
        }

        public static void LoadState(Control root, string fname)
        {
            string path = GetStatePath(fname, false);
            if (File.Exists(path))
            {
                var doc = new XmlDocument();
                doc.Load(path);

                LoadState(root, doc.DocumentElement);
            }
        }

        private static bool GetBool(XmlElement elem, string name, bool defval)
        {
            if (!elem.HasAttribute(name)) return defval;
            string s = elem.GetAttribute(name);
            if (string.IsNullOrEmpty(s)) return false;
            if (string.Compare(s, "true", true) == 0) return true;
            if (string.Compare(s, "1", true) == 0) return true;
            return false;
        }

        private static void LoadData(Control ctrl, XmlElement elem)
        {
            var tb = ctrl as TextBox;
            if (tb != null)
            {
                tb.Text = elem.GetAttribute("value");
                return;
            }

            var cb = ctrl as CheckBox;
            if (cb != null)
            {
                cb.Checked = GetBool(elem, "value", false);
                return;
            }
        }

        private static void LoadState(Control ctrl, XmlElement elem)
        {
            LoadData(ctrl, elem);

            foreach (XmlNode childNode in elem.ChildNodes)
            {
                var kidElem = childNode as XmlElement;
                if (kidElem == null) continue;

                string name = kidElem.GetAttribute("name");
                if (string.IsNullOrEmpty(name)) continue;

                var kid = ctrl.Controls.Cast<Control>().FirstOrDefault(c => c.Name == name);
                if (kid != null)
                {
                    LoadState(kid, kidElem);
                }
            }
        }
        #endregion
        #endregion

        #region SelectOperations
        public static int[] SelectOperations(string title, params string[] items)
        {
            int w = 300;
            int h = 300;
            using (var form = new Form())
            {
                form.Text = title;
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Size = new Size(w, h);
                w = form.ClientSize.Width;
                h = form.ClientSize.Height;

                form.SuspendLayout();
                const int padding = 10;

            	var ok = new Button
            	         	{
            	         		DialogResult = DialogResult.OK,
            	         		Text = "OK",
            	         		Anchor = AnchorStyles.Right | AnchorStyles.Bottom
            	         	};

            	var cancel = new Button
            	             	{
            	             		DialogResult = DialogResult.Cancel,
            	             		Text = "Cancel",
            	             		Anchor = AnchorStyles.Right | AnchorStyles.Bottom
            	             	};

            	cancel.Location = new Point(w - padding - cancel.Width, h - padding - cancel.Height);
                ok.Location = new Point(cancel.Left - padding - ok.Width, cancel.Top);

            	var list = new CheckedListBox
            	           	{
            	           		Location = new Point(padding, padding),
            	           		Size = new Size(w - 2*padding, ok.Top - padding),
            	           		Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom
            	           	};

            	foreach (var item in items)
                {
                    list.Items.Add(item);
                }

                form.Controls.AddRange(new Control[] { ok, cancel, list });
                form.AcceptButton = ok;

                form.ResumeLayout();

                if (form.ShowDialog() == DialogResult.OK)
                {
                    var res = new List<int>();
                    for (int i = 0; i < items.Length; ++i)
                    {
                        if (list.GetItemChecked(i))
                            res.Add(i);
                    }
                    if (res.Count > 0)
                        return res.ToArray();
                }
            }
            return null;
        }
        #endregion
    }
}