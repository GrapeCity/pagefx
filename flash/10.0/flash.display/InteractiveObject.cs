using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The InteractiveObject class is the abstract base class for all display objects with which the user can
    /// interact, using the mouse and keyboard.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class InteractiveObject : DisplayObject
    {
        public extern virtual flash.accessibility.AccessibilityImplementation accessibilityImplementation
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

        public extern virtual object focusRect
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

        public extern virtual bool doubleClickEnabled
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

        public extern virtual flash.ui.ContextMenu contextMenu
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

        public extern virtual bool tabEnabled
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

        public extern virtual bool mouseEnabled
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

        public extern virtual int tabIndex
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

        [PageFX.Event("tabIndexChange")]
        public event flash.events.EventHandler tabIndexChange
        {
            add { }
            remove { }
        }

        [PageFX.Event("tabEnabledChange")]
        public event flash.events.EventHandler tabEnabledChange
        {
            add { }
            remove { }
        }

        [PageFX.Event("tabChildrenChange")]
        public event flash.events.EventHandler tabChildrenChange
        {
            add { }
            remove { }
        }

        [PageFX.Event("keyUp")]
        public event flash.events.KeyboardEventHandler keyUp
        {
            add { }
            remove { }
        }

        [PageFX.Event("keyDown")]
        public event flash.events.KeyboardEventHandler keyDown
        {
            add { }
            remove { }
        }

        [PageFX.Event("rollOver")]
        public event flash.events.MouseEventHandler rollOver
        {
            add { }
            remove { }
        }

        [PageFX.Event("rollOut")]
        public event flash.events.MouseEventHandler rollOut
        {
            add { }
            remove { }
        }

        [PageFX.Event("mouseWheel")]
        public event flash.events.MouseEventHandler mouseWheel
        {
            add { }
            remove { }
        }

        [PageFX.Event("mouseUp")]
        public event flash.events.MouseEventHandler mouseUp
        {
            add { }
            remove { }
        }

        [PageFX.Event("mouseOver")]
        public event flash.events.MouseEventHandler mouseOver
        {
            add { }
            remove { }
        }

        [PageFX.Event("mouseOut")]
        public event flash.events.MouseEventHandler mouseOut
        {
            add { }
            remove { }
        }

        [PageFX.Event("mouseMove")]
        public event flash.events.MouseEventHandler mouseMove
        {
            add { }
            remove { }
        }

        [PageFX.Event("mouseDown")]
        public event flash.events.MouseEventHandler mouseDown
        {
            add { }
            remove { }
        }

        [PageFX.Event("doubleClick")]
        public event flash.events.MouseEventHandler doubleClick
        {
            add { }
            remove { }
        }

        [PageFX.Event("click")]
        public event flash.events.MouseEventHandler click
        {
            add { }
            remove { }
        }

        [PageFX.Event("mouseFocusChange")]
        public event flash.events.FocusEventHandler mouseFocusChange
        {
            add { }
            remove { }
        }

        [PageFX.Event("keyFocusChange")]
        public event flash.events.FocusEventHandler keyFocusChange
        {
            add { }
            remove { }
        }

        [PageFX.Event("focusOut")]
        public event flash.events.FocusEventHandler focusOut
        {
            add { }
            remove { }
        }

        [PageFX.Event("focusIn")]
        public event flash.events.FocusEventHandler focusIn
        {
            add { }
            remove { }
        }

        [PageFX.Event("selectAll")]
        public event flash.events.EventHandler selectAll
        {
            add { }
            remove { }
        }

        [PageFX.Event("paste")]
        public event flash.events.EventHandler paste
        {
            add { }
            remove { }
        }

        [PageFX.Event("cut")]
        public event flash.events.EventHandler cut
        {
            add { }
            remove { }
        }

        [PageFX.Event("copy")]
        public event flash.events.EventHandler copy
        {
            add { }
            remove { }
        }

        [PageFX.Event("clear")]
        public event flash.events.EventHandler clear
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern InteractiveObject();


    }
}
