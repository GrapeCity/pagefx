using System;
using System.Runtime.CompilerServices;

namespace flash.geom
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class PerspectiveProjection : Avm.Object
    {
        public extern virtual Point projectionCenter
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

        public extern virtual double fieldOfView
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

        public extern virtual double focalLength
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern PerspectiveProjection();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Matrix3D toMatrix3D();
    }
}
