using System.Collections.Generic;
using System.IO;

namespace DataDynamics.PageFX.FlashLand.Abc
{
    public sealed class AbcProperty
    {
        public AbcMultiname Name
        {
            get
            {
                if (Getter != null)
                    return Getter.Trait.Name;
                if (Setter != null)
                    return Setter.Trait.Name;
                return null;
            }
        }

        public AbcMultiname Type
        {
            get
            {
                if (Getter != null)
                    return Getter.ReturnType;
                if (Setter != null)
                    return Setter.ReturnType;
                return null;
            }
        }

	    public AbcMethod Getter { get; set; }

	    public AbcMethod Setter { get; set; }

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

            DumpAccessor(writer, Getter, tab + "\t");
            DumpAccessor(writer, Setter, tab + "\t");

            writer.WriteLine(tab + "}");
        }
    }

    public sealed class PropertyCollection : List<AbcProperty>
    {
        public AbcProperty this[AbcMultiname name]
        {
            get { return Find(p => ReferenceEquals(p.Name, name)); }
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