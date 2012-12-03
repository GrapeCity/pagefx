using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class ShaderParameterType : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String INT2;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String INT3;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String INT4;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String BOOL2;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String BOOL3;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String BOOL4;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String INT;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String BOOL;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String MATRIX2X2;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String MATRIX3X3;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String MATRIX4X4;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String FLOAT2;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String FLOAT3;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String FLOAT;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String FLOAT4;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ShaderParameterType();
    }
}
