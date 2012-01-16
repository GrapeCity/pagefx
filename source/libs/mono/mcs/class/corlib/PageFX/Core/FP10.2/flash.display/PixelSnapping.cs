using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The PixelSnapping class is an enumeration of constant values for setting the pixel snapping options
    /// by using the pixelSnapping  property of a Bitmap object.
    /// </summary>
    [PageFX.AbcInstance(135)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class PixelSnapping : Avm.Object
    {
        /// <summary>
        /// A constant value used in the pixelSnapping property of a Bitmap object
        /// to specify that no pixel snapping occurs.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String NEVER;

        /// <summary>
        /// A constant value used in the pixelSnapping property of a Bitmap object
        /// to specify that the bitmap image is always snapped to the nearest
        /// pixel, independent of any transformation.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ALWAYS;

        /// <summary>
        /// A constant value used in the pixelSnapping property of a Bitmap object
        /// to specify that the bitmap image is snapped to the nearest pixel if it is drawn with no rotation
        /// or skew and it is drawn at a scale factor of 99.9% to 100.1%. If these conditions are satisfied,
        /// the image is drawn at 100% scale, snapped to the nearest pixel. Internally, this setting allows the image
        /// to be drawn as fast as possible by using the vector renderer.
        /// </summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String AUTO;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern PixelSnapping();
    }
}
