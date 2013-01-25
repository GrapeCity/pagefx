using System;
using System.Runtime.CompilerServices;

namespace flash.globalization
{
    [PageFX.AbcInstance(212)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class LastOperationStatus : Avm.Object
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NO_ERROR;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String USING_FALLBACK_WARNING;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String USING_DEFAULT_WARNING;

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String PARSE_ERROR;

        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String UNSUPPORTED_ERROR;

        [PageFX.AbcClassTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String ERROR_CODE_UNKNOWN;

        [PageFX.AbcClassTrait(6)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String PATTERN_SYNTAX_ERROR;

        [PageFX.AbcClassTrait(7)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String MEMORY_ALLOCATION_ERROR;

        [PageFX.AbcClassTrait(8)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String ILLEGAL_ARGUMENT_ERROR;

        [PageFX.AbcClassTrait(9)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String BUFFER_OVERFLOW_ERROR;

        [PageFX.AbcClassTrait(10)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String INVALID_ATTR_VALUE;

        [PageFX.AbcClassTrait(11)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String NUMBER_OVERFLOW_ERROR;

        [PageFX.AbcClassTrait(12)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String INVALID_CHAR_FOUND;

        [PageFX.AbcClassTrait(13)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String TRUNCATED_CHAR_FOUND;

        [PageFX.AbcClassTrait(14)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String INDEX_OUT_OF_BOUNDS_ERROR;

        [PageFX.AbcClassTrait(15)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String PLATFORM_API_FAILED;

        [PageFX.AbcClassTrait(16)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String UNEXPECTED_TOKEN;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern LastOperationStatus();
    }
}
