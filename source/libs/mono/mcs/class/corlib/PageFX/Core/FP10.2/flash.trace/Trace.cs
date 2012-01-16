using System;
using System.Runtime.CompilerServices;

namespace flash.trace
{
    [PageFX.AbcInstance(284)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class Trace : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static int OFF;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static int METHODS;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static int METHODS_WITH_ARGS;

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static int METHODS_AND_LINES;

        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static int METHODS_AND_LINES_WITH_ARGS;

        [PageFX.AbcClassTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static object FILE;

        [PageFX.AbcClassTrait(6)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static object LISTENER;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Trace();

        [PageFX.AbcClassTrait(7)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object setLevel(int l, int target);

        [PageFX.AbcClassTrait(7)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object setLevel(int l);

        [PageFX.AbcClassTrait(8)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static int getLevel(int target);

        [PageFX.AbcClassTrait(8)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static int getLevel();

        [PageFX.AbcClassTrait(9)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object setListener(Avm.Function f);

        [PageFX.AbcClassTrait(10)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.Function getListener();
    }
}
