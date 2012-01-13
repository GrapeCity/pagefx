package Shapes
{
	public final class Point
	{
		public var X:Number;
		public var Y:Number;

		function Point(x:Number, y:Number)
		{
			this.X = x;
			this.Y = y;
		}

		public function Move(dx:Number, dy:Number):void
		{
			X += dx;
			Y += dy;
		}
	}

	public class Shape
	{
		public virtual function Draw():void
		{
			print("Shape::Draw");
		}
	}

	public class Line extends Shape
	{
		public var Begin:Point = new Point(0, 0);
		public var End:Point = new Point(0, 0);

		public override function Draw():void
		{
			print("Line::Draw");
			print(Begin.X, Begin.Y, End.X, End.Y);
		}
	}

	public class Circle extends Shape
	{
		public var Center:Point;
		public var Radius:Number;

		public override function Draw():void
		{
			print("Circle::Draw");
		}
	}
}

package PageFX
{
	public namespace PFX = "http://www.datadynamics.com/pfx/internal";

	public class Component
	{
		public virtual function Init():void
		{
			PFX::OnInit();
			print("Component::initialized");
		}

		PFX virtual function OnInit():void
		{
			print("Component::OnInit");
		}

		public virtual function Print(s:String = "msg", n:int = 10):void
		{
			print(s);
			print(n);
		}
	}

	public class Util
	{
		public static function TestForEachInEmptyObject():void
		{
			var obj:Object = {};
			for each (var p in obj)
			{
				print("-");
			}
			print("ok");
		}
	}
}