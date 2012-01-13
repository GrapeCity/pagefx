using System;
using System.Runtime.CompilerServices;

namespace flash.geom
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class Matrix3D : Avm.Object
    {
        public extern virtual Avm.Vector<double> rawData
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Vector3D position
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double determinant
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Matrix3D(Avm.Vector<double> arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Matrix3D();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void transpose();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void prependTranslation(double arg0, double arg1, double arg2);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Vector3D deltaTransformVector(Vector3D arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void pointAt(Vector3D arg0, Vector3D arg1, Vector3D arg2);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void pointAt(Vector3D arg0, Vector3D arg1);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void pointAt(Vector3D arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void transformVectors(Avm.Vector<double> arg0, Avm.Vector<double> arg1);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void prependRotation(double arg0, Vector3D arg1, Vector3D arg2);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void prependRotation(double arg0, Vector3D arg1);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void prepend(Matrix3D arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Vector3D transformVector(Vector3D arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void appendScale(double arg0, double arg1, double arg2);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Vector<flash.geom.Vector3D> decompose(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Vector<flash.geom.Vector3D> decompose();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void interpolateTo(Matrix3D arg0, double arg1);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool invert();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void appendTranslation(double arg0, double arg1, double arg2);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void appendRotation(double arg0, Vector3D arg1, Vector3D arg2);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void appendRotation(double arg0, Vector3D arg1);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void append(Matrix3D arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void prependScale(double arg0, double arg1, double arg2);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Matrix3D clone();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void identity();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool recompose(Avm.Vector<flash.geom.Vector3D> arg0, Avm.String arg1);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool recompose(Avm.Vector<flash.geom.Vector3D> arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Matrix3D interpolate(Matrix3D arg0, Matrix3D arg1, double arg2);
    }
}
