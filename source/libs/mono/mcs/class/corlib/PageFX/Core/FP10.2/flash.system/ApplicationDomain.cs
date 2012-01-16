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
    [PageFX.AbcInstance(264)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class ApplicationDomain : Avm.Object
    {
        /// <summary>Gets the parent domain of this application domain.</summary>
        public extern virtual flash.system.ApplicationDomain parentDomain
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual flash.utils.ByteArray domainMemory
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>Gets the current application domain in which your code is executing.</summary>
        public extern static flash.system.ApplicationDomain currentDomain
        {
            [PageFX.AbcClassTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static uint MIN_DOMAIN_MEMORY_LENGTH
        {
            [PageFX.AbcClassTrait(1)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ApplicationDomain(flash.system.ApplicationDomain parentDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ApplicationDomain();

        /// <summary>
        /// Gets a public definition from the specified application domain.
        /// The definition can be that of a class, a namespace, or a function.
        /// </summary>
        /// <param name="name">The name of the definition.</param>
        /// <returns>The object associated with the definition.</returns>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object getDefinition(Avm.String name);

        /// <summary>
        /// Checks to see if a public definition exists within the specified application domain.
        /// The definition can be that of a class, a namespace, or a function.
        /// </summary>
        /// <param name="name">The name of the definition.</param>
        /// <returns>A value of true if the specified definition exists; otherwise, false.</returns>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool hasDefinition(Avm.String name);


    }
}
