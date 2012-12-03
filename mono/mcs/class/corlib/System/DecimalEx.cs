// The ECMA standard is based much on
// http://www2.hursley.ibm.com/decimal

using System;
using System.Text;
using System.Diagnostics;

#if NODDCORLIB
using DDDecimal = DDCorLibTest.Decimal;
namespace DDCorLibTest 
#else
using DDDecimal = System.Decimal;
namespace System
#endif
{
    internal class DecimalEx
    {
        private const UInt32 SIGN_FLAG32 = 0x80000000;
        private const UInt64 SIGN_FLAG64 = 0x8000000000000000;
        private const UInt32 SCALE_MASK = 0x00FF0000;
        private const int SCALE_SHIFT = 16;

        private const int DECIMAL_MAX_SCALE = 28;
        private const int DECIMAL_SUCCESS = 0;
        private const int DECIMAL_FINISHED = 1;
        private const int DECIMAL_OVERFLOW = 2;
        private const int DECIMAL_INVALID_CHARACTER = 2;
        private const int DECIMAL_INTERNAL_ERROR = 3;
        private const int DECIMAL_INVALID_BITS = 4;
        private const int DECIMAL_DIVIDE_BY_ZERO = 5;
        private const int DECIMAL_BUFFER_OVERFLOW = 6;
        private const int DECIMAL_LOG_NEGINF = -1000;
        private const int DECIMAL_MAX_INTFACTORS = 9;


        private static readonly UInt32[] constantsDecadeInt32Factors =
            new UInt32[] {1, 10, 100, 1000, 10000, 100000, 1000000, 
							 10000000, 100000000, 1000000000};

        struct dec128
        {
            internal UInt64 lo;
            internal UInt64 hi;
            internal dec128(UInt64 hi, UInt64 mid, UInt64 lo)
            {
                this.lo = (mid << 32) | lo;
                this.hi = hi;
            }
            internal dec128(UInt64 hi, UInt64 lo)
            {
                this.lo = lo;
                this.hi = hi;
            }
        };
#if false // BUGGY cctor 8/29/05
		private static readonly dec128[] dec128decadeFactors 
			= new dec128[] { 
							   new dec128( 0, 0, 1u), // == 1
							   new dec128( 0, 0, 10u), // == 10 
							   new dec128( 0, 0, 100u), // == 100																
							   new dec128( 0, 0, 1000u), // == 1e3m 
							   new dec128( 0, 0, 10000u), // == 1e4m 
							   new dec128( 0, 0, 100000u), // == 1e5m 
							   new dec128( 0, 0, 1000000u), // == 1e6m
							   new dec128( 0, 0, 10000000u), // == 1e7m 
							   new dec128( 0, 0, 100000000u), // == 1e8m 
							   new dec128( 0, 0, 1000000000u), // == 1e9m 
							   new dec128( 0, 2u, 1410065408u), // == 1e10m
							   new dec128( 0, 23u, 1215752192u), // == 1e11m
							   new dec128( 0, 232u, 3567587328u), // == 1e12m
							   new dec128( 0, 2328u, 1316134912u), // == 1e13m
							   new dec128( 0, 23283u, 276447232u), // == 1e14m 
							   new dec128( 0, 232830u, 2764472320u), // == 1e15m 
							   new dec128( 0, 2328306u, 1874919424u), // == 1e16m
							   new dec128( 0, 23283064u, 1569325056u), // == 1e17m 
							   new dec128( 0, 232830643u, 2808348672u), // == 1e18m
							   new dec128( 0, 2328306436u, 2313682944u), // == 1e19m 
							   new dec128( 5u, 1808227885u, 1661992960u), // == 1e20m 
							   new dec128( 54u, 902409669u, 3735027712u), // == 1e21m 
							   new dec128( 542u, 434162106u, 2990538752u), // == 1e22m 
							   new dec128( 5421u, 46653770u, 4135583744u), // == 1e23m 
							   new dec128( 54210u, 466537709u, 2701131776u), // == 1e24m 
							   new dec128( 542101u, 370409800u, 1241513984u), // == 1e25m 
							   new dec128( 5421010u, 3704098002u, 3825205248u), // == 1e26m 
							   new dec128( 54210108u, 2681241660u, 3892314112u), // == 1e27m 
							   new dec128( 542101086u, 1042612833u, 268435456u), // == 1e28m 
		};
#else
        private static readonly dec128[] dec128decadeFactors
            = new dec128[] { 
							   new dec128( 0, 1ul), // == 1
							   new dec128( 0, 10ul), // == 10 
							   new dec128( 0, 100ul), // == 100																
							   new dec128( 0, 1000ul), // == 1e3m 
							   new dec128( 0, 10000ul), // == 1e4m 
							   new dec128( 0, 100000ul), // == 1e5m 
							   new dec128( 0, 1000000ul), // == 1e6m
							   new dec128( 0, 10000000ul), // == 1e7m 
							   new dec128( 0, 100000000ul), // == 1e8m 
							   new dec128( 0, 1000000000ul), // == 1e9m 
							   new dec128( 0, 10000000000ul), // == 1e10m
							   new dec128( 0, 100000000000ul), // == 1e11m
							   new dec128( 0, 1000000000000ul), // == 1e12m
							   new dec128( 0, 10000000000000ul), // == 1e13m
							   new dec128( 0, 100000000000000ul), // == 1e14m 
							   new dec128( 0, 1000000000000000ul), // == 1e15m 
							   new dec128( 0, 10000000000000000ul), // == 1e16m
							   new dec128( 0, 100000000000000000ul), // == 1e17m 
							   new dec128( 0, 1000000000000000000ul), // == 1e18m
							   new dec128( 0, 10000000000000000000ul), // == 1e19m 
							   new dec128( 5ul, 7766279631452241920ul), // == 1e20m 
							   new dec128( 54ul, 3875820019684212736ul), // == 1e21m 
							   new dec128( 542ul, 1864712049423024128ul), // == 1e22m 
							   new dec128( 5421ul, 200376420520689664ul), // == 1e23m 
							   new dec128( 54210ul, 2003764205206896640ul), // == 1e24m 
							   new dec128( 542101ul, 1590897978359414784ul), // == 1e25m 
							   new dec128( 5421010ul, 15908979783594147840ul), // == 1e26m 
							   new dec128( 54210108ul, 11515845246265065472ul), // == 1e27m 
							   new dec128( 542101086ul, 4477988020393345024ul), // == 1e28m
		};
#endif

        public static int decimalAdd(ref Decimal A, ref Decimal B)
        {
            UInt64 alo, ahi, blo, bhi;
            int log2A, log2B, log2Result, log10Result;
            int rc = DECIMAL_SUCCESS;
            int subFlag, signA, signB, scaleA, scaleB, scaleA1;

            Unpack(ref A, out alo, out ahi, out signA, out scaleA);
            Unpack(ref B, out blo, out bhi, out signB, out scaleB);
            subFlag = signA - signB;
            //Console.WriteLine ("in decimalIncr");
            //Console.WriteLine ("  A: {0}:{1}  scale: {2}", ahi, alo, scaleA);
            //Console.WriteLine ("  B: {0}:{1}  scale: {2}", bhi, blo, scaleB);
            //Console.WriteLine ("  subflag = {0}", subFlag);

            if (scaleA == scaleB)
            {
                // same scale, that's easy 
                if (subFlag != 0)
                {
                    UInt64 clo, chi;
                    sub128(alo, ahi, blo, bhi, out clo, out chi);
                    alo = clo;
                    ahi = chi;
                    //Console.WriteLine("   sub128 result alo={0} ahi={1}",alo,ahi);
                    if ((ahi & SIGN_FLAG64) != 0)
                    {
                        alo--;
                        alo = ~alo;
                        if (alo == 0) ahi--;
                        ahi = ~ahi;
                        signA = (signA == 0) ? 1 : 0;
                    }
                }
                else
                {
                    add128(alo, ahi, blo, bhi, out alo, out ahi);
                }
                rc = normalize128(ref alo, ref ahi, ref scaleA, true, false);
            }
            else
            {
                // scales must be adjusted 
                // Estimate log10 and scale of result for adjusting scales 
                scaleA1 = scaleA;
                log2A = log2withScale_128(alo, ahi, scaleA);
                log2B = log2withScale_128(blo, bhi, scaleB);
                log2Result = (log2A >= log2B) ? log2A : log2B;
                if (subFlag == 0) log2Result++; // result can have one bit more
                log10Result = (log2Result * 1000) / 3322 + 1;
                // we will calculate in 128bit, so we may need to adjust scale
                if (scaleB > scaleA) scaleA = scaleB;
                if (scaleA + log10Result > DECIMAL_MAX_SCALE + 7)
                {
                    // this may not fit in 128bit, so limit it 
                    scaleA = DECIMAL_MAX_SCALE + 7 - log10Result;
                }

                rc = adjustScale128(ref alo, ref ahi, scaleA - scaleA1);
                if (rc != DECIMAL_SUCCESS) return rc;
                rc = adjustScale128(ref blo, ref bhi, scaleA - scaleB);
                if (rc != DECIMAL_SUCCESS) return rc;

                if (subFlag != 0)
                {
                    sub128(alo, ahi, blo, bhi, out alo, out ahi);
                    if ((ahi & SIGN_FLAG64) != 0)
                    {
                        alo--;
                        alo = ~alo;
                        if (alo == 0) ahi--;
                        ahi = ~ahi;
                        signA = (signA == 0) ? 1 : 0;
                    }
                }
                else
                {
                    add128(alo, ahi, blo, bhi, out alo, out ahi);
                }

                if (rc != DECIMAL_SUCCESS)
                    return rc;
                rc = rescale128(ref alo, ref ahi, ref scaleA, 0, 0, DECIMAL_MAX_SCALE, true);
            }

            if (rc != DECIMAL_SUCCESS)
                return rc;
            return pack128toDecimal(ref A, alo, ahi, scaleA, signA);
        }

        public static Int32 decimalMult(ref Decimal pA, Decimal B)
        {
            UInt64 low, mid, high;
            UInt32 factor;
            int scale, sign, rc;

            mult96by96to192(pA.lo32, pA.mid32, pA.hi32, B.lo32, B.mid32, B.hi32,
                out low, out mid, out high);

            // adjust scale and sign
            scale = pA.Scale + B.Scale;
            sign = pA.Sign ^ B.Sign;

            // first scaling step 
            factor = constantsDecadeInt32Factors[DECIMAL_MAX_INTFACTORS];
            while (high != 0 || (mid >> 32) >= factor)
            {
                if (high < 100)
                {
                    factor /= 1000; // we need some digits for final rounding
                    scale -= DECIMAL_MAX_INTFACTORS - 3;
                }
                else
                {
                    scale -= DECIMAL_MAX_INTFACTORS;
                }

                div192by32(ref low, ref mid, ref high, factor);
            }

            // second and final scaling 
            rc = rescale128(ref low, ref mid, ref scale, 0, 0, DECIMAL_MAX_SCALE, true);
            if (rc != DECIMAL_SUCCESS) return rc;

            return pack128toDecimal(ref pA, low, mid, scale, sign);
        }

        public static Int32 decimalIntDiv(out Decimal pC, ref Decimal A, ref Decimal B)
        {
            UInt64 clo, chi; // result 
            int scale, texp, rc;

            //Console.WriteLine("In decimalIntDiv: {0} / {1}", A, B);

            pC = new Decimal();
            rc = decimalDivSub(A, B, out clo, out chi, out texp);
            //Console.WriteLine ("   => {0}:{1}", chi.ToString("x"), clo.ToString("x"));
            if (rc != DECIMAL_SUCCESS)
            {
                if (rc == DECIMAL_FINISHED) rc = DECIMAL_SUCCESS;
                return rc;
            }

            // calc scale 
            scale = A.Scale - B.Scale;

            // truncate result to integer
            rc = rescale128(ref clo, ref chi, ref scale, texp, 0, 0, false);
            //Console.WriteLine ("   => {0}:{1}", chi, clo);
            if (rc != DECIMAL_SUCCESS) return rc;

            return pack128toDecimal(ref pC, clo, chi, scale, A.Sign);
        }

        public static Int32 decimalDiv(out Decimal C, ref Decimal A, ref Decimal B)
        {
            UInt64 clo, chi; // result
            int scale, texp, rc;

            C = new Decimal();
            rc = decimalDivSub(A, B, out clo, out chi, out texp);
            if (rc != DECIMAL_SUCCESS)
            {
                if (rc == DECIMAL_FINISHED) rc = DECIMAL_SUCCESS;
                return rc;
            }

            // adjust scale and sign
            scale = A.Scale - B.Scale;

            //test: printf("0: %.17e\n", (((double)chi) * pow(2,64) + clo) * pow(10, -scale) * pow(2, -exp));
            rc = rescale128(ref clo, ref chi, ref scale, texp, 0, DECIMAL_MAX_SCALE, true);
            if (rc != DECIMAL_SUCCESS) return rc;

            return pack128toDecimal(ref C, clo, chi, scale, A.Sign ^ B.Sign);
        }
        public static Int32 decimalCompare(ref Decimal A, ref Decimal B)
        {
            int log2a, log2b, delta, sign;
            Decimal aa;

            //			Console.WriteLine("in decimalCompare:");
            //			Console.WriteLine (" A: {0}:{1}:{2}", A.hi32, A.mid32, A.lo32);
            //			Console.WriteLine (" B: {0}:{1}:{2}", B.hi32, B.mid32, B.lo32);
            sign = A.Sign == 0 ? 1 : -1;
            if ((A.Sign ^ B.Sign) != 0)
            {
                return (decimalIsZero(A) && decimalIsZero(B)) ? 0 : sign;
            }

            // try fast comparison via log2 
            log2a = decimalLog2(A);
            log2b = decimalLog2(B);
            delta = log2a - log2b;
            // decimalLog2 is not exact, so we can say nothing 
            //   if abs(delta) <= 1 
            if (delta < -1) return -sign;
            if (delta > 1) return sign;

            aa = A;
            aa.ss32 ^= SIGN_FLAG32;
            decimalAdd(ref aa, ref B);

            if (decimalIsZero(aa)) return 0;

            return (aa.Sign != 0) ? 1 : -1;
        }
        // internal debugging routine
        internal static String IntoString(Decimal A)
        {
            String result;
            System.Text.StringBuilder sb;
            char[] digits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            int sign, scale, i, j;
            UInt64 hi, lo;
            UInt32 remainder;

            if (decimalIsZero(A))
                return "0";

            scale = A.Scale;
            sign = A.Sign;
            if (sign != 0)
                A.ss32 ^= SIGN_FLAG32;
            DECTO128(ref A, out lo, out hi);

            sb = new System.Text.StringBuilder(35);
            while (lo != 0 || hi != 0)
            {
                div128by32(ref lo, ref hi, 10, out remainder);
                sb.Append(digits[remainder]);
            }
            if (sign != 0)
                sb.Append('-');
            for (i = 0, j = sb.Length - 1; i < j; ++i, --j)
            {
                char tmp = sb[i];
                sb[i] = sb[j];
                sb[j] = tmp;
            }
            if (scale > 0)
                sb.Insert(sb.Length - scale, '.');
            result = sb.ToString();
            return result;
        }

        //	
        // string2decimal:
        // @decimal_repr:
        // @str:
        // @decrDecimal:
        // @sign:
        //
        // converts a digit string to decimal
        // The significant digits must be passed as an integer in buf !
        //
        // 1. Example:
        //   if you want to convert the number 123.456789012345678901234
        //     buf := "123456789012345678901234"
        //     decrDecimal := 3
        //     sign := 0
        //
        // 2. Example:
        //   you want to convert -79228162514264337593543950335 to decimal
        //     buf := "79228162514264337593543950335"
        //     decrDecimal := 29
        //     sign := 1
        //
        // 3. Example:
        //   you want to convert -7922816251426433759354395033.250000000000001
        //     buf := "7922816251426433759354395033250000000000001"
        //     decrDecimal := 29
        //     sign := 1
        //     returns (decimal)-7922816251426433759354395033.3
        //
        // 4. Example:
        //   you want to convert -7922816251426433759354395033.250000000000000
        //     buf := "7922816251426433759354395033250000000000000"
        //     decrDecimal := 29
        //     sign := 1
        //     returns (decimal)-7922816251426433759354395033.2
        //
        // 5. Example:
        //   you want to convert -7922816251426433759354395033.150000000000000
        //     buf := "7922816251426433759354395033150000000000000"
        //     decrDecimal := 29
        //     sign := 1
        //     returns (decimal)-7922816251426433759354395033.2
        //
        // Uses banker's rule for rounding if there are more digits than can be
        // represented by the significant
        //

        public static Int32 string2decimal(out Decimal pA, String str, UInt32 decrDecimal, Int32 sign)
        {
            char p;
            UInt64 alo, ahi;
            int n, rc, i, len;
            int sigLen = -1;
            int firstNonZero;
            int scale;
            bool roundBit = false;

            //            Console.WriteLine("  string2decimal entered with str={0}", str);

            alo = ahi = 0;
            pA = new Decimal();
            len = str.Length;

            for (i = 0; i < len; ++i)
            {
                p = str[i];
                n = p - '0';
                if (n < 0 || n > 9)
                {
                    return DECIMAL_INVALID_CHARACTER;
                }
                if (n != 0)
                {
                    if (sigLen < 0)
                    {
                        firstNonZero = i;
                        sigLen = (len - firstNonZero > DECIMAL_MAX_SCALE + 1) ? DECIMAL_MAX_SCALE + 1 + firstNonZero : len;
                        if (decrDecimal > sigLen + 1)
                            return DECIMAL_OVERFLOW;
                    }
                    if (i >= sigLen)
                        break;


                    rc = incMultConstant128(ref alo, ref ahi, sigLen - 1 - i, n);
                    //                    Console.WriteLine("digit={0}  rc result={1}:{2}", n, ahi, alo);

                    if (rc != DECIMAL_SUCCESS)
                    {
                        return rc;
                    }
                }
            }

            scale = sigLen - (int)decrDecimal;

            if (i < len)
            {
                //                Console.WriteLine("  too many digits");
                // too many digits, we must round
                n = str[i] - '0';
                if (n < 0 || n > 9)
                {
                    return DECIMAL_INVALID_CHARACTER;
                }
                if (n > 5) roundBit = true;
                else if (n == 5)
                { // we must take a nearer look
                    n = str[i - 1] - '0';
                    for (++i; i < len; ++i)
                    {
                        if (str[i] != '0') break; // we are greater than .5
                    }
                    if (i < len // greater than exactly .5
                        || n % 2 == 1)  // exactly .5, use banker's rule for rounding 
                    {
                        roundBit = true;
                    }
                }
            }

            if (ahi != 0)
            {
                //                Console.WriteLine("  calling normalize {0}:{1}", ahi, alo);
                rc = normalize128(ref alo, ref ahi, ref scale, true, roundBit);
                if (rc != DECIMAL_SUCCESS)
                    return rc;
            }

            if (alo == 0 && ahi == 0)
            {
                return DECIMAL_SUCCESS;
            }
            else
            {
                //                Console.WriteLine("  calling pack {0}:{1}", ahi, alo);
                int res = pack128toDecimal(ref pA, alo, ahi, sigLen - (int)decrDecimal, sign);
                //                Console.WriteLine("  packing result = {0},{1},{2},{3}", pA.lo32,pA.mid32,pA.hi32,pA.ss32);
                //              Console.WriteLine(pA.ToString());
                return res;
            }
        }
        // a *= 10^exp 
        public static Int32 decimalSetExponent(ref Decimal pA, Int32 texp)
        {
            UInt64 alo, ahi;
            int rc;
            int scale = pA.Scale;

            scale -= texp;

            if (scale < 0 || scale > DECIMAL_MAX_SCALE)
            {
                alo = ((UInt64)pA.mid32 << 32) | pA.lo32;
                ahi = pA.hi32;

                rc = rescale128(ref alo, ref ahi, ref scale, 0, 0, DECIMAL_MAX_SCALE, true);
                if (rc != DECIMAL_SUCCESS) return rc;
                return pack128toDecimal(ref pA, alo, ahi, scale, pA.Sign);
            }
            else
            {
                pA.Scale = scale;
                return DECIMAL_SUCCESS;
            }
        }

        //
        // decimal2Uint64
        // @pA
        // @pResult
        // converts a decimal to an UInt64 without rounding
        //

        public static Int32 decimal2UInt64(ref Decimal A, out UInt64 pResult)
        {
            UInt64 alo, ahi;
            int scale;

            pResult = 0;
            alo = ((UInt64)A.mid32 << 32) | A.lo32;
            ahi = A.hi32;
            scale = A.Scale;
            if (scale > 0)
                div128DecadeFactor(ref alo, ref ahi, scale);

            // overflow if integer too large or < 0
            if (ahi != 0 || (alo != 0 && A.Sign != 0)) return DECIMAL_OVERFLOW;

            pResult = alo;
            return DECIMAL_SUCCESS;
        }

        //
        // decimal2Int64:
        // @pA:
        // pResult:
        // converts a decimal to an Int64 without rounding
        //
        public static Int32 decimal2Int64(ref Decimal A, out Int64 pResult)
        {
            UInt64 alo, ahi;
            int sign, scale;

            pResult = 0;
            alo = ((UInt64)A.mid32 << 32) | A.lo32;
            ahi = A.hi32;
            scale = A.Scale;
            if (scale > 0)
                div128DecadeFactor(ref alo, ref ahi, scale);

            if (ahi != 0) return DECIMAL_OVERFLOW;

            sign = A.Sign;
            if (sign != 0 && alo != 0)
            {
                if (alo > SIGN_FLAG64) return DECIMAL_OVERFLOW;
                pResult = (Int64)~(alo - 1);
            }
            else
            {
                if ((alo & SIGN_FLAG64) != 0) return DECIMAL_OVERFLOW;
                pResult = (Int64)alo;
            }

            return DECIMAL_SUCCESS;
        }

        public static void decimalFloorAndTrunc(ref Decimal pA, int floorFlag)
        {
            UInt64 alo, ahi;
            UInt32 factor, rest;
            int scale, sign, idx;
            bool hasRest = false;

            scale = pA.Scale;
            if (scale == 0) return; // nothing to do

            alo = (((UInt64)pA.mid32) << 32) | pA.lo32;
            ahi = pA.hi32;
            sign = pA.Sign;

            while (scale > 0)
            {
                idx = (scale > DECIMAL_MAX_INTFACTORS) ? DECIMAL_MAX_INTFACTORS : scale;
                factor = constantsDecadeInt32Factors[idx];
                scale -= idx;
                div128by32(ref alo, ref ahi, factor, out rest);
                hasRest = hasRest || (rest != 0);
            }

            if (floorFlag != 0 && hasRest && sign != 0) // floor: if negative, we must round up 
                roundUp128(ref alo, ref ahi);


            pack128toDecimal(ref pA, alo, ahi, 0, sign);
        }

        /*public static void decimalRound(ref Decimal pA, Int32 decimals) {
            UInt64 alo, ahi;
            int scale, sign;

            alo = (((UInt64) pA.mid32) << 32) | pA.lo32;
            ahi = pA.hi32;
            sign = pA.Sign;
            scale = pA.Scale;

            if (scale > decimals) {
                div128DecadeFactor(ref alo, ref ahi, scale - decimals);
                scale = decimals;
            }
    
            pack128toDecimal(ref pA, alo, ahi, scale, sign);
        }
        */
#if false
		public static double decimal2double(ref Decimal pA) {
			double d;

			String s = decimal
			UInt64 alo, ahi, mantisse;
			UInt32 overhang, factor, roundBits, rest;
			int scale, texp, log5, i;

			ahi = (((UInt64)(pA.hi32)) << 32) | pA.mid32;
			alo = ((UInt64)(pA.lo32)) << 32;

			/* special case zero */
			if (ahi == 0 && alo == 0) return 0.0;

			texp = 0;
			scale = pA.Scale;

			/* transform n * 10^-scale and exp = 0 => m * 2^-exp and scale = 0 */
			while (scale > 0) {
				while ((ahi & SIGN_FLAG64) == 0) {
					lshift128(ref alo, ref ahi);
					texp++;
				}

				overhang = (UInt32) (ahi >> 32);
				if (overhang >= 5) {
					/* estimate log5 */
					log5 = (log2_32(overhang) * 1000) / 2322; /* ln(5)/ln(2) = 2.3219... */
					if (log5 < DECIMAL_MAX_INTFACTORS) {
						/* get maximal factor=5^i, so that overhang / factor >= 1 */
						factor = constantsDecadeInt32Factors[log5] >> log5; /* 5^n = 10^n/2^n */
						i = (int) (log5 + overhang / factor);
					} else {
						i = DECIMAL_MAX_INTFACTORS; /* we have only constants up to 10^DECIMAL_MAX_INTFACTORS */
					}
					if (i > scale) i = scale;
					factor = constantsDecadeInt32Factors[i] >> i; /* 5^n = 10^n/2^n */
					/* n * 10^-scale * 2^-exp => m * 10^-(scale-i) * 2^-(exp+i) with m = n * 5^-i */
					div128by32(ref alo, ref ahi, factor, out rest);
					scale -= i;
					texp += i;
				}
			}

			/* normalize significand (highest bit should be 1) */
			while ((ahi & SIGN_FLAG64) == 0) {
				lshift128(ref alo, ref ahi);
				texp++;
			}

			/* round to nearest even */
			roundBits = (UInt32)ahi & 0x7ff;
			ahi += 0x400;
			if ((ahi & SIGN_FLAG64) == 0) { /* overflow ? */
				ahi >>= 1;
				texp--;
			} else if ((roundBits & 0x400) == 0) ahi &= ~1u;

			/* 96 bit => 1 implizit bit and 52 explicit bits */
			mantisse = (ahi & ~SIGN_FLAG64) >> 11;

			d = 2.75;
			//buildIEEE754Double(ref d, pA.Sign, -texp+95, mantisse);

			return d;
		}
#endif
        //
        // decimal2string:
        // @
        // returns minimal number of digit string to represent decimal
        // No leading or trailing zeros !
        // Examples:
        // *pA == 0            =>   buf = "", *pDecPos = 1, *pSign = 0
        // *pA == 12.34        =>   buf = "1234", *pDecPos = 2, *pSign = 0
        // *pA == -1000.0000   =>   buf = "1", *pDecPos = 4, *pSign = 1
        // *pA == -0.00000076  =>   buf = "76", *pDecPos = -6, *pSign = 0
        // 
        // Parameters:
        //    pA         decimal instance to convert     
        //    digits     < 0: use decimals instead
        //               = 0: gets mantisse as integer
        //               > 0: gets at most <digits> digits, rounded according to banker's rule if necessary
        //    decimals   only used if digits < 0
        //               >= 0: number of decimal places
        //    buf        pointer to result buffer
        //    bufSize    size of buffer
        //    pDecPos    receives insert position of decimal point relative to start of buffer
        //    pSign      receives sign
        //
        internal static Int32 decimal2string(Decimal pA, Int32 digits, Int32 decimals,
            char[] pArray, Int32 bufSize, out Int32 pDecPos, out Int32 pSign)
        {
            char[] tmp = new char[41];
            int q = 0, p = 0;
            Decimal aa;
            UInt64 alo, ahi;
            UInt32 rest;
            Int32 sigDigits, d;
            int i, scale, len;

            //Console.WriteLine("!decimal2string ss32:{0} lo:{1} mid:{2} hi:{3}",pA.ss32,pA.lo32,pA.mid32,pA.hi32);
            pDecPos = 0;
            pSign = 0;

            alo = (((UInt64)pA.mid32) << 32) | pA.lo32;
            ahi = pA.hi32;
            scale = pA.Scale;

            //Console.WriteLine("calling calcDigits with alo={0}:{1} ahi={2}:{3}",alo.m_hi,alo.m_lo,ahi.m_hi,ahi.m_lo);
            sigDigits = calcDigits(alo, ahi); // significant digits
            //			Console.WriteLine("   digits: {0}, sigDigits: {1}", digits, sigDigits);


            // calc needed digits (without leading or trailing zeros)
            d = (digits == 0) ? sigDigits : digits;
            //Console.WriteLine("\td={0}",d);
            if (d < 0)
            { // use decimals ? 
                if (0 <= decimals && decimals < scale)
                {
                    d = sigDigits - scale + decimals;
                }
                else
                {
                    d = sigDigits; // use all you can get
                }
            }

            //Console.WriteLine("!decimal2string sigDigits={0} d={1}",sigDigits,d);


            if (sigDigits > d)
            { // we need to round decimal number 
                aa = pA;
                aa.Scale = DECIMAL_MAX_SCALE;
                aa = Decimal.Round(aa, DECIMAL_MAX_SCALE - sigDigits + d);
                DECTO128(ref aa, out alo, out ahi);
                sigDigits += calcDigits(alo, ahi) - d;
            }

            len = 0;
            if (d > 0)
            {
                //Console.WriteLine("\tdigits from tail");

                // get digits starting from the tail
                for (; (alo != 0 || ahi != 0) && len < 40; len++)
                {
                    //Console.WriteLine("\talo entry={0}:{1} ahi entry={2}:{3}",alo.m_hi,alo.m_lo,ahi.m_hi,ahi.m_lo);

                    //div128by32(ref alo, ref ahi, 10, out rest);
                    div96by16(ref alo, ref ahi, 10, out rest);

                    //Console.WriteLine("\talo={0} rest={1}",alo.m_lo,rest);

                    tmp[p++] = (char)('0' + rest);
                }
            }
            tmp[p] = '\0';

            //Console.WriteLine("minimal sigDigits={0}",digits);

            if (len >= bufSize)
                return DECIMAL_BUFFER_OVERFLOW;

            // now we have the minimal count of digits, 
            //   extend to wished count of digits or decimals
            //q = buf;
            q = 0;

            //Console.WriteLine("** digits={0} bufSize={1} len={2}",digits,bufSize,len);

            if (digits >= 0)
            { // count digits 
                if (digits >= bufSize)
                    return DECIMAL_BUFFER_OVERFLOW;
                if (len == 0)
                {
                    // zero or rounded to zero 
                    pDecPos = 1;
                }
                else
                {
                    // copy significant digits 
                    for (i = 0; i < len; i++)
                    {
                        pArray[q++] = tmp[--p];
                        //*q++ = *(--p);
                    }
                    //					Console.WriteLine("setting pDecPos to {0} sigDigits={1} scale={2}",sigDigits - scale,sigDigits,scale);
                    pDecPos = sigDigits - scale;
                }
                // add trailing zeros 
                for (i = len; i < digits; i++)
                {
                    pArray[q++] = '0';
                }
            }
            else
            { // count decimals 
                if (scale >= sigDigits)
                { // add leading zeros
                    if (decimals + 2 >= bufSize) return DECIMAL_BUFFER_OVERFLOW;
                    pDecPos = 1;
                    for (i = 0; i <= scale - sigDigits; i++)
                    {
                        pArray[q++] = '0';
                    }
                }
                else
                {
                    if (sigDigits - scale + decimals + 1 >= bufSize) return DECIMAL_BUFFER_OVERFLOW;
                    pDecPos = sigDigits - scale;
                }

                // copy significant digits 
                for (i = 0; i < len; i++)
                {
                    //*q++ = *(--p);
                    pArray[q++] = tmp[--p];
                }

                // add trailing zeros 
                for (i = scale; i < decimals; i++)
                {
                    pArray[q++] = '0';
                }
            }
            pArray[q] = '\0';

            pSign = (sigDigits > 0) ? pA.Sign : 0; /* zero has positive sign */

            return DECIMAL_SUCCESS;
        }

        internal static bool decimal2HexString(Decimal A, Int32 digits, bool lowerCase, out String hex)
        {
            char[] lDigits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
            char[] uDigits = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
            char[] hDigits = lowerCase ? lDigits : uDigits;
            StringBuilder sb = new StringBuilder(25);
            UInt32 word, nibble;

            //Console.WriteLine("in decimal2HexString: hi {0} mid {1} lo {2} ss {3}", A.hi32, A.mid32, A.lo32, A.ss32);
            //Console.WriteLine("in decimal2HexString: hi {0} mid {1} lo {2} ss {3}", A.hi32.ToString("x"), A.mid32.ToString("x"), A.lo32.ToString("x"), A.ss32.ToString("x"));

            hex = String.Empty;
            if (A.Sign != 0)
                hex = "-";
            if (/* A.Sign != 0 || */ A.Scale != 0)
                return true;

            for (int j = 0; j < 3; ++j)
            {
                word = (j == 0) ? A.hi32 : (j == 1) ? A.mid32 : A.lo32;
                if (word == 0)
                {
                    sb.Append('0', 8);
                    continue;
                }
                if (word == UInt32.MaxValue)
                {
                    sb.Append(hDigits[15], 8);
                    continue;
                }
                for (int i = 0; i < 8; ++i)
                {
                    nibble = (word >> 28) & 0x0F;
                    sb.Append(hDigits[nibble]);
                    word <<= 4;
                }
            }
            digits = (digits == 0) ? 1 : digits;
            hex += sb.ToString().TrimStart('0').PadLeft(digits, '0');
            return false;
        }

        //======================================================
        // Private helper routines
        //	

        private static void Unpack(ref Decimal D, out UInt64 lo, out UInt64 hi,
            out int sign, out int scale)
        {

            lo = (((UInt64)D.mid32) << 32) | D.lo32;
            hi = D.hi32;
            sign = (D.ss32 & SIGN_FLAG32) == 0 ? 0 : 1;
            scale = ((int)(D.ss32 & SCALE_MASK)) >> SCALE_SHIFT;
        }
        private static void DECTO128(ref Decimal D, out UInt64 lo, out UInt64 hi)
        {
            lo = (((UInt64)D.mid32) << 32) | D.lo32;
            hi = D.hi32;
        }


        private static void add128(UInt64 alo, UInt64 ahi, UInt64 blo, UInt64 bhi,
            out UInt64 clo, out UInt64 chi)
        {

            alo += blo;
            if (alo < blo) ahi++; /* carry */
            ahi += bhi;

            clo = alo;
            chi = ahi;
        }
        // 192 bit addition: c = a + b 
        //   addition is modulo 2**192, any carry is lost 
        private static void add192(UInt64 alo, UInt64 ami, UInt64 ahi,
            UInt64 blo, UInt64 bmi, UInt64 bhi,
            ref UInt64 pclo, ref UInt64 pcmi, ref UInt64 pchi)
        {
            alo += blo;
            if (alo < blo)
            { // carry low 
                ami++;
                if (ami == 0) ahi++; // carry mid
            }
            ami += bmi;
            if (ami < bmi) ahi++; // carry mid 
            ahi += bhi;
            pclo = alo;
            pcmi = ami;
            pchi = ahi;
        }

        private static void sub128(UInt64 alo, UInt64 ahi, UInt64 blo, UInt64 bhi,
            out UInt64 clo, out UInt64 chi)
        {
            //            Console.WriteLine("  entered sub128 alo={0},blo={1}", alo, blo);
            UInt64 dlo = alo - blo;
            UInt64 dhi = ahi - bhi;
            //            Console.WriteLine("  inside sub128 {0} {1} alo={2},blo={3}", dlo, dhi,alo,blo);
            if (alo < blo) dhi--; /* borrow */

            clo = dlo;
            chi = dhi;
        }

        private static int normalize128(ref UInt64 clo, ref UInt64 chi,
            ref int pScale, bool roundFlag, bool roundBit)
        {
            UInt32 overhang = (UInt32)(chi >> 32);
            int scale = pScale;
            int deltaScale;
            UInt32 rest;

            while (overhang != 0)
            {
                for (deltaScale = 1; deltaScale < constantsDecadeInt32Factors.Length; deltaScale++)
                {
                    if (overhang < constantsDecadeInt32Factors[deltaScale]) break;
                }

                scale -= deltaScale;
                if (scale < 0) return DECIMAL_OVERFLOW;

                roundBit = div128by32(ref clo, ref chi, constantsDecadeInt32Factors[deltaScale], out rest);

                overhang = (UInt32)(chi >> 32);
                if (roundFlag && roundBit && clo == 0xFFFFffffFFFFffff
                    && (Int32)chi == -1)
                {
                    overhang = 1;
                }
            }

            pScale = scale;

            if (roundFlag && roundBit)
            {
                roundUp128(ref clo, ref chi);
                //TEST((chi >> 32) == 0);
            }

            return DECIMAL_SUCCESS;
        }

        private static void roundUp128(ref UInt64 clo, ref UInt64 chi)
        {
            if (++clo == 0) ++chi;
        }

        // division: x(192bit) /= factor(32bit) 
        //   no rest and no rounding
        private static void div192by32(ref UInt64 plo, ref UInt64 pmi, ref UInt64 phi,
            UInt32 factor)
        {
            UInt64 a, b, c, h;

            h = phi;
            a = (UInt32)(h >> 32);
            b = a / factor;
            a -= b * factor;
            a <<= 32;
            a |= (UInt32)h;
            c = a / factor;
            a -= c * factor;
            a <<= 32;
            phi = b << 32 | (UInt32)c;

            h = pmi;
            a |= (UInt32)(h >> 32);
            b = a / factor;
            a -= b * factor;
            a <<= 32;
            a |= (UInt32)h;
            c = a / factor;
            a -= c * factor;
            a <<= 32;
            pmi = b << 32 | (UInt32)c;

            h = plo;
            a |= (UInt32)(h >> 32);
            b = a / factor;
            a -= b * factor;
            a <<= 32;
            a |= (UInt32)h;
            c = a / factor;
            a -= c * factor;
            a <<= 32;
            plo = b << 32 | (UInt32)c;
        }


        // division: x(128bit) /= factor(32bit) 
        //  returns roundBit 
        private static bool div128by32(ref UInt64 plo, ref UInt64 phi, UInt32 factor, out UInt32 pRest)
        {
            UInt64 a, b, c, h;

            // See if we can call an optimized version
            if (phi.m_hi == 0 && factor <= 0xffff)
            {
                //Console.WriteLine ("divide short cut: {0}", factor);
                return div96by16(ref plo, ref phi, factor, out pRest);
            }

            //Console.WriteLine("plo {0} phi {1} factor {2}", plo.ToString("x"), phi.ToString("x"), factor);
            h = phi;
            a = (UInt32)(h >> 32);
            b = a / factor;
            a -= b * factor;
            a <<= 32;
            a |= (UInt32)h;
            c = a / factor;
            a -= c * factor;
            a <<= 32;
            phi = b << 32 | (UInt32)c;

            h = plo;
            //Console.WriteLine("Step1 a.Lo={0} plo.Lo={1} h.Lo={2}",a.m_lo,plo.m_lo,h.m_lo);

            a |= (UInt32)(h >> 32);

            //Console.WriteLine("Step2 a.Lo={0} plo.Lo={1} h.Lo={2} factor={3}",a.m_lo,plo.m_lo,h.m_lo,factor);


            b = a / factor;
            a -= b * factor;
            a <<= 32;
            a |= (UInt32)h;

            //Console.WriteLine("Step3 a.Lo={0} ",a.m_lo);

            c = a / factor;

            //Console.WriteLine("Step4 c.Lo={0} ",c.m_lo);
            a -= c * factor;

            //Console.WriteLine("Step5 a={0} b={1} c={2}",a.m_lo,b.m_lo,c.m_lo);

            plo = b << 32 | (UInt32)c;

            pRest = (UInt32)a;
            //Console.WriteLine("Step6 rest={0}", pRest);

            a <<= 1;
            return (a >= factor || (a == factor && (c & 1) == 1)) ? true : false;
        }
        private static bool div96by16(ref UInt64 lo, ref UInt64 hi, UInt32 divisor, out UInt32 pRest)
        {
            bool b = div96by16(lo.m_lo, lo.m_hi, hi.m_lo, divisor,
                out lo.m_lo, out lo.m_hi, out hi.m_lo, out pRest);
            return b;

        }
        private static bool div96by16(UInt32 plo, UInt32 pmi, UInt32 phi,
            UInt32 divisor,
            out UInt32 qlo, out UInt32 qmi, out UInt32 qhi,
            out UInt32 pRest)
        {

            UInt32 a, b, c;

            a = (UInt16)(phi >> 16);
            b = a / divisor;
            a -= b * divisor;
            a <<= 16;
            a |= (UInt16)phi;
            c = a / divisor;
            a -= c * divisor;
            a <<= 16;
            qhi = b << 16 | (UInt16)c;

            a |= (UInt16)(pmi >> 16);
            b = a / divisor;
            a -= b * divisor;
            a <<= 16;
            a |= (UInt16)pmi;
            c = a / divisor;
            a -= c * divisor;
            a <<= 16;
            qmi = b << 16 | (UInt16)c;


            a |= (UInt16)(plo >> 16);
            b = a / divisor;
            a -= b * divisor;
            a <<= 16;
            a |= (UInt16)plo;
            c = a / divisor;
            a -= c * divisor;
            qlo = b << 16 | (UInt16)c;

            pRest = (UInt32)(UInt16)a;

            a <<= 1;
            return (a >= divisor || (a == divisor && (c & 1) == 1)) ? true : false;
        }

        [Conditional("DEBUG")]
        private static void Assert(bool condition)
        {
            if (!condition)
                throw new InvalidOperationException();
        }

        private static int pack128toDecimal(ref Decimal A, UInt64 alo, UInt64 ahi, int scale, int sign)
        {
            Assert((ahi >> 32) == 0);
            Assert(sign == 0 || sign == 1);
            Assert(scale >= 0 && scale <= DECIMAL_MAX_SCALE);

            if (scale < 0 || scale > DECIMAL_MAX_SCALE || (ahi >> 32) != 0)
            {
                return DECIMAL_OVERFLOW;
            }

            A.lo32 = (UInt32)alo;
            A.mid32 = (UInt32)(alo >> 32);
            A.hi32 = (UInt32)ahi;
            A.ss32 = ((UInt32)scale) << SCALE_SHIFT;
            //            Console.WriteLine("  packing {0}:{1} to {2},{3},{4},{5} scale={6}", ahi, alo, A.lo32, A.mid32, A.hi32, A.ss32,scale);
            if (sign != 0)
                A.ss32 |= SIGN_FLAG32;

            return DECIMAL_SUCCESS;
        }
        // returns log2(a) or DECIMAL_LOG_NEGINF for a = 0 
        private static int log2_32(UInt32 a)
        {
            int tlog2 = 0;

            if (a == 0) return DECIMAL_LOG_NEGINF;

            if ((a >> 16) != 0)
            {
                a >>= 16;
                tlog2 += 16;
            }
            if ((a >> 8) != 0)
            {
                a >>= 8;
                tlog2 += 8;
            }
            if ((a >> 4) != 0)
            {
                a >>= 4;
                tlog2 += 4;
            }
            if ((a >> 2) != 0)
            {
                a >>= 2;
                tlog2 += 2;
            }
            if ((a >> 1) != 0)
            {
                a >>= 1;
                tlog2 += 1;
            }
            tlog2 += (int)a;

            return tlog2;
        }

        // returns log2(a) or DECIMAL_LOG_NEGINF for a = 0
        private static int log2_64(UInt64 a)
        {
            int tlog2 = 0;

            //Console.WriteLine("a 0x{0}", a.ToString("X"));
            if (a == 0)
                return DECIMAL_LOG_NEGINF;

            if ((a >> 32) != 0)
            {
                a >>= 32;
                tlog2 += 32;
                //Console.WriteLine("shift: 32  a 0x{0} tlog2 {1}", a.ToString("x"), tlog2);
            }
            if ((a >> 16) != 0)
            {
                a >>= 16;
                tlog2 += 16;
                //Console.WriteLine("shift: 16  a 0x{0} tlog2 {1}", a.ToString("x"), tlog2);
            }
            if ((a >> 8) != 0)
            {
                a >>= 8;
                tlog2 += 8;
                //Console.WriteLine("shift:  8  a 0x{0} tlog2 {1}", a.ToString("x"), tlog2);
            }
            if ((a >> 4) != 0)
            {
                a >>= 4;
                tlog2 += 4;
                //Console.WriteLine("shift:  4  a 0x{0} tlog2 {1}", a.ToString("x"), tlog2);
            }
            if ((a >> 2) != 0)
            {
                a >>= 2;
                tlog2 += 2;
                //Console.WriteLine("shift:  2  a 0x{0} tlog2 {1}", a.ToString("x"), tlog2);
            }
            if ((a >> 1) != 0)
            {
                a >>= 1;
                tlog2 += 1;
                //Console.WriteLine("shift:  1  a 0x{0} tlog2 {1}", a.ToString("x"), tlog2);
            }
            tlog2 += (int)a;

            return tlog2;
        }


        // returns log2(a) or DECIMAL_LOG_NEGINF for a = 0 
        private static int log2_128(UInt64 alo, UInt64 ahi)
        {
            if (ahi == 0) return log2_64(alo);
            else return log2_64(ahi) + 64;
        }

        // returns a upper limit for log2(a) considering scale
        private static int log2withScale_128(UInt64 alo, UInt64 ahi, int scale)
        {
            int tlog2 = log2_128(alo, ahi);
            if (tlog2 < 0) tlog2 = 0;
            return tlog2 - (scale * 33219) / 10000;
        }

        private static int adjustScale128(ref UInt64 alo, ref UInt64 ahi, int deltaScale)
        {
            int idx, rc;
            UInt32 rest;

            if (deltaScale < 0)
            {
                deltaScale *= -1;
                if (deltaScale > DECIMAL_MAX_SCALE) return DECIMAL_INTERNAL_ERROR;
                while (deltaScale > 0)
                {
                    idx = (deltaScale > DECIMAL_MAX_INTFACTORS) ? DECIMAL_MAX_INTFACTORS : deltaScale;
                    deltaScale -= idx;
                    div128by32(ref alo, ref ahi, constantsDecadeInt32Factors[idx], out rest);
                }
            }
            else if (deltaScale > 0)
            {
                if (deltaScale > DECIMAL_MAX_SCALE) return DECIMAL_INTERNAL_ERROR;
                while (deltaScale > 0)
                {
                    idx = (deltaScale > DECIMAL_MAX_INTFACTORS) ? DECIMAL_MAX_INTFACTORS : deltaScale;
                    deltaScale -= idx;
                    rc = mult128by32(ref alo, ref ahi, constantsDecadeInt32Factors[idx], false);
                    if (rc != DECIMAL_SUCCESS) return rc;
                }
            }

            return DECIMAL_SUCCESS;
        }
        // multiplication c(128bit) *= b(32bit) 
        private static int mult128by32(ref UInt64 clo, ref UInt64 chi, UInt32 factor,
            bool roundBit)
        {
            UInt64 a;
            UInt32 h0, h1;

            a = ((UInt64)(UInt32)clo) * factor;
            if (roundBit)
                a += factor / 2;
            h0 = (UInt32)a;

            a >>= 32;
            a += (clo >> 32) * factor;
            h1 = (UInt32)a;

            clo = ((UInt64)h1) << 32 | h0;

            a >>= 32;
            a += ((UInt64)(UInt32)chi) * factor;
            h0 = (UInt32)a;

            a >>= 32;
            a += (chi >> 32) * factor;
            h1 = (UInt32)a;

            chi = ((UInt64)h1) << 32 | h0;

            return ((a >> 32) == 0) ? DECIMAL_SUCCESS : DECIMAL_OVERFLOW;
        }
        // multiplication c(128bit) = a(96bit) * b(32bit) 
        private static void mult96by32to128(UInt32 alo, UInt32 ami, UInt32 ahi, UInt32 factor,
            out UInt64 pclo, out UInt64 pchi)
        {
            UInt64 a;
            UInt32 h0, h1;

            a = ((UInt64)alo) * factor;
            h0 = (UInt32)a;

            a >>= 32;
            a += ((UInt64)ami) * factor;
            h1 = (UInt32)a;

            a >>= 32;
            a += ((UInt64)ahi) * factor;

            pclo = ((UInt64)h1) << 32 | h0;
            pchi = a;
        }


        // input: c * 10^-(*pScale) * 2^-exp
        //   output: c * 10^-(*pScale) with 
        //   minScale <= *pScale <= maxScale and (chi >> 32) == 0 
        private static int rescale128(ref UInt64 pclo, ref UInt64 pchi, ref int pScale,
            int texp, int minScale, int maxScale, bool roundFlag)
        {
            UInt32 factor, overhang, rest;
            int scale, i, rc;
            bool roundBit = false;
            bool b;

            Assert(texp >= 0);

            scale = pScale;

            if (texp > 0)
            {
                // reduce exp 
                while (texp > 0 && scale <= maxScale)
                {
                    overhang = (UInt32)(pchi >> 32);
                    while (texp > 0 && ((pclo & 1) == 0 || overhang > (2 << DECIMAL_MAX_INTFACTORS)))
                    {
                        if (--texp == 0)
                            roundBit = (pclo & 1) != 0;
                        rshift128(ref pclo, ref pchi);
                        overhang = (UInt32)(pchi >> 32);
                    }

                    if (texp > DECIMAL_MAX_INTFACTORS) i = DECIMAL_MAX_INTFACTORS;
                    else i = texp;
                    if (scale + i > maxScale) i = maxScale - scale;
                    if (i == 0) break;
                    texp -= i;
                    scale += i;
                    factor = constantsDecadeInt32Factors[i] >> i; // 10^i/2^i=5^i 
                    mult128by32(ref pclo, ref pchi, factor, false);
                    //printf("3: %.17e\n", (((double)chi) * pow(2,64) + clo) * pow(10, -scale) * pow(2, -texp));
                }

                while (texp > 0)
                {
                    if (--texp == 0)
                        roundBit = (pclo & 1) != 0;
                    rshift128(ref pclo, ref pchi);
                }
            }

            Assert(texp == 0);

            while (scale > maxScale)
            {
                i = scale - maxScale;
                if (i > DECIMAL_MAX_INTFACTORS) i = DECIMAL_MAX_INTFACTORS;
                scale -= i;
                b = div128by32(ref pclo, ref pchi, constantsDecadeInt32Factors[i], out rest);
                roundBit = b ? true : false;
            }

            while (scale < minScale)
            {
                if (roundFlag) roundBit = false;
                i = minScale - scale;
                if (i > DECIMAL_MAX_INTFACTORS) i = DECIMAL_MAX_INTFACTORS;
                scale += i;
                rc = mult128by32(ref pclo, ref pchi, constantsDecadeInt32Factors[i], roundBit);
                if (rc != DECIMAL_SUCCESS) return rc;
                roundBit = false;
            }

            Assert(scale >= 0 && scale <= DECIMAL_MAX_SCALE);

            pScale = scale;

            b = roundBit;
            return normalize128(ref pclo, ref pchi, ref pScale, roundFlag, b);
        }

        private static void rshift128(ref UInt64 pclo, ref UInt64 pchi)
        {
            pclo >>= 1;
            if ((pchi & 1) != 0) pclo |= SIGN_FLAG64;
            pchi >>= 1;
        }

        private static void lshift128(ref UInt64 pclo, ref UInt64 pchi)
        {
            pchi <<= 1;
            if ((pclo & SIGN_FLAG64) != 0) pchi++;
            pclo <<= 1;
        }
        private static void lshift96(ref UInt32 pclo, ref UInt32 pcmid, ref UInt32 pchi)
        {
            pchi <<= 1;
            if ((pcmid & SIGN_FLAG32) != 0) pchi++;
            pcmid <<= 1;
            if ((pclo & SIGN_FLAG32) != 0) pcmid++;
            pclo <<= 1;
        }


        // multiplication c(192bit) = a(96bit) * b(96bit)
        private static void mult96by96to192(UInt32 alo, UInt32 ami, UInt32 ahi,
            UInt32 blo, UInt32 bmi, UInt32 bhi,
            out UInt64 pclo, out UInt64 pcmi, out UInt64 pchi)
        {

            UInt64 a, b, c, d;
            UInt32 h0, h1, h2, h3, h4, h5;
            UInt32 carry0, carry1;

            a = ((UInt64)alo) * blo;
            h0 = (UInt32)a;

            a >>= 32; carry0 = 0;
            b = ((UInt64)alo) * bmi;
            c = ((UInt64)ami) * blo;
            a += b; if (a < b) carry0++;
            a += c; if (a < c) carry0++;
            h1 = (UInt32)a;

            a >>= 32; carry1 = 0;
            b = ((UInt64)alo) * bhi;
            c = ((UInt64)ami) * bmi;
            d = ((UInt64)ahi) * blo;
            a += b; if (a < b) carry1++;
            a += c; if (a < c) carry1++;
            a += d; if (a < d) carry1++;
            h2 = (UInt32)a;

            a >>= 32; a += carry0; carry0 = 0;
            b = ((UInt64)ami) * bhi;
            c = ((UInt64)ahi) * bmi;
            a += b; if (a < b) carry0++;
            a += c; if (a < c) carry0++;
            h3 = (UInt32)a;

            a >>= 32; a += carry1;
            b = ((UInt64)ahi) * bhi;
            a += b;
            h4 = (UInt32)a;

            a >>= 32; a += carry0;
            h5 = (UInt32)a;

            pclo = ((UInt64)h1) << 32 | h0;
            pcmi = ((UInt64)h3) << 32 | h2;
            pchi = ((UInt64)h5) << 32 | h4;
        }
        private static int decimalDivSub(Decimal A, Decimal B,
            out UInt64 pclo, out UInt64 pchi, out int pExp)
        {

            UInt64 alo, ami, ahi;
            UInt64 tlo, tmi, thi;
            UInt32 blo, bmi, bhi;
            int ashift, bshift, extraBit, texp;

            ahi = (((UInt64)(A.hi32)) << 32) | A.mid32;
            ami = ((UInt64)(A.lo32)) << 32;
            alo = 0;
            blo = B.lo32;
            bmi = B.mid32;
            bhi = B.hi32;

            pclo = pchi = 0;
            pExp = 0;
            if (blo == 0 && bmi == 0 && bhi == 0)
            {
                return DECIMAL_DIVIDE_BY_ZERO;
            }

            if (ami == 0 && ahi == 0)
            {
                return DECIMAL_FINISHED;
            }

            // enlarge dividend to get maximal precision 
            for (ashift = 0; (ahi & SIGN_FLAG64) == 0; ++ashift)
            {
                lshift128(ref ami, ref ahi);
            }

            // ensure that divisor is at least 2^95
            for (bshift = 0; (bhi & SIGN_FLAG32) == 0; ++bshift)
            {
                lshift96(ref blo, ref bmi, ref bhi);
            }

            thi = ((UInt64)bhi) << 32 | bmi;
            tmi = ((UInt64)blo) << 32;
            tlo = 0;
            if (ahi > thi || (ahi == thi && ami >= tmi))
            {
                sub192(alo, ami, ahi, tlo, tmi, thi, out alo, out ami, out ahi);
                extraBit = 1;
            }
            else
            {
                extraBit = 0;
            }

            div192by96to128(alo, ami, ahi, blo, bmi, bhi, out pclo, out pchi);
            texp = 128 + ashift - bshift;

            if (extraBit != 0)
            {
                rshift128(ref pclo, ref pchi);
                pchi += SIGN_FLAG64;
                texp--;
            }

            // try loss free right shift 
            while (texp > 0 && (pclo & 1) == 0)
            {
                // right shift 
                rshift128(ref pclo, ref pchi);
                texp--;
            }

            pExp = texp;

            return DECIMAL_SUCCESS;
        }
        // 192 bit subtraction: c = a - b
        //   subtraction is modulo 2**192, any carry is lost 
        private static void sub192(UInt64 alo, UInt64 ami, UInt64 ahi,
            UInt64 blo, UInt64 bmi, UInt64 bhi,
            out UInt64 pclo, out UInt64 pcmi, out UInt64 pchi)
        {
            UInt64 clo, cmi, chi;

            clo = alo - blo;
            cmi = ami - bmi;
            chi = ahi - bhi;
            if (alo < blo)
            {
                if (cmi == 0) chi--; // borrow mid
                cmi--; // borrow low
            }
            if (ami < bmi) chi--; // borrow mid 
            pclo = clo;
            pcmi = cmi;
            pchi = chi;
        }
        // c(128bit) = a(192bit) / b(96bit) 
        //   b must be >= 2^95 
        private static void div192by96to128(UInt64 alo, UInt64 ami, UInt64 ahi,
            UInt32 blo, UInt32 bmi, UInt32 bhi,
            out UInt64 pclo, out UInt64 pchi)
        {
            UInt64 rlo, rmi, rhi; // remainder 
            UInt32 h, c;

            //Assert(ahi < (((UInt64)bhi) << 32 | bmi) || (ahi == (((UInt64)bhi) << 32 | bmi) && (ami >> 32) > blo));

            /* high 32 bit*/
            rlo = alo; rmi = ami; rhi = ahi;
            h = div192by96to32withRest(ref rlo, ref rmi, ref rhi, blo, bmi, bhi);

            /* mid 32 bit*/
            rhi = (rhi << 32) | (rmi >> 32); rmi = (rmi << 32) | (rlo >> 32); rlo <<= 32;
            pchi = (((UInt64)h) << 32)
                | div192by96to32withRest(ref rlo, ref rmi, ref rhi, blo, bmi, bhi);

            /* low 32 bit */
            rhi = (rhi << 32) | (rmi >> 32); rmi = (rmi << 32) | (rlo >> 32); rlo <<= 32;
            h = div192by96to32withRest(ref rlo, ref rmi, ref rhi, blo, bmi, bhi);

            // estimate lowest 32 bit (two last bits may be wrong) 
            if (rhi >= bhi)
            {
                c = 0xFFFFFFFF;
            }
            else
            {
                rhi <<= 32;
                c = (UInt32)(rhi / bhi);
            }
            pclo = (((UInt64)h) << 32) | c;
        }
        // returns upper 32bit for a(192bit) /= b(32bit)
        //	a will contain remainder
        private static UInt32 div192by96to32withRest(ref UInt64 palo, ref UInt64 pami, ref UInt64 pahi,
            UInt32 blo, UInt32 bmi, UInt32 bhi)
        {
            UInt64 rlo, rmi, rhi; // remainder 
            UInt64 tlo, thi; // term 
            UInt32 c;

            rlo = palo; rmi = pami; rhi = pahi;
            if (rhi >= (((UInt64)bhi) << 32))
            {
                c = 0xFFFFFFFF;
            }
            else
            {
                c = (UInt32)(rhi / bhi);
            }
            mult96by32to128(blo, bmi, bhi, c, out tlo, out thi);
            sub192(rlo, rmi, rhi, 0, tlo, thi, out rlo, out rmi, out rhi);
            while (((Int64)rhi) < 0)
            {
                c--;
                add192(rlo, rmi, rhi, 0, (((UInt64)bmi) << 32) | blo, bhi, ref rlo, ref rmi, ref rhi);
            }
            palo = rlo; pami = rmi; pahi = rhi;

            //Assert(rhi >> 32 == 0);

            return c;
        }

        private static bool decimalIsZero(Decimal A)
        {
            return (A.lo32 == 0 && A.mid32 == 0 && A.hi32 == 0);
        }

        // approximation for log2 of a 
        //   If q is the exact value for log2(a), then q <= decimalLog2(a) <= q+1 
        private static int decimalLog2(Decimal A)
        {
            int tlog2;
            int scale = A.Scale;

            if (A.hi32 != 0) tlog2 = 64 + log2_32(A.hi32);
            else if (A.mid32 != 0) tlog2 = 32 + log2_32(A.mid32);
            else tlog2 = log2_32(A.lo32);

            if (tlog2 != DECIMAL_LOG_NEGINF)
            {
                tlog2 -= (scale * 33219) / 10000;
            }

            return tlog2;
        }

        // performs a += factor * constants[idx] 
        private static int incMultConstant128(ref UInt64 palo, ref UInt64 pahi, int idx, int factor)
        {
            UInt64 blo, bhi, h;

            Assert(idx >= 0 && idx <= DECIMAL_MAX_SCALE);
            Assert(factor > 0 && factor <= 9);

            blo = dec128decadeFactors[idx].lo;
            h = bhi = dec128decadeFactors[idx].hi;
            if (factor != 1)
            {
                mult128by32(ref blo, ref bhi, (UInt32)factor, false);
                if (h > bhi)
                    return DECIMAL_OVERFLOW;
            }
            h = pahi;
            add128(palo, pahi, blo, bhi, out palo, out pahi);
            if (h > pahi)
                return DECIMAL_OVERFLOW;
            return DECIMAL_SUCCESS;
        }

        private static void div128DecadeFactor(ref UInt64 palo, ref UInt64 pahi, int powerOfTen)
        {
            int idx;
            UInt32 rest;
            bool roundBit = false;

            while (powerOfTen > 0)
            {
                idx = (powerOfTen > DECIMAL_MAX_INTFACTORS) ? DECIMAL_MAX_INTFACTORS : powerOfTen;
                powerOfTen -= idx;
                roundBit = div128by32(ref palo, ref pahi, constantsDecadeInt32Factors[idx], out rest);
            }

            if (roundBit) roundUp128(ref palo, ref pahi);
        }

        private static double ConvertToDouble(byte[] buffer, uint offset)
        {
            double result;
            bool isNegative = false;

            int exp = buffer[7 + offset];
            if ((exp & 128) != 0)
            {
                isNegative = true;
            }

            // exponent is 8 bits, shift sign bit out and 1 bit from 3rd byte in)
            exp = ((exp & 0x7F) << 4) | ((buffer[6 + offset] & 0xF0) >> 4);
            long m = ((buffer[6 + offset] & 0x0F));
            int shift;
            if (exp != 0 && exp != 0xFF)
            {
                // implied M=1.bbbbb
                m |= 0x10;
            }
            m = (m << 8) | buffer[5 + offset];
            m = (m << 8) | buffer[4 + offset];
            m = (m << 8) | buffer[3 + offset];
            m = (m << 8) | buffer[2 + offset];
            m = (m << 8) | buffer[1 + offset];
            m = (m << 8) | buffer[offset];
            shift = 52; // number of fractional bits

            if (exp == 0)
            {
                //e=-127+1;
                if (m == 0)
                    return 0.0;
                result = (m * System.Math.Pow(2, -1023 + 1 - shift));
                if (isNegative)
                    return -result;
                return result;
            }
            else if (exp == 0x7FF)
            {
                if (m == 0)
                    return 0.0f; // infinity
                else
                    return 0.0f; // Nan
            }
            else
            {
                result = (m * System.Math.Pow(2, exp - 1023 - shift));
                if (isNegative)
                    return -result;
                return result;
            }
        }
        // calc significant digits of mantisse 
        private static int calcDigits(UInt64 alo, UInt64 ahi)
        {
            int tlog2 = 0;
            int tlog10;
            //Console.WriteLine("calcdig entered");
            if (ahi == 0)
            {
                if (alo == 0)
                {
                    //Console.WriteLine("!calcdig1");
                    return 0; // zero has no signficant digits
                }
                else
                {
                    //Console.WriteLine("!calcdig2");
                    tlog2 = log2_64(alo);
                }
            }
            else
            {
                //Console.WriteLine("!calcdig3");
                tlog2 = 64 + log2_64(ahi);
            }

            tlog10 = (tlog2 * 1000) / 3322;

            //Console.WriteLine("!calcdig4");

            // we need an exact floor value of log10(a)
            if (dec128decadeFactors[tlog10].hi > ahi
                || (dec128decadeFactors[tlog10].hi == ahi && dec128decadeFactors[tlog10].lo > alo))
            {
                //				Console.WriteLine("!calcdig5");
                --tlog10;
            }
            //			Console.WriteLine("tlog10={0}  tlog2={1}",tlog10, tlog2);

            return tlog10 + 1;
        }


    } // EOClass
}
