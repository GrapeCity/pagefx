using System;
using System.Runtime.CompilerServices;

namespace flash.geom
{
    [PageFX.AbcInstance(274)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class Vector3D : Avm.Object
    {
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public double x;

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public double y;

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public double z;

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public double w;

        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static flash.geom.Vector3D X_AXIS;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static flash.geom.Vector3D Y_AXIS;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static flash.geom.Vector3D Z_AXIS;

        public extern virtual double length
        {
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double lengthSquared
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector3D(double x, double y, double z, double w);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector3D(double x, double y, double z);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector3D(double x, double y);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector3D(double x);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector3D();

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Vector3D clone();

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double dotProduct(flash.geom.Vector3D a);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Vector3D crossProduct(flash.geom.Vector3D a);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double normalize();

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void scaleBy(double s);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void incrementBy(flash.geom.Vector3D a);

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void decrementBy(flash.geom.Vector3D a);

        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Vector3D add(flash.geom.Vector3D a);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Vector3D subtract(flash.geom.Vector3D a);

        [PageFX.AbcInstanceTrait(15)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void negate();

        [PageFX.AbcInstanceTrait(16)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool equals(flash.geom.Vector3D toCompare, bool allFour);

        [PageFX.AbcInstanceTrait(16)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool equals(flash.geom.Vector3D toCompare);

        [PageFX.AbcInstanceTrait(17)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool nearEquals(flash.geom.Vector3D toCompare, double tolerance, bool allFour);

        [PageFX.AbcInstanceTrait(17)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool nearEquals(flash.geom.Vector3D toCompare, double tolerance);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void project();

        [PageFX.AbcInstanceTrait(19)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double angleBetween(flash.geom.Vector3D a, flash.geom.Vector3D b);

        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double distance(flash.geom.Vector3D pt1, flash.geom.Vector3D pt2);
    }
}
