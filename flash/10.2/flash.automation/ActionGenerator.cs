using System;
using System.Runtime.CompilerServices;

namespace flash.automation
{
    [PageFX.AbcInstance(193)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class ActionGenerator : Avm.Object
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ActionGenerator();

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void generateActions(Avm.Array a);

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void generateAction(flash.automation.AutomationAction action);
    }
}
