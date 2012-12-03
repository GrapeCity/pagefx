using System;
using System.Runtime.CompilerServices;

namespace flash.ui
{
    /// <summary>
    /// The ContextMenuBuiltInItems class describes the items that are built in to a context menu.
    /// You can hide these items by using the ContextMenu.hideBuiltInItems()  method.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class ContextMenuBuiltInItems : Avm.Object
    {
        /// <summary>
        /// Lets the user set a SWF file to start over automatically when it reaches the final
        /// frame (does not appear for a single-frame SWF file).
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public bool loop;

        /// <summary>Lets the user send the displayed frame image to a printer.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public bool print;

        /// <summary>Lets the user zoom in and out on a SWF file at run time.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public bool zoom;

        /// <summary>Lets the user start a paused SWF file (does not appear for a single-frame SWF file).</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public bool play;

        /// <summary>
        /// Lets the user move forward or backward one frame in a SWF file at run time (does not
        /// appear for a single-frame SWF file).
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public bool forwardAndBack;

        /// <summary>
        /// Lets the user set a SWF file to play from the first frame when selected, at any time (does not
        /// appear for a single-frame SWF file).
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public bool rewind;

        /// <summary>Lets the user with Shockmachine installed save a SWF file.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public bool save;

        /// <summary>Lets the user set the resolution of the SWF file at run time.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public bool quality;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenuBuiltInItems();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual ContextMenuBuiltInItems clone();
    }
}
