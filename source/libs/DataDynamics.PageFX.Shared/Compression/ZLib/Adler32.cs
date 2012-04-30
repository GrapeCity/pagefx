namespace DataDynamics.ZLib
{
	internal class Adler32
	{
		// largest prime smaller than 65536
		private const int BASE = 65521; 

		// NMAX is the largest n such that 255 n (n + 1) / 2 + (n+1)(BASE - 1) <= 2 ^ 32 - 1
		private const int NMAX = 5552;

		// adler this is your check sum
		// buffer Buffer of bits to check
		// index - Index into the buffer
		// nLength - the length of the buffer

		internal long Calculate(long adler, byte[] buffer, int index, int length)
		{
			if (buffer == null)
				return 1L;

			long nS1 = adler & 0xffff;
			long nS2 = (adler >> 16) & 0xffff;
			int nK;

			while (length > 0) 
			{
				nK = length < NMAX ? length : NMAX;
				length -= nK;
				while (nK >= 16)
				{
					nS1 += buffer[index++] & 0xff; 
					nS2 += nS1;
					nS1 += buffer[index++] & 0xff; 
					nS2 += nS1;
					nS1 += buffer[index++] & 0xff; 
					nS2 += nS1;
					nS1 += buffer[index++] & 0xff; 
					nS2 += nS1;
					nS1 += buffer[index++] & 0xff; 
					nS2 += nS1;
					nS1 += buffer[index++] & 0xff; 
					nS2 += nS1;
					nS1 += buffer[index++] & 0xff; 
					nS2 += nS1;
					nS1 += buffer[index++] & 0xff; 
					nS2 += nS1;
					nS1 += buffer[index++] & 0xff; 
					nS2 += nS1;
					nS1 += buffer[index++] & 0xff; 
					nS2 += nS1;
					nS1 += buffer[index++] & 0xff; 
					nS2 += nS1;
					nS1 += buffer[index++] & 0xff; 
					nS2 += nS1;
					nS1 += buffer[index++] & 0xff; 
					nS2 += nS1;
					nS1 += buffer[index++] & 0xff; 
					nS2 += nS1;
					nS1 += buffer[index++] & 0xff; 
					nS2 += nS1;
					nS1 += buffer[index++] & 0xff; 
					nS2 += nS1;
					nK -= 16;
				}
				if (nK != 0)
				{
					do
					{
						nS1 += buffer[index++] & 0xff; 
						nS2 += nS1;
					}
					while(--nK != 0);
				}
				nS1 %= BASE;
				nS2 %= BASE;
			}
			return (nS2 << 16) | nS1;
		}
	}
}