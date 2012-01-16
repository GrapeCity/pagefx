using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The Graphics class contains a set of methods that you can use to create a vector shape.
    /// Display objects that support drawing include Sprite and Shape objects.
    /// Each of these classes includes a graphics  property that is a Graphics object.
    /// The following are among those helper functions provided for ease of use:
    /// drawRect() , drawRoundRect() ,
    /// drawCircle() , and drawEllipse() .
    /// </summary>
    [PageFX.AbcInstance(245)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class Graphics : Avm.Object
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Graphics();

        /// <summary>
        /// Clears the graphics that were drawn to this Graphics object, and resets fill and
        /// line style settings.
        /// </summary>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void clear();

        /// <summary>
        /// Specifies a simple one-color fill that Flash Player uses for subsequent calls to other
        /// Graphics methods (such as lineTo() or drawCircle()) for the object.
        /// The fill remains in effect until you call the beginFill(),
        /// beginBitmapFill(), or beginGradientFill() method.
        /// Calling the clear() method clears the fill.
        /// Flash Player does not render the fill until the endFill() method is
        /// called.
        /// </summary>
        /// <param name="color">The color of the fill (0xRRGGBB).</param>
        /// <param name="alpha">(default = 1.0)  The alpha value of the fill (0.0 to 1.0).</param>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void beginFill(uint color, double alpha);

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void beginFill(uint color);

        /// <summary>
        /// Specifies a gradient fill that Flash Player uses for subsequent calls to other
        /// Graphics methods (such as lineTo() or drawCircle()) for the object.
        /// The fill remains in effect until you call the beginFill(),
        /// beginBitmapFill(), or beginGradientFill() method.
        /// Calling the clear() method clears the fill.
        /// Flash Player does not render the fill until the endFill() method is
        /// called.
        /// </summary>
        /// <param name="type">
        /// A value from the GradientType class that
        /// specifies which gradient type to use: GradientType.LINEAR or
        /// GradientType.RADIAL.
        /// </param>
        /// <param name="colors">
        /// An array of RGB hexadecimal color values to be used in the gradient; for example,
        /// red is 0xFF0000, blue is 0x0000FF, and so on. You can specify up to 15 colors.
        /// For each color, be sure you specify a corresponding value in the alphas and ratios parameters.
        /// </param>
        /// <param name="alphas">
        /// An array of alpha values for the corresponding colors in the colors array;
        /// valid values are 0 to 1. If the value is less than 0, the default is 0. If the value is
        /// greater than 1, the default is 1.
        /// </param>
        /// <param name="ratios">
        /// An array of color distribution ratios; valid values are 0 to 255. This value
        /// defines the percentage of the width where the color is sampled at 100%. The value 0 represents
        /// the left-hand position in the gradient box, and 255 represents the right-hand position in the
        /// gradient box.
        /// Note: This value represents positions in the gradient box, not the
        /// coordinate space of the final gradient, which might be wider or thinner than the gradient box.
        /// Specify a value for each value in the colors parameter. For example, for a linear gradient that includes two colors, blue and green, the
        /// following example illustrates the placement of the colors in the gradient based on different values
        /// in the ratios array:ratiosGradient[0, 127][0, 255][127, 255]The values in the array must increase sequentially; for example,
        /// [0, 63, 127, 190, 255].
        /// </param>
        /// <param name="matrix">
        /// (default = null)  A transformation matrix as defined by the
        /// flash.geom.Matrix class. The flash.geom.Matrix class includes a
        /// createGradientBox() method, which lets you conveniently set up
        /// the matrix for use with the beginGradientFill() method.
        /// </param>
        /// <param name="spreadMethod">
        /// (default = &quot;pad&quot;)  A value from the SpreadMethod class that
        /// specifies which spread method to use, either: SpreadMethod.PAD,
        /// SpreadMethod.REFLECT, or SpreadMethod.REPEAT.
        /// For example, consider a simple linear gradient between two colors:
        /// import flash.geom.*
        /// import flash.display.*
        /// var fillType:String = GradientType.LINEAR;
        /// var colors:Array = [0xFF0000, 0x0000FF];
        /// var alphas:Array = [100, 100];
        /// var ratios:Array = [0x00, 0xFF];
        /// var matr:Matrix = new Matrix();
        /// matr.createGradientBox(20, 20, 0, 0, 0);
        /// var spreadMethod:String = SpreadMethod.PAD;
        /// this.graphics.beginGradientFill(fillType, colors, alphas, ratios, matr, spreadMethod);
        /// this.graphics.drawRect(0,0,100,100);
        /// This example uses SpreadMethod.PAD for the spread method, and
        /// the gradient fill looks like the following:If you use SpreadMethod.REFLECT for the spread method, the gradient fill
        /// looks like the following:If you use SpreadMethod.REPEAT for the spread method, the gradient fill
        /// looks like the following:
        /// </param>
        /// <param name="interpolationMethod">
        /// (default = &quot;rgb&quot;)  A value from the InterpolationMethod class that
        /// specifies which value to use: InterpolationMethod.linearRGB or
        /// InterpolationMethod.RGBFor example, consider a simple linear gradient between two colors (with the spreadMethod
        /// parameter set to SpreadMethod.REFLECT). The different interpolation methods affect
        /// the appearance as follows: InterpolationMethod.LINEAR_RGBInterpolationMethod.RGB
        /// </param>
        /// <param name="focalPointRatio">
        /// (default = 0)  A number that controls the
        /// location of the focal point of the gradient. 0 means that the focal point is in the center. 1
        /// means that the focal point is at one border of the gradient circle. -1 means that the focal point
        /// is at the other border of the gradient circle. A value less than -1 or greater than
        /// 1 is rounded to -1 or 1. For example, the following example
        /// shows a focalPointRatio set to 0.75:
        /// </param>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void beginGradientFill(Avm.String type, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, flash.geom.Matrix matrix, Avm.String spreadMethod, Avm.String interpolationMethod, double focalPointRatio);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void beginGradientFill(Avm.String type, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, flash.geom.Matrix matrix, Avm.String spreadMethod, Avm.String interpolationMethod);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void beginGradientFill(Avm.String type, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, flash.geom.Matrix matrix, Avm.String spreadMethod);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void beginGradientFill(Avm.String type, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, flash.geom.Matrix matrix);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void beginGradientFill(Avm.String type, Avm.Array colors, Avm.Array alphas, Avm.Array ratios);

        /// <summary>
        /// Fills a drawing area with a bitmap image. The bitmap can be repeated or tiled to fill
        /// the area. The fill remains in effect until you call the beginFill(),
        /// beginBitmapFill(), or beginGradientFill() method.
        /// Calling the clear() method clears the fill.
        /// Flash Player does not render the fill until the endFill() method is
        /// called.
        /// </summary>
        /// <param name="bitmap">A transparent or opaque bitmap image that contains the bits to be displayed.</param>
        /// <param name="matrix">
        /// (default = null)  A matrix object (of the flash.geom.Matrix class), which you can use to
        /// define transformations on the bitmap. For instance, you can use the following matrix
        /// to rotate a bitmap by 45 degrees (pi/4 radians):
        /// matrix = new flash.geom.Matrix();
        /// matrix.rotate(Math.PI/4);
        /// </param>
        /// <param name="repeat">
        /// (default = true)  If true, the bitmap image repeats in a tiled pattern. If
        /// false, the bitmap image does not repeat, and the edges of the bitmap are
        /// used for any fill area that extends beyond the bitmap.
        /// For example, consider the following bitmap (a 20 x 20-pixel checkerboard pattern):When repeat is set to true (as in the following example), the bitmap fill
        /// repeats the bitmap:When repeat is set to false, the bitmap fill uses the edge
        /// pixels for the fill area outside of the bitmap:
        /// </param>
        /// <param name="smooth">
        /// (default = false)  If false, upscaled bitmap images are rendered by using a
        /// nearest-neighbor algorithm and look pixelated. If true, upscaled
        /// bitmap images are rendered by using a bilinear algorithm. Rendering by using the nearest
        /// neighbor algorithm is usually faster.
        /// </param>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void beginBitmapFill(flash.display.BitmapData bitmap, flash.geom.Matrix matrix, bool repeat, bool smooth);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void beginBitmapFill(flash.display.BitmapData bitmap, flash.geom.Matrix matrix, bool repeat);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void beginBitmapFill(flash.display.BitmapData bitmap, flash.geom.Matrix matrix);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void beginBitmapFill(flash.display.BitmapData bitmap);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void beginShaderFill(flash.display.Shader shader, flash.geom.Matrix matrix);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void beginShaderFill(flash.display.Shader shader);

        /// <summary>
        /// Specifies a gradient for the line style that Flash Player uses for subsequent calls to other
        /// Graphics methods (such as lineTo() or drawCircle()) for the object.
        /// The line style remains in effect until you call the lineStyle() method or the
        /// lineGradientStyle() method with different parameters. You can call the
        /// lineGradientStyle() method in the middle of drawing a path to specify different
        /// styles for different line segments within a path.
        /// Call lineStyle() before you call
        /// lineGradientStyle() to enable a stroke, otherwise the value of the line style
        /// remains undefined.Calls to clear() set the line style back to
        /// undefined.
        /// </summary>
        /// <param name="type">
        /// A value from the GradientType class that
        /// specifies which gradient type to use, either GradientType.LINEAR or GradientType.RADIAL.
        /// </param>
        /// <param name="colors">
        /// An array of RGB hex color values to be used in the gradient (for example,
        /// red is 0xFF0000, blue is 0x0000FF, and so on).
        /// </param>
        /// <param name="alphas">
        /// An array of alpha values for the corresponding colors in the
        /// colors array; valid values are 0 to 100. If the value is less than 0,
        /// Flash Player uses 0. If the value is greater than 100, Flash Player uses 100.
        /// </param>
        /// <param name="ratios">
        /// An array of color distribution ratios; valid values are from 0 to 255. This value
        /// defines the percentage of the width where the color is sampled at 100%. The value 0 represents
        /// the left-hand position in the gradient box, and 255 represents the right-hand position in the
        /// gradient box. This value represents positions in the gradient box, not the
        /// coordinate space of the final gradient, which might be wider or thinner than the gradient box.
        /// Specify a value for each value in the colors parameter.
        /// For example, for a linear gradient that includes two colors, blue and green, the
        /// following figure illustrates the placement of the colors in the gradient based on different values
        /// in the ratios array:ratiosGradient[0, 127][0, 255][127, 255]The values in the array must increase, sequentially; for example,
        /// [0, 63, 127, 190, 255].
        /// </param>
        /// <param name="matrix">
        /// (default = null)  A transformation matrix as defined by the
        /// flash.geom.Matrix class. The flash.geom.Matrix class includes a
        /// createGradientBox() method, which lets you conveniently set up
        /// the matrix for use with the lineGradientStyle() method.
        /// </param>
        /// <param name="spreadMethod">
        /// (default = &quot;pad&quot;)  A value from the SpreadMethod class that
        /// specifies which spread method to use:
        /// SpreadMethod.PADSpreadMethod.REFLECTSpreadMethod.REPEAT
        /// </param>
        /// <param name="interpolationMethod">
        /// (default = &quot;rgb&quot;)  A value from the InterpolationMethod class that
        /// specifies which value to use. For example, consider a simple linear gradient between two colors (with the spreadMethod
        /// parameter set to SpreadMethod.REFLECT). The different interpolation methods affect
        /// the appearance as follows:
        /// InterpolationMethod.LINEAR_RGBInterpolationMethod.RGB
        /// </param>
        /// <param name="focalPointRatio">
        /// (default = 0)  A number that controls the location of the focal
        /// point of the gradient. The value 0 means the focal point is in the center. The value 1 means the focal
        /// point is at one border of the gradient circle. The value -1 means that the focal point is
        /// at the other border of the gradient circle. Values less than -1 or greater than 1 are
        /// rounded to -1 or 1. The following image shows a gradient with a
        /// focalPointRatio of -0.75:
        /// </param>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void lineGradientStyle(Avm.String type, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, flash.geom.Matrix matrix, Avm.String spreadMethod, Avm.String interpolationMethod, double focalPointRatio);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void lineGradientStyle(Avm.String type, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, flash.geom.Matrix matrix, Avm.String spreadMethod, Avm.String interpolationMethod);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void lineGradientStyle(Avm.String type, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, flash.geom.Matrix matrix, Avm.String spreadMethod);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void lineGradientStyle(Avm.String type, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, flash.geom.Matrix matrix);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void lineGradientStyle(Avm.String type, Avm.Array colors, Avm.Array alphas, Avm.Array ratios);

        /// <summary>
        /// Specifies a line style that Flash uses for subsequent calls to other
        /// Graphics methods (such as lineTo() or drawCircle()) for the object.
        /// The line style remains in effect until you call the lineGradientStyle()
        /// method or the lineStyle() method with different parameters.
        /// You can call lineStyle() in the middle of drawing a path to specify different
        /// styles for different line segments within the path.
        /// Note: Calls to the clear() method set the line style back to
        /// undefined.
        /// </summary>
        /// <param name="thickness">
        /// An integer that indicates the thickness of the line in
        /// points; valid values are 0 to 255. If a number is not specified, or if the
        /// parameter is undefined, a line is not drawn. If a value of less than 0 is passed,
        /// the default is 0. The value 0 indicates hairline thickness; the maximum thickness
        /// is 255. If a value greater than 255 is passed, the default is 255.
        /// </param>
        /// <param name="color">
        /// (default = 0)  A hexadecimal color value of the line; for example, red is 0xFF0000, blue is
        /// 0x0000FF, and so on. If a value is not indicated, the default is 0x000000 (black). Optional.
        /// </param>
        /// <param name="alpha">
        /// (default = 1.0)  A number that indicates the alpha value of the color of the line;
        /// valid values are 0 to 1. If a value is not indicated, the default is 1 (solid). If
        /// the value is less than 0, the default is 0. If the value is greater than 1, the default is 1.
        /// </param>
        /// <param name="pixelHinting">
        /// (default = false)  A Boolean value that specifies whether to hint strokes
        /// to full pixels. This affects both the position of anchors of a curve and the line stroke size
        /// itself. With pixelHinting set to true, Flash Player hints line widths
        /// to full pixel widths. With pixelHinting set to false, disjoints can
        /// appear for curves and straight lines. For example, the following illustrations show how
        /// Flash Player renders two rounded rectangles that are identical, except that the
        /// pixelHinting parameter used in the lineStyle() method is set
        /// differently (the images are scaled by 200%, to emphasize the difference):
        /// If a value is not supplied, the line does not use pixel hinting.
        /// </param>
        /// <param name="scaleMode">
        /// (default = &quot;normal&quot;)  A value from the LineScaleMode class that
        /// specifies which scale mode to use:
        /// LineScaleMode.NORMALAlways scale the line thickness when the object is scaled
        /// (the default).
        /// LineScaleMode.NONENever scale the line thickness.
        /// LineScaleMode.VERTICALDo not scale the line thickness if the object is scaled vertically
        /// only. For example, consider the following circles, drawn with a one-pixel line, and each with the
        /// scaleMode parameter set to LineScaleMode.VERTICAL. The circle on the left
        /// is scaled vertically only, and the circle on the right is scaled both vertically and horizontally:
        /// LineScaleMode.HORIZONTALDo not scale the line thickness if the object is scaled horizontally
        /// only. For example, consider the following circles, drawn with a one-pixel line, and each with the
        /// scaleMode parameter set to LineScaleMode.HORIZONTAL. The circle on the left
        /// is scaled horizontally only, and the circle on the right is scaled both vertically and horizontally:
        /// </param>
        /// <param name="caps">
        /// (default = null)  A value from the CapsStyle class that specifies the type of caps at the end
        /// of lines. Valid values are: CapsStyle.NONE, CapsStyle.ROUND, and CapsStyle.SQUARE.
        /// If a value is not indicated, Flash uses round caps.
        /// For example, the following illustrations show the different capsStyle
        /// settings. For each setting, the illustration shows a blue line with a thickness of 30 (for
        /// which the capsStyle applies), and a superimposed black line with a thickness of 1
        /// (for which no capsStyle applies):
        /// </param>
        /// <param name="joints">
        /// (default = null)  A value from the JointStyle class that specifies the type of joint appearance
        /// used at angles. Valid
        /// values are: JointStyle.BEVEL, JointStyle.MITER, and JointStyle.ROUND.
        /// If a value is not indicated, Flash uses round joints.
        /// For example, the following illustrations show the different joints
        /// settings. For each setting, the illustration shows an angled blue line with a thickness of
        /// 30 (for which the jointStyle applies), and a superimposed angled black line with a
        /// thickness of 1 (for which no jointStyle applies):
        /// Note: For joints set to JointStyle.MITER,
        /// you can use the miterLimit parameter to limit the length of the miter.
        /// </param>
        /// <param name="miterLimit">
        /// (default = 3)  A number that indicates the limit at which a miter is cut off.
        /// Valid values range from 1 to 255 (and values outside of that range are rounded to 1 or 255).
        /// This value is only used if the jointStyle
        /// is set to &quot;miter&quot;. The
        /// miterLimit value represents the length that a miter can extend beyond the point
        /// at which the lines meet to form a joint. The value expresses a factor of the line
        /// thickness. For example, with a miterLimit factor of 2.5 and a
        /// thickness of 10 pixels, the miter is cut off at 25 pixels.
        /// For example, consider the following angled lines, each drawn with a thickness
        /// of 20, but with miterLimit set to 1, 2, and 4. Superimposed are black reference
        /// lines showing the meeting points of the joints:Notice that a given miterLimit value has a specific maximum angle
        /// for which the miter is cut off. The following table lists some examples:miterLimit value:Angles smaller than this are cut off:1.41490 degrees260 degrees430 degrees815 degrees
        /// </param>
        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void lineStyle(double thickness, uint color, double alpha, bool pixelHinting, Avm.String scaleMode, Avm.String caps, Avm.String joints, double miterLimit);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void lineStyle(double thickness, uint color, double alpha, bool pixelHinting, Avm.String scaleMode, Avm.String caps, Avm.String joints);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void lineStyle(double thickness, uint color, double alpha, bool pixelHinting, Avm.String scaleMode, Avm.String caps);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void lineStyle(double thickness, uint color, double alpha, bool pixelHinting, Avm.String scaleMode);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void lineStyle(double thickness, uint color, double alpha, bool pixelHinting);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void lineStyle(double thickness, uint color, double alpha);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void lineStyle(double thickness, uint color);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void lineStyle(double thickness);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void lineStyle();

        /// <summary>
        /// Draws a rectangle. You must set the line style, fill, or both before
        /// you call the drawRect() method, by calling the linestyle(),
        /// lineGradientStyle(), beginFill(), beginGradientFill(),
        /// or beginBitmapFill() method.
        /// </summary>
        /// <param name="x">
        /// A number indicating the horizontal position relative to the
        /// registration point of the parent display object (in pixels).
        /// </param>
        /// <param name="y">
        /// A number indicating the vertical position relative to the
        /// registration point of the parent display object (in pixels).
        /// </param>
        /// <param name="width">The width of the rectangle (in pixels).</param>
        /// <param name="height">The height of the rectangle (in pixels).</param>
        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void drawRect(double x, double y, double width, double height);

        /// <summary>
        /// Draws a rounded rectangle. You must set the line style, fill, or both before
        /// you call the drawRoundRect() method, by calling the linestyle(),
        /// lineGradientStyle(), beginFill(), beginGradientFill(), or
        /// beginBitmapFill() method.
        /// </summary>
        /// <param name="x">
        /// A number indicating the horizontal position relative to the
        /// registration point of the parent display object (in pixels).
        /// </param>
        /// <param name="y">
        /// A number indicating the vertical position relative to the
        /// registration point of the parent display object (in pixels).
        /// </param>
        /// <param name="width">The width of the round rectangle (in pixels).</param>
        /// <param name="height">The height of the round rectangle (in pixels).</param>
        /// <param name="ellipseWidth">The width of the ellipse used to draw the rounded corners (in pixels).</param>
        /// <param name="ellipseHeight">
        /// The height of the ellipse used to draw the rounded corners (in pixels).
        /// Optional; if no value is specified, the default value matches that provided for the
        /// ellipseWidth parameter.
        /// </param>
        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void drawRoundRect(double x, double y, double width, double height, double ellipseWidth, double ellipseHeight);

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void drawRoundRect(double x, double y, double width, double height, double ellipseWidth);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void drawRoundRectComplex(double x, double y, double width, double height, double topLeftRadius, double topRightRadius, double bottomLeftRadius, double bottomRightRadius);

        /// <summary>
        /// Draws a circle. You must set the line style, fill, or both before
        /// you call the drawCircle() method, by calling the linestyle(),
        /// lineGradientStyle(), beginFill(), beginGradientFill(),
        /// or beginBitmapFill() method.
        /// </summary>
        /// <param name="x">
        /// The x location of the center of the circle relative to the
        /// registration point of the parent display object (in pixels).
        /// </param>
        /// <param name="y">
        /// The y location of the center of the circle relative to the
        /// registration point of the parent display object (in pixels).
        /// </param>
        /// <param name="radius">The radius of the circle (in pixels).</param>
        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void drawCircle(double x, double y, double radius);

        /// <summary>
        /// Draws an ellipse. You must set the line style, fill, or both before
        /// you call the drawEllipse() method, by calling the linestyle(),
        /// lineGradientStyle(), beginFill(), beginGradientFill(),
        /// or beginBitmapFill() method.
        /// </summary>
        /// <param name="x">
        /// A number indicating the horizontal position relative to the
        /// registration point of the parent display object (in pixels).
        /// </param>
        /// <param name="y">
        /// A number indicating the vertical position relative to the
        /// registration point of the parent display object (in pixels).
        /// </param>
        /// <param name="width">The width of the ellipse (in pixels).</param>
        /// <param name="height">The height of the ellipse (in pixels).</param>
        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void drawEllipse(double x, double y, double width, double height);

        /// <summary>
        /// Moves the current drawing position to (x, y). If any of the parameters
        /// are missing, this method fails and the current drawing position is not changed.
        /// </summary>
        /// <param name="x">
        /// A number that indicates the horizontal position relative to the
        /// registration point of the parent display object (in pixels).
        /// </param>
        /// <param name="y">
        /// A number that indicates the vertical position relative to the
        /// registration point of the parent display object (in pixels).
        /// </param>
        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void moveTo(double x, double y);

        /// <summary>
        /// Draws a line using the current line style from the current drawing position to (x, y);
        /// the current drawing position is then set to (x, y).
        /// If the display object in which you are drawing contains content that was created with
        /// the Flash drawing tools, calls to the lineTo() method are drawn underneath the content. If
        /// you call lineTo() before any calls to the moveTo() method, the
        /// default position for the current drawing is (0, 0). If any of the parameters are missing, this
        /// method fails and the current drawing position is not changed.
        /// </summary>
        /// <param name="x">
        /// A number that indicates the horizontal position relative to the
        /// registration point of the parent display object (in pixels).
        /// </param>
        /// <param name="y">
        /// A number that indicates the vertical position relative to the
        /// registration point of the parent display object (in pixels).
        /// </param>
        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void lineTo(double x, double y);

        /// <summary>
        /// Draws a curve using the current line style from the current drawing position
        /// to (anchorX, anchorY) and using the control point that (controlX,
        /// controlY) specifies. The current drawing position is then set to
        /// (anchorX, anchorY). If the movie clip in which you are
        /// drawing contains content created with the Flash drawing tools, calls to the
        /// curveTo() method are drawn underneath this content. If you call the
        /// curveTo() method before any calls to the moveTo() method,
        /// the default of the current drawing position is (0, 0). If any of the parameters are
        /// missing, this method fails and the current drawing position is not changed.
        /// The curve drawn is a quadratic Bezier curve. Quadratic Bezier curves
        /// consist of two anchor points and one control point. The curve interpolates the two anchor
        /// points and curves toward the control point.
        /// </summary>
        /// <param name="controlX">
        /// A number that specifies the horizontal position of the control
        /// point relative to the registration point of the parent display object.
        /// </param>
        /// <param name="controlY">
        /// A number that specifies the vertical position of the control
        /// point relative to the registration point of the parent display object.
        /// </param>
        /// <param name="anchorX">
        /// A number that specifies the horizontal position of the next anchor
        /// point relative to the registration point of the parent display object.
        /// </param>
        /// <param name="anchorY">
        /// A number that specifies the vertical position of the next anchor
        /// point relative to the registration point of the parent display object.
        /// </param>
        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void curveTo(double controlX, double controlY, double anchorX, double anchorY);

        /// <summary>
        /// Applies a fill to the lines and curves that were added since the last call to the
        /// beginFill(), beginGradientFill(), or
        /// beginBitmapFill() method. Flash uses the fill that was specified in the previous
        /// call to the beginFill(), beginGradientFill(), or beginBitmapFill()
        /// method. If the current drawing position does not equal the previous position specified in a
        /// moveTo() method and a fill is defined, the path is closed with a line and then
        /// filled.
        /// </summary>
        [PageFX.AbcInstanceTrait(15)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void endFill();

        [PageFX.AbcInstanceTrait(16)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void copyFrom(flash.display.Graphics sourceGraphics);

        [PageFX.AbcInstanceTrait(17)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void lineBitmapStyle(flash.display.BitmapData bitmap, flash.geom.Matrix matrix, bool repeat, bool smooth);

        [PageFX.AbcInstanceTrait(17)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void lineBitmapStyle(flash.display.BitmapData bitmap, flash.geom.Matrix matrix, bool repeat);

        [PageFX.AbcInstanceTrait(17)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void lineBitmapStyle(flash.display.BitmapData bitmap, flash.geom.Matrix matrix);

        [PageFX.AbcInstanceTrait(17)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void lineBitmapStyle(flash.display.BitmapData bitmap);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void lineShaderStyle(flash.display.Shader shader, flash.geom.Matrix matrix);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void lineShaderStyle(flash.display.Shader shader);

        [PageFX.AbcInstanceTrait(19)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void drawPath(Avm.Vector<Avm.String> commands, Avm.Vector<Avm.String> data, Avm.String winding);

        [PageFX.AbcInstanceTrait(19)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void drawPath(Avm.Vector<Avm.String> commands, Avm.Vector<Avm.String> data);

        [PageFX.AbcInstanceTrait(20)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void drawTriangles(Avm.Vector<Avm.String> vertices, Avm.Vector<Avm.String> indices, Avm.Vector<Avm.String> uvtData, Avm.String culling);

        [PageFX.AbcInstanceTrait(20)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void drawTriangles(Avm.Vector<Avm.String> vertices, Avm.Vector<Avm.String> indices, Avm.Vector<Avm.String> uvtData);

        [PageFX.AbcInstanceTrait(20)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void drawTriangles(Avm.Vector<Avm.String> vertices, Avm.Vector<Avm.String> indices);

        [PageFX.AbcInstanceTrait(20)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void drawTriangles(Avm.Vector<Avm.String> vertices);

        [PageFX.AbcInstanceTrait(24)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
		public extern virtual void drawGraphicsData(Avm.Vector<IGraphicsData> graphicsData);
    }
}
