using System;
using System.Runtime.CompilerServices;

namespace flash.automation
{
    [PageFX.AbcInstance(234)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class KeyboardAutomationAction : flash.automation.AutomationAction
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEY_DOWN;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String KEY_UP;

        public extern virtual uint keyCode
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
        public extern KeyboardAutomationAction(Avm.String type, uint keyCode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern KeyboardAutomationAction(Avm.String type);


    }
}
