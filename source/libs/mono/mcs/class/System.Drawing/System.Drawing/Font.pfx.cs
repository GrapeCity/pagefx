//
// System.Drawing.Fonts.cs
//
// Authors:
//	Alexandre Pigolkine (pigolkine@gmx.de)
//	Miguel de Icaza (miguel@ximian.com)
//	Todd Berman (tberman@sevenl.com)
//	Jordi Mas i Hernandez (jordi@ximian.com)
//	Ravindra (rkumar@novell.com)
//
// Copyright (C) 2004 Ximian, Inc. (http://www.ximian.com)
// Copyright (C) 2004, 2006 Novell, Inc (http://www.novell.com)
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

using System.ComponentModel;
using flash.text.engine;

namespace System.Drawing
{
    public sealed class Font : MarshalByRefObject, ICloneable, IDisposable
    {
#if NET_2_0
        string systemFontName;
#endif
        const byte DefaultCharSet = 1;
        static int CharSetOffset = -1;

        #region ctors
        public Font(Font prototype, FontStyle newStyle)
        {
            // no null checks, MS throws a NullReferenceException if original is null
            SetProperties(prototype.FontFamily, prototype.Size, newStyle, prototype.Unit, prototype.GdiCharSet, prototype.GdiVerticalFont);
        }

        public Font(FontFamily family, float emSize, GraphicsUnit unit)
            : this(family, emSize, FontStyle.Regular, unit, DefaultCharSet, false)
        {
        }

        public Font(string familyName, float emSize, GraphicsUnit unit)
            : this(new FontFamily(familyName), emSize, FontStyle.Regular, unit, DefaultCharSet, false)
        {
        }

        public Font(FontFamily family, float emSize)
            : this(family, emSize, FontStyle.Regular, GraphicsUnit.Point, DefaultCharSet, false)
        {
        }

        public Font(FontFamily family, float emSize, FontStyle style)
            : this(family, emSize, style, GraphicsUnit.Point, DefaultCharSet, false)
        {
        }

        public Font(FontFamily family, float emSize, FontStyle style, GraphicsUnit unit)
            : this(family, emSize, style, unit, DefaultCharSet, false)
        {
        }

        public Font(FontFamily family, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet)
            : this(family, emSize, style, unit, gdiCharSet, false)
        {
        }

        public Font(FontFamily family, float emSize, FontStyle style,
                GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont)
        {
            if (family == null)
                throw new ArgumentNullException("family");

            SetProperties(family, emSize, style, unit, gdiCharSet, gdiVerticalFont);
        }

        public Font(string familyName, float emSize)
            : this(familyName, emSize, FontStyle.Regular, GraphicsUnit.Point, DefaultCharSet, false)
        {
        }

        public Font(string familyName, float emSize, FontStyle style)
            : this(familyName, emSize, style, GraphicsUnit.Point, DefaultCharSet, false)
        {
        }

        public Font(string familyName, float emSize, FontStyle style, GraphicsUnit unit)
            : this(familyName, emSize, style, unit, DefaultCharSet, false)
        {
        }

        public Font(string familyName, float emSize, FontStyle style, GraphicsUnit unit, byte gdiCharSet)
            : this(familyName, emSize, style, unit, gdiCharSet, false)
        {
        }

        public Font(string familyName, float emSize, FontStyle style,
                GraphicsUnit unit, byte gdiCharSet, bool gdiVerticalFont)
        {
            CreateFont(familyName, emSize, style, unit, gdiCharSet, gdiVerticalFont);
        }

#if NET_2_0
        internal Font(string familyName, float emSize, string systemName)
            : this(familyName, emSize, FontStyle.Regular, GraphicsUnit.Point, DefaultCharSet, false)
        {
            systemFontName = systemName;
        }
#endif
        #endregion

        #region CreateFont
        private void CreateFont(string familyName, float emSize, FontStyle style, GraphicsUnit unit, byte charSet, bool isVertical)
        {
#if ONLY_1_1
			if (familyName == null)
				throw new ArgumentNullException ("familyName");
#endif
            FontFamily family;
            // NOTE: If family name is null, empty or invalid,
            // MS creates Microsoft Sans Serif font.
            try
            {
                family = new FontFamily(familyName);
            }
            catch (Exception)
            {
                family = FontFamily.GenericSansSerif;
            }

            SetProperties(family, emSize, style, unit, charSet, isVertical);

            //Status status = GDIPlus.GdipCreateFont(family.NativeObject, emSize, style, unit, out fontObject);

            //if (status == Status.FontStyleNotFound)
            //    throw new ArgumentException(Locale.GetText("Style {0} isn't supported by font {1}.", style.ToString(), familyName));

            //GDIPlus.CheckStatus(status);
        }
        #endregion

        #region ConvertUnit
        internal static float ConvertUnit(GraphicsUnit fromUnit, GraphicsUnit toUnit, float value)
        {
            if (fromUnit == toUnit)
                return value;

            float inchs;

            switch (fromUnit)
            {
                case GraphicsUnit.Display:
                    inchs = value / 75f;
                    break;
                case GraphicsUnit.Document:
                    inchs = value / 300f;
                    break;
                case GraphicsUnit.Inch:
                    inchs = value;
                    break;
                case GraphicsUnit.Millimeter:
                    inchs = value / 25.4f;
                    break;
                case GraphicsUnit.Pixel:
                case GraphicsUnit.World:
                    //inchs = value / Graphics.systemDpiX;
                    //break;
                case GraphicsUnit.Point:
                    inchs = value / 72f;
                    break;
                default:
                    throw new ArgumentException("Invalid GraphicsUnit");
            }

            switch (toUnit)
            {
                case GraphicsUnit.Display:
                    return inchs * 75;

                case GraphicsUnit.Document:
                    return inchs * 300;

                case GraphicsUnit.Inch:
                    return inchs;

                case GraphicsUnit.Millimeter:
                    return inchs * 25.4f;

                case GraphicsUnit.Pixel:
                case GraphicsUnit.World:
                    //return inchs * Graphics.systemDpiX;

                case GraphicsUnit.Point:
                    return inchs * 72;

                default:
                    throw new ArgumentException("Invalid GraphicsUnit");
            }
        }
        #endregion

        #region SetProperties
        internal void SetProperties(FontFamily family, float emSize, FontStyle style, GraphicsUnit unit, byte charSet, bool isVertical)
        {
            _name = family.Name;
            _fontFamily = family;
            _size = emSize;

            // MS throws ArgumentException, if unit is set to GraphicsUnit.Display
            _unit = unit;
            _style = style;
            _gdiCharSet = charSet;
            _gdiVerticalFont = isVertical;

            _sizeInPoints = ConvertUnit(unit, GraphicsUnit.Point, emSize);
            _sizeInPixels = ConvertUnit(unit, GraphicsUnit.Pixel, emSize);
        }
        #endregion

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        public object Clone()
        {
            return new Font(this, Style);
        }

        #region Properties
        [Browsable(false)]
        public FontFamily FontFamily
        {
            get { return _fontFamily; }
        }
        FontFamily _fontFamily;

        public byte GdiCharSet
        {
            get { return _gdiCharSet; }
        }
        byte _gdiCharSet;

        public bool GdiVerticalFont
        {
            get { return _gdiVerticalFont; }
        }
        bool _gdiVerticalFont;

        [Browsable(false)]
        public int Height
        {
            get
            {
                return (int)Math.Ceiling(GetHeight());
            }
        }

#if NET_2_0
        [Browsable(false)]
        public bool IsSystemFont
        {
            get
            {
                if (systemFontName == null)
                    return false;

                return StringComparer.InvariantCulture.Compare(systemFontName, string.Empty) != 0;
            }
        }
#endif

        public string Name
        {
            get { return _name; }
        }
        string _name;

        public float Size
        {
            get { return _size; }
        }
        float _size;

        [Browsable(false)]
        public float SizeInPoints
        {
            get { return _sizeInPoints; }
        }
        float _sizeInPoints;

        internal float SizeInPixels
        {
            get { return _sizeInPixels; }
        }
        float _sizeInPixels;

        [Browsable(false)]
        public FontStyle Style
        {
            get { return _style; }
        }
        FontStyle _style;

        public bool Bold
        {
            get { return (_style & FontStyle.Bold) != 0; }
        }

        public bool Italic
        {
            get { return (_style & FontStyle.Italic) != 0; }
        }

        public bool Strikeout
        {
            get { return (_style & FontStyle.Strikeout) != 0; }
        }

        public bool Underline
        {
            get { return (_style & FontStyle.Underline) != 0; }
        }

#if NET_2_0
        [Browsable(false)]
        public string SystemFontName
        {
            get
            {
                return systemFontName;
            }
        }
#endif
        
        public GraphicsUnit Unit
        {
            get { return _unit; }
        }
        GraphicsUnit _unit;
        #endregion

        public FontDescription FontDescription
        {
            get
            {
                if (_fd == null)
                {
                    _fd = new FontDescription(_name,
                                              Bold ? FontWeight.BOLD : FontWeight.NORMAL,
                                              Italic ? FontPosture.ITALIC : FontPosture.NORMAL)
                              {
                                  renderingMode = RenderingMode.CFF
                              };
                }
                return _fd;
            }
        }

        private FontDescription _fd;

        #region Object Override Methods
        public override bool Equals(object obj)
        {
            Font fnt = (obj as Font);
            if (fnt == null)
                return false;

            if (fnt.FontFamily.Equals(FontFamily) && fnt.Size == Size &&
                fnt.Style == Style && fnt.Unit == Unit &&
                fnt.GdiCharSet == GdiCharSet &&
                fnt.GdiVerticalFont == GdiVerticalFont)
                return true;
            else
                return false;
        }

        public override int GetHashCode()
        {
            return _name.GetHashCode() ^ FontFamily.GetHashCode() ^ _size.GetHashCode() ^ _style.GetHashCode() ^
                _gdiCharSet ^ _gdiVerticalFont.GetHashCode();
        }

        public override String ToString()
        {
            return String.Format("[Font: Name={0}, Size={1}, Units={2}, GdiCharSet={3}, GdiVerticalFont={4}]",
                _name, Size, (int)_unit, _gdiCharSet, _gdiVerticalFont);
        }
        #endregion

        #region TODO: GetHeight
        public float GetHeight()
        {
            return GetHeight(Graphics.systemDpiY);
        }

        public float GetHeight(Graphics graphics)
        {
            if (graphics == null)
                throw new ArgumentNullException("graphics");

            throw new NotImplementedException();
        }

        public float GetHeight(float dpi)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
