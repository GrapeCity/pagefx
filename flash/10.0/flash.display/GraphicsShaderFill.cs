using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class GraphicsShaderFill : Avm.Object, IGraphicsFill, IGraphicsData
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public flash.geom.Matrix matrix;

        [PageFX.ABC]
        [PageFX.FP10]
        public Shader shader;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsShaderFill(Shader arg0, flash.geom.Matrix arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsShaderFill(Shader arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsShaderFill();
    }
}
