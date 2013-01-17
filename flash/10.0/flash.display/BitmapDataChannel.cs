using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The BitmapDataChannel class is an enumeration of constant values that indicate which channel to
    /// use: red, blue, green, or alpha transparency.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class BitmapDataChannel : Avm.Object
    {
        /// <summary>The red channel.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint RED;

        /// <summary>The green channel.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint GREEN;

        /// <summary>The blue channel.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint BLUE;

        /// <summary>The alpha channel.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint ALPHA;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BitmapDataChannel();
    }
}
