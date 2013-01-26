using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.AbcInstance(134)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class GraphicsTrianglePath : Avm.Object, flash.display.IGraphicsPath, flash.display.IGraphicsData
    {
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public Avm.Vector<int> indices;

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public Avm.Vector<double> vertices;

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public Avm.Vector<double> uvtData;

        public extern virtual Avm.String culling
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsTrianglePath(Avm.Vector<double> vertices, Avm.Vector<int> indices, Avm.Vector<double> uvtData, Avm.String culling);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsTrianglePath(Avm.Vector<double> vertices, Avm.Vector<int> indices, Avm.Vector<double> uvtData);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsTrianglePath(Avm.Vector<double> vertices, Avm.Vector<int> indices);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsTrianglePath(Avm.Vector<double> vertices);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsTrianglePath();


    }
}
