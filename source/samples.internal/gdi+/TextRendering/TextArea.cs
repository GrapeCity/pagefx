using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TextRendering
{
    public partial class TextArea : UserControl
    {
        public TextArea()
        {
            InitializeComponent();
        }

        public Brush Brush
        {
            get { return _brush; }
            set
            {
                if (value != _brush)
                {
                    _brush = value;
                    Invalidate();
                }
            }
        }
        Brush _brush;

        public StringFormat Format
        {
            get
            {
                return _format;
            }
            set
            {
                if (value == null)
                    value = StringFormat.GenericDefault;

                if (value != _format)
                {
                    _format = value;
                    Invalidate();
                }
            }
        }
        StringFormat _format = StringFormat.GenericDefault;

        StringFormatFlags FormatFlags
        {
            get { return Format.FormatFlags; }
            set { Format.FormatFlags = value; }
        }

        void SetFlag(StringFormatFlags f, bool value)
        {
            if (value) FormatFlags |= f;
            else FormatFlags &= ~f;
        }

        public bool DirectionVertical
        {
            get { return (FormatFlags & StringFormatFlags.DirectionVertical) != 0; }
            set
            {
                if (value != DirectionVertical)
                {
                    SetFlag(StringFormatFlags.DirectionVertical, value);
                    Invalidate();
                }
            }
        }

        public bool DirectionRightToLeft
        {
            get { return (FormatFlags & StringFormatFlags.DirectionRightToLeft) != 0; }
            set
            {
                if (value != DirectionRightToLeft)
                {
                    SetFlag(StringFormatFlags.DirectionRightToLeft, value);
                    Invalidate();
                }
            }
        }

        public bool NoWrap
        {
            get { return (FormatFlags & StringFormatFlags.NoWrap) != 0; }
            set
            {
                if (value != NoWrap)
                {
                    SetFlag(StringFormatFlags.NoWrap, value);
                    Invalidate();
                }
            }
        }

        public bool NoClip
        {
            get { return (FormatFlags & StringFormatFlags.NoClip) != 0; }
            set
            {
                if (value != NoClip)
                {
                    SetFlag(StringFormatFlags.NoClip, value);
                    Invalidate();
                }
            }
        }

        public StringAlignment Align
        {
            get { return Format.Alignment; }
            set
            {
                if (value != Align)
                {
                    Format.Alignment = value;
                    Invalidate();
                }
            }
        }

        public StringAlignment LineAlign
        {
            get { return Format.LineAlignment; }
            set
            {
                if (value != LineAlign)
                {
                    Format.LineAlignment = value;
                    Invalidate();
                }
            }
        }

        public StringTrimming Trimming
        {
            get { return Format.Trimming; }
            set
            {
                if (value != Trimming)
                {
                    Format.Trimming = value;
                    Invalidate();
                }
            }
        }

        public HotkeyPrefix HotkeyPrefix
        {
            get { return Format.HotkeyPrefix; }
            set
            {
                if (value != HotkeyPrefix)
                {
                    Format.HotkeyPrefix = value;
                    Invalidate();
                }
            }
        }

        const string LoremIpsum =
            "Lorem ipsum dolor sit amet, consectetuer adipiscing elit,"
            + " sed diam nonummy nibh euismod tincidunt ut laoreet dolore magna aliquam erat volutpat."
            + " Ut wisi enim ad minim veniam, quis nostrud exerci tation ullamcorper suscipit lobortis"
            + " nisl ut aliquip ex ea commodo consequat. Duis autem vel eum iriure dolor in hendrerit"
            + " in vulputate velit esse molestie consequat, vel illum dolore eu feugiat nulla facilisis"
            + " at vero eros et accumsan et iusto odio dignissim qui blandit praesent luptatum zzril"
            + " delenit augue duis dolore te feugait nulla facilisi.";

        private void TextArea_Paint(object sender, PaintEventArgs e)
        {
            var g = e.Graphics;
            var font = Font;

            var brush = _brush ?? Brushes.Black;

            var format = Format ?? StringFormat.GenericDefault;

            g.DrawString(LoremIpsum, font, brush, ClientRectangle, format);
        }
    }
}
