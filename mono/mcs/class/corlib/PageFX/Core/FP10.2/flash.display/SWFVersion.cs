using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The SWFVersion class is an enumeration of constant values that indicate the
    /// file format version of a loaded SWF file.
    /// The SWFVersion constants are provided for use in checking the
    /// swfVersion  property of a flash.display.LoaderInfo object.
    /// </summary>
    [PageFX.AbcInstance(149)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class SWFVersion : Avm.Object
    {
        /// <summary>SWF file format version 1.0.</summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint FLASH1;

        /// <summary>SWF file format version 2.0.</summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint FLASH2;

        /// <summary>SWF file format version 3.0.</summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint FLASH3;

        /// <summary>SWF file format version 4.0.</summary>
        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint FLASH4;

        /// <summary>SWF file format version 5.0.</summary>
        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint FLASH5;

        /// <summary>SWF file format version 6.0.</summary>
        [PageFX.AbcClassTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint FLASH6;

        /// <summary>SWF file format version 7.0.</summary>
        [PageFX.AbcClassTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint FLASH7;

        /// <summary>SWF file format version 8.0.</summary>
        [PageFX.AbcClassTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint FLASH8;

        /// <summary>SWF file format version 9.0.</summary>
        [PageFX.AbcClassTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint FLASH9;

        [PageFX.AbcClassTrait(9)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint FLASH10;

        [PageFX.AbcClassTrait(10)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static uint FLASH11;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SWFVersion();
    }
}
