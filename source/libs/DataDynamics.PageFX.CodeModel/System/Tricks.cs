namespace System
{
    //http://www.hackersdelight.org/HDcode

    public static class Tricks
    {
        /// <summary>
        /// Computes the number of leading zeros in a word.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int Nlz(uint x)
        {
            if (x == 0) return 32;
            int n = 0;
            if (x <= 0x0000FFFF) { n = n + 16; x = x << 16; }
            if (x <= 0x00FFFFFF) { n = n + 8; x = x << 8; }
            if (x <= 0x0FFFFFFF) { n = n + 4; x = x << 4; }
            if (x <= 0x3FFFFFFF) { n = n + 2; x = x << 2; }
            if (x <= 0x7FFFFFFF) { n = n + 1; }
            return n;
        }

        /// <summary>
        /// computes the number of 0-bits
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int Pop0(uint x)
        {
            x = (x & 0x55555555) + ((x >> 1) & 0x55555555);
            x = (x & 0x33333333) + ((x >> 2) & 0x33333333);
            x = (x & 0x0F0F0F0F) + ((x >> 4) & 0x0F0F0F0F);
            x = (x & 0x00FF00FF) + ((x >> 8) & 0x00FF00FF);
            x = (x & 0x0000FFFF) + ((x >> 16) & 0x0000FFFF);
            return (int)x;
        }

        /// <summary>
        /// computes the number of 1-bits
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int Pop1(uint x)
        {
            x = x - ((x >> 1) & 0x55555555);
            x = (x & 0x33333333) + ((x >> 2) & 0x33333333);
            x = (x + (x >> 4)) & 0x0F0F0F0F;
            x = x + (x << 8);
            x = x + (x << 16);
            return (int)(x >> 24);
        }

        /// <summary>
        /// Bitwise version of <see cref="Nlz"/>
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int NlzBitwise(uint x)
        {
            x = x | (x >> 1);
            x = x | (x >> 2);
            x = x | (x >> 4);
            x = x | (x >> 8);
            x = x | (x >> 16);
            return Pop1(~x);
        }

        /// <summary>
        /// Computes the number of trailing zeros
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static int Ntz(uint x)
        {
            if (x == 0) return 32;
            int n = 1;
            if ((x & 0x0000FFFF) == 0) { n = n + 16; x = x >> 16; }
            if ((x & 0x000000FF) == 0) { n = n + 8; x = x >> 8; }
            if ((x & 0x0000000F) == 0) { n = n + 4; x = x >> 4; }
            if ((x & 0x00000003) == 0) { n = n + 2; x = x >> 2; }
            return n - (int)(x & 1);
        }

        #region clp2
        /* Round up to a power of 2. */
        public static uint clp2(uint x)
        {
            x = x - 1;
            x = x | (x >> 1);
            x = x | (x >> 2);
            x = x | (x >> 4);
            x = x | (x >> 8);
            x = x | (x >> 16);
            return x + 1;
        }
        #endregion

        #region mulhs
        /// <summary>
        /// Computes the high-order half of the 64-bit product.
        /// Max line length is 57, to fit in hacker.book.
        /// Derived from Knuth's Algorithm M, altered for signed multiplication.
        /// Subscript 0 denotes the least significant half (little endian).
        /// </summary>
        /// <param name="u"></param>
        /// <param name="v"></param>
        /// <returns></returns>
        public static int mulhs(int u, int v)
        {
            uint u0, v0, w0;
            int u1, v1, w1, w2, t;
            u0 = (uint)(u & 0xFFFF);
            u1 = u >> 16;
            v0 = (uint)(v & 0xFFFF);
            v1 = v >> 16;
            w0 = u0 * v0;
            t = (int)(u1 * v0 + (w0 >> 16));
            w1 = t & 0xFFFF;
            w2 = t >> 16;
            w1 = (int)(u0 * v1 + w1);
            return u1 * v1 + w2 + (w1 >> 16);
        }
        #endregion

        #region mulhu
        // Computes the high-order half of the 64-bit product, uint.
        // Max line length is 57, to fit in hacker.book. (But not used there.)
        // Derived from Knuth's Algorithm M.
        // Subscript 0 denotes the least significant half (little endian).
        public static uint mulhu(uint u, uint v)
        {
            uint u0, u1, v0, v1, w0, w1, w2, t;
            u0 = u & 0xFFFF;
            u1 = u >> 16;
            v0 = v & 0xFFFF;
            v1 = v >> 16;
            w0 = u0 * v0;
            t = u1 * v0 + (w0 >> 16);
            w1 = t & 0xFFFF;
            w2 = t >> 16;
            w1 = u0 * v1 + w1;
            return u1 * v1 + w2 + (w1 >> 16);
        }
        #endregion

        #region divbm2
        // Base -2 division, 32/32 ==> 32 bit.
        // Inputs n and d are 2's-complement integers and the result is a base-2 integer.
        // Max line length is 57, to fit in hacker.book.
        public static int divbm2(int n, int d)
        {
            // q = n/d in base -2.
            int r, dw, c, q, i;

            r = n; // Init. remainder.
            dw = (-128) * d; // Position d.
            c = (-43) * d; // Init. comparand.
            if (d > 0) c = c + d;
            q = 0; // Init. quotient.
            for (i = 7; i >= 0; i--)
            {
                if (d > 0 ^ (i & 1) == 0 ^ r >= c)
                {
                    q = q | (1 << i); // Set a quotient bit.
                    r = r - dw; // Subtract d shifted.
                }
                dw = dw / (-2); // Position d.
                if (d > 0) c = c - 2 * d; // Set comparand for
                else c = c + d; // next iteration.
                c = c / (-2);
            }
            return q; // Return quotient in base -2. Remainder is r, 0 <= r < |d|.
        }
        #endregion

        #region divlu & divls
        // Long division, signed (64/32 ==> 32).
        // A procedure below performs signed long division (64/32 ==> 32) using
        // a procedure for uint long division.  In the overflow cases (divide
        // by 0, or quotient exceeds 32 bits), it returns a remainder of 0x80000000
        // (an impossible value), and for good measure a quotient of 0x80000000.
        // The dividend is u1 and u0, with u1 being the most significant word.
        // The divisor is parameter v. The value returned is the quotient.
        // Max line length is 57, to fit in hacker.book.
        
        /* Here is the uint long division routine.  It is divlu2 in
        divlu.cc. */

        public static uint divlu(uint u1, uint u0, uint v, out uint r)
        {
            const uint b = 65536; // Number base (16 bits).
            uint un1, un0; // Norm. dividend LSD's.
            uint vn1, vn0; // Norm. divisor digits.
            uint q1, q0; // Quotient digits.
            uint un32, un21, un10; // Dividend digit pairs.
            uint rhat; // A remainder.
            int s; // Shift amount for norm.

            if (u1 >= v)
            {
                // If overflow, set rem.
                // to an impossible value,
                // and return the largest
                r = 0xFFFFFFFF;
                return 0xFFFFFFFF;
            } // possible quotient.

            s = Nlz(v); // 0 <= s <= 31.
            v = v << s; // Normalize divisor.
            vn1 = v >> 16; // Break divisor up into
            vn0 = v & 0xFFFF; // two 16-bit digits.

            un32 = (uint)((u1 << s) | (u0 >> 32 - s) & (-s >> 31));
            un10 = u0 << s; // Shift dividend left.

            un1 = un10 >> 16; // Break right half of
            un0 = un10 & 0xFFFF; // dividend into two digits.

            q1 = un32 / vn1; // Compute the first
            rhat = un32 - q1 * vn1; // quotient digit, q1.
            again1:
            if (q1 >= b || q1 * vn0 > b * rhat + un1)
            {
                q1 = q1 - 1;
                rhat = rhat + vn1;
                if (rhat < b) goto again1;
            }

            un21 = un32 * b + un1 - q1 * v; // Multiply and subtract.

            q0 = un21 / vn1; // Compute the second
            rhat = un21 - q0 * vn1; // quotient digit, q0.
            again2:
            if (q0 >= b || q0 * vn0 > b * rhat + un0)
            {
                q0 = q0 - 1;
                rhat = rhat + vn1;
                if (rhat < b) goto again2;
            }

            r = (un21 * b + un0 - q0 * v) >> s; // return it.
            return q1 * b + q0;
        }

        /* Here is the signed long division routine, for inclusion in the Hacker book. */

        // ------------------------------ cut ----------------------------------
        public static int divls(int u1, uint u0, int v, out int r)
        {
            int q, uneg, vneg, diff;

            uneg = u1 >> 31; // -1 if u < 0.
            if (uneg != 0)
            {
                // Compute the absolute
                u0 = (uint)-u0; // value of the dividend u.
                int borrow = (u0 != 0) ? 1 : 0;
                u1 = -u1 - borrow;
            }

            vneg = v >> 31; // -1 if v < 0.
            v = (v ^ vneg) - vneg; // Absolute value of v.

            if ((uint)u1 >= (uint)v)
                goto overflow;

            uint ur;
            q = (int)divlu((uint)u1, u0, (uint)v, out ur);
            r = (int)ur;

            diff = uneg ^ vneg; // Negate q if signs of
            q = (q ^ diff) - diff; // u and v differed.
            if (uneg != 0)
                r = -r;

            // possible neg. quotient.
            if (!((diff ^ q) < 0 && q != 0))
                return q;

            overflow:
            // If overflow, set remainder to an impossible value, and return the largest
            r = unchecked((int)0x80000000);
            q = unchecked((int)0x80000000);

            return q;
        }
        #endregion

        #region mulmnu
        // Computes the m+n-halfword product of n halfwords x m halfwords, unsigned.
        // Max line length is 57, to fit in hacker.book.

        // w[0], u[0], and v[0] contain the LEAST significant halfwords.
        // (The halfwords are in little-endian order).

        // This is Knuth's Algorithm M from [Knuth Vol. 2 Third edition (1998)]
        // section 4.3.1.  Picture is:
        //                   u[m-1] ... u[1] u[0]
        //                 x v[n-1] ... v[1] v[0]
        //                   --------------------
        //        w[m+n-1] ............ w[1] w[0]

        public static void mulmnu(ushort[] w, ushort[] u, ushort[] v, int m, int n)
        {
            int i, j;

            for (i = 0; i < m; i++)
                w[i] = 0;

            for (j = 0; j < n; j++)
            {
                uint k = 0;
                for (i = 0; i < m; i++)
                {
                    uint t = (uint)(u[i] * v[j] + w[i + j] + k);
                    w[i + j] = (ushort)t; // (I.e., t & 0xFFFF).
                    k = t >> 16;
                }
                w[j + m] = (ushort)k;
            }
        }
        #endregion

        #region mulmns
        // Computes the m+n-halfword product of n halfwords x m halfwords, signed.
        // Max line length is 57, to fit in hacker.book.
        // w[0], u[0], and v[0] contain the LEAST significant halfwords.
        // (The words are in little-endian order).
        // This is Knuth's Algorithm M from [Knuth Vol. 2 Third edition (1998)]
        // section 4.3.1, altered for signed numbers.  Picture is:
        //                   u[m-1] ... u[1] u[0]
        //                 x v[n-1] ... v[1] v[0]
        //                   --------------------
        //        w[m+n-1] ............ w[1] w[0]

        public static void mulmns(ushort[] w, ushort[] u, ushort[] v, int m, int n)
        {
            uint t, b;
            int i, j;

            for (i = 0; i < m; i++)
                w[i] = 0;

            for (j = 0; j < n; j++)
            {
                uint k = 0;
                for (i = 0; i < m; i++)
                {
                    t = (uint)(u[i] * v[j] + w[i + j] + k);
                    w[i + j] = (ushort)t; // (I.e., t & 0xFFFF).
                    k = t >> 16;
                }
                w[j + m] = (ushort)k;
            }

            // Now w[] has the unsigned product.  Correct by
            // subtracting v*2**16m if u < 0, and
            // subtracting u*2**16n if v < 0.

            if ((short)u[m - 1] < 0)
            {
                b = 0; // Initialize borrow.
                for (j = 0; j < n; j++)
                {
                    t = (uint)(w[j + m] - v[j] - b);
                    w[j + m] = (ushort)t;
                    b = t >> 31;
                }
            }
            if ((short)v[n - 1] < 0)
            {
                b = 0;
                for (i = 0; i < m; i++)
                {
                    t = (uint)(w[i + n] - u[i] - b);
                    w[i + n] = (ushort)t;
                    b = t >> 31;
                }
            }
        }
        #endregion
    }
}