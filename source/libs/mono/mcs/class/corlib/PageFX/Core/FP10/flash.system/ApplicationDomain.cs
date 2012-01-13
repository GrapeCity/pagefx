using System;
using System.Runtime.CompilerServices;

namespace flash.system
{
    /// <summary>
    /// The ApplicationDomain class is a container for discrete groups of class definitions.
    /// Application domains are used to partition classes that are in the same security domain.
    /// They allow multiple definitions of the same class to exist and allow children to reuse parent
    /// definitions.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class ApplicationDomain : Avm.Object
    {
        public extern virtual flash.utils.ByteArray domainMemory
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual ApplicationDomain parentDomain
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static ApplicationDomain currentDomain
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static uint MIN_DOMAIN_MEMORY_LENGTH
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ApplicationDomain(ApplicationDomain arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ApplicationDomain();

        /// <summary>
        /// Gets a public definition from the specified application domain.
        /// The definition can be that of a class, a namespace, or a function.
        /// </summary>
        /// <param name="arg0">The name of the definition.</param>
        /// <returns>The object associated with the definition.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object getDefinition(Avm.String arg0);

        /// <summary>
        /// Checks to see if a public definition exists within the specified application domain.
        /// The definition can be that of a class, a namespace, or a function.
        /// </summary>
        /// <param name="arg0">The name of the definition.</param>
        /// <returns>A value of true if the specified definition exists; otherwise, false.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool hasDefinition(Avm.String arg0);


    }
}
