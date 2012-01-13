using System;

namespace tmx
{
    class Errors
    {
        public static void Report(int id, string msg)
        {
            Console.WriteLine("error TMX{0:G3}: {1}.", id, msg);
        }

        public static void Report(int id, string format, params object[] args)
        {
            Report(id, string.Format(format, args));
        }

        public static void ReportException(Exception e)
        {
            Console.WriteLine("Exception: {0}", e);
        }

        public static void InvalidCommandLine()
        {
            Report(1, "Invalid command line arguments.");
            Environment.Exit(-1);
        }

        public static void OutDir(Exception e)
        {
            Report(2, "Unable to create output directory.");
            ReportException(e);
            Environment.Exit(-1);
        }

        public static void NoInputFiles()
        {
            Report(3, "No input files");
            Environment.Exit(-1);
        }

        public static void UnableToTranslate(Exception e, string path)
        {
            Report(4, "Unable to translate {0}.", path);
            ReportException(e);
        }

        public static void UnableToLoadMXmlFile(Exception e)
        {
            Report(5, "Unable to load MXML file.");
            ReportException(e);
        }

        public static void UnableToCreateFile(Exception e, string path)
        {
            Report(6, "Unable to load create file {0}.", path);
            ReportException(e);
        }
    }
}