using System;
using System.Runtime.CompilerServices;

namespace flash.desktop
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class ClipboardFormats : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String TEXT_FORMAT;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String HTML_FORMAT;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String RICH_TEXT_FORMAT;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String BITMAP_FORMAT;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String FILE_LIST_FORMAT;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String URL_FORMAT;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ClipboardFormats();
    }
}
