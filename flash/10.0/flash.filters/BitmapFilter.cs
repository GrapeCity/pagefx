using System;
using System.Runtime.CompilerServices;

namespace flash.filters
{
    /// <summary>The BitmapFilter class is the base class for all image filter effects.</summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class BitmapFilter : Avm.Object
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BitmapFilter();

        /// <summary>
        /// Returns a BitmapFilter object that is an exact copy of the original
        /// BitmapFilter object.
        /// </summary>
        /// <returns>A BitmapFilter object.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual BitmapFilter clone();
    }
}
