using System.Collections.Generic;
using System.IO;

namespace DataDynamics.PageFX.FLI.ABC
{
    public class AbcProperty
    {
        public AbcMultiname Name
        {
            get
            {
                if (_getter != null)
                    return _getter.Trait.Name;
                if (_setter != null)
                    return _setter.Trait.Name;
                return null;
            }
        }

        public AbcMultiname Type
        {
            get
            {
                if (_getter != null)
                    return _getter.ReturnType;
                if (_setter != null)
                    return _setter.ReturnType;
                return null;
            }
        }

        public AbcMethod Getter
        {
            get { return _getter; }
            set { _getter = value; }
        }
        private AbcMethod _getter;

        public AbcMethod Setter
        {
            get { return _setter; }
            set { _setter = value; }
        }
        private AbcMethod _setter;

        private static void DumpAccessor(TextWriter writer, AbcMethod m, string tab)
        {
            if (m != null)
            {
                if (AbcDumpService.DumpCode && !m.IsAbstract)
                {
                    writer.WriteLine(tab + "get");
                    writer.WriteLine(tab + "{");
                    if (m.Body != null)
                        m.Body.IL.Dump(writer, tab + "\t");
                    writer.WriteLine(tab + "}");
                }
                else
                {
                    writer.WriteLine(tab + "get;");
                }
            }
        }

        public void Dump(TextWriter writer, string tab, bool isStatic)
        {
            writer.Write(tab);

            writer.Write("public ");

            writer.Write(Type.ToString());
            writer.Write(" ");

            //if ((_trait.Attributes & AbcTraitAttributes.Final) != 0)
            //    writer.Write("final ");
            //if ((_trait.Attributes & TraitAttributes.Override) != 0)
            //    writer.Write("override ");

            writer.Write(Name.Name);
            //writer.Write("(");
            //int n = Parameters.Count;
            //if (n > 0)
            //{
            //    for (int i = 0; i < n; ++i)
            //    {
            //        if (i > 0) writer.Write(", ");
            //        AbcParameter p = Parameters[i];
            //        writer.Write(p.Type.ToString());
            //        if (string.IsNullOrEmpty(p.Name))
            //        {
            //            writer.Write(" ");
            //            writer.Write(string.Format("arg{0}", i));
            //        }
            //        else
            //        {
            //            writer.Write(" ");
            //            writer.Write(p.Name);
            //        }
            //    }
            //}
            //writer.Write(")");
            writer.WriteLine();

            writer.WriteLine(tab + "{");

            DumpAccessor(writer, _getter, tab + "\t");
            DumpAccessor(writer, _setter, tab + "\t");

            writer.WriteLine(tab + "}");
        }
    }

    public class PropertyCollection : List<AbcProperty>
    {
        public AbcProperty this[AbcMultiname name]
        {
            get
            {
                return Find(delegate(AbcProperty p) { return p.Name == name; });
            }
        }

        public void Dump(TextWriter writer, string tab, bool isStatic)
        {
            int n = Count;
            for (int i = 0; i < n; ++i)
            {
                if (i > 0) writer.WriteLine();
                this[i].Dump(writer, tab, isStatic);
            }
        }
    }   
}