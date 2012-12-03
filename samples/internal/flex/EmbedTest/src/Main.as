package 
{
	import flash.display.Graphics;
	import flash.display.Sprite;
	
	import mx.core.BitmapAsset;
	
	public class Main extends Sprite
	{
		[Embed(source="../images/docnew.png")]
		private static var DocNew:Class;
		
		public function Main()
		{
			var img:BitmapAsset = new DocNew() as BitmapAsset;
			var g:Graphics = this.graphics;
			g.beginBitmapFill(img.bitmapData);
			g.drawCircle(100, 100, 100);
		}
	}
}
