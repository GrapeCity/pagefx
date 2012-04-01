using System;
using System.IO;
using DataDynamics.Compression.Zip;
using DataDynamics.PageFX.FLI.ABC;

namespace DataDynamics.PageFX.FLI.SWC
{
    public static class Extensions
    {
        internal static string ToSwcId(this AbcMultiname mn)
        {
            string ns = mn.NamespaceString;
            string name = mn.NameString;
            if (string.IsNullOrEmpty(ns)) return name;
            return ns + ":" + name;
        }

        public static Stream ExtractSwfLibrary(this Stream input)
        {
            var zip = new ZipFile(input);
            return zip.ExtractSwfLibrary();
        }

        public static Stream ExtractSwfLibrary(this string path)
        {
            using (var input = File.OpenRead(path))
                return input.ExtractSwfLibrary();
        }

        public static Stream ExtractSwfLibrary(this ZipFile zip)
        {
            return Zip.Extract(zip, e => string.Compare(e.Name, SwcFile.LIBRARY_SWF, StringComparison.CurrentCultureIgnoreCase) == 0);
        }
    }
}