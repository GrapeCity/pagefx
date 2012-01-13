using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class GraphicsTrianglePath : Avm.Object, IGraphicsPath, IGraphicsData
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public Avm.Vector<int> indices;

        [PageFX.ABC]
        [PageFX.FP10]
        public Avm.Vector<double> vertices;

        [PageFX.ABC]
        [PageFX.FP10]
        public Avm.Vector<double> uvtData;

        public extern virtual Avm.String culling
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

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsTrianglePath(Avm.Vector<double> arg0, Avm.Vector<int> arg1, Avm.Vector<double> arg2, Avm.String arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsTrianglePath(Avm.Vector<double> arg0, Avm.Vector<int> arg1, Avm.Vector<double> arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsTrianglePath(Avm.Vector<double> arg0, Avm.Vector<int> arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsTrianglePath(Avm.Vector<double> arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsTrianglePath();


    }
}
