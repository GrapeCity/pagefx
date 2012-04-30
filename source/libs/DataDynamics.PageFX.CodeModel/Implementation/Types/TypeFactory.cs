using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace DataDynamics.PageFX.CodeModel
{
    public static class TypeFactory
    {
        static readonly Hashtable TypeCache = new Hashtable();

        public static void ClearCache()
        {
            TypeCache.Clear();
        }

        #region GetKey
        public static string GetKey(IType type, string suffix)
        {
            return type.Key + suffix;
        }

        public static string GetKey(IGenericType type, IEnumerable<IType> args)
        {
            var sb = new StringBuilder();
            sb.Append(type.FullName);
            sb.Append('<');
            foreach (var arg in args)
            {
                sb.Append(arg.Key);
                sb.Append(',');
            }
            sb.Length -= 1;
            sb.Append('>');
            return sb.ToString();
        }
        #endregion

        public static IType MakeArray(IType type, IArrayDimensionCollection dim)
        {
            if (dim == null)
                dim = new ArrayDimensionCollection();
        	var dimensionString = dim.ToString();
        	string key = GetKey(type, dimensionString);
            var res = (IType)TypeCache[key];
            if (res != null) return res;
            res = new ArrayType(type, dim);
            TypeCache[key] = res;
            return res;
        }

        public static IType MakeArray(IType type)
        {
            return MakeArray(type, null);
        }

        public static IType MakePointerType(IType type)
        {
            string key = GetKey(type, CLRNames.Ptr);
            var res = (IType)TypeCache[key];
            if (res != null) return res;
            res = new PointerType(type);
            TypeCache[key] = res;
            return res;
        }

        public static IType MakeReferenceType(IType type)
        {
            string key = GetKey(type, CLRNames.Ref);
            var res = (IType)TypeCache[key];
            if (res != null) return res;
            res = new ReferenceType(type);
            TypeCache[key] = res;
            return res;
        }

        public static IType MakeGenericType(IGenericType type, IEnumerable<IType> args)
        {
            string key = GetKey(type, args);
            var res = (IType)TypeCache[key];
            if (res != null) return res;
            var gi = new GenericInstance(type, args) {Key = key};
            TypeCache[key] = gi;
            return gi;
        }

        static IEnumerable<T> One<T>(T item)
        {
            yield return item;
        }

        public static IType MakeGenericType(IGenericType type, IType arg)
        {
            return MakeGenericType(type, One(arg));
        }
    }
}