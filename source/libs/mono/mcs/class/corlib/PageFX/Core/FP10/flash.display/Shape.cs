using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// This class is used to create lightweight shapes using the ActionScript drawing application program interface (API).
    /// The Shape class includes a graphics  property, which lets you access methods from the Graphics class.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class Shape : DisplayObject
    {
        public extern virtual Graphics graphics
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Shape();


    }
}
