using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// An object dispatches a FocusEvent object when the user changes the focus from one object
    /// in the display list to another. There are four types of focus events:
    /// FocusEvent.FOCUS_INFocusEvent.FOCUS_OUTFocusEvent.KEY_FOCUS_CHANGEFocusEvent.MOUSE_FOCUS_CHANGE
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class FocusEvent : Event
    {
        /// <summary>
        /// Defines the value of the type property of a mouseFocusChange event object.
        /// This event has the following properties:PropertyValuebubblestruecancelabletrue; call the preventDefault() method
        /// to cancel default behavior.currentTargetThe object that is actively processing the Event
        /// object with an event listener.keyCode0; applies only to keyFocusChange events.relatedObjectThe complementary InteractiveObject instance that is affected by the change in focus.shiftKeyfalse; applies only to keyFocusChange events.targetThe InteractiveObject instance that currently has focus.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String MOUSE_FOCUS_CHANGE;

        /// <summary>
        /// Defines the value of the type property of a focusOut event object.
        /// This event has the following properties:PropertyValuebubblestruecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.keyCode0; applies only to keyFocusChange events.relatedObjectThe complementary InteractiveObject instance that is affected by the change in focus.shiftKeyfalse; applies only to keyFocusChange events.targetThe InteractiveObject instance that has just lost focus.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String FOCUS_OUT;

        /// <summary>
        /// Defines the value of the type property of a keyFocusChange event object.
        /// This event has the following properties:PropertyValuebubblestruecancelabletrue; call the preventDefault() method
        /// to cancel default behavior.currentTargetThe object that is actively processing
        /// the Event
        /// object with an event listener.keyCodeThe key code value of the key pressed to trigger a keyFocusChange event.relatedObjectThe complementary InteractiveObject instance that is affected by the change in focus.shiftKeytrue if the Shift key modifier is activated; false otherwise.targetThe InteractiveObject instance that currently has focus.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String KEY_FOCUS_CHANGE;

        /// <summary>
        /// Defines the value of the type property of a focusIn event object.
        /// This event has the following properties:PropertyValuebubblestruecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.keyCode0; applies only to keyFocusChange events.relatedObjectThe complementary InteractiveObject instance that is affected by the change in focus.shiftKeyfalse; applies only to keyFocusChange events.targetThe InteractiveObject instance that has just received focus.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String FOCUS_IN;

        public extern virtual bool shiftKey
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

        public extern virtual flash.display.InteractiveObject relatedObject
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

        public extern virtual uint keyCode
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

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FocusEvent(Avm.String arg0, bool arg1, bool arg2, flash.display.InteractiveObject arg3, bool arg4, uint arg5);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FocusEvent(Avm.String arg0, bool arg1, bool arg2, flash.display.InteractiveObject arg3, bool arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FocusEvent(Avm.String arg0, bool arg1, bool arg2, flash.display.InteractiveObject arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FocusEvent(Avm.String arg0, bool arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FocusEvent(Avm.String arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FocusEvent(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Event clone();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();


    }
}
