using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The Bitmap class represents display objects that represent bitmap images. These can be images
    /// that you load with the flash.display.Loader class, or they can be images that you create with
    /// the Bitmap()  constructor.
    /// </summary>
    [PageFX.AbcInstance(65)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class Bitmap : flash.display.DisplayObject
    {
        /// <summary>
        /// Controls whether or not the Bitmap object is snapped to the nearest pixel. The PixelSnapping
        /// class includes possible values:
        /// PixelSnapping.NEVERNo pixel snapping occurs.PixelSnapping.ALWAYSThe image is always snapped to the nearest
        /// pixel, independent of transformation.PixelSnapping.AUTOThe image is snapped
        /// to the nearest pixel if it is drawn with no rotation
        /// or skew and it is drawn at a scale factor of 99.9% to 100.1%. If these conditions are satisfied,
        /// the bitmap image is drawn at 100% scale, snapped to the nearest pixel. Internally, this value allows the image
        /// to be drawn as fast as possible using the vector renderer.
        /// </summary>
        public extern virtual Avm.String pixelSnapping
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Controls whether or not the bitmap is smoothed when scaled. If true, the bitmap is
        /// smoothed when scaled. If false, the bitmap is not smoothed when scaled.
        /// </summary>
        public extern virtual bool smoothing
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>The BitmapData object being referenced.</summary>
        public extern virtual flash.display.BitmapData bitmapData
        {
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Bitmap(flash.display.BitmapData bitmapData, Avm.String pixelSnapping, bool smoothing);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Bitmap(flash.display.BitmapData bitmapData, Avm.String pixelSnapping);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Bitmap(flash.display.BitmapData bitmapData);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Bitmap();


    }
}
