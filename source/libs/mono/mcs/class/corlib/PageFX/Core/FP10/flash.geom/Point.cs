using System;
using System.Runtime.CompilerServices;

namespace flash.geom
{
    /// <summary>
    /// The Point object represents a location in a two-dimensional coordinate system, where
    /// x  represents the horizontal axis and y  represents the vertical axis.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class Point : Avm.Object
    {
        /// <summary>The horizontal coordinate of the point. The default value is 0.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public double x;

        /// <summary>The vertical coordinate of the point. The default value is 0.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public double y;

        public extern virtual double length
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Point(double arg0, double arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Point(double arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Point();

        /// <summary>
        /// Adds the coordinates of another point to the coordinates of this point to create
        /// a new point.
        /// </summary>
        /// <param name="arg0">The point to be added.</param>
        /// <returns>The new point.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Point add(Point arg0);

        /// <summary>
        /// Returns a string that contains the values of the x and y coordinates.
        /// The string has the form &quot;(x=x, y=y)&quot;, so calling the
        /// toString() method for a point at 23,17 would return &quot;(x=23, y=17)&quot;.
        /// </summary>
        /// <returns>
        /// The string representation
        /// of the coordinates.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();

        /// <summary>Scales the line segment between (0,0) and the current point to a set length.</summary>
        /// <param name="arg0">
        /// The scaling value. For example, if the current point is (0,5), and you normalize
        /// it to 1, the point returned is at (0,1).
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void normalize(double arg0);

        /// <summary>
        /// Subtracts the coordinates of another point from the coordinates of this point to
        /// create a new point.
        /// </summary>
        /// <param name="arg0">The point to be subtracted.</param>
        /// <returns>The new point.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Point subtract(Point arg0);

        /// <summary>
        /// Offsets the Point object by the specified amount. The value of dx is
        /// added to the original value of x to create the new x value. The value
        /// of dy is added to the original value of y to create the new
        /// y value.
        /// </summary>
        /// <param name="arg0">The amount by which to offset the horizontal coordinate, x.</param>
        /// <param name="arg1">The amount by which to offset the vertical coordinate, y.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void offset(double arg0, double arg1);

        /// <summary>Creates a copy of this Point object.</summary>
        /// <returns>
        /// The new Point
        /// object.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Point clone();

        /// <summary>
        /// Determines whether two points are equal. Two points are equal if they have the same
        /// x and y values.
        /// </summary>
        /// <param name="arg0">The point to be compared.</param>
        /// <returns>
        /// A value of true
        /// if the object is equal to this Point object; false if it is not equal.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool equals(Point arg0);

        /// <summary>
        /// Determines a point between two specified points. The parameter f determines
        /// where the new interpolated point is located relative to the two end points specified
        /// by parameters pt1 and pt2. The closer the value of the
        /// parameter f is to 1.0, the closer the interpolated point
        /// is to the first point (parameter pt1). The closer the value of the
        /// parameter f is to 0, the closer the interpolated point is to the second
        /// point (parameter pt2).
        /// </summary>
        /// <param name="arg0">The first point.</param>
        /// <param name="arg1">The second point.</param>
        /// <param name="arg2">
        /// The level of interpolation between the two points. Indicates where the new
        /// point will be, along the line between pt1 and pt2. If
        /// f=1, pt1 is returned; if f=0, pt2
        /// is returned.
        /// </param>
        /// <returns>
        /// The new, interpolated
        /// point.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Point interpolate(Point arg0, Point arg1, double arg2);

        /// <summary>Returns the distance between pt1 and pt2.</summary>
        /// <param name="arg0">The first point.</param>
        /// <param name="arg1">The second point.</param>
        /// <returns>
        /// The distance between
        /// the first and second points.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double distance(Point arg0, Point arg1);

        /// <summary>Converts a pair of polar coordinates to a Cartesian point coordinate.</summary>
        /// <param name="arg0">The length coordinate of the polar pair.</param>
        /// <param name="arg1">The angle, in radians, of the polar pair.</param>
        /// <returns>
        /// The Cartesian
        /// point.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Point polar(double arg0, double arg1);
    }
}
