using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// A Boolean object is a data type that can have one of two values, either true
    /// or false , used for logical operations. Use the Boolean class to retrieve
    /// the primitive data type or string representation of a Boolean object.
    /// </summary>
    [PageFX.AbcInstance(4)]
    [PageFX.ABC]
    [PageFX.QName("Boolean")]
    [PageFX.FP9]
    public class Boolean : Avm.Object
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Boolean(object value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Boolean();

        /// <summary>
        /// Returns the string representation (&quot;true&quot; or &quot;false&quot;)
        /// of the Boolean object. The output is not localized, and is &quot;true&quot; or
        /// &quot;false&quot; regardless of the system language.
        /// </summary>
        /// <returns>
        /// The string &quot;true&quot;
        /// or &quot;false&quot;.
        /// </returns>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.QName("toString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();

        /// <summary>
        /// Returns true if the value of the specified Boolean object is true;
        /// false otherwise.
        /// </summary>
        /// <returns>A Boolean value.</returns>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.QName("valueOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool valueOf();
    }
}
