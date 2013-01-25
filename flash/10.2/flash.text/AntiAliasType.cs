using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    /// <summary>The AntiAliasType class provides values for anti-aliasing in the flash.text.TextField class.</summary>
    [PageFX.AbcInstance(153)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class AntiAliasType : Avm.Object
    {
        /// <summary>
        /// Sets anti-aliasing to the anti-aliasing that is used in Flash Player 7 and earlier.
        /// This setting is recommended for applications that do not have a lot of text.
        /// This constant is used for the antiAliasType property in the TextField
        /// class.
        /// Use the syntax AntiAliasType.NORMAL.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String NORMAL;

        /// <summary>
        /// Sets anti-aliasing to advanced anti-aliasing. Advanced anti-aliasing
        /// allows font faces to be rendered at very high quality at small sizes. It is best used
        /// with applications that have a lot of small text. Advanced anti-aliasing is not recommended
        /// for very large fonts (larger than 48 points).
        /// This constant is used for the antiAliasType property in the TextField
        /// class.
        /// Use the syntax AntiAliasType.ADVANCED.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ADVANCED;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern AntiAliasType();
    }
}
