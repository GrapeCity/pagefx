using System;
using System.Runtime.CompilerServices;

namespace flash.geom
{
    [PageFX.AbcInstance(249)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class Utils3D : Avm.Object
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Utils3D();

        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.geom.Vector3D projectVector(flash.geom.Matrix3D m, flash.geom.Vector3D v);

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void projectVectors(flash.geom.Matrix3D m, Avm.Vector<double> verts, Avm.Vector<double> projectedVerts, Avm.Vector<double> uvts);

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.geom.Matrix3D pointTowards(double percent, flash.geom.Matrix3D mat, flash.geom.Vector3D pos, flash.geom.Vector3D at, flash.geom.Vector3D up);

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.geom.Matrix3D pointTowards(double percent, flash.geom.Matrix3D mat, flash.geom.Vector3D pos, flash.geom.Vector3D at);

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.geom.Matrix3D pointTowards(double percent, flash.geom.Matrix3D mat, flash.geom.Vector3D pos);
    }
}
