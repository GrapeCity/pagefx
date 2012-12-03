using System;
using System.Runtime.CompilerServices;

namespace flash.geom
{
    /// <summary>
    /// The Point object represents a location in a two-dimensional coordinate system, where
    /// x  represents the horizontal axis and y  represents the vertical axis.
    /// </summary>
    [PageFX.AbcInstance(99)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class Point : Avm.Object
    {
        /// <summary>The horizontal coordinate of the point. The default value is 0.</summary>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double x;

        /// <summary>The vertical coordinate of the point. The default value is 0.</summary>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double y;

        /// <summary>The length of the line segment from (0,0) to this point.</summary>
        public extern virtual double length
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Point(double x, double y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Point(double x);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Point();

        /// <summary>Creates a copy of this Point object.</summary>
        /// <returns>
        /// The new Point
        /// object.
        /// </returns>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Point clone();

        /// <summary>
        /// Offsets the Point object by the specified amount. The value of dx is
        /// added to the original value of x to create the new x value. The value
        /// of dy is added to the original value of y to create the new
        /// y value.
        /// </summary>
        /// <param name="dx">The amount by which to offset the horizontal coordinate, x.</param>
        /// <param name="dy">The amount by which to offset the vertical coordinate, y.</param>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void offset(double dx, double dy);

        /// <summary>
        /// Determines whether two points are equal. Two points are equal if they have the same
        /// x and y values.
        /// </summary>
        /// <param name="toCompare">The point to be compared.</param>
        /// <returns>
        /// A value of true
        /// if the object is equal to this Point object; false if it is not equal.
        /// </returns>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool equals(flash.geom.Point toCompare);

        /// <summary>
        /// Subtracts the coordinates of another point from the coordinates of this point to
        /// create a new point.
        /// </summary>
        /// <param name="v">The point to be subtracted.</param>
        /// <returns>The new point.</returns>
        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Point subtract(flash.geom.Point v);

        /// <summary>
        /// Adds the coordinates of another point to the coordinates of this point to create
        /// a new point.
        /// </summary>
        /// <param name="v">The point to be added.</param>
        /// <returns>The new point.</returns>
        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Point add(flash.geom.Point v);

        /// <summary>Scales the line segment between (0,0) and the current point to a set length.</summary>
        /// <param name="thickness">
        /// The scaling value. For example, if the current point is (0,5), and you normalize
        /// it to 1, the point returned is at (0,1).
        /// </param>
        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void normalize(double thickness);

        /// <summary>
        /// Returns a string that contains the values of the x and y coordinates.
        /// The string has the form &quot;(x=x, y=y)&quot;, so calling the
        /// toString() method for a point at 23,17 would return &quot;(x=23, y=17)&quot;.
        /// </summary>
        /// <returns>
        /// The string representation
        /// of the coordinates.
        /// </returns>
        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();

        /// <summary>
        /// Determines a point between two specified points. The parameter f determines
        /// where the new interpolated point is located relative to the two end points specified
        /// by parameters pt1 and pt2. The closer the value of the
        /// parameter f is to 1.0, the closer the interpolated point
        /// is to the first point (parameter pt1). The closer the value of the
        /// parameter f is to 0, the closer the interpolated point is to the second
        /// point (parameter pt2).
        /// </summary>
        /// <param name="pt1">The first point.</param>
        /// <param name="pt2">The second point.</param>
        /// <param name="f">
        /// The level of interpolation between the two points. Indicates where the new
        /// point will be, along the line between pt1 and pt2. If
        /// f=1, pt1 is returned; if f=0, pt2
        /// is returned.
        /// </param>
        /// <returns>
        /// The new, interpolated
        /// point.
        /// </returns>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.geom.Point interpolate(flash.geom.Point pt1, flash.geom.Point pt2, double f);

        /// <summary>Returns the distance between pt1 and pt2.</summary>
        /// <param name="pt1">The first point.</param>
        /// <param name="pt2">The second point.</param>
        /// <returns>
        /// The distance between
        /// the first and second points.
        /// </returns>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double distance(flash.geom.Point pt1, flash.geom.Point pt2);

        /// <summary>Converts a pair of polar coordinates to a Cartesian point coordinate.</summary>
        /// <param name="len">The length coordinate of the polar pair.</param>
        /// <param name="angle">The angle, in radians, of the polar pair.</param>
        /// <returns>
        /// The Cartesian
        /// point.
        /// </returns>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.geom.Point polar(double len, double angle);
    }
}
