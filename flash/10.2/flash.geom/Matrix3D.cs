using System;
using System.Runtime.CompilerServices;

namespace flash.geom
{
    [PageFX.AbcInstance(209)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class Matrix3D : Avm.Object
    {
        public extern virtual Avm.Vector<Avm.String> rawData
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual flash.geom.Vector3D position
        {
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double determinant
        {
            [PageFX.AbcInstanceTrait(21)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Matrix3D(Avm.Vector<Avm.String> v);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Matrix3D();

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Matrix3D clone();

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void append(flash.geom.Matrix3D lhs);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void prepend(flash.geom.Matrix3D rhs);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool invert();

        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void identity();

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Vector<Avm.String> decompose(Avm.String orientationStyle);

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Vector<Avm.String> decompose();

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool recompose(Avm.Vector<Avm.String> components, Avm.String orientationStyle);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool recompose(Avm.Vector<Avm.String> components);

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void appendTranslation(double x, double y, double z);

        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void appendRotation(double degrees, flash.geom.Vector3D axis, flash.geom.Vector3D pivotPoint);

        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void appendRotation(double degrees, flash.geom.Vector3D axis);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void appendScale(double xScale, double yScale, double zScale);

        [PageFX.AbcInstanceTrait(15)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void prependTranslation(double x, double y, double z);

        [PageFX.AbcInstanceTrait(16)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void prependRotation(double degrees, flash.geom.Vector3D axis, flash.geom.Vector3D pivotPoint);

        [PageFX.AbcInstanceTrait(16)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void prependRotation(double degrees, flash.geom.Vector3D axis);

        [PageFX.AbcInstanceTrait(17)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void prependScale(double xScale, double yScale, double zScale);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Vector3D transformVector(flash.geom.Vector3D v);

        [PageFX.AbcInstanceTrait(19)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Vector3D deltaTransformVector(flash.geom.Vector3D v);

        [PageFX.AbcInstanceTrait(20)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void transformVectors(Avm.Vector<Avm.String> vin, Avm.Vector<Avm.String> vout);

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void transpose();

        [PageFX.AbcInstanceTrait(23)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void pointAt(flash.geom.Vector3D pos, flash.geom.Vector3D at, flash.geom.Vector3D up);

        [PageFX.AbcInstanceTrait(23)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void pointAt(flash.geom.Vector3D pos, flash.geom.Vector3D at);

        [PageFX.AbcInstanceTrait(23)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void pointAt(flash.geom.Vector3D pos);

        [PageFX.AbcInstanceTrait(24)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void interpolateTo(flash.geom.Matrix3D toMat, double percent);

        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.geom.Matrix3D interpolate(flash.geom.Matrix3D thisMat, flash.geom.Matrix3D toMat, double percent);
    }
}
