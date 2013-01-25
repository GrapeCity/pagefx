using System;
using System.Runtime.CompilerServices;

namespace __AS3__.vec
{
    [PageFX.AbcInstance(10)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class Vector : Avm.Object
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector();
    }
}
