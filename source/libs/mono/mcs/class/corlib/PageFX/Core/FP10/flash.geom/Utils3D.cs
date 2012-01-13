using System;
using System.Runtime.CompilerServices;

namespace flash.geom
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class Utils3D : Avm.Object
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Utils3D();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Matrix3D pointTowards(double arg0, Matrix3D arg1, Vector3D arg2, Vector3D arg3, Vector3D arg4);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Matrix3D pointTowards(double arg0, Matrix3D arg1, Vector3D arg2, Vector3D arg3);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Matrix3D pointTowards(double arg0, Matrix3D arg1, Vector3D arg2);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Vector3D projectVector(Matrix3D arg0, Vector3D arg1);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void projectVectors(Matrix3D arg0, Avm.Vector<double> arg1, Avm.Vector<double> arg2, Avm.Vector<double> arg3);
    }
}
