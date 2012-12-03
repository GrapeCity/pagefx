using System;
using System.Runtime.CompilerServices;

namespace flash.geom
{
    [PageFX.AbcInstance(259)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class Orientation3D : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String EULER_ANGLES;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String AXIS_ANGLE;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String QUATERNION;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Orientation3D();
    }
}
