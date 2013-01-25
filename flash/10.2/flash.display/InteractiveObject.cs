using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The InteractiveObject class is the abstract base class for all display objects with which the user can
    /// interact, using the mouse and keyboard.
    /// </summary>
    [PageFX.AbcInstance(87)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class InteractiveObject : flash.display.DisplayObject
    {
        /// <summary>
        /// Specifies whether this object is in the tab order. If this object is in the tab order,
        /// the value is true; otherwise, the value is false. By default,
        /// the value is false, except for the following:
        /// For a SimpleButton object, the value is true.For a TextField object with type = &quot;input&quot;, the value is true.For a Sprite object or MovieClip object with buttonMode = true, the value is true.
        /// </summary>
        public extern virtual bool tabEnabled
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
        /// Specifies the tab ordering of objects in a SWF file. The tabIndex
        /// property is -1 by default, meaning no tab index is set for the object.
        /// If any currently displayed object in the SWF file contains a tabIndex property, automatic
        /// tab ordering is disabled, and the tab ordering is calculated from the tabIndex properties of
        /// objects in the SWF file. The custom tab ordering includes only objects that have tabIndex
        /// properties.The tabIndex property can be a non-negative integer. The objects are ordered according to
        /// their tabIndex properties, in ascending order. An object with a tabIndex
        /// value of 1 precedes an object with a tabIndex value of 2. If two objects have
        /// the same tabIndex value, the object that precedes the other in the default tab ordering
        /// precedes the other object.The custom tab ordering that the tabIndex property defines is flat.
        /// This means that no attention is paid to the hierarchical relationships of objects in the SWF file.
        /// All objects in the SWF file with tabIndex properties are placed in the tab order, and the
        /// tab order is determined by the order of the tabIndex values.
        /// </summary>
        public extern virtual int tabIndex
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

        /// <summary>
        /// Specifies whether this object displays a focus rectangle. A value of null
        /// indicates that this object obeys the stageFocusRect property set on the Stage.
        /// </summary>
        public extern virtual object focusRect
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

        /// <summary>
        /// Specifies whether this object receives mouse messages. The default value is true,
        /// which means that by default any InteractiveObject instance that is on the display list
        /// receives mouse events.
        /// If mouseEnabled is set to false, the instance does not receive any
        /// mouse events. Any children of this instance on the display list are not affected. To change
        /// the mouseEnabled behavior for all children of an object on the display list, use
        /// flash.display.DisplayObjectContainer.mouseChildren.
        /// No event is dispatched by setting this property. You must use the
        /// addEventListener() method to create interactive functionality.
        /// </summary>
        public extern virtual bool mouseEnabled
        {
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Specifies whether the object receives doubleClick events. The default value
        /// is false, which means that by default an InteractiveObject instance does not receive
        /// doubleClick events. If the doubleClickEnabled property is set to
        /// true, the instance receives doubleClick events within its bounds.
        /// The mouseEnabled property of the InteractiveObject instance must also be
        /// set to true for the object to receive doubleClick events.
        /// No event is dispatched by setting this property. You must use the
        /// addEventListener() method to add an event listener
        /// for the doubleClick event.
        /// </summary>
        public extern virtual bool doubleClickEnabled
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

        public extern virtual flash.accessibility.AccessibilityImplementation accessibilityImplementation
        {
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(12)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual flash.geom.Rectangle softKeyboardInputAreaOfInterest
        {
            [PageFX.AbcInstanceTrait(13)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(14)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool needsSoftKeyboard
        {
            [PageFX.AbcInstanceTrait(15)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(16)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>Specifies the context menu associated with this object.</summary>
        public extern virtual flash.ui.ContextMenu contextMenu
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

        [PageFX.Event("softKeyboardDeactivate")]
        public event flash.events.SoftKeyboardEventHandler softKeyboardDeactivate
        {
            add { }
            remove { }
        }

        [PageFX.Event("softKeyboardActivate")]
        public event flash.events.SoftKeyboardEventHandler softKeyboardActivate
        {
            add { }
            remove { }
        }

        [PageFX.Event("softKeyboardActivating")]
        public event flash.events.SoftKeyboardEventHandler softKeyboardActivating
        {
            add { }
            remove { }
        }

        [PageFX.Event("textInput")]
        public event flash.events.TextEventHandler textInput
        {
            add { }
            remove { }
        }

        [PageFX.Event("imeStartComposition")]
        public event flash.events.IMEEventHandler imeStartComposition
        {
            add { }
            remove { }
        }

        [PageFX.Event("contextMenu")]
        public event flash.events.MouseEventHandler OnContextMenu
        {
            add { }
            remove { }
        }

        [PageFX.Event("nativeDragComplete")]
        public event flash.events.EventHandler nativeDragComplete
        {
            add { }
            remove { }
        }

        [PageFX.Event("nativeDragUpdate")]
        public event flash.events.EventHandler nativeDragUpdate
        {
            add { }
            remove { }
        }

        [PageFX.Event("nativeDragStart")]
        public event flash.events.EventHandler nativeDragStart
        {
            add { }
            remove { }
        }

        [PageFX.Event("nativeDragExit")]
        public event flash.events.EventHandler nativeDragExit
        {
            add { }
            remove { }
        }

        [PageFX.Event("nativeDragDrop")]
        public event flash.events.EventHandler nativeDragDrop
        {
            add { }
            remove { }
        }

        [PageFX.Event("nativeDragOver")]
        public event flash.events.EventHandler nativeDragOver
        {
            add { }
            remove { }
        }

        [PageFX.Event("nativeDragEnter")]
        public event flash.events.EventHandler nativeDragEnter
        {
            add { }
            remove { }
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

        [PageFX.Event("rightMouseUp")]
        public event flash.events.MouseEventHandler rightMouseUp
        {
            add { }
            remove { }
        }

        [PageFX.Event("rightMouseDown")]
        public event flash.events.MouseEventHandler rightMouseDown
        {
            add { }
            remove { }
        }

        [PageFX.Event("rightClick")]
        public event flash.events.MouseEventHandler rightClick
        {
            add { }
            remove { }
        }

        [PageFX.Event("middleMouseUp")]
        public event flash.events.MouseEventHandler middleMouseUp
        {
            add { }
            remove { }
        }

        [PageFX.Event("middleMouseDown")]
        public event flash.events.MouseEventHandler middleMouseDown
        {
            add { }
            remove { }
        }

        [PageFX.Event("middleClick")]
        public event flash.events.MouseEventHandler middleClick
        {
            add { }
            remove { }
        }

        [PageFX.Event("gestureSwipe")]
        public event flash.events.TransformGestureEventHandler gestureSwipe
        {
            add { }
            remove { }
        }

        [PageFX.Event("gestureZoom")]
        public event flash.events.TransformGestureEventHandler gestureZoom
        {
            add { }
            remove { }
        }

        [PageFX.Event("gestureRotate")]
        public event flash.events.TransformGestureEventHandler gestureRotate
        {
            add { }
            remove { }
        }

        [PageFX.Event("gesturePressAndTap")]
        public event flash.events.PressAndTapGestureEventHandler gesturePressAndTap
        {
            add { }
            remove { }
        }

        [PageFX.Event("gesturePan")]
        public event flash.events.TransformGestureEventHandler gesturePan
        {
            add { }
            remove { }
        }

        [PageFX.Event("gestureTwoFingerTap")]
        public event flash.events.GestureEventHandler gestureTwoFingerTap
        {
            add { }
            remove { }
        }

        [PageFX.Event("touchTap")]
        public event flash.events.TouchEventHandler touchTap
        {
            add { }
            remove { }
        }

        [PageFX.Event("touchRollOver")]
        public event flash.events.TouchEventHandler touchRollOver
        {
            add { }
            remove { }
        }

        [PageFX.Event("touchRollOut")]
        public event flash.events.TouchEventHandler touchRollOut
        {
            add { }
            remove { }
        }

        [PageFX.Event("touchOver")]
        public event flash.events.TouchEventHandler touchOver
        {
            add { }
            remove { }
        }

        [PageFX.Event("touchOut")]
        public event flash.events.TouchEventHandler touchOut
        {
            add { }
            remove { }
        }

        [PageFX.Event("touchMove")]
        public event flash.events.TouchEventHandler touchMove
        {
            add { }
            remove { }
        }

        [PageFX.Event("touchEnd")]
        public event flash.events.TouchEventHandler touchEnd
        {
            add { }
            remove { }
        }

        [PageFX.Event("touchBegin")]
        public event flash.events.TouchEventHandler touchBegin
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

        [PageFX.AbcInstanceTrait(17)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool requestSoftKeyboard();


    }
}
