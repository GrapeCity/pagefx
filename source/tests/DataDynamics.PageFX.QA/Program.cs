using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Text;
using System.Windows.Forms;
using DataDynamics.PageFX.CLI.Metadata;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.SWF;
using DataDynamics.PageFX.PDB;
using Infrastructure=DataDynamics.PageFX.FLI.Infrastructure;

namespace DataDynamics.PageFX
{
    class Program
    {
        #region MDB
        private static void ExportMscorlibToHtml()
        {
            string path = typeof(int).Assembly.Location;
            string dir = "c:\\tests\\cli";
            Directory.CreateDirectory(dir);
            MdbHtmlExport.Export(path, Path.Combine(dir, "mscorlib"));
        }

        private static void ExportMscorlibToXml()
        {
            string path = typeof(int).Assembly.Location;
            string dir = "c:\\tests\\cli";
            Directory.CreateDirectory(dir);
            MdbXmlExport.Export(path, Path.Combine(dir, "mscorlib.xml"));
        }
        #endregion

        #region CLI_TestDeserialization
        private static void CheckAssembly(IAssembly asm)
        {
            AssemblyVerifier.Verify(asm);
        }

        private static void CLI_TestDeserialization()
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "CSharp Files (*.cs, *.csc)|*.cs;*.csc|Managed Assemblies (*.dll, *.exe)|*.dll;*.exe";
                dlg.Multiselect = true;
                if (dlg.ShowDialog() != DialogResult.OK) return;

                var files = dlg.FileNames;
                switch (dlg.FilterIndex)
                {
                    case 1:
                        {
                            string output = "c:\\temp\\app.exe";
                            var options = new CompilerOptions();
                            options.Input.AddRange(files);
                            options.Output = output;
                            var errs = CompilerConsole.RunErr(options);
                            if (!errs.HasErrors)
                            {
                                var asm = DataDynamics.PageFX.CLI.Infrastructure.Deserialize(output, null);
                                CheckAssembly(asm);
                            }
                        }
                        break;

                    case 2:
                        {
                            var asm = DataDynamics.PageFX.CLI.Infrastructure.Deserialize(files[0], null);
                            CheckAssembly(asm);
                        }
                        break;
                }
            }
        }
        #endregion

        #region Select
        private static int[] Select(params string[] items)
        {
            int w = 300;
            int h = 300;
            using (var form = new Form())
            {
                form.Text = "Select Items...";
                form.StartPosition = FormStartPosition.CenterScreen;
                form.Size = new Size(w, h);
                w = form.ClientSize.Width;
                h = form.ClientSize.Height;

                form.SuspendLayout();
                int d = 10;

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

            	cancel.Location = new Point(w - d - cancel.Width, h - d - cancel.Height);
                ok.Location = new Point(cancel.Left - d - ok.Width, cancel.Top);

            	var list = new CheckedListBox
            	           	{
            	           		Location = new Point(d, d),
            	           		Size = new Size(w - 2*d, ok.Top - d),
            	           		Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom
            	           	};

            	foreach (var item in items)
                {
                    list.Items.Add(item);
                }

                form.Controls.AddRange(new Control[] {ok, cancel, list});
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

        #region CopyCombinations
        private static void CopyCombinations(string func, int n)
        {
            var s = new StringBuilder();
            int N = 1 << n;
            for (int k = 0; k < N; ++k)
            {
                s.Append(func);
                s.Append("(");
                for (int j = n - 1; j >= 0; --j)
                {
                    if (j != n - 1) s.Append(", ");
                    bool v = (k & (1 << j)) != 0;
                    s.Append(v ? "true" : "false");
                }
                s.AppendLine(");");
            }
            Clipboard.SetText(s.ToString());
        }
        #endregion

        #region SWF_CreateMovingRectangles
        private static float[] CreatePositions(int n)
        {
            float d = 1f / ((float)n - 1);
            var pos = new float[n];
            float v = 0;
            for (int i = 0; i < n; ++i)
            {
                pos[i] = v;
                v += d;
            }
            pos[n - 1] = 1;
            return pos;
        }

        public static ColorBlend CreatePalette(params Color[] colors)
        {
        	return new ColorBlend
        	       	{
        	       		Positions = CreatePositions(colors.Length),
        	       		Colors = colors
        	       	};
        }

        private static LinearGradientBrush CreateGradient(RectangleF r, params Color[] colors)
        {
        	return new LinearGradientBrush(r, Color.Lime, Color.Red, LinearGradientMode.Horizontal)
        	         	{
        	         		InterpolationColors = CreatePalette(colors)
        	         	};
        }

        private static void SWF_CreateMovingRectangles()
        {
            var swf = new SwfMovie();

            swf.SetBackgroundColor(Color.LightSteelBlue);

            var g = swf.Graphics;

            g.DrawRectangle(Pens.Black, 100, 100, 100, 100);
            g.FillRectangle(Brushes.Red, 100, 250, 100, 100);

            var r = new RectangleF(100, 400, 100, 100);
            var lg = new LinearGradientBrush(r, Color.Lime, Color.Yellow, LinearGradientMode.Horizontal);
            g.FillRectangle(lg, r);

            r = new RectangleF(250, 100, 200, 250);
            lg = CreateGradient(r, Color.Purple, Color.Blue, Color.Cyan, Color.Green, Color.Yellow, Color.Orange, Color.Red);
            g.FillRectangle(lg, r);

            r = new RectangleF(500, 100, 200, 250);
            var bmp = ResourceHelper.GetImage(typeof(QA), "images.bg5.jpg");
            var tb = new TextureBrush(bmp);
            g.FillRectangle(tb, r);

            swf.ShowFrame();

            int n = 6;

            for (int i = 0; i < 100; ++i)
            {
                for (int id = 1; id <= n; ++id)
                    swf.MoveObject((ushort)id, i, 0);
                swf.ShowFrame();
            }
            for (int i = 100; i >= 0; --i)
            {
                for (int id = 1; id <= n; ++id)
                    swf.MoveObject((ushort)id, i, 0);
                swf.ShowFrame();
            }

            QA.SaveSwf(swf, "MovingRectangles.swf");
        }
        #endregion

        #region FLI_Serialize
        private static void FLI_Serialize()
        {
            using (var dlg = new OpenFileDialog())
            {
                dlg.Filter = "Managed Assemblies (*.dll, *.exe)|*.dll;*.exe";
                dlg.Multiselect = true;
                if (dlg.ShowDialog() != DialogResult.OK) return;

                try
                {
                    string path = dlg.FileName;
                    var asm = DataDynamics.PageFX.CLI.Infrastructure.Deserialize(path, null);
                    Infrastructure.Serialize(asm, Path.ChangeExtension(path, ".swf"), "/format:swf");
                }
                catch (Exception exc)
                {
                    MessageBox.Show(exc.ToString());
                }
            }
        }
        #endregion

        #region PDB_Test
        private static void PDB_Test()
        {
            try
            {
                //PdbDump.Write(typeof(string).Assembly, "c:\\mscorlib.xml");
                //PdbDump.Write(@"c:\pfx\libs\corlib.dll");
                PdbDump.Write(@"c:\pfx\temp\HelloWorld.exe");
            }
            catch
            {
            }
        }
        #endregion

        #region DumpMdbTableIds
        static void DumpMdbTableIds()
        {
            var vals = Enum.GetValues(typeof(MdbTableId));
            using (var writer = new StreamWriter("c:\\pfx.mdbtables"))
            {
                foreach (var o in vals)
                {
                    int v = (o as IConvertible).ToInt32(null);
                    writer.WriteLine("{0} - {1} (0x{1:X2})", o, v);
                }
            }
        }
        #endregion

        #region Main
        static void Tests()
        {
            //ExportMscorlibToHtml();
            //ExportMscorlibToXml();
            //CLI_TestDeserialization();
            //TestAbcReadWrite();
            //CopyCombinations("f17", 12);
            //QA.CopyAvmCoreAssembly("c:\\QA\\avm");
            //SWF_CreateMovingRectangles();
            //FLI_Serialize();
            //PDB_Test();
        }

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            if (args.Length > 0)
            {
                var cl = CommandLine.Parse(args);
                if (cl != null && RunTools(cl)) return;
            }

            Tests();
            //DumpMdbTableIds();

            //NOTE: This flag is used to optimize loading of QAForm.
            //QA.LoadNUnitTests = false;
            //QA.ProtectNUnitTest = false;
            QA.ShowUI();
        }

        static bool RunTools(CommandLine cl)
        {
            string testset = cl.GetOption(null, "testset", "tests");
            if (!string.IsNullOrEmpty(testset))
            {
                string path = cl.GetOutputFile();
                if (string.IsNullOrEmpty(path))
                {
                    path = Path.Combine(Environment.CurrentDirectory, "tests.cs");
                }

                string ns = cl.GetOption(null, "ns");
                string classname = cl.GetOption(null, "class", "classname");

                QA.RunSuiteAsOneTest = true;
                QA.TestSet = testset;
                QA.GenerateNUnitTestFixture(path, ns, classname, "swf");
                return true;
            }

            return false;
        }
        #endregion
    }
}