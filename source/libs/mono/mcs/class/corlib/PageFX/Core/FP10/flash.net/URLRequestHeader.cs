using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// A URLRequestHeader object encapsulates a single HTTP request header
    /// and consists of a name/value pair.
    /// URLRequestHeader objects are used in the requestHeaders  property of the URLRequest class.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class URLRequestHeader : Avm.Object
    {
        /// <summary>The value associated with the name property (such as text/plain).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String value;

        /// <summary>An HTTP request header name (such as Content-Type or SOAPAction).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String name;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern URLRequestHeader(Avm.String arg0, Avm.String arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern URLRequestHeader(Avm.String arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern URLRequestHeader();
    }
}
