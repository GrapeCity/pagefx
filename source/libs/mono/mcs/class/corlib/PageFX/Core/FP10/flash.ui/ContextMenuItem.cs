using System;
using System.Runtime.CompilerServices;

namespace flash.ui
{
    /// <summary>
    /// Use the ContextMenuItem class to create custom menu items to display in the Flash Player context
    /// menu. Each ContextMenuItem object has a caption (text) that is displayed in the context menu. To add
    /// a new item to a context menu, you add it to the customItems  array of a
    /// ContextMenu object.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class ContextMenuItem : flash.events.EventDispatcher
    {
        public extern virtual bool enabled
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool separatorBefore
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String caption
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool visible
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [PageFX.Event("menuItemSelect")]
        public event flash.events.ContextMenuEventHandler menuItemSelect
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenuItem(Avm.String arg0, bool arg1, bool arg2, bool arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenuItem(Avm.String arg0, bool arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenuItem(Avm.String arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenuItem(Avm.String arg0);

        /// <summary>
        /// Creates and returns a copy of the specified ContextMenuItem object. The copy includes all properties
        /// of the original object.
        /// </summary>
        /// <returns>A ContextMenuItem object that contains all the properties of the original object.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual ContextMenuItem clone();
    }
}
