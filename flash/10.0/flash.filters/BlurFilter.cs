using System;
using System.Runtime.CompilerServices;

namespace flash.filters
{
    /// <summary>
    /// The BlurFilter class lets you apply a blur visual effect to display objects.
    /// A blur effect softens the details of an image. You can produce blurs that
    /// range from a softly unfocused look to a Gaussian blur, a hazy
    /// appearance like viewing an image through semi-opaque glass. When the quality  property
    /// of this filter is set to low, the result is a softly unfocused look.
    /// When the quality  property is set to high, it approximates a Gaussian blur
    /// filter.  You can apply the filter to any display object (that is, objects that inherit
    /// from the DisplayObject class),
    /// such as MovieClip, SimpleButton, TextField, and Video objects, as well as to BitmapData objects.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class BlurFilter : BitmapFilter
    {
        public extern virtual double blurX
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

        public extern virtual double blurY
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

        public extern virtual int quality
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
        public extern BlurFilter(double arg0, double arg1, int arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BlurFilter(double arg0, double arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BlurFilter(double arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BlurFilter();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override BitmapFilter clone();


    }
}
