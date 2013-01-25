using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(260)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class TextBlock : Avm.Object
    {
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public object userData;

        public extern virtual bool applyNonLinearFontScaling
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual flash.text.engine.FontDescription baselineFontDescription
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double baselineFontSize
        {
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String baselineZero
        {
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual flash.text.engine.ContentElement content
        {
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int bidiLevel
        {
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(12)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual flash.text.engine.TextLine firstInvalidLine
        {
            [PageFX.AbcInstanceTrait(13)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual flash.text.engine.TextLine firstLine
        {
            [PageFX.AbcInstanceTrait(14)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual flash.text.engine.TextLine lastLine
        {
            [PageFX.AbcInstanceTrait(15)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual flash.text.engine.TextJustifier textJustifier
        {
            [PageFX.AbcInstanceTrait(16)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(17)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String textLineCreationResult
        {
            [PageFX.AbcInstanceTrait(20)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String lineRotation
        {
            [PageFX.AbcInstanceTrait(21)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(22)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

		public extern virtual Avm.Vector<TabStop> tabStops
        {
            [PageFX.AbcInstanceTrait(23)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(24)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextBlock(flash.text.engine.ContentElement content, Avm.Vector<TabStop> tabStops, flash.text.engine.TextJustifier textJustifier, Avm.String lineRotation, Avm.String baselineZero, int bidiLevel, bool applyNonLinearFontScaling, flash.text.engine.FontDescription baselineFontDescription, double baselineFontSize);

        [MethodImpl(MethodImplOptions.InternalCall)]
		public extern TextBlock(flash.text.engine.ContentElement content, Avm.Vector<TabStop> tabStops, flash.text.engine.TextJustifier textJustifier, Avm.String lineRotation, Avm.String baselineZero, int bidiLevel, bool applyNonLinearFontScaling, flash.text.engine.FontDescription baselineFontDescription);

        [MethodImpl(MethodImplOptions.InternalCall)]
		public extern TextBlock(flash.text.engine.ContentElement content, Avm.Vector<TabStop> tabStops, flash.text.engine.TextJustifier textJustifier, Avm.String lineRotation, Avm.String baselineZero, int bidiLevel, bool applyNonLinearFontScaling);

        [MethodImpl(MethodImplOptions.InternalCall)]
		public extern TextBlock(flash.text.engine.ContentElement content, Avm.Vector<TabStop> tabStops, flash.text.engine.TextJustifier textJustifier, Avm.String lineRotation, Avm.String baselineZero, int bidiLevel);

        [MethodImpl(MethodImplOptions.InternalCall)]
		public extern TextBlock(flash.text.engine.ContentElement content, Avm.Vector<TabStop> tabStops, flash.text.engine.TextJustifier textJustifier, Avm.String lineRotation, Avm.String baselineZero);

        [MethodImpl(MethodImplOptions.InternalCall)]
		public extern TextBlock(flash.text.engine.ContentElement content, Avm.Vector<TabStop> tabStops, flash.text.engine.TextJustifier textJustifier, Avm.String lineRotation);

        [MethodImpl(MethodImplOptions.InternalCall)]
		public extern TextBlock(flash.text.engine.ContentElement content, Avm.Vector<TabStop> tabStops, flash.text.engine.TextJustifier textJustifier);

        [MethodImpl(MethodImplOptions.InternalCall)]
		public extern TextBlock(flash.text.engine.ContentElement content, Avm.Vector<TabStop> tabStops);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextBlock(flash.text.engine.ContentElement content);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextBlock();

        [PageFX.AbcInstanceTrait(27)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int findNextAtomBoundary(int afterCharIndex);

        [PageFX.AbcInstanceTrait(28)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int findPreviousAtomBoundary(int beforeCharIndex);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int findNextWordBoundary(int afterCharIndex);

        [PageFX.AbcInstanceTrait(30)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int findPreviousWordBoundary(int beforeCharIndex);

        [PageFX.AbcInstanceTrait(31)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.text.engine.TextLine getTextLineAtCharIndex(int charIndex);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.text.engine.TextLine createTextLine(flash.text.engine.TextLine previousLine, double width, double lineOffset, bool fitSomething);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern flash.text.engine.TextLine createTextLine(flash.text.engine.TextLine previousLine, double width, double lineOffset);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern flash.text.engine.TextLine createTextLine(flash.text.engine.TextLine previousLine, double width);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern flash.text.engine.TextLine createTextLine(flash.text.engine.TextLine previousLine);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern flash.text.engine.TextLine createTextLine();

        [PageFX.AbcInstanceTrait(33)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.text.engine.TextLine recreateTextLine(flash.text.engine.TextLine textLine, flash.text.engine.TextLine previousLine, double width, double lineOffset, bool fitSomething);

        [PageFX.AbcInstanceTrait(33)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern flash.text.engine.TextLine recreateTextLine(flash.text.engine.TextLine textLine, flash.text.engine.TextLine previousLine, double width, double lineOffset);

        [PageFX.AbcInstanceTrait(33)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern flash.text.engine.TextLine recreateTextLine(flash.text.engine.TextLine textLine, flash.text.engine.TextLine previousLine, double width);

        [PageFX.AbcInstanceTrait(33)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern flash.text.engine.TextLine recreateTextLine(flash.text.engine.TextLine textLine, flash.text.engine.TextLine previousLine);

        [PageFX.AbcInstanceTrait(33)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern flash.text.engine.TextLine recreateTextLine(flash.text.engine.TextLine textLine);

        [PageFX.AbcInstanceTrait(35)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void releaseLineCreationData();

        [PageFX.AbcInstanceTrait(36)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void releaseLines(flash.text.engine.TextLine firstLine, flash.text.engine.TextLine lastLine);

        [PageFX.AbcInstanceTrait(37)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String dump();
    }
}
