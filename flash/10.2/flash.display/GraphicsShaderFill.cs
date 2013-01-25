using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.AbcInstance(106)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class GraphicsShaderFill : Avm.Object, flash.display.IGraphicsFill, flash.display.IGraphicsData
    {
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public flash.display.Shader shader;

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public flash.geom.Matrix matrix;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsShaderFill(flash.display.Shader shader, flash.geom.Matrix matrix);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsShaderFill(flash.display.Shader shader);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsShaderFill();
    }
}
