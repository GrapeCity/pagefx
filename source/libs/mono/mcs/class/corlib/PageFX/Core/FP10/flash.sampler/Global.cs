using System;
using System.Runtime.CompilerServices;

namespace flash.sampler
{
    [PageFX.GlobalFunctions]
    public class Global
    {
        [PageFX.ABC]
        [PageFX.QName("pauseSampling", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void pauseSampling();

        [PageFX.ABC]
        [PageFX.QName("stopSampling", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void stopSampling();

        [PageFX.ABC]
        [PageFX.QName("getMemberNames", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object getMemberNames(object arg0, bool arg1);

        [PageFX.ABC]
        [PageFX.QName("getMemberNames", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object getMemberNames(object arg0);

        [PageFX.ABC]
        [PageFX.QName("getGetterInvocationCount", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double getGetterInvocationCount(object arg0, Avm.QName arg1);

        [PageFX.ABC]
        [PageFX.QName("getInvocationCount", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double getInvocationCount(object arg0, Avm.QName arg1);

        [PageFX.ABC]
        [PageFX.QName("getSetterInvocationCount", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double getSetterInvocationCount(object arg0, Avm.QName arg1);

        [PageFX.ABC]
        [PageFX.QName("isGetterSetter", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static bool isGetterSetter(object arg0, Avm.QName arg1);

        [PageFX.ABC]
        [PageFX.QName("getSamples", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object getSamples();

        [PageFX.ABC]
        [PageFX.QName("getSampleCount", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double getSampleCount();

        [PageFX.ABC]
        [PageFX.QName("startSampling", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void startSampling();

        [PageFX.ABC]
        [PageFX.QName("getSize", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double getSize(object arg0);

        [PageFX.ABC]
        [PageFX.QName("clearSamples", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void clearSamples();
    }
}
