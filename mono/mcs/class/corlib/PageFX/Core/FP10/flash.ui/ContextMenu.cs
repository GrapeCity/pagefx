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
    [PageFX.ABC]
    [PageFX.FP9]
    public class ContextMenu : flash.events.EventDispatcher
    {
        public extern virtual ContextMenuBuiltInItems builtInItems
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

        public extern virtual ContextMenuClipboardItems clipboardItems
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

        public extern virtual Avm.Array customItems
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

        public extern virtual bool clipboardMenu
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

        public extern virtual flash.net.URLRequest link
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

        [PageFX.Event("menuSelect")]
        public event flash.events.ContextMenuEventHandler menuSelect
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenu();

        /// <summary>
        /// Creates a copy of the specified ContextMenu object. The copy inherits all the properties of the
        /// original menu object.
        /// </summary>
        /// <returns>A ContextMenu object with all the properties of the original menu object.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual ContextMenu clone();

        /// <summary>
        /// Hides all built-in menu items (except Settings) in the specified ContextMenu object. If the debugger version of Flash
        /// Player is running, the Debugging menu item appears, although it is dimmed for SWF files that
        /// do not have remote debugging enabled.
        /// This method hides only menu items that appear in the standard context menu; it does not affect
        /// items that appear in the edit and error menus. This method works by setting all the Boolean members of my_cm.builtInItems to false. You can selectively make a built-in item visible by setting its
        /// corresponding member in my_cm.builtInItems to true.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void hideBuiltInItems();
    }
}
