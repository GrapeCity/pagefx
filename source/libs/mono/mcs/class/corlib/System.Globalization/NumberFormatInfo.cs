//CHANGED

//
// System.Globalization.NumberFormatInfo.cs
//
// Author:
//   Derek Holden (dholden@draper.com)
//   Bob Smith    (bob@thestuff.net)
//   Mohammad DAMT (mdamt@cdl2000.com)
//
// (C) Derek Holden
// (C) Bob Smith     http://www.thestuff.net
// (c) 2003, PT Cakram Datalingga Duaribu   http://www.cdl2000.com
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

//
// NumberFormatInfo. One can only assume it is the class gotten
// back from a GetFormat() method from an IFormatProvider / 
// IFormattable implementer. There are some discrepencies with the
// ECMA spec and the SDK docs, surprisingly. See my conversation
// with myself on it at:
// http://lists.ximian.com/archives/public/mono-list/2001-July/000794.html
// 
// Other than that this is totally ECMA compliant.
//

namespace System.Globalization
{
    [Serializable]
    public sealed partial class NumberFormatInfo : ICloneable, IFormatProvider
    {
        private bool isReadOnly;

        // Currency Related Format Info
        private int currencyDecimalDigits;
        private string currencyDecimalSeparator;
        private string currencyGroupSeparator;
        private int[] currencyGroupSizes;
        private int currencyNegativePattern;
        private int currencyPositivePattern;
        private string currencySymbol;

        private string nanSymbol;
        private string negativeInfinitySymbol;
        private string negativeSign;

        // Number Related Format Info
        private int numberDecimalDigits;
        private string numberDecimalSeparator;
        private string numberGroupSeparator;
        private int[] numberGroupSizes;
        private int numberNegativePattern;

        // Percent Related Format Info
        private int percentDecimalDigits;
        private string percentDecimalSeparator;
        private string percentGroupSeparator;
        private int[] percentGroupSizes;
        private int percentNegativePattern;
        private int percentPositivePattern;
        private string percentSymbol;

        private string perMilleSymbol;
        private string positiveInfinitySymbol;
        private string positiveSign;

        string ansiCurrencySymbol;	// TODO, MS.NET serializes this.
        int m_dataItem;	// Unused, but MS.NET serializes this.
        bool m_useUserOverride; // Unused, but MS.NET serializes this.
        bool validForParseAsNumber; // Unused, but MS.NET serializes this.
        bool validForParseAsCurrency; // Unused, but MS.NET serializes this.
#if NET_2_0
		string[] nativeDigits; // Unused, but MS.NET serializes this.
		int digitSubstitution; // Unused, but MS.NET serializes this.
#endif

        internal NumberFormatInfo(int lcid)
        {
            ConstructFromLcid(lcid);

            //switch (lcid)
            //{

            //    // The Invariant Culture Info ID.
            //    case CultureInfo.InvariantCultureId:
            //        isReadOnly = false;

            //        // Currency Related Format Info
            //        currencyDecimalDigits = 2;
            //        currencyDecimalSeparator = ".";
            //        currencyGroupSeparator = ",";
            //        currencyGroupSizes = new int[1] { 3 };
            //        currencyNegativePattern = 0;
            //        currencyPositivePattern = 0;
            //        currencySymbol = "$";

            //        nanSymbol = "NaN";
            //        negativeInfinitySymbol = "-Infinity";
            //        negativeSign = "-";

            //        // Number Related Format Info
            //        numberDecimalDigits = 2;
            //        numberDecimalSeparator = ".";
            //        numberGroupSeparator = ",";
            //        numberGroupSizes = new int[1] { 3 };
            //        numberNegativePattern = 1;

            //        // Percent Related Format Info
            //        percentDecimalDigits = 2;
            //        percentDecimalSeparator = ".";
            //        percentGroupSeparator = ",";
            //        percentGroupSizes = new int[1] { 3 };
            //        percentNegativePattern = 0;
            //        percentPositivePattern = 0;
            //        percentSymbol = "%";

            //        perMilleSymbol = "\u2030";
            //        positiveInfinitySymbol = "Infinity";
            //        positiveSign = "+";
            //        break;
            //}
        }

        public NumberFormatInfo()
            : this(CultureInfo.InvariantCultureId)
        {
        }

        // =========== Currency Format Properties =========== //

        public int CurrencyDecimalDigits
        {
            get
            {
                return currencyDecimalDigits;
            }

            set
            {
                if (value < 0 || value > 99)
                    throw new ArgumentOutOfRangeException
                    ("The value specified for the property is less than 0 or greater than 99");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                currencyDecimalDigits = value;
            }
        }

        public string CurrencyDecimalSeparator
        {
            get
            {
                return currencyDecimalSeparator;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException
                    ("The value specified for the property is a null reference");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                currencyDecimalSeparator = value;
            }
        }


        public string CurrencyGroupSeparator
        {
            get
            {
                return currencyGroupSeparator;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException
                    ("The value specified for the property is a null reference");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                currencyGroupSeparator = value;
            }
        }

        public int[] CurrencyGroupSizes
        {
            get
            {
                return (int[])currencyGroupSizes.Clone();
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException
                    ("The value specified for the property is a null reference");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                if (value.Length == 0)
                {
                    currencyGroupSizes = new int[0];
                    return;
                }

                // All elements except last need to be in range 1 - 9, last can be 0.
                int last = value.Length - 1;

                for (int i = 0; i < last; i++)
                    if (value[i] < 1 || value[i] > 9)
                        throw new ArgumentOutOfRangeException
                        ("One of the elements in the array specified is not between 1 and 9");

                if (value[last] < 0 || value[last] > 9)
                    throw new ArgumentOutOfRangeException
                    ("Last element in the array specified is not between 0 and 9");

                currencyGroupSizes = (int[])value.Clone();
            }
        }

        public int CurrencyNegativePattern
        {
            get
            {
                // See ECMA NumberFormatInfo page 8
                return currencyNegativePattern;
            }

            set
            {
                if (value < 0 || value > 15)
                    throw new ArgumentOutOfRangeException
                    ("The value specified for the property is less than 0 or greater than 15");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                currencyNegativePattern = value;
            }
        }

        public int CurrencyPositivePattern
        {
            get
            {
                // See ECMA NumberFormatInfo page 11 
                return currencyPositivePattern;
            }

            set
            {
                if (value < 0 || value > 3)
                    throw new ArgumentOutOfRangeException
                    ("The value specified for the property is less than 0 or greater than 3");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                currencyPositivePattern = value;
            }
        }

        public string CurrencySymbol
        {
            get
            {
                return currencySymbol;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException
                    ("The value specified for the property is a null reference");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                currencySymbol = value;
            }
        }

        // =========== Static Read-Only Properties =========== //

        public static NumberFormatInfo CurrentInfo
        {
            get
            {
                NumberFormatInfo nfi = (NumberFormatInfo)CultureInfo.CurrentCulture.NumberFormat;
                nfi.isReadOnly = true;
                return nfi;
            }
        }

        public static NumberFormatInfo InvariantInfo
        {
            get
            {
                // This uses invariant info, which is same as in the constructor
                NumberFormatInfo nfi = new NumberFormatInfo();
                nfi.NumberNegativePattern = 1;
                nfi.isReadOnly = true;
                return nfi;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return isReadOnly;
            }
        }



        public string NaNSymbol
        {
            get
            {
                return nanSymbol;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException
                    ("The value specified for the property is a null reference");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                nanSymbol = value;
            }
        }

        public string NegativeInfinitySymbol
        {
            get
            {
                return negativeInfinitySymbol;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException
                    ("The value specified for the property is a null reference");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                negativeInfinitySymbol = value;
            }
        }

        public string NegativeSign
        {
            get
            {
                return negativeSign;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException
                    ("The value specified for the property is a null reference");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                negativeSign = value;
            }
        }

        // =========== Number Format Properties =========== //

        public int NumberDecimalDigits
        {
            get
            {
                return numberDecimalDigits;
            }

            set
            {
                if (value < 0 || value > 99)
                    throw new ArgumentOutOfRangeException
                    ("The value specified for the property is less than 0 or greater than 99");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                numberDecimalDigits = value;
            }
        }

        public string NumberDecimalSeparator
        {
            get
            {
                return numberDecimalSeparator;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException
                    ("The value specified for the property is a null reference");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                numberDecimalSeparator = value;
            }
        }


        public string NumberGroupSeparator
        {
            get
            {
                return numberGroupSeparator;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException
                    ("The value specified for the property is a null reference");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                numberGroupSeparator = value;
            }
        }

        public int[] NumberGroupSizes
        {
            get
            {
                return (int[])numberGroupSizes.Clone();
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException
                    ("The value specified for the property is a null reference");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                if (value.Length == 0)
                {
                    numberGroupSizes = new int[0];
                    return;
                }
                // All elements except last need to be in range 1 - 9, last can be 0.
                int last = value.Length - 1;

                for (int i = 0; i < last; i++)
                    if (value[i] < 1 || value[i] > 9)
                        throw new ArgumentOutOfRangeException
                        ("One of the elements in the array specified is not between 1 and 9");

                if (value[last] < 0 || value[last] > 9)
                    throw new ArgumentOutOfRangeException
                    ("Last element in the array specified is not between 0 and 9");

                numberGroupSizes = (int[])value.Clone();
            }
        }

        public int NumberNegativePattern
        {
            get
            {
                // See ECMA NumberFormatInfo page 27
                return numberNegativePattern;
            }

            set
            {
                if (value < 0 || value > 4)
                    throw new ArgumentOutOfRangeException
                    ("The value specified for the property is less than 0 or greater than 15");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                numberNegativePattern = value;
            }
        }

        // =========== Percent Format Properties =========== //

        public int PercentDecimalDigits
        {
            get
            {
                return percentDecimalDigits;
            }

            set
            {
                if (value < 0 || value > 99)
                    throw new ArgumentOutOfRangeException
                    ("The value specified for the property is less than 0 or greater than 99");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                percentDecimalDigits = value;
            }
        }

        public string PercentDecimalSeparator
        {
            get
            {
                return percentDecimalSeparator;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException
                    ("The value specified for the property is a null reference");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                percentDecimalSeparator = value;
            }
        }


        public string PercentGroupSeparator
        {
            get
            {
                return percentGroupSeparator;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException
                    ("The value specified for the property is a null reference");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                percentGroupSeparator = value;
            }
        }

        public int[] PercentGroupSizes
        {
            get
            {
                return (int[])percentGroupSizes.Clone();
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException
                    ("The value specified for the property is a null reference");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                if (this == CultureInfo.CurrentCulture.NumberFormat)
                    throw new Exception("HERE the value was modified");

                if (value.Length == 0)
                {
                    percentGroupSizes = new int[0];
                    return;
                }

                // All elements except last need to be in range 1 - 9, last can be 0.
                int last = value.Length - 1;

                for (int i = 0; i < last; i++)
                    if (value[i] < 1 || value[i] > 9)
                        throw new ArgumentOutOfRangeException
                        ("One of the elements in the array specified is not between 1 and 9");

                if (value[last] < 0 || value[last] > 9)
                    throw new ArgumentOutOfRangeException
                    ("Last element in the array specified is not between 0 and 9");

                percentGroupSizes = (int[])value.Clone();
            }
        }

        public int PercentNegativePattern
        {
            get
            {
                // See ECMA NumberFormatInfo page 8
                return percentNegativePattern;
            }

            set
            {
                if (value < 0 || value > 2)
                    throw new ArgumentOutOfRangeException
                    ("The value specified for the property is less than 0 or greater than 15");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                percentNegativePattern = value;
            }
        }

        public int PercentPositivePattern
        {
            get
            {
                // See ECMA NumberFormatInfo page 11 
                return percentPositivePattern;
            }

            set
            {
                if (value < 0 || value > 2)
                    throw new ArgumentOutOfRangeException
                    ("The value specified for the property is less than 0 or greater than 3");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                percentPositivePattern = value;
            }
        }

        public string PercentSymbol
        {
            get
            {
                return percentSymbol;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException
                    ("The value specified for the property is a null reference");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                percentSymbol = value;
            }
        }

        public string PerMilleSymbol
        {
            get
            {
                return perMilleSymbol;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException
                    ("The value specified for the property is a null reference");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                perMilleSymbol = value;
            }
        }

        public string PositiveInfinitySymbol
        {
            get
            {
                return positiveInfinitySymbol;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException
                    ("The value specified for the property is a null reference");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                positiveInfinitySymbol = value;
            }
        }

        public string PositiveSign
        {
            get
            {
                return positiveSign;
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException
                    ("The value specified for the property is a null reference");

                if (isReadOnly)
                    throw new InvalidOperationException
                    ("The current instance is read-only and a set operation was attempted");

                positiveSign = value;
            }
        }

        public object GetFormat(Type formatType)
        {
            return (formatType == typeof(NumberFormatInfo)) ? this : null;
        }

        public object Clone()
        {
            NumberFormatInfo clone = (NumberFormatInfo)MemberwiseClone();
            // clone is not read only
            clone.isReadOnly = false;
            return clone;
        }

        public static NumberFormatInfo ReadOnly(NumberFormatInfo nfi)
        {
            NumberFormatInfo copy = (NumberFormatInfo)nfi.Clone();
            copy.isReadOnly = true;
            return copy;
        }

        public static NumberFormatInfo GetInstance(IFormatProvider provider)
        {
            if (provider != null)
            {
                NumberFormatInfo nfi;
                nfi = (NumberFormatInfo)provider.GetFormat(typeof(NumberFormatInfo));
                if (nfi != null)
                    return nfi;
            }

            return CurrentInfo;
        }
    }
}
