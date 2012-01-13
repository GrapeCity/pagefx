using System;
using System.Runtime.CompilerServices;

namespace flash.filters
{
    /// <summary>The BitmapFilterType class contains values to set the type of a BitmapFilter.</summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class BitmapFilterType : Avm.Object
    {
        /// <summary>Defines the setting that applies a filter to the outer area of an object.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String OUTER;

        /// <summary>Defines the setting that applies a filter to the inner area of an object.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String INNER;

        /// <summary>Defines the setting that applies a filter to the entire area of an object.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String FULL;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BitmapFilterType();
    }
}
