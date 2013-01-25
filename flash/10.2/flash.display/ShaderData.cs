using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.AbcInstance(142)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class ShaderData : Avm.Object
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ShaderData(flash.utils.ByteArray byteCode);
    }
}
