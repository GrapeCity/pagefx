using System.Drawing.Drawing2D;
using System.Drawing;
using Avm;
using flash.display;
using flash.geom;

namespace System.Drawing.Flash
{
    using Graphics = flash.display.Graphics;
    using Matrix = flash.geom.Matrix;

	internal static class DashedLine
	{
		public static void SetLineStyle(Graphics graphics, DashStyle dashStyle, float thickness, Color foreColor, Color backColor)
		{
			SetLineStyle(graphics, dashStyle, thickness, foreColor, backColor, 0, "none");
		}

		public static void SetLineStyle(Graphics graphics, DashStyle dashStyle, float thickness, Color foreColor, Color backColor, double rotation, string LineCap)
		{
			graphics.lineStyle(thickness, (uint)foreColor.ToArgb(), 100, false, "normal", LineCap);
			if (dashStyle != DashStyle.Solid)
			{
				BitmapData bmData = GetTileBitmap(dashStyle, thickness, foreColor);
				Matrix matrix = new Matrix();
				if (rotation != 0)
					matrix.rotate(rotation);
				graphics.lineBitmapStyle(bmData, matrix, true, false);
			}
		}

		private static System.Collections.Generic.List<Tile> _tiles;
		private static System.Collections.Generic.List<Tile> Tiles
		{
			get
			{
				if (_tiles == null)
				{
					_tiles = new System.Collections.Generic.List<Tile>();
				}
				return _tiles;
			}
		}

		public static BitmapData GetTileBitmap(DashStyle dashStyle, float thickness, Color color)
		{
			// fix for case 122461: values lower 1 causes bitmap rectangle to get empty
			thickness = System.Math.Max(1, thickness);

			//try to find DashTile in cache
			foreach (Tile tile in Tiles)
			{
				if (tile.DashStyle == dashStyle && tile.Color == color && tile.Thickness == thickness)
					return tile.BitmapData;
			}

			//if dashtile is not cached, create it and add to cache
			Shape dashTile;
			switch (dashStyle)
			{
				case DashStyle.Dash:
					dashTile = DashTile;
					break;
				case DashStyle.DashDot:
					dashTile = DashDotTile;
					break;
				case DashStyle.DashDotDot:
					dashTile = DashDotDotTile;
					break;
				case DashStyle.Dot:
					dashTile = DotTile;
					break;
				default:
					throw new System.NotSupportedException();
			}

			dashTile.height = DotCap;

			// prepare matrix and color transforms for the tile
			flash.geom.Matrix scaleMatrix = new flash.geom.Matrix();
			scaleMatrix.scale(thickness, thickness);
			ColorTransform colorTransform = new ColorTransform(0, 0, 0, 0, color.R, color.G, color.B, 255);

			// draw bitmap with a transformed tile
			BitmapData bmData = new BitmapData((int)Avm.Math.ceil(dashTile.width * thickness), (int)(dashTile.height * thickness), true, 0x00ffffff); //default transparent color
			bmData.draw(dashTile, scaleMatrix, colorTransform);

			//add to cache
			Tiles.Add(new Tile(bmData, dashStyle, thickness, color));
			
			return bmData;

		}

		private struct Tile
		{
			public readonly BitmapData BitmapData;
			public readonly DashStyle DashStyle;
			public readonly float Thickness;
			public readonly Color Color;

			public Tile(BitmapData bitmapData, DashStyle dashStyle, float thickness, Color color)
			{
				DashStyle = dashStyle;
				BitmapData = bitmapData;
				Thickness = thickness;
				Color = color;
			}
		}

		#region Dash tiles implementation

		private const int DashCap = 3;
		private const int DotCap = 1;
		private const int SpaceCap = 1;
		

		private static Shape _dashTile;
		private static Shape DashTile
		{
			get
			{
				if (_dashTile == null)
				{
					_dashTile = new Shape();
					_dashTile.graphics.beginFill(0x000000, 1);
					_dashTile.graphics.drawRect(0, 0, DashCap, DotCap);
					_dashTile.width = DashCap + SpaceCap;
					_dashTile.graphics.endFill();
				}
				return _dashTile;
			}
		}

		private static Shape _dotTile;
		private static Shape DotTile
		{
			get
			{
				if (_dotTile == null)
				{
					_dotTile = new Shape();
					_dotTile.graphics.beginFill(0x000000, 1);
					_dotTile.graphics.drawRect(0, 0, DotCap, DotCap);
					_dotTile.width = DotCap + SpaceCap;
					_dotTile.graphics.endFill();
				}
				return _dotTile;
			}
		}

		private static Shape _dashDotTile;
		private static Shape DashDotTile
		{
			get
			{
				if (_dashDotTile == null)
				{
					_dashDotTile = new Shape();
					_dashDotTile.graphics.beginFill(0x000000, 1);
					_dashDotTile.graphics.drawRect(0, 0, DashCap, DotCap);
					_dashDotTile.graphics.drawRect(DashCap + SpaceCap, 0, DotCap, DotCap);
					_dashDotTile.width = DashCap + SpaceCap + DotCap + SpaceCap;
					_dashDotTile.graphics.endFill();
				}
				return _dashDotTile;
			}
		}

		private static Shape _dashDotDotTile;
		private static Shape DashDotDotTile
		{
			get
			{
				if (_dashDotDotTile == null)
				{
					_dashDotDotTile = new Shape();
					_dashDotDotTile.graphics.beginFill(0x000000, 1);
					_dashDotDotTile.graphics.drawRect(0, 0, DashCap, DotCap);
					_dashDotDotTile.graphics.drawRect(DashCap + SpaceCap, 0, DotCap, DotCap);
					_dashDotDotTile.graphics.drawRect(DashCap + SpaceCap + DotCap + SpaceCap, 0, DotCap, DotCap);
					_dashDotDotTile.width = DashCap + (SpaceCap + DotCap) * 2 + SpaceCap;
					_dashDotDotTile.graphics.endFill();

				}
				return _dashDotDotTile;
			}
		}

		#endregion

	}
}
