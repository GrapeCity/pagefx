using System;
using System.Runtime.CompilerServices;

namespace flash.geom
{
    /// <summary>
    /// The Matrix class represents a transformation matrix that determines how to map points from one
    /// coordinate space to another.  You can perform various graphical
    /// transformations on a display object by setting the properties of a Matrix object,
    /// applying that Matrix object to the matrix  property of a Transform object,
    /// and then applying that Transform object as the transform  property of the display object.
    /// These transformation functions include translation
    /// ( x  and y  repositioning), rotation, scaling, and skewing.
    /// </summary>
    [PageFX.AbcInstance(74)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class Matrix : Avm.Object
    {
        /// <summary>
        /// The value in the first row and first column of the Matrix object, which affects the positioning of pixels
        /// along the x axis when scaling or rotating an image.
        /// </summary>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double a;

        /// <summary>
        /// The value in the first row and second column of the Matrix object, which affects the positioning of pixels
        /// along the y axis when rotating or skewing an image.
        /// </summary>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double b;

        /// <summary>
        /// The value in the second row and first column of the Matrix object, which affects the positioning of pixels
        /// along the x axis when rotating or skewing an image.
        /// </summary>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double c;

        /// <summary>
        /// The value in the second row and second column of the Matrix object, which affects the positioning of pixels
        /// along the y axis when scaling or rotating an image.
        /// </summary>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double d;

        /// <summary>
        /// The distance by which to translate each point along the x axis.
        /// This represents the value in the first row and third column of the Matrix object.
        /// </summary>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double tx;

        /// <summary>
        /// The distance by which to translate each point along the y axis.
        /// This represents the value in the second row and third column of the Matrix object.
        /// </summary>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double ty;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Matrix(double a, double b, double c, double d, double tx, double ty);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Matrix(double a, double b, double c, double d, double tx);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Matrix(double a, double b, double c, double d);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Matrix(double a, double b, double c);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Matrix(double a, double b);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Matrix(double a);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Matrix();

        /// <summary>
        /// Returns a new Matrix object that is a clone of this
        /// matrix, with an exact copy of the contained object.
        /// </summary>
        /// <returns>A Matrix object.</returns>
        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Matrix clone();

        /// <summary>
        /// Concatenates a matrix with the current matrix, effectively combining the
        /// geometric effects of the two. In mathematical terms, concatenating two matrixes
        /// is the same as combining them using matrix multiplication.
        /// For example, if matrix m1 scales an object by a factor of four, and
        /// matrix m2 rotates an object by 1.5707963267949 radians
        /// (Math.PI/2), then m1.concat(m2) transforms m1
        /// into a matrix that scales an object by a factor of four and rotates the object by
        /// Math.PI/2 radians. This method replaces the source matrix with the concatenated matrix. If you
        /// want to concatenate two matrixes without altering either of the two source matrixes,
        /// first copy the source matrix by using the clone() method, as shown in the Class Examples section.
        /// </summary>
        /// <param name="m">The matrix to be concatenated to the source matrix.</param>
        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void concat(flash.geom.Matrix m);

        /// <summary>
        /// Performs the opposite transformation
        /// of the original matrix. You can apply an inverted matrix to an object to undo the transformation
        /// performed when applying the original matrix.
        /// </summary>
        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void invert();

        /// <summary>
        /// Sets each matrix property to a value that causes a null transformation. An object transformed
        /// by applying an identity matrix will be identical to the original.
        /// After calling the identity() method, the resulting matrix has the following properties:
        /// a=1, b=0, c=0, d=1, tx=0, ty=0.In matrix notation, the identity matrix looks like this:
        /// </summary>
        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void identity();

        /// <summary>
        /// Includes parameters for scaling,
        /// rotation, and translation. When applied to a matrix it sets the matrix&apos;s values
        /// based on those parameters.
        /// Using the createBox() method lets you obtain the same matrix as you would if
        /// you applied the identity(), rotate(), scale(), and translate() methods
        /// in succession. For example, mat1.createBox(2,2,Math.PI/4, 100, 100) has the
        /// same effect as the following:
        /// import flash.geom.Matrix;
        /// var mat1:Matrix = new Matrix();
        /// mat1.identity();
        /// mat1.rotate(Math.PI/4);
        /// mat1.scale(2,2);
        /// mat1.translate(10,20);
        /// </summary>
        /// <param name="scaleX">The factor by which to scale horizontally.</param>
        /// <param name="scaleY">The factor by which scale vertically.</param>
        /// <param name="rotation">(default = 0)  The amount to rotate, in radians.</param>
        /// <param name="tx">(default = 0)  The number of pixels to translate (move) to the right along the x axis.</param>
        /// <param name="ty">(default = 0)  The number of pixels to translate (move) down along the y axis.</param>
        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void createBox(double scaleX, double scaleY, double rotation, double tx, double ty);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void createBox(double scaleX, double scaleY, double rotation, double tx);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void createBox(double scaleX, double scaleY, double rotation);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void createBox(double scaleX, double scaleY);

        /// <summary>
        /// Creates the specific style of matrix expected by the beginGradientFill() and
        /// lineGradientStyle() methods of the Graphics class. Width and height are scaled to
        /// a scaleX/scaleY pair and the tx/ty
        /// values are offset by half the width and height.
        /// For example, consider a gradient with the following characteristics:GradientType.LINEARTwo colors, green and blue, with the ratios array set to [0, 255]SpreadMethod.PADInterpolationMethod.LINEAR_RGBThe following illustrations show gradients in which the matrix was defined using the
        /// createGradientBox() method with different parameter settings:createGradientBox() settingsResulting gradientwidth = 25;
        /// height = 25;
        /// rotation = 0;
        /// tx = 0;
        /// ty = 0;width = 25;
        /// height = 25;
        /// rotation = 0;
        /// tx = 25;
        /// ty = 0;width = 50;
        /// height = 50;
        /// rotation = 0;
        /// tx = 0;
        /// ty = 0;width = 50;
        /// height = 50;
        /// rotation = Math.PI / 4; // 45°
        /// tx = 0;
        /// ty = 0;
        /// </summary>
        /// <param name="width">The width of the gradient box.</param>
        /// <param name="height">The height of the gradient box.</param>
        /// <param name="rotation">(default = 0)  The amount to rotate, in radians.</param>
        /// <param name="tx">
        /// (default = 0)  The distance, in pixels, to translate to the right along the x axis.
        /// This value is offset by half of the width parameter.
        /// </param>
        /// <param name="ty">
        /// (default = 0)  The distance, in pixels, to translate down along the y axis.
        /// This value is offset by half of the height parameter.
        /// </param>
        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void createGradientBox(double width, double height, double rotation, double tx, double ty);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void createGradientBox(double width, double height, double rotation, double tx);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void createGradientBox(double width, double height, double rotation);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void createGradientBox(double width, double height);

        /// <summary>
        /// Applies a rotation transformation to the Matrix object.
        /// The rotate() method alters the a, b, c,
        /// and d properties of the Matrix object.
        /// In matrix notation, this is the same as concatenating the current matrix with the following:
        /// </summary>
        /// <param name="angle">The rotation angle in radians.</param>
        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void rotate(double angle);

        /// <summary>
        /// Translates the matrix along the x and y axes, as specified by the dx
        /// and dy parameters.
        /// </summary>
        /// <param name="dx">The amount of movement along the x axis to the right, in pixels.</param>
        /// <param name="dy">The amount of movement down along the y axis, in pixels.</param>
        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void translate(double dx, double dy);

        /// <summary>
        /// Applies a scaling transformation to the matrix. The x axis is multiplied
        /// by sx, and the y axis it is multiplied by sy.
        /// The scale() method alters the a and d properties of
        /// the Matrix object.
        /// In matrix notation, this is the same as concatenating the current matrix with the following matrix:
        /// </summary>
        /// <param name="sx">A multiplier used to scale the object along the x axis.</param>
        /// <param name="sy">A multiplier used to scale the object along the y axis.</param>
        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void scale(double sx, double sy);

        /// <summary>
        /// Given a point in the pretransform coordinate space, returns the coordinates of
        /// that point after the transformation occurs. Unlike the standard transformation applied using
        /// the transformPoint() method, the deltaTransformPoint() method&apos;s
        /// transformation does not consider the translation parameters tx and ty.
        /// </summary>
        /// <param name="point">The point for which you want to get the result of the matrix transformation.</param>
        /// <returns>The point resulting from applying the matrix transformation.</returns>
        [PageFX.AbcInstanceTrait(15)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Point deltaTransformPoint(flash.geom.Point point);

        /// <summary>
        /// Returns the result of applying the geometric transformation represented by the Matrix object to the
        /// specified point.
        /// </summary>
        /// <param name="point">The point for which you want to get the result of the Matrix transformation.</param>
        /// <returns>The point resulting from applying the Matrix transformation.</returns>
        [PageFX.AbcInstanceTrait(16)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Point transformPoint(flash.geom.Point point);

        /// <summary>Returns a text value listing the properties of the Matrix object.</summary>
        /// <returns>
        /// A string containing the values of the properties of the Matrix object: a, b, c,
        /// d, tx, and ty.
        /// </returns>
        [PageFX.AbcInstanceTrait(17)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();
    }
}
