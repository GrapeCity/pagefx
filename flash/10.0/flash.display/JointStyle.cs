using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The JointStyle class is an enumeration of constant values that specify the joint
    /// style to use in drawing lines. These constants are provided for use as values in
    /// the joints  parameter of the flash.display.Graphics.lineStyle()
    /// method. The method supports three types of joints: miter, round, and bevel, as the
    /// following example shows:
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class JointStyle : Avm.Object
    {
        /// <summary>
        /// Specifies mitered joints in the joints parameter of the flash.display.Graphics.lineStyle()
        /// method.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String MITER;

        /// <summary>
        /// Specifies beveled joints in the joints parameter of the flash.display.Graphics.lineStyle()
        /// method.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String BEVEL;

        /// <summary>
        /// Specifies round joints in the joints parameter of the flash.display.Graphics.lineStyle()
        /// method.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ROUND;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern JointStyle();
    }
}
