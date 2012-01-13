using System.Drawing.Text;

namespace System.Drawing
{

    public sealed class FontFamily : MarshalByRefObject, IDisposable
    {

        static readonly FontFamily _genericMonospace;
        static readonly FontFamily _genericSansSerif;
        static readonly FontFamily _genericSerif;
        static readonly FontCollection _installedFonts;

        static FontFamily()
        {
            _installedFonts = new InstalledFontCollection();
            _genericMonospace = new FontFamily(GenericFontFamilies.Monospace);
            _genericSansSerif = new FontFamily(GenericFontFamilies.SansSerif);
            _genericSerif = new FontFamily(GenericFontFamilies.Serif);
        }

        private readonly string _name;

        private FontStyle _lastStyle = FontStyle.Regular;
        
        // this is unavailable through Java API, usually 2048 for TT fonts
        const int UnitsPerEm = 2048;
        // the margin for text drawing
        const int DrawMargin = 571;

        #region ctors

        // dummy ctors to work around convertor problems
        internal FontFamily() { }
        internal FontFamily(IntPtr family) { }

        static string ToGenericFontName(GenericFontFamilies genericFamily)
        {
            switch (genericFamily)
            {
                case GenericFontFamilies.SansSerif:
                    return "SansSerif";
                case GenericFontFamilies.Serif:
                    return "Serif";
                default:
                    return "Monospaced";
            }
        }

        public FontFamily(string familyName)
            : this(familyName, null)
        {
        }

        public FontFamily(string name, FontCollection fontCollection)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            if (fontCollection == null)
                fontCollection = _installedFonts;

            _name = name;

            //if (fontCollection.Contains(name))
            //    _name = name;
            //else
            //{
            //    _name = ToGenericFontName(GenericFontFamilies.SansSerif);
            //    fontCollection = _installedFonts;
            //}
        }

        public FontFamily(GenericFontFamilies genericFamily)
            : this(ToGenericFontName(genericFamily))
        {
        }

        #endregion

        public string Name
        {
            get
            {
                return _name;
            }
        }

        internal int GetDrawMargin(FontStyle style)
        {
            return DrawMargin;
        }

        //awt.FontMetrics GetMetrics(FontStyle style)
        //{
        //    if ((_lastStyle != style) || (_fontMetrics == null))
        //    {
        //        java.util.Map attrib = Font.DeriveStyle(FamilyFont.getAttributes(), style, true);
        //        attrib.put(TextAttribute.SIZE, new java.lang.Float((float)(UnitsPerEm << 1)));
        //        _fontMetrics = Container.getFontMetrics(FamilyFont.deriveFont(attrib));
        //    }
        //    return _fontMetrics;
        //}

        public int GetCellAscent(FontStyle style)
        {
            //return GetMetrics(style).getMaxAscent() >> 1;
            throw new NotImplementedException();
        }

        public int GetCellDescent(FontStyle style)
        {
            //return GetMetrics(style).getMaxDecent() >> 1;
            throw new NotImplementedException();
        }

        public int GetEmHeight(FontStyle style)
        {
            return UnitsPerEm;
        }

        public int GetLineSpacing(FontStyle style)
        {
            //return GetMetrics(style).getHeight() >> 1;
            throw new NotImplementedException();
        }

        public string GetName(int language)
        {
            try
            {
                //CultureInfo culture = new CultureInfo(language, false);
                //java.util.Locale locale = vmw.@internal.EnvironmentUtils.getLocaleFromCultureInfo(culture);
                //return FamilyFont.getFamily(locale);
                throw new NotImplementedException();
            }
            catch
            {
                return Name;
            }
        }

        public bool IsStyleAvailable(FontStyle style)
        {
            //unable to get this infromation from java
            return true;
        }

        #region static members

        public static FontFamily[] Families
        {
            get
            {
                return _installedFonts.Families;
            }
        }

        public static FontFamily GenericMonospace
        {
            get
            {
                return (FontFamily)_genericMonospace.MemberwiseClone();
            }
        }

        public static FontFamily GenericSansSerif
        {
            get
            {
                return (FontFamily)_genericSansSerif.MemberwiseClone();
            }
        }

        public static FontFamily GenericSerif
        {
            get
            {
                return (FontFamily)_genericSerif.MemberwiseClone();
            }
        }

        public static FontFamily[] GetFamilies(Graphics graphics)
        {
            if (graphics == null)
            {
                throw new ArgumentNullException("graphics");
            }
            return _installedFonts.Families;
        }

        #endregion

        #region Object members

        public override bool Equals(object obj)
        {
            if (this == obj)
                return true;

            if (!(obj is FontFamily))
                return false;

            return string.Compare(Name, ((FontFamily)obj).Name, true) == 0;
        }

        public override int GetHashCode()
        {
            return Name.ToLower().GetHashCode();
        }

        public override string ToString()
        {
            return string.Format("[{0}: Name={1}]", GetType().Name, Name);
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
        }

        #endregion
    }
}

