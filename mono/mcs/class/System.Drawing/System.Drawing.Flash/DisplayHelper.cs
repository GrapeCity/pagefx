using System.Collections.Generic;
using flash.display;

namespace System.Drawing.Flash
{
    class DisplayHelper
    {
        public static IEnumerable<DisplayObject> EnumKinds(DisplayObjectContainer container)
        {
            int n = container.numChildren;
            for (int i = 0; i < n; i++)
                yield return container.getChildAt(i);
        }

        public static void RemoveAllChildren(DisplayObjectContainer container)
        {
            while (container.numChildren > 0)
                container.removeChildAt(0);
        }
    }
}