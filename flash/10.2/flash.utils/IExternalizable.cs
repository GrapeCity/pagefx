using System;
using System.Runtime.CompilerServices;

namespace flash.utils
{
    /// <summary>
    /// The IExternalizable interface provides control over serialization of a class as it is encoded
    /// into a data stream. The writeExternal()  and
    /// readExternal()  methods of the IExternalizable interface are implemented by a class to allow customization
    /// of the contents and format of the data stream (but not the classname or type) for an object and its supertypes.
    /// Each individual class must serialize and reconstruct the state of its instances. These methods must be symmetrical with
    /// the supertype to save its state. These methods supercede the native Action Message Format (AMF) serialization behavior.
    /// </summary>
    [PageFX.AbcInstance(343)]
    [PageFX.ABC]
    [PageFX.FP9]
    public interface IExternalizable
    {
        /// <summary>
        /// A class implements this method to encode itself for a data stream by calling the methods of the IDataOutput
        /// interface.
        /// </summary>
        /// <param name="output">The name of the class that implements the IDataOutput interface.</param>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.QName("writeExternal", "flash.utils:IExternalizable", "public")]
        [PageFX.FP9]
        void writeExternal(flash.utils.IDataOutput output);

        /// <summary>
        /// A class implements this method to decode itself from a data stream by calling the methods of the IDataInput
        /// interface. This method must read the values in the same sequence and with the same types as
        /// were written by the writeExternal() method.
        /// </summary>
        /// <param name="input">The name of the class that implements the IDataInput interface.</param>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.QName("readExternal", "flash.utils:IExternalizable", "public")]
        [PageFX.FP9]
        void readExternal(flash.utils.IDataInput input);
    }
}
