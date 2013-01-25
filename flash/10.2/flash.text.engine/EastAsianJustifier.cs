using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(222)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class EastAsianJustifier : flash.text.engine.TextJustifier
    {
        public extern virtual Avm.String justificationStyle
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EastAsianJustifier(Avm.String locale, Avm.String lineJustification, Avm.String justificationStyle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EastAsianJustifier(Avm.String locale, Avm.String lineJustification);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EastAsianJustifier(Avm.String locale);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EastAsianJustifier();

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.text.engine.TextJustifier clone();
    }
}
