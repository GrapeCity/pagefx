using System;
using System.Runtime.CompilerServices;

namespace flash.system
{
    /// <summary>
    /// This class contains constants for use with the IME.conversionMode
    /// property. Setting conversionMode  to either
    /// ALPHANUMERIC_FULL  or JAPANESE_KATAKANA_FULL  causes the
    /// player to use a full width font, whereas using ALPHANUMERIC_HALF  or
    /// JAPANESE_KATAKANA_HALF  uses a half width font.
    /// </summary>
    [PageFX.AbcInstance(285)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class IMEConversionMode : Avm.Object
    {
        /// <summary>
        /// The string &quot;ALPHANUMERIC_FULL&quot;, for use with the
        /// IME.conversionMode property.
        /// This constant is used with all IMEs.
        /// Use the syntax IMEConversionMode.ALPHANUMERIC_FULL.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ALPHANUMERIC_FULL;

        /// <summary>
        /// The string &quot;ALPHANUMERIC_HALF&quot;, for use with the
        /// IME.conversionMode property.
        /// This constant is used with all IMEs.
        /// Use the syntax IMEConversionMode.ALPHANUMERIC_HALF.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ALPHANUMERIC_HALF;

        /// <summary>
        /// The string &quot;CHINESE&quot;, for use with the
        /// IME.conversionMode property.
        /// This constant is used with simplified and traditional Chinese IMEs.
        /// Use the syntax IMEConversionMode.CHINESE.
        /// </summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String CHINESE;

        /// <summary>
        /// The string &quot;JAPANESE_HIRAGANA&quot;, for use with the
        /// IME.conversionMode property.
        /// This constant is used with Japanese IMEs.
        /// Use the syntax IMEConversionMode.JAPANESE_HIRAGANA.
        /// </summary>
        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String JAPANESE_HIRAGANA;

        /// <summary>
        /// The string &quot;JAPANESE_KATAKANA_FULL&quot;, for use with the
        /// IME.conversionMode property.
        /// This constant is used with Japanese IMEs.
        /// Use the syntax IMEConversionMode.JAPANESE_KATAKANA_FULL.
        /// </summary>
        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String JAPANESE_KATAKANA_FULL;

        /// <summary>
        /// The string &quot;JAPANESE_KATAKANA_HALF&quot;, for use with the
        /// IME.conversionMode property.
        /// This constant is used with Japanese IMEs.
        /// Use the syntax IMEConversionMode.JAPANESE_KATAKANA_HALF.
        /// </summary>
        [PageFX.AbcClassTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String JAPANESE_KATAKANA_HALF;

        /// <summary>
        /// The string &quot;KOREAN&quot;, for use with the
        /// IME.conversionMode property.
        /// This constant is used with Korean IMEs.
        /// Use the syntax IMEConversionMode.KOREAN.
        /// </summary>
        [PageFX.AbcClassTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String KOREAN;

        /// <summary>
        /// The string &quot;UNKNOWN&quot;, which can be returned by a call to
        /// the IME.conversionMode property. This value cannot be set,
        /// and is returned only if the player is unable to identify the currently
        /// active IME.
        /// Use the syntax IMEConversionMode.UNKNOWN.
        /// </summary>
        [PageFX.AbcClassTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String UNKNOWN;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IMEConversionMode();
    }
}
