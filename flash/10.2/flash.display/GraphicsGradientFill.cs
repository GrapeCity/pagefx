using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.AbcInstance(203)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class GraphicsGradientFill : Avm.Object, flash.display.IGraphicsFill, flash.display.IGraphicsData
    {
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public Avm.Array colors;

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public Avm.Array alphas;

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public Avm.Array ratios;

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public flash.geom.Matrix matrix;

        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public double focalPointRatio;

        public extern virtual Avm.String type
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String spreadMethod
        {
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String interpolationMethod
        {
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsGradientFill(Avm.String type, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, object matrix, object spreadMethod, Avm.String interpolationMethod, double focalPointRatio);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsGradientFill(Avm.String type, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, object matrix, object spreadMethod, Avm.String interpolationMethod);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsGradientFill(Avm.String type, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, object matrix, object spreadMethod);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsGradientFill(Avm.String type, Avm.Array colors, Avm.Array alphas, Avm.Array ratios, object matrix);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsGradientFill(Avm.String type, Avm.Array colors, Avm.Array alphas, Avm.Array ratios);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsGradientFill(Avm.String type, Avm.Array colors, Avm.Array alphas);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsGradientFill(Avm.String type, Avm.Array colors);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsGradientFill(Avm.String type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicsGradientFill();


    }
}
