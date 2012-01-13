using System.Drawing.Drawing2D;
using System.Drawing.Flash;
using flash.display;
using flash.text;
using flash.text.engine;

namespace System.Drawing
{
    class TextRenderer
    {
        StringFormat _format;
        Font _font;
        Color _color;
        double _x, _y, _availWidth, _availHeight;
        double _ph; //paragraph height
        DisplayObjectContainer _container;
        bool _nowrap;
        //bool _noclip;
        bool _vertical;
        bool _rtl;

        #region DrawString
        public void DrawString(Graphics g, string s, Font font, Brush brush, RectangleF rect, StringFormat format)
        {
            if (format == null)
                format = StringFormat.GenericDefault;

            //case 128101
            flash.text.TextRenderer.antiAliasType = AntiAliasType.ADVANCED;
            flash.text.TextRenderer.maxLevel = 7;

            _font = font;
            _format = format;

            _nowrap = format.NoWrap;
            //_noclip = format.NoClip;
            _vertical = format.IsVertical;
            _rtl = format.IsRightToLeft;

            var fd = font.FontDescription;

            double fontSize = font.SizeInPixels;

            //double scale = 1;
            //while (fontSize > 127)
            //{
            //    fontSize /= 2;
            //    scale *= 2;
            //}

            var ef = new ElementFormat(fd) {fontSize = fontSize};

            //var solidBrush = brush as SolidBrush;
            //_color = solidBrush != null ? solidBrush.Color : Color.FromArgb(0xff, 0xff, 0xff, 0xff);

            _color = GetColor(brush);
            ef.color = ColorHelper.ToFlash(_color);
            ef.alpha = _color.A / 255.0;

            var textElem = new TextElement(s, ef);
            var textBlock = new TextBlock(textElem);

            bool addContainer = true;
            if (g.NoTextContainer)
            {
                _container = g._container;
                addContainer = false;
            }
            else
            {
                _container = new Sprite {x = rect.X, y = rect.Y};
            }

            _x = 0;
            _y = 0;

            if (_vertical)
            {
                textBlock.lineRotation = TextRotation.ROTATE_90;
                _availWidth = rect.Height;
                _availHeight = rect.Width;
                if (_rtl) _x = _availHeight;
            }
            else
            {
                _availWidth = rect.Width;
                _availHeight = rect.Height;
            }

            _ph = 0;
            Layout(textBlock);

            VAlign(_availHeight - _ph);

            //if (scale != 1)
            //{
            //    _container.scaleX = scale;
            //    _container.scaleY = scale;
            //}

            if (addContainer)
                g.AddChild(_container, true);

            //if (solidBrush != null)
            //{
            //    g.AddChild(_container, true);
            //}
            //else
            //{
            //    var sprite = g.BeginSprite(brush, null);
            //    sprite.graphics.drawRect(rect.X, rect.Y, rect.Width, rect.Height);
            //    g.EndObject(sprite, true);
            //    _container.x = 0;
            //    _container.y = 0;
            //    sprite.addChild(_container);
            //    sprite.mask = _container;
            //}
        }

        static Color GetColor(Brush brush)
        {
            var sb = brush as SolidBrush;
            if (sb != null) return sb.Color;
            var lg = brush as LinearGradientBrush;
            if (lg != null) return lg.LinearColors[0];
            var hb = brush as HatchBrush;
            if (hb != null) return hb.ForegroundColor;
            return Color.Black;
        }

        void VAlign(double dh)
        {
            switch (_format.LineAlignment)
            {
                //case StringAlignment.Near:
                //    if (_rtl)
                //        AlignContainer(dh);
                //    break;

                case StringAlignment.Center:
                    dh = dh / 2;
                    if (_vertical && _rtl)
                    {
                        MoveChildren(_container, -dh, 0);
                        return;
                    }
                    AlignContainer(dh);
                    break;

                case StringAlignment.Far:
                    if (_vertical && _rtl)
                    {
                        MoveChildren(_container, -dh, 0);
                        return;
                    }
                    if (!_rtl)
                        AlignContainer(dh);
                    break;
            }
        }

        void AlignContainer(double dh)
        {
            if (_vertical)
                _container.x += dh;
            else
                _container.y += dh;
        }

        static void MoveChildren(DisplayObjectContainer container, double dx, double dy)
        {
            if (dx == 0 && dy == 0) return;
            bool h = dx != 0;
            bool v = dy != 0;
            int n = container.numChildren;
            for (int i = 0; i < n; ++i)
            {
                var child = container.getChildAt(i);
                if (h) child.x += dx;
                if (v) child.y += dy;
            }
        }
        #endregion

        #region Layout
        void Layout(TextBlock textBlock)
        {
            if (_availWidth <= 1)
            {
                AddTextLine(textBlock.createTextLine());
                return;
            }

            //if (_nowrap && _noclip)
            //    _availWidth = 1000000;

            var line = textBlock.createTextLine(null, _availWidth);

            if (_nowrap)
            {
                AddTextLine(line);
            }
            else
            {
                while (line != null)
                {
                    if (AddTextLine(line)) break;
                    line = textBlock.createTextLine(line, _availWidth);
                }
            }
        }

        bool IsBreak(TextLine line)
        {
            //if (_noclip) return false;
            double lh = line.height;
            if (_vertical)
            {
                if (_rtl) return _x - lh <= 0;
                return _x + lh >= _availHeight;
            }
            return _y + lh >= _availHeight;
        }
        #endregion

        #region AddTextLine
        //NOTES:
        //line.textHeight = line.ascent + line.descent

        //bool _firstLine = true;
        //int _ln;

        static double GetLeading(TextLine line)
        {
            //see http://bugs.adobe.com/jira/browse/FP-476
            return line.textHeight * 0.2;
        }

        bool AddTextLine(TextLine line)
        {
            //++_ln;
            //Console.WriteLine("{0}. x: {1}, y: {2}", _ln, line.x, line.y);

            double lh = line.height;
            if (!IsFirst(line))
                lh += GetLeading(line);
            _ph += lh;

            #region research for case 128779
            //if (_firstLine)
            //{
            //    _firstLine = false;
            //    ElementFormat ef = line.textBlock.content.elementFormat;
            //    var em = ef.getFontMetrics().emBox;
            //    var fd = ef.fontDescription;

            //    var tf = new TextField
            //                 {
            //                     defaultTextFormat = new TextFormat
            //                                             {
            //                                                 font = fd.fontName,
            //                                                 italic = fd.fontPosture == FontPosture.ITALIC,
            //                                                 bold = fd.fontWeight == FontWeight.BOLD,
            //                                                 size = ef.fontSize,
            //                                             },
            //                     text = "Hello, World!"
            //                 };

            //    double leading = line.textHeight * 0.2;
            //    Console.WriteLine("---");
            //    Console.WriteLine(
            //        "FTE> FontSize: {0}, LH: {1}, TH: {2}, A: {3}, D: {4}, Leading: {5}, A+D: {6}, em: {7}, {8}, {9}, {10}",
            //        ef.fontSize, line.height, line.textHeight,
            //        line.ascent, line.descent, leading, line.ascent + line.descent,
            //        em.x, em.y, em.width, em.height);

            //    var tlm = tf.getLineMetrics(0);
            //    Console.WriteLine(
            //        "TF> A: {0}, D: {1}, Leading: {2}, Height: {3}",
            //        tlm.ascent, tlm.descent, tlm.leading, tlm.height);
            //}
            #endregion

            #region Positioning
            if (_vertical)
            {
                line.y = _y;
                if (_rtl)
                {
                    lh = -lh;
                    line.x = _x + lh;
                }
                else
                {
                    if (IsFirst(line))
                        _x += line.descent;
                    line.x = _x;
                }
                _x += lh;
            }
            else
            {
                _y += lh;
                line.x = _x;
                line.y = _y;
            }
            #endregion

            bool dobreak = IsBreak(line);

            var align = _format.Alignment;

            //Trimming
            if (_nowrap || dobreak || _availWidth <= 1)
            {
                if (!IsLast(line))
                {
                    var trimmedLine = Trim(line);
                    if (trimmedLine != line)
                    {
                        //TODO: Align of trimmed line. Investigate GDI+ behaviour and do the same
                        if (_format.Trimming == StringTrimming.EllipsisCharacter)
                        {
                            switch (align)
                            {
                                case StringAlignment.Center:
                                    align = StringAlignment.Far;
                                    break;
                            }
                        }
                    }
                    line = trimmedLine;
                }
            }

            Align(line, align);
            Decorate(line);

            _container.addChild(line);

            return dobreak;
        }

        void Decorate(TextLine line)
        {
            double tw = line.textWidth;

            if (_font.Strikeout)
            {
                var l = CreateHLine(_color, -line.ascent / 2.0, tw);
                line.addChild(l);
            }

            if (_font.Underline)
            {
                var l = CreateHLine(_color, line.descent / 2.0, tw);
                line.addChild(l);
            }
        }

        void Align(TextLine line, StringAlignment align)
        {
            double offset = EvalAlignOffset(line.textWidth, align);
            if (_vertical) line.y += offset;
            else line.x += offset;
        }

        double EvalAlignOffset(double lineWidth, StringAlignment align)
        {
            double offset = 0;
            switch (align)
            {
                case StringAlignment.Near:
                    if (!_vertical && _rtl)
                        offset = _availWidth - lineWidth;
                    break;

                case StringAlignment.Center:
                    offset = (_availWidth - lineWidth) / 2;
                    break;

                case StringAlignment.Far:
                    if (_vertical || !_rtl)
                        offset = _availWidth - lineWidth;
                    break;
            }
            return offset;
        }
        #endregion

        #region Trimming
        //TODO: Respect bidi context

        static bool IsFirst(TextLine line)
        {
            return line.textBlockBeginIndex == 0;
        }

        static bool IsLast(TextLine line)
        {
            int i = line.textBlockBeginIndex + line.rawTextLength;
            var e = line.textBlock.content as TextElement;
            return i >= e.rawText.length;
        }

        TextLine Trim(TextLine line)
        {
            StringTrimming kind = _format.Trimming;

            if (kind == StringTrimming.None
                || kind == StringTrimming.Character
                || kind == StringTrimming.Word)
                return line;

            if (kind == StringTrimming.EllipsisPath)
                return TrimPath(line);

            return TrimEllipsis(line, kind);
        }

        TextLine TrimPath(TextLine line)
        {
            var ellipsisLine = CreateEllipsis(line);
            double ew = ellipsisLine.textWidth;
            double lineWidth = line.textWidth;
            if (ew >= lineWidth) return ellipsisLine;

            var lastLine = CreateLastLine(line);

            string text = "";
            int i;
            double half = (_availWidth - ew) / 2;
            if (lineWidth > half)
            {
                i = FindBreak(line, half, false, false);
                if (i >= 0) text = Substr(line, i + 1);
            }
            else
            {
                double avail = half - lineWidth;
                text = GrabFromNext(line, lastLine, avail, false);
            }

            i = FindBreak(lastLine, half, false, true);
            text += Ellipsis;

            if (i >= 0)
            {
                SkipWS(lastLine, ref i, false);
                text += Substr2(lastLine, i);
            }

            return CreateLine(line, text);
        }

        TextLine TrimEllipsis(TextLine line, StringTrimming kind)
        {
            var ellipsisLine = CreateEllipsis(line);
            double ew = ellipsisLine.textWidth;
            double lineWidth = line.textWidth;
            if (ew >= lineWidth) return ellipsisLine;

            bool byword = kind == StringTrimming.EllipsisWord;

            double avail = _availWidth - lineWidth - ew - GetLastWSWidth(line);
            if (avail > 0)
            {
                var lastLine = CreateLastLine(line);
                string text = GrabFromNext(line, lastLine, avail, byword);
                return CreateLineWithEllipsis(line, text);
            }

            int i = FindBreak(line, ew, byword, true);
            if (i < 0) return ellipsisLine;

            string s = Substr(line, i + 1);
            return CreateLineWithEllipsis(line, s);
        }

        string GrabFromNext(TextLine line, TextLine lastLine, double avail, bool byword)
        {
            int k = FindBreak(lastLine, avail, byword, false);
            string lineText = GetText(line);
            if (k >= 0)
            {
                string ls = Substr(lastLine, k + 1);
                return lineText + ls;
            }
            //we must ignore last white spaces
            return lineText.TrimEnd();
        }

        double GetCharWidth(TextLine line, int i)
        {
            var b = line.getAtomBounds(i);
            return _vertical ? b.height : b.width;
        }

        int FindBreak(TextLine line, double width, bool byword, bool reverse)
        {
            int n = line.atomCount;
            int i, inc;
            if (reverse)
            {
                i = n - 1;
                inc = -1;
            }
            else
            {
                i = 0;
                inc = 1;
            }
            for ( ; reverse ? i >= 0 : i < n; i += inc)
            {
                width -= GetCharWidth(line, i);
                if (width > 0)
                {
                    if (!reverse)
                    {
                        if (i + 1 >= n) continue;
                        double aw = GetCharWidth(line, i + 1);
                        if (aw <= width) continue;
                    }
                    else continue;
                }
                if (byword)
                {
                    //find first white space
                    if (NextWS(line, ref i, true))
                    {
                        //skip white spaces
                        i += inc;
                        SkipWS(line, ref i, true);
                    }
                }
                break;
            }
            line.flushAtomData();
            return i;
        }

        double GetLastWSWidth(TextLine line)
        {
            double w = 0;
            int n = line.atomCount;
            for (int i = n - 1; i >= 0; --i)
            {
                if (!line.getAtomWordBoundaryOnLeft(i)) break;
                w += GetCharWidth(line, i);
            }
            line.flushAtomData();
            return w;
        }

        //TODO: char.IsWhiteSpace or getAtomWordBoundaryOnLeft?

        //Skips white spaces
        static char GetChar(TextLine line, int i)
        {
            return line.textBlock.content.rawText[line.textBlockBeginIndex + i];
        }

        static bool NextWS(TextLine line, ref int i, bool reverse)
        {
            int n = line.atomCount;
            int inc = reverse ? -1 : 1;
            for (; reverse ? i >= 0 : i < n; i += inc)
            {
                //if (line.getAtomWordBoundaryOnLeft(i))
                //    return true;
                char c = GetChar(line, i);
                if (char.IsWhiteSpace(c))
                    return true;
            }
            return false;
        }

        static void SkipWS(TextLine line, ref int i, bool reverse)
        {
            int n = line.atomCount;
            int inc = reverse ? -1 : 1;
            for (; reverse ? i >= 0 : i < n; i += inc)
            {
                char c = GetChar(line, i);
                if (!char.IsWhiteSpace(c)) break;

                //if (!line.getAtomWordBoundaryOnLeft(i))
                //{
                //    if (!reverse) --i;
                //    break;
                //}
            }
        }

        static int GetEndIndex(TextLine line)
        {
            int start = line.textBlockBeginIndex;
            int len = line.rawTextLength;
            if (len == 0) return start;
            return start + len - 1;
        }

        static string GetText(TextLine line)
        {
            int start = line.textBlockBeginIndex;
            return line.textBlock.content.rawText.substr(start, line.rawTextLength);
        }

        static string Substr(TextLine line, int len)
        {
            int start = line.textBlockBeginIndex;
            return line.textBlock.content.rawText.substr(start, len);
        }

        static string Substr2(TextLine line, int index)
        {
            int start = line.textBlockBeginIndex;
            return line.textBlock.content.rawText.substring(start + index);
        }

        const string Ellipsis = "...";

        TextBlock NewBlock(TextLine line, string text)
        {
            var e = new TextElement(text, line.textBlock.content.elementFormat);
            var block = new TextBlock(e);
            if (_vertical)
                block.lineRotation = TextRotation.ROTATE_90;
            return block;
        }

        TextLine CreateLine(TextLine line, string s)
        {
            var block = NewBlock(line, s);
            var l = block.createTextLine();
            l.x = line.x;
            l.y = line.y;
            return l;
        }

        TextLine CreateEllipsis(TextLine line)
        {
            return CreateLine(line, Ellipsis);
        }

        TextLine CreateLineWithEllipsis(TextLine line, string s)
        {
            return CreateLine(line, s + Ellipsis);
        }

        TextLine CreateLastLine(TextLine line)
        {
            int start = GetEndIndex(line) + 1;
            string text = line.textBlock.content.rawText.substr(start);
            return CreateLine(line, text);
        }
        #endregion

        #region CreateHLine
        static Shape CreateHLine(Color color, double y, double width)
        {
            var shape = new Shape();
            var g = shape.graphics;
            g.lineStyle(1, ColorHelper.ToFlash(color), color.A / 255.0);
            g.moveTo(0, y);
            g.lineTo(width, y);
            g.endFill();
            return shape;
        }
        #endregion
    }
}