namespace Microsoft.VisualBasic.CompilerServices
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using System.Resources;
    using System.Runtime.InteropServices;
    using System.Text;
    using System.Threading;

    public sealed class Utils
    {
        internal const char chBackslash = '\\';
        internal const char chCharH0A = '\n';
        internal const char chCharH0B = '\v';
        internal const char chCharH0C = '\f';
        internal const char chCharH0D = '\r';
        internal const char chColon = ':';
        internal const char chDblQuote = '"';
        internal const char chGenericManglingChar = '`';
        internal const char chHyphen = '-';
        internal const char chIntlSpace = '　';
        internal const char chLetterA = 'A';
        internal const char chLetterZ = 'Z';
        internal const char chLineFeed = '\n';
        internal const char chPeriod = '.';
        internal const char chPlus = '+';
        internal const char chSlash = '/';
        internal const char chSpace = ' ';
        internal const char chTab = '\t';
        internal const char chZero = '0';
        private const int ERROR_INVALID_PARAMETER = 0x57;
        internal const int FACILITY_CONTROL = 0xa0000;
        internal const int FACILITY_ITF = 0x40000;
        internal const int FACILITY_RPC = 0x10000;
        internal static char[] m_achIntlSpace = new char[] { ' ', '　' };
        private static bool m_TriedLoadingResourceManager;
#if NOT_PFX
        private static ResourceManager m_VBAResourceManager;
#endif
        private static Assembly m_VBRuntimeAssembly;
        internal const CompareOptions OptionCompareTextFlags = (CompareOptions.IgnoreWidth | CompareOptions.IgnoreKanaType | CompareOptions.IgnoreCase);
        private static readonly object ResourceManagerSyncObj = new object();
        private const string ResourceMsgDefault = "Message text unavailable.  Resource file 'Microsoft.VisualBasic resources' not found.";
        internal const int SCODE_FACILITY = 0x1fff0000;
        internal const int SEVERITY_ERROR = -2147483648;
        private const string VBDefaultErrorID = "ID95";
        private static readonly Type VoidType = Type.GetType("System.Void");

        private Utils()
        {
        }

        public static Array CopyArray(Array arySrc, Array aryDest)
        {
            if (arySrc != null)
            {
                int length = arySrc.Length;
                if (length == 0)
                {
                    return aryDest;
                }
                if (aryDest.Rank != arySrc.Rank)
                {
                    throw ExceptionUtils.VbMakeException(new InvalidCastException(GetResourceString("Array_RankMismatch")), 9);
                }
                int num8 = aryDest.Rank - 2;
                for (int i = 0; i <= num8; i++)
                {
                    if (aryDest.GetUpperBound(i) != arySrc.GetUpperBound(i))
                    {
                        throw ExceptionUtils.VbMakeException(new ArrayTypeMismatchException(GetResourceString("Array_TypeMismatch")), 9);
                    }
                }
                if (length > aryDest.Length)
                {
                    length = aryDest.Length;
                }
                if (arySrc.Rank > 1)
                {
                    int rank = arySrc.Rank;
                    int num7 = arySrc.GetLength(rank - 1);
                    int num6 = aryDest.GetLength(rank - 1);
                    if (num6 != 0)
                    {
                        int num5 = Math.Min(num7, num6);
                        int num9 = (arySrc.Length / num7) - 1;
                        for (int j = 0; j <= num9; j++)
                        {
                            Array.Copy(arySrc, j * num7, aryDest, j * num6, num5);
                        }
                    }
                    return aryDest;
                }
                Array.Copy(arySrc, aryDest, length);
            }
            return aryDest;
        }

        internal static string FieldToString(FieldInfo Field)
        {
            string str = "";
            Type fieldType = Field.FieldType;
            if (Field.IsPublic)
            {
                str = str + "Public ";
            }
            else if (Field.IsPrivate)
            {
                str = str + "Private ";
            }
            else if (Field.IsAssembly)
            {
                str = str + "Friend ";
            }
            else if (Field.IsFamily)
            {
                str = str + "Protected ";
            }
            else if (Field.IsFamilyOrAssembly)
            {
                str = str + "Protected Friend ";
            }
            return ((str + Field.Name) + " As " + VBFriendlyNameOfType(fieldType, true));
        }

        private static string GetArraySuffixAndElementType(ref Type typ)
        {
            if (!typ.IsArray)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            do
            {
                builder.Append("(");
                builder.Append(',', typ.GetArrayRank() - 1);
                builder.Append(")");
                typ = typ.GetElementType();
            }
            while (typ.IsArray);
            return builder.ToString();
        }

        internal static CultureInfo GetCultureInfo()
        {
            return Thread.CurrentThread.CurrentCulture;
        }

        internal static DateTimeFormatInfo GetDateTimeFormatInfo()
        {
            return Thread.CurrentThread.CurrentCulture.DateTimeFormat;
        }

        internal static Encoding GetFileIOEncoding()
        {
            return Encoding.UTF8;
        }

        private static string GetGenericArgsSuffix(Type typ)
        {
            if (!typ.IsGenericType)
            {
                return null;
            }
            Type[] genericArguments = typ.GetGenericArguments();
            int length = genericArguments.Length;
            int num2 = length;
            if ((typ.DeclaringType != null) && typ.DeclaringType.IsGenericType)
            {
                num2 -= typ.DeclaringType.GetGenericArguments().Length;
            }
            if (num2 == 0)
            {
                return null;
            }
            StringBuilder builder = new StringBuilder();
            builder.Append("(Of ");
            int num4 = length - 1;
            for (int i = length - num2; i <= num4; i++)
            {
                builder.Append(VBFriendlyNameOfType(genericArguments[i], false));
                if (i != (length - 1))
                {
                    builder.Append(',');
                }
            }
            builder.Append(")");
            return builder.ToString();
        }

        internal static CultureInfo GetInvariantCultureInfo()
        {
            return CultureInfo.InvariantCulture;
        }

        internal static int GetLocaleCodePage()
        {
            return 0;
        }

        internal static string GetResourceString(vbErrors ResourceId)
        {
            return GetResourceString("ID" + Conversions.ToString((int) ResourceId));
        }

        internal static string GetResourceString(string ResourceKey)
        {
#if NOT_PFX
            string str2;
            if (VBAResourceManager == null)
            {
                return "Message text unavailable.  Resource file 'Microsoft.VisualBasic resources' not found.";
            }
            try
            {
                str2 = VBAResourceManager.GetString(ResourceKey, GetCultureInfo());
                if (str2 == null)
                {
                    str2 = VBAResourceManager.GetString("ID95");
                }
            }
            catch (StackOverflowException exception)
            {
                throw exception;
            }
            catch (OutOfMemoryException exception2)
            {
                throw exception2;
            }
            catch (ThreadAbortException exception3)
            {
                throw exception3;
            }
            catch (Exception)
            {
                str2 = "Message text unavailable.  Resource file 'Microsoft.VisualBasic resources' not found.";
            }
            return str2;
#else
            return ResourceKey;
#endif
        }

        public static string GetResourceString(string ResourceKey, params string[] Args)
        {
            string str = null;
            string format = null;
            try
            {
                format = GetResourceString(ResourceKey);
                str = string.Format(Thread.CurrentThread.CurrentUICulture, format, Args);
            }
            catch (StackOverflowException exception)
            {
                throw exception;
            }
            catch (OutOfMemoryException exception2)
            {
                throw exception2;
            }
            catch (ThreadAbortException exception3)
            {
                throw exception3;
            }
            catch (Exception)
            {
            }
            if (str != "")
            {
                return str;
            }
            return format;
        }

        internal static string GetResourceString(string ResourceKey, bool NotUsed)
        {
#if NOT_PFX
            string str2;
            if (VBAResourceManager == null)
            {
                return "Message text unavailable.  Resource file 'Microsoft.VisualBasic resources' not found.";
            }
            try
            {
                str2 = VBAResourceManager.GetString(ResourceKey, GetCultureInfo());
                if (str2 == null)
                {
                    str2 = VBAResourceManager.GetString(ResourceKey);
                }
            }
            catch (StackOverflowException exception)
            {
                throw exception;
            }
            catch (OutOfMemoryException exception2)
            {
                throw exception2;
            }
            catch (ThreadAbortException exception3)
            {
                throw exception3;
            }
            catch (Exception)
            {
                str2 = null;
            }
            return str2;
#else
            return ResourceKey;
#endif
        }

        internal static bool IsHexOrOctValue(string Value, ref long i64Value)
        {
            int num = 0;
            int length = Value.Length;
            while (num < length)
            {
                char ch = Value[num];
                if ((ch == '&') && ((num + 2) < length))
                {
                    ch = char.ToLower(Value[num + 1], CultureInfo.InvariantCulture);
                    string str = ToHalfwidthNumbers(Value.Substring(num + 2), GetCultureInfo());
                    switch (ch)
                    {
                        case 'h':
                            i64Value = Convert.ToInt64(str, 0x10);
                            goto Label_0087;

                        case 'o':
                            i64Value = Convert.ToInt64(str, 8);
                            goto Label_0087;
                    }
                    throw new FormatException();
                }
                if ((ch != ' ') && (ch != '　'))
                {
                    return false;
                }
                num++;
            }
            return false;
        Label_0087:
            return true;
        }

        internal static bool IsHexOrOctValue(string Value, ref ulong ui64Value)
        {
            int num = 0;
            int length = Value.Length;
            while (num < length)
            {
                char ch = Value[num];
                if ((ch == '&') && ((num + 2) < length))
                {
                    ch = char.ToLower(Value[num + 1], CultureInfo.InvariantCulture);
                    string str = ToHalfwidthNumbers(Value.Substring(num + 2), GetCultureInfo());
                    switch (ch)
                    {
                        case 'h':
                            ui64Value = Convert.ToUInt64(str, 0x10);
                            goto Label_0087;

                        case 'o':
                            ui64Value = Convert.ToUInt64(str, 8);
                            goto Label_0087;
                    }
                    throw new FormatException();
                }
                if ((ch != ' ') && (ch != '　'))
                {
                    return false;
                }
                num++;
            }
            return false;
        Label_0087:
            return true;
        }

        internal static string MemberToString(MemberInfo Member)
        {
            switch (Member.MemberType)
            {
                case MemberTypes.Constructor:
                case MemberTypes.Method:
                    return MethodToString((MethodBase) Member);

                case MemberTypes.Field:
                    return FieldToString((FieldInfo) Member);

                case MemberTypes.Property:
                    return PropertyToString((PropertyInfo) Member);
            }
            return Member.Name;
        }

        public static string MethodToString(MethodBase Method)
        {
            bool flag;
            Type typ = null;
            string str = "";
            if (Method.MemberType == MemberTypes.Method)
            {
                typ = ((MethodInfo) Method).ReturnType;
            }
            if (Method.IsPublic)
            {
                str = str + "Public ";
            }
            else if (Method.IsPrivate)
            {
                str = str + "Private ";
            }
            else if (Method.IsAssembly)
            {
                str = str + "Friend ";
            }
            if ((Method.Attributes & MethodAttributes.Virtual) != MethodAttributes.ReuseSlot)
            {
                if (!Method.DeclaringType.IsInterface)
                {
                    str = str + "Overrides ";
                }
            }
            else if (Symbols.IsShared(Method))
            {
                str = str + "Shared ";
            }
            Symbols.UserDefinedOperator uNDEF = Symbols.UserDefinedOperator.UNDEF;
            if (Symbols.IsUserDefinedOperator(Method))
            {
                uNDEF = Symbols.MapToUserDefinedOperator(Method);
            }
            if (uNDEF != Symbols.UserDefinedOperator.UNDEF)
            {
                if (uNDEF == Symbols.UserDefinedOperator.Narrow)
                {
                    str = str + "Narrowing ";
                }
                else if (uNDEF == Symbols.UserDefinedOperator.Widen)
                {
                    str = str + "Widening ";
                }
                str = str + "Operator ";
            }
            else if ((typ == null) || (typ == VoidType))
            {
                str = str + "Sub ";
            }
            else
            {
                str = str + "Function ";
            }
            if (uNDEF != Symbols.UserDefinedOperator.UNDEF)
            {
                str = str + Symbols.OperatorNames[(int) uNDEF];
            }
            else if (Method.MemberType == MemberTypes.Constructor)
            {
                str = str + "New";
            }
            else
            {
                str = str + Method.Name;
            }
            if (Symbols.IsGeneric(Method))
            {
                str = str + "(Of ";
                flag = true;
                foreach (Type type2 in Symbols.GetTypeParameters(Method))
                {
                    if (!flag)
                    {
                        str = str + ", ";
                    }
                    else
                    {
                        flag = false;
                    }
                    str = str + VBFriendlyNameOfType(type2, false);
                }
                str = str + ")";
            }
            str = str + "(";
            flag = true;
            foreach (ParameterInfo info in Method.GetParameters())
            {
                if (!flag)
                {
                    str = str + ", ";
                }
                else
                {
                    flag = false;
                }
                str = str + ParameterToString(info);
            }
            str = str + ")";
            if ((typ == null) || (typ == VoidType))
            {
                return str;
            }
            return (str + " As " + VBFriendlyNameOfType(typ, true));
        }

        internal static string ParameterToString(ParameterInfo Parameter)
        {
            string str2 = "";
            Type parameterType = Parameter.ParameterType;
            if (Parameter.IsOptional)
            {
                str2 = str2 + "[";
            }
            if (parameterType.IsByRef)
            {
                str2 = str2 + "ByRef ";
                parameterType = parameterType.GetElementType();
            }
            else if (Symbols.IsParamArray(Parameter))
            {
                str2 = str2 + "ParamArray ";
            }
            str2 = str2 + Parameter.Name + " As " + VBFriendlyNameOfType(parameterType, true);
            if (!Parameter.IsOptional)
            {
                return str2;
            }
            object defaultValue = Parameter.DefaultValue;
            if (defaultValue == null)
            {
                str2 = str2 + " = Nothing";
            }
            else
            {
                Type type = defaultValue.GetType();
                if (type != VoidType)
                {
                    if (Symbols.IsEnum(type))
                    {
                        str2 = str2 + " = " + Enum.GetName(type, defaultValue);
                    }
                    else
                    {
                        str2 = str2 + " = " + Conversions.ToString(defaultValue);
                    }
                }
            }
            return (str2 + "]");
        }

        internal static string PropertyToString(PropertyInfo Prop)
        {
            ParameterInfo[] parameters;
            Type returnType;
            string str2 = "";
            PropertyKind readWrite = PropertyKind.ReadWrite;
            MethodInfo getMethod = Prop.GetGetMethod();
            if (getMethod != null)
            {
                if (Prop.GetSetMethod() != null)
                {
                    readWrite = PropertyKind.ReadWrite;
                }
                else
                {
                    readWrite = PropertyKind.ReadOnly;
                }
                parameters = getMethod.GetParameters();
                returnType = getMethod.ReturnType;
            }
            else
            {
                readWrite = PropertyKind.WriteOnly;
                getMethod = Prop.GetSetMethod();
                ParameterInfo[] sourceArray = getMethod.GetParameters();
                parameters = new ParameterInfo[(sourceArray.Length - 2) + 1];
                Array.Copy(sourceArray, parameters, parameters.Length);
                returnType = sourceArray[sourceArray.Length - 1].ParameterType;
            }
            str2 = str2 + "Public ";
            if ((getMethod.Attributes & MethodAttributes.Virtual) != MethodAttributes.ReuseSlot)
            {
                if (!Prop.DeclaringType.IsInterface)
                {
                    str2 = str2 + "Overrides ";
                }
            }
            else if (Symbols.IsShared(getMethod))
            {
                str2 = str2 + "Shared ";
            }
            switch (readWrite)
            {
                case PropertyKind.ReadOnly:
                    str2 = str2 + "ReadOnly ";
                    break;

                case PropertyKind.WriteOnly:
                    str2 = str2 + "WriteOnly ";
                    break;
            }
            str2 = str2 + "Property " + Prop.Name + "(";
            bool flag = true;
            foreach (ParameterInfo info2 in parameters)
            {
                if (!flag)
                {
                    str2 = str2 + ", ";
                }
                else
                {
                    flag = false;
                }
                str2 = str2 + ParameterToString(info2);
            }
            return (str2 + ") As " + VBFriendlyNameOfType(returnType, true));
        }

        internal static string StdFormat(string s)
        {
            char ch = (char)0;
            char ch2 = (char)0;
            char ch3 = (char)0;
            NumberFormatInfo numberFormat = Thread.CurrentThread.CurrentCulture.NumberFormat;
            int index = s.IndexOf(numberFormat.NumberDecimalSeparator);
            if (index == -1)
            {
                return s;
            }
            try
            {
                ch = s[0];
                ch2 = s[1];
                ch3 = s[2];
            }
            catch (StackOverflowException exception)
            {
                throw exception;
            }
            catch (OutOfMemoryException exception2)
            {
                throw exception2;
            }
            catch (ThreadAbortException exception3)
            {
                throw exception3;
            }
            catch (Exception)
            {
            }
            if (s[index] == '.')
            {
                if ((ch == '0') && (ch2 == '.'))
                {
                    return s.Substring(1);
                }
                if (((ch != '-') && (ch != '+')) && (ch != ' '))
                {
                    return s;
                }
                if ((ch2 != '0') || (ch3 != '.'))
                {
                    return s;
                }
            }
            StringBuilder builder = new StringBuilder(s);
            builder[index] = '.';
            if ((ch == '0') && (ch2 == '.'))
            {
                return builder.ToString(1, builder.Length - 1);
            }
            if ((((ch == '-') || (ch == '+')) || (ch == ' ')) && ((ch2 == '0') && (ch3 == '.')))
            {
                builder.Remove(1, 1);
                return builder.ToString();
            }
            return builder.ToString();
        }

        public static void ThrowException(int hr)
        {
            throw ExceptionUtils.VbMakeException(hr);
        }

        internal static string ToHalfwidthNumbers(string s, CultureInfo culture)
        {
            return s;
        }

        internal static string VBFriendlyName(object Obj)
        {
            if (Obj == null)
            {
                return "Nothing";
            }
            return VBFriendlyName(Obj.GetType(), Obj);
        }

        internal static string VBFriendlyName(Type typ)
        {
            return VBFriendlyNameOfType(typ, false);
        }

        internal static string VBFriendlyName(Type typ, object o)
        {
            return VBFriendlyNameOfType(typ, false);
        }

        internal static string VBFriendlyNameOfType(Type typ, [Optional, DefaultParameterValue(false)] bool FullName)
        {
            string name;
            TypeCode typeCode;
            string arraySuffixAndElementType = GetArraySuffixAndElementType(ref typ);
            if (typ.IsEnum)
            {
                typeCode = TypeCode.Object;
            }
            else
            {
                typeCode = Type.GetTypeCode(typ);
            }
            switch (typeCode)
            {
                case TypeCode.DBNull:
                    name = "DBNull";
                    break;

                case TypeCode.Boolean:
                    name = "Boolean";
                    break;

                case TypeCode.Char:
                    name = "Char";
                    break;

                case TypeCode.SByte:
                    name = "SByte";
                    break;

                case TypeCode.Byte:
                    name = "Byte";
                    break;

                case TypeCode.Int16:
                    name = "Short";
                    break;

                case TypeCode.UInt16:
                    name = "UShort";
                    break;

                case TypeCode.Int32:
                    name = "Integer";
                    break;

                case TypeCode.UInt32:
                    name = "UInteger";
                    break;

                case TypeCode.Int64:
                    name = "Long";
                    break;

                case TypeCode.UInt64:
                    name = "ULong";
                    break;

                case TypeCode.Single:
                    name = "Single";
                    break;

                case TypeCode.Double:
                    name = "Double";
                    break;

                case TypeCode.Decimal:
                    name = "Decimal";
                    break;

                case TypeCode.DateTime:
                    name = "Date";
                    break;

                case TypeCode.String:
                    name = "String";
                    break;

                default:
                    if (Symbols.IsGenericParameter(typ))
                    {
                        name = typ.Name;
                    }
                    else
                    {
                        string fullName;
                        string str6 = null;
                        string genericArgsSuffix = GetGenericArgsSuffix(typ);
                        if (FullName)
                        {
                            if (typ.DeclaringType != null)
                            {
                                str6 = VBFriendlyNameOfType(typ.DeclaringType, true);
                                fullName = typ.Name;
                            }
                            else
                            {
                                fullName = typ.FullName;
                            }
                        }
                        else
                        {
                            fullName = typ.Name;
                        }
                        if (genericArgsSuffix != null)
                        {
                            int length = fullName.LastIndexOf('`');
                            if (length != -1)
                            {
                                fullName = fullName.Substring(0, length);
                            }
                            name = fullName + genericArgsSuffix;
                        }
                        else
                        {
                            name = fullName;
                        }
                        if (str6 != null)
                        {
                            name = str6 + "." + name;
                        }
                    }
                    break;
            }
            if (arraySuffixAndElementType != null)
            {
                name = name + arraySuffixAndElementType;
            }
            return name;
        }

#if NOT_PFX
        internal static ResourceManager VBAResourceManager
        {
            get
            {
                if (m_VBAResourceManager == null)
                {
                    object resourceManagerSyncObj = ResourceManagerSyncObj;
                    ObjectFlowControl.CheckForSyncLockOnValueType(resourceManagerSyncObj);
                    lock (resourceManagerSyncObj)
                    {
                        if (!m_TriedLoadingResourceManager)
                        {
                            try
                            {
                                m_VBAResourceManager = new ResourceManager("Microsoft.VisualBasic", Assembly.GetExecutingAssembly());
                            }
                            catch (StackOverflowException exception)
                            {
                                throw exception;
                            }
                            catch (OutOfMemoryException exception2)
                            {
                                throw exception2;
                            }
                            catch (ThreadAbortException exception3)
                            {
                                throw exception3;
                            }
                            catch (Exception)
                            {
                            }
                            m_TriedLoadingResourceManager = true;
                        }
                    }
                }
                return m_VBAResourceManager;
            }
        }
#endif

        internal static Assembly VBRuntimeAssembly
        {
            get
            {
                if (m_VBRuntimeAssembly == null)
                {
                    m_VBRuntimeAssembly = Assembly.GetExecutingAssembly();
                }
                return m_VBRuntimeAssembly;
            }
        }

        private enum PropertyKind
        {
            ReadWrite,
            ReadOnly,
            WriteOnly
        }
    }
}

