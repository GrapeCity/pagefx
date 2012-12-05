using System;
using System.Globalization;
using System.Text;

class CustomInfo
{
    public bool UseGroup = false;
    public int DecimalDigits = 0;
    public int DecimalPointPos = -1;
    public int DecimalTailSharpDigits = 0;
    public int IntegerDigits = 0;
    public int IntegerHeadSharpDigits = 0;
    public int IntegerHeadPos = 0;
    public bool UseExponent = false;
    public int ExponentDigits = 0;
    public int ExponentTailSharpDigits = 0;
    public bool ExponentNegativeSignOnly = true;
    public int DividePlaces = 0;
    public int Percents = 0;
    public int Permilles = 0;

    public static CustomInfo Parse(string format, int offset, int length, NumberFormatInfo nfi)
    {
        char literal = '\0';
        bool integerArea = true;
        bool decimalArea = false;
        bool exponentArea = false;
        bool sharpContinues = true;

        CustomInfo info = new CustomInfo();
        int groupSeparatorCounter = 0;

        for (int i = offset; i - offset < length; i++)
        {
            char c = format[i];

            if (c == literal && c != '\0')
            {
                literal = '\0';
                continue;
            }
            if (literal != '\0')
                continue;

            if (exponentArea && (c != '\0' && c != '0' && c != '#'))
            {
                exponentArea = false;
                integerArea = (info.DecimalPointPos < 0);
                decimalArea = !integerArea;
                i--;
                continue;
            }

            switch (c)
            {
                case '\\':
                    i++;
                    continue;
                case '\'':
                case '\"':
                    if (c == '\"' || c == '\'')
                    {
                        literal = c;
                    }
                    continue;
                case '#':
                    if (sharpContinues && integerArea)
                        info.IntegerHeadSharpDigits++;
                    else if (decimalArea)
                        info.DecimalTailSharpDigits++;
                    else if (exponentArea)
                        info.ExponentTailSharpDigits++;

                    goto case '0';
                case '0':
                    if (c != '#')
                    {
                        sharpContinues = false;
                        if (decimalArea)
                            info.DecimalTailSharpDigits = 0;
                        else if (exponentArea)
                            info.ExponentTailSharpDigits = 0;
                    }
                    if (info.IntegerHeadPos == -1)
                        info.IntegerHeadPos = i;

                    if (integerArea)
                    {
                        info.IntegerDigits++;
                        if (groupSeparatorCounter > 0)
                            info.UseGroup = true;
                        groupSeparatorCounter = 0;
                    }
                    else if (decimalArea)
                    {
                        info.DecimalDigits++;
                    }
                    else if (exponentArea)
                    {
                        info.ExponentDigits++;
                    }
                    break;
                case 'e':
                case 'E':
                    if (info.UseExponent)
                        break;

                    info.UseExponent = true;
                    integerArea = false;
                    decimalArea = false;
                    exponentArea = true;
                    if (i + 1 - offset < length)
                    {
                        char nc = format[i + 1];
                        if (nc == '+')
                            info.ExponentNegativeSignOnly = false;
                        if (nc == '+' || nc == '-')
                        {
                            i++;
                        }
                        else if (nc != '0' && nc != '#')
                        {
                            info.UseExponent = false;
                            if (info.DecimalPointPos < 0)
                                integerArea = true;
                        }
                        c = '\0';
                    }

                    break;
                case '.':
                    integerArea = false;
                    decimalArea = true;
                    exponentArea = false;
                    if (info.DecimalPointPos == -1)
                        info.DecimalPointPos = i;
                    break;
                case '%':
                    info.Percents++;
                    break;
                case '\u2030':
                    info.Permilles++;
                    break;
                case ',':
                    if (integerArea && info.IntegerDigits > 0)
                        groupSeparatorCounter++;
                    break;
                default:
                    break;
            }
        }

        if (info.ExponentDigits == 0)
            info.UseExponent = false;
        else
            info.IntegerHeadSharpDigits = 0;

        if (info.DecimalDigits == 0)
            info.DecimalPointPos = -1;

        info.DividePlaces += groupSeparatorCounter * 3;

        return info;
    }

    //public string Format(string format, int offset, int length, NumberFormatInfo nfi, bool positive, StringBuilder sb_int, StringBuilder sb_dec, StringBuilder sb_exp)
    //{
    //    StringBuilder sb = new StringBuilder();
    //    char literal = '\0';
    //    bool integerArea = true;
    //    bool decimalArea = false;
    //    int intSharpCounter = 0;
    //    int sb_int_index = 0;
    //    int sb_dec_index = 0;

    //    int[] groups = nfi.NumberGroupSizes;
    //    string groupSeparator = nfi.NumberGroupSeparator;
    //    int intLen = 0, total = 0, groupIndex = 0, counter = 0, groupSize = 0, fraction = 0;
    //    if (UseGroup && groups.Length > 0)
    //    {
    //        intLen = sb_int.Length;
    //        for (int i = 0; i < groups.Length; i++)
    //        {
    //            total += groups[i];
    //            if (total <= intLen)
    //                groupIndex = i;
    //        }
    //        groupSize = groups[groupIndex];
    //        fraction = intLen > total ? intLen - total : 0;
    //        if (groupSize == 0)
    //        {
    //            while (groupIndex >= 0 && groups[groupIndex] == 0)
    //                groupIndex--;

    //            groupSize = fraction > 0 ? fraction : groups[groupIndex];
    //        }
    //        if (fraction == 0)
    //        {
    //            counter = groupSize;
    //        }
    //        else
    //        {
    //            groupIndex += fraction / groupSize;
    //            counter = fraction % groupSize;
    //            if (counter == 0)
    //                counter = groupSize;
    //            else
    //                groupIndex++;
    //        }
    //    }
    //    else
    //    {
    //        UseGroup = false;
    //    }

    //    for (int i = offset; i - offset < length; i++)
    //    {
    //        char c = format[i];

    //        if (c == literal && c != '\0')
    //        {
    //            literal = '\0';
    //            continue;
    //        }
    //        if (literal != '\0')
    //        {
    //            sb.Append(c);
    //            continue;
    //        }

    //        switch (c)
    //        {
    //            case '\\':
    //                i++;
    //                if (i - offset < length)
    //                    sb.Append(format[i]);
    //                continue;
    //            case '\'':
    //            case '\"':
    //                if (c == '\"' || c == '\'')
    //                {
    //                    literal = c;
    //                }
    //                continue;
    //            case '#':
    //                goto case '0';
    //            case '0':
    //                if (integerArea)
    //                {
    //                    intSharpCounter++;
    //                    if (IntegerDigits - intSharpCounter < sb_int.Length + sb_int_index || c == '0')
    //                        while (IntegerDigits - intSharpCounter + sb_int_index < sb_int.Length)
    //                        {
    //                            sb.Append(sb_int[sb_int_index++]);
    //                            if (UseGroup && --intLen > 0 && --counter == 0)
    //                            {
    //                                sb.Append(groupSeparator);
    //                                if (--groupIndex < groups.Length && groupIndex >= 0)
    //                                    groupSize = groups[groupIndex];
    //                                counter = groupSize;
    //                            }
    //                        }
    //                    break;
    //                }
    //                else if (decimalArea)
    //                {
    //                    if (sb_dec_index < sb_dec.Length)
    //                        sb.Append(sb_dec[sb_dec_index++]);
    //                    break;
    //                }

    //                sb.Append(c);
    //                break;
    //            case 'e':
    //            case 'E':
    //                if (sb_exp == null || !UseExponent)
    //                {
    //                    sb.Append(c);
    //                    break;
    //                }

    //                bool flag1 = true;
    //                bool flag2 = false;

    //                int q;
    //                for (q = i + 1; q - offset < length; q++)
    //                {
    //                    if (format[q] == '0')
    //                    {
    //                        flag2 = true;
    //                        continue;
    //                    }
    //                    if (q == i + 1 && (format[q] == '+' || format[q] == '-'))
    //                    {
    //                        continue;
    //                    }
    //                    if (!flag2)
    //                        flag1 = false;
    //                    break;
    //                }

    //                if (flag1)
    //                {
    //                    i = q - 1;
    //                    integerArea = (DecimalPointPos < 0);
    //                    decimalArea = !integerArea;

    //                    sb.Append(c);
    //                    sb.Append(sb_exp);
    //                    sb_exp = null;
    //                }
    //                else
    //                    sb.Append(c);

    //                break;
    //            case '.':
    //                if (DecimalPointPos == i)
    //                {
    //                    if (DecimalDigits > 0)
    //                    {
    //                        while (sb_int_index < sb_int.Length)
    //                            sb.Append(sb_int[sb_int_index++]);
    //                    }
    //                    if (sb_dec.Length > 0)
    //                        sb.Append(nfi.NumberDecimalSeparator);
    //                }
    //                integerArea = false;
    //                decimalArea = true;
    //                break;
    //            case ',':
    //                break;
    //            case '%':
    //                sb.Append(nfi.PercentSymbol);
    //                break;
    //            case '\u2030':
    //                sb.Append(nfi.PerMilleSymbol);
    //                break;
    //            default:
    //                sb.Append(c);
    //                break;
    //        }
    //    }

    //    if (!positive)
    //        sb.Insert(0, nfi.NegativeSign);

    //    return sb.ToString();
    //}
}

class BigSwitch2
{
    static void f(string format)
    {
        CustomInfo ci = CustomInfo.Parse(format, 0, format.Length, CultureInfo.InvariantCulture.NumberFormat);
        Console.WriteLine(ci.DecimalDigits);
        Console.WriteLine(ci.DecimalPointPos);
        Console.WriteLine(ci.DecimalTailSharpDigits);
        Console.WriteLine(ci.DividePlaces);
        Console.WriteLine(ci.ExponentDigits);
        Console.WriteLine(ci.ExponentNegativeSignOnly);
        Console.WriteLine(ci.ExponentNegativeSignOnly);
        Console.WriteLine(ci.ExponentTailSharpDigits);
        Console.WriteLine(ci.IntegerDigits);
        Console.WriteLine(ci.IntegerHeadPos);
        Console.WriteLine(ci.IntegerHeadSharpDigits);
        Console.WriteLine(ci.Percents);
        Console.WriteLine(ci.Permilles);
        Console.WriteLine(ci.UseExponent);
    }

    static void Main()
    {
        f("0.###");
        f("\\\'aaa\\\'0.###");
        Console.WriteLine("<%END%>");
    }
}