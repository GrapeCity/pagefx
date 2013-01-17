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
    [PageFX.ABC]
    [PageFX.FP9]
    public class SWFVersion : Avm.Object
    {
        /// <summary>SWF file format version 1.0.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint FLASH1;

        /// <summary>SWF file format version 3.0.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint FLASH3;

        /// <summary>SWF file format version 5.0.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint FLASH5;

        /// <summary>SWF file format version 7.0.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint FLASH7;

        /// <summary>SWF file format version 4.0.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint FLASH4;

        /// <summary>SWF file format version 6.0.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint FLASH6;

        /// <summary>SWF file format version 8.0.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint FLASH8;

        /// <summary>SWF file format version 2.0.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint FLASH2;

        [PageFX.ABC]
        [PageFX.FP10]
        public static uint FLASH10;

        /// <summary>SWF file format version 9.0.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint FLASH9;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SWFVersion();
    }
}
