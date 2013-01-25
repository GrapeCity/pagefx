using System;
using System.Runtime.CompilerServices;

namespace flash.automation
{
    [PageFX.AbcInstance(350)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class Configuration : Avm.Object
    {
        public extern static Avm.String testAutomationConfiguration
        {
            [PageFX.AbcClassTrait(0)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Configuration();


    }
}
