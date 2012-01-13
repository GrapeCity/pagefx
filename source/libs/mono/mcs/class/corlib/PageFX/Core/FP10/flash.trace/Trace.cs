using System;
using System.Runtime.CompilerServices;

namespace flash.trace
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class Trace : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public static int METHODS_AND_LINES_WITH_ARGS;

        [PageFX.ABC]
        [PageFX.FP10]
        public static int METHODS_AND_LINES;

        [PageFX.ABC]
        [PageFX.FP10]
        public static int OFF;

        [PageFX.ABC]
        [PageFX.FP10]
        public static int METHODS_WITH_ARGS;

        [PageFX.ABC]
        [PageFX.FP10]
        public static int METHODS;

        [PageFX.ABC]
        [PageFX.FP10]
        public static object LISTENER;

        [PageFX.ABC]
        [PageFX.FP10]
        public static object FILE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Trace();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.Function getListener();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static int getLevel(int arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static int getLevel();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object setLevel(int arg0, int arg1);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object setLevel(int arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object setListener(Avm.Function arg0);
    }
}
