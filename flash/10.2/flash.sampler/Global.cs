using System;
using System.Runtime.CompilerServices;

namespace flash.sampler
{
    [PageFX.GlobalFunctions]
    public partial class Global
    {
        [PageFX.AbcScript(3)]
        [PageFX.AbcScriptTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("clearSamples", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void clearSamples();

        [PageFX.AbcScript(3)]
        [PageFX.AbcScriptTrait(5)]
        [PageFX.ABC]
        [PageFX.QName("startSampling", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void startSampling();

        [PageFX.AbcScript(3)]
        [PageFX.AbcScriptTrait(6)]
        [PageFX.ABC]
        [PageFX.QName("stopSampling", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void stopSampling();

        [PageFX.AbcScript(3)]
        [PageFX.AbcScriptTrait(7)]
        [PageFX.ABC]
        [PageFX.QName("pauseSampling", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void pauseSampling();

        [PageFX.AbcScript(3)]
        [PageFX.AbcScriptTrait(8)]
        [PageFX.ABC]
        [PageFX.QName("sampleInternalAllocs", "flash.sampler", "package")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void sampleInternalAllocs(bool b);

        [PageFX.AbcScript(3)]
        [PageFX.AbcScriptTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("setSamplerCallback", "flash.sampler", "package")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void setSamplerCallback(Avm.Function f);

        [PageFX.AbcScript(3)]
        [PageFX.AbcScriptTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("getSize", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double getSize(object o);

        [PageFX.AbcScript(3)]
        [PageFX.AbcScriptTrait(12)]
        [PageFX.ABC]
        [PageFX.QName("getMemberNames", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object getMemberNames(object o, bool instanceNames);

        [PageFX.AbcScript(3)]
        [PageFX.AbcScriptTrait(12)]
        [PageFX.ABC]
        [PageFX.QName("getMemberNames", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object getMemberNames(object o);

        [PageFX.AbcScript(3)]
        [PageFX.AbcScriptTrait(13)]
        [PageFX.ABC]
        [PageFX.QName("getSamples", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object getSamples();

        [PageFX.AbcScript(3)]
        [PageFX.AbcScriptTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("getSampleCount", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double getSampleCount();

        [PageFX.AbcScript(3)]
        [PageFX.AbcScriptTrait(15)]
        [PageFX.ABC]
        [PageFX.QName("getInvocationCount", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double getInvocationCount(object obj, Avm.QName qname);

        [PageFX.AbcScript(3)]
        [PageFX.AbcScriptTrait(16)]
        [PageFX.ABC]
        [PageFX.QName("getSetterInvocationCount", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double getSetterInvocationCount(object obj, Avm.QName qname);

        [PageFX.AbcScript(3)]
        [PageFX.AbcScriptTrait(17)]
        [PageFX.ABC]
        [PageFX.QName("getGetterInvocationCount", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double getGetterInvocationCount(object obj, Avm.QName qname);

        [PageFX.AbcScript(3)]
        [PageFX.AbcScriptTrait(19)]
        [PageFX.ABC]
        [PageFX.QName("isGetterSetter", "flash.sampler", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static bool isGetterSetter(object obj, Avm.QName qname);

        [PageFX.AbcScript(3)]
        [PageFX.AbcScriptTrait(20)]
        [PageFX.ABC]
        [PageFX.QName("getLexicalScopes", "flash.sampler", "package")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.Array getLexicalScopes(Avm.Function obj);

        [PageFX.AbcScript(3)]
        [PageFX.AbcScriptTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("getSavedThis", "flash.sampler", "package")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object getSavedThis(Avm.Function obj);

        [PageFX.AbcScript(3)]
        [PageFX.AbcScriptTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("getMasterString", "flash.sampler", "package")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String getMasterString(Avm.String str);
    }
}
