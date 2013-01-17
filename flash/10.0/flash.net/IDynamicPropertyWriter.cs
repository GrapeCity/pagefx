using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// This interface is used with the IDynamicPropertyOutput interface to control
    /// the serialization of dynamic properties of dynamic objects. To use this interface,
    /// assign an object that implements the IDynamicPropertyWriter interface to
    /// the ObjectEncoding.dynamicPropertyWriter  property.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public interface IDynamicPropertyWriter
    {
        /// <summary>
        /// Writes the name and value of an IDynamicPropertyOutput object to an object with
        /// dynamic properties. If ObjectEncoding.dynamicPropertyWriter is set,
        /// this method is invoked for each object with dynamic properties.
        /// </summary>
        /// <param name="arg0">The object to write to.</param>
        /// <param name="arg1">
        /// The IDynamicPropertyOutput object that contains the name and value
        /// to dynamically write to the object.
        /// </param>
        [PageFX.ABC]
        [PageFX.QName("writeDynamicProperties", "flash.net:IDynamicPropertyWriter", "public")]
        [PageFX.FP9]
        void writeDynamicProperties(object arg0, IDynamicPropertyOutput arg1);
    }
}
