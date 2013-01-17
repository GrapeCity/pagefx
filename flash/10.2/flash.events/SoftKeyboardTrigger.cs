using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    [PageFX.AbcInstance(73)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class SoftKeyboardTrigger : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String CONTENT_TRIGGERED;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String USER_TRIGGERED;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SoftKeyboardTrigger();
    }
}
