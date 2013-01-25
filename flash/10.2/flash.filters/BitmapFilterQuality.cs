using System;
using System.Runtime.CompilerServices;

namespace flash.filters
{
    /// <summary>The BitmapFilterQuality class contains values to set the rendering quality of a BitmapFilter object.</summary>
    [PageFX.AbcInstance(169)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class BitmapFilterQuality : Avm.Object
    {
        /// <summary>Defines the low quality filter setting.</summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static int LOW;

        /// <summary>Defines the medium quality filter setting.</summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static int MEDIUM;

        /// <summary>Defines the high quality filter setting.</summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static int HIGH;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BitmapFilterQuality();
    }
}
