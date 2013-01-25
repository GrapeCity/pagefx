using System;
using System.Runtime.CompilerServices;

namespace flash.ui
{
    /// <summary>
    /// The ContextMenu class provides control over the items in the Flash Player context menu. Users open
    /// the context menu by right-clicking (Windows) or Control-clicking (Macintosh) Flash Player. You can use the
    /// methods and properties of the ContextMenu class to add custom menu items, control the display of the
    /// built-in context menu items (for example, Zoom In and Print), or create copies of menus.
    /// </summary>
    [PageFX.AbcInstance(53)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class ContextMenu : flash.display.NativeMenu
    {
        /// <summary>
        /// An object that has the following properties of the ContextMenuBuiltInItems class: forwardAndBack, loop,
        /// play, print, quality,
        /// rewind, save, and zoom.
        /// Setting these properties to false removes the corresponding menu items from the
        /// specified ContextMenu object. These properties are enumerable and are set to true by
        /// default.
        /// </summary>
        public extern virtual flash.ui.ContextMenuBuiltInItems builtInItems
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// An array of ContextMenuItem objects. Each object in the array represents a context menu item that you
        /// have defined. Use this property to add, remove, or modify these custom menu items.
        /// To add new menu items, you create a ContextMenuItem object and then add it to the
        /// customItems array (for example, by using Array.push()). For more information about creating
        /// menu items, see the ContextMenuItem class entry.
        /// </summary>
        public extern virtual Avm.Array customItems
        {
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual flash.net.URLRequest link
        {
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool clipboardMenu
        {
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual flash.ui.ContextMenuClipboardItems clipboardItems
        {
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(12)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern static bool isSupported
        {
            [PageFX.AbcClassTrait(0)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [PageFX.Event("menuSelect")]
        public event flash.events.ContextMenuEventHandler menuSelect
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenu();

        /// <summary>
        /// Hides all built-in menu items (except Settings) in the specified ContextMenu object. If the debugger version of Flash
        /// Player is running, the Debugging menu item appears, although it is dimmed for SWF files that
        /// do not have remote debugging enabled.
        /// This method hides only menu items that appear in the standard context menu; it does not affect
        /// items that appear in the edit and error menus. This method works by setting all the Boolean members of my_cm.builtInItems to false. You can selectively make a built-in item visible by setting its
        /// corresponding member in my_cm.builtInItems to true.
        /// </summary>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void hideBuiltInItems();

        /// <summary>
        /// Creates a copy of the specified ContextMenu object. The copy inherits all the properties of the
        /// original menu object.
        /// </summary>
        /// <returns>A ContextMenu object with all the properties of the original menu object.</returns>
        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.ui.ContextMenu clone();


    }
}
