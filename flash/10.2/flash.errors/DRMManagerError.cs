using System;
using System.Runtime.CompilerServices;

namespace flash.errors
{
    [PageFX.AbcInstance(319)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class DRMManagerError : Avm.Error
    {
        public extern virtual int subErrorID
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMManagerError(Avm.String message, int id, int subErrorID);

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();
    }
}
