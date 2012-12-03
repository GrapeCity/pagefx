using flash.display;
using flash.text;
using flash.events;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Collections.Generic;

[Root]
class App : Sprite
{
    List<KeyValuePair<TextField, Action<TextField>>> Actions = new List<KeyValuePair<TextField, Action<TextField>>>();
    TextField info1;
    TextField info2;
    TextField info3;

    public App()
    {
	    var frame = new Sprite();
        addChild(frame);
        InitActions();
        frame.enterFrame += DoActions;
    }

    void DoActions(Event e)
    {
        foreach (var item in Actions)
        {
            item.Value(item.Key);
        }
    }

    void InitActions()
    {
        info1 = new TextField();
        addChild(info1);

        info2 = new TextField();
        info2.y = 20;
        addChild(info2);

        info3 = new TextField();
        info3.y = 40;
        info3.width += 30;
        addChild(info3);

        var p = new KeyValuePair<TextField, Action<TextField>>(info1, Linq6);
        Actions.Add(p);
        p = new KeyValuePair<TextField, Action<TextField>>(info2, Linq32);
        Actions.Add(p);
        p = new KeyValuePair<TextField, Action<TextField>>(info3, Linq64);
        Actions.Add(p);
    }

    // MSDN LINQ samples

    public void Linq6(TextField tf)
    {
        int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

        var numsPlusOne =
            from n in numbers
            select n + 1;

        foreach (var i in numsPlusOne)
        {
            tf.text += i.ToString() + " ";
        }
    }

    public void Linq64(TextField tf)
    {
        int[] numbers = { 5, 4, 1, 3, 9, 8, 6, 7, 2, 0 };

        int fourthLowNum = (
            from n in numbers
            where n < 5
            select n)
            .ElementAt(3); // 3 because sequences use 0-based indexing

        tf.text += "Fourth number < 5:" + fourthLowNum;
    }

    public void Linq32(TextField tf)
    {
        double[] doubles = { 1.7, 2.3, 1.9, 4.1, 2.9 };

        var sortedDoubles =
            from d in doubles
            orderby d descending
            select d;

        foreach (var d in sortedDoubles)
        {
            tf.text += d + " ";
        }
    }
}
