using System;
using System.Runtime.CompilerServices;

namespace flash.xml
{
    /// <summary>
    /// The XMLNode class represents the legacy XML object
    /// that was present in ActionScript 2.0 and that was renamed in ActionScript 3.0.
    /// In ActionScript 3.0, consider using the new top-level XML
    /// class and related classes instead,
    /// which support E4X (ECMAScript for XML).
    /// The XMLNode class is present for backward compatibility.
    /// </summary>
    [PageFX.AbcInstance(283)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class XMLNode : Avm.Object
    {
        /// <summary>
        /// A nodeType constant value, either XMLNodeType.ELEMENT_NODE for an XML element or
        /// XMLNodeType.TEXT_NODE for a text node.
        /// The nodeType is a numeric value from the NodeType enumeration in the W3C DOM
        /// Level 1 recommendation:
        /// http://www.w3.org/tr/1998/REC-DOM-Level-1-19981001/level-one-core.html.
        /// The following table lists the values:Integer valueDefined
        /// constant1ELEMENT_NODE2ATTRIBUTE_NODE3TEXT_NODE4CDATA_SECTION_NODE5ENTITY_REFERENCE_NODE6ENTITY_NODE7PROCESSING_INSTRUCTION_NODE8COMMENT_NODE9DOCUMENT_NODE10DOCUMENT_TYPE_NODE11DOCUMENT_FRAGMENT_NODE12NOTATION_NODEIn Flash Player, the built-in XMLNode class only supports XMLNodeType.ELEMENT_NODE and
        /// XMLNodeType.TEXT_NODE.
        /// </summary>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public uint nodeType;

        /// <summary>
        /// An XMLNode value that references the previous sibling in the parent node&apos;s child list.
        /// The property has a value of null if the node does not have a previous sibling node. This property
        /// cannot be used to manipulate child nodes; use the appendChild(),
        /// insertBefore(), and removeNode() methods to manipulate child nodes.
        /// </summary>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public flash.xml.XMLNode previousSibling;

        /// <summary>
        /// An XMLNode value that references the next sibling in the parent node&apos;s child list. This property is
        /// null if the node does not have a next sibling node. This property cannot be used to
        /// manipulate child nodes; use the appendChild(), insertBefore(), and
        /// removeNode() methods to manipulate child nodes.
        /// </summary>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public flash.xml.XMLNode nextSibling;

        /// <summary>
        /// An XMLNode value that references the parent node of the specified XML object, or returns
        /// null if the node has no parent. This is a read-only property and cannot be used to
        /// manipulate child nodes; use the appendChild(), insertBefore(), and
        /// removeNode() methods to manipulate child nodes.
        /// </summary>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public flash.xml.XMLNode parentNode;

        /// <summary>
        /// Evaluates the specified XMLDocument object and references the first child in the parent node&apos;s child list.
        /// This property is null if the node does not have children. This property is
        /// undefined if the node is a text node. This is a read-only property and cannot be used
        /// to manipulate child nodes; use the appendChild(), insertBefore(), and
        /// removeNode() methods to manipulate child nodes.
        /// </summary>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        public flash.xml.XMLNode firstChild;

        /// <summary>
        /// An XMLNode value that references the last child in the node&apos;s child list. The
        /// XMLNode.lastChild property is null if the node does not have children.
        /// This property cannot be used to manipulate child nodes; use the appendChild(),
        /// insertBefore(), and removeNode() methods to manipulate child nodes.
        /// </summary>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        public flash.xml.XMLNode lastChild;

        /// <summary>
        /// A string representing the node name of the XMLNode object. If the XMLNode object is an XML
        /// element (nodeType == 1), nodeName is the name of the tag that
        /// represents the node in the XML file. For example, TITLE is the nodeName
        /// of an HTML TITLE tag. If the XMLNode object is a text node
        /// (nodeType == 3), nodeName is null.
        /// </summary>
        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String nodeName;

        /// <summary>
        /// The node value of the XMLDocument object. If the XMLDocument object is a text node, the nodeType
        /// is 3, and the nodeValue is the text of the node. If the XMLDocument object is an XML element
        /// (nodeType is 1), nodeValue is null and read-only.
        /// </summary>
        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String nodeValue;

        /// <summary>
        /// An array of the specified XMLNode object&apos;s children. Each element in the array is a reference
        /// to an XMLNode object that represents a child node. This is a read-only property and cannot be
        /// used to manipulate child nodes. Use the appendChild(), insertBefore(),
        /// and removeNode()  methods to manipulate child nodes.
        /// This property is undefined for text nodes (nodeType == 3).
        /// </summary>
        public extern virtual Avm.Array childNodes
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// An object containing all of the attributes of the specified XMLNode instance. The
        /// XMLNode.attributes object contains one variable for each attribute of the XMLNode instance.
        /// Because these variables are defined as part of the object, they are generally referred to as
        /// properties of the object. The value of each attribute is stored in the corresponding property as a
        /// string. For example, if you have an attribute named color, you would retrieve
        /// that attribute&apos;s value
        /// by specifying color as the property name, as the following code shows:
        /// var myColor:String = doc.firstChild.attributes.color
        /// </summary>
        public extern virtual object attributes
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The local name portion of the XML node&apos;s name. This is the element name without
        /// the namespace prefix. For example, the node
        /// &lt;contact:mailbox/&gt;bob@example.com&lt;/contact:mailbox&gt;
        /// has the local name &quot;mailbox&quot;, and the prefix &quot;contact&quot;, which comprise the full
        /// element name &quot;contact.mailbox&quot;.
        /// You can access the namespace prefix through the prefix property of
        /// the XML node object. The nodeName property returns the full name
        /// (including the prefix and the local name).
        /// </summary>
        public extern virtual Avm.String localName
        {
            [PageFX.AbcInstanceTrait(22)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The prefix portion of the XML node name. For example, the node
        /// &lt;contact:mailbox/&gt;bob@example.com&lt;/contact:mailbox&gt; prefix
        /// &quot;contact&quot; and the local name &quot;mailbox&quot;, which comprise the full element name &quot;contact.mailbox&quot;.
        /// The nodeName property of an XML node object returns the full name
        /// (including the prefix and the  local name). You can access the local name portion of the
        /// element&apos;s name via the localName property.
        /// </summary>
        public extern virtual Avm.String prefix
        {
            [PageFX.AbcInstanceTrait(23)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// If the XML node has a prefix, namespaceURI is the value of the xmlns
        /// declaration for that prefix (the URI), which is typically called the namespace URI.
        /// The xmlns declaration is in the current node or in a node higher in the XML
        /// hierarchy.
        /// If the XML node does not have a prefix, the value of the namespaceURI property
        /// depends on whether there is a default namespace defined (as in
        /// xmlns=&quot;http://www.example.com/&quot;). If there is a default namespace, the value of
        /// the namespaceURI property is the value of the default namespace.
        /// If there is no default namespace, the namespaceURI property for
        /// that node is an empty string (&quot;&quot;).You can use the getNamespaceForPrefix() method to identify the namespace associated with a
        /// specific prefix. The namespaceURI property returns the prefix associated with the node name.
        /// </summary>
        public extern virtual Avm.String namespaceURI
        {
            [PageFX.AbcInstanceTrait(24)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern XMLNode(uint type, Avm.String value);

        /// <summary>
        /// Indicates whether the specified XMLNode object has child nodes. This property is true if the
        /// specified XMLNode object has child nodes; otherwise, it is false.
        /// </summary>
        /// <returns>
        /// Returns true if the
        /// specified XMLNode object has child nodes; otherwise, false.
        /// </returns>
        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool hasChildNodes();

        /// <summary>
        /// Constructs and returns a new XML node of the same type, name, value, and attributes as the
        /// specified XML object. If deep is set to true, all child nodes are
        /// recursively cloned, resulting in an exact copy of the original object&apos;s document tree.
        /// The clone of the node that is returned is no longer associated with the tree of the cloned item.
        /// Consequently, nextSibling, parentNode, and previousSibling
        /// all have a value of null. If the deep parameter is set to
        /// false, or the my_xml node has no child nodes,
        /// firstChild and lastChild are also null.
        /// </summary>
        /// <param name="deep">A Boolean value; if set to true, the children of the specified XML object will be recursively cloned.</param>
        /// <returns>An XMLNode Object.</returns>
        [PageFX.AbcInstanceTrait(15)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.xml.XMLNode cloneNode(bool deep);

        /// <summary>Removes the specified XML object from its parent. Also deletes all descendants of the node.</summary>
        [PageFX.AbcInstanceTrait(16)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void removeNode();

        /// <summary>
        /// Inserts a new child node into the XML object&apos;s child list, before the
        /// beforeNode node. If the beforeNode parameter is undefined
        /// or null, the node is added using the appendChild() method. If beforeNode
        /// is not a child of my_xml, the insertion fails.
        /// </summary>
        /// <param name="node">The XMLNode object to be inserted.</param>
        /// <param name="before">The XMLNode object before the insertion point for the childNode.</param>
        [PageFX.AbcInstanceTrait(17)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void insertBefore(flash.xml.XMLNode node, flash.xml.XMLNode before);

        /// <summary>
        /// Appends the specified node to the XML object&apos;s child list. This method operates directly on the
        /// node referenced by the childNode parameter; it does not append a copy of the
        /// node. If the node to be appended already exists in another tree structure, appending the node to the
        /// new location will remove it from its current location. If the childNode
        /// parameter refers to a node that already exists in another XML tree structure, the appended child node
        /// is placed in the new tree structure after it is removed from its existing parent node.
        /// </summary>
        /// <param name="node">
        /// An XMLNode that represents the node to be moved from its current location to the child
        /// list of the my_xml object.
        /// </param>
        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void appendChild(flash.xml.XMLNode node);

        /// <summary>
        /// Evaluates the specified XMLNode object, constructs a textual representation of the XML structure,
        /// including the node, children, and attributes, and returns the result as a string.
        /// For top-level XMLDocument objects (those created with the constructor),
        /// the XMLDocument.toString() method outputs the document&apos;s XML declaration
        /// (stored in the XMLDocument.xmlDecl property), followed by the document&apos;s
        /// DOCTYPE declaration (stored in the XMLDocument.docTypeDecl property),
        /// followed by the text representation of all XML nodes in the object. The XML declaration is not
        /// output if the XMLDocument.xmlDecl property is null.
        /// The DOCTYPE declaration is not output if the
        /// XMLDocument.docTypeDecl property is null.
        /// </summary>
        /// <returns>The string representing the XMLNode object.</returns>
        [PageFX.AbcInstanceTrait(19)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();

        /// <summary>
        /// Returns the namespace URI that is associated with the specified prefix for the node. To determine
        /// the URI, getPrefixForNamespace() searches up the XML hierarchy from the node, as
        /// necessary, and returns the namespace URI of the first xmlns declaration for the
        /// given prefix.
        /// If no namespace is defined for the specified prefix, the method returns null.If you specify an empty string (&quot;&quot;) as the prefix and there is a
        /// default namespace defined for the node (as in xmlns=&quot;http://www.example.com/&quot;),
        /// the method returns that default namespace URI.
        /// </summary>
        /// <param name="prefix">The prefix for which the method returns the associated namespace.</param>
        /// <returns>The namespace that is associated with the specified prefix.</returns>
        [PageFX.AbcInstanceTrait(20)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String getNamespaceForPrefix(Avm.String prefix);

        /// <summary>
        /// Returns the prefix that is associated with the specified namespace URI for the node. To determine
        /// the prefix, getPrefixForNamespace() searches up the XML hierarchy from the node, as
        /// necessary, and returns the prefix of the first xmlns declaration with a namespace URI
        /// that matches ns.
        /// If there is no xmlns
        /// assignment for the given URI, the method returns null. If there is an
        /// xmlns assignment for the given URI but no prefix is associated with the assignment,
        /// the method returns an empty string
        /// (&quot;&quot;).
        /// </summary>
        /// <param name="ns">The namespace URI for which the method returns the associated prefix.</param>
        /// <returns>The prefix associated with the specified namespace.</returns>
        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String getPrefixForNamespace(Avm.String ns);


    }
}
