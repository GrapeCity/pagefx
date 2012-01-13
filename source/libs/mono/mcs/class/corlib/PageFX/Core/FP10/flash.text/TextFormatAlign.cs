using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    /// <summary>The TextFormatAlign class provides values for text alignment in the TextFormat class.</summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class TextFormatAlign : Avm.Object
    {
        /// <summary>
        /// Constant; justifies text within the text field.
        /// Use the syntax TextFormatAlign.JUSTIFY.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String JUSTIFY;

        /// <summary>
        /// Constant; centers the text in the text field.
        /// Use the syntax TextFormatAlign.CENTER.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String CENTER;

        /// <summary>
        /// Constant; aligns text to the left within the text field.
        /// Use the syntax TextFormatAlign.LEFT.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String LEFT;

        /// <summary>
        /// Constant; aligns text to the right within the text field.
        /// Use the syntax TextFormatAlign.RIGHT.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String RIGHT;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFormatAlign();
    }
}
