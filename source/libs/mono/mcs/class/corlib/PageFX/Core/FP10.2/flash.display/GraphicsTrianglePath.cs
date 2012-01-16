using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.AbcInstance(134)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class GraphicsTrianglePath : Avm.Object, flash.display.IGraphicsPath, flash.display.IGraphicsData
    {
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public Avm.Vector<Avm.String> indices;

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public Avm.Vector<Avm.String> vertices;

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public Avm.Vector<Avm.String> uvtData;

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
        public extern GraphicsTrianglePath(Avm.Vector<Avm.String> vertices, Avm.Vector<Avm.String> indices, Avm.Vector<Avm.String> uvtData, Avm.String culling);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsTrianglePath(Avm.Vector<Avm.String> vertices, Avm.Vector<Avm.String> indices, Avm.Vector<Avm.String> uvtData);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsTrianglePath(Avm.Vector<Avm.String> vertices, Avm.Vector<Avm.String> indices);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsTrianglePath(Avm.Vector<Avm.String> vertices);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsTrianglePath();


    }
}
