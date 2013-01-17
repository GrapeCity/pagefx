using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    /// <summary>
    /// The TextFormat class represents character formatting information.  Use the TextFormat class
    /// to create specific text formatting for text fields. You can apply text formatting
    /// to both static and dynamic text fields. The properties of the TextFormat class apply to device and
    /// embedded fonts. However, for embedded fonts, bold and italic text actually require specific fonts. If you
    /// want to display bold or italic text with an embedded font, you need to embed the bold and italic variations
    /// of that font.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class TextFormat : Avm.Object
    {
        public extern virtual object size
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

        public extern virtual object bullet
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

        public extern virtual Avm.String align
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

        public extern virtual object color
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

        public extern virtual Avm.String display
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

        public extern virtual object bold
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

        public extern virtual object leading
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

        public extern virtual Avm.String font
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

        public extern virtual object rightMargin
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

        public extern virtual object leftMargin
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

        public extern virtual object indent
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

        public extern virtual object blockIndent
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

        public extern virtual object kerning
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

        public extern virtual Avm.Array tabStops
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

        public extern virtual object italic
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

        public extern virtual Avm.String target
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

        public extern virtual object underline
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

        public extern virtual Avm.String url
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

        public extern virtual object letterSpacing
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

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String arg0, object arg1, object arg2, object arg3, object arg4, object arg5, Avm.String arg6, Avm.String arg7, Avm.String arg8, object arg9, object arg10, object arg11, object arg12);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String arg0, object arg1, object arg2, object arg3, object arg4, object arg5, Avm.String arg6, Avm.String arg7, Avm.String arg8, object arg9, object arg10, object arg11);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String arg0, object arg1, object arg2, object arg3, object arg4, object arg5, Avm.String arg6, Avm.String arg7, Avm.String arg8, object arg9, object arg10);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String arg0, object arg1, object arg2, object arg3, object arg4, object arg5, Avm.String arg6, Avm.String arg7, Avm.String arg8, object arg9);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String arg0, object arg1, object arg2, object arg3, object arg4, object arg5, Avm.String arg6, Avm.String arg7, Avm.String arg8);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String arg0, object arg1, object arg2, object arg3, object arg4, object arg5, Avm.String arg6, Avm.String arg7);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String arg0, object arg1, object arg2, object arg3, object arg4, object arg5, Avm.String arg6);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String arg0, object arg1, object arg2, object arg3, object arg4, object arg5);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String arg0, object arg1, object arg2, object arg3, object arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String arg0, object arg1, object arg2, object arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String arg0, object arg1, object arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String arg0, object arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat(Avm.String arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormat();


    }
}
