using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(344)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class FontDescription : Avm.Object
    {
        public extern virtual Avm.String renderingMode
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

        public extern virtual Avm.String fontLookup
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String fontName
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String fontPosture
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String fontWeight
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String cffHinting
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

        public extern virtual bool locked
        {
            [PageFX.AbcInstanceTrait(12)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(13)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontDescription(Avm.String fontName, Avm.String fontWeight, Avm.String fontPosture, Avm.String fontLookup, Avm.String renderingMode, Avm.String cffHinting);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontDescription(Avm.String fontName, Avm.String fontWeight, Avm.String fontPosture, Avm.String fontLookup, Avm.String renderingMode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontDescription(Avm.String fontName, Avm.String fontWeight, Avm.String fontPosture, Avm.String fontLookup);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontDescription(Avm.String fontName, Avm.String fontWeight, Avm.String fontPosture);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontDescription(Avm.String fontName, Avm.String fontWeight);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontDescription(Avm.String fontName);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontDescription();

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.text.engine.FontDescription clone();

        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static bool isFontCompatible(Avm.String fontName, Avm.String fontWeight, Avm.String fontPosture);

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static bool isDeviceFontCompatible(Avm.String fontName, Avm.String fontWeight, Avm.String fontPosture);
    }
}
