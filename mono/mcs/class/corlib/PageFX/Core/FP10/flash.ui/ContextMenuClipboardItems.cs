using System;
using System.Runtime.CompilerServices;

namespace flash.ui
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class ContextMenuClipboardItems : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public bool cut;

        [PageFX.ABC]
        [PageFX.FP10]
        public bool paste;

        [PageFX.ABC]
        [PageFX.FP10]
        public bool copy;

        [PageFX.ABC]
        [PageFX.FP10]
        public bool selectAll;

        [PageFX.ABC]
        [PageFX.FP10]
        public bool clear;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenuClipboardItems();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual ContextMenuClipboardItems clone();
    }
}
