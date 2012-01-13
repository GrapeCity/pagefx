namespace Microsoft.VisualBasic
{
    using Microsoft.VisualBasic.CompilerServices;
    using System;
    using System.Reflection;
    using System.Runtime.InteropServices;

    [StandardModule]
    public sealed class Information
    {
        public static int Erl()
        {
            return ProjectData.GetProjectData().m_Err.Erl;
        }

        public static ErrObject Err()
        {
            ProjectData projectData = ProjectData.GetProjectData();
            if (projectData.m_Err == null)
            {
                projectData.m_Err = new ErrObject();
            }
            return projectData.m_Err;
        }

        public static bool IsArray(object VarName)
        {
            if (VarName == null)
            {
                return false;
            }
            return (VarName is Array);
        }

        public static bool IsDate(object Expression)
        {
            if (Expression == null)
            {
                return false;
            }
            if (Expression is DateTime)
            {
                return true;
            }
            string str = Expression as string;
            DateTime time = default(DateTime);
            return ((str != null) && Conversions.TryParseDate(str, ref time));
        }

        public static bool IsDBNull(object Expression)
        {
            if (Expression == null)
            {
                return false;
            }
            return (Expression is DBNull);
        }

        public static bool IsError(object Expression)
        {
            if (Expression == null)
            {
                return false;
            }
            return (Expression is Exception);
        }

        public static bool IsNothing(object Expression)
        {
            return (Expression == null);
        }

        public static bool IsReference(object Expression)
        {
            return !(Expression is ValueType);
        }

        public static int LBound(Array Array, [Optional, DefaultParameterValue(1)] int Rank)
        {
            if (Array == null)
            {
                throw ExceptionUtils.VbMakeException(new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "Array" })), 9);
            }
            if ((Rank < 1) || (Rank > Array.Rank))
            {
                throw new RankException(Utils.GetResourceString("Argument_InvalidRank1", new string[] { "Rank" }));
            }
            return Array.GetLowerBound(Rank - 1);
        }

        public static int UBound(Array Array, [Optional, DefaultParameterValue(1)] int Rank)
        {
            if (Array == null)
            {
                throw ExceptionUtils.VbMakeException(new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "Array" })), 9);
            }
            if ((Rank < 1) || (Rank > Array.Rank))
            {
                throw new RankException(Utils.GetResourceString("Argument_InvalidRank1", new string[] { "Rank" }));
            }
            return Array.GetUpperBound(Rank - 1);
        }

        public static VariantType VarType(object VarName)
        {
            if (VarName == null)
            {
                return VariantType.Object;
            }
            return VarTypeFromComType(VarName.GetType());
        }

        internal static VariantType VarTypeFromComType(Type typ)
        {
            if (typ != null)
            {
                if (typ.IsArray)
                {
                    typ = typ.GetElementType();
                    if (typ.IsArray)
                    {
                        return (VariantType.Array | VariantType.Object);
                    }
                    VariantType type2 = VarTypeFromComType(typ);
                    if ((type2 & VariantType.Array) != VariantType.Empty)
                    {
                        return (VariantType.Array | VariantType.Object);
                    }
                    return (type2 | VariantType.Array);
                }
                if (typ.IsEnum)
                {
                    typ = Enum.GetUnderlyingType(typ);
                }
                if (typ == null)
                {
                    return VariantType.Empty;
                }
                switch (Type.GetTypeCode(typ))
                {
                    case TypeCode.DBNull:
                        return VariantType.Null;

                    case TypeCode.Boolean:
                        return VariantType.Boolean;

                    case TypeCode.Char:
                        return VariantType.Char;

                    case TypeCode.Byte:
                        return VariantType.Byte;

                    case TypeCode.Int16:
                        return VariantType.Short;

                    case TypeCode.Int32:
                        return VariantType.Integer;

                    case TypeCode.Int64:
                        return VariantType.Long;

                    case TypeCode.Single:
                        return VariantType.Single;

                    case TypeCode.Double:
                        return VariantType.Double;

                    case TypeCode.Decimal:
                        return VariantType.Decimal;

                    case TypeCode.DateTime:
                        return VariantType.Date;

                    case TypeCode.String:
                        return VariantType.String;
                }
                if (((typ == typeof(Missing)) || (typ == typeof(Exception))) || typ.IsSubclassOf(typeof(Exception)))
                {
                    return VariantType.Error;
                }
                if (typ.IsValueType)
                {
                    return VariantType.UserDefinedType;
                }
            }
            return VariantType.Object;
        }
    }
}

