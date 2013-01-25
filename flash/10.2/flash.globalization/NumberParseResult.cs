using System;
using System.Runtime.CompilerServices;

namespace flash.globalization
{
    [PageFX.AbcInstance(345)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class NumberParseResult : Avm.Object
    {
        public extern virtual double value
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual int startIndex
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual int endIndex
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NumberParseResult(double value, int startIndex, int endIndex);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NumberParseResult(double value, int startIndex);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NumberParseResult(double value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NumberParseResult();


    }
}
