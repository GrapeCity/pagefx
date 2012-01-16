using System;
using System.Runtime.CompilerServices;

namespace flash.globalization
{
    [PageFX.AbcInstance(322)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class StringTools : Avm.Object
    {
        public extern virtual Avm.String lastOperationStatus
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String requestedLocaleIDName
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String actualLocaleIDName
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StringTools(Avm.String requestedLocaleIDName);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toLowerCase(Avm.String s);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toUpperCase(Avm.String s);

        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.Vector<Avm.String> getAvailableLocaleIDNames();
    }
}
