using System;
using System.Runtime.CompilerServices;

namespace flash.geom
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class Orientation3D : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String AXIS_ANGLE;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String EULER_ANGLES;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String QUATERNION;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Orientation3D();
    }
}
