using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// QName objects represent qualified names of XML elements and attributes. Each QName
    /// object has a local name and a namespace Uniform Resource Identifier (URI). When
    /// the value of the namespace URI is null , the QName object matches any
    /// namespace. Use the QName constructor to create a new QName object that is either
    /// a copy of another QName object or a new QName object with a uri  from
    /// a Namespace object and a localName  from a QName object.
    /// </summary>
    [PageFX.ABC]
    [PageFX.QName("QName")]
    [PageFX.FP9]
    public class QName : Object
    {
        public extern virtual object uri
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual String localName
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern QName(object arg0, object arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern QName(object arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern QName();

        /// <summary>Returns the QName object.</summary>
        /// <returns>
        /// The primitive value of a QName
        /// instance.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("valueOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual QName valueOf();

        /// <summary>
        /// Returns a string composed of the URI, and the local name for the QName object, separated
        /// by &quot;::&quot;.
        /// The format depends on the uri property of the QName object:If uri == &quot;&quot;
        /// toString returns localName
        /// else if uri == null
        /// toString returns *::localName
        /// else
        /// toString returns uri::localName
        /// </summary>
        /// <returns>The qualified name, as a string.</returns>
        [PageFX.ABC]
        [PageFX.QName("toString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String toString();


    }
}
