using System;
using System.Runtime.CompilerServices;

namespace flash.system
{
    /// <summary>
    /// The SecurityDomain class represents the current security &quot;sandbox,&quot; also known as a security domain.
    /// By passing an instance of this class to Loader.load() , you can request that loaded media be placed in
    /// a particular sandbox.
    /// </summary>
    [PageFX.AbcInstance(119)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class SecurityDomain : Avm.Object
    {
        /// <summary>Gets the current security domain.</summary>
        public extern static flash.system.SecurityDomain currentDomain
        {
            [PageFX.AbcClassTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SecurityDomain();


    }
}
