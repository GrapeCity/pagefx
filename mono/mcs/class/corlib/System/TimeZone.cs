//
// System.TimeZone.cs
//
// Authors:
//   Duncan Mak (duncan@ximian.com)
//   Ajay Kumar Dwivedi (adwiv@yahoo.com)
//   Martin Baulig (martin@gnome.org)
//
// (C) Ximian, Inc.
//

//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
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

using System;
using System.Collections;
using System.Globalization;

namespace System
{
    //[Serializable]
    /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;T:System.TimeZone&quot;]/*"/>
    internal abstract class TimeZone
    {
        // Fields
        private static TimeZone currentTimeZone;

        // Constructor
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.TimeZone.#ctor&quot;]/*"/>
        protected TimeZone()
        {
        }

        // Properties
        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;P:System.TimeZone.CurrentTimeZone&quot;]/*"/>
        public static TimeZone CurrentTimeZone
        {
            get
            {
                if (currentTimeZone == null)
                    currentTimeZone = new CurrentTimeZone();
                return currentTimeZone;
            }
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;P:System.TimeZone.DaylightName&quot;]/*"/>
        public abstract string DaylightName
        {
            get;
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;P:System.TimeZone.StandardName&quot;]/*"/>
        public abstract string StandardName
        {
            get;
        }

        // Methods
        
#if NOT_PFX
        public
#else
       internal 
#endif
        abstract DaylightTime GetDaylightChanges(int year);

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.TimeZone.GetUtcOffset(System.DateTime)&quot;]/*"/>
        public abstract TimeSpan GetUtcOffset(DateTime time);

        public abstract int GetUtcOffsetMinutes(DateTime time);

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.TimeZone.IsDaylightSavingTime(System.DateTime)&quot;]/*"/>
        public virtual bool IsDaylightSavingTime(DateTime time)
        {
            //return IsDaylightSavingTime(time, GetDaylightChanges(time.Year));
            return IsDaylightSavingTime(time, null);
        }

#if NOT_PFX
        public
#else
        internal 
#endif
        static bool IsDaylightSavingTime(DateTime time, DaylightTime daylightTimes)
        {
            // Another timezone hack. We determine if it daylightsavings time by
            // getting timezone offset for the specified time and for a known 
            // point in standard time (at least for the northern hemisphere).
            // If the offsets differ then it must be DST.

            int noDLS, now;

            Avm.Date dt = new Avm.Date();
            dt.fullYear = time.Year; // Enter a known standard time
            dt.month = 1;
            dt.setDate(1);
            dt.hours = 12;
            dt.minutes = 0;
            dt.seconds = 0;
            dt.milliseconds = 0;
            noDLS = dt.timezoneOffset;

            dt.month = time.Month;
            dt.setDate(time.Day);
            dt.hours = time.Hour;
            dt.minutes = time.Minute;
            dt.milliseconds = time.Millisecond;
            now = dt.timezoneOffset;

            return (now != noDLS); // If they differ it's DSL
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.TimeZone.ToLocalTime(System.DateTime)&quot;]/*"/>
        public virtual DateTime ToLocalTime(DateTime time)
        {
            return time + GetUtcOffset(time);
        }

        /// <include file="D:\\PageFX\\Source\\DDCorLib\\Docs\\DDCorLib.xml" path="doc/members/member[@name=&quot;M:System.TimeZone.ToUniversalTime(System.DateTime)&quot;]/*"/>
        public virtual DateTime ToUniversalTime(DateTime time)
        {
            return time - GetUtcOffset(time);
        }

        //
        // This routine returns the TimeDiff that would have to be
        // added to "time" to turn it into a local time.   This would
        // be equivalent to call ToLocalTime.
        //
        // There is one important consideration:
        //
        //    This information is only valid during the minute it
        //    was called.
        //
        //    This only works with a real time, not one of the boundary
        //    cases like DateTime.MaxValue, so validation must be done
        //    before.
        // 
        //    This is intended to be used by DateTime.Now
        //
        // We use a minute, just to be conservative and cope with
        // any potential time zones that might be defined in the future
        // that might not nicely fit in hour or half-hour steps. 
        //    
        internal TimeSpan GetLocalTimeDiff(DateTime time)
        {
            return GetLocalTimeDiff(time, GetUtcOffset(time));
        }

        //
        // This routine is intended to be called by GetLocalTimeDiff(DatetTime)
        // or by ToLocalTime after validation has been performed
        //
        // time is the time to map, utc_offset is the utc_offset that
        // has been computed for calling GetUtcOffset on time.
        //
        // When called by GetLocalTime, utc_offset is assumed to come
        // from a time constructed by new DateTime (DateTime.GetNow ()), that
        // is a valid time.
        //
        // When called by ToLocalTime ranges are checked before this is
        // called.
        //
        internal TimeSpan GetLocalTimeDiff(DateTime time, TimeSpan utc_offset)
        {
            DaylightTime dlt = GetDaylightChanges(time.Year);

            if (dlt.Delta.Ticks == 0)
                return utc_offset;

            DateTime local = time.Add(utc_offset);
            if (local < dlt.End && dlt.End.Subtract(dlt.Delta) <= local)
                return utc_offset;

            if (local >= dlt.Start && dlt.Start.Add(dlt.Delta) > local)
                return utc_offset - dlt.Delta;

            return GetUtcOffset(local);
        }
    }

    internal class CurrentTimeZone : TimeZone
    {
        // Fields
        private static string daylightName;
        private static string standardName;
        private static TimeSpan utcOffsetWithOutDLS;
        private static TimeSpan utcOffsetWithDLS;
        private static int noDLS;
        private static int yesDLS;

        private static string[] GetTimeZoneNames(int utcOffset)
        {
            switch (utcOffset)
            {
                case 600:
                    return new string[] {"Hawaiian Standard Time", "Hawaiian Daylight Time"};
                case 540:
                    return new string[] {"Alaskan Standard Time", "Alaskan Daylight Time"};
                case 480:
                    return new string[] {"Pacific Standard Time", "Pacific Daylight Time"};
                case 420:
                    return new string[] {"Mountain Standard Time", "Mountain Daylight Time"};
                case 360:
                    return new string[] {"Central Standard Time", "Central Daylight Time"};
                case 300:
                    return new string[] {"Eastern Standard Time", "Eastern Daylight Time"};
                case 240:
                    return new string[] {"Atlantic Standard Time", "Atlantic Daylight Time"};
                case 0:
                    return new string[] {"GMT Standard Time", "GMT Daylight Time"};
                case -60:
                    return new string[] {"Central European Standard Time", "Central European Daylight Time"};
                case -540:
                    return new string[] {"Tokyo Standard Time", "Tokyo Daylight Time"};
                default:
                    return new string[] {"?ST", "?DT"};
            }
        }


        // Constructor
        internal CurrentTimeZone()
        {
            Avm.Date dt = new Avm.Date();

            // How to get daylight savings info: Flash hides daylight savings details
            // from the user so we create two dates one in winter (standard time) and 
            // one in summer (daylight time) and subtract their TimeZoneOffset values.
            // A hack but it should work.
            dt.month = 1;
            dt.setDate(1);
            dt.hours = 12;
            dt.minutes = 0;
            dt.seconds = 0;
            dt.milliseconds = 0;
            noDLS = -dt.timezoneOffset;
            dt.month = 7;
            yesDLS = -dt.timezoneOffset;

            string[] names = GetTimeZoneNames(noDLS);
            standardName = names[0];
            daylightName = names[1];

            utcOffsetWithOutDLS = new TimeSpan(noDLS * TimeSpan.TicksPerMinute);
            utcOffsetWithDLS = new TimeSpan(yesDLS * TimeSpan.TicksPerMinute);
        }

        // Properties
        public override string DaylightName
        {
            get { return daylightName; }
        }

        public override string StandardName
        {
            get { return standardName; }
        }

        // Methods
#if NOT_PFX
        public
#else
        internal 
#endif
        override DaylightTime GetDaylightChanges(int year)
        {
            if (year < 1 || year > 9999)
                throw new ArgumentOutOfRangeException("year", year +
                    Locale.GetText(" is not in a range between 1 and 9999."));

            DaylightTime dlt = new DaylightTime(DateTime.MinValue, DateTime.MinValue, TimeSpan.Zero);
            return dlt;
        }

        public override TimeSpan GetUtcOffset(DateTime time)
        {
            if (IsDaylightSavingTime(time))
                return utcOffsetWithDLS;
            return utcOffsetWithOutDLS;
        }

        public override int GetUtcOffsetMinutes(DateTime time)
        {
            if (IsDaylightSavingTime(time))
                return yesDLS;
            return noDLS;
        }
    }
}
