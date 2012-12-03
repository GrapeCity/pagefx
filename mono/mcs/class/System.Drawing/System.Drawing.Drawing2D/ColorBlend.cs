//CHANGED
//
// System.Drawing.Drawing2D.ColorBlend.cs
//
// Authors:
//   Dennis Hayes (dennish@Raytek.com)
//   Ravindra (rkumar@novell.com)
//
// (C) 2002/3 Ximian, Inc. http://www.ximian.com
// Copyright (C) 2004, 2006 Novell, Inc (http://www.novell.com)
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

namespace System.Drawing.Drawing2D
{
	public sealed class ColorBlend : ICloneable
	{
		private float [] positions;
		private Color [] colors;

		public ColorBlend ()
		{
			positions = new float [1];
			colors = new Color [1];
		}

		public ColorBlend (int count)
		{
			positions = new float [count];
			colors = new Color [count];
		}

        internal ColorBlend(ColorBlend cb)
        {
            int n = cb.positions.Length;
            positions = new float[n];
            colors = new Color[n];
            for (int i = 0; i < n; ++i)
            {
                positions[i] = cb.positions[i];
                colors[i] = cb.colors[i];
            }
        }

	    public Color[] Colors
        {
			get {
				return colors;
			}

			set {
				colors = value;
			}
		}

		public float [] Positions {
			get {
				return positions;
			}

			set {
				positions = value;				
			}
		}

        public ColorBlend Clone()
        {
            return new ColorBlend(this);
        }

	    object ICloneable.Clone()
	    {
            return Clone();
	    }
	}
}
