using System;
using System.Runtime.CompilerServices;

namespace flash.geom
{
    /// <summary>
    /// A Rectangle object is an area defined by its position, as
    /// indicated by its top-left corner point ( x , y ) and by its width
    /// and its height.
    /// </summary>
    [PageFX.AbcInstance(77)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class Rectangle : Avm.Object
    {
        /// <summary>
        /// The x coordinate of the top-left corner of the rectangle. Changing
        /// the value of the x property of a Rectangle object has no effect on the
        /// y,
        /// width, and height properties.
        /// The value of the x property is equal to the value of the
        /// left property.
        /// </summary>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double x;

        /// <summary>
        /// The y coordinate of the top-left corner of the rectangle. Changing
        /// the value of the y property of a Rectangle object has no effect on the
        /// x, width, and height properties.
        /// The value of the y property is equal to the value of
        /// the top property.
        /// </summary>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double y;

        /// <summary>
        /// The width of the rectangle, in pixels. Changing the width value of a Rectangle object
        /// has no effect on the x, y, and height
        /// properties.
        /// </summary>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double width;

        /// <summary>
        /// The height of the rectangle, in pixels. Changing the height value of a Rectangle
        /// object has no effect on the x, y, and
        /// width properties.
        /// </summary>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double height;

        /// <summary>
        /// The x coordinate of the top-left corner of the rectangle. Changing
        /// the left property of a Rectangle object has no effect on the y
        /// and height properties. However it does affect the width
        /// property, whereas changing the x value does not affect the
        /// width property.
        /// The value of the left property is equal to the value of the
        /// x property.
        /// </summary>
        public extern virtual double left
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
        /// The sum of the x and
        /// width properties.
        /// </summary>
        public extern virtual double right
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

        /// <summary>
        /// The y coordinate of the top-left corner of the rectangle. Changing
        /// the top property of a Rectangle object has no effect on the x
        /// and width properties. However it does affect the height
        /// property, whereas changing the y value does not affect the
        /// height property.
        /// The value of the top property is equal to the value of the y property.
        /// </summary>
        public extern virtual double top
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

        /// <summary>
        /// The sum of the y and
        /// height properties.
        /// </summary>
        public extern virtual double bottom
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
        /// The location of the Rectangle object&apos;s top-left corner, determined by the x and
        /// y coordinates of the point.
        /// </summary>
        public extern virtual flash.geom.Point topLeft
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
        /// The location of the Rectangle object&apos;s bottom-right corner, determined by the values of the right and
        /// bottom properties.
        /// </summary>
        public extern virtual flash.geom.Point bottomRight
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
        /// The size of the Rectangle object, expressed as a Point object with the values
        /// of the width and height properties.
        /// </summary>
        public extern virtual flash.geom.Point size
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
        public extern Rectangle(double x, double y, double width, double height);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Rectangle(double x, double y, double width);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Rectangle(double x, double y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Rectangle(double x);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Rectangle();

        /// <summary>
        /// Returns a new Rectangle object with the same values for the x, y,
        /// width, and height properties as the original Rectangle object.
        /// </summary>
        /// <returns>
        /// A new Rectangle object with the same values for the x, y,
        /// width, and height properties as the original Rectangle object.
        /// </returns>
        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Rectangle clone();

        /// <summary>Determines whether or not this Rectangle object is empty.</summary>
        /// <returns>
        /// A value of true if the Rectangle object&apos;s width or height is less than
        /// or equal to 0; otherwise false.
        /// </returns>
        [PageFX.AbcInstanceTrait(19)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool isEmpty();

        /// <summary>
        /// Sets all of the Rectangle object&apos;s properties to 0. A Rectangle object is empty if its width or
        /// height is less than or equal to 0.
        /// This method sets the values of the x, y,
        /// width, and height properties to 0.
        /// </summary>
        [PageFX.AbcInstanceTrait(20)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setEmpty();

        /// <summary>
        /// Increases the size of the Rectangle object by the specified amounts, in pixels. The center point of the
        /// Rectangle object stays the same, and its size increases to the left and right by the
        /// dx value, and to the top and the bottom by the dy value.
        /// </summary>
        /// <param name="dx">
        /// The value to be added to the left and the right of the Rectangle object. The following
        /// equation is used to calculate the new width and position of the rectangle:
        /// x -= dx;
        /// width += 2 ~~ dx;
        /// </param>
        /// <param name="dy">
        /// The value to be added to the top and the bottom of the Rectangle. The
        /// following equation is used to calculate the new height and position of the rectangle:
        /// y -= dy;
        /// height += 2 ~~ dy;
        /// </param>
        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void inflate(double dx, double dy);

        /// <summary>
        /// Increases the size of the Rectangle object.
        /// This method is similar to the Rectangle.inflate() method
        /// except it takes a Point object as a parameter.
        /// The following two code examples give the same result:
        /// rect1=new flash.geom.Rectangle(0,0,2,5);
        /// rect1.inflate(2,2)
        /// rect1=new flash.geom.Rectangle(0,0,2,5);
        /// pt1=new flash.geom.Point(2,2);
        /// rect1.inflatePoint(pt1)
        /// </summary>
        /// <param name="point">
        /// The x property of this Point object is used to increase the
        /// horizontal dimension of the Rectangle object. The y property
        /// is used to increase the vertical dimension of the Rectangle object.
        /// </param>
        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void inflatePoint(flash.geom.Point point);

        /// <summary>
        /// Adjusts the location of the Rectangle object, as determined by its top-left corner,
        /// by the specified amounts.
        /// </summary>
        /// <param name="dx">Moves the x value of the Rectangle object by this amount.</param>
        /// <param name="dy">Moves the y value of the Rectangle object by this amount.</param>
        [PageFX.AbcInstanceTrait(23)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void offset(double dx, double dy);

        /// <summary>
        /// Adjusts the location of the Rectangle object using a Point object as a parameter.
        /// This method is similar to the Rectangle.offset() method, except that it takes a Point
        /// object as a parameter.
        /// </summary>
        /// <param name="point">A Point object to use to offset this Rectangle object.</param>
        [PageFX.AbcInstanceTrait(24)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void offsetPoint(flash.geom.Point point);

        /// <summary>
        /// Determines whether the specified point is contained within the rectangular region defined
        /// by this Rectangle object.
        /// </summary>
        /// <param name="x">The x coordinate (horizontal position) of the point.</param>
        /// <param name="y">The y coordinate (vertical position) of the point.</param>
        /// <returns>
        /// A value of true if the Rectangle object contains the specified point;
        /// otherwise false.
        /// </returns>
        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool contains(double x, double y);

        /// <summary>
        /// Determines whether the specified point is contained within the rectangular region defined
        /// by this Rectangle object. This method is similar to the Rectangle.contains() method,
        /// except that it takes a Point object as a parameter.
        /// </summary>
        /// <param name="point">The point, as represented by its x and y coordinates.</param>
        /// <returns>
        /// A value of true if the Rectangle object contains the specified point;
        /// otherwise false.
        /// </returns>
        [PageFX.AbcInstanceTrait(26)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool containsPoint(flash.geom.Point point);

        /// <summary>
        /// Determines whether the Rectangle object specified by the rect parameter is contained
        /// within this Rectangle object. A Rectangle object is said to contain another if the second
        /// Rectangle object falls entirely within the boundaries of the first.
        /// </summary>
        /// <param name="rect">The Rectangle object being checked.</param>
        /// <returns>
        /// A value of true if the Rectangle object that you specify is
        /// contained by this Rectangle object; otherwise false.
        /// </returns>
        [PageFX.AbcInstanceTrait(27)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool containsRect(flash.geom.Rectangle rect);

        /// <summary>
        /// If the Rectangle object specified in the toIntersect parameter intersects with this Rectangle
        /// object, returns the area of intersection as a Rectangle object.
        /// If the rectangles do not intersect, this method returns an empty Rectangle object with its properties
        /// set to 0.
        /// </summary>
        /// <param name="toIntersect">
        /// The Rectangle object to compare against to see if it intersects with
        /// this Rectangle object.
        /// </param>
        /// <returns>
        /// A Rectangle object that equals the area of intersection. If the rectangles do not
        /// intersect, this method returns an empty Rectangle object; that is, a rectangle with its x,
        /// y, width, and height properties set to 0.
        /// </returns>
        [PageFX.AbcInstanceTrait(28)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Rectangle intersection(flash.geom.Rectangle toIntersect);

        /// <summary>
        /// Determines whether the object specified in the toIntersect parameter intersects
        /// with this Rectangle object. This method checks the x, y,
        /// width, and height properties of the specified Rectangle object to see
        /// if it intersects with this Rectangle object.
        /// </summary>
        /// <param name="toIntersect">The Rectangle object to compare against this Rectangle object.</param>
        /// <returns>
        /// A value of true if the specified object intersects with this Rectangle object;
        /// otherwise false.
        /// </returns>
        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool intersects(flash.geom.Rectangle toIntersect);

        /// <summary>
        /// Adds two rectangles together to create a new Rectangle object, by
        /// filling in the horizontal and vertical space between the two rectangles.
        /// </summary>
        /// <param name="toUnion">A Rectangle object to add to this Rectangle object.</param>
        /// <returns>A new Rectangle object that is the union of the two rectangles.</returns>
        [PageFX.AbcInstanceTrait(30)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Rectangle union(flash.geom.Rectangle toUnion);

        /// <summary>
        /// Determines whether the object specified in the toCompare parameter is
        /// equal to this Rectangle object. This method compares the x, y,
        /// width, and height properties of an object against the same properties
        /// of this Rectangle object.
        /// </summary>
        /// <param name="toCompare">The rectangle to compare to this Rectangle object.</param>
        /// <returns>
        /// A value of true if the object has exactly the same values for the
        /// x, y, width, and height properties
        /// as this Rectangle object; otherwise false.
        /// </returns>
        [PageFX.AbcInstanceTrait(31)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool equals(flash.geom.Rectangle toCompare);

        /// <summary>
        /// Builds and returns a string that lists the horizontal and vertical positions
        /// and the width and height of the Rectangle object.
        /// </summary>
        /// <returns>
        /// A string listing the value of each of the following properties of the Rectangle object:
        /// x, y, width, and height.
        /// </returns>
        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();
    }
}
