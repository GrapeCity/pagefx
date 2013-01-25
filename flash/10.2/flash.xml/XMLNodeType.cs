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
    [PageFX.AbcInstance(240)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class XMLNodeType : Avm.Object
    {
        /// <summary>
        /// Specifies that the node is an element.
        /// This constant is used with XMLNode.nodeType.
        /// The value is defined by the NodeType enumeration in the
        /// W3C DOM Level 1 recommendation:
        /// http://www.w3.org/tr/1998/REC-DOM-Level-1-19981001/level-one-core.html
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint ELEMENT_NODE;

        /// <summary>
        /// Specifies that the node is a text node.
        /// This constant is used with XMLNode.nodeType.
        /// The value is defined by the NodeType enumeration in the
        /// W3C DOM Level 1 recommendation:
        /// http://www.w3.org/tr/1998/REC-DOM-Level-1-19981001/level-one-core.html
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint TEXT_NODE;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint CDATA_NODE;

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint PROCESSING_INSTRUCTION_NODE;

        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint COMMENT_NODE;

        [PageFX.AbcClassTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint DOCUMENT_TYPE_NODE;

        [PageFX.AbcClassTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint XML_DECLARATION;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern XMLNodeType();
    }
}
