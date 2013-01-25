using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    /// <summary>
    /// The TextFieldAutoSize class is an enumeration of constant values used in setting the autoSize
    /// property of the TextField class.
    /// </summary>
    [PageFX.AbcInstance(231)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class TextFieldAutoSize : Avm.Object
    {
        /// <summary>Specifies that no resizing is to occur.</summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String NONE;

        /// <summary>
        /// Specifies that the text is to be treated as left-justified text,
        /// meaning that the left side of the text field remains fixed and any
        /// resizing of a single line is on the right side.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String LEFT;

        /// <summary>
        /// Specifies that the text is to be treated as center-justified text.
        /// Any resizing of a single line of a text field is equally distributed
        /// to both the right and left sides.
        /// </summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String CENTER;

        /// <summary>
        /// Specifies that the text is to be treated as right-justified text,
        /// meaning that the right side of the text field remains fixed and any
        /// resizing of a single line is on the left side.
        /// </summary>
        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String RIGHT;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextFieldAutoSize();
    }
}
