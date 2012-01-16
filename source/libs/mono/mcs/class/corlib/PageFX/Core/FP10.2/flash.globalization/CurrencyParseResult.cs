using System;
using System.Runtime.CompilerServices;

namespace flash.globalization
{
    [PageFX.AbcInstance(238)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class CurrencyParseResult : Avm.Object
    {
        public extern virtual double value
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String currencyString
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern CurrencyParseResult(double value, Avm.String symbol);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern CurrencyParseResult(double value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern CurrencyParseResult();


    }
}
