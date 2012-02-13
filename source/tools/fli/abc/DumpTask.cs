using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;
using DataDynamics;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.SWC;
using DataDynamics.PageFX.FLI.SWF;

namespace abc
{
    partial class Program
    {
        #region Entry Point
        static void Dump(CommandLine cl)
        {
            var exclude = cl.GetOptions("exclude");
            if (exclude != null && exclude.Length > 0)
            {
                Hashtable filter;
                if (exclude.Length == 1 && string.IsNullOrEmpty(exclude[0]))
                    filter = LoadStandardExcludes();
                else
                    filter = LoadExcludes(exclude);
                AbcDumpService.ClassFilter = filter;
            }
            else if (cl.HasOption("min"))
            {
                AbcDumpService.ClassFilter = LoadStandardExcludes();
            }

            foreach (var file in cl.GetInputFiles())
                Dump(file, cl);
        }

        public static void Dump(string path, CommandLine cl)
        {
            if (cl == null)
                cl = new CommandLine();

            SetupDumpServices(cl);

            if (cl.HasOption("classlist"))
            {
                bool sort = cl.HasOption("sort");
                DumpClassList(path, sort);
                return;
            }

            if (cl.HasOption("full") || SwfDumpService.SwfOnly)
            {
                FullDump(path);
                return;
            }

            if (cl.HasOption("instances", "instancelist"))
            {
                DumpInstances(cl, path);
                return;
            }

            string instanceName = cl.GetOption("", "instance");
            if (!string.IsNullOrEmpty(instanceName))
            {
                bool fh = cl.HasOption("fh");
                DumpInstance(path, instanceName, fh);
                return;
            }

            if (cl.HasOption("main"))
            {
                DumpMain(path);
                return;
            }

            if (cl.HasOption("method_names", "method-names"))
            {
                DumpMethodNames(path);
                return;
            }

            if (cl.HasOption("assets"))
            {
                DumpAssets(path);
                return;
            }

            if (cl.HasOption("method"))
            {
                string method = cl.GetOption(null, "method");
                if (!string.IsNullOrEmpty(method))
                {
                    if (DumpMethod(path, method))
                        return;
                }
            }

            if (cl.HasOption("refs"))
            {
                DumpRefs(path);
                return;
            }

            if (cl.HasOption("onlyfuncs"))
            {
                DumpFuncs(path);
                return;
            }

            if (cl.HasOption("swfchars", "swf-chars"))
            {
                DumpSwfChars(path);
                return;
            }

            DumpInstances(cl, path);
        }
        #endregion

        #region SetupDumpServices
        static void SetupDumpServices(CommandLine cl)
        {
            AbcDumpService.DumpCode = true;
            AbcDumpService.DumpInitializerCode = true;

            bool min = cl.HasOption("min");
            if (cl.HasOption("nocode"))
            {
                AbcDumpService.DumpCode = false;
                AbcDumpService.DumpInitializerCode = false;
            }

            if (min || cl.HasOption("noconst", "noconsts", "nocpool", "noconstpool"))
                AbcDumpService.DumpConstPool = false;

            if (cl.HasOption("const", "consts", "cpool", "constpool"))
                AbcDumpService.DumpConstPool = true;

            if (cl.HasOption("notraits", "notrait"))
                AbcDumpService.DumpTraits = false;

            if (cl.HasOption("noclass", "noclasses", "noklass"))
                AbcDumpService.DumpInstances = false;

            if (cl.HasOption("noscript", "noscripts"))
                AbcDumpService.DumpScripts = false;

            if (cl.HasOption("scripts"))
                AbcDumpService.DumpInstances = false;

            if (cl.HasOption("nomethod", "nomethods"))
                AbcDumpService.DumpMethods = false;

            if (cl.HasOption("func", "funcs", "functions"))
                AbcDumpService.DumpFunctions = true;

            if (cl.HasOption("nofunc", "nofuncs", "nofunctions"))
                AbcDumpService.DumpFunctions = false;

            if (min || cl.HasOption("nometadata", "nometa"))
                AbcDumpService.DumpMetadata = false;

            if (cl.HasOption("images"))
                SwfDumpService.DumpImages = true;

            if (cl.HasOption("shapes"))
                SwfDumpService.DumpShapes = true;

            if (cl.HasOption("verbose"))
            {
                SwfDumpService.Verbose = true;
                SwfDumpService.DumpShapes = true;
                SwfDumpService.DumpDisplayListTags = true;
            }

            if (cl.HasOption("swfonly"))
                SwfDumpService.SwfOnly = true;

            if (cl.HasOption("dumprefs", "dump-refs", "dump_refs"))
                SwfDumpService.DumpRefs = true;
        }
        #endregion

        #region FullDump
        static void FullDump(string path)
        {
            string ext = Utils.GetExt(path, "abc");

            if (ext == "abc")
            {
                AbcDumpService.Dump(path);
                return;
            }

            if (ext == "swf")
            {
                SwfDumpService.DumpSwf(path);
                return;
            }

            if (ext == "swc")
            {
                SwfDumpService.DumpSwc(path);
                return;
            }
        }
        #endregion

        #region DumpMethodNames
        static void DumpMethodNames(string path)
        {
            string ext = Utils.GetExt(path);
            if (ext == "abc")
            {
                var abc = new AbcFile(path);
                foreach (var m in abc.Methods)
                {
                    Console.WriteLine(m.FullName);
                }
                return;
            }
        }
        #endregion

        #region DumpMain
        static void DumpMain(string path)
        {
            string ext = Utils.GetExt(path);
            AbcDumpService.DumpInitializerCode = true;

            if (ext == "abc")
            {
                var abc = new AbcFile(path);
                DumpMain(abc, path);
                return;
            }

            if (ext == "swf")
            {
                var mov = new SwfMovie(path);
                var list = mov.GetAbcFiles();
                if (list.Count > 0)
                {
                    DumpMain(list[list.Count - 1], path);
                }
                return;
            }
        }

        static void DumpMain(AbcFile abc, string path)
        {
            var xws = XmlHelper.DefaultIndentedSettings;
            using (var writer = XmlWriter.Create(path + ".main.xml", xws))
            {
                var s = abc.LastScript;
                writer.WriteStartDocument();
                writer.WriteStartElement("abc");
                s.DumpXml(writer);
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
        #endregion

        #region DumpInstance
        static List<AbcFile> GetAbcFiles(string path)
        {
            string ext = Utils.GetExt(path);
            if (ext == "abc")
            {
                var abc = new AbcFile(path);
                var list = new List<AbcFile>();
                list.Add(abc);
                return list;
            }

            if (ext == "swf")
            {
                var swf = new SwfMovie(path);
                var list = swf.GetAbcFiles();
                return list;
            }

            if (ext == "swc")
            {
                var swc = new SwcFile(path);
                var list = swc.GetAbcFiles();
                return list;
            }

            return new List<AbcFile>();
        }

        static void DumpInstance(string path, string instanceName, bool fullHierarchy)
        {
            if (AbcDumpService.DumpCode)
                AbcDumpService.DumpInitializerCode = true;
            var list = GetAbcFiles(path);
            DumpInstance(list, instanceName, path, fullHierarchy);
            return;
        }

        static void DumpInstance(IEnumerable<AbcFile> list, string instanceName, string path, bool fullHierarchy)
        {
            var instance = AbcFile.FindInstance(list, instanceName);
            if (instance != null)
            {
                if (fullHierarchy)
                {
                    string xmlpath = MakeInstanceDumpPath(instance, path);
                    using (var writer = Utils.CreateXmlWriter(xmlpath))
                    {
                        writer.WriteStartDocument();
                        writer.WriteStartElement("abc");

                        while (true)
                        {
                            instance.DumpXml(writer);
                            var bn = instance.SuperName;
                            if (bn == null) break;
                            instanceName = bn.FullName;
                            instance = AbcFile.FindInstance(list, instanceName);
                            if (instance == null) break;
                        }

                        writer.WriteEndElement();
                        writer.WriteEndDocument();
                    }
                }
                else
                {
                    DumpInstance(instance, path);
                }
                return;
            }
            Console.WriteLine("Unable to find instance {0}", instanceName);
            return;
        }

        static IEnumerable<char> GetInvalidPathChars()
        {
            foreach (var c in Path.GetInvalidPathChars())
                yield return c;
            foreach (var c in Path.GetInvalidFileNameChars())
                yield return c;
        }

        static char ReplaceInvalidPathChar(char c)
        {
        	switch (c)
            {
                case '<': return '[';
                case '>': return ']';
                case '`': return (char)0;
            }
        	return GetInvalidPathChars().Contains(c) ? '_' : c;
        }

    	static string MakeValidPath(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            var list = new List<char>();
            for (int i = 0; i < path.Length; ++i)
            {
                char c = path[i];
                if (c == '`')
                {
                    i = ParseHelper.Skip(path, i + 1, char.IsDigit);
                    if (i < 0) break;
                    c = path[i];
                }

                c = ReplaceInvalidPathChar(c);
                if (c != 0)
                {
                    list.Add(c);
                }
            }
            return new string(list.ToArray());
        }

        static string MakeInstanceDumpPath(AbcInstance instance, string path)
        {
            return MakeValidPath(path + "." + instance.FullName + ".xml");
        }

        static void DumpInstance(AbcInstance instance, string path)
        {
            if (instance == null) return;

            path = MakeInstanceDumpPath(instance, path);
            DumpInstanceCore(instance, path, true);
        }

        static void DumpInstanceCore(AbcInstance instance, string path, bool withScript)
        {
            try
            {
                using (var writer = Utils.CreateXmlWriter(path))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("abc");

                    instance.DumpXml(writer);

                    if (withScript)
                    {
                        var abc = instance.ABC;
                        if (abc != null)
                        {
                            var script = instance.Script;
                            if (script != null)
                            {
                                script.DumpXml(writer);
                            }
                        }
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to dump instance {0}", instance.FullName);
                Console.WriteLine(e);
            }
        }
        #endregion

        #region DumpClassList
        static void DumpClassList(string path, bool sort)
        {
            string ext = Utils.GetExt(path);

            if (ext == "abc")
            {
                var abc = new AbcFile(path);
                var list = new List<string>();
                GetClassList(list, abc);
                WriteClassList(path, "", list, sort);
                return;
            }

            if (ext == "swf")
            {
                var swf = new SwfMovie(path);
                var all = new List<string>();
                int i = 1;
                foreach (var abc in swf.GetAbcFiles())
                {
                    GetClassList(all, abc);
                    var list = new List<string>();
                    GetClassList(list, abc);
                    WriteClassList(path, ".frame" + i, list, sort);
                    ++i;
                }
                WriteClassList(path, "", all, sort);
                return;
            }

            if (ext == "swc")
            {
                var swc = new SwcFile(path);
                var list = new List<string>();
                foreach (var abc in swc.GetAbcFiles())
                    GetClassList(list, abc);
                WriteClassList(path, "", list, sort);
                return;
            }
        }

        private static void GetClassList(ICollection<string> list, AbcFile abc)
        {
            foreach (var instance in abc.Instances)
            {
                list.Add(instance.FullName);
            }
        }

        private static List<string> GetDuplicates(IEnumerable<string> list)
        {
            var hash = new Hashtable();
            var dup = new List<string>();
            foreach (var s in list)
            {
                if (hash.Contains(s))
                {
                    dup.Add(s);
                }
                else
                {
                    hash[s] = s;
                }
            }
            return dup;
        }

        private static void WriteClassList(string path, string frame, List<string> list, bool sort)
        {
            if (sort) list.Sort();

            using (var writer = new StreamWriter(path + frame + ".classlist"))
            {
                var dup = GetDuplicates(list);
                if (dup.Count > 0)
                {
                    writer.WriteLine("------------------------------------------------------------");
                    writer.WriteLine("#duplicates: {0}", list.Count);
                    foreach (var s in dup)
                        writer.WriteLine(s);
                }

                writer.WriteLine("------------------------------------------------------------");
                writer.WriteLine("#all classes: {0}", list.Count);
                foreach (var s in list)
                    writer.WriteLine(s);
            }
        }
        #endregion

        #region DumpInstances
        static void DumpInstances(CommandLine cl, string path)
        {
            string ext = Utils.GetExt(path);

            if (ext == "abc")
            {
                var abc = new AbcFile(path);
                DumpInstances(path, new[] { abc });
                return;
            }

            bool frames = cl.HasOption("frames");

            if (ext == "swf")
            {
                var swf = new SwfMovie(path);
                var list = swf.GetAbcFiles();
                int n = list.Count;
                for (int i = 0; i < n; ++i)
                {
                    var abc = list[i];
                    string dir = path + ".instances";
                    if (frames)
                        dir += "\\" + i;
                    DumpInstancesTo(dir, abc);
                }
                return;
            }

            if (ext == "swc")
            {
                var swc = new SwcFile(path);
                DumpInstances(path, swc.GetAbcFiles());
                return;
            }
        }

        static void DumpInstances(string path, AbcFile abc)
        {
            DumpInstances(path, new[] { abc });
        }

        static void DumpInstances(string path, IEnumerable<AbcFile> list)
        {
            if (list == null) return;
            string dir = null;
            foreach (var abc in list)
            {
                if (dir == null)
                {
                    dir = Path.Combine(Path.GetDirectoryName(path), Path.GetFileName(path) + ".instances");
                    Directory.CreateDirectory(dir);
                }
                DumpInstancesTo(dir, abc);
            }
        }

        static void DumpInstancesTo(string dir, AbcFile abc)
        {
            Directory.CreateDirectory(dir);
            foreach (var instance in abc.Instances)
            {
                string path = Path.Combine(dir, MakeValidPath(instance.FullName + ".xml"));
                DumpInstanceCore(instance, path, true);
            }
        }
        #endregion

        #region DumpAssets
        static void DumpAssets(string path)
        {
            string ext = Utils.GetExt(path);
            if (ext == "swf")
            {
                var swf = new SwfMovie(path);
                DumpAssetsCore(swf, path + ".assets");
                return;
            }

            if (ext == "swc")
            {
                var swc = new SwcFile(path);
                foreach (var swf in swc.ExtractSwfs())
                    DumpAssetsCore(swf, path + "." + swf.Name + ".txt");
                return;
            }
        }

        private static void DumpAssetsCore(SwfMovie swf, string path)
        {
            try
            {
                using (var writer = new StreamWriter(path))
                {
                    WriteAssets(writer, "Import Assets:", swf, swf.GetImportAssets());
                    WriteAssets(writer, "Export Assets:", swf, swf.GetExportAssets());
                    WriteAssets(writer, "Symbol Table:", swf, swf.GetSymbolAssets());
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Unable to dump assets");
                Console.WriteLine(e);
            }
        }

        private static void WriteAssets(TextWriter writer, string header, SwfMovie swf, IEnumerable<SwfAsset> assets)
        {
            bool h = true;
            foreach (var asset in assets)
            {
                if (h)
                {
                    writer.WriteLine("-------------------------------------------------");
                    writer.WriteLine(header);
                    writer.WriteLine("-------------------------------------------------");
                    h = false;
                }
                DumpAsset(writer, swf, asset);
            }
        }

        private static void DumpAsset(TextWriter writer, SwfMovie swf, SwfAsset asset)
        {
            var tag = asset.Character as SwfTag;
            if (tag != null)
            {
                writer.WriteLine("[{0}] {1} - {2}", asset.Id, asset.Name, tag.TagCode);

                var refs = tag.GetRefs();
                if (refs != null && refs.Length > 0)
                {
                    foreach (int rid in refs)
                    {
                    	var rc = swf.GetCharacter((ushort)rid);
                    	if (rc != null)
                    	{
                    		if (string.IsNullOrEmpty(rc.Name))
                    			writer.WriteLine("\t{0}[{1}]", rc.TagCode, rid);
                    		else
                    			writer.WriteLine("\t{0}[{1}]", rc.Name, rid);
                    	}
                    	else
                    	{
                    		writer.Write("\tnull[{0}]", rid);
                    	}
                    }
                }
            }
            else
            {
                writer.WriteLine("[{0}] {1}", asset.Id, asset.Name);
            }
        }
        #endregion

        #region DumpMethod
        static bool DumpMethod(string path, string method)
        {
            string ext = Utils.GetExt(path);
            if (ext == "abc")
            {
                int index;
                if (!int.TryParse(method, out index))
                    return false;
                var abc = new AbcFile(path);
                return DumpMethod(abc, index, path + "." + method + ".xml");
            }

            if (ext == "swf")
            {
                int i = method.IndexOf('.');
                if (i < 0) return false;

                int frame;
                if (!int.TryParse(method.Substring(0, i), out frame))
                    return false;

                int index;
                if (!int.TryParse(method.Substring(i + 1), out index))
                    return false;

                if (frame < 0) return false;
                if (index < 0) return false;

                var swf = new SwfMovie(path);
                var list = swf.GetAbcFiles();

                if (frame >= list.Count) return false;

                return DumpMethod(list[frame], index, path + "." + method + ".xml");
            }

            return false;
        }

        static bool DumpMethod(AbcFile abc, int index, string path)
        {
            if (index < 0) return false;
            if (index >= abc.Methods.Count) return false;

            var m = abc.Methods[index];

            AbcDumpService.DumpCode = true;

            using (var writer = Utils.CreateXmlWriter(path))
            {
                writer.WriteStartDocument();
                m.DumpXml(writer);
                writer.WriteEndDocument();
            }

            return true;
        }

        #endregion

        #region DumpRefs
        static void DumpRefs(string path)
        {
            string ext = Utils.GetExt(path);
            if (ext == "swf")
            {
                var swf = new SwfMovie(path);
                swf.LinkAssets();

                using (var writer = new StreamWriter(path + ".refs"))
                {
                    foreach (var tag in swf.Tags)
                    {
                        var ch = tag as ISwfCharacter;
                        if (ch != null)
                        {
                            writer.Write("{0} - ", ch.CharacterID);
                            writer.Write(tag.TagCode.ToString());
                            if (!string.IsNullOrEmpty(ch.Name))
                            {
                                writer.Write(" - ");
                                writer.Write(ch.Name);
                            }
                            writer.WriteLine();
                            var refs = tag.GetRefs();
                            if (refs != null && refs.Length > 0)
                            {
                                writer.Write("\trefs: ");
                                for (int i = 0; i < refs.Length; ++i)
                                {
                                    if (i > 0) writer.Write(", ");
                                    writer.Write(refs[i]);
                                }
                                writer.WriteLine();
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region DumpFuncs
        static void DumpFuncs(string path)
        {
            string ext = Utils.GetExt(path);
            if (ext == "abc")
            {
                var abc = new AbcFile(path);
                DumpFuncsCore(abc, path + ".funcs.xml");
                return;
            }

            if (ext == "swf")
            {
                var swf = new SwfMovie(path);
                foreach (var abc in swf.GetAbcFiles())
                {
                    DumpFuncsCore(abc, path + "." + abc.Name + ".funcs.xml");
                }
                return;
            }
        }

        static void DumpFuncsCore(AbcFile abc, string path)
        {
            using (var writer = Utils.CreateXmlWriter(path))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("funcs");
                foreach (var method in abc.Methods)
                {
                    if (method.Trait != null) continue;
                    if (method.IsInitializer) continue;
                    method.DumpXml(writer);
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }
        #endregion

        #region DumpSwfChars
        static void DumpSwfChars(string path)
        {
            string ext = Utils.GetExt(path);
            if (ext == "swf")
            {
                var swf = new SwfMovie(path,
                                            SwfTagDecodeOptions.DonotDecodeCharacters
                                            | SwfTagDecodeOptions.DonotDecodeSprites);
                DumpSwfChars(swf, path + ".chars");
                return;
            }
        }

        static void DumpSwfChars(SwfMovie swf, string dir)
        {
            swf.LinkAssets();

            foreach (var tag in swf.Tags)
            {
                var sprite = tag as SwfSprite;
                if (sprite != null)
                {
                    
                }
                else
                {
                    var c = tag as ISwfCharacter;
                    if (c != null)
                    {
                        string name = c.CharacterID.ToString();
                        if (!string.IsNullOrEmpty(c.Name))
                        {
                            name += "_";
                            name += PathHelper.ReplaceInvalidFileNameChars(c.Name);
                        }
                        var data = tag.GetData();
                        data = TrimStart(data, 2);
                        Directory.CreateDirectory(dir);
                        string fname = Path.Combine(dir, name + ".bin");
                        File.WriteAllBytes(fname, data);
                    }
                }
            }
        }

        static byte[] TrimStart(byte[] data, int count)
        {
            if (count > data.Length)
                throw new ArgumentOutOfRangeException("count");
            int n = data.Length - count;
            var res = new byte[n];
            Buffer.BlockCopy(data, count, res, 0, n);
            return res;
        }
        #endregion
    }
}