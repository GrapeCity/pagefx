using System;
using System.Runtime.CompilerServices;

namespace flash.xml
{
    /// <summary>
    /// The XMLDocument class represents the legacy XML object
    /// that was present in ActionScript 2.0. It was renamed in ActionScript 3.0
    /// to XMLDocument to avoid name conflicts with the new
    /// XML class in ActionScript 3.0. In ActionScript 3.0,
    /// it is recommended that you use the new
    /// XML  class and related classes,
    /// which support E4X (ECMAScript for XML).
    /// </summary>
    [PageFX.AbcInstance(310)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class XMLDocument : flash.xml.XMLNode
    {
        /// <summary>
        /// A string that specifies information about a document&apos;s XML declaration.
        /// After the XML document is parsed into an XMLDocument object, this property is set
        /// to the text of the document&apos;s XML declaration. This property is set using a string
        /// representation of the XML declaration, not an XMLNode object. If no XML declaration
        /// is encountered during a parse operation, the property is set to null.
        /// The XMLDocument.toString() method outputs the contents of the
        /// XML.xmlDecl property before any other text in the XML object.
        /// If the XML.xmlDecl property contains null,
        /// no XML declaration is output.
        /// </summary>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public object xmlDecl;

        /// <summary>
        /// Specifies information about the XML document&apos;s DOCTYPE declaration.
        /// After the XML text has been parsed into an XMLDocument object, the
        /// XMLDocument.docTypeDecl property of the XMLDocument object is set to the
        /// text of the XML document&apos;s DOCTYPE declaration
        /// (for example, &lt;!DOCTYPEgreeting SYSTEM &quot;hello.dtd&quot;&gt;).
        /// This property is set using a string representation of the DOCTYPE declaration,
        /// not an XMLNode object.
        /// The legacy ActionScript XML parser is not a validating parser. The DOCTYPE
        /// declaration is read by the parser and stored in the XMLDocument.docTypeDecl property,
        /// but no DTD validation is performed.If no DOCTYPE declaration was encountered during a parse operation,
        /// the XMLDocument.docTypeDecl property is set to null.
        /// The XML.toString() method outputs the contents of XML.docTypeDecl
        /// immediately after the XML declaration stored in XML.xmlDecl, and before any other
        /// text in the XML object. If XMLDocument.docTypeDecl is null, no
        /// DOCTYPE declaration is output.
        /// </summary>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public object docTypeDecl;

        /// <summary>
        /// An Object containing the nodes of the XML that have an id attribute assigned.
        /// The names of the properties of the object (each containing a node) match the values of the
        /// id attributes.
        /// Consider the following XMLDocument object:
        /// &lt;employee id=&apos;41&apos;&gt;
        /// &lt;name&gt;
        /// John Doe
        /// &lt;/name&gt;
        /// &lt;address&gt;
        /// 601 Townsend St.
        /// &lt;/address&gt;
        /// &lt;/employee&gt;
        /// &lt;employee id=&apos;42&apos;&gt;
        /// &lt;name&gt;
        /// Jane Q. Public
        /// &lt;/name&gt;
        /// &lt;/employee&gt;
        /// &lt;department id=&quot;IT&quot;&gt;
        /// Information Technology
        /// &lt;/department&gt;
        /// In this example, the idMap property for this XMLDocument object is an Object with
        /// three properties: 41, 42, and IT. Each of these
        /// properties is an XMLNode that has the matching id value. For example,
        /// the IT property of the idMap object is this node:
        /// &lt;department id=&quot;IT&quot;&gt;
        /// Information Technology
        /// &lt;/department&gt;
        /// You must use the parseXML() method on the XMLDocument object for the
        /// idMap property to be instantiated.If there is more than one XMLNode with the same id value, the matching property
        /// of the idNode object is that of the last node parsed. For example:
        /// var x1:XML = new XMLDocument(&quot;&lt;a id=&apos;1&apos;&gt;&lt;b id=&apos;2&apos; /&gt;&lt;c id=&apos;1&apos; /&gt;&lt;/a&gt;&quot;);
        /// x2 = new XMLDocument();
        /// x2.parseXML(x1);
        /// trace (x2.idMap[&apos;1&apos;]);
        /// This will output the &lt;c&gt; node:
        /// &lt;c id=&apos;1&apos; /&gt;
        /// </summary>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public object idMap;

        /// <summary>
        /// When set to true, text nodes that contain only white space are discarded during the parsing process. Text nodes with leading or trailing white space are unaffected. The default setting is false.
        /// You can set the ignoreWhite property for individual XMLDocument objects, as the following code shows:
        /// my_xml.ignoreWhite = true;
        /// </summary>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public bool ignoreWhite;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern XMLDocument(Avm.String source);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern XMLDocument();

        /// <summary>
        /// Creates a new XMLNode object with the name specified in the parameter.
        /// The new node initially has no parent, no children, and no siblings.
        /// The method returns a reference to the newly created XMLNode object
        /// that represents the element. This method and the XMLDocument.createTextNode()
        /// method are the constructor methods for creating nodes for an XMLDocument object.
        /// </summary>
        /// <param name="name">The tag name of the XMLDocument element being created.</param>
        /// <returns>An XMLNode object.</returns>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.xml.XMLNode createElement(Avm.String name);

        /// <summary>Creates a new XML text node with the specified text. The new node initially has no parent, and text nodes cannot have children or siblings. This method returns a reference to the XMLDocument object that represents the new text node. This method and the XMLDocument.createElement() method are the constructor methods for creating nodes for an XMLDocument object.</summary>
        /// <param name="text">The text used to create the new text node.</param>
        /// <returns>An XMLNode object.</returns>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.xml.XMLNode createTextNode(Avm.String text);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();

        /// <summary>
        /// Parses the XML text specified in the value parameter
        /// and populates the specified XMLDocument object with the resulting XML tree. Any existing trees in the XMLDocument object are discarded.
        /// </summary>
        /// <param name="source">The XML text to be parsed and passed to the specified XMLDocument object.</param>
        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void parseXML(Avm.String source);
    }
}
