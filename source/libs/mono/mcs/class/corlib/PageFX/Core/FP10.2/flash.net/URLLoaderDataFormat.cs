using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>The URLLoaderDataFormat class provides values that specify how downloaded data is received.</summary>
    [PageFX.AbcInstance(31)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class URLLoaderDataFormat : Avm.Object
    {
        /// <summary>Specifies that downloaded data is received as text.</summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String TEXT;

        /// <summary>Specifies that downloaded data is received as raw binary data.</summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String BINARY;

        /// <summary>Specifies that downloaded data is received as URL-encoded variables.</summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String VARIABLES;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern URLLoaderDataFormat();
    }
}
