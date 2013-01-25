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
    [PageFX.AbcInstance(100)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class Mouse : Avm.Object
    {
        public extern static bool supportsCursor
        {
            [PageFX.AbcClassTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static Avm.String cursor
        {
            [PageFX.AbcClassTrait(3)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcClassTrait(4)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern static bool supportsNativeCursor
        {
            [PageFX.AbcClassTrait(7)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Mouse();

        /// <summary>Hides the pointer. The pointer is visible by default.</summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void hide();

        /// <summary>Displays the pointer. The pointer is visible by default.</summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void show();

        [PageFX.AbcClassTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void registerCursor(Avm.String name, flash.ui.MouseCursorData cursor);

        [PageFX.AbcClassTrait(6)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void unregisterCursor(Avm.String name);


    }
}
