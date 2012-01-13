// CHANGED

/*
 * System.TimeZoneInfo
 *
 * Author(s)
 * 	Stephane Delcroix <stephane@delcroix.org>
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the
 * "Software"), to deal in the Software without restriction, including
 * without limitation the rights to use, copy, modify, merge, publish,
 * distribute, sublicense, and/or sell copies of the Software, and to
 * permit persons to whom the Software is furnished to do so, subject to
 * the following conditions:
 * 
 * The above copyright notice and this permission notice shall be
 * included in all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
 * EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
 * NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
 * LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
 * OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
 * WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using System.Text;

#if LIBC
using System.IO;
using Mono;
#endif

namespace System
{
	[SerializableAttribute]
	public sealed partial class TimeZoneInfo : IEquatable<TimeZoneInfo>, ISerializable
#if NOT_PFX
        , IDeserializationCallback
#endif
	{
		TimeSpan baseUtcOffset;
		public TimeSpan BaseUtcOffset {
			get { return baseUtcOffset; }
		}

		string daylightDisplayName;
		public string DaylightName {
			get { 
				if (disableDaylightSavingTime)
					return String.Empty;
				return daylightDisplayName; 
			}
		}

		string displayName;
		public string DisplayName {
			get { return displayName; }
		}

		string id;
		public string Id {
			get { return id; }
		}

		static TimeZoneInfo local;
		public static TimeZoneInfo Local {
			get { 
				if (local == null) {
#if LIBC
					try {
						local = FindSystemTimeZoneByFileName ("Local", "/etc/localtime");	
					} catch {
						try {
							local = FindSystemTimeZoneByFileName ("Local", Path.Combine (TimeZoneDirectory, "localtime"));	
						} catch {
							throw new TimeZoneNotFoundException ();
						}
					}
#else
					throw new TimeZoneNotFoundException ();
#endif
				}
				return local;
			}
		}

		string standardDisplayName;
		public string StandardName {
			get { return standardDisplayName; }
		}

		bool disableDaylightSavingTime;
		public bool SupportsDaylightSavingTime {
			get  { return !disableDaylightSavingTime; }
		}

		static TimeZoneInfo utc;
		public static TimeZoneInfo Utc {
			get {
				if (utc == null)
					utc = CreateCustomTimeZone ("UTC", new TimeSpan (0), "UTC", "UTC");
				return utc;
			}
		}
#if LIBC
		static string timeZoneDirectory = null;
		public static string TimeZoneDirectory {
			get {
				if (timeZoneDirectory == null)
					timeZoneDirectory = "/usr/share/zoneinfo";
				return timeZoneDirectory;
			}
			set {
				ClearCachedData ();
				timeZoneDirectory = value;
			}
		}
#endif
		private AdjustmentRule [] adjustmentRules;

		public static void ClearCachedData ()
		{
			local = null;
			utc = null;
			systemTimeZones = null;
		}

		public static DateTime ConvertTime (DateTime dateTime, TimeZoneInfo destinationTimeZone)
		{
			return ConvertTime (dateTime, TimeZoneInfo.Local, destinationTimeZone);
		}

		public static DateTime ConvertTime (DateTime dateTime, TimeZoneInfo sourceTimeZone, TimeZoneInfo destinationTimeZone)
		{
			if (dateTime.Kind == DateTimeKind.Local && sourceTimeZone != TimeZoneInfo.Local)
				throw new ArgumentException ("Kind propery of dateTime is Local but the sourceTimeZone does not equal TimeZoneInfo.Local");

			if (dateTime.Kind == DateTimeKind.Utc && sourceTimeZone != TimeZoneInfo.Utc)
				throw new ArgumentException ("Kind propery of dateTime is Utc but the sourceTimeZone does not equal TimeZoneInfo.Utc");

			if (sourceTimeZone.IsInvalidTime (dateTime))
				throw new ArgumentException ("dateTime parameter is an invalid time");

			if (sourceTimeZone == null)
				throw new ArgumentNullException ("sourceTimeZone");

			if (destinationTimeZone == null)
				throw new ArgumentNullException ("destinationTimeZone");

			if (dateTime.Kind == DateTimeKind.Local && sourceTimeZone == TimeZoneInfo.Local && destinationTimeZone == TimeZoneInfo.Local)
				return dateTime;

			DateTime utc = ConvertTimeToUtc (dateTime);

			if (destinationTimeZone == TimeZoneInfo.Utc)
				return utc;

			return ConvertTimeFromUtc (utc, destinationTimeZone);	

		}

#if NOT_PFX
		public static DateTimeOffset ConvertTime (DateTimeOffset dateTimeOffset, TimeZoneInfo destinationTimeZone)
		{
			throw new NotImplementedException ();
		}
#endif

		public static DateTime ConvertTimeBySystemTimeZoneId (DateTime dateTime, string destinationTimeZoneId)
		{
			return ConvertTime (dateTime, FindSystemTimeZoneById (destinationTimeZoneId));
		}

		public static DateTime ConvertTimeBySystemTimeZoneId (DateTime dateTime, string sourceTimeZoneId, string destinationTimeZoneId)
		{
			return ConvertTime (dateTime, FindSystemTimeZoneById (sourceTimeZoneId), FindSystemTimeZoneById (destinationTimeZoneId));
		}

#if NOT_PFX
		public static DateTimeOffset ConvertTimeBySystemTimeZoneId (DateTimeOffset dateTimeOffset, string destinationTimeZoneId)
		{
			return ConvertTime (dateTimeOffset, FindSystemTimeZoneById (destinationTimeZoneId));
		}
#endif

		private DateTime ConvertTimeFromUtc (DateTime dateTime)
		{
			if (dateTime.Kind == DateTimeKind.Local)
				throw new ArgumentException ("Kind property of dateTime is Local");

			if (this == TimeZoneInfo.Utc)
				return DateTime.SpecifyKind (dateTime, DateTimeKind.Utc);

			//FIXME: do not rely on DateTime implementation !
			if (this == TimeZoneInfo.Local)
				return DateTime.SpecifyKind (dateTime.ToLocalTime (), DateTimeKind.Unspecified);

			AdjustmentRule rule = GetApplicableRule (dateTime);
		
			if (IsDaylightSavingTime (DateTime.SpecifyKind (dateTime, DateTimeKind.Utc)))
				return DateTime.SpecifyKind (dateTime + BaseUtcOffset + rule.DaylightDelta , DateTimeKind.Unspecified);
			else
				return DateTime.SpecifyKind (dateTime + BaseUtcOffset, DateTimeKind.Unspecified);
		}

		public static DateTime ConvertTimeFromUtc (DateTime dateTime, TimeZoneInfo destinationTimeZone)
		{
			if (destinationTimeZone == null)
				throw new ArgumentNullException ("destinationTimeZone");

			return destinationTimeZone.ConvertTimeFromUtc (dateTime);
		}

		public static DateTime ConvertTimeToUtc (DateTime dateTime)
		{
			if (dateTime.Kind == DateTimeKind.Utc)
				return dateTime;

			//FIXME: do not rely on DateTime implementation !
			return DateTime.SpecifyKind (dateTime.ToUniversalTime (), DateTimeKind.Utc);
		}

		public static DateTime ConvertTimeToUtc (DateTime dateTime, TimeZoneInfo sourceTimeZone)
		{
			if (sourceTimeZone == null)
				throw new ArgumentNullException ("sourceTimeZone");

			if (dateTime.Kind == DateTimeKind.Utc && sourceTimeZone != TimeZoneInfo.Utc)
				throw new ArgumentException ("Kind propery of dateTime is Utc but the sourceTimeZone does not equal TimeZoneInfo.Utc");

			if (dateTime.Kind == DateTimeKind.Local && sourceTimeZone != TimeZoneInfo.Local)
				throw new ArgumentException ("Kind propery of dateTime is Local but the sourceTimeZone does not equal TimeZoneInfo.Local");

			if (sourceTimeZone.IsInvalidTime (dateTime))
				throw new ArgumentException ("dateTime parameter is an invalid time");

			if (dateTime.Kind == DateTimeKind.Utc && sourceTimeZone == TimeZoneInfo.Utc)
				return dateTime;

			if (dateTime.Kind == DateTimeKind.Utc)
				return dateTime;

			if (dateTime.Kind == DateTimeKind.Local)
				return ConvertTimeToUtc (dateTime);

			if (sourceTimeZone.IsAmbiguousTime (dateTime) || !sourceTimeZone.IsDaylightSavingTime (dateTime))
				return DateTime.SpecifyKind (dateTime - sourceTimeZone.BaseUtcOffset, DateTimeKind.Utc);
			else {
				AdjustmentRule rule = sourceTimeZone.GetApplicableRule (dateTime);
				return DateTime.SpecifyKind (dateTime - sourceTimeZone.BaseUtcOffset - rule.DaylightDelta, DateTimeKind.Utc);
			}
		}

		public static TimeZoneInfo CreateCustomTimeZone (string id, TimeSpan baseUtcOffset, string displayName, string standardDisplayName) 
		{
			return CreateCustomTimeZone (id, baseUtcOffset, displayName, standardDisplayName, null, null, true);
		}

		public static TimeZoneInfo CreateCustomTimeZone (string id, TimeSpan baseUtcOffset, string displayName, string standardDisplayName, string daylightDisplayName, TimeZoneInfo.AdjustmentRule [] adjustmentRules)
		{
			return CreateCustomTimeZone (id, baseUtcOffset, displayName, standardDisplayName, daylightDisplayName, adjustmentRules, false);
		}

		public static TimeZoneInfo CreateCustomTimeZone ( string id, TimeSpan baseUtcOffset, string displayName, string standardDisplayName, string daylightDisplayName, TimeZoneInfo.AdjustmentRule [] adjustmentRules, bool disableDaylightSavingTime)
		{
			return new TimeZoneInfo (id, baseUtcOffset, displayName, standardDisplayName, daylightDisplayName, adjustmentRules, disableDaylightSavingTime);
		}

		public bool Equals (TimeZoneInfo other)
		{
			if (other == null)
				return false;

			return other.Id == this.Id && HasSameRules (other);
		}

		public static TimeZoneInfo FindSystemTimeZoneById (string id)
		{
			//FIXME: this method should check for cached values in systemTimeZones
			if (id == null)
				throw new ArgumentNullException ("id");
#if LIBC	
			string filepath = Path.Combine (TimeZoneDirectory, id);
			return FindSystemTimeZoneByFileName (id, filepath);
#else
			throw new NotImplementedException ();
#endif
		}

#if LIBC
		const int BUFFER_SIZE = 8192; //Big enough for any tz file
		private static TimeZoneInfo FindSystemTimeZoneByFileName (string id, string filepath)
		{
			if (!File.Exists (filepath))
				throw new TimeZoneNotFoundException ();

			byte [] buffer = new byte [BUFFER_SIZE];
			int length;
			using (FileStream stream = File.OpenRead (filepath)) {
				length = stream.Read (buffer, 0, BUFFER_SIZE);
			}

			if (!ValidTZFile (buffer, length))
				throw new InvalidTimeZoneException ();

			try {
				return ParseTZBuffer (id, buffer, length);
			} catch (Exception e) {
				throw new InvalidTimeZoneException (e.Message);
			}
		}
#endif

		public static TimeZoneInfo FromSerializedString (string source)
		{
			throw new NotImplementedException ();
		}

		public AdjustmentRule [] GetAdjustmentRules ()
		{
			if (disableDaylightSavingTime)
				return new AdjustmentRule [0];
			else
				return (AdjustmentRule []) adjustmentRules.Clone ();
		}

		public TimeSpan [] GetAmbiguousTimeOffsets (DateTime dateTime)
		{
			if (!IsAmbiguousTime (dateTime))
				throw new ArgumentException ("dateTime is not an ambiguous time");

			AdjustmentRule rule = GetApplicableRule (dateTime);
			return new TimeSpan[] {baseUtcOffset, baseUtcOffset + rule.DaylightDelta};
		}

#if NOT_PFX
		public TimeSpan [] GetAmbiguousTimeOffsets (DateTimeOffset dateTimeOffset)
		{
			if (!IsAmbiguousTime (dateTimeOffset))
				throw new ArgumentException ("dateTimeOffset is not an ambiguous time");

			throw new NotImplementedException ();
		}
#endif

		public override int GetHashCode ()
		{
			int hash_code = Id.GetHashCode ();
			foreach (AdjustmentRule rule in GetAdjustmentRules ())
				hash_code ^= rule.GetHashCode ();
			return hash_code;
		}

		public void GetObjectData (SerializationInfo info, StreamingContext context)
		{
			throw new NotImplementedException ();
		}

		//FIXME: change this to a generic Dictionary and allow caching for FindSystemTimeZoneById
		private static List<TimeZoneInfo> systemTimeZones = null;
		public static ReadOnlyCollection<TimeZoneInfo> GetSystemTimeZones ()
		{
			if (systemTimeZones == null) {
				systemTimeZones = new List<TimeZoneInfo> ();
#if LIBC
				string[] continents = new string [] {"Africa", "America", "Antarctica", "Arctic", "Asia", "Atlantic", "Brazil", "Canada", "Chile", "Europe", "Indian", "Mexico", "Mideast", "Pacific", "US"};
				foreach (string continent in continents) {
					try {
						foreach (string zonepath in Directory.GetFiles (Path.Combine (TimeZoneDirectory, continent))) {
							try {
								string id = String.Format ("{0}/{1}", continent, Path.GetFileName (zonepath));
								systemTimeZones.Add (FindSystemTimeZoneById (id));
							} catch (ArgumentNullException) {
							} catch (TimeZoneNotFoundException) {
							} catch (InvalidTimeZoneException) {
							} catch (Exception) {
								throw;
							}
						}
					} catch {}
				}
#else
				throw new NotImplementedException ("This method is not implemented for this platform");
#endif
			}
			return new ReadOnlyCollection<TimeZoneInfo> (systemTimeZones);
		}

		public TimeSpan GetUtcOffset (DateTime dateTime)
		{
			if (IsDaylightSavingTime (dateTime)) {
				AdjustmentRule rule = GetApplicableRule (dateTime);
				return BaseUtcOffset + rule.DaylightDelta;
			}
			
			return BaseUtcOffset;
		}
#if NOT_PFX
		public TimeSpan GetUtcOffset (DateTimeOffset dateTimeOffset)
		{
			throw new NotImplementedException ();
		}
#endif

		public bool HasSameRules (TimeZoneInfo other)
		{
			if (other == null)
				throw new ArgumentNullException ("other");

			if ((this.adjustmentRules == null) != (other.adjustmentRules == null))
				return false;

			if (this.adjustmentRules == null)
      				return true;

			if (this.BaseUtcOffset != other.BaseUtcOffset)
				return false;

			if (this.adjustmentRules.Length != other.adjustmentRules.Length)
				return false;

			for (int i = 0; i < adjustmentRules.Length; i++) {
				if (! (this.adjustmentRules [i]).Equals (other.adjustmentRules [i]))
					return false;
			}
			
			return true;
		}

		public bool IsAmbiguousTime (DateTime dateTime)
		{
			if (dateTime.Kind == DateTimeKind.Local && IsInvalidTime (dateTime))
				throw new ArgumentException ("Kind is Local and time is Invalid");

			if (this == TimeZoneInfo.Utc)
				return false;
			
			if (dateTime.Kind == DateTimeKind.Utc)
				dateTime = ConvertTimeFromUtc (dateTime);

			if (dateTime.Kind == DateTimeKind.Local && this != TimeZoneInfo.Local)
				dateTime = ConvertTime (dateTime, TimeZoneInfo.Local, this);

			AdjustmentRule rule = GetApplicableRule (dateTime);
			DateTime tpoint = TransitionPoint (rule.DaylightTransitionEnd, dateTime.Year);
			if (dateTime > tpoint - rule.DaylightDelta  && dateTime <= tpoint)
				return true;
				
			return false;
		}

#if NOT_PFX
		public bool IsAmbiguousTime (DateTimeOffset dateTimeOffset)
		{
			throw new NotImplementedException ();
		}
#endif

		public bool IsDaylightSavingTime (DateTime dateTime)
		{
			if (dateTime.Kind == DateTimeKind.Local && IsInvalidTime (dateTime))
				throw new ArgumentException ("dateTime is invalid and Kind is Local");

			if (this == TimeZoneInfo.Utc)
				return false;

			if (!SupportsDaylightSavingTime)
				return false;
			//FIXME: do not rely on DateTime implementation !
			if ((dateTime.Kind == DateTimeKind.Local || dateTime.Kind == DateTimeKind.Unspecified) && this == TimeZoneInfo.Local)
				return dateTime.IsDaylightSavingTime ();

			//FIXME: do not rely on DateTime implementation !
			if (dateTime.Kind == DateTimeKind.Local && this != TimeZoneInfo.Utc)
				return IsDaylightSavingTime (DateTime.SpecifyKind (dateTime.ToUniversalTime (), DateTimeKind.Utc));
				
			AdjustmentRule rule = GetApplicableRule (dateTime.Date);
			if (rule == null)
				return false;

			DateTime DST_start = TransitionPoint (rule.DaylightTransitionStart, dateTime.Year);
			DateTime DST_end = TransitionPoint (rule.DaylightTransitionEnd, dateTime.Year + ((rule.DaylightTransitionStart.Month < rule.DaylightTransitionEnd.Month) ? 0 : 1));
			if (dateTime.Kind == DateTimeKind.Utc) {
				DST_start -= BaseUtcOffset;
				DST_end -= (BaseUtcOffset + rule.DaylightDelta);
			}

			return (dateTime >= DST_start && dateTime < DST_end);
		}

#if NOT_PFX
		public bool IsDaylightSavingTime (DateTimeOffset dateTimeOffset)
		{
			throw new NotImplementedException ();
		}
#endif

		public bool IsInvalidTime (DateTime dateTime)
		{
			if (dateTime.Kind == DateTimeKind.Utc)
				return false;
			if (dateTime.Kind == DateTimeKind.Local && this != Local)
				return false;

			AdjustmentRule rule = GetApplicableRule (dateTime);
			DateTime tpoint = TransitionPoint (rule.DaylightTransitionStart, dateTime.Year);
			if (dateTime >= tpoint && dateTime < tpoint + rule.DaylightDelta)
				return true;
				
			return false;
		}

		public void OnDeserialization (object sender)
		{
			throw new NotImplementedException ();
		}
		
		public string ToSerializedString ()
		{
			throw new NotImplementedException ();
		}

		public override string ToString ()
		{
			return DisplayName;
		}

		private TimeZoneInfo (string id, TimeSpan baseUtcOffset, string displayName, string standardDisplayName, string daylightDisplayName, TimeZoneInfo.AdjustmentRule [] adjustmentRules, bool disableDaylightSavingTime)
		{
			if (id == null)
				throw new ArgumentNullException ("id");

			if (id == String.Empty)
				throw new ArgumentException ("id parameter is an empty string");

			if (baseUtcOffset.Ticks % TimeSpan.TicksPerMinute != 0)
				throw new ArgumentException ("baseUtcOffset parameter does not represent a whole number of minutes");

			if (baseUtcOffset > new TimeSpan (14, 0, 0) || baseUtcOffset < new TimeSpan (-14, 0, 0))
				throw new ArgumentOutOfRangeException ("baseUtcOffset parameter is greater than 14 hours or less than -14 hours");

#if STRICT
			if (id.Length > 32)
				throw new ArgumentException ("id parameter shouldn't be longer than 32 characters");
#endif

			if (adjustmentRules != null && adjustmentRules.Length != 0) {
				AdjustmentRule prev = null;
				foreach (AdjustmentRule current in adjustmentRules) {
					if (current == null)
						throw new InvalidTimeZoneException ("one or more elements in adjustmentRules are null");

					if ((baseUtcOffset + current.DaylightDelta < new TimeSpan (-14, 0, 0)) ||
							(baseUtcOffset + current.DaylightDelta > new TimeSpan (14, 0, 0)))
						throw new InvalidTimeZoneException ("Sum of baseUtcOffset and DaylightDelta of one or more object in adjustmentRules array is greater than 14 or less than -14 hours;");

					if (prev != null && prev.DateStart > current.DateStart)
						throw new InvalidTimeZoneException ("adjustment rules specified in adjustmentRules parameter are not in chronological order");
					
					if (prev != null && prev.DateEnd > current.DateStart)
						throw new InvalidTimeZoneException ("some adjustment rules in the adjustmentRules parameter overlap");

					if (prev != null && prev.DateEnd == current.DateStart)
						throw new InvalidTimeZoneException ("a date can have multiple adjustment rules applied to it");

					prev = current;
				}
			}
			
			this.id = id;
			this.baseUtcOffset = baseUtcOffset;
			this.displayName = displayName ?? id;
			this.standardDisplayName = standardDisplayName ?? id;
			this.daylightDisplayName = daylightDisplayName;
			this.disableDaylightSavingTime = disableDaylightSavingTime;
			this.adjustmentRules = adjustmentRules;
		}

		private AdjustmentRule GetApplicableRule (DateTime dateTime)
		{
			//Transitions are always in standard time
			DateTime date = dateTime;

			if (dateTime.Kind == DateTimeKind.Local && this != TimeZoneInfo.Local)
				date = date.ToUniversalTime () + BaseUtcOffset;

			if (dateTime.Kind == DateTimeKind.Utc && this != TimeZoneInfo.Utc)
				date = date + BaseUtcOffset;

			foreach (AdjustmentRule rule in adjustmentRules) {
				if (rule.DateStart > date.Date)
					return null;
				if (rule.DateEnd < date.Date)
					continue;
				return rule;
			}
			return null;
		}

		private static DateTime TransitionPoint (TransitionTime transition, int year)
		{
			if (transition.IsFixedDateRule)
				return new DateTime (year, transition.Month, transition.Day) + transition.TimeOfDay.TimeOfDay;

			DayOfWeek first = (new DateTime (year, transition.Month, 1)).DayOfWeek;
			int day = 1 + (transition.Week - 1) * 7 + (transition.DayOfWeek - first) % 7;
			if (day >  DateTime.DaysInMonth (year, transition.Month))
				day -= 7;
			return new DateTime (year, transition.Month, day) + transition.TimeOfDay.TimeOfDay;
		}

#if LIBC
		private static bool ValidTZFile (byte [] buffer, int length)
		{
			StringBuilder magic = new StringBuilder ();

			for (int i = 0; i < 4; i++)
				magic.Append ((char)buffer [i]);
			
			if (magic.ToString () != "TZif")
				return false;

			if (length >= BUFFER_SIZE)
				return false;

			return true;
		}

		struct TimeType 
		{
			public readonly int Offset;
			public readonly bool IsDst;
			public string Name;

			public TimeType (int offset, bool is_dst, string abbrev)
			{
				this.Offset = offset;
				this.IsDst = is_dst;
				this.Name = abbrev;
			}

			public override string ToString ()
			{
				return "offset: " + Offset + "s, is_dst: " + IsDst + ", zone name: " + Name;
			}
		}

		static int SwapInt32 (int i)
		{
			return (((i >> 24) & 0xff)
				| ((i >> 8) & 0xff00)
				| ((i << 8) & 0xff0000)
				| ((i << 24)));
		}

		static int ReadBigEndianInt32 (byte [] buffer, int start)
		{
			int i = BitConverter.ToInt32 (buffer, start);
			if (!BitConverter.IsLittleEndian)
				return i;

			return SwapInt32 (i);
		}

		private static TimeZoneInfo ParseTZBuffer (string id, byte [] buffer, int length)
		{
			//Reading the header. 4 bytes for magic, 16 are reserved
			int ttisgmtcnt = ReadBigEndianInt32 (buffer, 20);
			int ttisstdcnt = ReadBigEndianInt32 (buffer, 24);
			int leapcnt = ReadBigEndianInt32 (buffer, 28);
			int timecnt = ReadBigEndianInt32 (buffer, 32);
			int typecnt = ReadBigEndianInt32 (buffer, 36);
			int charcnt = ReadBigEndianInt32 (buffer, 40);

			if (length < 44 + timecnt * 5 + typecnt * 6 + charcnt + leapcnt * 8 + ttisstdcnt + ttisgmtcnt)
				throw new InvalidTimeZoneException ();

			Dictionary<int, string> abbreviations = ParseAbbreviations (buffer, 44 + 4 * timecnt + timecnt + 6 * typecnt, charcnt);
			Dictionary<int, TimeType> time_types = ParseTimesTypes (buffer, 44 + 4 * timecnt + timecnt, typecnt, abbreviations);
			List<KeyValuePair<DateTime, TimeType>> transitions = ParseTransitions (buffer, 44, timecnt, time_types);

			if (time_types.Count == 0)
				throw new InvalidTimeZoneException ();

			if (time_types.Count == 1 && ((TimeType)time_types[0]).IsDst)
				throw new InvalidTimeZoneException ();

			TimeSpan baseUtcOffset = new TimeSpan (0);
			TimeSpan dstDelta = new TimeSpan (0);
			string standardDisplayName = null;
			string daylightDisplayName = null;
			bool dst_observed = false;
			DateTime dst_start = DateTime.MinValue;
			List<AdjustmentRule> adjustmentRules = new List<AdjustmentRule> ();

			for (int i = 0; i < transitions.Count; i++) {
				var pair = transitions [i];
				DateTime ttime = pair.Key;
				TimeType ttype = pair.Value;
				if (!ttype.IsDst) {
					if (standardDisplayName != ttype.Name || baseUtcOffset.TotalSeconds != ttype.Offset) {
						standardDisplayName = ttype.Name;
						daylightDisplayName = null;
						baseUtcOffset = new TimeSpan (0, 0, ttype.Offset);
						adjustmentRules = new List<AdjustmentRule> ();
						dst_observed = false;
					}
					if (dst_observed) {
						//FIXME: check additional fields for this:
						//most of the transitions are expressed in GMT 
						dst_start += baseUtcOffset;
						DateTime dst_end = ttime + baseUtcOffset + dstDelta;

						//some weird timezone (America/Phoenix) have end dates on Jan 1st
						if (dst_end.Date == new DateTime (dst_end.Year, 1, 1) && dst_end.Year > dst_start.Year)
							dst_end -= new TimeSpan (24, 0, 0);

						DateTime dateStart, dateEnd;
						if (dst_start.Month < 7)
							dateStart = new DateTime (dst_start.Year, 1, 1);
						else
							dateStart = new DateTime (dst_start.Year, 7, 1);

						if (dst_end.Month >= 7)
							dateEnd = new DateTime (dst_end.Year, 12, 31);
						else
							dateEnd = new DateTime (dst_end.Year, 6, 30);

						
						TransitionTime transition_start = TransitionTime.CreateFixedDateRule (new DateTime (1, 1, 1) + dst_start.TimeOfDay, dst_start.Month, dst_start.Day);
						TransitionTime transition_end = TransitionTime.CreateFixedDateRule (new DateTime (1, 1, 1) + dst_end.TimeOfDay, dst_end.Month, dst_end.Day);
						adjustmentRules.Add (AdjustmentRule.CreateAdjustmentRule (dateStart, dateEnd, dstDelta, transition_start, transition_end));
					}
					dst_observed = false;
				} else {
					if (daylightDisplayName != ttype.Name || dstDelta.TotalSeconds != ttype.Offset - baseUtcOffset.TotalSeconds) {
						daylightDisplayName = ttype.Name;
						dstDelta = new TimeSpan(0, 0, ttype.Offset) - baseUtcOffset;
					}
					dst_start = ttime;
					dst_observed = true;
				}
			}

			if (adjustmentRules.Count == 0) {
				TimeType t = (TimeType)time_types [0];
				if (standardDisplayName == null) {
					standardDisplayName = t.Name;
					baseUtcOffset = new TimeSpan (0, 0, t.Offset);
				}
				return CreateCustomTimeZone (id, baseUtcOffset, id, standardDisplayName);
			} else {
				return CreateCustomTimeZone (id, baseUtcOffset, id, standardDisplayName, daylightDisplayName, adjustmentRules.ToArray ());
			}
		}

		static Dictionary<int, string> ParseAbbreviations (byte [] buffer, int index, int count)
		{
			var abbrevs = new Dictionary<int, string> ();
			int abbrev_index = 0;
			var sb = new StringBuilder ();
			for (int i = 0; i < count; i++) {
				char c = (char) buffer [index + i];
				if (c != '\0')
					sb.Append (c);
				else {
					abbrevs.Add (abbrev_index, sb.ToString ());
					abbrev_index = i + 1;
					sb = new StringBuilder ();
				}
			}
			return abbrevs;
		}

		static Dictionary<int, TimeType> ParseTimesTypes (byte [] buffer, int index, int count, Dictionary<int, string> abbreviations)
		{
			var types = new Dictionary<int, TimeType> (count);
			for (int i = 0; i < count; i++) {
				int offset = ReadBigEndianInt32 (buffer, index + 6 * i);
				byte is_dst = buffer [index + 6 * i + 4];
				byte abbrev = buffer [index + 6 * i + 5];
				types.Add (i, new TimeType (offset, (is_dst != 0), abbreviations [(int)abbrev]));
			}
			return types;
		}

		static List<KeyValuePair<DateTime, TimeType>> ParseTransitions (byte [] buffer, int index, int count, Dictionary<int, TimeType> time_types)
		{
			var list = new List<KeyValuePair<DateTime, TimeType>> (count);
			for (int i = 0; i < count; i++) {
				int unixtime = ReadBigEndianInt32 (buffer, index + 4 * i);
				DateTime ttime = DateTimeFromUnixTime (unixtime);
				byte ttype = buffer [index + 4 * count + i];
				list.Add (new KeyValuePair<DateTime, TimeType> (ttime, time_types [(int)ttype]));
			}
			return list;
		}

		static DateTime DateTimeFromUnixTime (long unix_time)
		{
			DateTime date_time = new DateTime (1970, 1, 1);
			return date_time.AddSeconds (unix_time);
		}
#endif
	}
}
