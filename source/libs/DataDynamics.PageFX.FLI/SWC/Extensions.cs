using System;
using System.IO;
using DataDynamics.PageFX.FLI.ABC;
using Ionic.Zip;

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
            var zip = ZipFile.Read(input);
            return zip.ExtractSwfLibrary();
        }

        public static Stream ExtractSwfLibrary(this string path)
        {
            using (var input = File.OpenRead(path))
                return input.ExtractSwfLibrary();
        }

        public static Stream ExtractSwfLibrary(this ZipFile zip)
        {
            return zip.ExtractEntry(e => e.FileName.Equals(SwcFile.LIBRARY_SWF, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}