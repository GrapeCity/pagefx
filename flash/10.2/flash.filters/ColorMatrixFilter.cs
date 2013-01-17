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
    [PageFX.AbcInstance(288)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class ColorMatrixFilter : flash.filters.BitmapFilter
    {
        /// <summary>
        /// An array of 20 items for 4 x 5 color transform. The matrix property cannot
        /// be changed by directly modifying its value (for example, myFilter.matrix[2] = 1;).
        /// Instead, you must get a reference to the array, make the change to the reference, and reset the
        /// value.
        /// The color matrix filter separates each source pixel into its red, green, blue,
        /// and alpha components as srcR, srcG, srcB, srcA. To calculate the result of each of
        /// the four channels, the value of each pixel in the image is multiplied by the values in
        /// the transformation matrix. An offset, between -255 and 255, can optionally be added
        /// to each result (the fifth item in each row of the matrix). The filter combines each
        /// color component back into a single pixel and writes out the result. In the following formula,
        /// a[0] through a[19] correspond to entries 0 through 19 in the 20-item array that is
        /// passed to the matrix property:
        /// redResult   = (a[0]  * srcR) + (a[1]  * srcG) + (a[2]  * srcB) + (a[3]  * srcA) + a[4]
        /// greenResult = (a[5]  * srcR) + (a[6]  * srcG) + (a[7]  * srcB) + (a[8]  * srcA) + a[9]
        /// blueResult  = (a[10] * srcR) + (a[11] * srcG) + (a[12] * srcB) + (a[13] * srcA) + a[14]
        /// alphaResult = (a[15] * srcR) + (a[16] * srcG) + (a[17] * srcB) + (a[18] * srcA) + a[19]
        /// For each color value in the array, a value of 1 is equal to 100% of that channel
        /// being sent to the output, preserving the value of the color channel.The calculations are performed on unmultiplied color values. If the input graphic consists
        /// of premultiplied color values, those values are automatically converted into unmultiplied color
        /// values for this operation.Two optimized modes are available:Alpha only. When you pass to the filter a matrix that adjusts only the alpha component, as shown here, the filter optimizes its performance:
        /// 1 0 0 0 0
        /// 0 1 0 0 0
        /// 0 0 1 0 0
        /// 0 0 0 N 0  (where N is between 0.0 and 1.0)
        /// Faster version. Available only with SSE/AltiVec accelerator-enabled processors,
        /// such as Intel® Pentium 3 and later and Apple® G4 and later. The accelerator is used when the multiplier terms are in the range
        /// -15.99 to 15.99 and the adder terms a[4], a[9], a[14], and a[19] are in the range -8000 to 8000.
        /// </summary>
        public extern virtual Avm.Array matrix
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorMatrixFilter(Avm.Array matrix);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ColorMatrixFilter();

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.filters.BitmapFilter clone();
    }
}
