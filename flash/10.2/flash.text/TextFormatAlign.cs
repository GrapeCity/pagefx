using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    /// <summary>The TextFormatAlign class provides values for text alignment in the TextFormat class.</summary>
    [PageFX.AbcInstance(161)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class TextFormatAlign : Avm.Object
    {
        /// <summary>
        /// Constant; aligns text to the left within the text field.
        /// Use the syntax TextFormatAlign.LEFT.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String LEFT;

        /// <summary>
        /// Constant; centers the text in the text field.
        /// Use the syntax TextFormatAlign.CENTER.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String CENTER;

        /// <summary>
        /// Constant; aligns text to the right within the text field.
        /// Use the syntax TextFormatAlign.RIGHT.
        /// </summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String RIGHT;

        /// <summary>
        /// Constant; justifies text within the text field.
        /// Use the syntax TextFormatAlign.JUSTIFY.
        /// </summary>
        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String JUSTIFY;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormatAlign();
    }
}
