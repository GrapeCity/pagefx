using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// The Namespace class contains methods and properties for defining and working with namespaces.
    /// There are three scenarios for using namespaces:
    /// Namespaces of XML objects Namespaces associate a namespace prefix with a Uniform Resource Identifier (URI)
    /// that identifies the namespace. The prefix is a string used to reference the namespace within an
    /// XML object. If the prefix is undefined, when the XML is converted to a string, a prefix is
    /// automatically generated.
    /// Namespace to differentiate methods Namespaces can differentiate methods with the same name to perform different tasks.
    /// If two methods have the same name but separate namespaces, they can perform different tasks.
    /// Namespaces for access control
    /// Namespaces can be used to control access to a group of
    /// properties and methods in a class. If you place the
    /// properties and methods into a private
    /// namespace, they are
    /// inaccessible to any code that does not have access to
    /// that namespace. You can grant access to the group of
    /// properties and methods by passing the namespace to
    /// other classes, methods or functions.
    /// </summary>
    [PageFX.ABC]
    [PageFX.QName("Namespace")]
    [PageFX.FP9]
    public class Namespace : Object
    {
        public extern virtual object prefix
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual String uri
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Namespace(object arg0, object arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Namespace(object arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Namespace();

        /// <summary>Returns the URI value of the specified object.</summary>
        /// <returns>The Uniform Resource Identifier (URI) of the namespace, as a string.</returns>
        [PageFX.ABC]
        [PageFX.QName("valueOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String valueOf();

        /// <summary>Equivalent to the Namespace.uri property.</summary>
        /// <returns>The Uniform Resource Identifier (URI) of the namespace, as a string.</returns>
        [PageFX.ABC]
        [PageFX.QName("toString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String toString();


    }
}
