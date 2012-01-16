using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The LineScaleMode class provides values for the scaleMode
    /// parameter in the Graphics.lineStyle()  method.
    /// </summary>
    [PageFX.AbcInstance(166)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class LineScaleMode : Avm.Object
    {
        /// <summary>
        /// With this setting used as the scaleMode parameter of the lineStyle()
        /// method, the thickness of the line always scales when the object is scaled (the default).
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String NORMAL;

        /// <summary>
        /// With this setting used as the scaleMode parameter of the lineStyle()
        /// method, the thickness of the line scales only horizontally. For example,
        /// consider the following circles, drawn with a one-pixel line, and each with the
        /// scaleMode parameter set to LineScaleMode.HORIZONTAL. The circle on the left
        /// is scaled only horizontally, and the circle on the right is scaled both vertically and horizontally.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String VERTICAL;

        /// <summary>
        /// With this setting used as the scaleMode parameter of the lineStyle()
        /// method, the thickness of the line scales only vertically. For example,
        /// consider the following circles, drawn with a one-pixel line, and each with the
        /// scaleMode parameter set to LineScaleMode.VERTICAL. The circle on the left
        /// is scaled only vertically, and the circle on the right is scaled both vertically and horizontally.
        /// </summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String HORIZONTAL;

        /// <summary>
        /// With this setting used as the scaleMode parameter of the lineStyle()
        /// method, the thickness of the line never scales.
        /// </summary>
        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String NONE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern LineScaleMode();
    }
}
