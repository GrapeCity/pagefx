using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// The XMLList class contains methods for working with one or more XML elements. An XMLList object
    /// can represent one or more XML objects or elements (including multiple nodes or attributes), so
    /// you can call methods on the elements as a group or on the individual elements in the collection.
    /// </summary>
    [PageFX.AbcInstance(174)]
    [PageFX.ABC]
    [PageFX.QName("XMLList")]
    [PageFX.FP9]
    public class XMLList : Avm.Object
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern XMLList(object value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern XMLList();

        /// <summary>
        /// Returns a string representation of all the XML objects in an XMLList object. The rules for
        /// this conversion depend on whether the XML object has simple content or complex content:
        /// If the XML object has simple content, toString() returns the string contents of the
        /// XML object with  the following stripped out: the start tag, attributes, namespace declarations, and
        /// end tag. If the XML object has complex content, toString() returns an XML encoded string
        /// representing the entire XML object, including the start tag, attributes, namespace declarations,
        /// and end tag.To return the entire XML object every time, use the toXMLString() method.
        /// </summary>
        /// <returns>The string representation of the XML object.</returns>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.QName("toString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();

        /// <summary>Returns the XMLList object.</summary>
        /// <returns>Returns the current XMLList object.</returns>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.QName("valueOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.XMLList valueOf();

        /// <summary>Checks for the property specified by p.</summary>
        /// <param name="p">The property to match.</param>
        /// <returns>If the parameter exists, then true; otherwise false.</returns>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("hasOwnProperty", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override bool hasOwnProperty(object p);

        /// <summary>
        /// Checks whether the property p is in the set of properties that can be iterated in a for..in statement
        /// applied to the XMLList object. This is true only if toNumber(p) is greater than or equal to 0
        /// and less than the length of the XMLList object.
        /// </summary>
        /// <param name="p">The index of a property to check.</param>
        /// <returns>If the property can be iterated in a for..in statement, then true; otherwise false.</returns>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.QName("propertyIsEnumerable", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override bool propertyIsEnumerable(object p);

        /// <summary>
        /// Calls the attribute() method of each XML object and returns an XMLList object
        /// of the results. The results match the given attributeName parameter. If there is no
        /// match, the attribute() method returns an empty XMLList object.
        /// </summary>
        /// <param name="attributeName">The name of the attribute that you want to include in an XMLList object.</param>
        /// <returns>An XMLList object of matching XML objects or an empty XMLList object.</returns>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("attribute", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.XMLList attribute(object attributeName);

        /// <summary>
        /// Calls the attributes() method of each XML object and
        /// returns an XMLList object of attributes for each XML object.
        /// </summary>
        /// <returns>An XMLList object of attributes for each XML object.</returns>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.QName("attributes", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.XMLList attributes();

        /// <summary>
        /// Calls the child() method of each XML object and returns an XMLList object that
        /// contains the results in order.
        /// </summary>
        /// <param name="propertyName">The element name or integer of the XML child.</param>
        /// <returns>An XMLList object of child nodes that match the input parameter.</returns>
        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.QName("child", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.XMLList child(object propertyName);

        /// <summary>
        /// Calls the children() method of each XML object and
        /// returns an XMLList object that contains the results.
        /// </summary>
        /// <returns>An XMLList object of the children in the XML objects.</returns>
        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.QName("children", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.XMLList children();

        /// <summary>
        /// Calls the comments() method of each XML object and returns
        /// an XMLList of comments.
        /// </summary>
        /// <returns>An XMLList of the comments in the XML objects.</returns>
        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.QName("comments", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.XMLList comments();

        /// <summary>
        /// Checks whether the XMLList object contains an XML object that is equal to the given
        /// value parameter.
        /// </summary>
        /// <param name="value">An XML object to compare against the current XMLList object.</param>
        /// <returns>
        /// If the XMLList contains the XML object declared in the value parameter,
        /// then true; otherwise false.
        /// </returns>
        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("contains", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool contains(object value);

        /// <summary>
        /// Returns a copy of the given XMLList object. The copy is a duplicate of the entire tree of nodes.
        /// The copied XML object has no parent and returns null if you attempt to call the parent() method.
        /// </summary>
        /// <returns>The copy of the XMLList object.</returns>
        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.QName("copy", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.XMLList copy();

        /// <summary>
        /// Returns all descendants (children, grandchildren, great-grandchildren, and so on) of the XML object
        /// that have the given name parameter. The name parameter can be a
        /// QName object, a String data type, or any other data type that is then converted to a String
        /// data type.
        /// To return all descendants, use
        /// the asterisk (*) parameter. If no parameter is passed,
        /// the string &quot;*&quot; is passed and returns all descendants of the XML object.
        /// </summary>
        /// <param name="name">(default = *)  The name of the element to match.</param>
        /// <returns>
        /// An XMLList object of the matching descendants (children, grandchildren, and so on) of the XML objects
        /// in the original list. If there are no descendants, returns an empty XMLList object.
        /// </returns>
        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("descendants", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.XMLList descendants(object name);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("descendants", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.XMLList descendants();

        /// <summary>
        /// Calls the elements() method of each XML object. The name parameter is
        /// passed to the descendants() method. If no parameter is passed, the string &quot;*&quot; is passed to the
        /// descendants() method.
        /// </summary>
        /// <param name="name">(default = *)  The name of the elements to match.</param>
        /// <returns>An XMLList object of the matching child elements of the XML objects.</returns>
        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.QName("elements", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.XMLList elements(object name);

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.QName("elements", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.XMLList elements();

        /// <summary>
        /// Checks whether the XMLList object contains complex content. An XMLList object is
        /// considered to contain complex content if it is not empty and either of the following conditions is true:
        /// The XMLList object contains a single XML item with complex content.The XMLList object contains elements.
        /// </summary>
        /// <returns>If the XMLList object contains complex content, then true; otherwise false.</returns>
        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.QName("hasComplexContent", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool hasComplexContent();

        /// <summary>
        /// Checks whether the XMLList object contains simple content. An XMLList object is
        /// considered to contain simple content if one or more of the following
        /// conditions is true:
        /// The XMLList object is emptyThe XMLList object contains a single XML item with simple contentThe XMLList object contains no elements
        /// </summary>
        /// <returns>If the XMLList contains simple content, then true; otherwise false.</returns>
        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("hasSimpleContent", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool hasSimpleContent();

        /// <summary>Returns the number of properties in the XMLList object.</summary>
        /// <returns>The number of properties in the XMLList object.</returns>
        [PageFX.AbcInstanceTrait(15)]
        [PageFX.ABC]
        [PageFX.QName("length", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int length();

        [PageFX.AbcInstanceTrait(16)]
        [PageFX.ABC]
        [PageFX.QName("name", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object name();

        /// <summary>
        /// Merges adjacent text nodes and eliminates empty text nodes for each
        /// of the following: all text nodes in the XMLList, all the XML objects
        /// contained in the XMLList, and the descendants of all the XML objects in
        /// the XMLList.
        /// </summary>
        /// <returns>The normalized XMLList object.</returns>
        [PageFX.AbcInstanceTrait(17)]
        [PageFX.ABC]
        [PageFX.QName("normalize", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.XMLList normalize();

        /// <summary>
        /// Returns the parent of the XMLList object if all items in the XMLList object have the same parent.
        /// If the XMLList object has no parent or different parents, the method returns undefined.
        /// </summary>
        /// <returns>Returns the parent XML object.</returns>
        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("parent", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object parent();

        /// <summary>
        /// If a name parameter is provided, lists all the children of the XMLList object that
        /// contain processing instructions with that name. With no parameters, the method lists all the
        /// children of the XMLList object that contain any processing instructions.
        /// </summary>
        /// <param name="name">(default = &quot;*&quot;)  The name of the processing instructions to match.</param>
        /// <returns>An XMLList object that contains the processing instructions for each XML object.</returns>
        [PageFX.AbcInstanceTrait(19)]
        [PageFX.ABC]
        [PageFX.QName("processingInstructions", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.XMLList processingInstructions(object name);

        [PageFX.AbcInstanceTrait(19)]
        [PageFX.ABC]
        [PageFX.QName("processingInstructions", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.XMLList processingInstructions();

        /// <summary>
        /// Calls the text() method of each XML
        /// object and returns an XMLList object that contains the results.
        /// </summary>
        /// <returns>An XMLList object of all XML properties of the XMLList object that represent XML text nodes.</returns>
        [PageFX.AbcInstanceTrait(20)]
        [PageFX.ABC]
        [PageFX.QName("text", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.XMLList text();

        /// <summary>
        /// Returns a string representation of all the XML objects in an XMLList object.
        /// Unlike the toString() method, the toXMLString()
        /// method always returns the start tag, attributes,
        /// and end tag of the XML object, regardless of whether the XML object has simple content
        /// or complex content. (The toString() method strips out these items for XML
        /// objects that contain simple content.)
        /// </summary>
        /// <returns>The string representation of the XML object.</returns>
        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.QName("toXMLString", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toXMLString();

        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.QName("addNamespace", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.XML addNamespace(object ns);

        [PageFX.AbcInstanceTrait(23)]
        [PageFX.ABC]
        [PageFX.QName("appendChild", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.XML appendChild(object child);

        [PageFX.AbcInstanceTrait(24)]
        [PageFX.ABC]
        [PageFX.QName("childIndex", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int childIndex();

        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.QName("inScopeNamespaces", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Array inScopeNamespaces();

        [PageFX.AbcInstanceTrait(26)]
        [PageFX.ABC]
        [PageFX.QName("insertChildAfter", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object insertChildAfter(object child1, object child2);

        [PageFX.AbcInstanceTrait(27)]
        [PageFX.ABC]
        [PageFX.QName("insertChildBefore", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object insertChildBefore(object child1, object child2);

        [PageFX.AbcInstanceTrait(28)]
        [PageFX.ABC]
        [PageFX.QName("nodeKind", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String nodeKind();

        [PageFX.AbcInstanceTrait(30)]
        [PageFX.ABC]
        [PageFX.QName("namespace", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object @namespace(object prefix);

        [PageFX.AbcInstanceTrait(30)]
        [PageFX.ABC]
        [PageFX.QName("namespace", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object @namespace();

        [PageFX.AbcInstanceTrait(31)]
        [PageFX.ABC]
        [PageFX.QName("localName", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object localName();

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.QName("namespaceDeclarations", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Array namespaceDeclarations();

        [PageFX.AbcInstanceTrait(33)]
        [PageFX.ABC]
        [PageFX.QName("prependChild", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.XML prependChild(object value);

        [PageFX.AbcInstanceTrait(34)]
        [PageFX.ABC]
        [PageFX.QName("removeNamespace", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.XML removeNamespace(object ns);

        [PageFX.AbcInstanceTrait(35)]
        [PageFX.ABC]
        [PageFX.QName("replace", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.XML replace(object propertyName, object value);

        [PageFX.AbcInstanceTrait(36)]
        [PageFX.ABC]
        [PageFX.QName("setChildren", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.XML setChildren(object value);

        [PageFX.AbcInstanceTrait(37)]
        [PageFX.ABC]
        [PageFX.QName("setLocalName", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setLocalName(object name);

        [PageFX.AbcInstanceTrait(38)]
        [PageFX.ABC]
        [PageFX.QName("setName", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setName(object name);

        [PageFX.AbcInstanceTrait(39)]
        [PageFX.ABC]
        [PageFX.QName("setNamespace", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setNamespace(object ns);

        #region Custom Members
        public extern XML this[int index]
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }
        
        public extern XMLList this[Avm.String name]
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }
        
        public extern XMLList this[Avm.Namespace ns, Avm.String name]
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }
        
        public extern XMLList this[Avm.String ns, Avm.String name]
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }
        #endregion



    }
}
