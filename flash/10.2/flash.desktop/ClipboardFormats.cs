using System;
using System.Runtime.CompilerServices;

namespace flash.desktop
{
    [PageFX.AbcInstance(293)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class ClipboardFormats : Avm.Object
    {
        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String TEXT_FORMAT;

        [PageFX.AbcClassTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String HTML_FORMAT;

        [PageFX.AbcClassTrait(6)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String RICH_TEXT_FORMAT;

        [PageFX.AbcClassTrait(7)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String URL_FORMAT;

        [PageFX.AbcClassTrait(8)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String FILE_LIST_FORMAT;

        [PageFX.AbcClassTrait(9)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String BITMAP_FORMAT;

        [PageFX.AbcClassTrait(10)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String FILE_PROMISE_LIST_FORMAT;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ClipboardFormats();
    }
}
