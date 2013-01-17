using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// This interface controls the serialization of dynamic properties of dynamic objects.
    /// You use this interface with the IDynamicPropertyWriter interface
    /// and the ObjectEncoding.dynamicPropertyWriter  property.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public interface IDynamicPropertyOutput
    {
        /// <summary>
        /// Adds a dynamic property to the binary output of a serialized object.
        /// When the object is subsequently read (using a method such as
        /// readObject), it contains the new property.
        /// You can use this method
        /// to exclude properties of dynamic objects from serialization; to write values
        /// to properties of dynamic objects; or to create new properties
        /// for dynamic objects.
        /// </summary>
        /// <param name="arg0">
        /// The name of the property. You can use this parameter either to specify
        /// the name of an existing property of the dynamic object or to create a
        /// new property.
        /// </param>
        /// <param name="arg1">The value to write to the specified property.</param>
        [PageFX.ABC]
        [PageFX.QName("writeDynamicProperty", "flash.net:IDynamicPropertyOutput", "public")]
        [PageFX.FP9]
        void writeDynamicProperty(Avm.String arg0, object arg1);
    }
}
