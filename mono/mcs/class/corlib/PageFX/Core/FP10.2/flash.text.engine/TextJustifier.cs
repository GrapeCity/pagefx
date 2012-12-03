using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(80)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class TextJustifier : Avm.Object
    {
        public extern virtual Avm.String locale
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String lineJustification
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextJustifier(Avm.String locale, Avm.String lineJustification);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.text.engine.TextJustifier clone();

        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.text.engine.TextJustifier getJustifierForLocale(Avm.String locale);
    }
}
