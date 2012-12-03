//
// System.Security.Cryptography SHA256Managed Class implementation
//
// Author:
//   Matthew S. Ford (Matthew.S.Ford@Rose-Hulman.Edu)
//
// (C) 2001 
// Copyright (C) 2004, 2005 Novell, Inc (http://www.novell.com)
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System.Runtime.InteropServices;

namespace System.Security.Cryptography {
	
#if NET_2_0
	[ComVisible (true)]
#endif
	public class SHA256Managed : SHA256 {

		private const int BLOCK_SIZE_BYTES =  64;
		private const int HASH_SIZE_BYTES  =  32;
		private uint[] _H;
		private uint[] K;
		private ulong count;
		private byte[] _ProcessingBuffer;   // Used to start data when passed less than a block worth.
		private int _ProcessingBufferCount; // Counts how much data we have stored that still needs processed.
		private uint[] buff;
	
		public SHA256Managed () 
		{
			_H = new uint [8];
			_ProcessingBuffer = new byte [BLOCK_SIZE_BYTES];
			buff = new uint[64];
			Initialize ();
		}

		private uint Ch (uint u, uint v, uint w) 
		{
			return (u&v) ^ (~u&w);
		}

		private uint Maj (uint u, uint v, uint w) 
		{
			return (u&v) ^ (u&w) ^ (v&w);
		}

		private uint Ro0 (uint x) 
		{
			return ((x >> 7) | (x << 25))
				^ ((x >> 18) | (x << 14))
				^ (x >> 3);
		}

		private uint Ro1 (uint x) 
		{
			return ((x >> 17) | (x << 15))
				^ ((x >> 19) | (x << 13))
				^ (x >> 10);
		}

		private uint Sig0 (uint x) 
		{
			return ((x >> 2) | (x << 30))
				^ ((x >> 13) | (x << 19))
				^ ((x >> 22) | (x << 10));
		}

		private uint Sig1 (uint x) 
		{
			return ((x >> 6) | (x << 26))
				^ ((x >> 11) | (x << 21))
				^ ((x >> 25) | (x << 7));
		}

		protected override void HashCore (byte[] rgb, int start, int size) 
		{
			int i;
			State = 1;

			if (_ProcessingBufferCount != 0) {
				if (size < (BLOCK_SIZE_BYTES - _ProcessingBufferCount)) {
					System.Buffer.BlockCopy (rgb, start, _ProcessingBuffer, _ProcessingBufferCount, size);
					_ProcessingBufferCount += size;
					return;
				}
				else {
					i = (BLOCK_SIZE_BYTES - _ProcessingBufferCount);
					System.Buffer.BlockCopy (rgb, start, _ProcessingBuffer, _ProcessingBufferCount, i);
					ProcessBlock (_ProcessingBuffer, 0);
					_ProcessingBufferCount = 0;
					start += i;
					size -= i;
				}
			}

			for (i=0; i<size-size%BLOCK_SIZE_BYTES; i += BLOCK_SIZE_BYTES) {
				ProcessBlock (rgb, start+i);
			}

			if (size%BLOCK_SIZE_BYTES != 0) {
				System.Buffer.BlockCopy (rgb, size-size%BLOCK_SIZE_BYTES+start, _ProcessingBuffer, 0, size%BLOCK_SIZE_BYTES);
				_ProcessingBufferCount = size%BLOCK_SIZE_BYTES;
			}
		}
	
		protected override byte[] HashFinal () 
		{
			byte[] hash = new byte[32];
			int i, j;

			ProcessFinalBlock (_ProcessingBuffer, 0, _ProcessingBufferCount);

			for (i=0; i<8; i++) {
				for (j=0; j<4; j++) {
					hash[i*4+j] = (byte)(_H[i] >> (24-j*8));
				}
			}

			State = 0;
			return hash;
		}

		public override void Initialize () 
		{
			count = 0;
			_ProcessingBufferCount = 0;
		
			_H[0] = 0x6A09E667;
			_H[1] = 0xBB67AE85;
			_H[2] = 0x3C6EF372;
			_H[3] = 0xA54FF53A;
			_H[4] = 0x510E527F;
			_H[5] = 0x9B05688C;
			_H[6] = 0x1F83D9AB;
			_H[7] = 0x5BE0CD19;
		}

		private void ProcessBlock (byte[] inputBuffer, int inputOffset) 
		{
			uint a, b, c, d, e, f, g, h;
			uint t1, t2;
			int i;
		
			count += BLOCK_SIZE_BYTES;

			for (i=0; i<16; i++) {
				buff[i] = ((uint)(inputBuffer[inputOffset+4*i]) << 24)
					| ((uint)(inputBuffer[inputOffset+4*i+1]) << 16)
					| ((uint)(inputBuffer[inputOffset+4*i+2]) <<  8)
					| ((uint)(inputBuffer[inputOffset+4*i+3]));
			}

		
			for (i=16; i<64; i++) {
				buff[i] = Ro1(buff[i-2]) + buff[i-7] + Ro0(buff[i-15]) + buff[i-16];
			}

			a = _H[0];
			b = _H[1];
			c = _H[2];
			d = _H[3];
			e = _H[4];
			f = _H[5];
			g = _H[6];
			h = _H[7];

			for (i=0; i<64; i++) {
				t1 = h + Sig1(e) + Ch(e,f,g) + SHAConstants.K1 [i] + buff[i];
				t2 = Sig0(a) + Maj(a,b,c);
				h = g;
				g = f;
				f = e;
				e = d + t1;
				d = c;
				c = b;
				b = a;
				a = t1 + t2;
			}

			_H[0] += a;
			_H[1] += b;
			_H[2] += c;
			_H[3] += d;
			_H[4] += e;
			_H[5] += f;
			_H[6] += g;
			_H[7] += h;
		}
	
		private void ProcessFinalBlock (byte[] inputBuffer, int inputOffset, int inputCount) 
		{
			ulong total = count + (ulong)inputCount;
			int paddingSize = (56 - (int)(total % BLOCK_SIZE_BYTES));

			if (paddingSize < 1)
				paddingSize += BLOCK_SIZE_BYTES;

			byte[] fooBuffer = new byte[inputCount+paddingSize+8];

			for (int i=0; i<inputCount; i++) {
				fooBuffer[i] = inputBuffer[i+inputOffset];
			}

			fooBuffer[inputCount] = 0x80;
			for (int i=inputCount+1; i<inputCount+paddingSize; i++) {
				fooBuffer[i] = 0x00;
			}

			// I deal in bytes. The algorithm deals in bits.
			ulong size = total << 3;
			AddLength (size, fooBuffer, inputCount+paddingSize);
			ProcessBlock (fooBuffer, 0);

			if (inputCount+paddingSize+8 == 128) {
				ProcessBlock(fooBuffer, 64);
			}
		}

		internal void AddLength (ulong length, byte[] buffer, int position)
		{
			buffer [position++] = (byte)(length >> 56);
			buffer [position++] = (byte)(length >> 48);
			buffer [position++] = (byte)(length >> 40);
			buffer [position++] = (byte)(length >> 32);
			buffer [position++] = (byte)(length >> 24);
			buffer [position++] = (byte)(length >> 16);
			buffer [position++] = (byte)(length >>  8);
			buffer [position]   = (byte)(length);
		}
	}
}

