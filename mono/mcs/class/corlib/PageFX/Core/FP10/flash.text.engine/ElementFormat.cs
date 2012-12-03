using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class ElementFormat : Avm.Object
    {
        public extern virtual double baselineShift
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double trackingLeft
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String dominantBaseline
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual uint color
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String alignmentBaseline
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String textRotation
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String kerning
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double trackingRight
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String breakOpportunity
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String digitWidth
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual FontDescription fontDescription
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool locked
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double alpha
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String ligatureLevel
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double fontSize
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String locale
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String typographicCase
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String digitCase
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ElementFormat(FontDescription arg0, double arg1, uint arg2, double arg3, Avm.String arg4, Avm.String arg5, Avm.String arg6, double arg7, Avm.String arg8, double arg9, double arg10, Avm.String arg11, Avm.String arg12, Avm.String arg13, Avm.String arg14, Avm.String arg15, Avm.String arg16);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ElementFormat(FontDescription arg0, double arg1, uint arg2, double arg3, Avm.String arg4, Avm.String arg5, Avm.String arg6, double arg7, Avm.String arg8, double arg9, double arg10, Avm.String arg11, Avm.String arg12, Avm.String arg13, Avm.String arg14, Avm.String arg15);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ElementFormat(FontDescription arg0, double arg1, uint arg2, double arg3, Avm.String arg4, Avm.String arg5, Avm.String arg6, double arg7, Avm.String arg8, double arg9, double arg10, Avm.String arg11, Avm.String arg12, Avm.String arg13, Avm.String arg14);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ElementFormat(FontDescription arg0, double arg1, uint arg2, double arg3, Avm.String arg4, Avm.String arg5, Avm.String arg6, double arg7, Avm.String arg8, double arg9, double arg10, Avm.String arg11, Avm.String arg12, Avm.String arg13);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ElementFormat(FontDescription arg0, double arg1, uint arg2, double arg3, Avm.String arg4, Avm.String arg5, Avm.String arg6, double arg7, Avm.String arg8, double arg9, double arg10, Avm.String arg11, Avm.String arg12);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ElementFormat(FontDescription arg0, double arg1, uint arg2, double arg3, Avm.String arg4, Avm.String arg5, Avm.String arg6, double arg7, Avm.String arg8, double arg9, double arg10, Avm.String arg11);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ElementFormat(FontDescription arg0, double arg1, uint arg2, double arg3, Avm.String arg4, Avm.String arg5, Avm.String arg6, double arg7, Avm.String arg8, double arg9, double arg10);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ElementFormat(FontDescription arg0, double arg1, uint arg2, double arg3, Avm.String arg4, Avm.String arg5, Avm.String arg6, double arg7, Avm.String arg8, double arg9);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ElementFormat(FontDescription arg0, double arg1, uint arg2, double arg3, Avm.String arg4, Avm.String arg5, Avm.String arg6, double arg7, Avm.String arg8);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ElementFormat(FontDescription arg0, double arg1, uint arg2, double arg3, Avm.String arg4, Avm.String arg5, Avm.String arg6, double arg7);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ElementFormat(FontDescription arg0, double arg1, uint arg2, double arg3, Avm.String arg4, Avm.String arg5, Avm.String arg6);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ElementFormat(FontDescription arg0, double arg1, uint arg2, double arg3, Avm.String arg4, Avm.String arg5);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ElementFormat(FontDescription arg0, double arg1, uint arg2, double arg3, Avm.String arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ElementFormat(FontDescription arg0, double arg1, uint arg2, double arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ElementFormat(FontDescription arg0, double arg1, uint arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ElementFormat(FontDescription arg0, double arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ElementFormat(FontDescription arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ElementFormat();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual ElementFormat clone();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual FontMetrics getFontMetrics();
    }
}
