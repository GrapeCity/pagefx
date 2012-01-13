using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class GraphicsEndFill : Avm.Object, IGraphicsFill, IGraphicsData
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsEndFill();
    }
}
