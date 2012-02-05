using System;
using System.IO;
using DataDynamics;
using DataDynamics.PageFX.FLI.ABC;

namespace abc
{
    internal partial class Program
    {
        private static void List(CommandLine cl)
        {
            var files = cl.GetInputFiles();
            var types = cl.GetOptions("t", "type");
            bool g = cl.GetBoolOption(false, "g", "global");
            foreach (var file in files)
                List(file, types, g);
        }

        private static void List(string path, string[] types, bool listGlobal)
        {
            var abc = new AbcFile(path);
            foreach (var instance in abc.Instances)
            {
                if (types != null && types.Length > 0)
                {
                    string fullname = instance.FullName;
                    if (
                        Algorithms.Contains(types,
                                            delegate(string type) { return string.Compare(type, fullname, true) == 0; }))
                    {
                        List(Console.Out, instance);
                    }
                }
                else
                {
                    List(Console.Out, instance);
                }
            }
            if (listGlobal)
            {
                var writer = Console.Out;
                writer.WriteLine("Global Functions:");
                foreach (var script in abc.Scripts)
                {
                    foreach (var trait in script.Traits)
                        ListTrait(writer, trait);
                }
            }
        }

        private static void List(TextWriter writer, AbcInstance instance)
        {
            writer.WriteLine("class {0}:", instance.FullName);
            foreach (var trait in instance.Traits)
            {
                ListTrait(writer, trait);
            }
            foreach (var trait in instance.Class.Traits)
            {
                ListTrait(writer, trait);
            }
        }

        private static void WriteQName(TextWriter writer, AbcMultiname mn)
        {
            string ns = mn.NamespaceString;
            if (!string.IsNullOrEmpty(ns))
            {
                writer.Write(ns);
                writer.Write(".");
            }
            writer.Write(mn.NameString);
        }

        private static void WriteTypeName(TextWriter writer, AbcMultiname mn)
        {
            if (mn == null || mn.Index == 0)
                writer.Write("*");
            else
                WriteQName(writer, mn);
        }

        private static void WriteTraitName(TextWriter writer, AbcMultiname mn)
        {
            string ns = mn.NamespaceString;
            writer.Write("[");
            writer.Write(mn.Namespace.Kind);
            writer.Write(" = ");
            writer.Write(ns);
            writer.Write("] ");
            writer.Write(mn.NameString);
        }

        private static void BeginTrait(TextWriter writer, AbcTrait trait)
        {
            var k = trait.Kind;

            writer.Write("\t");

            //switch (k)
            //{
            //    case AbcTraitKind.Const:
            //    case AbcTraitKind.Slot:
            //        {
            //            SlotTrait slot = (SlotTrait)trait.Data;
            //            writer.Write(slot.SlotID);
            //            writer.Write(" ");
            //        }
            //        break;

            //    case AbcTraitKind.Method:
            //    case AbcTraitKind.Getter:
            //    case AbcTraitKind.Setter:
            //        {
            //            MethodTrait mt = ((MethodTrait)trait.Data);
            //            writer.Write(mt.DispID);
            //            writer.Write(" ");
            //        }
            //        break;
            //}

            writer.Write(trait.Visibility.ToString().ToLower());
            writer.Write(" ");
            if (k == AbcTraitKind.Const)
            {
                writer.Write("const ");
            }
            else if (trait.IsStatic)
            {
                writer.Write("static ");
            }
        }

        private static void WriteParams(TextWriter writer, AbcMethod method)
        {
            writer.Write("(");
            int n = method.Parameters.Count;
            for (int i = 0; i < n; ++i)
            {
                if (i > 0) writer.Write(", ");
                var p = method.Parameters[i];
                WriteQName(writer, p.Type);
                writer.Write(" ");
                if (p.HasName)
                    writer.Write(p.Name.Value);
                else
                    writer.Write("arg{0}", i);
            }
            if ((method.Flags & AbcMethodFlags.NeedRest) != 0)
            {
                if (n > 0) writer.Write(", ");
                writer.Write("params object[] rest");
            }
            writer.Write(")");
        }

        private static void ListTrait(TextWriter writer, AbcTrait trait)
        {
            var k = trait.Kind;
            switch (k)
            {
                case AbcTraitKind.Slot:
                case AbcTraitKind.Const:
                    {
                        BeginTrait(writer, trait);
                        WriteTypeName(writer, trait.SlotType);
                        writer.Write(" ");
                        WriteTraitName(writer, trait.Name);
                        writer.WriteLine(";");
                    }
                    break;

                case AbcTraitKind.Method:
                case AbcTraitKind.Getter:
                case AbcTraitKind.Setter:
                    {
                        var method = trait.Method;
                        BeginTrait(writer, trait);
                        WriteTypeName(writer, method.ReturnType);
                        writer.Write(" ");

                        if (k == AbcTraitKind.Getter)
                            writer.Write("get ");
                        else if (k == AbcTraitKind.Setter)
                            writer.Write("set ");

                        WriteTraitName(writer, trait.Name);
                        WriteParams(writer, method);
                        writer.WriteLine(";");
                    }
                    break;

                case AbcTraitKind.Function:
                    break;
            }
        }
    }
}