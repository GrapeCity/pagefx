using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// The URLVariables class allows you to transfer
    /// variables between an application and a
    /// server.
    /// Use URLVariables objects with methods of the URLLoader
    /// class, with the data  property
    /// of the URLRequest class, and with flash.net package
    /// functions.
    /// </summary>
    [PageFX.AbcInstance(336)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class URLVariables : Avm.Object
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern URLVariables(Avm.String source);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern URLVariables();

        /// <summary>
        /// Converts the variable string to properties of the specified URLVariables object.
        /// This method is used internally by the URLVariables events.
        /// Most users do not need to call this method directly.
        /// </summary>
        /// <param name="source">A URL-encoded query string containing name/value pairs.</param>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void decode(Avm.String source);

        /// <summary>
        /// Returns a string containing all enumerable variables,
        /// in the MIME content encoding application/x-www-form-urlencoded.
        /// </summary>
        /// <returns>A URL-encoded string containing name/value pairs.</returns>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();
    }
}
