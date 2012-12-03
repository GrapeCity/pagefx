using System;
using System.Drawing.Imaging;
using System.Drawing;
using System.Drawing.Drawing2D;

[Root]
public class App : flash.display.Sprite
{
    public App()
	{
		bool drawSolid = true;
		Graphics dc = new Graphics(this);

		// Clears and set the background color to red
		//dc.Clear (Color.Red);

		SolidBrush blueBrush = new SolidBrush (Color.Blue);
		SolidBrush redBrush = new SolidBrush (Color.Red);
		SolidBrush yellowBrush = new SolidBrush (Color.Yellow);
		SolidBrush whiteBrush = new SolidBrush (Color.White);				
		Pen bluePen = new Pen (Color.Blue);

		LetsDrawIt(dc, bluePen, blueBrush, redBrush, yellowBrush, whiteBrush);
	        stage.click += e => 
        	{
		    if (drawSolid)
			{
			 	drawSolid = false;
				bluePen.DashStyle = DashStyle.Dash;
			}
		    else
			{
			 	drawSolid = true;
				bluePen.DashStyle = DashStyle.Solid;
			}
			LetsDrawIt(dc, bluePen, blueBrush, redBrush, yellowBrush, whiteBrush);
       		};
	}

	private void LetsDrawIt(Graphics dc, Pen bluePen, SolidBrush blueBrush, SolidBrush redBrush, SolidBrush yellowBrush, SolidBrush whiteBrush)
	{
		graphics.clear();
		while (this.numChildren > 0)
	            removeChildAt(0);

		// We have a column starting at x=50 for Draw operations
		// and another column starting at x=200 for Fill operations.
		// Both the columns grow downwards.

		// Column 1
		Rectangle rect11 = new Rectangle (50, 0, 75, 75);
		dc.DrawPie (bluePen, rect11, 10, 60);
		
		Rectangle rect12 = new Rectangle (50,100, 75, 75);
		dc.DrawPie (bluePen, rect12, 100, 75);

		Rectangle rect13 = new Rectangle (50, 200, 75, 75);
		dc.DrawPie (bluePen, rect13, 100, 400);

		Rectangle rect14 = new Rectangle (50, 300, 75, 75);
		dc.DrawPie (bluePen, rect14, 0, 0);

		// Column 2
		Rectangle rect21 = new Rectangle (200, 0, 75, 75);
		dc.FillPie (yellowBrush, rect21, 0, 300);
		
		Rectangle rect22 = new Rectangle (200, 100, 75, 75);
		dc.FillPie (whiteBrush, rect22, 200, 30);
                
		Rectangle rect23 = new Rectangle (200, 200, 75, 75);
		dc.FillPie (whiteBrush, rect23, 200, 400);

		Rectangle rect24 = new Rectangle (200, 300, 75, 75);
		dc.FillPie (yellowBrush, rect24, 190, 300);
		
		Rectangle rect25 = new Rectangle (200, 400, 75, 75);
		dc.FillPie (whiteBrush, rect25, 200, 20);

		Rectangle rect26 = new Rectangle (200, 500, 75, 75);
		dc.FillPie (yellowBrush, rect26, 50, 0);

		stage.invalidate();
	}
}
