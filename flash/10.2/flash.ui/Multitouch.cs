using System;
using System.Runtime.CompilerServices;

namespace flash.ui
{
    [PageFX.AbcInstance(145)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class Multitouch : Avm.Object
    {
        public extern static Avm.String inputMode
        {
            [PageFX.AbcClassTrait(0)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcClassTrait(1)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern static bool supportsTouchEvents
        {
            [PageFX.AbcClassTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static bool supportsGestureEvents
        {
            [PageFX.AbcClassTrait(3)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static Avm.Vector<Avm.String> supportedGestures
        {
            [PageFX.AbcClassTrait(4)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static int maxTouchPoints
        {
            [PageFX.AbcClassTrait(5)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Multitouch();


    }
}
