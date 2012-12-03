//CHANGED
/*
 * UTF8Encoding.cs - Implementation of the "System.Text.UTF8Encoding" class.
 *
 * Copyright (c) 2001, 2002  Southern Storm Software, Pty Ltd
 * Copyright (C) 2004 Novell, Inc (http://www.novell.com)
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

using System;
using System.Runtime.InteropServices;

namespace System.Text
{
    [Serializable]
    [MonoTODO("Serialization format not compatible with .NET")]
#if NET_2_0
[MonoTODO ("EncoderFallback is not handled")]
[ComVisible (true)]
#endif
    public class UTF8Encoding : Encoding
    {
        // Magic number used by Windows for UTF-8.
        internal const int UTF8_CODE_PAGE = 65001;

        // Internal state.
        private bool emitIdentifier;
//#if !NET_2_0
        private bool throwOnInvalid;
//#endif

        // Constructors.
        public UTF8Encoding() : this(false, false) { }
        public UTF8Encoding(bool encoderShouldEmitUTF8Identifier)
            : this(encoderShouldEmitUTF8Identifier, false) { }

        public UTF8Encoding(bool encoderShouldEmitUTF8Identifier, bool throwOnInvalidBytes)
            : base(UTF8_CODE_PAGE)
        {
            emitIdentifier = encoderShouldEmitUTF8Identifier;
#if NET_2_0
		if (throwOnInvalidBytes)
			SetFallbackInternal (null, new DecoderExceptionFallback ());
		else
			SetFallbackInternal (null, new DecoderReplacementFallback ("\uFFFD"));
#else
            throwOnInvalid = throwOnInvalidBytes;
#endif

            web_name = body_name = header_name = "utf-8";
            encoding_name = "Unicode (UTF-8)";
            is_browser_save = true;
            is_browser_display = true;
            is_mail_news_display = true;
            is_mail_news_save = true;
            windows_code_page = UnicodeEncoding.UNICODE_CODE_PAGE;
        }

        #region GetByteCount()

        // Internal version of "GetByteCount" which can handle a rolling
        // state between multiple calls to this method.
        private static int InternalGetByteCount(char[] chars, int index, int count, ref char leftOver, bool flush)
        {
            // Validate the parameters.
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

            if (index == chars.Length)
            {
                if (flush && leftOver != '\0')
                {
                    // Flush the left-over surrogate pair start.
                    leftOver = '\0';
                    return 3;
                }
                return 0;
            }

            // Determine the lengths of all characters.
            char ch;
            int length = 0;
            char pair = leftOver;
            while (count > 0)
            {
                ch = chars[index];
                if (pair == 0)
                {
                    if (ch < '\u0080')
                    {
                        // fast path optimization
                        int end = index + count;
                        for (; index < end; index++, count--)
                        {
                            if (chars[index] < '\x80')
                                ++length;
                            else
                                break;
                        }
                        continue;
                        //length++;
                    }
                    else if (ch < '\u0800')
                    {
                        length += 2;
                    }
                    else if (ch >= '\uD800' && ch <= '\uDBFF')
                    {
                        // This is the start of a surrogate pair.
                        pair = ch;
                    }
                    else
                    {
                        length += 3;
                    }
                }
                else if (ch >= '\uDC00' && ch <= '\uDFFF')
                {
                    if (pair != 0)
                    {
                        // We have a surrogate pair.
                        length += 4;
                        pair = '\0';
                    }
                    else
                    {
                        // We have a surrogate tail without 
                        // leading surrogate. In NET_2_0 it
                        // uses fallback. In NET_1_1 we output
                        // wrong surrogate.
                        length += 3;
                        pair = '\0';
                    }
                }
                else
                {
                    // We have a surrogate start followed by a
                    // regular character.  Technically, this is
                    // invalid, but we have to do something.
                    // We write out the surrogate start and then
                    // re-visit the current character again.
                    length += 3;
                    pair = '\0';
                    continue;
                }
                ++index;
                --count;
            }
            if (flush)
            {
                if (pair != '\0')
                    // Flush the left-over surrogate pair start.
                    length += 3;
                leftOver = '\0';
            }
            else
                leftOver = pair;

            // Return the final length to the caller.
            return length;
        }

        // Get the number of bytes needed to encode a character buffer.
        public override int GetByteCount(char[] chars, int index, int count)
        {
            char dummy = '\0';
            return InternalGetByteCount(chars, index, count, ref dummy, true);
        }

#if !NET_2_0
        // Convenience wrappers for "GetByteCount".
        public override int GetByteCount(String s)
        {
            // Validate the parameters.
            if (s == null)
                throw new ArgumentNullException("s");

            if (s.Length == 0)
                return 0;

            char dummy = '\0';
            char[] chars = s.ToCharArray();
            return InternalGetByteCount(chars, 0, s.Length, ref dummy, true);
        }
#endif

        #endregion

        #region GetBytes()

        // Internal version of "GetBytes" which can handle a rolling
        // state between multiple calls to this method.
        private static int InternalGetBytes(char[] chars, int charIndex,
                             int charCount, byte[] bytes,
                             int byteIndex, ref char leftOver,
                             bool flush)
        {
            // Validate the parameters.
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

            if (charIndex == chars.Length)
            {
                if (flush && leftOver != '\0')
                {
#if NET_2_0
    // FIXME: use EncoderFallback.
    //
    // By default it is empty, so I do nothing for now.
				leftOver = '\0';
#else
                    // Flush the left-over surrogate pair start.
                    if (byteIndex >= bytes.Length - 3)
                        throw new ArgumentException(_("Arg_InsufficientSpace"), "bytes");
                    bytes[byteIndex++] = 0xEF;
                    bytes[byteIndex++] = 0xBB;
                    bytes[byteIndex++] = 0xBF;
                    leftOver = '\0';
                    return 3;
#endif
                }
                return 0;
            }

            if (bytes.Length == byteIndex)
                return _InternalGetBytes(
                    chars, charIndex, charCount,
                    null, 0, 0, ref leftOver, flush);

            return _InternalGetBytes(
                chars, charIndex, charCount,
                bytes, byteIndex, bytes.Length - byteIndex,
                ref leftOver, flush);
        }

        private static int _InternalGetBytes(char[] chars, int charIndex, int charCount,
                             byte[] bytes, int byteIndex, int byteCount,
                             ref char leftOver, bool flush)
        {
            // Convert the characters into bytes.
            // Convert the characters into bytes.
            char ch;
            int length = byteCount;
            char pair = leftOver;
            int posn = byteIndex;
            int code = 0;

            while (charCount > 0)
            {
                // Fetch the next UTF-16 character pair value.
                ch = chars[charIndex];
                if (pair == '\0')
                {
                    if (ch < '\uD800' || ch >= '\uE000')
                    {
                        if (ch < '\x80')
                        { // fast path optimization
                            int end = charIndex + charCount;
                            for (; charIndex < end; posn++, charIndex++, charCount--)
                            {
                                if (chars[charIndex] < '\x80')
                                    bytes[posn] = (byte)chars[charIndex];
                                else
                                    break;
                            }
                            continue;
                        }
                        code = ch;
                    }
                    else if (ch < '\uDC00')
                    {
                        // surrogate start
                        pair = ch;
                        ++charIndex;
                        --charCount;
                        continue;
                    }
                    else
                    { // ch <= '\uDFFF'
                        // We have a surrogate tail without leading 
                        // surrogate. In NET_2_0 it uses fallback.
                        // In NET_1_1 we output wrong surrogate.
                        if (posn > length - 3)
                        {
                            throw new ArgumentException(_("Arg_InsufficientSpace"), "bytes");
                        }
                        bytes[posn++] = (byte)(0xE0 | (ch >> 12));
                        bytes[posn++] = (byte)(0x80 | ((ch >> 6) & 0x3F));
                        bytes[posn++] = (byte)(0x80 | (ch & 0x3F));
                        ++charIndex;
                        --charCount;
                        continue;
                    }
                }
                else
                {
                    if ('\uDC00' <= ch && ch <= '\uDFFF')
                        code = 0x10000 + (int)ch - 0xDC00 +
                            (((int)pair - 0xD800) << 10);
                    else
                    {
                        // We have a surrogate start followed by a
                        // regular character.  Technically, this is
                        // invalid, but we have to do something.
                        // We write out the surrogate start and then
                        // re-visit the current character again.
                        if (posn > length - 3)
                        {
                            throw new ArgumentException(_("Arg_InsufficientSpace"), "bytes");
                        }
                        bytes[posn++] = (byte)(0xE0 | (pair >> 12));
                        bytes[posn++] = (byte)(0x80 | ((pair >> 6) & 0x3F));
                        bytes[posn++] = (byte)(0x80 | (pair & 0x3F));
                        pair = '\0';
                        continue;
                    }
                    pair = '\0';
                }
                ++charIndex;
                --charCount;

                // Encode the character pair value.
                if (code < 0x0080)
                {
                    if (posn >= length)
                        throw new ArgumentException(_("Arg_InsufficientSpace"), "bytes");
                    bytes[posn++] = (byte)code;
                }
                else if (code < 0x0800)
                {
                    if ((posn + 2) > length)
                        throw new ArgumentException(_("Arg_InsufficientSpace"), "bytes");
                    bytes[posn++] = (byte)(0xC0 | (code >> 6));
                    bytes[posn++] = (byte)(0x80 | (code & 0x3F));
                }
                else if (code < 0x10000)
                {
                    if (posn > length - 3)
                        throw new ArgumentException(_("Arg_InsufficientSpace"), "bytes");
                    bytes[posn++] = (byte)(0xE0 | (code >> 12));
                    bytes[posn++] = (byte)(0x80 | ((code >> 6) & 0x3F));
                    bytes[posn++] = (byte)(0x80 | (code & 0x3F));
                }
                else
                {
                    if (posn > length - 4)
                        throw new ArgumentException(_("Arg_InsufficientSpace"), "bytes");
                    bytes[posn++] = (byte)(0xF0 | (code >> 18));
                    bytes[posn++] = (byte)(0x80 | ((code >> 12) & 0x3F));
                    bytes[posn++] = (byte)(0x80 | ((code >> 6) & 0x3F));
                    bytes[posn++] = (byte)(0x80 | (code & 0x3F));
                }
            }

            if (flush)
            {
                if (pair != '\0')
                {
                    // Flush the left-over incomplete surrogate.
                    if (posn > length - 3)
                    {
                        throw new ArgumentException(_("Arg_InsufficientSpace"), "bytes");
                    }
                    bytes[posn++] = (byte)(0xE0 | (pair >> 12));
                    bytes[posn++] = (byte)(0x80 | ((pair >> 6) & 0x3F));
                    bytes[posn++] = (byte)(0x80 | (pair & 0x3F));
                }
                leftOver = '\0';
            }
            else
                leftOver = pair;
            Char.IsLetterOrDigit(pair);

            // Return the final count to the caller.
            return posn - byteIndex;
        }

        //private unsafe int Fallback(byte* bytes, int byteCount, char lead, char tail)
        //{
        //    throw new NotImplementedException();
        //}

        // Get the bytes that result from encoding a character buffer.
        public override int GetBytes(char[] chars, int charIndex, int charCount,
                                     byte[] bytes, int byteIndex)
        {
            char leftOver = '\0';
            return InternalGetBytes(chars, charIndex, charCount, bytes, byteIndex, ref leftOver, true);
        }

        // Convenience wrappers for "GetBytes".
        public override int GetBytes(String s, int charIndex, int charCount,
                                     byte[] bytes, int byteIndex)
        {
            // Validate the parameters.
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

            if (charIndex == s.Length)
                return 0;

            char[] chars = s.ToCharArray();
            char dummy = '\0';
            return InternalGetBytes(chars, charIndex, charCount, bytes, byteIndex, ref dummy, true);

            //unsafe
            //{
            //    fixed (char* cptr = s)
            //    {
            //        char dummy = '\0';
            //        if (bytes.Length == byteIndex)
            //            return InternalGetBytes(
            //                cptr + charIndex, charCount,
            //                null, 0, ref dummy, true);

            //        fixed (byte* bptr = bytes)
            //        {
            //            return InternalGetBytes(
            //                cptr + charIndex, charCount,
            //                bptr + byteIndex, bytes.Length - byteIndex,
            //                ref dummy, true);
            //        }
            //    }
            //}
        }

        #endregion

        #region Internal version of "GetCharCount"
        // Internal version of "GetCharCount" which can handle a rolling
        // state between multiple calls to this method.
//#if NET_2_0
//    private static int InternalGetCharCount (
//        byte[] bytes, int index, int count, uint leftOverBits,
//        uint leftOverCount, object provider,
//        ref DecoderFallbackBuffer fallbackBuffer, ref byte [] bufferArg, bool flush)
//#else
        private static int InternalGetCharCount(
            byte[] bytes, int index, int count, uint leftOverBits,
            uint leftOverCount, bool throwOnInvalid, bool flush)
//#endif
        {
            // Validate the parameters.
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

            if (count == 0)
                return 0;
//#if NET_2_0
//            return _InternalGetCharCount (bytes, index, count,
//                leftOverBits, leftOverCount, provider, ref fallbackBuffer, ref bufferArg, flush);
//#else
            return _InternalGetCharCount(bytes, index, count,
                    leftOverBits, leftOverCount, throwOnInvalid, flush);
//#endif
        }

//#if NET_2_0
//    private static int _InternalGetCharCount (
//        byte[] bytes, int index, int count, uint leftOverBits,
//        uint leftOverCount, object provider,
//        ref DecoderFallbackBuffer fallbackBuffer, ref byte [] bufferArg, bool flush)
//#else
        private static int _InternalGetCharCount(
            byte[] bytes, int index, int count, uint leftOverBits,
            uint leftOverCount, bool throwOnInvalid, bool flush)
//#endif
        {
            int length = 0;

            if (leftOverCount == 0)
            {
                int end = index + count;
                for (; index < end; index++, count--)
                {
                    if (bytes[index] < 0x80)
                        length++;
                    else
                        break;
                }
            }

            // Determine the number of characters that we have.
            uint ch;
            uint leftBits = leftOverBits;
            uint leftSoFar = (leftOverCount & (uint)0x0F);
            uint leftSize = ((leftOverCount >> 4) & (uint)0x0F);
            while (count > 0)
            {
                ch = (uint)(bytes[index++]);
                --count;
                if (leftSize == 0)
                {
                    // Process a UTF-8 start character.
                    if (ch < (uint)0x0080)
                    {
                        // Single-byte UTF-8 character.
                        ++length;
                    }
                    else if ((ch & (uint)0xE0) == (uint)0xC0)
                    {
                        // Double-byte UTF-8 character.
                        leftBits = (ch & (uint)0x1F);
                        leftSoFar = 1;
                        leftSize = 2;
                    }
                    else if ((ch & (uint)0xF0) == (uint)0xE0)
                    {
                        // Three-byte UTF-8 character.
                        leftBits = (ch & (uint)0x0F);
                        leftSoFar = 1;
                        leftSize = 3;
                    }
                    else if ((ch & (uint)0xF8) == (uint)0xF0)
                    {
                        // Four-byte UTF-8 character.
                        leftBits = (ch & (uint)0x07);
                        leftSoFar = 1;
                        leftSize = 4;
                    }
                    else if ((ch & (uint)0xFC) == (uint)0xF8)
                    {
                        // Five-byte UTF-8 character.
                        leftBits = (ch & (uint)0x03);
                        leftSoFar = 1;
                        leftSize = 5;
                    }
                    else if ((ch & (uint)0xFE) == (uint)0xFC)
                    {
                        // Six-byte UTF-8 character.
                        leftBits = (ch & (uint)0x03);
                        leftSoFar = 1;
                        leftSize = 6;
                    }
                    else
                    {
                        // Invalid UTF-8 start character.
//#if NET_2_0
//                    length += Fallback (provider, ref fallbackBuffer, ref bufferArg, bytes, index - 1, 1);
//#else
                        if (throwOnInvalid)
                            throw new ArgumentException(_("Arg_InvalidUTF8"), "bytes");
//#endif
                    }
                }
                else
                {
                    // Process an extra byte in a multi-byte sequence.
                    if ((ch & (uint)0xC0) == (uint)0x80)
                    {
                        leftBits = ((leftBits << 6) | (ch & (uint)0x3F));
                        if (++leftSoFar >= leftSize)
                        {
                            // We have a complete character now.
                            if (leftBits < (uint)0x10000)
                            {
                                // is it an overlong ?
                                bool overlong = false;
                                switch (leftSize)
                                {
                                    case 2:
                                        overlong = (leftBits <= 0x7F);
                                        break;
                                    case 3:
                                        overlong = (leftBits <= 0x07FF);
                                        break;
                                    case 4:
                                        overlong = (leftBits <= 0xFFFF);
                                        break;
                                    case 5:
                                        overlong = (leftBits <= 0x1FFFFF);
                                        break;
                                    case 6:
                                        overlong = (leftBits <= 0x03FFFFFF);
                                        break;
                                }
                                if (overlong)
                                {
                                    if (throwOnInvalid)
                                        throw new ArgumentException(_("Overlong"), leftBits.ToString());
                                }
                                else
                                    ++length;
                            }
                            else if (leftBits < (uint)0x110000)
                            {
                                length += 2;
                            }
                            else
                            {
                                if (throwOnInvalid)
                                    throw new ArgumentException(_("Arg_InvalidUTF8"), "bytes");
                            }
                            leftSize = 0;
                        }
                    }
                    else
                    {
                        // Invalid UTF-8 sequence: clear and restart.
                        if (throwOnInvalid)
                            throw new ArgumentException(_("Arg_InvalidUTF8"), "bytes");
                        leftSize = 0;
                        --index;
                        ++count;
                    }
                }
            }
            if (flush && leftSize != 0)
            {
                // We had left-over bytes that didn't make up
                // a complete UTF-8 character sequence.
                if (throwOnInvalid)
                    throw new ArgumentException(_("Arg_InvalidUTF8"), "bytes");
            }

            // Return the final length to the caller.
            return length;
        }
        #endregion

        // Get the number of characters needed to decode a byte buffer.
        public override int GetCharCount(byte[] bytes, int index, int count)
        {
//#if NET_2_0
//        DecoderFallbackBuffer buf = null;
//        byte [] bufferArg = null;
//        return InternalGetCharCount (bytes, index, count, 0, 0, DecoderFallback, ref buf, ref bufferArg, true);
//#else
            return InternalGetCharCount(bytes, index, count, 0, 0, throwOnInvalid, true);
//#endif
        }

        #region InternalGetChars
        // Get the characters that result from decoding a byte buffer.

        private static int InternalGetChars(
            byte[] bytes, int byteIndex, int byteCount, char[] chars,
            int charIndex, ref uint leftOverBits, ref uint leftOverCount,
            bool throwOnInvalid, bool flush)
        {
            // Validate the parameters.
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

            if (charIndex == chars.Length)
                return 0;

            if (byteCount == 0 || byteIndex == bytes.Length)
                return InternalGetChars(null, 0, 0, chars, charIndex, chars.Length - charIndex, ref leftOverBits,
                                        ref leftOverCount, throwOnInvalid, flush);

            return InternalGetChars(bytes, byteIndex, byteCount, chars, charIndex, chars.Length - charIndex,
                                    ref leftOverBits, ref leftOverCount, throwOnInvalid, flush);

        }


        private static int InternalGetChars(
            byte[] bytes, int byteIndex, int byteCount, char[] chars, int charIndex, int charCount,
            ref uint leftOverBits, ref uint leftOverCount,
            bool throwOnInvalid, bool flush)
        {
            int length = charCount;
            int posn = charIndex;

            if (leftOverCount == 0)
            {
                int end = byteIndex + byteCount;
                for (; byteIndex < end; posn++, byteIndex++, byteCount--)
                {
                    if (bytes[byteIndex] < 0x80)
                        chars[posn] = (char)bytes[byteIndex];
                    else
                        break;
                }
            }

            // Convert the bytes into the output buffer.
            uint ch;
            uint leftBits = leftOverBits;
            uint leftSoFar = (leftOverCount & (uint)0x0F);
            uint leftSize = ((leftOverCount >> 4) & (uint)0x0F);

            int byteEnd = byteIndex + byteCount;
            for (; byteIndex < byteEnd; byteIndex++)
            {
                // Fetch the next character from the byte buffer.
                ch = (uint)(bytes[byteIndex]);
                if (leftSize == 0)
                {
                    // Process a UTF-8 start character.
                    if (ch < (uint)0x0080)
                    {
                        // Single-byte UTF-8 character.
                        if (posn >= length)
                        {
                            throw new ArgumentException(_("Arg_InsufficientSpace"), "chars");
                        }
                        chars[posn++] = (char)ch;
                    }
                    else if ((ch & (uint)0xE0) == (uint)0xC0)
                    {
                        // Double-byte UTF-8 character.
                        leftBits = (ch & (uint)0x1F);
                        leftSoFar = 1;
                        leftSize = 2;
                    }
                    else if ((ch & (uint)0xF0) == (uint)0xE0)
                    {
                        // Three-byte UTF-8 character.
                        leftBits = (ch & (uint)0x0F);
                        leftSoFar = 1;
                        leftSize = 3;
                    }
                    else if ((ch & (uint)0xF8) == (uint)0xF0)
                    {
                        // Four-byte UTF-8 character.
                        leftBits = (ch & (uint)0x07);
                        leftSoFar = 1;
                        leftSize = 4;
                    }
                    else if ((ch & (uint)0xFC) == (uint)0xF8)
                    {
                        // Five-byte UTF-8 character.
                        leftBits = (ch & (uint)0x03);
                        leftSoFar = 1;
                        leftSize = 5;
                    }
                    else if ((ch & (uint)0xFE) == (uint)0xFC)
                    {
                        // Six-byte UTF-8 character.
                        leftBits = (ch & (uint)0x03);
                        leftSoFar = 1;
                        leftSize = 6;
                    }
                    else
                    {
                        // Invalid UTF-8 start character.
                        if (throwOnInvalid)
                            throw new ArgumentException(_("Arg_InvalidUTF8"), "bytes");
                    }
                }
                else
                {
                    // Process an extra byte in a multi-byte sequence.
                    if ((ch & (uint)0xC0) == (uint)0x80)
                    {
                        leftBits = ((leftBits << 6) | (ch & (uint)0x3F));
                        if (++leftSoFar >= leftSize)
                        {
                            // We have a complete character now.
                            if (leftBits < (uint)0x10000)
                            {
                                // is it an overlong ?
                                bool overlong = false;
                                switch (leftSize)
                                {
                                    case 2:
                                        overlong = (leftBits <= 0x7F);
                                        break;
                                    case 3:
                                        overlong = (leftBits <= 0x07FF);
                                        break;
                                    case 4:
                                        overlong = (leftBits <= 0xFFFF);
                                        break;
                                    case 5:
                                        overlong = (leftBits <= 0x1FFFFF);
                                        break;
                                    case 6:
                                        overlong = (leftBits <= 0x03FFFFFF);
                                        break;
                                }
                                if (overlong)
                                {
                                    if (throwOnInvalid)
                                        throw new ArgumentException(_("Overlong"), leftBits.ToString());
                                }
                                else if ((leftBits & 0xF800) == 0xD800)
                                {
                                    // UTF-8 doesn't use surrogate characters
                                    if (throwOnInvalid)
                                        throw new ArgumentException(_("Arg_InvalidUTF8"), "bytes");
                                }
                                else
                                {
                                    if (posn >= length)
                                    {
                                        throw new ArgumentException
                                            (_("Arg_InsufficientSpace"), "chars");
                                    }
                                    chars[posn++] = (char)leftBits;
                                }
                            }
                            else if (leftBits < (uint)0x110000)
                            {
                                if ((posn + 2) > length)
                                {
                                    throw new ArgumentException
                                        (_("Arg_InsufficientSpace"), "chars");
                                }
                                leftBits -= (uint)0x10000;
                                chars[posn++] = (char)((leftBits >> 10) +
                                                       (uint)0xD800);
                                chars[posn++] =
                                    (char)((leftBits & (uint)0x3FF) + (uint)0xDC00);
                            }
                            else
                            {
                                if (throwOnInvalid)
                                    throw new ArgumentException(_("Arg_InvalidUTF8"), "bytes");
                            }
                            leftSize = 0;
                        }
                    }
                    else
                    {
                        // Invalid UTF-8 sequence: clear and restart.
                        if (throwOnInvalid)
                            throw new ArgumentException(_("Arg_InvalidUTF8"), "bytes");
                        leftSize = 0;
                        --byteIndex;
                    }
                }
            }
            if (flush && leftSize != 0)
            {
                // We had left-over bytes that didn't make up
                // a complete UTF-8 character sequence.
                if (throwOnInvalid)
                    throw new ArgumentException(_("Arg_InvalidUTF8"), "bytes");
            }
            leftOverBits = leftBits;
            leftOverCount = (leftSoFar | (leftSize << 4));

            // Return the final length to the caller.
            return posn - charIndex;
        }
        #endregion


        // Get the characters that result from decoding a byte buffer.
        public override int GetChars(byte[] bytes, int byteIndex, int byteCount,
                                     char[] chars, int charIndex)
        {
            uint leftOverBits = 0;
            uint leftOverCount = 0;
//#if NET_2_0
//        DecoderFallbackBuffer buf = null;
//        byte [] bufferArg = null;
//        return InternalGetChars (bytes, byteIndex, byteCount, chars, 
//                charIndex, ref leftOverBits, ref leftOverCount, DecoderFallback, ref buf, ref bufferArg, true);
//#else
            return InternalGetChars(bytes, byteIndex, byteCount, chars,
                    charIndex, ref leftOverBits, ref leftOverCount, throwOnInvalid, true);
        }

        // Get the maximum number of bytes needed to encode a
        // specified number of characters.
        public override int GetMaxByteCount(int charCount)
        {
            if (charCount < 0)
            {
                throw new ArgumentOutOfRangeException("charCount", _("ArgRange_NonNegative"));
            }
            return charCount * 4;
        }

        // Get the maximum number of characters needed to decode a
        // specified number of bytes.
        public override int GetMaxCharCount(int byteCount)
        {
            if (byteCount < 0)
            {
                throw new ArgumentOutOfRangeException("byteCount", _("ArgRange_NonNegative"));
            }
            return byteCount;
        }

        // Get a UTF8-specific decoder that is attached to this instance.
        public override Decoder GetDecoder()
        {
#if NET_2_0
		return new UTF8Decoder (DecoderFallback);
#else
            return new UTF8Decoder(throwOnInvalid);
#endif
        }

        // Get a UTF8-specific encoder that is attached to this instance.
        public override Encoder GetEncoder()
        {
            return new UTF8Encoder(emitIdentifier);
        }

        // Get the UTF8 preamble.
        public override byte[] GetPreamble()
        {
            if (emitIdentifier)
            {
                byte[] pre = new byte[3];
                pre[0] = (byte)0xEF;
                pre[1] = (byte)0xBB;
                pre[2] = (byte)0xBF;
                return pre;
            }
            else
            {
                return new byte[0];
            }
        }

        // Determine if this object is equal to another.
        public override bool Equals(Object value)
        {
            UTF8Encoding enc = (value as UTF8Encoding);
            if (enc != null)
            {
#if NET_2_0
			return (codePage == enc.codePage &&
					emitIdentifier == enc.emitIdentifier &&
					DecoderFallback == enc.DecoderFallback &&
					EncoderFallback == enc.EncoderFallback);
#else
                return (codePage == enc.codePage &&
                        emitIdentifier == enc.emitIdentifier &&
                        throwOnInvalid == enc.throwOnInvalid);
#endif
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

#if NET_2_0
	[MonoTODO]
	public override int GetByteCount (string s)
	{
		// hmm, does this override make any sense?
		return base.GetByteCount (s);
	}

	[MonoTODO]
	[ComVisible (false)]
	public override string GetString (byte [] bytes, int index, int count)
	{
		// hmm, does this override make any sense?
		return base.GetString (bytes, index, count);
	}
#endif

#if !NET_2_0
        public override byte[] GetBytes(String s)
        {
            if (s == null)
                throw new ArgumentNullException("s");

            int length = GetByteCount(s);
            byte[] bytes = new byte[length];
            GetBytes(s, 0, s.Length, bytes, 0);
            return bytes;
        }
#endif

        // UTF-8 decoder implementation.
        [Serializable]
        private class UTF8Decoder : Decoder
        {
//#if !NET_2_0
            private bool throwOnInvalid;
//#endif
            private uint leftOverBits;
            private uint leftOverCount;

            // Constructor.
#if NET_2_0
		public UTF8Decoder (DecoderFallback fallback)
#else
            public UTF8Decoder(bool throwOnInvalid)
#endif
            {
#if NET_2_0
			Fallback = fallback;
#else
                this.throwOnInvalid = throwOnInvalid;
#endif
                leftOverBits = 0;
                leftOverCount = 0;
            }

            // Override inherited methods.
            public override int GetCharCount(byte[] bytes, int index, int count)
            {

                return InternalGetCharCount(bytes, index, count,
                        leftOverBits, leftOverCount, throwOnInvalid, false);
            }
            public override int GetChars(byte[] bytes, int byteIndex,
                             int byteCount, char[] chars, int charIndex)
            {
                return InternalGetChars(bytes, byteIndex, byteCount,
                    chars, charIndex, ref leftOverBits, ref leftOverCount, throwOnInvalid, false);
            }

        } // class UTF8Decoder

        // UTF-8 encoder implementation.
        [Serializable]
        private class UTF8Encoder : Encoder
        {
            private bool emitIdentifier;
            private char leftOverForCount;
            private char leftOverForConv;

            // Constructor.
            public UTF8Encoder(bool emitIdentifier)
            {
                this.emitIdentifier = emitIdentifier;
                leftOverForCount = '\0';
                leftOverForConv = '\0';
            }

            // Override inherited methods.
            public override int GetByteCount(char[] chars, int index,
                         int count, bool flush)
            {
                return InternalGetByteCount(chars, index, count, ref leftOverForCount, flush);
            }

            public override int GetBytes(char[] chars, int charIndex,
                         int charCount, byte[] bytes, int byteIndex, bool flush)
            {
                int result;
                result = InternalGetBytes(chars, charIndex, charCount, bytes, byteIndex, ref leftOverForConv, flush);
                emitIdentifier = false;
                return result;
            }

        } // class UTF8Encoder

    }; // class UTF8Encoding

}
