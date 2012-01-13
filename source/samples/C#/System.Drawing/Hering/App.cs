//
// hering.cs 
// Creates image for Hering illusion.
// Converted to C# from Xr demo application.
//
// Author:
//   Alexandre Pigolkine(pigolkine@gmx.de)
// 
//
// Copyright (C) Ximian, Inc.  http://www.ximian.com
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
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

namespace MonoSamples.System.Drawing
{
    [Root]
    public class Hering : flash.display.Sprite
    {
        public Hering()
        {
            float W = 400.0F;
            float H = 800.0F;

            Graphics gr = new Graphics(this);
            gr.Clear(Color.White);

            int LINES = 32;
            float MAX_THETA = (.80F * 90.0F);
            float THETA = (2 * MAX_THETA / (LINES - 1));

            GraphicsState oldState = gr.Save();

            Pen blackPen = new Pen(Color.Black, 2.0F);
            gr.TranslateTransform(W / 2.0F, H / 2.0F);
            gr.RotateTransform(MAX_THETA);
            for (int i = 0; i < LINES; i++)
            {
                gr.DrawLine(blackPen, -2.0F * W, 0.0F, 2.0F * W, 0.0F);
                gr.RotateTransform(-THETA);
            }

            gr.Restore(oldState);

            Pen redPen = new Pen(Color.Red, 6F);
            gr.DrawLine(redPen, W / 4F, 0F, W / 4F, H);
            gr.DrawLine(redPen, 3F * W / 4F, 0F, 3F * W / 4F, H);
        }
    }
}
