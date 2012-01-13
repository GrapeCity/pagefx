using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    /// <summary>
    /// This class represents StaticText objects on the display list.
    /// You cannot create a StaticText object using ActionScript. Only the authoring tool
    /// can create a StaticText object. An attempt to create a new StaticText object generates
    /// an ArgumentError .
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class StaticText : flash.display.DisplayObject
    {
        public extern virtual Avm.String text
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StaticText();


    }
}
