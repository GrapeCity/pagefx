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
    [PageFX.AbcInstance(317)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class JointStyle : Avm.Object
    {
        /// <summary>
        /// Specifies round joints in the joints parameter of the flash.display.Graphics.lineStyle()
        /// method.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ROUND;

        /// <summary>
        /// Specifies beveled joints in the joints parameter of the flash.display.Graphics.lineStyle()
        /// method.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String BEVEL;

        /// <summary>
        /// Specifies mitered joints in the joints parameter of the flash.display.Graphics.lineStyle()
        /// method.
        /// </summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String MITER;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern JointStyle();
    }
}
