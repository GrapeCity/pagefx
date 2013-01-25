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
    [PageFX.AbcInstance(246)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class ContextMenuItem : flash.display.NativeMenuItem
    {
        /// <summary>
        /// Specifies the menu item caption (text) displayed in the context menu.
        /// See the ContextMenuItem class overview for caption value restrictions.
        /// </summary>
        public extern virtual Avm.String caption
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

        /// <summary>
        /// Indicates whether a separator bar should appear above the specified menu item.
        /// Note: A separator bar always appears between any custom menu items and the
        /// built-in menu items.
        /// </summary>
        public extern virtual bool separatorBefore
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates whether the specified menu item is visible when the Flash Player
        /// context menu is displayed.
        /// </summary>
        public extern virtual bool visible
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(5)]
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
        public extern ContextMenuItem(Avm.String caption, bool separatorBefore, bool enabled, bool visible);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenuItem(Avm.String caption, bool separatorBefore, bool enabled);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenuItem(Avm.String caption, bool separatorBefore);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenuItem(Avm.String caption);

        /// <summary>
        /// Creates and returns a copy of the specified ContextMenuItem object. The copy includes all properties
        /// of the original object.
        /// </summary>
        /// <returns>A ContextMenuItem object that contains all the properties of the original object.</returns>
        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.ui.ContextMenuItem clone();
    }
}
