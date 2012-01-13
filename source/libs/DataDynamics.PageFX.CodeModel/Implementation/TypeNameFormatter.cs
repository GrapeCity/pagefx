using System.Collections.Generic;
using System.IO;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    public enum TypeNameStyle
    {
        User,
        CLR,
    }

    #region class TypeNameFormatter
    public static class TypeNameFormatter
    {
        #region Config
        public static bool Friendly;
        public static bool UseKeywords = true;

        public static TypeNameStyle Style;

        public static string BeginGenericArgs
        {
            get
            {
                switch (Style)
                {
                    case TypeNameStyle.CLR:
                        return "[";
                }
                return "<";
            }
        }

        public static string EndGenericArgs
        {
            get
            {
                switch (Style)
                {
                    case TypeNameStyle.CLR:
                        return "]";
                }
                return ">";
            }
        }
        #endregion

        public static string GetName(IType type, TypeNameKind kind)
        {
            var builder = new TypeNameBuilder();
            return builder.Build(type, kind);
        }

        public static string GetFullName(IType type)
        {
            return GetName(type, TypeNameKind.FullName);
        }

        public static string GetShortName(IType type)
        {
            return GetName(type, TypeNameKind.SigName);
        }

        public static string GetDisplayName(IType type)
        {
            return GetName(type, TypeNameKind.DisplayName);
        }

        public static string GetKey(IType type)
        {
            return GetName(type, TypeNameKind.Key);
        }

        public static string GetGenericParams(IEnumerable<IGenericParameter> parameters)
        {
            return Str.ToString(parameters, BeginGenericArgs, EndGenericArgs, ",", x => x.Name);
        }

        public static string GetName(IType type, TypeNameKind kind, TypeNameStyle style)
        {
            var old = Style;
            Style = style;
            string name = GetName(type, kind);
            Style = old;
            return name;
        }

        
        
        public static string QName(string ns, string name)
        {
            if (string.IsNullOrEmpty(ns)) return name;
            return ns + "." + name;
        }
    }
    #endregion

    #region class TypeNameBuilder
    internal class TypeNameBuilder
    {
        StringWriter _writer;

        public string Build(IType type, TypeNameKind kind)
        {
            _writer = new StringWriter();
            WriteName(type, kind);
            return _writer.ToString();
        }

        void WriteName(IType type, TypeNameKind kind)
        {
            switch (type.TypeKind)
            {
                case TypeKind.Array:
                    {
                        var arrType = (IArrayType)type;
                        WriteName(arrType.ElementType, kind);
                        Write(arrType.Dimensions.ToString());
                        return;
                    }

                case TypeKind.Pointer:
                    {
                        var ct = (ICompoundType)type;
                        WriteName(ct.ElementType, kind);
                        Write('*');
                        return;
                    }

                case TypeKind.Reference:
                    {
                        var ct = (ICompoundType)type;
                        WriteName(ct.ElementType, kind);
                        Write('&');
                        return;
                    }

                case TypeKind.GenericParameter:
                    {
                        var gp = (IGenericParameter)type;
                        string s = kind == TypeNameKind.Key ? type.Name + gp.ID : type.Name;
                        Write(s);
                        return;
                    }
            }

            var gt = type as IGenericType;
            if (gt != null)
            {
                if (kind == TypeNameKind.DisplayName)
                {
                    WriteGenericTypeName(gt, kind);
                    WriteGenericParams(gt.GenericParameters);
                }
                else
                {
                    WriteGenericTypeName(gt, kind);
                }
                return;
            }

            var gi = type as IGenericInstance;
            if (gi != null)
            {
                WriteGenericTypeName(gi.Type, kind);
                WriteTypes(gi.GenericArguments, kind, TypeNameFormatter.BeginGenericArgs, TypeNameFormatter.EndGenericArgs, ",");
                return;
            }

            if ((kind == TypeNameKind.DisplayName
                 || kind == TypeNameKind.SigName
                 || TypeNameFormatter.Friendly) && TypeNameFormatter.UseKeywords)
            {
                string k = TypeNameFormatter.GetKeyword("c#", type);
                if (!string.IsNullOrEmpty(k))
                {
                    Write(k);
                    return;
                }
            }

            if (UseFormatName(kind))
            {
                FormatName(type, kind);
                return;
            }

            Write(type.Name);
        }

        void WriteGenericParams(IEnumerable<IGenericParameter> parameters)
        {
            Write(TypeNameFormatter.BeginGenericArgs);
            bool comma = false;
            foreach (var p in parameters)
            {
                if (comma) Write(',');
                Write(p.Name);
                comma = true;
            }
            Write(TypeNameFormatter.EndGenericArgs);
        }

        void WriteTypes(IEnumerable<IType> types, TypeNameKind kind, string prefix, string suffix, string sep)
        {
            if (!string.IsNullOrEmpty(prefix))
                Write(prefix);
            bool s = false;
            foreach (var type in types)
            {
                if (s) Write(sep);
                WriteName(type, kind);
                s = true;
            }
            if (!string.IsNullOrEmpty(suffix))
                Write(suffix);
        }

        void WriteGenericTypeName(IType type, TypeNameKind kind)
        {
            if (UseFormatName(kind))
                FormatName(type, kind);
            else
                Write(type.Name);
        }

        void WriteNestedTypePrefix(IType type, TypeNameKind kind)
        {
            var list = new List<IType>();
            var dt = type.DeclaringType;
            while (dt != null)
            {
                list.Insert(0, dt);
                dt = dt.DeclaringType;
            }
            _writingNestedPrefix = true;
            WriteNsPrefix(list[0], kind);
            WriteTypes(list, kind, null, null, "+");
            Write('+');
            _writingNestedPrefix = false;
        }
        bool _writingNestedPrefix;

        void FormatName(IType type, TypeNameKind kind)
        {
            if (!_writingNestedPrefix)
            {
                if (type.DeclaringType != null)
                    WriteNestedTypePrefix(type, kind);
                else
                    WriteNsPrefix(type, kind);
            }

            bool r = TypeNameFormatter.Friendly || kind == TypeNameKind.DisplayName;
            if (r) WriteGoodName(type.Name);
            else Write(type.Name);
        }

        void WriteNsPrefix(IType type, TypeNameKind kind)
        {
            string ns = GetNamespace(type, kind);
            if (!string.IsNullOrEmpty(ns))
            {
                Write(ns);
                Write('.');
            }
        }

        void Write(string s)
        {
            _writer.Write(s);
        }

        void Write(char c)
        {
            _writer.Write(c);
        }

        #region Utils
        static string GetNamespace(IType type, TypeNameKind kind)
        {
            switch (kind)
            {
                case TypeNameKind.DisplayName:
                case TypeNameKind.FullName:
                case TypeNameKind.Key:
                case TypeNameKind.SigName:
                    return type.Namespace;
            }
            return "";
        }

        static bool UseFormatName(TypeNameKind kind)
        {
            switch (kind)
            {
                case TypeNameKind.DisplayName:
                case TypeNameKind.FullName:
                case TypeNameKind.NestedName:
                case TypeNameKind.Key:
                case TypeNameKind.SigName:
                    return true;
            }
            return false;
        }

        void WriteGoodName(string name)
        {
            if (name == null) return;
            int n = name.Length;
            if (n <= 2)
            {
                Write(name);
                return;
            }

            for (int i = 0; i < n; ++i)
            {
                char c = name[i];
                if (c == '`')
                {
                    int j = i + 1;
                    for (; j < n; ++j)
                    {
                        if (!char.IsDigit(name[j]))
                            break;
                    }
                    if (j == i + 1)
                        Write(c);
                    else
                        i = j - 1;
                }
                else
                {
                    Write(c);
                }
            }
        }
        #endregion
    }
    #endregion
}