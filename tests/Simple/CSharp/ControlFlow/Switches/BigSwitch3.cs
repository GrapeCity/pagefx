using System;
using System.Globalization;
using System.Text;

class BigSwitch3
{
    static string _ToString(string format, DateTimeFormatInfo dfi, DateTime value)
    {
        // the length of the format is usually a good guess of the number
        // of chars in the result. Might save us a few bytes sometimes
        // Add + 10 for cases like mmmm dddd
        StringBuilder result = new StringBuilder(format.Length + 10);

        // For some cases, the output should not use culture dependent calendar
        DateTimeFormatInfo inv = DateTimeFormatInfo.InvariantInfo;
        if (format == inv.RFC1123Pattern)
            dfi = inv;
        else if (format == inv.UniversalSortableDateTimePattern)
            dfi = inv;

        int i = 0;

        while (i < format.Length)
        {
            int tokLen;
            bool omitZeros = false;
            char ch = format[i];

            switch (ch)
            {

                //
                // Time Formats
                //
                case 'h':
                    // hour, [1, 12]
                    tokLen = CountRepeat(format, i, ch);

                    int hr = value.Hour % 12;
                    if (hr == 0)
                        hr = 12;

                    ZeroPad(result, hr, tokLen == 1 ? 1 : 2);
                    break;
                case 'H':
                    // hour, [0, 23]
                    tokLen = CountRepeat(format, i, ch);
                    ZeroPad(result, value.Hour, tokLen == 1 ? 1 : 2);
                    break;
                case 'm':
                    // minute, [0, 59]
                    tokLen = CountRepeat(format, i, ch);
                    ZeroPad(result, value.Minute, tokLen == 1 ? 1 : 2);
                    break;
                case 's':
                    // second [0, 29]
                    tokLen = CountRepeat(format, i, ch);
                    ZeroPad(result, value.Second, tokLen == 1 ? 1 : 2);
                    break;
#if NET_2_0
                case 'F':
                    omitZeros = true;
                    goto case 'f';
#endif
                case 'f':
                    // fraction of second, to same number of
                    // digits as there are f's

                    tokLen = CountRepeat(format, i, ch);
                    if (tokLen > 7)
                        throw new FormatException("Invalid Format String");

                    int dec = (int)((long)(value.Ticks % TimeSpan.TicksPerSecond) / (long)Math.Pow(10, 7 - tokLen));
                    int startLen = result.Length;
                    ZeroPad(result, dec, tokLen);

                    if (omitZeros)
                    {
                        while (result.Length > startLen && result[result.Length - 1] == '0')
                            result.Length--;
                        // when the value was 0, then trim even preceding '.' (!) It is fixed character.
                        if (dec == 0 && startLen > 0 && result[startLen - 1] == '.')
                            result.Length--;
                    }

                    break;
                case 't':
                    // AM/PM. t == first char, tt+ == full
                    tokLen = CountRepeat(format, i, ch);
                    string desig = value.Hour < 12 ? dfi.AMDesignator : dfi.PMDesignator;

                    if (tokLen == 1)
                    {
                        if (desig.Length >= 1)
                            result.Append(desig[0]);
                    }
                    else
                        result.Append(desig);

                    break;
                case 'z':
                    // timezone. t = +/-h; tt = +/-hh; ttt+=+/-hh:mm
                    tokLen = CountRepeat(format, i, ch);
                    TimeSpan offset = TimeZone.CurrentTimeZone.GetUtcOffset(value);

                    if (offset.Ticks >= 0)
                        result.Append('+');
                    else
                        result.Append('-');

                    switch (tokLen)
                    {
                        case 1:
                            result.Append(Math.Abs(offset.Hours));
                            break;
                        case 2:
                            result.Append(Math.Abs(offset.Hours).ToString("00"));
                            break;
                        default:
                            result.Append(Math.Abs(offset.Hours).ToString("00"));
                            result.Append(':');
                            result.Append(Math.Abs(offset.Minutes).ToString("00"));
                            break;
                    }
                    break;
#if NET_2_0
                case 'K': // 'Z' (UTC) or zzz (Local)
                    tokLen = 1;
                    switch (value.Kind)
                    {
                        case DateTimeKind.Utc:
                            result.Append('Z');
                            break;
                        case DateTimeKind.Local:
                            offset = TimeZone.CurrentTimeZone.GetUtcOffset(value);
                            if (offset.Ticks >= 0)
                                result.Append('+');
                            else
                                result.Append('-');
                            result.Append(Math.Abs(offset.Hours).ToString("00"));
                            result.Append(':');
                            result.Append(Math.Abs(offset.Minutes).ToString("00"));
                            break;
                    }
                    break;
#endif
                //
                // Date tokens
                //
                case 'd':
                    // day. d(d?) = day of month (leading 0 if two d's)
                    // ddd = three leter day of week
                    // dddd+ full day-of-week
                    tokLen = CountRepeat(format, i, ch);

                    if (tokLen <= 2)
                        ZeroPad(result, dfi.Calendar.GetDayOfMonth(value), tokLen == 1 ? 1 : 2);
                    else if (tokLen == 3)
                        result.Append(dfi.GetAbbreviatedDayName(dfi.Calendar.GetDayOfWeek(value)));
                    else
                        result.Append(dfi.GetDayName(dfi.Calendar.GetDayOfWeek(value)));

                    break;
                case 'M':
                    // Month.m(m?) = month # (with leading 0 if two mm)
                    // mmm = 3 letter name
                    // mmmm+ = full name
                    tokLen = CountRepeat(format, i, ch);
                    int month = dfi.Calendar.GetMonth(value);
                    if (tokLen <= 2)
                        ZeroPad(result, month, tokLen);
                    else if (tokLen == 3)
                        result.Append(dfi.GetAbbreviatedMonthName(month));
                    else
                        result.Append(dfi.GetMonthName(month));

                    break;
                case 'y':
                    // Year. y(y?) = two digit year, with leading 0 if yy
                    // yyy+ full year, if yyy and yr < 1000, displayed as three digits
                    tokLen = CountRepeat(format, i, ch);

                    if (tokLen <= 2)
                        ZeroPad(result, dfi.Calendar.GetYear(value) % 100, tokLen);
                    else
                        ZeroPad(result, dfi.Calendar.GetYear(value), (tokLen == 3 ? 3 : 4));

                    break;
                case 'g':
                    // Era name
                    tokLen = CountRepeat(format, i, ch);
                    result.Append(dfi.GetEraName(dfi.Calendar.GetEra(value)));
                    break;

                //
                // Other
                //
                case ':':
                    result.Append(dfi.TimeSeparator);
                    tokLen = 1;
                    break;
                case '/':
                    result.Append(dfi.DateSeparator);
                    tokLen = 1;
                    break;
                case '\'':
                case '"':
                    tokLen = ParseQuotedString(format, i, result);
                    break;
                case '%':
                    if (i >= format.Length - 1)
                        throw new FormatException("% at end of date time string");
                    if (format[i + 1] == '%')
                        throw new FormatException("%% in date string");

                    // Look for the next char
                    tokLen = 1;
                    break;
                case '\\':
                    // C-Style escape
                    if (i >= format.Length - 1)
                        throw new FormatException("\\ at end of date time string");

                    result.Append(format[i + 1]);
                    tokLen = 2;

                    break;
                default:
                    // catch all
                    result.Append(ch);
                    tokLen = 1;
                    break;
            }
            i += tokLen;
        }
        return result.ToString();
    }

    static int CountRepeat(string fmt, int p, char c)
    {
        int l = fmt.Length;
        int i = p + 1;
        while ((i < l) && (fmt[i] == c))
            i++;

        return i - p;
    }

    static int ParseQuotedString(string fmt, int pos, StringBuilder output)
    {
        // pos == position of " or '

        int len = fmt.Length;
        int start = pos;
        char quoteChar = fmt[pos++];

        while (pos < len)
        {
            char ch = fmt[pos++];

            if (ch == quoteChar)
                return pos - start;

            if (ch == '\\')
            {
                // C-Style escape
                if (pos >= len)
                    throw new FormatException("Un-ended quote");

                output.Append(fmt[pos++]);
            }
            else
            {
                output.Append(ch);
            }
        }

        throw new FormatException("Un-ended quote");
    }

    static void ZeroPad(StringBuilder output, int digits, int len)
    {
        // more than enough for an int
        char[] buffer = new char[16];
        int pos = 16;

        do
        {
            buffer[--pos] = (char)('0' + digits % 10);
            digits /= 10;
            len--;
        } while (digits > 0);

        while (len-- > 0)
            buffer[--pos] = '0';

        output.Append(new string(buffer, pos, 16 - pos));
    }

    static void f(DateTime value)
    {
        DateTimeFormatInfo dfi = DateTimeFormatInfo.InvariantInfo;
        string[] pats = dfi.GetAllDateTimePatterns();
        for (int i = 0; i < pats.Length; ++i)
        {
            string s = _ToString(pats[i], dfi, value);
            Console.WriteLine(s);
        }
    }

    static void Main()
    {
        f(DateTime.Now);
        Console.WriteLine("<%END%>");
    }
}