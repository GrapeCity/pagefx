//CHANGED

//
// RegionInfo.cs
//
// Author:
//	Atsushi Enomoto  <atsushi@ximian.com>
//

//
// Copyright (C) 2005 Novell, Inc (http://www.novell.com)
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
using System.Globalization;
using System.Runtime.CompilerServices;

namespace System.Globalization
{
    [Serializable]
    public partial class RegionInfo
    {
        static RegionInfo currentRegion;

        // This property is not synchronized with CurrentCulture, so
        // we need to use bootstrap CurrentCulture LCID.
        public static RegionInfo CurrentRegion
        {
            get
            {
                if (currentRegion == null)
                {
                    // make sure to fill BootstrapCultureID.
                    CultureInfo ci = CultureInfo.CurrentCulture;
                    // If current culture is invariant then region is not available.
                    if (ci == null || CultureInfo.BootstrapCultureID == 0x7F)
                        return null;
                    currentRegion = new RegionInfo(CultureInfo.BootstrapCultureID);
                }
                return currentRegion;
            }
        }

#if NET_2_0
		int lcid; // it is used only for Equals() (not even used in GetHashCode()).
#endif
        int regionId;
        string iso2Name;
        string iso3Name;
        string win3Name;
        string englishName;
        string currencySymbol;
        string isoCurrencySymbol;
#if NET_2_0
        string currencyEnglishName;
        string currencyNativeName;
#endif

        internal RegionInfo(int lcid)
        {
            ConstructFromLcid(lcid);
#if NET_2_0
            this.lcid = lcid;
#endif
        }

        public RegionInfo(string name)
        {
            if (name == null)
                throw new ArgumentNullException();
        }

#if NET_2_0
		[System.Runtime.InteropServices.ComVisible(false)]
		public virtual string CurrencyEnglishName 
        {
			get { return currencyEnglishName; }
		}
#endif

        public virtual string CurrencySymbol
        {
            get { return currencySymbol; }
        }

        [MonoTODO("DisplayName currently only returns the EnglishName")]
        public virtual string DisplayName
        {
            get { return englishName; }
        }

        public virtual string EnglishName
        {
            get { return englishName; }
        }

#if NET_2_0
		[System.Runtime.InteropServices.ComVisible(false)]
		public virtual int GeoId 
        {
			get { return regionId; }
		}
#endif

        public virtual bool IsMetric
        {
            get
            {
                switch (iso2Name)
                {
                    case "US":
                    case "UK":
                        return false;
                    default:
                        return true;
                }
            }
        }

        public virtual string ISOCurrencySymbol
        {
            get { return isoCurrencySymbol; }
        }

#if NET_2_0
		public virtual string NativeName 
        {
			get { return DisplayName; }
		}

		public virtual string CurrencyNativeName 
        {
			get { return currencyNativeName; }
		}
#endif

        public virtual string Name
        {
            get { return iso2Name; }
        }

        internal virtual string ThreeLetterISORegionName
        {
            get { return iso3Name; }
        }

        internal virtual string ThreeLetterWindowsRegionName
        {
            get { return win3Name; }
        }

        public virtual string TwoLetterISORegionName
        {
            get { return iso2Name; }
        }

        //
        // methods

#if NET_2_0
		public override bool Equals (object value)
		{
			RegionInfo other = value as RegionInfo;
			return other != null && lcid == other.lcid;
		}

		public override int GetHashCode ()
		{
			return (int) (0x80000000 + (regionId << 3) + regionId); // it i still based on regionId
		}
#else
        public override bool Equals(object value)
        {
            RegionInfo other = value as RegionInfo;
            return other != null && regionId == other.regionId;
        }

        public override int GetHashCode()
        {
            return regionId;
        }
#endif

        public override string ToString()
        {
            return Name;
        }
    }
}
