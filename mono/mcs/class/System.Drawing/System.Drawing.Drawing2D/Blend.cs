//CHANGED
//
// System.Drawing.Drawing2D.Blend.cs
//
// Authors:
//   Dennis Hayes (dennish@Raytek.com)
//   Ravindra (rkumar@novell.com)
//
// (C) 2002/3 Ximian, Inc. http://www.ximian.com
// (C) 2004 Novell, Inc. http://www.novell.com
//

//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
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
using System;

namespace System.Drawing.Drawing2D
{
    /// <summary>
    /// Summary description for Blend.
    /// </summary>
#if TARGET_JVM
	[MonoTODO]
#endif
    public sealed class Blend
    {
        float[] positions;
        float[] factors;

        public Blend()
        {
            positions = new float[1];
            factors = new float[1];
        }

        public Blend(int count)
        {
            positions = new float[count];
            factors = new float[count];
        }

        Blend(Blend b)
        {
            int n = b.positions.Length;
            positions = new float[n];
            factors = new float[n];
            for (int i = 0; i < n; ++i)
            {
                positions[i] = b.positions[i];
                factors[i] = b.factors[i];
            }
        }

        public float[] Factors
        {
            get
            {
                return factors;
            }

            set
            {
                factors = value;
            }
        }

        public float[] Positions
        {
            get
            {
                return positions;
            }

            set
            {
                positions = value;
            }
        }

        internal Blend Clone()
        {
            return new Blend(this);
        }
    }
}
