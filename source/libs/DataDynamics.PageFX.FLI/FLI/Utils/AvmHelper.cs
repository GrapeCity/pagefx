using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using DataDynamics.PageFX.CodeModel;
using DataDynamics.PageFX.FLI.ABC;

namespace DataDynamics.PageFX.FLI
{
    static class AvmHelper
    {
        public static bool IsInitializer(IMethod method)
        {
            if (method == null) return false;
            if (!method.IsConstructor) return false;
            var m = method.Tag as AbcMethod;
            if (m == null) return false;
            return m.IsInitializer;
        }

        public static bool IsFlashEventMethod(IMethod method)
        {
            if (method == null) return false;
            var e = method.Association as IEvent;
            if (e == null) return false;
            return Attrs.Has(e, "EventAttribute");
        }

        public static AbcInstance ImportType(AbcFile abc, IAssembly assembly, string fullname)
        {
            var type = AssemblyIndex.FindType(assembly, fullname);
            if (type == null)
                throw Errors.Type.UnableToFind.CreateException(fullname);

            var instance = type.Tag as AbcInstance;
            if (instance == null)
                throw Errors.Type.NotLinked.CreateException(fullname);

            return abc.ImportInstance(instance);
        }

        public static bool HasEmbeddedAsset(IField field)
        {
            var attr = Attrs.Find(field, Attrs.Embed);
            return attr != null;
        }

        public static IManifestResource FindResource(IAssembly asm, string subname)
        {
            subname = subname.ToLower();
            foreach (var res in asm.MainModule.Resources)
            {
                string name = res.Name.ToLower();
                if (name.Contains(subname))
                    return res;
            }
            return null;
        }

        public static string GetExtension(string path)
        {
            string ext = Path.GetExtension(path);
            if (string.IsNullOrEmpty(ext)) return null;
            if (ext[0] == '.')
                return ext.Substring(1).ToLower();
            return ext.ToLower();
        }

        public static IField[] GetEnumFields(IType type)
        {
            if (type == null)
                throw new ArgumentNullException("type");
            if (type.TypeKind != TypeKind.Enum)
                throw new ArgumentException("type is not enum");
            var list = new List<IField>();
            foreach (var f in type.Fields)
            {
                if (f.IsStatic)
                    list.Add(f);
            }
            return list.ToArray();
        }

        //NOTE: For enums we will use m_value name for internal value.
        public static void RenameField(IField field)
        {
            var dt = field.DeclaringType;
            if (!dt.IsEnum) return;
            if (field.IsStatic) return;
            field.Name = Const.Boxing.Value;
        }

        public static bool HasProtectedNamespace(IType type)
        {
            switch (type.TypeKind)
            {
                case TypeKind.Interface:
                case TypeKind.Enum:
                    return false;
            }
            return true;
        }

        #region GetUsedTypes
        public static void GetUsedTypes(ICodeNode node, List<IType> list, Hashtable hash)
        {
            var rp = node as ITypeReferenceProvider;
            if (rp != null)
            {
                var refs = rp.GetTypeReferences();
                if (refs != null)
                {
                    foreach (var t in refs)
                    {
                        if (t != null && !hash.Contains(t))
                        {
                            hash[t] = t;
                            list.Add(t);
                        }
                    }
                }
            }

            var e = node as IExpression;
            if (e != null)
            {
                var t = e.ResultType;
                if (t != null && !hash.Contains(t))
                {
                    hash[t] = t;
                    list.Add(t);
                }
            }

            var kids = node.ChildNodes;
            if (kids != null)
            {
                foreach (var kid in kids)
                {
                    GetUsedTypes(kid, list, hash);
                }
            }
        }

        public static List<IType> GetUsedTypes(IMethod m)
        {
            if (m != null)
            {
                var body = m.Body;
                if (body != null)
                {
                    var list = new List<IType>();
                    var hash = new Hashtable();
                    foreach (var st in body.Statements)
                    {
                        GetUsedTypes(st, list, hash);
                    }
                    return list;
                }
            }
            return null;
        }
        #endregion
    }
}