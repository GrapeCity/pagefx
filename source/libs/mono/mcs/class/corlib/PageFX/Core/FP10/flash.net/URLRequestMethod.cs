using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// The URLRequestMethod class provides values that specify whether the URLRequest object should
    /// use the POST  method or the GET  method when sending data to a server.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class URLRequestMethod : Avm.Object
    {
        /// <summary>Specifies that the URLRequest object is a POST.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String POST;

        /// <summary>Specifies that the URLRequest object is a GET.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String GET;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern URLRequestMethod();
    }
}
