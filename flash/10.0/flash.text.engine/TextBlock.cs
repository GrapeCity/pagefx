using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class TextBlock : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public object userData;

        public extern virtual TextJustifier textJustifier
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

        public extern virtual TextLine firstLine
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual ContentElement content
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

        public extern virtual Avm.String baselineZero
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

        public extern virtual FontDescription baselineFontDescription
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

        public extern virtual Avm.String lineRotation
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

        public extern virtual bool applyNonLinearFontScaling
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

        public extern virtual int bidiLevel
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

        public extern virtual double baselineFontSize
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

        public extern virtual Avm.Vector<flash.text.engine.TabStop> tabStops
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

        public extern virtual TextLine lastLine
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual TextLine firstInvalidLine
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String textLineCreationResult
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextBlock(ContentElement arg0, Avm.Vector<flash.text.engine.TabStop> arg1, TextJustifier arg2, Avm.String arg3, Avm.String arg4, int arg5, bool arg6, FontDescription arg7, double arg8);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextBlock(ContentElement arg0, Avm.Vector<flash.text.engine.TabStop> arg1, TextJustifier arg2, Avm.String arg3, Avm.String arg4, int arg5, bool arg6, FontDescription arg7);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextBlock(ContentElement arg0, Avm.Vector<flash.text.engine.TabStop> arg1, TextJustifier arg2, Avm.String arg3, Avm.String arg4, int arg5, bool arg6);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextBlock(ContentElement arg0, Avm.Vector<flash.text.engine.TabStop> arg1, TextJustifier arg2, Avm.String arg3, Avm.String arg4, int arg5);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextBlock(ContentElement arg0, Avm.Vector<flash.text.engine.TabStop> arg1, TextJustifier arg2, Avm.String arg3, Avm.String arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextBlock(ContentElement arg0, Avm.Vector<flash.text.engine.TabStop> arg1, TextJustifier arg2, Avm.String arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextBlock(ContentElement arg0, Avm.Vector<flash.text.engine.TabStop> arg1, TextJustifier arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextBlock(ContentElement arg0, Avm.Vector<flash.text.engine.TabStop> arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextBlock(ContentElement arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextBlock();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual TextLine getTextLineAtCharIndex(int arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int findPreviousAtomBoundary(int arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int findNextAtomBoundary(int arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int findNextWordBoundary(int arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int findPreviousWordBoundary(int arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual TextLine createTextLine(TextLine arg0, double arg1, double arg2, bool arg3);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextLine createTextLine(TextLine arg0, double arg1, double arg2);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextLine createTextLine(TextLine arg0, double arg1);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextLine createTextLine(TextLine arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextLine createTextLine();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String dump();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void releaseLines(TextLine arg0, TextLine arg1);
    }
}
