namespace Microsoft.VisualBasic
{
    using Microsoft.VisualBasic.CompilerServices;
    using System;
    using System.Threading;

    [StandardModule]
    public sealed class Conversion
    {
        private const int LCID_US_ENGLISH = 0x409;
        private const int LOCALE_NOUSEROVERRIDE = -2147483648;
        private const int MAX_ERR_NUMBER = 0xffff;
        private const int NUMPRS_CURRENCY = 0x400;
        private const int NUMPRS_DECIMAL = 0x100;
        private const int NUMPRS_EXPONENT = 0x800;
        private const int NUMPRS_HEX_OCT = 0x40;
        private const int NUMPRS_INEXACT = 0x20000;
        private const int NUMPRS_LEADING_MINUS = 0x10;
        private const int NUMPRS_LEADING_PLUS = 4;
        private const int NUMPRS_LEADING_WHITE = 1;
        private const int NUMPRS_NEG = 0x10000;
        private const int NUMPRS_PARENS = 0x80;
        private const int NUMPRS_STD = 0x1fff;
        private const int NUMPRS_THOUSANDS = 0x200;
        private const int NUMPRS_TRAILING_MINUS = 0x20;
        private const int NUMPRS_TRAILING_PLUS = 8;
        private const int NUMPRS_TRAILING_WHITE = 2;
        private const int NUMPRS_USE_ALL = 0x1000;
        private const int PRSFLAGS = 0x954;
        private const char TYPE_INDICATOR_DECIMAL = '@';
        private const char TYPE_INDICATOR_INT16 = '%';
        private const char TYPE_INDICATOR_INT32 = '&';
        private const char TYPE_INDICATOR_SINGLE = '!';
        private const int VTBIT_BOOL = 0x800;
        private const int VTBIT_BSTR = 0x100;
        private const int VTBIT_BYTE = 0x20000;
        private const int VTBIT_CHAR = 0x40000;
        private const int VTBIT_CY = 0x40;
        private const int VTBIT_DATAOBJECT = 0x2000;
        private const int VTBIT_DATE = 0x80;
        private const int VTBIT_DECIMAL = 0x4000;
        private const int VTBIT_EMPTY = 0;
        private const int VTBIT_ERROR = 0x400;
        private const int VTBIT_I2 = 4;
        private const int VTBIT_I4 = 8;
        private const int VTBIT_LONG = 0x100000;
        private const int VTBIT_NULL = 2;
        private const int VTBIT_OBJECT = 0x200;
        private const int VTBIT_R4 = 0x10;
        private const int VTBIT_R8 = 0x20;
        private const int VTBIT_VARIANT = 0x1000;
        private const int VTBITS = 0x402c;

        public static string ErrorToString()
        {
            return Information.Err().Description;
        }

        public static string ErrorToString(int ErrorNumber)
        {
            if (ErrorNumber >= 0xffff)
            {
                throw new ArgumentException(Utils.GetResourceString("MaxErrNumber"));
            }
            if (ErrorNumber > 0)
            {
                ErrorNumber = -2146828288 | ErrorNumber;
            }
            if ((ErrorNumber & 0x1fff0000) == 0xa0000)
            {
                ErrorNumber &= 0xffff;
                return Utils.GetResourceString((vbErrors) ErrorNumber);
            }
            if (ErrorNumber != 0)
            {
                return Utils.GetResourceString(vbErrors.UserDefined);
            }
            return "";
        }

        public static decimal Fix(decimal Number)
        {
            if (Number < decimal.Zero)
            {
                return decimal.Negate(decimal.Floor(decimal.Negate(Number)));
            }
            return decimal.Floor(Number);
        }

        public static double Fix(double Number)
        {
            if (Number >= 0.0)
            {
                return Math.Floor(Number);
            }
            return -Math.Floor(-Number);
        }

        public static short Fix(short Number)
        {
            return Number;
        }

        public static int Fix(int Number)
        {
            return Number;
        }

        public static long Fix(long Number)
        {
            return Number;
        }

        public static object Fix(object Number)
        {
            if (Number == null)
            {
                throw new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "Number" }));
            }
            IConvertible convertible = Number as IConvertible;
            if (convertible != null)
            {
                switch (convertible.GetTypeCode())
                {
                    case TypeCode.Boolean:
                        return convertible.ToInt32(null);

                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                        return Number;

                    case TypeCode.Single:
                        return Fix(convertible.ToSingle(null));

                    case TypeCode.Double:
                        return Fix(convertible.ToDouble(null));

                    case TypeCode.Decimal:
                        return Fix(convertible.ToDecimal(null));

                    case TypeCode.String:
                        return Fix(Conversions.ToDouble(convertible.ToString(null)));
                }
            }
            throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_NotNumericType2", new string[] { "Number", Number.GetType().FullName })), 13);
        }

        public static float Fix(float Number)
        {
            if (Number >= 0f)
            {
                return (float) Math.Floor((double) Number);
            }
            return (float) -Math.Floor((double) -Number);
        }

        public static string Hex(byte Number)
        {
            return Number.ToString("X");
        }

        public static string Hex(short Number)
        {
            return Number.ToString("X");
        }

        public static string Hex(int Number)
        {
            return Number.ToString("X");
        }

        public static string Hex(long Number)
        {
            return Number.ToString("X");
        }

        public static string Hex(object Number)
        {
            long num;
            if (Number == null)
            {
                throw new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "Number" }));
            }
            IConvertible convertible = Number as IConvertible;
            if (convertible != null)
            {
                switch (convertible.GetTypeCode())
                {
                    case TypeCode.Boolean:
                    case TypeCode.Char:
                    case TypeCode.DateTime:
                    case (TypeCode.DateTime | TypeCode.Object):
                        goto Label_013D;

                    case TypeCode.SByte:
                        return Hex(convertible.ToSByte(null));

                    case TypeCode.Byte:
                        return Hex(convertible.ToByte(null));

                    case TypeCode.Int16:
                        return Hex(convertible.ToInt16(null));

                    case TypeCode.UInt16:
                        return Hex(convertible.ToUInt16(null));

                    case TypeCode.Int32:
                        return Hex(convertible.ToInt32(null));

                    case TypeCode.UInt32:
                        return Hex(convertible.ToUInt32(null));

                    case TypeCode.Int64:
                    case TypeCode.Single:
                    case TypeCode.Double:
                    case TypeCode.Decimal:
                        num = convertible.ToInt64(null);
                        goto Label_010E;

                    case TypeCode.UInt64:
                        return Hex(convertible.ToUInt64(null));

                    case TypeCode.String:
                        try
                        {
                            num = Conversions.ToLong(convertible.ToString(null));
                        }
                        catch (OverflowException)
                        {
                            return Hex(Conversions.ToULong(convertible.ToString(null)));
                        }
                        goto Label_010E;
                }
            }
            goto Label_013D;
        Label_010E:
            if (num == 0L)
            {
                return "0";
            }
            if ((num <= 0L) && (num >= -2147483648L))
            {
                return Hex((int) num);
            }
            return Hex(num);
        Label_013D:;
            throw new ArgumentException(Utils.GetResourceString("Argument_InvalidValueType2", new string[] { "Number", Utils.VBFriendlyName(Number) }));
        }

        [CLSCompliant(false)]
        public static string Hex(sbyte Number)
        {
            return Number.ToString("X");
        }

        [CLSCompliant(false)]
        public static string Hex(ushort Number)
        {
            return Number.ToString("X");
        }

        [CLSCompliant(false)]
        public static string Hex(uint Number)
        {
            return Number.ToString("X");
        }

        [CLSCompliant(false)]
        public static string Hex(ulong Number)
        {
            return Number.ToString("X");
        }

        private static double HexOrOctValue(string InputStr, int i)
        {
            int num2;
            long num5 = 0;
            int num = 0;
            int length = InputStr.Length;
            char ch = InputStr[i];
            i++;
            if ((ch != 'H') && (ch != 'h'))
            {
                if ((ch != 'O') && (ch != 'o'))
                {
                    return 0.0;
                }
                while ((i < length) && (num < 0x16))
                {
                    ch = InputStr[i];
                    i++;
                    char ch3 = ch;
                    if ((((ch3 != '\t') && (ch3 != '\n')) && (ch3 != '\r')) && ((ch3 != ' ') && (ch3 != '　')))
                    {
                        if (ch3 == '0')
                        {
                            if (num == 0)
                            {
                                continue;
                            }
                            num2 = 0;
                        }
                        else
                        {
                            if ((ch3 < '1') || (ch3 > '7'))
                            {
                                break;
                            }
                            num2 = ch - '0';
                        }
                        if (num5 >= 0x1000000000000000L)
                        {
                            num5 = (num5 & 0xfffffffffffffffL) * 8L;
                            num5 |= 0x1000000000000000L;
                        }
                        else
                        {
                            num5 *= 8L;
                        }
                        num5 += num2;
                        num++;
                    }
                }
            }
            else
            {
                while ((i < length) && (num < 0x11))
                {
                    ch = InputStr[i];
                    i++;
                    char ch2 = ch;
                    if ((((ch2 != '\t') && (ch2 != '\n')) && ((ch2 != '\r') && (ch2 != ' '))) && (ch2 != '　'))
                    {
                        if (ch2 == '0')
                        {
                            if (num == 0)
                            {
                                continue;
                            }
                            num2 = 0;
                        }
                        else if ((ch2 >= '1') && (ch2 <= '9'))
                        {
                            num2 = ch - '0';
                        }
                        else if ((ch2 >= 'A') && (ch2 <= 'F'))
                        {
                            num2 = ch - '7';
                        }
                        else
                        {
                            if ((ch2 < 'a') || (ch2 > 'f'))
                            {
                                break;
                            }
                            num2 = ch - 'W';
                        }
                        if ((num == 15) && (num5 > 0x7ffffffffffffffL))
                        {
                            num5 = (num5 & 0x7ffffffffffffffL) * 0x10L;
                            num5 |= -9223372036854775808L;
                        }
                        else
                        {
                            num5 *= 0x10L;
                        }
                        num5 += num2;
                        num++;
                    }
                }
                if (num == 0x10)
                {
                    i++;
                    if (i < length)
                    {
                        ch = InputStr[i];
                    }
                }
                if (num <= 8)
                {
                    if ((num > 4) || (ch == '&'))
                    {
                        if (num5 > 0x7fffffffL)
                        {
                            num5 = -2147483648L + (num5 & 0x7fffffffL);
                        }
                    }
                    else if (((num > 2) || (ch == '%')) && (num5 > 0x7fffL))
                    {
                        num5 = -32768L + (num5 & 0x7fffL);
                    }
                }
                switch (ch)
                {
                    case '%':
                        num5 = (short) num5;
                        break;

                    case '&':
                        num5 = (int) num5;
                        break;
                }
                return (double) num5;
            }
            if (num == 0x16)
            {
                i++;
                if (i < length)
                {
                    ch = InputStr[i];
                }
            }
            if (num5 <= 0x100000000L)
            {
                if ((num5 > 0xffffL) || (ch == '&'))
                {
                    if (num5 > 0x7fffffffL)
                    {
                        num5 = -2147483648L + (num5 & 0x7fffffffL);
                    }
                }
                else if (((num5 > 0xffL) || (ch == '%')) && (num5 > 0x7fffL))
                {
                    num5 = -32768L + (num5 & 0x7fffL);
                }
            }
            switch (ch)
            {
                case '%':
                    num5 = (short) num5;
                    break;

                case '&':
                    num5 = (int) num5;
                    break;
            }
            return (double) num5;
        }

        public static decimal Int(decimal Number)
        {
            return decimal.Floor(Number);
        }

        public static double Int(double Number)
        {
            return Math.Floor(Number);
        }

        public static short Int(short Number)
        {
            return Number;
        }

        public static int Int(int Number)
        {
            return Number;
        }

        public static long Int(long Number)
        {
            return Number;
        }

        public static object Int(object Number)
        {
            if (Number == null)
            {
                throw new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "Number" }));
            }
            IConvertible convertible = Number as IConvertible;
            if (convertible != null)
            {
                switch (convertible.GetTypeCode())
                {
                    case TypeCode.Boolean:
                        return convertible.ToInt32(null);

                    case TypeCode.SByte:
                    case TypeCode.Byte:
                    case TypeCode.Int16:
                    case TypeCode.UInt16:
                    case TypeCode.Int32:
                    case TypeCode.UInt32:
                    case TypeCode.Int64:
                    case TypeCode.UInt64:
                        return Number;

                    case TypeCode.Single:
                        return Int(convertible.ToSingle(null));

                    case TypeCode.Double:
                        return Int(convertible.ToDouble(null));

                    case TypeCode.Decimal:
                        return Int(convertible.ToDecimal(null));

                    case TypeCode.String:
                        return Int(Conversions.ToDouble(convertible.ToString(null)));
                }
            }
            throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_NotNumericType2", new string[] { "Number", Number.GetType().FullName })), 13);
        }

        public static float Int(float Number)
        {
            return (float) Math.Floor((double) Number);
        }

        public static string Str(object Number)
        {
            string str;
            if (Number == null)
            {
                throw new ArgumentNullException(Utils.GetResourceString("Argument_InvalidNullValue1", new string[] { "Number" }));
            }
            IConvertible convertible = Number as IConvertible;
            if (convertible == null)
            {
                throw new InvalidCastException(Utils.GetResourceString("ArgumentNotNumeric1", new string[] { "Number" }));
            }
            TypeCode typeCode = convertible.GetTypeCode();
            switch (typeCode)
            {
                case TypeCode.DBNull:
                    return "Null";

                case TypeCode.Boolean:
                    if (convertible.ToBoolean(null))
                    {
                        return "True";
                    }
                    return "False";

                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                case TypeCode.Single:
                case TypeCode.Double:
                case TypeCode.Decimal:
                    str = Conversions.ToString(Number);
                    break;

                default:
                    if (typeCode == TypeCode.String)
                    {
                        try
                        {
                            str = Conversions.ToString(Conversions.ToDouble(convertible.ToString(null)));
                            break;
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
                    }
                    throw new InvalidCastException(Utils.GetResourceString("ArgumentNotNumeric1", new string[] { "Number" }));
            }
            if ((str.Length > 0) && (str[0] != '-'))
            {
                return (" " + Utils.StdFormat(str));
            }
            return Utils.StdFormat(str);
        }

        public static int Val(char Expression)
        {
            int num = Expression;
            if ((num >= 0x31) && (num <= 0x39))
            {
                return (num - 0x30);
            }
            return 0;
        }

        public static double Val(object Expression)
        {
            string str2;
            string inputStr = Expression as string;
            if (inputStr != null)
            {
                return Val(inputStr);
            }
            if (Expression is char)
            {
                return (double) Val((char) Expression);
            }
            if (Versioned.IsNumeric(Expression))
            {
                return Conversions.ToDouble(Expression);
            }
            try
            {
                str2 = Conversions.ToString(Expression);
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
                throw ExceptionUtils.VbMakeException(new ArgumentException(Utils.GetResourceString("Argument_InvalidValueType2", new string[] { "Expression", Utils.VBFriendlyName(Expression) })), 0x1b6);
            }
            return Val(str2);
        }

        public static double Val(string InputStr)
        {
            char ch;
            int num = 0;
            int num2 = 0;
            int num3 = 0;
            int length;
            double num8 = 0;
            if (InputStr == null)
            {
                length = 0;
            }
            else
            {
                length = InputStr.Length;
            }
            int num4 = 0;
            while (num4 < length)
            {
                ch = InputStr[num4];
                switch (ch)
                {
                    case '\t':
                    case '\n':
                    case '\r':
                    case ' ':
                        break;
                }
                num4++;
            }
            if (num4 >= length)
            {
                return 0.0;
            }
            ch = InputStr[num4];
            if (ch == '&')
            {
                return HexOrOctValue(InputStr, num4 + 1);
            }
            bool flag = false;
            bool flag2 = false;
            bool flag3 = false;
            double y = 0.0;
            ch = InputStr[num4];
            switch (ch)
            {
                case '-':
                    flag3 = true;
                    num4++;
                    break;

                case '+':
                    num4++;
                    break;
            }
            while (num4 < length)
            {
                ch = InputStr[num4];
                char ch3 = ch;
                if ((((ch3 == '\t') || (ch3 == '\n')) || (ch3 == '\r')) || ((ch3 == ' ') || (ch3 == '　')))
                {
                    num4++;
                }
                else
                {
                    if (ch3 == '0')
                    {
                        if ((num != 0) || flag)
                        {
                            num8 = ((num8 * 10.0) + ((double) ch)) - 48.0;
                            num4++;
                            num++;
                        }
                        else
                        {
                            num4++;
                        }
                        continue;
                    }
                    if ((ch3 >= '1') && (ch3 <= '9'))
                    {
                        num8 = ((num8 * 10.0) + ((double) ch)) - 48.0;
                        num4++;
                        num++;
                    }
                    else
                    {
                        if (ch3 == '.')
                        {
                            num4++;
                            if (flag)
                            {
                                break;
                            }
                            flag = true;
                            num3 = num;
                            continue;
                        }
                        if ((((ch3 == 'e') || (ch3 == 'E')) || (ch3 == 'd')) || (ch3 == 'D'))
                        {
                            flag2 = true;
                            num4++;
                        }
                        break;
                    }
                }
            }
            if (flag)
            {
                num2 = num - num3;
            }
            if (!flag2)
            {
                if (flag && (num2 != 0))
                {
                    num8 /= Math.Pow(10.0, (double) num2);
                }
            }
            else
            {
                bool flag4 = false;
                bool flag5 = false;
                while (num4 < length)
                {
                    ch = InputStr[num4];
                    char ch4 = ch;
                    if ((((ch4 == '\t') || (ch4 == '\n')) || (ch4 == '\r')) || ((ch4 == ' ') || (ch4 == '　')))
                    {
                        num4++;
                    }
                    else if ((ch4 >= '0') && (ch4 <= '9'))
                    {
                        y = ((y * 10.0) + ((double) ch)) - 48.0;
                        num4++;
                    }
                    else
                    {
                        if (ch4 == '+')
                        {
                            if (flag4)
                            {
                                break;
                            }
                            flag4 = true;
                            num4++;
                            continue;
                        }
                        if ((ch4 != '-') || flag4)
                        {
                            break;
                        }
                        flag4 = true;
                        flag5 = true;
                        num4++;
                    }
                }
                if (flag5)
                {
                    y += num2;
                    num8 *= Math.Pow(10.0, -y);
                }
                else
                {
                    y -= num2;
                    num8 *= Math.Pow(10.0, y);
                }
            }
            if (double.IsInfinity(num8))
            {
                throw ExceptionUtils.VbMakeException(6);
            }
            if (flag3)
            {
                num8 = -num8;
            }
            switch (ch)
            {
                case '%':
                    if (num2 > 0)
                    {
                        throw ExceptionUtils.VbMakeException(13);
                    }
                    return (double) ((short) Math.Round(num8));

                case '&':
                    if (num2 > 0)
                    {
                        throw ExceptionUtils.VbMakeException(13);
                    }
                    return (double) ((int) Math.Round(num8));

                case '!':
                    return (double) ((float) num8);

                case '@':
                    return Convert.ToDouble(new decimal(num8));
            }
            return num8;
        }
    }
}

