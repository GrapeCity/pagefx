//
// System.Drawing.Drawing2D.ExtendedGraphicsState.jvm.cs
//
// Author:
//   Vladimir Krasnov (vladimirk@mainsoft.com)
//
// Copyright (C) 2005 Mainsoft Corporation (http://www.mainsoft.com)
//

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
using System.Drawing.Text;
using flash.display;

namespace System.Drawing.Drawing2D 
{
	public sealed class GraphicsState : MarshalByRefObject
	{
		readonly CompositingMode _compositingMode;
		readonly CompositingQuality _compositingQuality;
		readonly Region _clip;
		readonly InterpolationMode _interpolationMode;
		readonly float _pageScale;
		readonly GraphicsUnit _pageUnit;
		readonly PixelOffsetMode _pixelOffsetMode;
		readonly Point _renderingOrigin;
		readonly SmoothingMode _smoothingMode;
		readonly int _textContrast;
		readonly TextRenderingHint _textRenderingHint;
		readonly Matrix _transform;
        readonly DisplayObject _clipMask;

		internal GraphicsState(Graphics graphics, bool resetState) 
			: this(graphics, Matrix.IdentityTransform, resetState) {}

		internal GraphicsState(Graphics g, Matrix matrix, bool resetState)
		{
			_compositingMode = g.CompositingMode;
			_compositingQuality = g.CompositingQuality;
			_clip = g.Clip;
			_interpolationMode = g.InterpolationMode;
			_pageScale = g.PageScale;
			_pageUnit = g.PageUnit;
			_pixelOffsetMode = g.PixelOffsetMode;
			
			// FIXME: render orign is not implemented yet
			_renderingOrigin = g.RenderingOrigin;

			_smoothingMode = g.SmoothingMode;
			_transform = g.Transform;
			
			_textContrast = g.TextContrast;
			_textRenderingHint = g.TextRenderingHint;

            _clipMask = g.ClipMask;

			if (resetState)
				ResetState(g, matrix);
		}

        internal void RestoreState(Graphics g)
        {
            g.CompositingMode = _compositingMode;
            g.CompositingQuality = _compositingQuality;
            //g.Clip = _clip;
            g.InterpolationMode = _interpolationMode;
            g.PageScale = _pageScale;
            g.PageUnit = _pageUnit;
            g.PixelOffsetMode = _pixelOffsetMode;

            g.RenderingOrigin = _renderingOrigin;

            g.SmoothingMode = _smoothingMode;
            g.Transform = _transform;
            g.TextContrast = _textContrast;
            g.TextRenderingHint = _textRenderingHint;

            // must be set after the base transform is restored
            //graphics.NativeObject.setClip(_baseClip);
            g.SetMask(_clipMask);
        }

	    void ResetState(Graphics g, Matrix matrix)
		{
			g.CompositingMode = CompositingMode.SourceOver;
			g.CompositingQuality = CompositingQuality.Default;
			g.Clip = null;
			g.InterpolationMode = InterpolationMode.Bilinear;
			g.PageScale = 1.0f;
			g.PageUnit = GraphicsUnit.Display;
			g.PixelOffsetMode = PixelOffsetMode.Default;
			g.RenderingOrigin = new Point(0, 0);

            g.SmoothingMode = SmoothingMode.None;

            g.ResetTransform();
            g.MultiplyTransform(_transform);
            if (matrix != null)
                g.MultiplyTransform(matrix);

            g.TextContrast = 4;
            g.TextRenderingHint = TextRenderingHint.SystemDefault;

            g.SetMask(null);
		}
	}
}
