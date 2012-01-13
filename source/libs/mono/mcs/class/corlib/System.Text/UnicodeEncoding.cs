//CHANGED
/*
 * UnicodeEncoding.cs - Implementation of the
 *		"System.Text.UnicodeEncoding" class.
 *
 * Copyright (c) 2001, 2002  Southern Storm Software, Pty Ltd
 * Copyright (C) 2003, 2004 Novell, Inc.
 * Copyright (C) 2006 Kornél Pál <http://www.kornelpal.hu/>
 *
 * Permission is hereby granted, free of charge, to any person obtaining
 * a copy of this software and associated documentation files (the "Software"),
 * to deal in the Software without restriction, including without limitation
 * the rights to use, copy, modify, merge, publish, distribute, sublicense,
 * and/or sell copies of the Software, and to permit persons to whom the
 * Software is furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included
 * in all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS
 * OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL
 * THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR
 * OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
 * ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
 * OTHER DEALINGS IN THE SOFTWARE.
 */

namespace System.Text
{

    using System;
    using System.Runtime.InteropServices;

    [Serializable]
#if NET_2_0
[ComVisible (true)]
#endif
    [MonoTODO("Serialization format not compatible with .NET")]
    public class UnicodeEncoding : Encoding
    {
        // Magic numbers used by Windows for Unicode.
        internal const int UNICODE_CODE_PAGE = 1200;
        internal const int BIG_UNICODE_CODE_PAGE = 1201;

#if !ECMA_COMPAT
        // Size of characters in this encoding.
        internal const int CharSize = 2;
#endif

        // Internal state.
        private bool bigEndian;
        private bool byteOrderMark;

        #region Constructors
        public UnicodeEncoding()
            : this(false, true)
        {
            bigEndian = false;
            byteOrderMark = true;
        }

        public UnicodeEncoding(bool bigEndian, bool byteOrderMark)
            : this(bigEndian, byteOrderMark, false)
        {
        }

#if NET_2_0
	public
#endif
        UnicodeEncoding(bool bigEndian, bool byteOrderMark, bool throwOnInvalidBytes)
            : base((bigEndian ? BIG_UNICODE_CODE_PAGE : UNICODE_CODE_PAGE))
        {
#if NET_2_0
		if (throwOnInvalidBytes)
			SetFallbackInternal (null, new DecoderExceptionFallback ());
		else
			SetFallbackInternal (null, new DecoderReplacementFallback ("\uFFFD"));
#endif

            this.bigEndian = bigEndian;
            this.byteOrderMark = byteOrderMark;

            if (bigEndian)
            {
                body_name = "unicodeFFFE";
                encoding_name = "Unicode (Big-Endian)";
                header_name = "unicodeFFFE";
                is_browser_save = false;
                web_name = "unicodeFFFE";
            }
            else
            {
                body_name = "utf-16";
                encoding_name = "Unicode";
                header_name = "utf-16";
                is_browser_save = true;
                web_name = "utf-16";
            }

            // Windows reports the same code page number for
            // both the little-endian and big-endian forms.
            windows_code_page = UNICODE_CODE_PAGE;
        }
        #endregion

        #region GetByteCount
        // Get the number of bytes needed to encode a character buffer.
        public override int GetByteCount(char[] chars, int index, int count)
        {
            if (chars == null)
            {
                throw new ArgumentNullException("chars");
            }
            if (index < 0 || index > chars.Length)
            {
                throw new ArgumentOutOfRangeException("index", _("ArgRange_Array"));
            }
            if (count < 0 || count > (chars.Length - index))
            {
                throw new ArgumentOutOfRangeException("count", _("ArgRange_Array"));
            }
            return count * 2;
        }

        public override int GetByteCount(String s)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            return s.Length * 2;
        }

        #endregion

        #region GetBytes
        // Get the bytes that result from encoding a character buffer.
        public override int GetBytes(char[] chars, int charIndex, int charCount,
                                            byte[] bytes, int byteIndex)
        {
            if (chars == null)
            {
                throw new ArgumentNullException("chars");
            }
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if (charIndex < 0 || charIndex > chars.Length)
            {
                throw new ArgumentOutOfRangeException("charIndex", _("ArgRange_Array"));
            }
            if (charCount < 0 || charCount > (chars.Length - charIndex))
            {
                throw new ArgumentOutOfRangeException("charCount", _("ArgRange_Array"));
            }
            if (byteIndex < 0 || byteIndex > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("byteIndex", _("ArgRange_Array"));
            }

            if (charCount == 0)
                return 0;

            int blen = bytes.Length;
            if (blen - byteIndex <= 1)
                return 0;

            int n = 0;
            while (byteIndex + 1 < blen && charCount > 0)
            {
                char c = chars[charIndex++];
                if (bigEndian)
                {
                    bytes[byteIndex] = (byte)(c >> 8);
                    bytes[byteIndex + 1] = (byte)c;
                }
                else
                {
                    bytes[byteIndex] = (byte)c;
                    bytes[byteIndex + 1] = (byte)(c >> 8);
                }
                charCount--;
                byteIndex += 2;
                n += 2;
            }

            return n;
        }

#if !NET_2_0
        public override byte[] GetBytes(String s)
        {
            if (s == null)
                throw new ArgumentNullException("s");

            int byteCount = GetByteCount(s);
            byte[] bytes = new byte[byteCount];

            GetBytes(s, 0, s.Length, bytes, 0);

            return bytes;
        }
#endif

        public override int GetBytes(String s, int charIndex, int charCount,
            byte[] bytes, int byteIndex)
        {
            if (s == null)
            {
                throw new ArgumentNullException("s");
            }
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if (charIndex < 0 || charIndex > s.Length)
            {
                throw new ArgumentOutOfRangeException("charIndex", _("ArgRange_StringIndex"));
            }
            if (charCount < 0 || charCount > (s.Length - charIndex))
            {
                throw new ArgumentOutOfRangeException("charCount", _("ArgRange_StringRange"));
            }
            if (byteIndex < 0 || byteIndex > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("byteIndex", _("ArgRange_Array"));
            }

            // For consistency
            if (charCount == 0)
                return 0;

            int blen = bytes.Length;
            if (blen - byteIndex <= 1)
                return 0;

            int n = 0;
            while (byteIndex + 1 < blen && charCount > 0)
            {
                char c = s[charIndex++];
                if (bigEndian)
                {
                    bytes[byteIndex] = (byte)(c >> 8);
                    bytes[byteIndex + 1] = (byte)c;
                }
                else
                {
                    bytes[byteIndex] = (byte)c;
                    bytes[byteIndex + 1] = (byte)(c >> 8);
                }
                charCount--;
                byteIndex += 2;
                n += 2;
            }

            return n;
        }

        #endregion

        #region GetCharCount
        // Get the number of characters needed to decode a byte buffer.
        public override int GetCharCount(byte[] bytes, int index, int count)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if (index < 0 || index > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("index", _("ArgRange_Array"));
            }
            if (count < 0 || count > (bytes.Length - index))
            {
                throw new ArgumentOutOfRangeException("count", _("ArgRange_Array"));
            }
            return count / 2;
        }

        #endregion

        #region GetChars
        // Get the characters that result from decoding a byte buffer.
        public override int GetChars(byte[] bytes, int byteIndex, int byteCount,
                                            char[] chars, int charIndex)
        {
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if (chars == null)
            {
                throw new ArgumentNullException("chars");
            }
            if (byteIndex < 0 || byteIndex > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("byteIndex", _("ArgRange_Array"));
            }
            if (byteCount < 0 || byteCount > (bytes.Length - byteIndex))
            {
                throw new ArgumentOutOfRangeException("byteCount", _("ArgRange_Array"));
            }
            if (charIndex < 0 || charIndex > chars.Length)
            {
                throw new ArgumentOutOfRangeException("charIndex", _("ArgRange_Array"));
            }

            if (byteCount == 0)
                return 0;

            if (chars.Length == 0)
                return 0;

            return GetCharsInternal(bytes, byteIndex, byteCount, chars, charIndex, bigEndian);
        }

        internal static int GetCharsInternal(byte[] bytes, int byteIndex, int byteCount,
                                            char[] chars, int charIndex, bool bigEndian)
        {
            int clen = chars.Length;
            int blen = bytes.Length;
            int n = 0;
            while (byteCount > 1 && byteIndex + 1 < blen && charIndex < clen)
            {
                char c;
                if (bigEndian)
                {
                    c = (char)((bytes[byteIndex] << 8) | (bytes[byteIndex + 1]));
                }
                else
                {
                    c = (char)((bytes[byteIndex + 1] << 8) | (bytes[byteIndex]));
                }
                chars[charIndex++] = c;
                byteIndex += 2;
                byteCount -= 2;
                ++n;
            }

            return n;
        }

        // Decode a buffer of bytes into a string.
        [ComVisible(false)]
        public override String GetString(byte[] bytes, int index, int count)
        {
            if (bytes == null)
                throw new ArgumentNullException("bytes");
            if (index < 0 || index >= bytes.Length)
                throw new ArgumentOutOfRangeException("index", _("ArgRange_Array"));
            if (count < 0 || count > (bytes.Length - index))
                throw new ArgumentOutOfRangeException("count", _("ArgRange_Array"));

            if (count == 0)
                return "";

            string s = "";

            int len = bytes.Length;
            while (count > 0 && index + 1 < len)
            {
                char c;
                if (bigEndian)
                {
                    c = (char)((bytes[index] << 8) | (bytes[index + 1]));
                }
                else
                {
                    c = (char)((bytes[index + 1] << 8) | (bytes[index]));
                }
                s += c;
                index += 2;
                count -= 2;
            }

            return s;
        }
        #endregion

        [ComVisible(false)]
        public override Encoder GetEncoder()
        {
            return (base.GetEncoder());
        }

        // Get the maximum number of bytes needed to encode a
        // specified number of characters.
        public override int GetMaxByteCount(int charCount)
        {
            if (charCount < 0)
            {
                throw new ArgumentOutOfRangeException("charCount", _("ArgRange_NonNegative"));
            }
            return charCount * 2;
        }

        // Get the maximum number of characters needed to decode a
        // specified number of bytes.
        public override int GetMaxCharCount(int byteCount)
        {
            if (byteCount < 0)
            {
                throw new ArgumentOutOfRangeException
                    ("byteCount", _("ArgRange_NonNegative"));
            }
            return byteCount / 2;
        }

        // Get a Unicode-specific decoder that is attached to this instance.
        public override Decoder GetDecoder()
        {
            return new UnicodeDecoder(bigEndian);
        }

        // Get the Unicode preamble.
        public override byte[] GetPreamble()
        {
            if (byteOrderMark)
            {
                byte[] preamble = new byte[2];
                if (bigEndian)
                {
                    preamble[0] = (byte)0xFE;
                    preamble[1] = (byte)0xFF;
                }
                else
                {
                    preamble[0] = (byte)0xFF;
                    preamble[1] = (byte)0xFE;
                }
                return preamble;
            }
            else
            {
                return new byte[0];
            }
        }

        // Determine if this object is equal to another.
        public override bool Equals(Object value)
        {
            UnicodeEncoding enc = (value as UnicodeEncoding);
            if (enc != null)
            {
                return (codePage == enc.codePage &&
                        bigEndian == enc.bigEndian &&
                        byteOrderMark == enc.byteOrderMark);
            }
            else
            {
                return false;
            }
        }

        // Get the hash code for this object.
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        // Unicode decoder implementation.
        private sealed class UnicodeDecoder : Decoder
        {
            private bool bigEndian;
            private int leftOverByte;

            // Constructor.
            public UnicodeDecoder(bool bigEndian)
            {
                this.bigEndian = bigEndian;
                leftOverByte = -1;
            }

            // Override inherited methods.
            public override int GetCharCount(byte[] bytes, int index, int count)
            {
                if (bytes == null)
                {
                    throw new ArgumentNullException("bytes");
                }
                if (index < 0 || index > bytes.Length)
                {
                    throw new ArgumentOutOfRangeException("index", _("ArgRange_Array"));
                }
                if (count < 0 || count > (bytes.Length - index))
                {
                    throw new ArgumentOutOfRangeException("count", _("ArgRange_Array"));
                }
                if (leftOverByte != -1)
                {
                    return (count + 1) / 2;
                }
                else
                {
                    return count / 2;
                }
            }

            public override int GetChars(byte[] bytes, int byteIndex,
                                                int byteCount, char[] chars,
                                                int charIndex)
            {
                if (bytes == null)
                {
                    throw new ArgumentNullException("bytes");
                }
                if (chars == null)
                {
                    throw new ArgumentNullException("chars");
                }
                if (byteIndex < 0 || byteIndex > bytes.Length)
                {
                    throw new ArgumentOutOfRangeException("byteIndex", _("ArgRange_Array"));
                }
                if (byteCount < 0 || byteCount > (bytes.Length - byteIndex))
                {
                    throw new ArgumentOutOfRangeException("byteCount", _("ArgRange_Array"));
                }
                if (charIndex < 0 || charIndex > chars.Length)
                {
                    throw new ArgumentOutOfRangeException("charIndex", _("ArgRange_Array"));
                }

                if (byteCount == 0)
                    return 0;

                int leftOver = leftOverByte;
                int count;

                if (leftOver != -1)
                    count = (byteCount + 1) / 2;
                else
                    count = byteCount / 2;

                if (chars.Length - charIndex < count)
                    throw new ArgumentException(_("Arg_InsufficientSpace"));

                if (leftOver != -1)
                {
                    if (bigEndian)
                        chars[charIndex] = unchecked((char)((leftOver << 8) | (int)bytes[byteIndex]));
                    else
                        chars[charIndex] = unchecked((char)(((int)bytes[byteIndex] << 8) | leftOver));
                    charIndex++;
                    byteIndex++;
                    byteCount--;
                }

                if ((byteCount & unchecked((int)0xFFFFFFFE)) != 0)
                {
                    GetCharsInternal(bytes, byteIndex, byteCount, chars, charIndex, bigEndian);
                }

                if ((byteCount & 1) == 0)
                    leftOverByte = -1;
                else
                    leftOverByte = bytes[byteCount + byteIndex - 1];

                return count;
            }

        } // class UnicodeDecoder
    }
}
