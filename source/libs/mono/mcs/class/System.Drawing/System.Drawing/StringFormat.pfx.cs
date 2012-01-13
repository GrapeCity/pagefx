using System;
using System.Drawing.Text;

namespace System.Drawing
{
    /// <summary>
    /// Summary description for StringFormat.
    /// </summary>
    public sealed class StringFormat : MarshalByRefObject, IDisposable, ICloneable
    {        
        #region Constructors
        public StringFormat()
            : this(0, 0)
        {
        }

        public StringFormat(StringFormatFlags options)
            : this(options, 0)
        {
        }

        public StringFormat(StringFormatFlags options, int lang)
        {
            _alignment = StringAlignment.Near;
            _digitSubstituteLanguage = lang;
            _digitSubstituteMethod = StringDigitSubstitute.User;
            _flags = options;
            _hotkeyPrefix = HotkeyPrefix.None;
            _lineAlignment = StringAlignment.Near;
            _trimming = StringTrimming.Character;
        }

        public StringFormat(StringFormat source)
        {
            if (source == null)
                throw new ArgumentNullException("format");

            _alignment = source.LineAlignment;
            _digitSubstituteLanguage = source.DigitSubstitutionLanguage;
            _digitSubstituteMethod = source.DigitSubstitutionMethod;
            _flags = source.FormatFlags;
            _hotkeyPrefix = source.HotkeyPrefix;
            _lineAlignment = source.LineAlignment;
            _trimming = source.Trimming;
        }
        #endregion

        #region IDisposable
        public void Dispose()
        {
        }
        #endregion

        #region Public properties
        public StringAlignment Alignment
        {
            get { return _alignment; }
            set { _alignment = value; }
        }
        StringAlignment _alignment;

        public StringAlignment LineAlignment
        {
            get { return _lineAlignment; }
            set { _lineAlignment = value; }
        }
        StringAlignment _lineAlignment;

        [MonoTODO]
        public StringFormatFlags FormatFlags
        {
            get { return _flags; }
            set { _flags = value; }
        }
        StringFormatFlags _flags;

        [MonoTODO]
        public HotkeyPrefix HotkeyPrefix
        {
            get { return _hotkeyPrefix; }
            set { _hotkeyPrefix = value; }
        }
        HotkeyPrefix _hotkeyPrefix;

        [MonoTODO]
        public StringTrimming Trimming
        {
            get { return _trimming; }
            set { _trimming = value; }
        }
        StringTrimming _trimming;

        public int DigitSubstitutionLanguage
        {
            get { return _digitSubstituteLanguage; }
        }
        int _digitSubstituteLanguage;

        public StringDigitSubstitute DigitSubstitutionMethod
        {
            get { return _digitSubstituteMethod; }
        }
        StringDigitSubstitute _digitSubstituteMethod;
        #endregion

        #region static properties
        public static StringFormat GenericDefault
        {
            get
            {
                StringFormat genericDefault = new StringFormat();
                return genericDefault;
            }
        }

        public static StringFormat GenericTypographic
        {
            get
            {
                StringFormat genericTypographic = new StringFormat(
                    StringFormatFlags.FitBlackBox |
                    StringFormatFlags.LineLimit |
                    StringFormatFlags.NoClip,
                    0);
                genericTypographic.Trimming = StringTrimming.None;
                genericTypographic._genericTypeographic = true;
                return genericTypographic;
            }
        }
        #endregion

        #region internal accessors
        internal bool NoWrap
        {
            get { return (FormatFlags & StringFormatFlags.NoWrap) != 0; }
        }

        internal bool IsVertical
        {
            get { return (FormatFlags & StringFormatFlags.DirectionVertical) != 0; }
        }

        internal bool MeasureTrailingSpaces
        {
            get { return (FormatFlags & StringFormatFlags.MeasureTrailingSpaces) != 0; }
        }

        internal bool LineLimit
        {
            get { return (FormatFlags & StringFormatFlags.LineLimit) != 0; }
        }

        internal bool NoClip
        {
            get { return (FormatFlags & StringFormatFlags.NoClip) != 0; }
        }

        internal bool IsRightToLeft
        {
            get { return (FormatFlags & StringFormatFlags.DirectionRightToLeft) != 0; }
        }

        internal CharacterRange[] CharRanges
        {
            get { return _charRanges; }
        }
        CharacterRange[] _charRanges;

        internal bool IsGenericTypographic
        {
            get { return _genericTypeographic; }
        }
        bool _genericTypeographic;
        #endregion

        #region public methods
        public void SetMeasurableCharacterRanges(CharacterRange[] range)
        {
            _charRanges = range != null ? (CharacterRange[])range.Clone() : null;
        }

        public object Clone()
        {
            StringFormat copy = (StringFormat)MemberwiseClone();
            if (_charRanges != null)
                copy._charRanges = (CharacterRange[])_charRanges.Clone();
            if (_tabStops != null)
                copy._tabStops = (float[])_tabStops.Clone();
            return copy;
        }

        public override string ToString()
        {
            return "[StringFormat, FormatFlags=" + this.FormatFlags.ToString() + "]";
        }

        float _firstTabOffset;
        float[] _tabStops;

        public void SetTabStops(float firstTabOffset, float[] tabStops)
        {
            _firstTabOffset = firstTabOffset;
            _tabStops = tabStops != null ? (float[])tabStops.Clone() : null;
        }

        public void SetDigitSubstitution(int language, StringDigitSubstitute substitute)
        {
            _digitSubstituteMethod = substitute;
            _digitSubstituteLanguage = language;
        }

        [MonoTODO]
        public float[] GetTabStops(out float firstTabOffset)
        {
            firstTabOffset = _firstTabOffset;
            return _tabStops != null ? (float[])_tabStops.Clone() : null;
        }
        #endregion
    }
}
