using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// The URLRequest class captures all of the information in a single HTTP request. URLRequest
    /// objects are passed to the load()  methods of URLStream, URLLoader, Loader and
    /// other loading operations to initiate URL downloads, as well as to the upload()
    /// and download()  methods of the FileReference class.
    /// </summary>
    [PageFX.AbcInstance(313)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class URLRequest : Avm.Object
    {
        /// <summary>
        /// The URL to be requested.
        /// For content running in Flash Player, by default, the URL must be in exactly the same domain
        /// as the calling SWF file, including subdomains. For example, SWF files at
        /// www.adobe.com and store.adobe.com are in different domains.
        /// To load data from a different domain, put a cross-domain policy file on the server
        /// that is hosting the SWF file. For more information, see the security documentation
        /// described in the URLRequest class description.For content running in Apollo, files in the application security domain
        ///  files installed with the Apollo application  can access URLs using any of the
        /// following URL schemes:http and httpsfileapp-storageapp-resourceContent running in Apollo that is not in  the application security domain
        /// observes the same restrictions as content running in Flash Player (in the browser), and
        /// loading is governed by the content&apos;s domain and any permissions granted in cross-domain
        /// policy files.
        /// </summary>
        public extern virtual Avm.String url
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// An object containing data to be transmitted with the URL request.
        /// This property is used with the method property.
        /// If the value of URLRequest.method is POST,
        /// the data is transmitted with the URLRequest object with the HTTP POST method.If the value of URLRequest.method is GET,
        /// the data defines variables to be sent with the URLRequest object with
        /// the HTTP GET method.The URLRequest API offers binary POST support and support for URL-encoded variables,
        /// as well as support for strings. The data object can be of  ByteArray, URLVariables,
        /// or String type.The way in which the data is used depends on the type of object used:If the object is a ByteArray object, the binary
        /// data of the ByteArray object is used as POST data. For GET, data of ByteArray type
        /// is not supported. Also, data of ByteArray type is not supported for
        /// FileReference.upload() and FileReference.download().If the object is a URLVariables object and the method is POST,
        /// the variables are encoded using x-www-form-urlencoded format
        /// and the resulting string is used as POST data. An exception is a call to
        /// FileReference.upload(), in which the variables are sent as separate fields in
        /// a multipart/form-data post.If the object is a URLVariables object and the method is GET,
        /// the URLVariables object defines variables to be sent with the URLRequest object.Otherwise, the object is converted to a string, and the string
        /// is used as the POST or GET data.This data is not sent until a method, such as navigateToURL()
        /// or FileReference.upload(), uses the URLRequest object.
        /// </summary>
        public extern virtual object data
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Controls the HTTP form submission method.
        /// For SWF content running in Flash Player (in the browser), this property is
        /// limitted to GET or POST operation, and valid values are
        /// URLRequestMethod.GET or URLRequestMethod.POST.
        /// For content running in the Apollo runtime, if the content is in the
        /// application security domain, you can use any string value; otherwise
        /// (if the content is not in the Apollo application security domain)
        /// you are still restricted to using GET or POST.
        /// </summary>
        public extern virtual Avm.String method
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The MIME content type of any POST data.
        /// Note:The FileReference.upload() and
        /// FileReference.download() methods do not
        /// support the URLRequest.contentType parameter.
        /// </summary>
        public extern virtual Avm.String contentType
        {
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The array of HTTP request headers to be appended to the
        /// HTTP request. The array is composed of URLRequestHeader objects.
        /// Each object in the array must be a URLRequestHeader object that
        /// contains a name string and a value string, as follows:
        /// var rhArray:Array = new Array(new URLRequestHeader(&quot;Content-Type&quot;, &quot;text/html&quot;));
        /// Flash Player imposes certain restrictions on request headers; for more information,
        /// see the URLRequestHeader class description.The FileReference.upload() and FileReference.download()
        /// methods do not
        /// support the URLRequest.requestHeaders parameter.
        /// </summary>
        public extern virtual Avm.Array requestHeaders
        {
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String digest
        {
            [PageFX.AbcInstanceTrait(13)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(14)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern URLRequest(Avm.String url);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern URLRequest();


    }
}
