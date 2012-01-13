using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// The Date class represents date and time information. An instance of the Date class represents a particular point
    /// in time for which the properties such as month, day, hours, and seconds can be queried or modified. The Date
    /// class lets you retrieve date and time values relative to universal time (Greenwich mean time, now called universal
    /// time or UTC) or relative to local time, which is determined by the local time zone setting on the operating system
    /// that is running Flash Player. The methods of the Date class are not static but apply only to the individual Date
    /// object specified when the method is called. The Date.UTC()  and Date.parse()  methods are
    /// exceptions; they are static methods.
    /// </summary>
    [PageFX.ABC]
    [PageFX.QName("Date")]
    [PageFX.FP9]
    public class Date : Object
    {
        public extern virtual int month
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int monthUTC
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int minutesUTC
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int hours
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int milliseconds
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int hoursUTC
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int dateUTC
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int fullYearUTC
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int day
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual int time
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int secondsUTC
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int date
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int minutes
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int fullYear
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int millisecondsUTC
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int dayUTC
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual int timezoneOffset
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual int seconds
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Returns the milliseconds (an integer from 0 to 999) portion of a Date object
        /// according to local time. Local time is determined by the operating system on which
        /// Flash Player is running.
        /// </summary>
        /// <returns>The milliseconds portion of a Date object.</returns>
        [PageFX.ABC]
        [PageFX.QName("getMilliseconds", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double getMilliseconds();

        /// <summary>
        /// Returns the minutes (an integer from 0 to 59) portion of a Date object
        /// according to universal time (UTC).
        /// </summary>
        /// <returns>The UTC minutes portion of a Date object.</returns>
        [PageFX.ABC]
        [PageFX.QName("getUTCMinutes", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double getUTCMinutes();

        /// <summary>
        /// Sets the milliseconds, according to local time, and returns the new time in
        /// milliseconds. Local time is determined by the operating system on which Flash Player
        /// is running.
        /// </summary>
        /// <param name="arg0">An integer from 0 to 999.</param>
        /// <returns>The new time, in milliseconds.</returns>
        [PageFX.ABC]
        [PageFX.QName("setMilliseconds", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double setMilliseconds(object arg0);

        [PageFX.ABC]
        [PageFX.QName("setMilliseconds", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setMilliseconds();

        /// <summary>
        /// Returns a String representation of the time and timezone only, and does not include the day and date.
        /// Contrast with the Date.toDateString() method, which returns only the day and date.
        /// </summary>
        /// <returns>The string representation of time and timezone only.</returns>
        [PageFX.ABC]
        [PageFX.QName("toTimeString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String toTimeString();

        /// <summary>
        /// Returns a String representation of the day, date, and time in universal time (UTC).
        /// For example, the date February 1, 2005 is returned as Tue Feb 1 00:00:00 2005 UTC.
        /// </summary>
        /// <returns>The string representation of a Date object in UTC time.</returns>
        [PageFX.ABC]
        [PageFX.QName("toUTCString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String toUTCString();

        /// <summary>Sets the milliseconds, in universal time (UTC), and returns the new time in milliseconds.</summary>
        /// <param name="arg0">An integer from 0 to 999.</param>
        /// <returns>The new time, in milliseconds.</returns>
        [PageFX.ABC]
        [PageFX.QName("setUTCMilliseconds", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double setUTCMilliseconds(object arg0);

        [PageFX.ABC]
        [PageFX.QName("setUTCMilliseconds", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setUTCMilliseconds();

        /// <summary>
        /// Sets the minutes, according to local time, and returns the new time in milliseconds.
        /// Local time is determined by the operating system on which Flash Player is running.
        /// </summary>
        /// <param name="arg0">An integer from 0 to 59.</param>
        /// <param name="arg1">An integer from 0 to 59.</param>
        /// <param name="arg2">An integer from 0 to 999.</param>
        /// <returns>The new time, in milliseconds.</returns>
        [PageFX.ABC]
        [PageFX.QName("setMinutes", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double setMinutes(object arg0, object arg1, object arg2);

        [PageFX.ABC]
        [PageFX.QName("setMinutes", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setMinutes(object arg0, object arg1);

        [PageFX.ABC]
        [PageFX.QName("setMinutes", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setMinutes(object arg0);

        [PageFX.ABC]
        [PageFX.QName("setMinutes", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setMinutes();

        /// <summary>
        /// Returns the milliseconds (an integer from 0 to 999) portion of a Date object
        /// according to universal time (UTC).
        /// </summary>
        /// <returns>The UTC milliseconds portion of a Date object.</returns>
        [PageFX.ABC]
        [PageFX.QName("getUTCMilliseconds", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double getUTCMilliseconds();

        /// <summary>
        /// Returns the day of the month (an integer from 1 to 31) specified by a Date object
        /// according to local time. Local time is determined by the operating system on which
        /// Flash Player is running.
        /// </summary>
        /// <returns>The day of the month (1 - 31) a Date object represents.</returns>
        [PageFX.ABC]
        [PageFX.QName("getDate", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double getDate();

        /// <summary>
        /// Returns a String representation of the day, date, time, given in local time.
        /// Contrast with the Date.toString() method, which returns the same information (plus the timezone)
        /// with the year listed at the end of the string.
        /// </summary>
        /// <returns>A string representation of a Date object in the local timezone.</returns>
        [PageFX.ABC]
        [PageFX.QName("toLocaleString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String toLocaleString();

        /// <summary>
        /// Returns the number of milliseconds since midnight January 1, 1970, universal time,
        /// for a Date object.
        /// </summary>
        /// <returns>The number of milliseconds since January 1, 1970 that a Date object represents.</returns>
        [PageFX.ABC]
        [PageFX.QName("valueOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double valueOf();

        /// <summary>
        /// Returns the minutes (an integer from 0 to 59) portion of a Date object
        /// according to local time. Local time is determined by the operating system on which
        /// Flash Player is running.
        /// </summary>
        /// <returns>The minutes portion of a Date object.</returns>
        [PageFX.ABC]
        [PageFX.QName("getMinutes", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double getMinutes();

        /// <summary>
        /// Sets the minutes, in universal time (UTC), and returns the new time in milliseconds.
        /// Optionally, you can specify the seconds and milliseconds.
        /// </summary>
        /// <param name="arg0">An integer from 0 to 59.</param>
        /// <param name="arg1">An integer from 0 to 59.</param>
        /// <param name="arg2">An integer from 0 to 999.</param>
        /// <returns>The new time, in milliseconds.</returns>
        [PageFX.ABC]
        [PageFX.QName("setUTCMinutes", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double setUTCMinutes(object arg0, object arg1, object arg2);

        [PageFX.ABC]
        [PageFX.QName("setUTCMinutes", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setUTCMinutes(object arg0, object arg1);

        [PageFX.ABC]
        [PageFX.QName("setUTCMinutes", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setUTCMinutes(object arg0);

        [PageFX.ABC]
        [PageFX.QName("setUTCMinutes", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setUTCMinutes();

        /// <summary>
        /// Sets the day of the month, according to local time, and returns the new time in
        /// milliseconds. Local time is determined by the operating system on which Flash Player
        /// is running.
        /// </summary>
        /// <param name="arg0">An integer from 1 to 31.</param>
        /// <returns>The new time, in milliseconds.</returns>
        [PageFX.ABC]
        [PageFX.QName("setDate", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double setDate(object arg0);

        [PageFX.ABC]
        [PageFX.QName("setDate", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setDate();

        /// <summary>
        /// Returns the seconds (an integer from 0 to 59) portion of a Date object
        /// according to universal time (UTC).
        /// </summary>
        /// <returns>The UTC seconds portion of a Date object.</returns>
        [PageFX.ABC]
        [PageFX.QName("getUTCSeconds", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double getUTCSeconds();

        /// <summary>
        /// Returns the month (0 [January] to 11 [December]) portion of a Date object
        /// according to universal time (UTC).
        /// </summary>
        /// <returns>The UTC month portion of a Date object.</returns>
        [PageFX.ABC]
        [PageFX.QName("getUTCMonth", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double getUTCMonth();

        /// <summary>
        /// Sets the day of the month, in universal time (UTC), and returns the new time in
        /// milliseconds. Calling this method does not modify the other fields of a Date
        /// object, but the Date.getUTCDay() and Date.getDay() methods can report
        /// a new value if the day of the week changes as a result of calling this method.
        /// </summary>
        /// <param name="arg0">A number; an integer from 1 to 31.</param>
        /// <returns>The new time, in milliseconds.</returns>
        [PageFX.ABC]
        [PageFX.QName("setUTCDate", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double setUTCDate(object arg0);

        [PageFX.ABC]
        [PageFX.QName("setUTCDate", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setUTCDate();

        /// <summary>
        /// Returns a string representation of the day and date only, and does not include the time or timezone.
        /// Contrast with the following methods:
        /// Date.toTimeString(), which returns only the time and timezoneDate.toString(), which returns not only the day and date, but also the time and timezone.
        /// </summary>
        /// <returns>The string representation of day and date only.</returns>
        [PageFX.ABC]
        [PageFX.QName("toDateString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String toDateString();

        /// <summary>
        /// Returns the day of the month (an integer from 1 to 31) of a Date object,
        /// according to universal time (UTC).
        /// </summary>
        /// <returns>The UTC day of the month (1 to 31) that a Date object represents.</returns>
        [PageFX.ABC]
        [PageFX.QName("getUTCDate", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double getUTCDate();

        /// <summary>
        /// Sets the seconds, and optionally the milliseconds, in universal time (UTC) and
        /// returns the new time in milliseconds.
        /// </summary>
        /// <param name="arg0">An integer from 0 to 59.</param>
        /// <param name="arg1">An integer from 0 to 999.</param>
        /// <returns>The new time, in milliseconds.</returns>
        [PageFX.ABC]
        [PageFX.QName("setUTCSeconds", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double setUTCSeconds(object arg0, object arg1);

        [PageFX.ABC]
        [PageFX.QName("setUTCSeconds", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setUTCSeconds(object arg0);

        [PageFX.ABC]
        [PageFX.QName("setUTCSeconds", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setUTCSeconds();

        /// <summary>
        /// Sets the month, and optionally the day, in universal time(UTC) and returns the new
        /// time in milliseconds. Calling this method does not modify the other fields, but the
        /// Date.getUTCDay() and Date.getDay() methods might report a new
        /// value if the day of the week changes as a result of calling this method.
        /// </summary>
        /// <param name="arg0">An integer from 0 (January) to 11 (December).</param>
        /// <param name="arg1">An integer from 1 to 31.</param>
        /// <returns>The new time, in milliseconds.</returns>
        [PageFX.ABC]
        [PageFX.QName("setUTCMonth", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double setUTCMonth(object arg0, object arg1);

        [PageFX.ABC]
        [PageFX.QName("setUTCMonth", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setUTCMonth(object arg0);

        [PageFX.ABC]
        [PageFX.QName("setUTCMonth", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setUTCMonth();

        /// <summary>
        /// Returns the hour (an integer from 0 to 23) of the day of a Date object
        /// according to universal time (UTC).
        /// </summary>
        /// <returns>The UTC hour of the day (0 to 23) a Date object represents.</returns>
        [PageFX.ABC]
        [PageFX.QName("getUTCHours", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double getUTCHours();

        /// <summary>
        /// Returns the number of milliseconds since midnight January 1, 1970, universal time,
        /// for a Date object. Use this method to represent a specific instant in time
        /// when comparing two or more Date objects.
        /// </summary>
        /// <returns>The number of milliseconds since Jan 1, 1970 that a Date object represents.</returns>
        [PageFX.ABC]
        [PageFX.QName("getTime", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double getTime();

        /// <summary>
        /// Sets the seconds, according to local time, and returns the new time in milliseconds.
        /// Local time is determined by the operating system on which Flash Player is running.
        /// </summary>
        /// <param name="arg0">An integer from 0 to 59.</param>
        /// <param name="arg1">An integer from 0 to 999.</param>
        /// <returns>The new time, in milliseconds.</returns>
        [PageFX.ABC]
        [PageFX.QName("setSeconds", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double setSeconds(object arg0, object arg1);

        [PageFX.ABC]
        [PageFX.QName("setSeconds", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setSeconds(object arg0);

        [PageFX.ABC]
        [PageFX.QName("setSeconds", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setSeconds();

        /// <summary>
        /// Sets the month and optionally the day of the month, according to local time, and
        /// returns the new time in milliseconds. Local time is determined by the operating
        /// system on which Flash Player is running.
        /// </summary>
        /// <param name="arg0">An integer from 0 (January) to 11 (December).</param>
        /// <param name="arg1">An integer from 1 to 31.</param>
        /// <returns>The new time, in milliseconds.</returns>
        [PageFX.ABC]
        [PageFX.QName("setMonth", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double setMonth(object arg0, object arg1);

        [PageFX.ABC]
        [PageFX.QName("setMonth", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setMonth(object arg0);

        [PageFX.ABC]
        [PageFX.QName("setMonth", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setMonth();

        /// <summary>
        /// Returns the seconds (an integer from 0 to 59) portion of a Date object
        /// according to local time. Local time is determined by the operating system on which
        /// Flash Player is running.
        /// </summary>
        /// <returns>The seconds (0 to 59) portion of a Date object.</returns>
        [PageFX.ABC]
        [PageFX.QName("getSeconds", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double getSeconds();

        /// <summary>
        /// Returns the month (0 for January, 1 for February, and so on) portion of this
        /// Date according to local time. Local time is determined by the operating system
        /// on which Flash Player is running.
        /// </summary>
        /// <returns>The month (0 - 11) portion of a Date object.</returns>
        [PageFX.ABC]
        [PageFX.QName("getMonth", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double getMonth();

        /// <summary>
        /// Sets the hour, according to local time, and returns the new time in milliseconds.
        /// Local time is determined by the operating system on which Flash Player is running.
        /// </summary>
        /// <param name="arg0">An integer from 0 (midnight) to 23 (11 p.m.).</param>
        /// <param name="arg1">An integer from 0 to 59.</param>
        /// <param name="arg2">An integer from 0 to 59.</param>
        /// <param name="arg3">An integer from 0 to 999.</param>
        /// <returns>The new time, in milliseconds.</returns>
        [PageFX.ABC]
        [PageFX.QName("setHours", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double setHours(object arg0, object arg1, object arg2, object arg3);

        [PageFX.ABC]
        [PageFX.QName("setHours", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setHours(object arg0, object arg1, object arg2);

        [PageFX.ABC]
        [PageFX.QName("setHours", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setHours(object arg0, object arg1);

        [PageFX.ABC]
        [PageFX.QName("setHours", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setHours(object arg0);

        [PageFX.ABC]
        [PageFX.QName("setHours", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setHours();

        /// <summary>
        /// Returns the day of the week (0 for Sunday, 1 for Monday, and so on) of this Date
        /// according to universal time (UTC).
        /// </summary>
        /// <returns>The UTC day of the week (0 to 6) that a Date object represents.</returns>
        [PageFX.ABC]
        [PageFX.QName("getUTCDay", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double getUTCDay();

        /// <summary>
        /// Sets the date in milliseconds since midnight on January 1, 1970, and returns the new
        /// time in milliseconds.
        /// </summary>
        /// <param name="arg0">An integer value where 0 is midnight on January 1, universal time (UTC).</param>
        /// <returns>The new time, in milliseconds.</returns>
        [PageFX.ABC]
        [PageFX.QName("setTime", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double setTime(object arg0);

        [PageFX.ABC]
        [PageFX.QName("setTime", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setTime();

        /// <summary>
        /// Returns a String representation of the time only, and does not include the day, date, year, or timezone.
        /// Contrast with the Date.toTimeString() method, which returns the time and timezone.
        /// </summary>
        /// <returns>The string representation of time and timezone only.</returns>
        [PageFX.ABC]
        [PageFX.QName("toLocaleTimeString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String toLocaleTimeString();

        /// <summary>
        /// Sets the hour, in universal time (UTC), and returns the new time in milliseconds.
        /// Optionally, the minutes, seconds, and milliseconds can be specified.
        /// </summary>
        /// <param name="arg0">An integer from 0 (midnight) to 23 (11 p.m.).</param>
        /// <param name="arg1">An integer from 0 to 59.</param>
        /// <param name="arg2">An integer from 0 to 59.</param>
        /// <param name="arg3">An integer from 0 to 999.</param>
        /// <returns>The new time, in milliseconds.</returns>
        [PageFX.ABC]
        [PageFX.QName("setUTCHours", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double setUTCHours(object arg0, object arg1, object arg2, object arg3);

        [PageFX.ABC]
        [PageFX.QName("setUTCHours", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setUTCHours(object arg0, object arg1, object arg2);

        [PageFX.ABC]
        [PageFX.QName("setUTCHours", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setUTCHours(object arg0, object arg1);

        [PageFX.ABC]
        [PageFX.QName("setUTCHours", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setUTCHours(object arg0);

        [PageFX.ABC]
        [PageFX.QName("setUTCHours", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setUTCHours();

        /// <summary>
        /// Returns the hour (an integer from 0 to 23) of the day portion of a Date object
        /// according to local time. Local time is determined by the operating system on which
        /// Flash Player is running.
        /// </summary>
        /// <returns>The hour (0 - 23) of the day a Date object represents.</returns>
        [PageFX.ABC]
        [PageFX.QName("getHours", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double getHours();

        /// <summary>
        /// Returns the difference, in minutes, between universal
        /// time (UTC) and the computer&apos;s local time.
        /// </summary>
        /// <returns>
        /// The minutes you need to add to the computer&apos;s local time value to equal UTC. If
        /// your computer&apos;s time is set later than UTC, the return value will be negative.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("getTimezoneOffset", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double getTimezoneOffset();

        /// <summary>
        /// Returns the day of the week (0 for Sunday, 1 for Monday, and so on) specified by this
        /// Date according to local time. Local time is determined by the operating
        /// system on which Flash Player is running.
        /// </summary>
        /// <returns>
        /// A numeric version of the day of the week (0 - 6) a Date object
        /// represents.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("getDay", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double getDay();

        /// <summary>
        /// Returns the full year (a four-digit number, such as 2000) of a Date object
        /// according to local time. Local time is determined by the operating system on which
        /// Flash Player is running.
        /// </summary>
        /// <returns>The full year a Date object represents.</returns>
        [PageFX.ABC]
        [PageFX.QName("getFullYear", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double getFullYear();

        /// <summary>
        /// Returns a String representation of the day, date, time, and timezone.
        /// The date format for the output is:
        /// Day Mon Date HH:MM:SS TZD YYYY
        /// For example:
        /// Wed Apr 12 15:30:17 GMT-0700 2006
        /// </summary>
        /// <returns>The string representation of a Date object.</returns>
        [PageFX.ABC]
        [PageFX.QName("toString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String toString();

        /// <summary>
        /// Sets the year, according to local time, and returns the new time in milliseconds. If
        /// the month and day parameters are specified,
        /// they are set to local time. Local time is determined by the operating system on which
        /// Flash Player is running.
        /// Calling this method does not modify the other fields of the Date but
        /// Date.getUTCDay() and Date.getDay() can report a new value
        /// if the day of the week changes as a result of calling this method.
        /// </summary>
        /// <param name="arg0">
        /// A four-digit number specifying a year. Two-digit numbers do not represent
        /// four-digit years; for example, 99 is not the year 1999, but the year 99.
        /// </param>
        /// <param name="arg1">An integer from 0 (January) to 11 (December).</param>
        /// <param name="arg2">A number from 1 to 31.</param>
        /// <returns>The new time, in milliseconds.</returns>
        [PageFX.ABC]
        [PageFX.QName("setFullYear", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double setFullYear(object arg0, object arg1, object arg2);

        [PageFX.ABC]
        [PageFX.QName("setFullYear", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setFullYear(object arg0, object arg1);

        [PageFX.ABC]
        [PageFX.QName("setFullYear", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setFullYear(object arg0);

        [PageFX.ABC]
        [PageFX.QName("setFullYear", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setFullYear();

        /// <summary>
        /// Returns a String representation of the day and date only, and does not include the time or timezone.
        /// This method returns the same value as Date.toDateString.
        /// Contrast with the following methods:
        /// Date.toTimeString(), which returns only the time and timezoneDate.toString(), which returns not only the day and date, but also the
        /// time and timezone.
        /// </summary>
        /// <returns>The String representation of day and date only.</returns>
        [PageFX.ABC]
        [PageFX.QName("toLocaleDateString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String toLocaleDateString();

        /// <summary>
        /// Sets the year, in universal time (UTC), and returns the new time in milliseconds.
        /// Optionally, this method can also set the month and day of the month. Calling
        /// this method does not modify the other fields, but the Date.getUTCDay() and
        /// Date.getDay() methods can report a new value if the day of the week changes as a
        /// result of calling this method.
        /// </summary>
        /// <param name="arg0">
        /// An integer that represents the year specified as a full four-digit year,
        /// such as 2000.
        /// </param>
        /// <param name="arg1">An integer from 0 (January) to 11 (December).</param>
        /// <param name="arg2">An integer from 1 to 31.</param>
        /// <returns>An integer.</returns>
        [PageFX.ABC]
        [PageFX.QName("setUTCFullYear", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double setUTCFullYear(object arg0, object arg1, object arg2);

        [PageFX.ABC]
        [PageFX.QName("setUTCFullYear", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setUTCFullYear(object arg0, object arg1);

        [PageFX.ABC]
        [PageFX.QName("setUTCFullYear", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setUTCFullYear(object arg0);

        [PageFX.ABC]
        [PageFX.QName("setUTCFullYear", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double setUTCFullYear();

        /// <summary>Returns the four-digit year of a Date object according to universal time (UTC).</summary>
        /// <returns>The UTC four-digit year a Date object represents.</returns>
        [PageFX.ABC]
        [PageFX.QName("getUTCFullYear", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double getUTCFullYear();

        /// <summary>
        /// Returns the number of milliseconds between midnight on January 1, 1970, universal time,
        /// and the time specified in the parameters. This method uses universal time, whereas the
        /// Date constructor uses local time.
        /// This method is useful if you want to pass a UTC date to the Date class constructor.
        /// Because the Date class constructor accepts the millisecond offset as an argument, you
        /// can use the Date.UTC() method to convert your UTC date into the corresponding millisecond
        /// offset, and send that offset as an argument to the Date class constructor:
        /// </summary>
        /// <param name="arg0">A four-digit integer that represents the year (for example, 2000).</param>
        /// <param name="arg1">An integer from 0 (January) to 11 (December).</param>
        /// <param name="arg2">(default = 1)  An integer from 1 to 31.</param>
        /// <param name="arg3">(default = 0)  An integer from 0 (midnight) to 23 (11 p.m.).</param>
        /// <param name="arg4">(default = 0)  An integer from 0 to 59.</param>
        /// <param name="arg5">(default = 0)  An integer from 0 to 59.</param>
        /// <param name="arg6">(default = 0)  An integer from 0 to 999.</param>
        /// <returns>The number of milliseconds since January 1, 1970 and the specified date and time.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double UTC(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double UTC(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object rest0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double UTC(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object rest0, object rest1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double UTC(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object rest0, object rest1, object rest2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double UTC(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object rest0, object rest1, object rest2, object rest3);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double UTC(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double UTC(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double UTC(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double UTC(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double UTC(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double UTC(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double UTC(object arg0, object arg1, object arg2, object arg3, object arg4, object arg5);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double UTC(object arg0, object arg1, object arg2, object arg3, object arg4);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double UTC(object arg0, object arg1, object arg2, object arg3);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double UTC(object arg0, object arg1, object arg2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double UTC(object arg0, object arg1);

        /// <summary>
        /// Converts a string representing a date into a number equaling the number of milliseconds
        /// elapsed since January 1, 1970, UTC.
        /// </summary>
        /// <param name="arg0">
        /// A string representation of a date, which conforms to the format for the output of
        /// Date.toString(). The date format for the output of Date.toString() is:
        /// Day Mon DD HH:MM:SS TZD YYYY
        /// For example:
        /// Wed Apr 12 15:30:17 GMT-0700 2006
        /// The Time Zone Designation (TZD) is always in the form GMT-HHMM or UTC-HHMM indicating the
        /// hour and minute offset relative to Greenwich Mean Time (GMT), which is now also called universal time (UTC).
        /// The year month and day terms can be separated by a forward slash (/) or by spaces, but never by a
        /// dash (-). Other supported formats include the following (you can include partial representations of these
        /// formats; that is, just the month, day, and year):
        /// MM/DD/YYYY HH:MM:SS TZD
        /// HH:MM:SS TZD Day Mon/DD/YYYY
        /// Mon DD YYYY HH:MM:SS TZD
        /// Day Mon DD HH:MM:SS TZD YYYY
        /// Day DD Mon HH:MM:SS TZD YYYY
        /// Mon/DD/YYYY HH:MM:SS TZD
        /// YYYY/MM/DD HH:MM:SS TZD
        /// </param>
        /// <returns>A number representing the milliseconds elapsed since January 1, 1970, UTC.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double parse(object arg0);

        #region Constructors
        //see: http://livedocs.adobe.com/flex/201/langref/index.html
        
        //If you pass no arguments, the Date object is assigned the current date and time.
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Date();
        
        //If you pass one argument of data type Number, the Date object is assigned a time value based 
        //on the number of milliseconds since January 1, 1970 0:00:000 GMT, as specified by the lone argument.
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Date(int timevalue);
        
        //If you pass one argument of data type String, and the string contains a valid date, 
        //the Date object contains a time value based on that date.
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Date(String timevalue);
        
        //If you pass two or more arguments, the Date object is assigned a time value based on the argument values 
        //passed, which represent the date's year, month, date, hour, minute, second, and milliseconds.
        //Defaults: date = 1, hour = 0, minute = 0, second = 0, millisecond = 0
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Date(int year, int month);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Date(int year, int month, int date);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Date(int year, int month, int date, int hour, int minute, int second);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Date(int year, int month, int date, int hour, int minute, int second, int millisecond);
        #endregion
        
        #region Custom Members
        public extern virtual int DayOfMonth
        {
            [PageFX.ABC]
            [PageFX.QName("date")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        
            [PageFX.ABC]
            [PageFX.QName("date")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }
        #endregion



    }
}
