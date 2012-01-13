using System;
using System.IO;
using System.Xml;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.SWC;

namespace DataDynamics.PageFX.FLI
{
    partial class SwfCompiler
    {
        XmlDocument _catalog;

        byte[] _libraryBytes;
        byte[] _catalogBytes;
        long _modAppAssembly;

        #region BuildCatalog
        void BuildCatalog()
        {
            _modAppAssembly = GetMod(ApplicationAssembly.Location);

            _catalog = new XmlDocument();
            var root = CreateXmlElement("swc");
            _catalog.AppendChild(root);

            var versions = CreateXmlElement("versions");
            root.AppendChild(versions);

            versions.AppendChild(CreateXmlElement("swc", "version", "1.2"));
            //TODO:
            if (HasFlexReference)
            {
                versions.AppendChild(CreateXmlElement("flex", "version", "3.0.0", "build", "477"));
            }

            var features = CreateXmlElement("features");
            root.AppendChild(features);
            features.AppendChild(CreateXmlElement("feature-script-deps"));
            if (SwcHasFiles)
                features.AppendChild(CreateXmlElement("feature-files"));

            //TODO: <components>

            var libs = CreateXmlElement("libraries");
            root.AppendChild(libs);

            var lib = CreateXmlElement(SwcCatalog.Elements.Library, "path", SwcFile.LIBRARY_SWF);
            libs.AppendChild(lib);

            CreateScriptElements(lib);

            _libraryBytes = ToByteArray(ms => _swf.Save(ms));

            var digests = CreateXmlElement(SwcCatalog.Elements.Digests);
            string digest = HashHelper.GetDigest(HashHelper.TypeSHA256, _libraryBytes);
            digests.AppendChild(CreateDigestElement(HashHelper.TypeSHA256, false, digest));
            lib.AppendChild(digests);
            
            _catalogBytes = ToByteArray(
                ms =>
                    {
                        var xws = new XmlWriterSettings {Indent = true, IndentChars = "  "};
                        using (var xw = XmlWriter.Create(ms, xws))
                            _catalog.Save(xw);
                    });
        }
        #endregion

        #region CreateScriptElements
        void CreateScriptElements(XmlElement libElem)
        {
            foreach (var abc in _scripts)
            {
                var e = CreateXmlElement("script", "name", abc.Name);
                libElem.AppendChild(e);
                FillScriptElement(e, abc);
            }
        }

        void FillScriptElement(XmlElement scriptElem, AbcFile abc)
        {
            SetModAndSignatureChecksum(scriptElem, abc);

            AbcInstance def;
            string defID = GetScriptDef(abc, out def);
            if (!string.IsNullOrEmpty(defID))
            {
                scriptElem.AppendChild(CreateDefElement(defID));
                if (def != null)
                    CreateDeps(scriptElem, def, defID);
            }
        }

        void CreateDeps(XmlElement scriptElem, AbcInstance def, string defID)
        {
            var builder = new SwcDepBuilder();
            var list = builder.Build(FrameApp, def, defID);
            foreach (var dep in list)
            {
                var depElem = CreateDepElement(dep[0], dep[1]);
                scriptElem.AppendChild(depElem);
            }
        }

        #region GetScriptDef
        static string GetScriptDef(AbcFile abc, out AbcInstance instance)
        {
            instance = null;
            int n = abc.Scripts.Count;
            if (n == 0) return "";
            if (n == 1)
            {
                var script = abc.Scripts[0];
                instance = script.SingleInstance;
                if (instance != null)
                    return SwcHelper.ToSwcID(instance.Name);
            }
            return "";
        }
        #endregion
        
        #region mod & signatureChecksum
        void SetModAndSignatureChecksum(XmlElement element, AbcFile abc)
        {
            //mod is required attribute!
            long mod = CalcScriptMod(abc);
            element.SetAttribute("mod", mod.ToString());

            //TODO: compute signatureChecksum - optional attribute
        }

        long CalcScriptMod(AbcFile abc)
        {
            return _modAppAssembly;
        }

        //NOTE: During watching at flex compiler source I found the following:
        //Adler32 is used to compute signatureChecksum. flex code comment: //adler32 is much faster than CRC32, almost as reliable
        //flex2.compiler.as3.SignatureEvaluator computes signature traversing script AST
        long CalcScriptSignatureChecksum(AbcFile abc)
        {
            return 0;
        }
        #endregion
        #endregion

        bool HasFlexReference
        {
            get { return false; }
        }

        bool SwcHasFiles
        {
            get { return _swcFiles != null && _swcFiles.Count > 0; }
        }

        #region XmlUtils
        XmlElement CreateDepElement(string id, string type)
        {
            return CreateXmlElement(SwcCatalog.Elements.Dep, "id", id, "type", type);
        }

        XmlElement CreateDefElement(string id)
        {
            return CreateXmlElement(SwcCatalog.Elements.Def, "id", id);
        }

        XmlElement CreateDigestElement(string type, bool signed, string value)
        {
            return CreateXmlElement(SwcCatalog.Elements.Digest,
                                    "type", type,
                                    "signed", signed ? "true" : "false",
                                    "value", value);
        }

        XmlElement CreateXmlElement(string name)
        {
            return _catalog.CreateElement(name, SwcCatalog.XmlNamespace);
        }

        XmlElement CreateXmlElement(string name, params string[] attrs)
        {
            var e = CreateXmlElement(name);
            for (int i = 0; i + 1 < attrs.Length; i += 2)
            {
                e.SetAttribute(attrs[i], attrs[i + 1]);
            }
            return e;
        }
        #endregion

        #region Utils
        static long GetMod(string path)
        {
            var wt = File.GetLastWriteTime(path);
            return wt.ToFileTime();
        }

        static byte[] ToByteArray(Action<MemoryStream> save)
        {
            var ms = new MemoryStream();
            save(ms);
            ms.Close();
            return ms.ToArray();
        }
        #endregion
    }
}