using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// An object dispatches a FocusEvent object when the user changes the focus from one object
    /// in the display list to another. There are four types of focus events:
    /// FocusEvent.FOCUS_INFocusEvent.FOCUS_OUTFocusEvent.KEY_FOCUS_CHANGEFocusEvent.MOUSE_FOCUS_CHANGE
    /// </summary>
    [PageFX.AbcInstance(254)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class FocusEvent : flash.events.Event
    {
        /// <summary>
        /// Defines the value of the type property of a focusIn event object.
        /// This event has the following properties:PropertyValuebubblestruecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.keyCode0; applies only to keyFocusChange events.relatedObjectThe complementary InteractiveObject instance that is affected by the change in focus.shiftKeyfalse; applies only to keyFocusChange events.targetThe InteractiveObject instance that has just received focus.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String FOCUS_IN;

        /// <summary>
        /// Defines the value of the type property of a focusOut event object.
        /// This event has the following properties:PropertyValuebubblestruecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.keyCode0; applies only to keyFocusChange events.relatedObjectThe complementary InteractiveObject instance that is affected by the change in focus.shiftKeyfalse; applies only to keyFocusChange events.targetThe InteractiveObject instance that has just lost focus.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
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
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String KEY_FOCUS_CHANGE;

        /// <summary>
        /// Defines the value of the type property of a mouseFocusChange event object.
        /// This event has the following properties:PropertyValuebubblestruecancelabletrue; call the preventDefault() method
        /// to cancel default behavior.currentTargetThe object that is actively processing the Event
        /// object with an event listener.keyCode0; applies only to keyFocusChange events.relatedObjectThe complementary InteractiveObject instance that is affected by the change in focus.shiftKeyfalse; applies only to keyFocusChange events.targetThe InteractiveObject instance that currently has focus.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String MOUSE_FOCUS_CHANGE;

        /// <summary>
        /// A reference to the complementary InteractiveObject instance that is affected by the
        /// change in focus. For example, when a focusOut event occurs, the
        /// relatedObject represents the InteractiveObject instance that has gained focus.
        /// </summary>
        public extern virtual flash.display.InteractiveObject relatedObject
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
        /// Indicates whether the Shift key modifier is activated, in which case the value is
        /// true. Otherwise, the value is false. This property is
        /// used only if the FocusEvent is of type keyFocusChange.
        /// </summary>
        public extern virtual bool shiftKey
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

        /// <summary>The key code value of the key pressed to trigger a keyFocusChange event.</summary>
        public extern virtual uint keyCode
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

        public extern virtual bool isRelatedObjectInaccessible
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

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FocusEvent(Avm.String type, bool bubbles, bool cancelable, flash.display.InteractiveObject relatedObject, bool shiftKey, uint keyCode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FocusEvent(Avm.String type, bool bubbles, bool cancelable, flash.display.InteractiveObject relatedObject, bool shiftKey);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FocusEvent(Avm.String type, bool bubbles, bool cancelable, flash.display.InteractiveObject relatedObject);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FocusEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FocusEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FocusEvent(Avm.String type);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.events.Event clone();

        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();
    }
}
