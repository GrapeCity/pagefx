using System;
using System.Runtime.CompilerServices;

namespace flash.xml
{
    /// <summary>
    /// The XMLNodeType class contains constants used with
    /// XMLNode.nodeType . The values are defined
    /// by the NodeType enumeration in the
    /// W3C DOM Level 1 recommendation:
    /// http://www.w3.org/tr/1998/REC-DOM-Level-1-19981001/level-one-core.html
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class XMLNodeType : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint DOCUMENT_TYPE_NODE;

        /// <summary>
        /// Specifies that the node is an element.
        /// This constant is used with XMLNode.nodeType.
        /// The value is defined by the NodeType enumeration in the
        /// W3C DOM Level 1 recommendation:
        /// http://www.w3.org/tr/1998/REC-DOM-Level-1-19981001/level-one-core.html
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint ELEMENT_NODE;

        [PageFX.ABC]
        [PageFX.FP9]
        public static uint COMMENT_NODE;

        [PageFX.ABC]
        [PageFX.FP9]
        public static uint XML_DECLARATION;

        [PageFX.ABC]
        [PageFX.FP9]
        public static uint CDATA_NODE;

        /// <summary>
        /// Specifies that the node is a text node.
        /// This constant is used with XMLNode.nodeType.
        /// The value is defined by the NodeType enumeration in the
        /// W3C DOM Level 1 recommendation:
        /// http://www.w3.org/tr/1998/REC-DOM-Level-1-19981001/level-one-core.html
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint TEXT_NODE;

        [PageFX.ABC]
        [PageFX.FP9]
        public static uint PROCESSING_INSTRUCTION_NODE;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern XMLNodeType();
    }
}
