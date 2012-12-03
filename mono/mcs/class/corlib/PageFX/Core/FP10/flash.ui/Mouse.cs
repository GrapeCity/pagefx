using System;
using System.Runtime.CompilerServices;

namespace flash.ui
{
    /// <summary>
    /// The methods of the Mouse class are used to hide and show the mouse pointer.
    /// The Mouse class is a top-level class whose properties and methods
    /// you can access without using a constructor. The pointer is visible by default,
    /// but you can hide it and implement a custom pointer.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class Mouse : Avm.Object
    {
        public extern static Avm.String cursor
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Mouse();

        /// <summary>Hides the pointer. The pointer is visible by default.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void hide();

        /// <summary>Displays the pointer. The pointer is visible by default.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void show();


    }
}
