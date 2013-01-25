using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The MorphShape class represents MorphShape objects on the display list.
    /// You cannot create MorphShape objects directly in ActionScript; they are created when you create a shape tween
    /// in the Flash authoring tool.
    /// </summary>
    [PageFX.AbcInstance(124)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class MorphShape : flash.display.DisplayObject
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern MorphShape();
    }
}
