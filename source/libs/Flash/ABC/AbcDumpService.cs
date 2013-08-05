using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace DataDynamics.PageFX.Flash.Abc
{
    public static class AbcDumpService
    {
        public static void Dump(string path)
        {
            var abc = new AbcFile();
            abc.Load(path);

            if (TextFormat)
                abc.DumpFile(path + ".txt");

            if (XmlFormat)
                abc.DumpXml(path + ".xml");

            if (DumpClassList)
                WriteClassList(abc, path);
        }

        private static void WriteClassList(AbcFile abc, string path)
        {
            var list = new List<AbcInstance>(abc.Instances);
            list.Sort((a, b) => a.FullName.CompareTo(b.FullName));

            string fname = path + ".classlist.txt";
            using (var writer = new StreamWriter(fname))
            {
                foreach (var instance in list)
                {
                    writer.WriteLine(instance.FullName);
                    if (DumpTraits)
                    {
                        foreach (var t in instance.Traits)
                            DumpTrait(writer, t);

                        foreach (var t in instance.Class.Traits)
                            DumpTrait(writer, t);
                    }
                }
            }
        }

        private static void DumpTrait(TextWriter writer, AbcTrait t)
        {
            switch (t.Kind)
            {
                case AbcTraitKind.Const:
                case AbcTraitKind.Slot:
                    writer.WriteLine("\tslot {0}: {1}", t.Name.FullName, t.SlotType.FullName);
                    break;

                case AbcTraitKind.Getter:
                    writer.WriteLine("\tgetter {0}", t.Name.FullName);
                    break;

                case AbcTraitKind.Setter:
                    writer.WriteLine("\tsetter {0}", t.Name.FullName);
                    break;

                case AbcTraitKind.Method:
                case AbcTraitKind.Function:
                    writer.WriteLine("\t{0}", t.Name.FullName);
                    break;

                case AbcTraitKind.Class:
                    writer.WriteLine("\tclass {0}", t.Name.FullName);
                    break;
            }
        }

        public static bool TextFormat;
        public static bool XmlFormat = true;
        public static bool DumpCode;
        public static bool DumpClassList;
        public static bool DumpTraits = true;
        public static bool DumpInitializerCode = true;
        public static bool DumpConstPool;
        public static bool DumpMetadata = true;
        public static bool DumpMethods = true;
        public static bool DumpScripts = true;
        public static bool DumpInstances = true;
        public static bool DumpFunctions;

        public static Hashtable ClassFilter;

        public static bool FilterClass(AbcInstance instance)
        {
            if (ClassFilter == null) return false;
            string key = instance.FullName;
            if (ClassFilter.Contains(key)) return true;
            return false;
        }
    }
}