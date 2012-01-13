//CHANGED
//
// System.Globalization.CultureInfo.cs
//
// Miguel de Icaza (miguel@ximian.com)
// Dick Porter (dick@ximian.com)
//
// (C) 2001, 2002, 2003 Ximian, Inc. (http://www.ximian.com)
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

using System.Collections;

namespace System.Globalization
{
    [Serializable]
    public partial class CultureInfo : ICloneable, IFormatProvider
    {
        #region Fields
        internal static int BootstrapCultureID;

        const int NumOptionalCalendars = 5;
        const int GregorianTypeMask = 0x00FFFFFF;
        const int CalendarTypeBits = 24;

        int m_lcid;
        bool m_isReadOnly;
        bool m_useUserOverride;
        private bool m_isSpecific;
        private bool m_isInstalled;

#if NOT_PFX
        [NonSerialized]
#endif
        int m_parent_lcid;

#if NOT_PFX
        [NonSerialized]
#endif
        int m_specific_lcid;

#if NOT_PFX
        [NonSerialized]
#endif
        volatile NumberFormatInfo m_numInfo;
        volatile DateTimeFormatInfo m_dateTimeInfo;
        volatile TextInfo m_textInfo;

        private string m_name;
#if NOT_PFX
        [NonSerialized]
#endif
        private string m_displayName;
#if NOT_PFX
        [NonSerialized]
#endif
        private string m_englishName;
#if NOT_PFX
        [NonSerialized]
#endif
        private string m_nativeName;
#if NOT_PFX
        [NonSerialized]
#endif
        private string m_iso3lang;
#if NOT_PFX
        [NonSerialized]
#endif
        private string m_iso2lang;
#if NOT_PFX
        [NonSerialized]
#endif
        private string m_icuName;
#if NOT_PFX
        [NonSerialized]
#endif
        private string m_win3lang;
#if NOT_PFX
        [NonSerialized]
#endif
        private string m_territory;

        CompareInfo m_compareInfo;

#if NOT_PFX
        [NonSerialized]
#endif
        private Calendar[] optional_calendars;
#if NOT_PFX
        [NonSerialized]
#endif
        CultureInfo m_parent_culture;

        private static readonly string MSG_READONLY = "This instance is read only";
        #endregion

        #region Shared Members
        public static CultureInfo CurrentCulture
        {
            get
            {
                if (_currentCulture == null)
                {
                    //string lang = flash.system.Capabilities.language;
                    //int lcid = NameToLCID(lang);
                    //if (lcid <= 0)
                    //    lcid = 1033; //english
                    //_currentCulture = new CultureInfo(lcid);
                    _currentCulture = InvariantCulture;
                }
                return _currentCulture;
            }
            set
            {
                if (value == null)
                {
                    _currentCulture = InvariantCulture;
                }
                else
                {
                    _currentCulture = value;
                }
            }
        }
        private static CultureInfo _currentCulture;

        public static CultureInfo CurrentUICulture
        {
            get
            {
                if (_currentUICulture == null)
                    _currentUICulture = CurrentCulture;
                return _currentUICulture;
            }
            set
            {
                if (value == null)
                {
                    _currentUICulture = InvariantCulture;
                }
                else
                {
                    _currentUICulture = value;
                }
            }
        }
        private static CultureInfo _currentUICulture;

        internal const int InvariantCultureId = 0x7F;

        static public CultureInfo InvariantCulture
        {
            get
            {
                if (_invariantCulture == null)
                {
                    _invariantCulture = new CultureInfo(InvariantCultureId, false, true);
                }
                return _invariantCulture;
            }
        }
        static CultureInfo _invariantCulture;
        #endregion

        #region Constructors
        internal CultureInfo(int culture)
            : this(culture, true)
        {
        }

        internal CultureInfo(int culture, bool use_user_override)
            : this(culture, use_user_override, false)
        {
        }

        private CultureInfo(int culture, bool use_user_override, bool read_only)
        {
            if (culture < 0)
                throw new ArgumentOutOfRangeException("culture", "Positive "
                    + "number required.");

            m_isReadOnly = read_only;
            m_useUserOverride = use_user_override;

            m_lcid = culture;
            if (culture == InvariantCultureId)
            {
                /* Short circuit the invariant culture */
                ConstructInvariant();
                return;
            }

            ConstructFromLcid(culture);
        }

        public CultureInfo(string name)
            : this(name, true)
        {
        }

        internal CultureInfo(string name, bool use_user_override)
            : this(name, use_user_override, false)
        {
        }

        private CultureInfo(string name, bool use_user_override, bool read_only)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            m_isReadOnly = read_only;
            m_useUserOverride = use_user_override;

            if (name.Length == 0)
            {
                /* Short circuit the invariant culture */
                ConstructInvariant();
                return;
            }

            ConstructFromName(name.ToLowerInvariant());
        }

        private void ConstructFromName(string name)
        {
            int lcid = NameToLCID(name);
            if (lcid == 0) 
                throw new ArgumentException("Culture name " + name + " is not supported.", "name");
            ConstructFromLcid(lcid);
            m_lcid = lcid;
        }

        private void ConstructInvariant()
        {
            m_lcid = InvariantCultureId;

            /* NumberFormatInfo defaults to the invariant data */
            m_numInfo = NumberFormatInfo.InvariantInfo;
            /* DateTimeFormatInfo defaults to the invariant data */
            m_dateTimeInfo = DateTimeFormatInfo.InvariantInfo;

            if (!m_isReadOnly)
            {
                m_numInfo = (NumberFormatInfo)m_numInfo.Clone();
                m_dateTimeInfo = (DateTimeFormatInfo)m_dateTimeInfo.Clone();
            }

            m_textInfo = new TextInfo(this, InvariantCultureId);

            m_name = "";
            m_displayName = m_englishName = m_nativeName = "Invariant Language (Invariant Country)";
            m_iso3lang = "IVL";
            m_iso2lang = "iv";
            m_icuName = "en_US_POSIX";
            m_win3lang = "IVL";
        }
        #endregion

        #region Properties
        // it is used for RegionInfo.
        internal string Territory
        {
            get { return m_territory; }
        }

        internal virtual int LCID
        {
            get { return m_lcid; }
        }

        public virtual string Name
        {
            get { return m_name; }
        }

        public virtual string NativeName
        {
            get { return m_nativeName; }
        }

        public virtual Calendar Calendar
        {
            get { return DateTimeFormat.Calendar; }
        }

        public virtual Calendar[] OptionalCalendars
        {
            get
            {
                if (optional_calendars == null)
                    ConstructCalendars();
                return optional_calendars;
            }
        }

        public virtual CultureInfo Parent
        {
            get
            {
                if (m_parent_culture == null)
                {
                    if (m_parent_lcid == m_lcid)
                        return null;
                    if (m_parent_lcid == InvariantCultureId)
                        m_parent_culture = InvariantCulture;
                    else if (m_lcid == InvariantCultureId)
                        m_parent_culture = this;
                    else
                        m_parent_culture = new CultureInfo(m_parent_lcid);
                }
                return m_parent_culture;
            }
        }

        public virtual TextInfo TextInfo
        {
            get
            {
                if (m_textInfo == null)
                    m_textInfo = new TextInfo(this, m_lcid);
                return m_textInfo;
            }
        }

        internal virtual string ThreeLetterISOLanguageName
        {
            get { return m_iso3lang; }
        }

        internal virtual string ThreeLetterWindowsLanguageName
        {
            get { return m_win3lang; }
        }

        public virtual string TwoLetterISOLanguageName
        {
            get { return m_iso2lang; }
        }

        internal bool UseUserOverride
        {
            get { return m_useUserOverride; }
        }

        internal string IcuName
        {
            get { return m_icuName; }
        }

        internal bool IsSpecific
        {
            get { return m_isSpecific; }
        }

        internal bool IsInstalled
        {
            get { return m_isInstalled; }
        }
        #endregion

        public virtual object Clone()
        {
            CultureInfo ci = (CultureInfo)MemberwiseClone();
            ci.m_isReadOnly = false;
            if (!IsNeutralCulture)
            {
                ci.NumberFormat = (NumberFormatInfo)NumberFormat.Clone();
                ci.DateTimeFormat = (DateTimeFormatInfo)DateTimeFormat.Clone();
            }
            return ci;
        }

        public override bool Equals(object value)
        {
            CultureInfo b = value as CultureInfo;

            if (b != null)
                return b.m_lcid == m_lcid;
            return false;
        }

        internal static CultureInfo[] GetCultures(CultureTypes types)
        {
            bool neutral = ((types & CultureTypes.NeutralCultures) != 0);
            bool specific = ((types & CultureTypes.SpecificCultures) != 0);
            bool installed = ((types & CultureTypes.InstalledWin32Cultures) != 0);  // TODO

            int[] ids = get_lcids();
            if (ids == null)
                return new CultureInfo[0];

            int n = ids.Length;
            if (n <= 0)
                return new CultureInfo[0];

            CultureInfo[] infos = new CultureInfo[n];
            int k = 0;
            for (int i = 0; i < n; ++i)
            {
                CultureInfo info = new CultureInfo(ids[i]);
#if ONLY_1_1
                info.m_useUserOverride = true;
#endif
                if (neutral && !info.IsNeutralCulture)
                    continue;
                if (specific && !info.IsSpecific)
                    continue;
                if (installed && !info.IsInstalled)
                    continue;
                infos[k++] = info;
            }

            CultureInfo[] result = new CultureInfo[k];
            for (int i = 0; i < k; ++i)
                result[i] = infos[i];

            return infos;
        }

        public override int GetHashCode()
        {
            return m_lcid;
        }

        public static CultureInfo ReadOnly(CultureInfo ci)
        {
            if (ci == null)
            {
                throw new ArgumentNullException("ci");
            }

            if (ci.m_isReadOnly)
            {
                return ci;
            }
            else
            {
                CultureInfo newci = (CultureInfo)ci.Clone();
                newci.m_isReadOnly = true;
                return newci;
            }
        }

        public override string ToString()
        {
            return m_name;
        }

        public virtual CompareInfo CompareInfo
        {
            get
            {
                if (m_compareInfo == null)
                    m_compareInfo = new CompareInfo(this);
                return m_compareInfo;
            }
        }

        public virtual bool IsNeutralCulture
        {
            get
            {
                if (m_lcid == InvariantCultureId) return false;
                return (m_lcid & 0xff00) == 0;
                //return ((m_lcid & 0xff00) == 0 || m_specific_lcid == 0);
            }
        }

        internal void CheckNeutral()
        {
            if (IsNeutralCulture)
            {
                throw new NotSupportedException("Culture \"" + m_name + "\" is " +
                        "a neutral culture. It can not be used in formatting " +
                        "and parsing and therefore cannot be set as the thread's " +
                        "current culture.");
            }
        }

        public virtual NumberFormatInfo NumberFormat
        {
            get
            {
                CheckNeutral();
                if (m_numInfo == null)
                    m_numInfo = new NumberFormatInfo(m_lcid);
                return m_numInfo;
            }

            set
            {
                if (m_isReadOnly) throw new InvalidOperationException(MSG_READONLY);
                if (value == null)
                    throw new ArgumentNullException("value");
                m_numInfo = value;
            }
        }

        public virtual DateTimeFormatInfo DateTimeFormat
        {
            get
            {
                CheckNeutral();
                if (m_dateTimeInfo == null)
                {
                    m_dateTimeInfo = new DateTimeFormatInfo(m_lcid);
                    if (optional_calendars != null)
                        m_dateTimeInfo.Calendar = optional_calendars[0];
                }
                return m_dateTimeInfo;
            }

            set
            {
                if (m_isReadOnly) throw new InvalidOperationException(MSG_READONLY);
                if (value == null)
                    throw new ArgumentNullException("value");
                m_dateTimeInfo = value;
            }
        }

        public virtual string DisplayName
        {
            get
            {
                return m_displayName;
            }
        }

        public virtual string EnglishName
        {
            get
            {
                return m_englishName;
            }
        }

        public bool IsReadOnly
        {
            get { return m_isReadOnly; }
        }


        // 
        // IFormatProvider implementation
        //
        public virtual object GetFormat(Type formatType)
        {
            object format = null;

            if (formatType == typeof(NumberFormatInfo))
                format = NumberFormat;
            else if (formatType == typeof(DateTimeFormatInfo))
                format = DateTimeFormat;

            return format;
        }

//#if NET_2_0
		static Hashtable shared_by_number, shared_by_name;
		
		static void insert_into_shared_tables (CultureInfo c)
		{
			if (shared_by_number == null){
				shared_by_number = new Hashtable ();
				shared_by_name = new Hashtable ();
			}
			shared_by_number [c.m_lcid] = c;
			shared_by_name [c.m_name] = c;
		}

        internal static CultureInfo GetCultureInfo(int culture)
        {
            CultureInfo c;

            //lock (shared_table_lock)
            {
                if (shared_by_number != null)
                {
                    c = shared_by_number[culture] as CultureInfo;

                    if (c != null)
                        return (CultureInfo)c;
                }
                c = new CultureInfo(culture, false, true);
                insert_into_shared_tables(c);
                return c;
            }
        }

        internal static CultureInfo GetCultureInfo(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            CultureInfo c;
            //lock (shared_table_lock)
            {
                if (shared_by_name != null)
                {
                    c = shared_by_name[name] as CultureInfo;

                    if (c != null)
                        return (CultureInfo)c;
                }
                c = new CultureInfo(name, false, true);
                insert_into_shared_tables(c);
                return c;
            }
        }

        [MonoTODO("Currently it ignores the altName parameter")]
        internal static CultureInfo GetCultureInfo(string name, string altName)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            if (altName == null)
                throw new ArgumentNullException("altName");

            return GetCultureInfo(name);
        }

        internal static CultureInfo GetCultureInfoByIetfLanguageTag(string name)
        {
            // There could be more consistent way to implement
            // it, but right now it works just fine with this...
            switch (name)
            {
                case "zh-Hans":
                    return GetCultureInfo("zh-CHS");
                case "zh-Hant":
                    return GetCultureInfo("zh-CHT");
                default:
                    return GetCultureInfo(name);
            }
        }

//#endif

        internal void ConstructCalendars()
        {
            //if (calendar_data == null) 
            //{
            //    optional_calendars = new Calendar [] {new GregorianCalendar (GregorianCalendarTypes.Localized)};
            //    return;
            //}

            //optional_calendars = new Calendar [NumOptionalCalendars];

            //for (int i=0; i<NumOptionalCalendars; i++) {
            //    Calendar cal = null;
            //    int caldata = *(calendar_data + i);
            //    int caltype = (caldata >> CalendarTypeBits);
            //    switch (caltype) {
            //    case 0:
            //        GregorianCalendarTypes greg_type;
            //        greg_type = (GregorianCalendarTypes) (caldata & GregorianTypeMask);
            //        cal = new GregorianCalendar (greg_type);
            //        break;
            //    case 1:
            //        cal = new HijriCalendar ();
            //        break;
            //    case 2:
            //        cal = new ThaiBuddhistCalendar ();
            //        break;
            //    default:
            //        throw new Exception ("invalid calendar type:  " + caldata);
            //    }
            //    optional_calendars [i] = cal;
            //}
        }
    }
}
