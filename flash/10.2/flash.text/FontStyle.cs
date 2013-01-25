using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    /// <summary>The FontStyle class provides values for the TextRenderer class.</summary>
    [PageFX.AbcInstance(104)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class FontStyle : Avm.Object
    {
        /// <summary>Defines the plain style of a font for the fontStyle parameter in the setAdvancedAntiAliasingTable() method. Use the syntax FontStyle.REGULAR.</summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String REGULAR;

        /// <summary>Defines the bold style of a font for the fontStyle parameter in the setAdvancedAntiAliasingTable() method. Use the syntax FontStyle.BOLD.</summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String BOLD;

        /// <summary>Defines the italic style of a font for the fontStyle parameter in the setAdvancedAntiAliasingTable() method. Use the syntax FontStyle.ITALIC.</summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ITALIC;

        /// <summary>Defines the combined bold and italic style of a font for the fontStyle parameter in the setAdvancedAntiAliasingTable() method. Use the syntax FontStyle.BOLD_ITALIC.</summary>
        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String BOLD_ITALIC;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontStyle();
    }
}
