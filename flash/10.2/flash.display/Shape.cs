using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// This class is used to create lightweight shapes using the ActionScript drawing application program interface (API).
    /// The Shape class includes a graphics  property, which lets you access methods from the Graphics class.
    /// </summary>
    [PageFX.AbcInstance(136)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class Shape : flash.display.DisplayObject
    {
        /// <summary>
        /// Specifies the Graphics object belonging to this Shape object, where vector
        /// drawing commands can occur.
        /// </summary>
        public extern virtual flash.display.Graphics graphics
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Shape();


    }
}
