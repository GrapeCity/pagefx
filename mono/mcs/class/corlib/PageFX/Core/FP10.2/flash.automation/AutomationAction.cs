using System;
using System.Runtime.CompilerServices;

namespace flash.automation
{
    [PageFX.AbcInstance(233)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class AutomationAction : Avm.Object
    {
        public extern virtual Avm.String type
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern AutomationAction();


    }
}
