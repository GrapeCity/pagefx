using System;
using System.Runtime.CompilerServices;

namespace flash.system
{
    /// <summary>
    /// The SecurityDomain class represents the current security &quot;sandbox,&quot; also known as a security domain.
    /// By passing an instance of this class to Loader.load() , you can request that loaded media be placed in
    /// a particular sandbox.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class SecurityDomain : Avm.Object
    {
        public extern static SecurityDomain currentDomain
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SecurityDomain();


    }
}
