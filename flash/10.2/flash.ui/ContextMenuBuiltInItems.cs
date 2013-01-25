using System;
using System.Runtime.CompilerServices;

namespace flash.ui
{
    /// <summary>
    /// The ContextMenuBuiltInItems class describes the items that are built in to a context menu.
    /// You can hide these items by using the ContextMenu.hideBuiltInItems()  method.
    /// </summary>
    [PageFX.AbcInstance(298)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class ContextMenuBuiltInItems : Avm.Object
    {
        /// <summary>Lets the user with Shockmachine installed save a SWF file.</summary>
        public extern virtual bool save
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>Lets the user zoom in and out on a SWF file at run time.</summary>
        public extern virtual bool zoom
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>Lets the user set the resolution of the SWF file at run time.</summary>
        public extern virtual bool quality
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>Lets the user start a paused SWF file (does not appear for a single-frame SWF file).</summary>
        public extern virtual bool play
        {
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Lets the user set a SWF file to start over automatically when it reaches the final
        /// frame (does not appear for a single-frame SWF file).
        /// </summary>
        public extern virtual bool loop
        {
            [PageFX.AbcInstanceTrait(12)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(13)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Lets the user set a SWF file to play from the first frame when selected, at any time (does not
        /// appear for a single-frame SWF file).
        /// </summary>
        public extern virtual bool rewind
        {
            [PageFX.AbcInstanceTrait(15)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(16)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Lets the user move forward or backward one frame in a SWF file at run time (does not
        /// appear for a single-frame SWF file).
        /// </summary>
        public extern virtual bool forwardAndBack
        {
            [PageFX.AbcInstanceTrait(18)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(19)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>Lets the user send the displayed frame image to a printer.</summary>
        public extern virtual bool print
        {
            [PageFX.AbcInstanceTrait(21)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(22)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenuBuiltInItems();

        [PageFX.AbcInstanceTrait(24)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.ui.ContextMenuBuiltInItems clone();
    }
}
