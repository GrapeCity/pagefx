using System;
using System.Runtime.CompilerServices;

namespace flash.filters
{
    /// <summary>
    /// The ColorMatrixFilter class lets you apply a 4 x 5 matrix transformation on the RGBA color and alpha values
    /// of every pixel in the input image to produce a result with a new set of RGBA color and alpha values.
    /// It allows saturation changes, hue rotation, luminance to alpha, and various other effects.
    /// You can apply the filter to any display object (that is, objects that inherit from the DisplayObject class),
    /// such as MovieClip, SimpleButton, TextField, and Video objects, as well as to BitmapData objects.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class ColorMatrixFilter : BitmapFilter
    {
        public extern virtual Avm.Array matrix
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
        public extern ColorMatrixFilter(Avm.Array arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorMatrixFilter();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override BitmapFilter clone();
    }
}
