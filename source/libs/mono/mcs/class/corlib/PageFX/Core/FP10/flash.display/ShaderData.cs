using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class ShaderData : Avm.Object
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ShaderData(flash.utils.ByteArray arg0);
    }
}
