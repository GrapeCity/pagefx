using flash.display;
using flash.text;
using flash.events;
using System;
using System.Collections.Generic;

[Root]
class App : Sprite
{
    
    class SimpleLine
    {
        public uint x0;
        public uint y0;
        public uint x1;
        public uint y1;
    }

    List<SimpleLine> lines = new List<SimpleLine>();

    public App()
    {
        InitLines();
        render += Render;
        enterFrame += MoveLines;
    }

    const int maxLineNumber = 10;
    private void InitLines()
    {
        for (int i = 0; i < maxLineNumber; i++)
        {
            SimpleLine line = new SimpleLine();
            lines.Add(line);
        }
    }

    private void Render(Event e)
    {
        var g = graphics;
        g.clear();
        foreach (var line in lines)
        {
            g.lineStyle(1, (uint)(Avm.Math.random() * 0xFFFFFF), 1.0);
            MoveLine(line);
            g.moveTo(line.x0, line.y0);
            g.lineTo(line.x1, line.y1);
        }
    }

    private void MoveLines(Event e)
    {
        foreach (var line in lines)
        {
            MoveLine(line);
        }
        stage.invalidate();
    }

    private void MoveLine(SimpleLine line)
    {
        line.x0 = (uint)(Avm.Math.abs(Avm.Math.random()) * stage.stageWidth);
        line.y0 = (uint)(Avm.Math.abs(Avm.Math.random()) * stage.stageHeight);
        line.x1 = (uint)(Avm.Math.abs(Avm.Math.random()) * stage.stageWidth);
        line.y1 = (uint)(Avm.Math.abs(Avm.Math.random()) * stage.stageHeight);
    }
}
