using System;
using System.Threading;
using System.Windows.Forms;

namespace DataDynamics.PageFX
{
    class Global
    {
        public static bool nodie;
        public static string swfpath;
    }

    class Program
    {
        static void Usage()
        {
            Console.WriteLine("usage: flashell [options] <swf-file>");
            Console.WriteLine("options:");
            Console.WriteLine("\t/snapshot[:filename]\t\t\tSpecify that you want to get snapshot.");
            Console.WriteLine("\t/nodie\t\t\tDisable automatic closing of flash player host form.");
        }

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var cl = CommandLine.Parse(args);
            if (cl == null)
                cl = new CommandLine();

            if (cl.HasOption("h", "help", "?"))
            {
                Usage();
                return;
            }

            Global.nodie = cl.HasOption("nodie");

            var files = cl.GetInputFiles();
            string path;
            if (files.Length == 0)
            {
                using (var dlg = new OpenFileDialog())
                {
                    dlg.Filter = "Flash Files (*.swf)|*.swf";
                    if (dlg.ShowDialog() != DialogResult.OK)
                        return;
                    path = dlg.FileName;
                }
            }
            else
            {
                path = files[0];
            }

            Global.swfpath = path;

            Play();
        }

        static void Play()
        {
            var thread = new Thread(Start);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            thread.Join();
        }

        static void Start()
        {
            Application.Run(new MainForm());
        }
    }
}
