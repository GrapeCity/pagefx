using System;
using System.Runtime.CompilerServices;

namespace flash.filters
{
    /// <summary>
    /// The DisplacementMapFilter class uses the pixel values from the specified BitmapData object
    /// (called the displacement map image ) to perform a displacement of an object.
    /// You can use this filter to apply a warped
    /// or mottled effect to any object that inherits from the DisplayObject class,
    /// such as MovieClip, SimpleButton, TextField, and Video objects, as well as to BitmapData objects.
    /// </summary>
    [PageFX.AbcInstance(237)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class DisplacementMapFilter : flash.filters.BitmapFilter
    {
        /// <summary>A BitmapData object containing the displacement map data.</summary>
        public extern virtual flash.display.BitmapData mapBitmap
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

        /// <summary>
        /// A value that contains the offset of the upper-left corner of
        /// the target display object from the upper-left corner of the map image.
        /// </summary>
        public extern virtual flash.geom.Point mapPoint
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Describes which color channel to use in the map image to displace the x result.
        /// Possible values are BitmapDataChannel constants:
        /// BitmapDataChannel.ALPHABitmapDataChannel.BLUEBitmapDataChannel.GREENBitmapDataChannel.RED
        /// </summary>
        public extern virtual uint componentX
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Describes which color channel to use in the map image to displace the y result.
        /// Possible values are BitmapDataChannel constants:
        /// BitmapDataChannel.ALPHABitmapDataChannel.BLUEBitmapDataChannel.GREENBitmapDataChannel.RED
        /// </summary>
        public extern virtual uint componentY
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>The multiplier to use to scale the x displacement result from the map calculation.</summary>
        public extern virtual double scaleX
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>The multiplier to use to scale the y displacement result from the map calculation.</summary>
        public extern virtual double scaleY
        {
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The mode for the filter. Possible values are DisplacementMapFilterMode
        /// constants:
        /// DisplacementMapFilterMode.WRAP — Wraps the displacement value to the other side of the source image.DisplacementMapFilterMode.CLAMP — Clamps the displacement value to the edge of the source image.DisplacementMapFilterMode.IGNORE — If the displacement value is out of range, ignores the displacement and uses the source pixel.DisplacementMapFilterMode.COLOR — If the displacement value is outside the image, substitutes the values in the color and alpha properties.
        /// </summary>
        public extern virtual Avm.String mode
        {
            [PageFX.AbcInstanceTrait(12)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(13)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Specifies what color to use for out-of-bounds displacements.  The valid range of
        /// displacements is 0.0 to 1.0. Values are in hexadecimal format. The default value
        /// for color is 0. Use this property if the mode property
        /// is set to DisplacementMapFilterMode.COLOR.
        /// </summary>
        public extern virtual uint color
        {
            [PageFX.AbcInstanceTrait(14)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(15)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Specifies the alpha transparency value to use for out-of-bounds displacements.
        /// It is specified as a normalized value from 0.0 to 1.0. For example,
        /// .25 sets a transparency value of 25%. The default value is 0.
        /// Use this property if the mode property is set to DisplacementMapFilterMode.COLOR.
        /// </summary>
        public extern virtual double alpha
        {
            [PageFX.AbcInstanceTrait(16)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(17)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplacementMapFilter(flash.display.BitmapData mapBitmap, flash.geom.Point mapPoint, uint componentX, uint componentY, double scaleX, double scaleY, Avm.String mode, uint color, double alpha);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplacementMapFilter(flash.display.BitmapData mapBitmap, flash.geom.Point mapPoint, uint componentX, uint componentY, double scaleX, double scaleY, Avm.String mode, uint color);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplacementMapFilter(flash.display.BitmapData mapBitmap, flash.geom.Point mapPoint, uint componentX, uint componentY, double scaleX, double scaleY, Avm.String mode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplacementMapFilter(flash.display.BitmapData mapBitmap, flash.geom.Point mapPoint, uint componentX, uint componentY, double scaleX, double scaleY);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplacementMapFilter(flash.display.BitmapData mapBitmap, flash.geom.Point mapPoint, uint componentX, uint componentY, double scaleX);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplacementMapFilter(flash.display.BitmapData mapBitmap, flash.geom.Point mapPoint, uint componentX, uint componentY);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplacementMapFilter(flash.display.BitmapData mapBitmap, flash.geom.Point mapPoint, uint componentX);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplacementMapFilter(flash.display.BitmapData mapBitmap, flash.geom.Point mapPoint);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplacementMapFilter(flash.display.BitmapData mapBitmap);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplacementMapFilter();

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.filters.BitmapFilter clone();
    }
}
