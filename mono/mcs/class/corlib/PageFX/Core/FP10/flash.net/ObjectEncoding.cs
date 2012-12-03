using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// The ObjectEncoding class allows classes that serialize objects
    /// (such as FileStream, NetStream, NetConnection, SharedObject,
    /// and ByteArray) to work with prior versions of ActionScript.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class ObjectEncoding : Avm.Object
    {
        /// <summary>
        /// Specifies the default (latest) format for the current player. Because object encoding control is
        /// only available in Flash® Player 9 and later and
        /// the Adobe® Integrated Runtime (AIR™), the earliest format used will be
        /// the Action Message Format for ActionScript 3.0.
        /// For example, if an object has the objectEncoding property set to
        /// ObjectEncoding.DEFAULT, AMF3 encoding is used.
        /// If, in the future, a later version of Flash Player or
        /// the Adobe Integrated Runtime introduces a new AMF version
        /// and you republish your content, the player will use that new AMF version.
        /// You can use this constant only if you&apos;re not concerned at all about interoperability
        /// with previous versions.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint DEFAULT;

        /// <summary>Specifies that objects are serialized using the Action Message Format for ActionScript 1.0 and 2.0.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint AMF0;

        /// <summary>Specifies that objects are serialized using the Action Message Format for ActionScript 3.0.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint AMF3;

        public extern static IDynamicPropertyWriter dynamicPropertyWriter
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ObjectEncoding();


    }
}
