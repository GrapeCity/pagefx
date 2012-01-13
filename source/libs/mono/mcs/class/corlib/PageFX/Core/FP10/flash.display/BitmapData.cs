using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The BitmapData class lets you work with the data (pixels) of a Bitmap object. You can use the methods of the
    /// BitmapData class to create arbitrarily sized transparent or opaque bitmap images and manipulate them in various
    /// ways at runtime. You can also access the BitmapData for a bitmap image
    /// that you load with the flash.display.Loader  class.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class BitmapData : Avm.Object, IBitmapDrawable
    {
        public extern virtual int width
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool transparent
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual flash.geom.Rectangle rect
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual int height
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BitmapData(int arg0, int arg1, bool arg2, uint arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BitmapData(int arg0, int arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern BitmapData(int arg0, int arg1);

        /// <summary>
        /// Provides a fast routine to perform pixel manipulation
        /// between images with no stretching, rotation, or color effects. This method copies a
        /// rectangular area of a source image to a
        /// rectangular area of the same size at the destination point of the destination
        /// BitmapData object.
        /// If you include the alphaBitmap and alphaPoint parameters, you can use a secondary
        /// image as an alpha source for the source image. If the source
        /// image has alpha data, both sets of alpha data are used to
        /// composite pixels from the source image to the destination image. The
        /// alphaPoint parameter is the point in the alpha image that
        /// corresponds to the upper-left corner of the source
        /// rectangle. Any pixels outside the intersection of the source
        /// image and alpha image are not copied to the destination image.The mergeAlpha property controls whether or not the alpha
        /// channel is used when a transparent image is copied onto
        /// another transparent image. To copy
        /// pixels with the alpha channel data, set the mergeAlpha
        /// property to true. By default, the mergeAlpha property is
        /// false.
        /// </summary>
        /// <param name="arg0">
        /// The input bitmap image from which to copy pixels. The source image can be a
        /// different BitmapData instance, or it can refer to the current BitmapData
        /// instance.
        /// </param>
        /// <param name="arg1">A rectangle that defines the area of the source image to use as input.</param>
        /// <param name="arg2">
        /// The destination point that represents the upper-left corner of the rectangular
        /// area where the new pixels are placed.
        /// </param>
        /// <param name="arg3">(default = null)  A secondary, alpha BitmapData object source.</param>
        /// <param name="arg4">
        /// (default = null)  The point in the alpha BitmapData object source that corresponds to
        /// the upper-left corner of the sourceRect parameter.
        /// </param>
        /// <param name="arg5">
        /// (default = false)  To use the alpha channel, set the value to
        /// true. To copy pixels with no alpha channel, set the value to
        /// false.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void copyPixels(BitmapData arg0, flash.geom.Rectangle arg1, flash.geom.Point arg2, BitmapData arg3, flash.geom.Point arg4, bool arg5);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void copyPixels(BitmapData arg0, flash.geom.Rectangle arg1, flash.geom.Point arg2, BitmapData arg3, flash.geom.Point arg4);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void copyPixels(BitmapData arg0, flash.geom.Rectangle arg1, flash.geom.Point arg2, BitmapData arg3);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void copyPixels(BitmapData arg0, flash.geom.Rectangle arg1, flash.geom.Point arg2);

        /// <summary>
        /// Sets a single pixel of a BitmapData object. The current
        /// alpha channel value of the image pixel is preserved during this
        /// operation. The value of the RGB color parameter is treated as an unmultiplied color value.
        /// Note: To increase performance, when you use the setPixel() or
        /// setPixel32() method repeatedly, call the lock() method before
        /// you call the setPixel() or setPixel32() method, and then call
        /// the unlock() method when you have made all pixel changes. This process prevents objects
        /// that reference this BitmapData instance from updating until you finish making
        /// the pixel changes.
        /// </summary>
        /// <param name="arg0">The x position of the pixel whose value changes.</param>
        /// <param name="arg1">The y position of the pixel whose value changes.</param>
        /// <param name="arg2">The resulting RGB color for the pixel.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setPixel(int arg0, int arg1, uint arg2);

        /// <summary>
        /// Performs pixel-level hit detection between one bitmap image
        /// and a point, rectangle, or other bitmap image. No stretching,
        /// rotation, or other transformation of either object is considered when
        /// the hit test is performed.
        /// If an image is an opaque image, it is considered a fully opaque rectangle for this
        /// method. Both images must be transparent images to perform pixel-level hit testing that
        /// considers transparency. When you are testing two transparent images, the alpha threshold
        /// parameters control what alpha channel values, from 0 to 255, are considered opaque.
        /// </summary>
        /// <param name="arg0">
        /// A position of the upper-left corner of the BitmapData image in an arbitrary coordinate space.
        /// The same coordinate space is used in defining the secondBitmapPoint parameter.
        /// </param>
        /// <param name="arg1">The highest alpha channel value that is considered opaque for this hit test.</param>
        /// <param name="arg2">A  Rectangle, Point, Bitmap, or BitmapData object.</param>
        /// <param name="arg3">
        /// (default = null)  A point that defines a pixel location in the second BitmapData object.
        /// Use this parameter only when the value of secondObject is a
        /// BitmapData object.
        /// </param>
        /// <param name="arg4">
        /// (default = 1)  The highest alpha channel value that is considered opaque in the second BitmapData object.
        /// Use this parameter only when the value of secondObject is a
        /// BitmapData object and both BitmapData objects are transparent.
        /// </param>
        /// <returns>A value of true if a hit occurs; otherwise, false.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool hitTest(flash.geom.Point arg0, uint arg1, object arg2, flash.geom.Point arg3, uint arg4);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool hitTest(flash.geom.Point arg0, uint arg1, object arg2, flash.geom.Point arg3);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool hitTest(flash.geom.Point arg0, uint arg1, object arg2);

        /// <summary>
        /// Takes a source image and a filter object and generates the
        /// filtered image.
        /// This method relies on the behavior of built-in filter
        /// objects, which determine the destination
        /// rectangle that is affected by an input source rectangle.After a filter is applied, the resulting image can be larger than the input image.
        /// For example, if you use a BlurFilter class
        /// to blur a source rectangle of (50,50,100,100) and a
        /// destination point of (10,10), the area that changes in the
        /// destination image is larger than (10,10,60,60) because of
        /// the blurring. This happens internally during the applyFilter()
        /// call.If the sourceRect parameter of the sourceBitmapData parameter is
        /// an interior region, such as (50,50,100,100) in a 200 x 200 image, the filter uses the source
        /// pixels outside the sourceRect parameter to generate
        /// the destination rectangle.If the BitmapData object and the object specified as the sourceBitmapData
        /// parameter are the same object, Flash Player uses a temporary copy of the object to
        /// perform the filter. For best performance, avoid this situation.
        /// </summary>
        /// <param name="arg0">
        /// The input bitmap image to use. The source image can be a different
        /// BitmapData object or it can refer to the current BitmapData instance.
        /// </param>
        /// <param name="arg1">A rectangle that defines the area of the source image to use as input.</param>
        /// <param name="arg2">
        /// The point within the destination image (the current BitmapData
        /// instance) that corresponds to the upper-left corner of the source rectangle.
        /// </param>
        /// <param name="arg3">
        /// The filter object that you use to perform the filtering operation. Each type
        /// of filter has certain requirements, as follows:
        /// BlurFilter
        /// This filter can use source and destination images
        /// that are either opaque or transparent. If the formats of the images do
        /// not match, the copy of the source image that is made during the
        /// filtering matches the format of the destination image.BevelFilter, DropShadowFilter, GlowFilter, ChromeFilter
        /// The destination image of these filters must be a transparent
        /// image. Calling DropShadowFilter or GlowFilter creates an image that
        /// contains the alpha channel data of the drop shadow or glow. It does not
        /// create the drop shadow onto the destination image. If you use any of these
        /// filters with an opaque destination image, an exception
        /// is thrown (ActionScript 3.0).ConvolutionFilter  This filter can use source and
        /// destination images that are either opaque or transparent.ColorMatrixFilter  This filter can use source and
        /// destination images that are either opaque or transparent.DisplacementMapFilter  This filter can use source and
        /// destination images that are either opaque or transparent, but the
        /// source and destination image formats must be the same.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void applyFilter(BitmapData arg0, flash.geom.Rectangle arg1, flash.geom.Point arg2, flash.filters.BitmapFilter arg3);

        /// <summary>Fills a rectangular area of pixels with a specified ARGB color.</summary>
        /// <param name="arg0">The rectangular area to fill.</param>
        /// <param name="arg1">
        /// The ARGB color value that fills the area. ARGB colors are often
        /// specified in hexadecimal format; for example, 0xFF336699.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void fillRect(flash.geom.Rectangle arg0, uint arg1);

        /// <summary>
        /// Adjusts the color values in a specified area of a bitmap image by using a
        /// ColorTransform object. If the rectangle
        /// matches the boundaries of the bitmap image, this method transforms the color values of
        /// the entire image.
        /// </summary>
        /// <param name="arg0">
        /// A Rectangle object that defines the area of the image in which the
        /// ColorTransform object is applied.
        /// </param>
        /// <param name="arg1">
        /// A ColorTransform object that describes the color transformation
        /// values to apply.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void colorTransform(flash.geom.Rectangle arg0, flash.geom.ColorTransform arg1);

        /// <summary>
        /// Draws the source display object onto the bitmap image, using the
        /// Flash Player vector renderer. You can specify matrix, colorTransform,
        /// blendMode, and a destination clipRect parameter to control
        /// how the rendering performs. Optionally, you can specify whether the bitmap
        /// should be smoothed when scaled (this works only if the source object
        /// is a BitmapData object).
        /// This method directly corresponds to how objects are drawn
        /// with the standard vector renderer for objects in the authoring tool
        /// interface.The source display object does not use any of its applied transformations
        /// for this call. It is treated as it exists in the library or
        /// file, with no matrix transform, no color transform, and no blend
        /// mode. To draw a display object (such as a movie clip) by using its own transform properties,
        /// you can copy its transform property object to the transform property
        /// of the Bitmap object that uses the BitmapData object.Security note: The source object and (in the case of
        /// a Sprite or MovieClip object) all of its child objects must come from the same domain as
        /// the caller, or must be in a SWF file that is accessible to the caller by having called the
        /// Security.allowDomain() method.  If these conditions are not met,
        /// the draw() method does not draw anything.
        /// </summary>
        /// <param name="arg0">
        /// The display object or BitmapData object to draw to the BitmapData object.
        /// (The DisplayObject and BitmapData classes implement the IBitmapDrawable interface.)
        /// </param>
        /// <param name="arg1">
        /// (default = null)  A Matrix object used to scale, rotate, or translate the coordinates
        /// of the bitmap. If you do not want to apply a matrix transformation to the image,
        /// set this parameter to an identity matrix, created with the default
        /// new Matrix() constructor, or pass a null value.
        /// </param>
        /// <param name="arg2">
        /// (default = null)  A ColorTransform object that you use to adjust the color values of
        /// the bitmap. If no object is supplied, the bitmap image&apos;s colors are not transformed.
        /// If you must pass this parameter but you do not want to transform the image, set this
        /// parameter to a ColorTransform object created with the default new ColorTransform()
        /// constructor.
        /// </param>
        /// <param name="arg3">
        /// (default = null)  A string value, from the flash.display.BlendMode class, specifying the
        /// blend mode to be applied to the resulting bitmap.
        /// </param>
        /// <param name="arg4">
        /// (default = null)  A Rectangle object that defines the area of the source object to draw.
        /// If you do not supply this value, no clipping occurs and the entire source object is drawn.
        /// </param>
        /// <param name="arg5">
        /// (default = false)  A Boolean value that determines whether a BitmapData object is
        /// smoothed when scaled or rotated, due to a scaling or rotation in the matrix
        /// parameter. The smoothing parameter only applies if the source
        /// parameter is a BitmapData object. With smoothing set to false,
        /// the rotated or scaled BitmapData image can appear pixelated or jagged. For example, the following
        /// two images use the same BitmapData object for the source parameter, but the
        /// smoothing parameter is set to true on the left and false
        /// on the right:
        /// Drawing a bitmap with smoothing set to true takes longer
        /// than doing so with smoothing set to false.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void draw(IBitmapDrawable arg0, flash.geom.Matrix arg1, flash.geom.ColorTransform arg2, Avm.String arg3, flash.geom.Rectangle arg4, bool arg5);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void draw(IBitmapDrawable arg0, flash.geom.Matrix arg1, flash.geom.ColorTransform arg2, Avm.String arg3, flash.geom.Rectangle arg4);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void draw(IBitmapDrawable arg0, flash.geom.Matrix arg1, flash.geom.ColorTransform arg2, Avm.String arg3);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void draw(IBitmapDrawable arg0, flash.geom.Matrix arg1, flash.geom.ColorTransform arg2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void draw(IBitmapDrawable arg0, flash.geom.Matrix arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void draw(IBitmapDrawable arg0);

        [PageFX.ABC]
        [PageFX.QName("setVector", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setVector(flash.geom.Rectangle arg0, Avm.Vector<uint> arg1);

        /// <summary>
        /// Transfers data from one channel of another BitmapData object or the current
        /// BitmapData object into a channel of the current BitmapData object.
        /// All of the data in the other channels in the destination BitmapData object are
        /// preserved.
        /// The source channel value and destination channel value can be
        /// one of following values: BitmapDataChannel.REDBitmapDataChannel.GREENBitmapDataChannel.BLUEBitmapDataChannel.ALPHA
        /// </summary>
        /// <param name="arg0">
        /// The input bitmap image to use. The source image can be a different BitmapData object
        /// or it can refer to the current BitmapData object.
        /// </param>
        /// <param name="arg1">
        /// The source Rectangle object. To copy only channel data from a smaller area
        /// within the bitmap, specify a source rectangle that is smaller than the overall size of the
        /// BitmapData object.
        /// </param>
        /// <param name="arg2">
        /// The destination Point object that represents the upper-left corner of the rectangular area
        /// where the new channel data is placed.
        /// To copy only channel data
        /// from one area to a different area in the destination image, specify a point other than (0,0).
        /// </param>
        /// <param name="arg3">
        /// The source channel. Use a value from the BitmapDataChannel class
        /// (BitmapDataChannel.RED, BitmapDataChannel.BLUE,
        /// BitmapDataChannel.GREEN, BitmapDataChannel.ALPHA).
        /// </param>
        /// <param name="arg4">
        /// The destination channel. Use a value from the BitmapDataChannel class
        /// (BitmapDataChannel.RED, BitmapDataChannel.BLUE,
        /// BitmapDataChannel.GREEN, BitmapDataChannel.ALPHA).
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void copyChannel(BitmapData arg0, flash.geom.Rectangle arg1, flash.geom.Point arg2, uint arg3, uint arg4);

        /// <summary>
        /// Returns an integer that represents  an RGB pixel value from a BitmapData object at
        /// a specific point (x, y). The getPixel() method returns an
        /// unmultiplied pixel value. No alpha information is returned.
        /// All pixels in a BitmapData object are stored as premultiplied color values.
        /// A premultiplied image pixel has the red, green, and blue
        /// color channel values already multiplied by the alpha data. For example, if the
        /// alpha value is 0, the values for the RGB channels are also 0, independent of their unmultiplied
        /// values. This loss of data can cause some problems when you perform operations. All BitmapData
        /// methods take and return unmultiplied values. The internal pixel representation is converted
        /// from premultiplied to unmultiplied before it is returned as a value. During a set operation,
        /// the pixel value is premultiplied before the raw image pixel is set.
        /// </summary>
        /// <param name="arg0">The x position of the pixel.</param>
        /// <param name="arg1">The y position of the pixel.</param>
        /// <returns>
        /// A number that represents an RGB pixel value. If the (x, y) coordinates are
        /// outside the bounds of the image, the method returns 0.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint getPixel(int arg0, int arg1);

        /// <summary>
        /// Determines the destination rectangle that the applyFilter() method call affects, given a
        /// BitmapData object, a source rectangle, and a filter object.
        /// For example, a blur filter normally affects an area larger than the size of the original
        /// image. A 100 x 200 pixel image that is being filtered by a default BlurFilter
        /// instance, where blurX = blurY = 4 generates a destination rectangle of
        /// (-2,-2,104,204).
        /// The generateFilterRect() method lets you find out the size of this destination
        /// rectangle in advance so that you can size the destination image appropriately before you perform a filter
        /// operation.Some filters clip their destination rectangle based on the source image size.
        /// For example, an inner DropShadow does not generate a larger result than its source
        /// image. In this API, the BitmapData object is used as the source bounds and not the
        /// source rect parameter.
        /// </summary>
        /// <param name="arg0">A rectangle defining the area of the source image to use as input.</param>
        /// <param name="arg1">A filter object that you use to calculate the destination rectangle.</param>
        /// <returns>
        /// A destination rectangle computed by using an image, the sourceRect parameter,
        /// and a filter.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Rectangle generateFilterRect(flash.geom.Rectangle arg0, flash.filters.BitmapFilter arg1);

        /// <summary>
        /// Unlocks an image so that any objects that reference the BitmapData object, such as Bitmap objects,
        /// are updated when this BitmapData object changes. To improve performance, use this method
        /// along with the lock() method before and after numerous calls to the
        /// setPixel() or setPixel32() method.
        /// </summary>
        /// <param name="arg0">
        /// (default = null)  The area of the BitmapData object that has changed. If you do not specify a value for
        /// this parameter, the entire area of the BitmapData object is considered
        /// changed.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void unlock(flash.geom.Rectangle arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void unlock();

        /// <summary>
        /// Scrolls an image by a certain (x, y) pixel amount. Edge
        /// regions outside the scrolling area are left unchanged.
        /// </summary>
        /// <param name="arg0">The amount by which to scroll horizontally.</param>
        /// <param name="arg1">The amount by which to scroll vertically.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void scroll(int arg0, int arg1);

        /// <summary>
        /// Determines a rectangular region that either fully encloses all pixels of a specified color within the
        /// bitmap image (if the findColor parameter is set to true) or fully encloses
        /// all pixels that do not include the specified color (if the findColor parameter is set
        /// to false).
        /// For example, if you have a source image and you want to determine the rectangle of
        /// the image that contains a nonzero alpha channel, pass
        /// {mask: 0xFF000000, color: 0x00000000} as parameters. If the findColor
        /// parameter is set to true, the entire image is searched for the bounds of pixels
        /// for which (value &amp; mask) == color (where value is the color value
        /// of the pixel). If the findColor parameter is set to false, the entire
        /// image is searched for the bounds of pixels for which (value &amp; mask) != color
        /// (where value is the color value of the pixel). To determine white space around an
        /// image, pass {mask: 0xFFFFFFFF, color: 0xFFFFFFFF}
        /// to find the bounds of nonwhite pixels.
        /// </summary>
        /// <param name="arg0">
        /// A hexadecimal value, specifying the bits of the ARGB color to consider. The color
        /// value is combined with this hexadecimal value, by using the &amp; (bitwise AND) operator.
        /// </param>
        /// <param name="arg1">
        /// A hexadecimal value, specifying the ARGB color to match (if findColor
        /// is set to true) or not to match (if findColor
        /// is set to false).
        /// </param>
        /// <param name="arg2">
        /// (default = true)  If the value is set to true, returns the bounds of a color value in an image.
        /// If the value is set to false, returns the bounds of where this color doesn&apos;t exist in an image.
        /// </param>
        /// <returns>The region of the image that is the specified color.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Rectangle getColorBoundsRect(uint arg0, uint arg1, bool arg2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern flash.geom.Rectangle getColorBoundsRect(uint arg0, uint arg1);

        /// <summary>
        /// Performs a pixel dissolve either from a source image to a
        /// destination image or by using the same image. Flash Player uses a randomSeed value
        /// to generate a random pixel dissolve. The return value
        /// of the function must be passed in on subsequent calls to
        /// continue the pixel dissolve until it is finished.
        /// If the source image does not equal the destination image,
        /// pixels are copied from the source to the destination by using all of the
        /// properties. This process allows dissolving from a blank image into a
        /// fully populated image.If the source and destination images are equal, pixels are
        /// filled with the color parameter. This process allows dissolving away
        /// from a fully populated image. In this mode, the destination
        /// point parameter is ignored.
        /// </summary>
        /// <param name="arg0">
        /// The input bitmap image to use. The source image can be a different
        /// BitmapData object, or it can refer to the current BitmapData instance.
        /// </param>
        /// <param name="arg1">A rectangle that defines the area of the source image to use as input.</param>
        /// <param name="arg2">
        /// The point within the destination image (the current BitmapData
        /// instance) that corresponds to the upper-left corner of the source rectangle.
        /// </param>
        /// <param name="arg3">(default = 0)  The random seed to use to start the pixel dissolve.</param>
        /// <param name="arg4">(default = 0)  The default is 1/30 of the source area (width x height).</param>
        /// <param name="arg5">
        /// (default = 0)  An ARGB color value that you use to fill pixels whose
        /// source value equals its destination value.
        /// </param>
        /// <returns>The new random seed value to use for subsequent calls.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int pixelDissolve(BitmapData arg0, flash.geom.Rectangle arg1, flash.geom.Point arg2, int arg3, int arg4, uint arg5);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int pixelDissolve(BitmapData arg0, flash.geom.Rectangle arg1, flash.geom.Point arg2, int arg3, int arg4);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int pixelDissolve(BitmapData arg0, flash.geom.Rectangle arg1, flash.geom.Point arg2, int arg3);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int pixelDissolve(BitmapData arg0, flash.geom.Rectangle arg1, flash.geom.Point arg2);

        /// <summary>Fills an image with pixels representing random noise.</summary>
        /// <param name="arg0">
        /// The random seed number to use. If you keep all other parameters
        /// the same, you can generate different pseudo-random results by varying the random seed value. The noise
        /// function is a mapping function, not a true random-number generation function, so it creates the same
        /// results each time from the same random seed.
        /// </param>
        /// <param name="arg1">(default = 0)  The lowest value to generate for each channel (0 to 255).</param>
        /// <param name="arg2">(default = 255)  The highest value to generate for each channel (0 to 255).</param>
        /// <param name="arg3">
        /// (default = 7)  A number that can be a combination of any of
        /// the four color channel values (BitmapDataChannel.RED,
        /// BitmapDataChannel.BLUE, BitmapDataChannel.GREEN, and
        /// BitmapDataChannel.ALPHA). You can use the logical OR
        /// operator (|) to combine channel values.
        /// </param>
        /// <param name="arg4">
        /// (default = false)  A Boolean value. If the value is true, a grayscale image is created by setting
        /// all of the color channels to the same value.
        /// The alpha channel selection is not affected by
        /// setting this parameter to true.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void noise(int arg0, uint arg1, uint arg2, uint arg3, bool arg4);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void noise(int arg0, uint arg1, uint arg2, uint arg3);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void noise(int arg0, uint arg1, uint arg2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void noise(int arg0, uint arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void noise(int arg0);

        /// <summary>
        /// Returns a new BitmapData object that is a clone of the original instance
        /// with an exact copy of the contained bitmap.
        /// </summary>
        /// <returns>A new BitmapData object that is identical to the original.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual BitmapData clone();

        /// <summary>
        /// Frees memory that is used to store the BitmapData object.
        /// When the dispose() method is called on an image, the width and height of the image are set to 0.
        /// All subsequent calls to methods or properties of this BitmapData instance fail, and an
        /// exception is thrown.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void dispose();

        /// <summary>
        /// Performs a flood fill operation on an image starting
        /// at an (x, y) coordinate and filling with a certain color. The
        /// floodFill() method is similar to the paint bucket tool in various paint
        /// programs. The color is an ARGB color that contains alpha information and
        /// color information.
        /// </summary>
        /// <param name="arg0">The x coordinate of the image.</param>
        /// <param name="arg1">The y coordinate of the image.</param>
        /// <param name="arg2">The ARGB color to use as a fill.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void floodFill(int arg0, int arg1, uint arg2);

        /// <summary>
        /// Sets the color and alpha transparency values of a single pixel of a BitmapData
        /// object. This method is similar to the setPixel() method; the main difference is
        /// that the setPixel32() method takes an
        /// ARGB color value that contains alpha channel information.
        /// All pixels in a BitmapData object are stored as premultiplied color values.
        /// A premultiplied image pixel has the red, green, and blue
        /// color channel values already multiplied by the alpha data. For example, if the
        /// alpha value is 0, the values for the RGB channels are also 0, independent of their unmultiplied
        /// values. This loss of data can cause some problems when you perform operations. All BitmapData
        /// methods take and return unmultiplied values. The internal pixel representation is converted
        /// from premultiplied to unmultiplied before it is returned as a value. During a set operation,
        /// the pixel value is premultiplied before the raw image pixel is set.Note: To increase performance, when you use the setPixel() or
        /// setPixel32() method repeatedly, call the lock() method before
        /// you call the setPixel() or setPixel32() method, and then call
        /// the unlock() method when you have made all pixel changes. This process prevents objects
        /// that reference this BitmapData instance from updating until you finish making
        /// the pixel changes.
        /// </summary>
        /// <param name="arg0">The x position of the pixel whose value changes.</param>
        /// <param name="arg1">The y position of the pixel whose value changes.</param>
        /// <param name="arg2">
        /// The resulting ARGB color for the pixel. If the bitmap is opaque
        /// (not transparent), the alpha transparency portion of this color value is ignored.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setPixel32(int arg0, int arg1, uint arg2);

        /// <summary>
        /// Compares two BitmapData objects. If the two BitmapData objects have the same dimensions
        /// (width and height), the method returns a new BitmapData object, in which each pixel is
        /// the &quot;difference&quot; between the pixels in the two source objects:
        /// If two pixels are equal, the difference pixel is 0x00000000. If two pixels have different RGB values (ignoring the alpha value), the difference pixel
        /// is 0xFFRRGGBB where RR/GG/BB are the individual difference values between red, green, and blue
        /// channels. Alpha channel differences are ignored in this case. If only the alpha channel value is different, the pixel value is 0xZZFFFFFF,
        /// where ZZ is the difference in the alpha value.For example, consider the following two BitmapData objects:
        /// var bmd1:BitmapData = new BitmapData(50, 50, true, 0xFFFF0000);
        /// var bmd2:BitmapData = new BitmapData(50, 50, true, 0xCCFFAA00);
        /// var diffBmpData:BitmapData = bmd1.compare(bmd2);
        /// Note: The colors used to fill the two BitmapData objects have slightly different RGB values
        /// (0xFF0000 and 0xFFAA00). The result of the compare() method is a new BitmapData
        /// object with each pixel showing the difference in the RGB values between the two bitmaps.Consider the following two BitmapData objects, in which the RGB colors are the same,
        /// but the alpha values are different:
        /// var bmd1:BitmapData = new BitmapData(50, 50, true, 0xFFFFAA00);
        /// var bmd2:BitmapData = new BitmapData(50, 50, true, 0xCCFFAA00);
        /// var diffBmpData:BitmapData = bmd1.compare(bmd2);
        /// The result of the compare() method is a new BitmapData
        /// object with each pixel showing the difference in the alpha values between the two bitmaps.If the BitmapData objects are equivalent (with the same width, height, and identical pixel values),
        /// the method returns the number 0. If the widths of the BitmapData objects are not equal, but the heights are the same,
        /// the method returns the number -3. If the heights of the BitmapData objects are not equal, but the widths are the same,
        /// the method returns the number -4. The following example compares two Bitmap objects with different widths (50 and 60):
        /// var bmd1:BitmapData = new BitmapData(100, 50, false, 0xFFFF0000);
        /// var bmd2:BitmapData = new BitmapData(100, 60, false, 0xFFFFAA00);
        /// trace(bmd1.compare(bmd2)); // -4
        /// </summary>
        /// <param name="arg0">The BitmapData object to compare with the source BitmapData object.</param>
        /// <returns>
        /// If the two BitmapData objects have the same dimensions (width and height), the
        /// method returns a new BitmapData object that has the difference between the two objects (see the
        /// main discussion). If the BitmapData objects are equivalent, the method returns the number 0.
        /// If the widths of the BitmapData objects are not equal, the method returns the number -3.
        /// If the heights of the BitmapData objects are not equal, the method returns the number -4.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object compare(BitmapData arg0);

        /// <summary>
        /// Generates a Perlin noise image.
        /// The Perlin noise generation algorithm interpolates and combines individual random noise functions (called octaves)
        /// into a single function that generates more natural-seeming random noise. Like musical octaves, each octave function is twice the
        /// frequency of the one before it. Perlin noise has been described as a &quot;fractal sum of noise&quot; because it combines multiple sets of noise data
        /// with different levels of detail.You can use Perlin noise functions to simulate natural
        /// phenomena and landscapes, such as wood grain, clouds, and mountain ranges. In most cases, the output of a Perlin noise function
        /// is not displayed directly but is used to enhance other images and give them pseudo-random variations.Simple digital random noise functions often produce images with harsh, contrasting points. This kind of harsh contrast
        /// is not often found in nature. The Perlin noise algorithm blends multiple noise functions that operate at different levels of detail.
        /// This algorithm results in smaller variations among neighboring pixel values.Note: The Perlin noise algorithm is named for Ken Perlin, who developed it after generating computer graphics for the 1982 film Tron.
        /// Perlin received an Academy Award for Technical Achievement for the Perlin noise function in 1997.
        /// </summary>
        /// <param name="arg0">
        /// Frequency to use in the x direction. For example, to generate a noise that
        /// is sized for a 64 x 128 image, pass 64 for the baseX value.
        /// </param>
        /// <param name="arg1">
        /// Frequency to use in the y direction. For example, to generate a noise that
        /// is sized for a 64 x 128 image, pass 128 for the baseY value.
        /// </param>
        /// <param name="arg2">
        /// Number of octaves or individual noise functions to combine to create this noise. Larger numbers of octaves create
        /// images with greater detail. Larger numbers of octaves also require more processing time.
        /// </param>
        /// <param name="arg3">
        /// The random seed number to use. If you keep all other parameters the same, you can generate different
        /// pseudo-random results by varying the random seed value. The Perlin noise function is a mapping function, not a
        /// true random-number generation function, so it creates the same results each time from the same random seed.
        /// </param>
        /// <param name="arg4">
        /// A Boolean value. If the value is true, the method attempts to smooth the transition edges of the image to create seamless textures for
        /// tiling as a bitmap fill.
        /// </param>
        /// <param name="arg5">
        /// A Boolean value. If the value is true, the method generates fractal noise; otherwise,
        /// it generates turbulence. An image with turbulence has visible discontinuities in the gradient
        /// that can make it better approximate sharper visual effects like flames and ocean waves.
        /// </param>
        /// <param name="arg6">
        /// (default = 7)   A number that can be a combination of any of
        /// the four color channel values (BitmapDataChannel.RED,
        /// BitmapDataChannel.BLUE, BitmapDataChannel.GREEN, and
        /// BitmapDataChannel.ALPHA). You can use the logical OR
        /// operator (|) to combine channel values.
        /// </param>
        /// <param name="arg7">
        /// (default = false)  A Boolean value. If the value is true, a grayscale image is created by setting
        /// each of the red, green, and blue color channels to
        /// identical values. The alpha channel value is not affected if this value is
        /// set to true.
        /// </param>
        /// <param name="arg8">
        /// (default = null)  An array of points that correspond to x and y offsets for each octave.
        /// By manipulating the offset values you can smoothly scroll the layers of a perlinNoise image.
        /// Each point in the offset array affects a specific octave noise function.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void perlinNoise(double arg0, double arg1, uint arg2, int arg3, bool arg4, bool arg5, uint arg6, bool arg7, Avm.Array arg8);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void perlinNoise(double arg0, double arg1, uint arg2, int arg3, bool arg4, bool arg5, uint arg6, bool arg7);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void perlinNoise(double arg0, double arg1, uint arg2, int arg3, bool arg4, bool arg5, uint arg6);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void perlinNoise(double arg0, double arg1, uint arg2, int arg3, bool arg4, bool arg5);

        /// <summary>
        /// Remaps the color channel values in an image that has up to four arrays of color palette data, one
        /// for each channel.
        /// Flash Player uses the following steps to generate the resulting image:After the red, green, blue, and alpha
        /// values are computed, they are added together using standard 32-bit-integer arithmetic. The red, green, blue, and alpha channel values of each pixel are extracted into separate 0 to 255 values.
        /// These values are used to look up new color values in the appropriate array: redArray,
        /// greenArray, blueArray, and alphaArray.
        /// Each of these four arrays should contain 256 values. After all four of the new channel values are retrieved, they are combined into a standard
        /// ARGB value that is applied to the pixel.Cross-channel effects can be supported with this method.
        /// Each input array can contain full 32-bit values, and no shifting occurs when the
        /// values are added together. This routine does not support per-channel
        /// clamping. If no array is specified for a channel,
        /// the color channel is copied from the source image to the
        /// destination image.You can use this method for a variety of effects such as
        /// general palette mapping (taking one channel and converting it
        /// to a false color image). You can also use this method for a variety of advanced color
        /// manipulation algorithms, such as gamma, curves, levels, and quantizing.
        /// </summary>
        /// <param name="arg0">
        /// The input bitmap image to use. The source image can be a different
        /// BitmapData object, or it can refer to the current BitmapData instance.
        /// </param>
        /// <param name="arg1">A rectangle that defines the area of the source image to use as input.</param>
        /// <param name="arg2">
        /// The point within the destination image (the current BitmapData
        /// object) that corresponds to the upper-left corner of the source rectangle.
        /// </param>
        /// <param name="arg3">
        /// (default = null)  If redArray is not null, red = redArray[source red value]
        /// else red = source rect value.
        /// </param>
        /// <param name="arg4">
        /// (default = null)  If greenArray is not null, green = greenArray[source
        /// green value] else green = source green value.
        /// </param>
        /// <param name="arg5">
        /// (default = null)  If blueArray is not null, blue = blueArray[source blue
        /// value] else blue = source blue value.
        /// </param>
        /// <param name="arg6">
        /// (default = null)  If alphaArray is not null, alpha = alphaArray[source
        /// alpha value] else alpha = source alpha value.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void paletteMap(BitmapData arg0, flash.geom.Rectangle arg1, flash.geom.Point arg2, Avm.Array arg3, Avm.Array arg4, Avm.Array arg5, Avm.Array arg6);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void paletteMap(BitmapData arg0, flash.geom.Rectangle arg1, flash.geom.Point arg2, Avm.Array arg3, Avm.Array arg4, Avm.Array arg5);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void paletteMap(BitmapData arg0, flash.geom.Rectangle arg1, flash.geom.Point arg2, Avm.Array arg3, Avm.Array arg4);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void paletteMap(BitmapData arg0, flash.geom.Rectangle arg1, flash.geom.Point arg2, Avm.Array arg3);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void paletteMap(BitmapData arg0, flash.geom.Rectangle arg1, flash.geom.Point arg2);

        /// <summary>
        /// Generates a byte array from a rectangular region of pixel data.
        /// Writes an unsigned integer (a 32-bit unmultiplied pixel value)
        /// for each pixel into the byte array.
        /// </summary>
        /// <param name="arg0">A rectangular area in the current BitmapData object.</param>
        /// <returns>A ByteArray representing the pixels in the given Rectangle.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.utils.ByteArray getPixels(flash.geom.Rectangle arg0);

        /// <summary>
        /// Tests pixel values in an image against a specified threshold and sets pixels that pass the test to new color values.
        /// Using the threshold() method, you can isolate and replace color ranges in an image and perform other
        /// logical operations on image pixels.
        /// The threshold() method&apos;s test logic is as follows:If ((pixelValue &amp; mask) operation (threshold &amp; mask)), then
        /// set the pixel to color;Otherwise, if copySource == true, then
        /// set the pixel to corresponding pixel value from sourceBitmap.The operation parameter specifies the comparison operator to use for the threshold test.
        /// For example, by using &quot;==&quot; as the operation parameter, you
        /// can isolate a specific color value in an image. Or by using {operation:
        /// &quot;&lt;&quot;, mask: 0xFF000000, threshold: 0x7F000000, color:
        /// 0x00000000}, you can set all destination pixels to be fully transparent
        /// when the source image pixel&apos;s alpha is less than 0x7F. You can use this technique
        /// for animated transitions and other effects.
        /// </summary>
        /// <param name="arg0">
        /// The input bitmap image to use. The source image can be a different
        /// BitmapData object or it can refer to the current BitmapData instance.
        /// </param>
        /// <param name="arg1">A rectangle that defines the area of the source image to use as input.</param>
        /// <param name="arg2">
        /// The point within the destination image (the current BitmapData
        /// instance) that corresponds to the upper-left corner of the source rectangle.
        /// </param>
        /// <param name="arg3">One of the following comparison operators, passed as a String: &quot;&lt;&quot;, &quot;&lt;=&quot;, &quot;&gt;&quot;, &quot;&gt;=&quot;, &quot;==&quot;, &quot;!=&quot;</param>
        /// <param name="arg4">The value that each pixel is tested against to see if it meets or exceeds the threshhold.</param>
        /// <param name="arg5">(default = 0)  The color value that a pixel is set to if the threshold test succeeds. The default value is 0x00000000.</param>
        /// <param name="arg6">(default = 0xFFFFFFFF)  The mask to use to isolate a color component.</param>
        /// <param name="arg7">
        /// (default = false)  If the value is true, pixel values from the source image are copied to the destination
        /// when the threshold test fails. If the value is false, the source image is not copied when the
        /// threshold test fails.
        /// </param>
        /// <returns>The number of pixels that were changed.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint threshold(BitmapData arg0, flash.geom.Rectangle arg1, flash.geom.Point arg2, Avm.String arg3, uint arg4, uint arg5, uint arg6, bool arg7);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint threshold(BitmapData arg0, flash.geom.Rectangle arg1, flash.geom.Point arg2, Avm.String arg3, uint arg4, uint arg5, uint arg6);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint threshold(BitmapData arg0, flash.geom.Rectangle arg1, flash.geom.Point arg2, Avm.String arg3, uint arg4, uint arg5);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint threshold(BitmapData arg0, flash.geom.Rectangle arg1, flash.geom.Point arg2, Avm.String arg3, uint arg4);

        /// <summary>
        /// Returns an ARGB color value that contains alpha channel data and RGB
        /// data. This method is similar to the getPixel() method, which returns an
        /// RGB color without alpha channel data.
        /// All pixels in a BitmapData object are stored as premultiplied color values.
        /// A premultiplied image pixel has the red, green, and blue
        /// color channel values already multiplied by the alpha data. For example, if the
        /// alpha value is 0, the values for the RGB channels are also 0, independent of their unmultiplied
        /// values. This loss of data can cause some problems when you perform operations. All BitmapData
        /// methods take and return unmultiplied values. The internal pixel representation is converted
        /// from premultiplied to unmultiplied before it is returned as a value. During a set operation,
        /// the pixel value is premultiplied before the raw image pixel is set.
        /// </summary>
        /// <param name="arg0">The x position of the pixel.</param>
        /// <param name="arg1">The y position of the pixel.</param>
        /// <returns>
        /// A number representing an ARGB pixel value. If the (x, y) coordinates are
        /// outside the bounds of the image, 0 is returned.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint getPixel32(int arg0, int arg1);

        /// <summary>
        /// Locks an image so that any objects that reference the BitmapData object, such as Bitmap objects,
        /// are not updated when this BitmapData object changes. To improve performance, use this method
        /// along with the unlock() method before and after numerous calls to the
        /// setPixel() or setPixel32() method.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void @lock();

        /// <summary>
        /// Converts a byte array into a rectangular region of pixel data. For each
        /// pixel, the ByteArray.readUnsignedInt() method is called and the return value is
        /// written into the pixel.  If the byte array ends before the full rectangle
        /// is written, the function returns.  The data in the byte array is
        /// expected to be 32-bit ARGB pixel values. No seeking is performed
        /// on the byte array before or after the pixels are read.
        /// </summary>
        /// <param name="arg0">Specifies the rectangular region of the BitmapData object.</param>
        /// <param name="arg1">
        /// A ByteArray object that consists of 32-bit unmultiplied pixel values
        /// to be used in the rectangular region.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setPixels(flash.geom.Rectangle arg0, flash.utils.ByteArray arg1);

        /// <summary>
        /// Performs per-channel blending from a source image to a
        /// destination image. The following formula is used for each
        /// channel:
        /// new red dest = (red source * redMultiplier) + (red dest * (256 - redMultiplier) / 256;
        /// The redMultiplier, greenMultiplier, blueMultiplier, and alphaMultiplier values
        /// are the multipliers used for each color channel. Their valid range is from 0 to 256.
        /// </summary>
        /// <param name="arg0">
        /// The input bitmap image to use. The source image can be a different
        /// BitmapData object, or it can refer to the current BitmapData object.
        /// </param>
        /// <param name="arg1">A rectangle that defines the area of the source image to use as input.</param>
        /// <param name="arg2">
        /// The point within the destination image (the current BitmapData
        /// instance) that corresponds to the upper-left corner of the source rectangle.
        /// </param>
        /// <param name="arg3">A number by which to multiply the red channel value.</param>
        /// <param name="arg4">A number by which to multiply the green channel value.</param>
        /// <param name="arg5">A number by which to multiply the blue channel value.</param>
        /// <param name="arg6">A number by which to multiply the alpha transparency value.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void merge(BitmapData arg0, flash.geom.Rectangle arg1, flash.geom.Point arg2, uint arg3, uint arg4, uint arg5, uint arg6);

        [PageFX.ABC]
        [PageFX.QName("getVector", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Vector<uint> getVector(flash.geom.Rectangle arg0);

        [PageFX.ABC]
        [PageFX.QName("histogram", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Vector<Avm.Vector<double>> histogram(flash.geom.Rectangle arg0);

        [PageFX.ABC]
        [PageFX.QName("histogram", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Vector<Avm.Vector<double>> histogram();
    }
}
