/*
 * Decoder.cs - Implementation of the "System.Text.Decoder" class.
 *
 * Copyright (c) 2001  Southern Storm Software, Pty Ltd
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
public abstract class Decoder
{

	// Constructor.
	protected Decoder () {}

#if NET_2_0
	DecoderFallback fallback = new DecoderReplacementFallback ();
	DecoderFallbackBuffer fallback_buffer;

	[ComVisible (false)]
	internal DecoderFallback Fallback {
		get { return fallback; }
		set {
			if (value == null)
				throw new ArgumentNullException ();
			fallback = value;
			fallback_buffer = null;
		}
	}

	[ComVisible (false)]
	internal DecoderFallbackBuffer FallbackBuffer {
		get {
			if (fallback_buffer == null)
				fallback_buffer = fallback.CreateFallbackBuffer ();
			return fallback_buffer;
		}
	}
#endif

	// Get the number of characters needed to decode a buffer.
	public abstract int GetCharCount (byte[] bytes, int index, int count);

	// Get the characters that result from decoding a buffer.
	public abstract int GetChars (byte[] bytes, int byteIndex, int byteCount,
								 char[] chars, int charIndex);

#if NET_2_0
	[ComVisible (false)]
	public virtual int GetCharCount (byte [] bytes, int index, int count, bool flush)
	{
		if (flush)
			Reset ();
		return GetCharCount (bytes, index, count);
	}


	[CLSCompliant (false)]
	[ComVisible (false)]
	public virtual int GetCharCount (ref byte[] bytes, int count, bool flush)
	{
		if (bytes == null)
			throw new ArgumentNullException ("bytes");
		if (count < 0)
			throw new ArgumentOutOfRangeException ("count");

		byte [] barr = new byte [count];
        //Marshal.Copy ((IntPtr) bytes, barr, 0, count);
        for (int i = 0; i < count; i++)
        {
            barr[i] = bytes[i];
        }
        
        return GetCharCount(barr, 0, count, flush);
	}


	public virtual int GetChars (
		byte[] bytes, int byteIndex, int byteCount,
		char[] chars, int charIndex, bool flush)
	{
		CheckArguments (bytes, byteIndex, byteCount);
		CheckArguments (chars, charIndex);

		if (flush)
			Reset ();
		return GetChars (bytes, byteIndex, byteCount, chars, charIndex);
	}

	[CLSCompliant (false)]
	[ComVisible (false)]
	public virtual int GetChars (ref byte[] bytes, int byteCount,
		ref char[] chars, int charCount, bool flush)
	{
		CheckArguments (ref chars, charCount, ref bytes, byteCount);

		char [] carr = new char [charCount];
		//Marshal.Copy ((IntPtr) chars, carr, 0, charCount);
        for (int i = 0; i < charCount; i++)
        {
            carr[i] = chars[i];
        }

		byte [] barr = new byte [byteCount];
		//sMarshal.Copy ((IntPtr) bytes, barr, 0, byteCount);
        for (int i = 0; i < byteCount; i++)
        {
            barr[i] = bytes[i];
        }
        return GetChars (barr, 0, byteCount, carr, 0, flush);
	}


	[ComVisible (false)]
	public virtual void Reset ()
	{
		if (fallback_buffer != null)
			fallback_buffer.Reset ();
	}


    [CLSCompliant (false)]
	[ComVisible (false)]
	public virtual void Convert (
		ref byte[] bytes, int byteCount,
		ref char[] chars, int charCount, bool flush,
		out int bytesUsed, out int charsUsed, out bool completed)
	{
		CheckArguments (ref chars, charCount, ref bytes, byteCount);

		bytesUsed = byteCount;
		while (true) {
			charsUsed = GetCharCount (ref bytes, bytesUsed, flush);
			if (charsUsed <= charCount)
				break;
			flush = false;
			bytesUsed >>= 1;
		}
		completed = bytesUsed == byteCount;
		charsUsed = GetChars (ref bytes, bytesUsed, ref chars, charCount, flush);
	}


	[ComVisible (false)]
	public virtual void Convert (
		byte [] bytes, int byteIndex, int byteCount,
		char [] chars, int charIndex, int charCount, bool flush,
		out int bytesUsed, out int charsUsed, out bool completed)
	{
		CheckArguments (bytes, byteIndex, byteCount);
		CheckArguments (chars, charIndex);
		if (charCount < 0 || chars.Length < charIndex + charCount)
			throw new ArgumentOutOfRangeException ("charCount");

		bytesUsed = byteCount;
		while (true) {
			charsUsed = GetCharCount (bytes, byteIndex, bytesUsed, flush);
			if (charsUsed <= charCount)
				break;
			flush = false;
			bytesUsed >>= 1;
		}
		completed = bytesUsed == byteCount;
		charsUsed = GetChars (bytes, byteIndex, bytesUsed, chars, charIndex, flush);
	}

	void CheckArguments (char [] chars, int charIndex)
	{
		if (chars == null)
			throw new ArgumentNullException ("chars");
		if (charIndex < 0 || chars.Length <= charIndex)
			throw new ArgumentOutOfRangeException ("charIndex");
	}

	void CheckArguments (byte [] bytes, int byteIndex, int byteCount)
	{
		if (bytes == null)
			throw new ArgumentNullException ("bytes");
		if (byteIndex < 0 || bytes.Length <= byteIndex)
			throw new ArgumentOutOfRangeException ("byteIndex");
		if (byteCount < 0 || bytes.Length < byteIndex + byteCount)
			throw new ArgumentOutOfRangeException ("byteCount");
	}

	void CheckArguments (ref char[] chars, int charCount, ref byte[] bytes, int byteCount)
	{
		if (chars == null)
			throw new ArgumentNullException ("chars");
		if (bytes == null)
			throw new ArgumentNullException ("bytes");
		if (charCount < 0)
			throw new ArgumentOutOfRangeException ("charCount");
		if (byteCount < 0)
			throw new ArgumentOutOfRangeException ("byteCount");
	}
#endif

}; // class Decoder

}; // namespace System.Text
