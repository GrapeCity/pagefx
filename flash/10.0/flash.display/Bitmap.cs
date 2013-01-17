using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The Bitmap class represents display objects that represent bitmap images. These can be images
    /// that you load with the flash.display.Loader class, or they can be images that you create with
    /// the Bitmap()  constructor.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class Bitmap : DisplayObject
    {
        public extern virtual BitmapData bitmapData
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String pixelSnapping
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool smoothing
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Bitmap(BitmapData arg0, Avm.String arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Bitmap(BitmapData arg0, Avm.String arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Bitmap(BitmapData arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Bitmap();


    }
}
