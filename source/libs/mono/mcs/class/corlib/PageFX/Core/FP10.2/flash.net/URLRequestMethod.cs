using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// The URLRequestMethod class provides values that specify whether the URLRequest object should
    /// use the POST  method or the GET  method when sending data to a server.
    /// </summary>
    [PageFX.AbcInstance(201)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class URLRequestMethod : Avm.Object
    {
        /// <summary>Specifies that the URLRequest object is a POST.</summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String POST;

        /// <summary>Specifies that the URLRequest object is a GET.</summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String GET;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String PUT;

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String DELETE;

        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String HEAD;

        [PageFX.AbcClassTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String OPTIONS;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern URLRequestMethod();
    }
}
