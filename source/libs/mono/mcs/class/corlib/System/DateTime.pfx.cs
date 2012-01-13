//CHANGED

//
// System.DateTime.cs
//
// author:
//   Marcel Narings (marcel@narings.nl)
//   Martin Baulig (martin@gnome.org)
//   Atsushi Enomoto (atsushi@ximian.com)
//
//   (C) 2001 Marcel Narings
// Copyright (C) 2004-2006 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Collections;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using Avm;

namespace System
{
    /// <summary>
    /// The DateTime structure represents dates and time ranging from
    /// 1-1-0001 12:00:00 AM to 31-12-9999 23:59:00 Common Era.
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Auto)]
    public struct DateTime : IFormattable, IConvertible, IComparable
#if NET_2_0
		, IComparable<DateTime>, IEquatable <DateTime>
#endif
    {
        public Avm.Date m_value;

#if NET_2_0
		DateTimeKind kind;
#endif

        private const int dp400 = 146097;
        private const int dp100 = 36524;
        private const int dp4 = 1461;

        // w32 file time starts counting from 1/1/1601 00:00 GMT
        // which is the constant ticks from the .NET epoch
        private const long w32file_epoch = 504911232000000000L;

        //private const long MAX_VALUE_TICKS = 3155378975400000000L;
        // -- Microsoft .NET has this value.
        private const long MAX_VALUE_TICKS = 3155378975999999999L;

        //
        // The UnixEpoch, it begins on Jan 1, 1970 at 0:0:0, expressed
        // in Ticks
        //
        internal const long UnixEpoch = 621355968000000000L;

        // for OLE Automation dates
        private const long ticks18991230 = 599264352000000000L;
        private const double OAMinValue = -657435.0d;
        private const double OAMaxValue = 2958466.0d;

        public static readonly DateTime MaxValue = new DateTime(false, new TimeSpan(MAX_VALUE_TICKS));
        public static readonly DateTime MinValue = new DateTime(false, new TimeSpan(0));

        private static readonly string[] commonFormats = {
			// For compatibility with MS's CLR, this format (which
			// doesn't have a one-letter equivalent) is parsed
			// too. It's important because it's used in XML
			// serialization.

			// Note that those format should be tried only for
			// invalid patterns; 

			// FIXME: SOME OF those patterns looks tried against 
			// the current culture, since some patterns fail in 
			// some culture.

			"yyyy-MM-dd",
			"yyyy-MM-ddTHH:mm:sszzz",
			"yyyy-MM-ddTHH:mm:ss.fffffff",
			"yyyy-MM-ddTHH:mm:ss.fffffffzzz",
			// bug #78618
			"yyyy-M-d H:m:s.fffffff",
			// UTC / allow any separator
			"yyyy/MM/ddTHH:mm:ssZ",
			"yyyy/M/dZ",
			// bug #58938
			"yyyy/M/d HH:mm:ss",
			// bug #47720
			"yyyy/MM/dd HH:mm:ss 'GMT'",
			// bug #53023
			"MM/dd/yyyy",
			// Close to RFC1123, but without 'GMT'
			"ddd, d MMM yyyy HH:mm:ss",
			// use UTC ('Z'; not literal "'Z'")
			// FIXME: 1078(af-ZA) and 1079(ka-GE) reject it
			"yyyy/MM/dd HH':'mm':'ssZ", 

			// bug #60912
			"M/d/yyyy HH':'mm':'ss tt",
			"H':'mm':'ss tt",
			// another funky COM dependent one
			"dd-MMM-yy",

			// DayOfTheWeek, dd full_month_name yyyy
			// FIXME: 1054(th-TH) rejects them
			"dddd, dd MMMM yyyy",
			"dddd, dd MMMM yyyy HH:mm",
			"dddd, dd MMMM yyyy HH:mm:ss",

			"yyyy MMMM",
			// DayOfTheWeek, dd yyyy. This works for every locales.
			"MMMM dd, yyyy",
#if NET_1_1
			// X509Certificate pattern is accepted by Parse() *in every culture*
			"yyyyMMddHHmmssZ",
#endif
			// In Parse() the 'r' equivalent pattern is first parsed as universal time
			"ddd, dd MMM yyyy HH':'mm':'ss 'GMT'",

			// Additionally there seems language-specific format
			// patterns that however works in all language
			// environment.
			// For example, the pattern below is for Japanese.
			"yyyy'\u5E74'MM'\u6708'dd'\u65E5' HH'\u6642'mm'\u5206'ss'\u79D2'",

			// This one is parsed for all cultures
			"HH':'mm tt MM/dd/yyyy",

/*
			// Full date and time
			"F", "G", "r", "s", "u", "U",
			// Full date and time, but no seconds
			"f", "g",
			// Only date
			"d", "D",
			// Only time
			"T", "t",
			// Only date, but no year
			"m",
			// Only date, but no day
			"y" 
*/
		};

        private enum Which
        {
            Day,
            DayYear,
            Month,
            Year
        };

        private static readonly int[] daysmonth = { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        private static readonly int[] daysmonthleap = { 0, 31, 29, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

        private static int AbsoluteDays(int year, int month, int day)
        {
            int[] days;
            int temp = 0, m = 1;

            days = (IsLeapYear(year) ? daysmonthleap : daysmonth);

            while (m < month)
                temp += days[m++];
            return ((day - 1) + temp + (365 * (year - 1)) + ((year - 1) / 4) - ((year - 1) / 100) + ((year - 1) / 400));
        }

        //private int FromTicks(Which what)
        //{
        //    int num400, num100, num4, numyears;
        //    int M = 1;

        //    int[] days = daysmonth;
        //    int totaldays = this.ticks.Days;

        //    num400 = (totaldays / dp400);
        //    totaldays -= num400 * dp400;

        //    num100 = (totaldays / dp100);
        //    if (num100 == 4)   // leap
        //        num100 = 3;
        //    totaldays -= (num100 * dp100);

        //    num4 = totaldays / dp4;
        //    totaldays -= (num4 * dp4);

        //    numyears = totaldays / 365;

        //    if (numyears == 4)  //leap
        //        numyears = 3;
        //    if (what == Which.Year)
        //        return num400 * 400 + num100 * 100 + num4 * 4 + numyears + 1;

        //    totaldays -= (numyears * 365);
        //    if (what == Which.DayYear)
        //        return totaldays + 1;

        //    if ((numyears == 3) && ((num100 == 3) || !(num4 == 24))) //31 dec leapyear
        //        days = daysmonthleap;

        //    while (totaldays >= days[M])
        //        totaldays -= days[M++];

        //    if (what == Which.Month)
        //        return M;

        //    return totaldays + 1;
        //}


        #region Constructors
        public DateTime(Avm.Date value)
        {
            m_value = value;
        }

        /// <summary>
        /// Constructs a DateTime for specified ticks
        /// </summary>
        public DateTime(long ticks)
        {
            TimeSpan ts = new TimeSpan(ticks);
            if (ts.Ticks < MinValue.Ticks || ts.Ticks > MaxValue.Ticks)
            {
                string msg = Locale.GetText("Value {0} is outside the valid range [{1},{2}].",
                    ticks, MinValue.Ticks, MaxValue.Ticks);
                throw new ArgumentOutOfRangeException("ticks", msg);
            }
            m_value = FromTicks(ts);
#if NET_2_0
			kind = DateTimeKind.Unspecified;
#endif
        }

        private static Avm.Date FromTicks(TimeSpan ts)
        {
            //TODO:
            return new Date(ts.Hours, ts.Minutes, ts.Seconds);
        }

        public DateTime(int year, int month, int day)
            : this(year, month, day, 0, 0, 0, 0) { }

        public DateTime(int year, int month, int day, int hour, int minute, int second)
            : this(year, month, day, hour, minute, second, 0) { }

        public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            if (year < 1 || year > 9999 ||
                month < 1 || month > 12 ||
                day < 1 || day > DaysInMonth(year, month) ||
                hour < 0 || hour > 23 ||
                minute < 0 || minute > 59 ||
                second < 0 || second > 59 ||
                millisecond < 0 || millisecond > 999)
                throw new ArgumentOutOfRangeException("Parameters describe an " +
                                    "unrepresentable DateTime.");

            //ticks = new TimeSpan(AbsoluteDays(year, month, day), hour, minute, second, millisecond);
            m_value = new Avm.Date(year, month, day, hour, minute, second, millisecond);

#if NET_2_0
			kind = DateTimeKind.Unspecified;
#endif
        }

        [MonoTODO("The Calendar is not taken into consideration")]
        public DateTime(int year, int month, int day, Calendar calendar)
            : this(year, month, day, 0, 0, 0, 0, calendar)
        {
        }

        [MonoTODO("The Calendar is not taken into consideration")]
        public DateTime(int year, int month, int day, int hour, int minute, int second, Calendar calendar)
            : this(year, month, day, hour, minute, second, 0, calendar)
        {
        }

        [MonoTODO("The Calendar is not taken into consideration")]
        public DateTime(int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar)
            : this(year, month, day, hour, minute, second, millisecond)
        {
            if (calendar == null)
                throw new ArgumentNullException("calendar");
        }

        internal DateTime(bool check, TimeSpan value)
        {
            if (check && (value.Ticks < MinValue.Ticks || value.Ticks > MaxValue.Ticks))
                throw new ArgumentOutOfRangeException();

            m_value = FromTicks(value);

#if NET_2_0
			kind = DateTimeKind.Unspecified;
#endif
        }

#if NET_2_0
		public DateTime (long ticks, DateTimeKind kind) : this (ticks)
		{
			CheckDateTimeKind (kind);
			this.kind = kind;
		}

		public DateTime (int year, int month, int day, int hour, int minute, int second, DateTimeKind kind)
			: this (year, month, day, hour, minute, second)
		{
			CheckDateTimeKind (kind);
			this.kind = kind;
		}

		public DateTime (int year, int month, int day, int hour, int minute, int second, int millisecond, DateTimeKind kind)
			: this (year, month, day, hour, minute, second, millisecond)
		{
			CheckDateTimeKind (kind);
			this.kind = kind;
		}

		public DateTime (int year, int month, int day, int hour, int minute, int second, int millisecond, Calendar calendar, DateTimeKind kind)
			: this (year, month, day, hour, minute, second, millisecond, calendar)
		{
			CheckDateTimeKind (kind);
			this.kind = kind;
		}			
#endif
        #endregion

        /* Properties  */

        public DateTime Date
        {
            get
            {
                DateTime ret = new DateTime(Year, Month, Day);
#if NET_2_0
				ret.kind = kind;
#endif
                return ret;
            }
        }

        public int Month
        {
            get
            {
                //NOTE: In AVM month starts from 0, in .NET - from 1.
                return m_value.month + 1;
                //return FromTicks(Which.Month);
            }
        }


        public int Day
        {
            get
            {
                return m_value.date;
                //return FromTicks(Which.Day); 
            }
        }

        public DayOfWeek DayOfWeek
        {
            get
            {
                return (DayOfWeek)m_value.day;
                //return ( (DayOfWeek) ((ticks.Days+1) % 7) ); 
            }
        }

        public int DayOfYear
        {
            get
            {
                int year = Year;
                int month = Month;
                int day = Day;
                int[] days = (IsLeapYear(year) ? daysmonthleap : daysmonth);
                int temp = 0, m = 1;
                while (m < month)
                    temp += days[m++];
                return (day - 1) + temp;
            }
        }

        public TimeSpan TimeOfDay
        {
            get
            {
                return new TimeSpan(Ticks % TimeSpan.TicksPerDay);
            }

        }

        public int Hour
        {
            get
            {
                return m_value.hours;
                //return ticks.Hours;
            }
        }

        public int Minute
        {
            get
            {
                return m_value.minutes;
                //return ticks.Minutes;
            }
        }

        public int Second
        {
            get
            {
                return m_value.seconds;
                //return ticks.Seconds;
            }
        }

        public int Millisecond
        {
            get
            {
                return m_value.milliseconds;
                //return ticks.Milliseconds;
            }
        }

        //
        // To reduce the time consumed by DateTime.Now, we keep
        // the difference to map the system time into a local
        // time into `to_local_time_span', we record the timestamp
        // for this in `last_now'
        //
        //static object to_local_time_span_object;
        //static long last_now;

        public static DateTime Now
        {
            get
            {
                DateTime ret = new DateTime(new Date());
#if NET_2_0
                ret.kind = DateTimeKind.Local;
#endif
                return ret;

//                long now = GetNow();
//                DateTime dt = new DateTime(now);

//                if ((now - last_now) > TimeSpan.TicksPerMinute)
//                {
//                    to_local_time_span_object = TimeZone.CurrentTimeZone.GetLocalTimeDiff(dt);
//                    last_now = now;

//                }

//                // This is boxed, so we avoid locking.
//                DateTime ret = dt + (TimeSpan)to_local_time_span_object;
//#if NET_2_0
//                ret.kind = DateTimeKind.Local;
//#endif
//                return ret;
            }
        }

        public long Ticks
        {
            get
            {
                return new TimeSpan(DayOfYear, Hour, Minute, Second, Millisecond).Ticks;
                //return ticks.Ticks;
            }
        }

        public TimeSpan TicksTS
        {
            get { return new TimeSpan(Ticks); }
        }

        public static DateTime Today
        {
            get
            {
                DateTime now = Now;
                DateTime today = new DateTime(now.Year, now.Month, now.Day);
#if NET_2_0
				today.kind = now.kind;
#endif
                return today;
            }
        }

        private static Date ToUtc(Date d)
        {
            return new Date(d.fullYearUTC, d.monthUTC, d.dateUTC,
                            d.hoursUTC, d.minutesUTC, d.secondsUTC, d.millisecondsUTC);
        }

        private static Date ToLocal(Date d)
        {
            return new Date(d.fullYear, d.month, d.date,
                            d.hours, d.minutes, d.seconds, d.milliseconds);
        }

        public static DateTime UtcNow
        {
            get
            {
                DateTime ret = new DateTime(ToUtc(new Date()));
#if NET_2_0
                ret.kind = DateTimeKind.Utc;
#endif
                return ret;
            }
        }

        public int Year
        {
            get
            {
                return m_value.fullYear;
                //return FromTicks(Which.Year);
            }
        }

#if NET_2_0
		public DateTimeKind Kind {
			get {
				return kind;
			}
		}
#endif

        /* methods */

        public DateTime Add(TimeSpan ts)
        {
            DateTime ret = AddTicks(ts.Ticks);
#if NET_2_0
			ret.kind = kind;
#endif
            return ret;
        }

        public DateTime AddDays(double days)
        {
            return AddMilliseconds(Math.Round(days * 86400000));
        }

        public DateTime AddTicks(long t)
        {
            long ct = Ticks;
            if ((t + ct) > MAX_VALUE_TICKS || (t + ct) < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            DateTime ret = new DateTime(t + ct);
#if NET_2_0
			ret.kind = kind;
#endif
            return ret;
        }

        public DateTime AddHours(double hours)
        {
            return AddMilliseconds(hours * 3600000);
        }

        public DateTime AddMilliseconds(double ms)
        {
            if ((ms * TimeSpan.TicksPerMillisecond) > long.MaxValue ||
                    (ms * TimeSpan.TicksPerMillisecond) < long.MinValue)
            {
                throw new ArgumentOutOfRangeException();
            }
            long msticks = (long)(ms * TimeSpan.TicksPerMillisecond);

            return AddTicks(msticks);
        }

        // required to match MS implementation for OADate (OLE Automation)
        private DateTime AddRoundedMilliseconds(double ms)
        {
            if ((ms * TimeSpan.TicksPerMillisecond) > long.MaxValue ||
                (ms * TimeSpan.TicksPerMillisecond) < long.MinValue)
            {
                throw new ArgumentOutOfRangeException();
            }
            long msticks = (long)(ms += ms > 0 ? 0.5 : -0.5) * TimeSpan.TicksPerMillisecond;

            return AddTicks(msticks);
        }

        public DateTime AddMinutes(double minutes)
        {
            return AddMilliseconds(minutes * 60000);
        }

        public DateTime AddMonths(int months)
        {
            int day, month, year, maxday;
            DateTime temp;

            day = this.Day;
            month = this.Month + (months % 12);
            year = this.Year + months / 12;

            if (month < 1)
            {
                month = 12 + month;
                year--;
            }
            else if (month > 12)
            {
                month = month - 12;
                year++;
            }
            maxday = DaysInMonth(year, month);
            if (day > maxday)
                day = maxday;

            temp = new DateTime(year, month, day);
#if NET_2_0
			temp.kind = kind;
#endif
            return temp.Add(this.TimeOfDay);
        }

        public DateTime AddSeconds(double seconds)
        {
            return AddMilliseconds(seconds * 1000);
        }

        public DateTime AddYears(int years)
        {
            return AddMonths(years * 12);
        }

        public static int Compare(DateTime t1, DateTime t2)
        {
            if (avm.LessThan(t1.m_value, t2.m_value))
                return -1;
            if (avm.GreaterThan(t1.m_value, t2.m_value))
                return 1;
            else
                return 0;
        }

        public int CompareTo(object v)
        {
            if (v == null)
                return 1;

            if (!(v is System.DateTime))
                throw new ArgumentException(Locale.GetText(
                    "Value is not a System.DateTime"));

            return Compare(this, (DateTime)v);
        }

        public int CompareTo(DateTime value)
        {
            return Compare(this, value);
        }

        public bool Equals(DateTime value)
        {
            return value.m_value == m_value;
        }

#if NET_2_0
		public bool IsDaylightSavingTime ()
		{
			if (kind == DateTimeKind.Utc)
				return false;
			return TimeZone.CurrentTimeZone.IsDaylightSavingTime (this);
		}

		public long ToBinary ()
		{
			switch (kind) {
			case DateTimeKind.Utc:
				return Ticks | 0x4000000000000000;
			case DateTimeKind.Local:
				return (long) ((ulong)Ticks | 0x8000000000000000);
			default:
				return Ticks;
			}
		}

		public static DateTime FromBinary (long dateData)
		{
			switch ((ulong)dateData >> 62) {
			case 1:
				return new DateTime (dateData ^ 0x4000000000000000, DateTimeKind.Utc);
			case 0:
				return new DateTime (dateData, DateTimeKind.Unspecified);
			default:
				return new DateTime (((dateData << 2) >> 2), DateTimeKind.Local);
			}
		}

		public static DateTime SpecifyKind (DateTime value, DateTimeKind kind)
		{
			return new DateTime (value.Ticks, kind);
		}
#endif

        public static int DaysInMonth(int year, int month)
        {
            int[] days;

            if (month < 1 || month > 12)
                throw new ArgumentOutOfRangeException();

            days = (IsLeapYear(year) ? daysmonthleap : daysmonth);
            return days[month];
        }

        public override bool Equals(object o)
        {
            if (!(o is DateTime))
                return false;

            return ((DateTime)o).m_value == m_value;
        }

        public static bool Equals(DateTime t1, DateTime t2)
        {
            return (t1.m_value == t2.m_value);
        }

        public static DateTime FromFileTime(long fileTime)
        {
            if (fileTime < 0)
                throw new ArgumentOutOfRangeException("fileTime", "< 0");

            return new DateTime(w32file_epoch + fileTime).ToLocalTime();
        }

#if NET_1_1
        public static DateTime FromFileTimeUtc(long fileTime)
        {
            if (fileTime < 0)
                throw new ArgumentOutOfRangeException("fileTime", "< 0");

            return new DateTime(w32file_epoch + fileTime);
        }
#endif

        public static DateTime FromOADate(double d)
        {
            // An OLE Automation date is implemented as a floating-point number
            // whose value is the number of days from midnight, 30 December 1899.

            // d must be negative 657435.0 through positive 2958466.0.
            if ((d <= OAMinValue) || (d >= OAMaxValue))
                throw new ArgumentException("d", "[-657435,2958466]");

            DateTime dt = new DateTime(ticks18991230);
            if (d < 0.0d)
            {
                Double days = Math.Ceiling(d);
                // integer part is the number of days (negative)
                dt = dt.AddRoundedMilliseconds(days * 86400000);
                // but decimals are the number of hours (in days fractions) and positive
                Double hours = (days - d);
                dt = dt.AddRoundedMilliseconds(hours * 86400000);
            }
            else
            {
                dt = dt.AddRoundedMilliseconds(d * 86400000);
            }

            return dt;
        }

        public string[] GetDateTimeFormats()
        {
            return GetDateTimeFormats(CultureInfo.CurrentCulture);
        }

        public string[] GetDateTimeFormats(char format)
        {
            if ("dDgGfFmMrRstTuUyY".IndexOf(format) < 0)
                throw new FormatException("Invalid format character.");
            string[] result = new string[1];
            result[0] = this.ToString(format.ToString());
            return result;
        }

        public string[] GetDateTimeFormats(IFormatProvider provider)
        {
            DateTimeFormatInfo info = (DateTimeFormatInfo)provider.GetFormat(typeof(DateTimeFormatInfo));
            //			return GetDateTimeFormats (info.GetAllDateTimePatterns ());
            ArrayList al = new ArrayList();
            foreach (char c in "dDgGfFmMrRstTuUyY")
                al.AddRange(GetDateTimeFormats(c, info));
            return al.ToArray(typeof(string)) as string[];
        }

        public string[] GetDateTimeFormats(char format, IFormatProvider provider)
        {
            if ("dDgGfFmMrRstTuUyY".IndexOf(format) < 0)
                throw new FormatException("Invalid format character.");

            // LAMESPEC: There is NO assurance that 'U' ALWAYS
            // euqals to 'F', but since we have to iterate all
            // the pattern strings, we cannot just use 
            // ToString("U", provider) here. I believe that the 
            // method's behavior cannot be formalized.
            bool adjustutc = false;
            switch (format)
            {
                case 'U':
                    //			case 'r':
                    //			case 'R':
                    //			case 'u':
                    adjustutc = true;
                    break;
            }
            DateTimeFormatInfo info = (DateTimeFormatInfo)provider.GetFormat(typeof(DateTimeFormatInfo));
            return GetDateTimeFormats(adjustutc, info.GetAllRawDateTimePatterns(format), info);
        }

        private string[] GetDateTimeFormats(bool adjustutc, string[] patterns, DateTimeFormatInfo dfi)
        {
            string[] results = new string[patterns.Length];
            DateTime val = adjustutc ? ToUniversalTime() : this;
            for (int i = 0; i < results.Length; i++)
                results[i] = val._ToString(patterns[i], dfi);
            return results;
        }

#if NET_2_0
		private void CheckDateTimeKind (DateTimeKind kind) {
			if ((kind != DateTimeKind.Unspecified) && (kind != DateTimeKind.Utc) && (kind != DateTimeKind.Local))
				throw new ArgumentException ("Invalid DateTimeKind value.", "kind");
		}
#endif

        public override int GetHashCode()
        {
            int days = AbsoluteDays(Year, Month, Day);
            return days + Hour + Minute + Second + Millisecond;
        }

        public TypeCode GetTypeCode()
        {
            return TypeCode.DateTime;
        }

        public static bool IsLeapYear(int year)
        {
            return ((year % 4 == 0 && year % 100 != 0) || year % 400 == 0);
        }

        public static DateTime Parse(string s)
        {
            return Parse(s, null);
        }

        public static DateTime Parse(string s, IFormatProvider fp)
        {
            return Parse(s, fp, DateTimeStyles.AllowWhiteSpaces);
        }

        public static DateTime Parse(string s, IFormatProvider fp, DateTimeStyles styles)
        {
            // This method should try only expected patterns. 
            // Should not try extra patterns.
            // Right now we also try InvariantCulture, but I
            // confirmed in some cases this method rejects what
            // InvariantCulture supports (can be checked against
            // "th-TH" with Gregorian Calendar). So basically it
            // should not be done.
            // I think it should be CurrentCulture to be tested,
            // but right now we don't support all the supported
            // patterns for each culture, so try InvariantCulture
            // as a quick remedy.

            const string formatExceptionMessage = "String was not recognized as a valid DateTime.";
            const string argumentYearRangeExceptionMessage = "Valid values are between 1 and 9999, inclusive.";

            if (s == null)
                throw new ArgumentNullException(Locale.GetText("s is null"));
            DateTime result;

            if (fp == null)
                fp = CultureInfo.CurrentCulture;
            DateTimeFormatInfo dfi = DateTimeFormatInfo.GetInstance(fp);

            bool longYear = false;

            // Try all the patterns
            if (ParseExact(s, dfi.GetAllDateTimePatternsInternal(), dfi, styles, out result, false, ref longYear))
                return result;

            // Try common formats.
            //			if (ParseExact (s, commonFormats, dfi, styles, out result, false, ref longYear))
            //				return result;

            // Try common formats with invariant culture
            if (ParseExact(s, commonFormats, DateTimeFormatInfo.InvariantInfo, styles, out result, false, ref longYear))
                return result;

#if NET_2_0
			// .NET does not throw an ArgumentOutOfRangeException, but .NET 1.1 does.
			throw new FormatException (formatExceptionMessage);
#else
            if (longYear)
            {
                throw new ArgumentOutOfRangeException("year",
                    argumentYearRangeExceptionMessage);
            }

            throw new FormatException(formatExceptionMessage);
#endif
        }

        public static DateTime ParseExact(string s, string format, IFormatProvider fp)
        {
            return ParseExact(s, format, fp, DateTimeStyles.None);
        }

        internal static int _ParseNumber(string s, int valuePos,
                          int min_digits,
                          int digits,
                          bool leadingzero,
                          bool sloppy_parsing,
                          out int num_parsed)
        {
            int number = 0, i;

            if (sloppy_parsing)
                leadingzero = false;

            if (!leadingzero)
            {
                int real_digits = 0;
                for (i = valuePos; i < s.Length && i < digits + valuePos; i++)
                {
                    if (!Char.IsDigit(s[i]))
                        break;

                    real_digits++;
                }

                digits = real_digits;
            }
            if (digits < min_digits)
            {
                num_parsed = -1;
                return 0;
            }

            if (s.Length - valuePos < digits)
            {
                num_parsed = -1;
                return 0;
            }

            for (i = valuePos; i < digits + valuePos; i++)
            {
                char c = s[i];
                if (!Char.IsDigit(c))
                {
                    num_parsed = -1;
                    return 0;
                }

                number = number * 10 + (byte)(c - '0');
            }

            num_parsed = digits;
            return number;
        }

        internal static int _ParseEnum(string s, int sPos, string[] values, out int num_parsed)
        {
            int i;

            // FIXME: I know this is somehow lame code. Probably
            // it should iterate all the enum value and return
            // the longest match. However right now I don't see
            // anything but "1" and "10" - "12" that might match
            // two or more values. (They are only abbrev month
            // names, so do reverse order search). See bug #80094.
            for (i = values.Length - 1; i >= 0; i--)
            {
                if (s.Length - sPos < values[i].Length)
                    continue;
                else if (values[i].Length == 0)
                    continue;
                String tmp = s.Substring(sPos, values[i].Length);
                if (String.Compare(tmp, values[i], true) == 0)
                {
                    num_parsed = values[i].Length;
                    return i;
                }
            }

            num_parsed = -1;
            return -1;
        }

        internal static bool _ParseString(string s, int sPos, int maxlength, string value, out int num_parsed)
        {
            if (maxlength <= 0)
                maxlength = value.Length;

            if (String.Compare(s, sPos, value, 0, maxlength, true, CultureInfo.InvariantCulture) == 0)
            {
                num_parsed = maxlength;
                return true;
            }

            num_parsed = -1;
            return false;
        }

        private static bool _DoParse(string s, string format, bool exact,
                           out DateTime result,
                           DateTimeFormatInfo dfi,
                           DateTimeStyles style,
                           ref bool longYear)
        {
#if NET_2_0
			DateTimeKind explicit_kind = DateTimeKind.Unspecified;
#endif
            bool useutc = false, use_localtime = true;
            bool use_invariant = false;
            bool sloppy_parsing = false;
            int valuePos = 0;
            if (format.Length == 1)
                format = _GetStandardPattern(format[0], dfi, out useutc, out use_invariant);
            else if (!exact && CultureInfo.InvariantCulture.CompareInfo.IndexOf(format, "GMT", CompareOptions.Ordinal) >= 0)
                useutc = true;

            if ((style & DateTimeStyles.AllowLeadingWhite) != 0)
            {
                format = format.TrimStart(null);

                s = s.TrimStart(null); // it could be optimized, but will make little good.
            }

            if ((style & DateTimeStyles.AllowTrailingWhite) != 0)
            {
                format = format.TrimEnd(null);
                s = s.TrimEnd(null); // it could be optimized, but will make little good.
            }

            if (use_invariant)
                dfi = DateTimeFormatInfo.InvariantInfo;

            if ((style & DateTimeStyles.AllowInnerWhite) != 0)
                sloppy_parsing = true;

            string chars = format;
            int len = format.Length, pos = 0, num = 0;

            int day = -1, dayofweek = -1, month = -1, year = -1;
            int hour = -1, minute = -1, second = -1;
            double fractionalSeconds = -1;
            int ampm = -1;
            int tzsign = -1, tzoffset = -1, tzoffmin = -1;

            result = new DateTime(0);
            while (pos + num < len)
            {
                if (s.Length == valuePos)
                    break;

                bool leading_zeros = true;

                if (chars[pos] == '\'')
                {
                    num = 1;
                    while (pos + num < len)
                    {
                        if (chars[pos + num] == '\'')
                            break;

                        if (valuePos == s.Length)
                            return false;
                        if (s[valuePos] != chars[pos + num])
                            return false;
                        valuePos++;

                        num++;
                    }
                    if (pos + num > len)
                        return false;

                    pos += num + 1;
                    num = 0;
                    continue;
                }
                else if (chars[pos] == '"')
                {
                    num = 1;
                    while (pos + num < len)
                    {
                        if (chars[pos + num] == '"')
                            break;

                        if (valuePos == s.Length)
                            return false;
                        if (s[valuePos] != chars[pos + num])
                            return false;
                        valuePos++;

                        num++;
                    }
                    if (pos + num > len)
                        return false;

                    pos += num + 1;
                    num = 0;
                    continue;
                }
                else if (chars[pos] == '\\')
                {
                    pos += num + 1;
                    num = 0;
                    if (pos >= len)
                        return false;

                    if (s[valuePos] != chars[pos])
                        return false;
                    valuePos++;
                    pos++;
                    continue;
                }
                else if (chars[pos] == '%')
                {
                    pos++;
                    continue;
                }
                else if (Char.IsWhiteSpace(s[valuePos]) ||
                  s[valuePos] == ',' && Char.IsWhiteSpace(chars[pos]))
                {
                    valuePos++;
                    num = 0;
                    if (exact && (style & DateTimeStyles.AllowInnerWhite) == 0)
                    {
                        if (!Char.IsWhiteSpace(chars[pos]))
                            return false;
                        pos++;
                        continue;
                    }

                    int ws = valuePos;
                    while (ws < s.Length)
                    {
                        if (Char.IsWhiteSpace(s[ws]) || s[ws] == ',')
                            ws++;
                        else
                            break;
                    }
                    valuePos = ws;
                    ws = pos;
                    while (ws < chars.Length)
                    {
                        if (Char.IsWhiteSpace(chars[ws]) || chars[ws] == ',')
                            ws++;
                        else
                            break;
                    }
                    pos = ws;
                    continue;
                }

                if ((pos + num + 1 < len) && (chars[pos + num + 1] == chars[pos + num]))
                {
                    num++;
                    continue;
                }


                int num_parsed = 0;

                switch (chars[pos])
                {
                    case 'd':
                        if (day != -1)
                            return false;
                        if (num == 0)
                            day = _ParseNumber(s, valuePos, 0, 2, false, sloppy_parsing, out num_parsed);
                        else if (num == 1)
                            day = _ParseNumber(s, valuePos, 0, 2, true, sloppy_parsing, out num_parsed);
                        else if (num == 2)
                            dayofweek = _ParseEnum(s, valuePos, dfi.RawAbbreviatedDayNames, out num_parsed);
                        else
                        {
                            dayofweek = _ParseEnum(s, valuePos, dfi.RawDayNames, out num_parsed);
                            num = 3;
                        }
                        break;
                    case 'M':
                        if (month != -1)
                            return false;
                        if (num == 0)
                            month = _ParseNumber(s, valuePos, 0, 2, false, sloppy_parsing, out num_parsed);
                        else if (num == 1)
                            month = _ParseNumber(s, valuePos, 0, 2, true, sloppy_parsing, out num_parsed);
                        else if (num == 2)
                            month = _ParseEnum(s, valuePos, dfi.RawAbbreviatedMonthNames, out num_parsed) + 1;
                        else
                        {
                            month = _ParseEnum(s, valuePos, dfi.RawMonthNames, out num_parsed) + 1;
                            num = 3;
                        }
                        break;
                    case 'y':
                        if (year != -1)
                            return false;

                        if (num == 0)
                        {
                            year = _ParseNumber(s, valuePos, 0, 2, false, sloppy_parsing, out num_parsed);
                        }
                        else if (num < 3)
                        {
                            year = _ParseNumber(s, valuePos, 0, 2, true, sloppy_parsing, out num_parsed);
                        }
                        else
                        {
                            year = _ParseNumber(s, valuePos, 4, 4, false, sloppy_parsing, out num_parsed);
                            if ((year >= 1000) && (num_parsed == 4) && (!longYear) && (s.Length > 4 + valuePos))
                            {
                                int np = 0;
                                int ly = _ParseNumber(s, valuePos, 5, 5, false, sloppy_parsing, out np);
                                longYear = (ly > 9999);
                            }
                            num = 3;
                        }

                        //FIXME: We should do use dfi.Calendat.TwoDigitYearMax
                        if (num_parsed <= 2)
                            year += (year < 30) ? 2000 : 1900;
                        break;
                    case 'h':
                        if (hour != -1)
                            return false;
                        if (num == 0)
                            hour = _ParseNumber(s, valuePos, 0, 2, false, sloppy_parsing, out num_parsed);
                        else
                        {
                            hour = _ParseNumber(s, valuePos, 0, 2, true, sloppy_parsing, out num_parsed);
                            num = 1;
                        }

                        if (hour > 12)
                            return false;
                        if (hour == 12)
                            hour = 0;

                        break;
                    case 'H':
                        if ((hour != -1) || (ampm >= 0))
                            return false;
                        if (num == 0)
                            hour = _ParseNumber(s, valuePos, 0, 2, false, sloppy_parsing, out num_parsed);
                        else
                        {
                            hour = _ParseNumber(s, valuePos, 0, 2, true, sloppy_parsing, out num_parsed);
                            num = 1;
                        }
                        if (hour >= 24)
                            return false;

                        //					ampm = -2;
                        break;
                    case 'm':
                        if (minute != -1)
                            return false;
                        if (num == 0)
                            minute = _ParseNumber(s, valuePos, 0, 2, false, sloppy_parsing, out num_parsed);
                        else
                        {
                            minute = _ParseNumber(s, valuePos, 0, 2, true, sloppy_parsing, out num_parsed);
                            num = 1;
                        }
                        if (minute >= 60)
                            return false;

                        break;
                    case 's':
                        if (second != -1)
                            return false;
                        if (num == 0)
                            second = _ParseNumber(s, valuePos, 0, 2, false, sloppy_parsing, out num_parsed);
                        else
                        {
                            second = _ParseNumber(s, valuePos, 0, 2, true, sloppy_parsing, out num_parsed);
                            num = 1;
                        }
                        if (second >= 60)
                            return false;

                        break;
#if NET_2_0
				case 'F':
					leading_zeros = false;
					goto case 'f';
#endif
                    case 'f':
                        if (fractionalSeconds != -1)
                            return false;
                        num = Math.Min(num, 6);
                        double decimalNumber = (double)_ParseNumber(s, valuePos, 0, num + 1, leading_zeros, sloppy_parsing, out num_parsed);
                        if (num_parsed == -1)
                            return false;

                        else
                            fractionalSeconds = decimalNumber / Math.Pow(10.0, num_parsed);
                        break;
                    case 't':
                        if (ampm != -1)
                            return false;
                        if (num == 0)
                        {
                            if (_ParseString(s, valuePos, 1, dfi.AMDesignator, out num_parsed))
                                ampm = 0;
                            else if (_ParseString(s, valuePos, 1, dfi.PMDesignator, out num_parsed))
                                ampm = 1;
                            else
                                return false;
                        }
                        else
                        {
                            if (_ParseString(s, valuePos, 0, dfi.AMDesignator, out num_parsed))
                                ampm = 0;
                            else if (_ParseString(s, valuePos, 0, dfi.PMDesignator, out num_parsed))
                                ampm = 1;
                            else
                                return false;
                            num = 1;
                        }
                        break;
                    case 'z':
                        if (tzsign != -1)
                            return false;
                        if (s[valuePos] == '+')
                            tzsign = 0;
                        else if (s[valuePos] == '-')
                            tzsign = 1;
                        else
                            return false;
                        valuePos++;
                        if (num == 0)
                            tzoffset = _ParseNumber(s, valuePos, 0, 2, false, sloppy_parsing, out num_parsed);
                        else if (num == 1)
                            tzoffset = _ParseNumber(s, valuePos, 0, 2, true, sloppy_parsing, out num_parsed);
                        else
                        {
                            tzoffset = _ParseNumber(s, valuePos, 0, 2, true, sloppy_parsing, out num_parsed);
                            if (num_parsed < 0)
                                return false;
                            valuePos += num_parsed;
                            if (Char.IsDigit(s[valuePos]))
                                num_parsed = 0;
                            else if (!_ParseString(s, valuePos, 0, dfi.TimeSeparator, out num_parsed))
                                return false;
                            valuePos += num_parsed;
                            tzoffmin = _ParseNumber(s, valuePos, 0, 2, true, sloppy_parsing, out num_parsed);
                            if (num_parsed < 0)
                                return false;
                            num = 2;
                        }
                        break;
#if NET_2_0
				case 'K':
					if (s [valuePos] == 'Z') {
						valuePos++;
						explicit_kind = DateTimeKind.Utc;
					} else if (s [valuePos] == '+' || s [valuePos] == '-') {
						if (tzsign != -1)
							return false;
						if (s [valuePos] == '+')
							tzsign = 0;
						else if (s [valuePos] == '-')
							tzsign = 1;
						valuePos++;

						// zzz
						tzoffset = _ParseNumber (s, valuePos, 0, 2, true, sloppy_parsing, out num_parsed);
						if (num_parsed < 0)
							return false;
						valuePos += num_parsed;
						if (Char.IsDigit (s [valuePos]))
							num_parsed = 0;
						else if (!_ParseString (s, valuePos, 0, dfi.TimeSeparator, out num_parsed))
							return false;
						valuePos += num_parsed;
						tzoffmin = _ParseNumber (s, valuePos, 0, 2, true, sloppy_parsing, out num_parsed);
						if (num_parsed < 0)
							return false;
						num = 2;
						explicit_kind = DateTimeKind.Local;
					}
					break;
#endif
                    // LAMESPEC: This should be part of UTCpattern
                    // string and thus should not be considered here.
                    //
                    // Note that 'Z' is not defined as a pattern
                    // character. Keep it for X509 certificate
                    // verification. Also, "Z" != "'Z'" under MS.NET
                    // ("'Z'" is just literal; handled above)
                    case 'Z':
                        if (s[valuePos] != 'Z')
                            return false;
                        num = 0;
                        num_parsed = 1;
                        useutc = true;
                        break;

                    case ':':
                        if (!_ParseString(s, valuePos, 0, dfi.TimeSeparator, out num_parsed))
                            return false;
                        break;
                    case '/':
                        /* Accept any character for
                         * DateSeparator, except
                         * TimeSeparator, a digit or a
                         * letter.  Not documented,
                         * but seems to be MS
                         * behaviour here.  See bug
                         * 54047.
                         */
                        if (exact && s[valuePos] != '/')
                            return false;

                        if (_ParseString(s, valuePos, 0,
                                  dfi.TimeSeparator,
                                  out num_parsed) ||
                            Char.IsDigit(s[valuePos]) ||
                            Char.IsLetter(s[valuePos]))
                        {
                            return (false);
                        }

                        num = 0;
                        if (num_parsed <= 0)
                        {
                            num_parsed = 1;
                        }

                        break;
                    default:
                        if (s[valuePos] != chars[pos])
                        {
                            // FIXME: It is not sure, but
                            // IsLetter() is introduced 
                            // because we have to reject 
                            // "2002a02b25" but have to
                            // allow "2002$02$25". The same
                            // thing applies to '/' case.
                            if (exact ||
                                Char.IsDigit(s[valuePos]) ||
                                Char.IsLetter(s[valuePos]))
                                return false;
                        }
                        num = 0;
                        num_parsed = 1;
                        break;
                }

                if (num_parsed < 0)
                    return false;

                valuePos += num_parsed;

                if (!exact)
                {
                    switch (chars[pos])
                    {
                        case 'm':
                        case 's':
#if NET_2_0
					case 'F':
#endif
                        case 'f':
                        case 'z':
                            if (s.Length > valuePos && s[valuePos] == 'Z'
                                && (pos + 1 == chars.Length
                                || chars[pos + 1] != 'Z'))
                            {
                                useutc = true;
                                valuePos++;
                            }
                            break;
                    }
                }

                pos = pos + num + 1;
                num = 0;
            }

            // possible empty value. Regarded as no match.
            if (pos == 0)
                return false;

            if (pos < len)
                return false;

            if (s.Length != valuePos) // extraneous tail.
                return false;

            if (hour == -1)
                hour = 0;
            if (minute == -1)
                minute = 0;

            if (second == -1)
                second = 0;
            if (fractionalSeconds == -1)
                fractionalSeconds = 0;

            // If no date was given
            if ((day == -1) && (month == -1) && (year == -1))
            {
                if ((style & DateTimeStyles.NoCurrentDateDefault) != 0)
                {
                    day = 1;
                    month = 1;
                    year = 1;
                }
                else
                {
                    day = Today.Day;
                    month = Today.Month;
                    year = Today.Year;
                }
            }


            if (day == -1)
                day = 1;
            if (month == -1)
                month = 1;
            if (year == -1)
            {
                if ((style & DateTimeStyles.NoCurrentDateDefault) != 0)
                    year = 1;
                else
                    year = Today.Year;
            }

            if (ampm == 1)
                hour = hour + 12;

            // For anything out of range 
            // return false
            if (year < 1 || year > 9999 ||
            month < 1 || month > 12 ||
            day < 1 || day > DaysInMonth(year, month) ||
            hour < 0 || hour > 23 ||
            minute < 0 || minute > 59 ||
            second < 0 || second > 59)
                return false;

            result = new DateTime(year, month, day, hour, minute, second, 0);
            result = result.AddSeconds(fractionalSeconds);

            if ((dayofweek != -1) && (dayofweek != (int)result.DayOfWeek))
                throw new FormatException(Locale.GetText("String was not recognized as valid DateTime because the day of week was incorrect."));

            // If no timezone was specified, default to the local timezone.
            TimeSpan utcoffset;

            if (useutc)
            {
                if ((style & DateTimeStyles.AdjustToUniversal) != 0)
                    use_localtime = false;
                utcoffset = new TimeSpan(0, 0, 0);
            }
            else if (tzsign == -1)
            {
                TimeZone tz = TimeZone.CurrentTimeZone;
                utcoffset = tz.GetUtcOffset(result);
            }
            else
            {
                if ((style & DateTimeStyles.AdjustToUniversal) != 0)
                    use_localtime = false;

                if (tzoffmin == -1)
                    tzoffmin = 0;
                if (tzoffset == -1)
                    tzoffset = 0;
                if (tzsign == 1)
                    tzoffset = -tzoffset;

                utcoffset = new TimeSpan(tzoffset, tzoffmin, 0);
            }

            long newticks = (result.TicksTS - utcoffset).Ticks;

            result = new DateTime(false, new TimeSpan(newticks));
#if NET_2_0
			if (explicit_kind != DateTimeKind.Unspecified)
				result.kind = explicit_kind;
			else if (use_localtime)
				result = result.ToLocalTime ();
			else
				result.kind = DateTimeKind.Utc;
#else
            if (use_localtime)
                result = result.ToLocalTime();
#endif

            return true;
        }


        public static DateTime ParseExact(string s, string format,
                           IFormatProvider fp, DateTimeStyles style)
        {
            if (format == null)
                throw new ArgumentNullException("format");

            string[] formats = new string[1];
            formats[0] = format;

            return ParseExact(s, formats, fp, style);
        }

        public static DateTime ParseExact(string s, string[] formats,
                           IFormatProvider fp,
                           DateTimeStyles style)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.GetInstance(fp);

            if (s == null)
                throw new ArgumentNullException("s");
            if (formats == null)
                throw new ArgumentNullException("formats");
            if (formats.Length == 0)
                throw new FormatException("Format specifier was invalid.");

            for (int i = 0; i < formats.Length; i++)
            {
                string format = formats[i];
                if (format == null || format.Length == 0)
                    throw new FormatException("Format specifier was invalid.");
            }

            DateTime result;
            bool longYear = false;
            if (!ParseExact(s, formats, dfi, style, out result, true, ref longYear))
                throw new FormatException();
            return result;
        }

#if NET_2_0
		public static bool TryParse (string s, out DateTime result)
		{
			try {
				result = Parse (s);
			} catch {
				result = MinValue;
				return false;
			}
			return true;
		}
		
		public static bool TryParse (string s, IFormatProvider provider, DateTimeStyles styles, out DateTime result)
		{
			try {
				result = Parse (s, provider, styles);
			} catch {
				result = MinValue;
				return false;
			}
			return true;
		}
		
		public static bool TryParseExact (string s, string format,
						  IFormatProvider fp,
						  DateTimeStyles style,
						  out DateTime result)
		{
			string[] formats;

			formats = new string [1];
			formats[0] = format;

			return TryParseExact (s, formats, fp, style, out result);
		}

		public static bool TryParseExact (string s, string[] formats,
						  IFormatProvider fp,
						  DateTimeStyles style,
						  out DateTime result)
		{
			DateTimeFormatInfo dfi = DateTimeFormatInfo.GetInstance (fp);

			bool longYear = false;
			return ParseExact (s, formats, dfi, style, out result, true, ref longYear);
		}
#endif

        private static bool ParseExact(string s, string[] formats,
            DateTimeFormatInfo dfi, DateTimeStyles style, out DateTime ret,
            bool exact, ref bool longYear)
        {
            int i;
            for (i = 0; i < formats.Length; i++)
            {
                DateTime result;

                if (_DoParse(s, formats[i], exact, out result, dfi, style, ref longYear))
                {
                    ret = result;
                    return true;
                }
            }
            ret = DateTime.MinValue;
            return false;
        }

        public TimeSpan Subtract(DateTime dt)
        {
            return new TimeSpan(Ticks) - new TimeSpan(dt.Ticks);
        }

        public DateTime Subtract(TimeSpan ts)
        {
            TimeSpan newticks;

            newticks = (new TimeSpan(Ticks)) - ts;
            DateTime ret = new DateTime(true, newticks);
#if NET_2_0
			ret.kind = kind;
#endif
            return ret;
        }

        public long ToFileTime()
        {
            DateTime universalTime = ToUniversalTime();

            if (universalTime.Ticks < w32file_epoch)
            {
                throw new ArgumentOutOfRangeException("file time is not valid");
            }

            return (universalTime.Ticks - w32file_epoch);
        }

#if NET_1_1
        public long ToFileTimeUtc()
        {
            if (Ticks < w32file_epoch)
            {
                throw new ArgumentOutOfRangeException("file time is not valid");
            }

            return (Ticks - w32file_epoch);
        }
#endif

        public string ToLongDateString()
        {
            return ToString("D");
        }

        public string ToLongTimeString()
        {
            return ToString("T");
        }

        public double ToOADate()
        {
            long t = this.Ticks;
            // uninitialized DateTime case
            if (t == 0)
                return 0;
            // we can't reach minimum value
            if (t < 31242239136000000)
                return OAMinValue + 0.001;

            TimeSpan ts = new TimeSpan(this.Ticks - ticks18991230);
            double result = ts.TotalDays;
            // t < 0 (where 599264352000000000 == 0.0d for OA)
            if (t < 599264352000000000)
            {
                // negative days (int) but decimals are positive
                double d = Math.Ceiling(result);
                result = d - 2 - (result - d);
            }
            else
            {
                // we can't reach maximum value
                if (result >= OAMaxValue)
                    result = OAMaxValue - 0.00000001d;
            }
            return result;
        }

        public string ToShortDateString()
        {
            return ToString("d");
        }

        public string ToShortTimeString()
        {
            return ToString("t");
        }

        public override string ToString()
        {
            return ToString("G", null);
        }

        public string ToString(IFormatProvider fp)
        {
            return ToString(null, fp);
        }

        public string ToString(string format)
        {
            return ToString(format, null);
        }

        internal static string _GetStandardPattern(char format, DateTimeFormatInfo dfi, out bool useutc, out bool use_invariant)
        {
            String pattern;

            useutc = false;
            use_invariant = false;

            switch (format)
            {
                case 'd':
                    pattern = dfi.ShortDatePattern;
                    break;
                case 'D':
                    pattern = dfi.LongDatePattern;
                    break;
                case 'f':
                    pattern = dfi.LongDatePattern + " " + dfi.ShortTimePattern;
                    break;
                case 'F':
                    pattern = dfi.FullDateTimePattern;
                    break;
                case 'g':
                    pattern = dfi.ShortDatePattern + " " + dfi.ShortTimePattern;
                    break;
                case 'G':
                    pattern = dfi.ShortDatePattern + " " + dfi.LongTimePattern;
                    break;
                case 'm':
                case 'M':
                    pattern = dfi.MonthDayPattern;
                    break;
#if NET_2_0
			case 'o':
				pattern = dfi.RoundtripPattern;
				break;
#endif
                case 'r':
                case 'R':
                    pattern = dfi.RFC1123Pattern;
                    // commented by LP 09/jun/2002, rfc 1123 pattern is always in GMT
                    // uncommented by AE 27/may/2004
                    //				useutc = true;
                    use_invariant = true;
                    break;
                case 's':
                    pattern = dfi.SortableDateTimePattern;
                    break;
                case 't':
                    pattern = dfi.ShortTimePattern;
                    break;
                case 'T':
                    pattern = dfi.LongTimePattern;
                    break;
                case 'u':
                    pattern = dfi.UniversalSortableDateTimePattern;
                    useutc = true;
                    break;
                case 'U':
                    //				pattern = dfi.LongDatePattern + " " + dfi.LongTimePattern;
                    pattern = dfi.FullDateTimePattern;
                    useutc = true;
                    break;
                case 'y':
                case 'Y':
                    pattern = dfi.YearMonthPattern;
                    break;
                default:
                    pattern = null;
                    break;
                //				throw new FormatException (String.Format ("Invalid format pattern: '{0}'", format));
            }

            return pattern;
        }

        internal string _ToString(string format, DateTimeFormatInfo dfi)
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

                        int hr = this.Hour % 12;
                        if (hr == 0)
                            hr = 12;

                        ZeroPad(result, hr, tokLen == 1 ? 1 : 2);
                        break;
                    case 'H':
                        // hour, [0, 23]
                        tokLen = CountRepeat(format, i, ch);
                        ZeroPad(result, this.Hour, tokLen == 1 ? 1 : 2);
                        break;
                    case 'm':
                        // minute, [0, 59]
                        tokLen = CountRepeat(format, i, ch);
                        ZeroPad(result, this.Minute, tokLen == 1 ? 1 : 2);
                        break;
                    case 's':
                        // second [0, 29]
                        tokLen = CountRepeat(format, i, ch);
                        ZeroPad(result, this.Second, tokLen == 1 ? 1 : 2);
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

                        int dec = (int)((long)(this.Ticks % TimeSpan.TicksPerSecond) / (long)Math.Pow(10, 7 - tokLen));
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
                        string desig = this.Hour < 12 ? dfi.AMDesignator : dfi.PMDesignator;

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
                        TimeSpan offset = TimeZone.CurrentTimeZone.GetUtcOffset(this);

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
					switch (kind) {
					case DateTimeKind.Utc:
						result.Append ('Z');
						break;
					case DateTimeKind.Local:
						offset = TimeZone.CurrentTimeZone.GetUtcOffset (this);
						if (offset.Ticks >= 0)
							result.Append ('+');
						else
							result.Append ('-');
						result.Append (Math.Abs (offset.Hours).ToString ("00"));
						result.Append (':');
						result.Append (Math.Abs (offset.Minutes).ToString ("00"));
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
                            ZeroPad(result, dfi.Calendar.GetDayOfMonth(this), tokLen == 1 ? 1 : 2);
                        else if (tokLen == 3)
                            result.Append(dfi.GetAbbreviatedDayName(dfi.Calendar.GetDayOfWeek(this)));
                        else
                            result.Append(dfi.GetDayName(dfi.Calendar.GetDayOfWeek(this)));

                        break;
                    case 'M':
                        // Month.m(m?) = month # (with leading 0 if two mm)
                        // mmm = 3 letter name
                        // mmmm+ = full name
                        tokLen = CountRepeat(format, i, ch);
                        int month = dfi.Calendar.GetMonth(this);
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
                            ZeroPad(result, dfi.Calendar.GetYear(this) % 100, tokLen);
                        else
                            ZeroPad(result, dfi.Calendar.GetYear(this), (tokLen == 3 ? 3 : 4));

                        break;
                    case 'g':
                        // Era name
                        tokLen = CountRepeat(format, i, ch);
                        result.Append(dfi.GetEraName(dfi.Calendar.GetEra(this)));
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

        public string ToString(string format, IFormatProvider fp)
        {
            DateTimeFormatInfo dfi = DateTimeFormatInfo.GetInstance(fp);

            if (format == null || format == String.Empty)
                format = "G";

            bool useutc = false, use_invariant = false;

            if (format.Length == 1)
            {
                char fchar = format[0];
                format = _GetStandardPattern(fchar, dfi, out useutc, out use_invariant);
            }

            // Don't convert UTC value. It just adds 'Z' for 
            // 'u' format, for the same ticks.
            return this._ToString(format, dfi);
        }

        public DateTime ToLocalTime()
        {
            return TimeZone.CurrentTimeZone.ToLocalTime(this);
        }

        public DateTime ToUniversalTime()
        {
            return TimeZone.CurrentTimeZone.ToUniversalTime(this);
        }

        /*  OPERATORS */

        public static DateTime operator +(DateTime d, TimeSpan t)
        {
            DateTime ret = new DateTime(true, d.TicksTS + t);
#if NET_2_0
			ret.kind = d.kind;
#endif
            return ret;
        }

        public static bool operator ==(DateTime d1, DateTime d2)
        {
            return (d1.m_value == d2.m_value);
        }

        public static bool operator >(DateTime t1, DateTime t2)
        {
            return avm.GreaterThan(t1.m_value, t2.m_value);
        }

        public static bool operator >=(DateTime t1, DateTime t2)
        {
            return avm.GreaterThanOrEqual(t1.m_value, t2.m_value);
        }

        public static bool operator !=(DateTime d1, DateTime d2)
        {
            return (d1.m_value != d2.m_value);
        }

        public static bool operator <(DateTime t1, DateTime t2)
        {
            return avm.LessThan(t1.m_value, t2.m_value);
        }

        public static bool operator <=(DateTime t1, DateTime t2)
        {
            return avm.LessThanOrEqual(t1.m_value, t2.m_value);
        }

        public static TimeSpan operator -(DateTime d1, DateTime d2)
        {
            return new TimeSpan((d1.TicksTS - d2.TicksTS).Ticks);
        }

        public static DateTime operator -(DateTime d, TimeSpan t)
        {
            DateTime ret = new DateTime(true, d.TicksTS - t);
#if NET_2_0
			ret.kind = d.kind;
#endif
            return ret;
        }

        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        byte IConvertible.ToByte(IFormatProvider provider)
        {
            throw new InvalidCastException();

        }

        char IConvertible.ToChar(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        System.DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return this;
        }

        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        double IConvertible.ToDouble(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        Int16 IConvertible.ToInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        Int32 IConvertible.ToInt32(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        Int64 IConvertible.ToInt64(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        SByte IConvertible.ToSByte(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        Single IConvertible.ToSingle(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            if (conversionType == null)
                throw new ArgumentNullException("conversionType");

            if (conversionType == typeof(DateTime))
                return this;
            else if (conversionType == typeof(String))
                return this.ToString(provider);
            else if (conversionType == typeof(Object))
                return this;
            else
                throw new InvalidCastException();
        }

        UInt16 IConvertible.ToUInt16(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        UInt32 IConvertible.ToUInt32(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }

        UInt64 IConvertible.ToUInt64(IFormatProvider provider)
        {
            throw new InvalidCastException();
        }
    }
}
