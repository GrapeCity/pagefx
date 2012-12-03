//refs:
//http://en.wikipedia.org/wiki/IEEE_754         - IEEE 754-1985
//http://en.wikipedia.org/wiki/Exponent_bias    - Exponent bias

//http://support.microsoft.com/kb/36068
//INFO: IEEE Floating-Point Representation and MS Languages
//The values are stored as follows:
//   real*4  sign bit, 8  bit exponent, 23 bit mantissa
//   real*8  sign bit, 11 bit exponent, 52 bit mantissa
//   real*10 sign bit, 15 bit exponent, 64 bit mantissa

//The exponents are biased as follows:
//   8-bit  (real*4)  exponents are biased by 127
//   11-bit (real*8)  exponents are biased by 1023
//   15-bit (real*10) exponents are biased by 16383

using System;

namespace DataDynamics.PageFX.Common.Utilities
{
    /// <summary>
    /// IEEEFloat is used to decode an IEEE float byte stream into a float/double
    /// There is no .NET framework support for this.
    /// The only code that could accomplish this is unsafe (*(double *)....
    /// </summary>
    public class IEEEFloat
    {
        /// Background info
        /// 
        /// IEEE floating point syntax (IEEE 754)
        /// ... b3 b2 b1 . b-1 b-2 b-3 ...
        /// Bits to the right of the decimal point are fractional powers of 2
        /// 
        /// So  5+3/4 =   101.11
        ///		2+7/8 =	  10.111
        ///	
        ///	The binary representation is :
        ///		s  exp  frac
        ///		
        ///	- MSB is sign bit
        ///	- Single precision : 8 bits exponent, 23 frac bits
        ///	- Double precision : 11 bits exponent, 52 frac bits
        ///	- Extended : 15 exp , 63 frac bits
        ///	
        ///	If exponent is all 00000s
        ///		E=-Bias+1
        ///		M=0.xxxxxxx
        ///	
        ///		if exp=0, m=0 -> represents 0 value
        ///	
        ///	If exponent is all 1111s
        ///		if frac=0.0000 ->  infinity
        ///		else NaN
        ///	
        ///	else
        ///		Exponent is coded as biased value
        ///			Single prec. : Bias= 127   E=EXP-Bias
        ///			Double prec. : Bias=1023
        ///		M=1.xxxxxxxx (implied 1 , we get one more bit)
        ///	
        ///	
        // MS Binary Format                         
        // byte order =>    m3 | m2 | m1 | exponent 
        // m1 is most significant byte => sbbb|bbbb 
        // m3 is the least significant byte         
        //      m = mantissa byte                   
        //      s = sign bit                        
        //      b = bit   
        //
        //If E=255 and m is nonzero, then V=NaN ("Not a number") 
        //If E=255 and m is zero and S is 1, then V=-Infinity 
        //If E=255 and m is zero and S is 0, then V=Infinity 
        public static float ToFloat(byte[] buffer, int offset)
        {
            float result;
            int exp = buffer[3 + offset];
            bool isNegative = (exp & 0x80) != 0;
            
            // exponent is 8 bits, shift sign bit out and 1 bit from 3rd byte in)
            exp = ((exp & 0x7F) << 1) | ((buffer[2 + offset] & 0x80) != 0 ? 1 : 0);
            int m = ((buffer[2 + offset] & 0x7F));
            int shift;
            if (exp != 0 && exp != 0xFF)
            {
                // implied M=1.bbbbb
                m |= 0x80;
            }
            m = (m << 8) | buffer[1 + offset];
            m = (m << 8) | buffer[offset];
            shift = 23;

            if (exp == 0)
            {
                if (m == 0) return 0f;
                result = (float)(m * Math.Pow(2, -126 - shift));
                return isNegative ? -result : result;
            }
            if (exp == 0xFF)
            {
                if (m == 0)
                    return isNegative ? Single.NegativeInfinity : Single.PositiveInfinity;
                return Single.NaN;
            }
            result = (float)(m * Math.Pow(2, exp - 127 - shift));
            return isNegative ? -result : result;
        }

        public static double ToDouble(byte[] buffer, int offset)
        {
            double result;
            
            int exp = buffer[7 + offset];
            bool isNegative = (exp & 0x80) != 0;
            
            // exponent is 8 bits, shift sign bit out and 1 bit from 3rd byte in)
            exp = ((exp & 0x7F) << 4) | ((buffer[6 + offset] & 0xF0) >> 4);
            long m = ((buffer[6 + offset] & 0x0F));
            int shift;
            if (exp != 0 && exp != 0x7FF)
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
                if (m == 0) return 0;
                result = (m * Math.Pow(2, -1022 - shift));
                return isNegative ? -result : result;
            }
            if (exp == 0x7FF)
            {
                if (m == 0)
                    return isNegative ? Double.NegativeInfinity : Double.PositiveInfinity;
                return Double.NaN;
            }
            result = (m * Math.Pow(2, exp - 1023 - shift));
            return isNegative ? -result : result;
        }
    }
}