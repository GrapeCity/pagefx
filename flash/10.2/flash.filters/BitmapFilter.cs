using System;
using System.Runtime.CompilerServices;

namespace flash.filters
{
    /// <summary>The BitmapFilter class is the base class for all image filter effects.</summary>
    [PageFX.AbcInstance(113)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class BitmapFilter : Avm.Object
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BitmapFilter();

        /// <summary>
        /// Returns a BitmapFilter object that is an exact copy of the original
        /// BitmapFilter object.
        /// </summary>
        /// <returns>A BitmapFilter object.</returns>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.filters.BitmapFilter clone();
    }
}
