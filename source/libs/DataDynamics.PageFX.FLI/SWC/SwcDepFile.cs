using System.Collections;
using System.Collections.Generic;
using System.IO;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;
using DataDynamics.PageFX.FLI.SWF;

namespace DataDynamics.PageFX.FLI.SWC
{
    public class SwcDepFile
    {
        #region ctors
        public SwcDepFile()
        {
        }

        public SwcDepFile(Stream input)
        {
            Load(input);
        }
        #endregion

        #region enum DepType
        public enum DepType : byte
        {
            FileRef,
            TypeRef,
            NamespaceRef,
        }
        #endregion

        #region class Dep
        public abstract class Dep
        {
            public abstract DepType Type { get; }

            public abstract void Read(SwfReader reader);

            public abstract void Write(SwfWriter writer);

            public static Dep Create(DepType type)
            {
                switch (type)
                {
                    case DepType.FileRef: return new FileRef();
                    case DepType.TypeRef: return new TypeRef();
                    case DepType.NamespaceRef: return new NamespaceRef();
                }
                return null;
            }
        }
        #endregion

        #region class FileRef
        public class FileRef : Dep
        {
            public int LibIndex;
            public int FileIndex;

            public override DepType Type
            {
                get { return DepType.FileRef; }
            }

            public override void Read(SwfReader reader)
            {
                LibIndex = (int)reader.ReadUIntEncoded();
                FileIndex = (int)reader.ReadUIntEncoded();
            }

            public override void Write(SwfWriter writer)
            {
                writer.WriteUIntEncoded(LibIndex);
                writer.WriteUIntEncoded(FileIndex);
            }
        }
        #endregion

        #region class TypeRef
        public class TypeRef : Dep
        {
            public override DepType Type
            {
                get { return DepType.TypeRef; }
            }

            public string FullName;

            public override void Read(SwfReader reader)
            {
                FullName = reader.ReadString();
            }

            public override void Write(SwfWriter writer)
            {
                writer.WriteString(FullName);
            }
        }
        #endregion

        #region class NamespaceRef
        public class NamespaceRef : Dep
        {
            public override DepType Type
            {
                get { return DepType.NamespaceRef; }
            }

            public int Index;

            public override void Read(SwfReader reader)
            {
                Index = (int)reader.ReadUIntEncoded();
            }

            public override void Write(SwfWriter writer)
            {
                writer.WriteUIntEncoded(Index);
            }
        }
        #endregion

        #region class DepList
        public class DepList : List<Dep>
        {
            public void Read(SwfReader reader)
            {
                int n = (int)reader.ReadUIntEncoded();
                for (int i = 0; i < n; ++i)
                {
                    var type = (DepType)reader.ReadByte();
                    var dep = Dep.Create(type);
                    dep.Read(reader);
                    Add(dep);
                }
            }

            public void Write(SwfWriter writer)
            {
                int n = Count;
                writer.WriteUIntEncoded(n);
                for (int i = 0; i < n; ++i)
                {
                    var dep = this[i];
                    writer.WriteByte((byte)dep.Type);
                    dep.Write(writer);
                }
            }
        }
        #endregion

        #region class File
        public class File
        {
            public int LibIndex;
            public int FileIndex;

            public DepList Deps
            {
                get { return _deps; }
            }
            private readonly DepList _deps = new DepList();

            public void Read(SwfReader reader)
            {
                LibIndex = (int)reader.ReadUIntEncoded();
                FileIndex = (int)reader.ReadUIntEncoded();
                _deps.Read(reader);
            }

            public void Write(SwfWriter writer)
            {
                writer.WriteUIntEncoded(LibIndex);
                writer.WriteUIntEncoded(FileIndex);
                _deps.Write(writer);
            }

            readonly Hashtable _fileRefs = new Hashtable();
            readonly Hashtable _typeRefs = new Hashtable();
            readonly Hashtable _nsRefs = new Hashtable();

            public void AddFileRef(int lib, int file)
            {
                string key = lib + ":" + file;
                if (_fileRefs.Contains(key)) return;
                var r = new FileRef {LibIndex = lib, FileIndex = file};
                _fileRefs[key] = r;
                _deps.Add(r);
            }

            public void AddTypeRef(string name)
            {
                if (_typeRefs.Contains(name)) return;
                var dep = new TypeRef {FullName = name};
                _typeRefs[name] = dep;
                _deps.Add(dep);
            }

            public void AddNamespaceRef(int index)
            {
                if (_nsRefs.Contains(index)) return;
                var r = new NamespaceRef {Index = index};
                _nsRefs[index] = r;
                _deps.Add(r);
            }
        }
        #endregion

        #region class FileList
        public class FileList : List<File>
        {
            public void Read(SwfReader reader)
            {
                int n = (int)reader.ReadUIntEncoded();
                for (int i = 0; i < n; ++i)
                {
                    var f = new File();
                    f.Read(reader);
                    Add(f);
                }
            }

            public void Write(SwfWriter writer)
            {
                int n = Count;
                writer.WriteUIntEncoded(n);
                for (int i = 0; i < n; ++i)
                    this[i].Write(writer);
            }
        }
        #endregion

        #region Properties
        public List<string> Namespaces
        {
            get { return _nslist; }
        }
        readonly List<string> _nslist = new List<string>();
        readonly Dictionary<string, int> _nsindex = new Dictionary<string, int>();

        public int AddNamespace(string name)
        {
            int i;
            if (_nsindex.TryGetValue(name, out i))
                return i;
            i = _nslist.Count;
            _nslist.Add(name);
            _nsindex.Add(name, i);
            return i;
        }

        public FileList Files
        {
            get { return _files; }
        }
        readonly FileList _files = new FileList();
        #endregion

        #region IO
        public void Load(byte[] data)
        {
            using (var reader = new SwfReader(data))
                Read(reader);
        }

        public void Load(Stream input)
        {
            using (var reader = new SwfReader(input))
                Read(reader);
        }

        public void Load(string path)
        {
            using (var stream = System.IO.File.OpenRead(path))
                Load(stream);
        }

        public void Save(Stream output)
        {
            Write(new SwfWriter(output));
        }

        public void Save(string path)
        {
            using (var stream = System.IO.File.OpenWrite(path))
                Save(stream);
        }

        public void Read(SwfReader reader)
        {
            int n = (int)reader.ReadUIntEncoded();
            for (int i = 0; i < n; ++i)
            {
                string ns = reader.ReadString();
                AddNamespace(ns);
            }
            _files.Read(reader);
        }

        public void Write(SwfWriter writer)
        {
            int n = _nslist.Count;
            writer.WriteUIntEncoded(n);
            for (int i = 0; i < n; ++i)
                writer.WriteString(_nslist[i]);
            _files.Write(writer);
        }
        #endregion

        #region Build
        public void Build(SwcFile swc, ITypeResolver resolver)
        {
        	var libs = swc.GetLibraries();
        	foreach (var lib in libs)
        	{
        		var abclist = lib.GetAbcFiles();
        		foreach (var abc in abclist)
        		{
        			var f = new File
        			        	{
        			        		LibIndex = lib.Index,
        			        		FileIndex = abc.Index
        			        	};

        			BuildTypeRefs(f, lib, abc, resolver);

        			var nsrefs = abc.GetNsRefs();
        			if (nsrefs != null)
        			{
        				foreach (var ns in nsrefs)
        				{
        					int nsIndex = AddNamespace(ns);
        					f.AddNamespaceRef(nsIndex);
        				}
        			}

        			if (f.Deps.Count > 0)
        				_files.Add(f);
        		}
        	}
        }

    	static void BuildTypeRefs(File f, SwfMovie lib, AbcFile abc, ITypeResolver resolver)
        {
            int n = abc.Multinames.Count;
            for (int i = 1; i < n; ++i)
            {
                var mn = abc.Multinames[i];
                if (mn.IsRuntime) continue;

                foreach (var fullName in mn.GetFullNames())
                {
                    if (string.IsNullOrEmpty(fullName)) continue;
                    var type = resolver.Resolve(fullName);
                    if (type == null) continue;
                    var instance = type.Tag as AbcInstance;
                    if (instance != null)
                    {
                        if (instance.SWC == lib.SWC)
                        {
                            int libIndex = instance.ABC.SWF.Index;
                            int abcIndex = instance.ABC.Index;
                            if (libIndex != lib.Index || abcIndex != abc.Index)
                                f.AddFileRef(libIndex, abcIndex);
                        }
                        else if (!instance.ABC.IsCoreAPI)
                        {
                            f.AddTypeRef(fullName);
                        }
                    }
                    else
                    {
                        f.AddTypeRef(fullName);
                    }
                }
            }
        }
        #endregion
    }
}