using System;
using System.Collections.Generic;
using System.IO;
using DataDynamics.PageFX.FlashLand.Abc;
using DataDynamics.PageFX.FlashLand.Swc;
using DataDynamics.PageFX.FlashLand.Swf.Tags.Control;
using Ionic.Zip;

namespace DataDynamics.PageFX.FLI
{
    internal partial class SwfCompiler
    {
        #region CreateScripts
        List<AbcFile> _scripts;

        void CreateScripts(AbcFile app, SwfTagSymbolClass symbols)
        {
            _scripts = new List<AbcFile>();
            foreach (var script in app.Scripts)
            {
                var abc = new AbcFile
                                    {
                                        ImportTypeStrategy = ImportTypeStrategy.External,
                                        IsSwcScript = true,
                                    };

                abc.ImportScript(script);
                abc.Name = GetScriptName(abc);
                abc.Finish();

                //abc.Test();

                //LogSwc(abc);

                _scripts.Add(abc);
                AddAbcTag(abc);
            }
        }

        //static bool pfxswclog_append;

        //static void LogSwc(AbcFile abc)
        //{
        //    if (abc.Instances.Count > 1)
        //    {
        //        using (var sw = new StreamWriter(@"c:\pfxswc.log", pfxswclog_append))
        //        {
        //            pfxswclog_append = true;
        //            sw.WriteLine("---" + abc.Name);
        //            foreach (var instance in abc.Instances)
        //            {
        //                sw.WriteLine("  " + instance.FullName);
        //            }
        //        }
        //    }
        //}
        #endregion

        #region BuildSwc
        private sealed class FileData
        {
            public string Path;
            public byte[] Data;
        }

        List<FileData> _swcFiles;

        void BuildSwc()
        {
            if (_catalog == null || _catalogBytes == null || _libraryBytes == null)
                throw new InvalidOperationException("SWC catalog has not been created.");
        }
        #endregion

        #region SaveSwc
		private void SaveSwc(string path)
        {
            var zip = CreateSwcZip();
            File.WriteAllBytes(path, zip);
        }

        private void SaveSwc(Stream output)
        {
            if (output == null)
                throw new ArgumentNullException("output");
            var zip = CreateSwcZip();
            output.Write(zip, 0, zip.Length);
        }

        private byte[] CreateSwcZip()
        {
            var output = new MemoryStream();
	        var zipFile = new ZipFile();
			BuildSwcZip(zipFile);
			zipFile.Save(output);
            return output.ToArray();
        }

        private void BuildSwcZip(ZipFile zipFile)
        {
	        zipFile.AddEntry(SwcFile.CATALOG_XML, "", _catalogBytes);
	        zipFile.AddEntry(SwcFile.LIBRARY_SWF, "", _libraryBytes);

	        //For testing purposes.
            //string outpath = OutputPath;
            //if (!string.IsNullOrEmpty(outpath))
            //{
            //    string dir = Path.GetDirectoryName(outpath);
            //    dir = Path.Combine(dir, outpath + ".content");
            //    Directory.CreateDirectory(dir);
            //    File.WriteAllBytes(Path.Combine(dir, SwcFile.CATALOG_XML), _catalogBytes);
            //    File.WriteAllBytes(Path.Combine(dir, SwcFile.LIBRARY_SWF), _libraryBytes);
            //}

            if (_swcFiles != null)
            {
                foreach (var f in _swcFiles)
                {
	                zipFile.AddEntry(f.Path, "", f.Data);
                }
            }
        }

	    #endregion

        #region Utils
        static string GetScriptName(AbcFile abc)
        {
            if (abc.Scripts.Count != 1)
                return "";
            var script = abc.Scripts[0];
            int n = script.Traits.Count;
            if (n == 0) return "";
            if (n == 1)
                return GetScriptName(script.Traits[0]);
            //TODO:
            return GetScriptName(script.Traits[0]);
        }

        static string GetScriptName(AbcTrait trait)
        {
            var mn = trait.Name;
            string ns = mn.NamespaceString;
            string name = mn.NameString;
            if (string.IsNullOrEmpty(ns))
                return name;
            ns = ns.Replace('.', '/');
            return ns + "/" + name;
        }
        #endregion
    }
}