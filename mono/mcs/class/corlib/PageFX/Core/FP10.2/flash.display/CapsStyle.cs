using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The CapsStyle class is an enumeration of constant values that specify the caps style to use in drawing lines.
    /// The constants are provided for use as values in the caps  parameter of the
    /// flash.display.Graphics.lineStyle()  method. You can specify the following three types of caps:
    /// </summary>
    [PageFX.AbcInstance(353)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class CapsStyle : Avm.Object
    {
        /// <summary>
        /// Used to specify round caps in the caps parameter of the
        /// flash.display.Graphics.lineStyle() method.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ROUND;

        /// <summary>
        /// Used to specify no caps in the caps parameter of the
        /// flash.display.Graphics.lineStyle() method.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String NONE;

        /// <summary>
        /// Used to specify square caps in the caps parameter of the
        /// flash.display.Graphics.lineStyle() method.
        /// </summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String SQUARE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern CapsStyle();
    }
}
