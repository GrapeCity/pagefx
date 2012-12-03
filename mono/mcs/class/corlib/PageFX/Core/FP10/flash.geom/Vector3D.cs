using System;
using System.Runtime.CompilerServices;

namespace flash.geom
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class Vector3D : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public double w;

        [PageFX.ABC]
        [PageFX.FP10]
        public double x;

        [PageFX.ABC]
        [PageFX.FP10]
        public double y;

        [PageFX.ABC]
        [PageFX.FP10]
        public double z;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Vector3D XAXIS;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Vector3D YAXIS;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Vector3D ZAXIS;

        public extern virtual double lengthSquared
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double length
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector3D(double arg0, double arg1, double arg2, double arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector3D(double arg0, double arg1, double arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector3D(double arg0, double arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector3D(double arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector3D();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void project();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void negate();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Vector3D add(Vector3D arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double dotProduct(Vector3D arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool nearEquals(Vector3D arg0, double arg1, bool arg2);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool nearEquals(Vector3D arg0, double arg1);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void scaleBy(double arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void decrementBy(Vector3D arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Vector3D crossProduct(Vector3D arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void incrementBy(Vector3D arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Vector3D subtract(Vector3D arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double normalize();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Vector3D clone();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool equals(Vector3D arg0, bool arg1);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool equals(Vector3D arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double angleBetween(Vector3D arg0, Vector3D arg1);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double distance(Vector3D arg0, Vector3D arg1);
    }
}
