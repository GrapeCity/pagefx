using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// An object dispatches a KeyboardEvent object in response to user input through a keyboard.
    /// There are two types of keyboard events: KeyboardEvent.KEY_DOWN  and
    /// KeyboardEvent.KEY_UP
    /// </summary>
    [PageFX.AbcInstance(72)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class KeyboardEvent : flash.events.Event
    {
        /// <summary>
        /// Defines the value of the type property of a keyDown event object.
        /// This event has the following properties:PropertyValuebubblestruecancelablefalse; there is no default behavior to cancel.charCodeThe character code value of the key pressed or released.ctrlKeytrue if the Control key is active; false if it is inactive.currentTargetThe object that is actively processing the Event
        /// object with an event listener.keyCodeThe key code value of the key pressed or released.keyLocationThe location of the key on the keyboard.shiftKeytrue if the Shift key is active; false if it is inactive.targetThe InteractiveObject instance with focus.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String KEY_DOWN;

        /// <summary>
        /// Defines the value of the type property of a keyUp event object.
        /// This event has the following properties:PropertyValuebubblestruecancelablefalse; there is no default behavior to cancel.charCodeContains the character code value of the key pressed or released.ctrlKeytrue if the Control key is active; false if it is inactive.currentTargetThe object that is actively processing the Event
        /// object with an event listener.keyCodeThe key code value of the key pressed or released.keyLocationThe location of the key on the keyboard.shiftKeytrue if the Shift key is active; false if it is inactive.targetThe InteractiveObject instance with focus.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String KEY_UP;

        /// <summary>
        /// Contains the character code value of the key pressed or released.
        /// The character code values are English keyboard values. For example,
        /// if you press Shift+3, charCode is # on a Japanese keyboard,
        /// just as it is on an English keyboard.
        /// Note: When an input method editor (IME) is running,
        /// charCode does not report accurate character codes.
        /// </summary>
        public extern virtual uint charCode
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

        /// <summary>
        /// The key code value of the key pressed or released.
        /// Note: When an input method editor (IME) is running,
        /// keyCode does not report accurate key codes.
        /// </summary>
        public extern virtual uint keyCode
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

        /// <summary>
        /// Indicates the location of the key on the keyboard. This is useful for differentiating keys
        /// that appear more than once on a keyboard. For example, you can differentiate between the
        /// left and right Shift keys by the value of this property: KeyLocation.LEFT
        /// for the left and KeyLocation.RIGHT for the right. Another example is
        /// differentiating between number keys pressed on the standard keyboard
        /// (KeyLocation.STANDARD) versus the numeric keypad (KeyLocation.NUM_PAD).
        /// </summary>
        public extern virtual uint keyLocation
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates whether the Control key is active (true) or inactive
        /// (false).
        /// Note: The Command key modifier on Macintosh systems must be represented
        /// using this key modifier.
        /// </summary>
        public extern virtual bool ctrlKey
        {
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>Reserved for future use. This property is not currently supported.</summary>
        public extern virtual bool altKey
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
        /// Indicates whether the Shift key modifier is active (true) or inactive
        /// (false).
        /// </summary>
        public extern virtual bool shiftKey
        {
            [PageFX.AbcInstanceTrait(14)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(15)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern KeyboardEvent(Avm.String type, bool bubbles, bool cancelable, uint charCodeValue, uint keyCodeValue, uint keyLocationValue, bool ctrlKeyValue, bool altKeyValue, bool shiftKeyValue);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern KeyboardEvent(Avm.String type, bool bubbles, bool cancelable, uint charCodeValue, uint keyCodeValue, uint keyLocationValue, bool ctrlKeyValue, bool altKeyValue);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern KeyboardEvent(Avm.String type, bool bubbles, bool cancelable, uint charCodeValue, uint keyCodeValue, uint keyLocationValue, bool ctrlKeyValue);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern KeyboardEvent(Avm.String type, bool bubbles, bool cancelable, uint charCodeValue, uint keyCodeValue, uint keyLocationValue);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern KeyboardEvent(Avm.String type, bool bubbles, bool cancelable, uint charCodeValue, uint keyCodeValue);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern KeyboardEvent(Avm.String type, bool bubbles, bool cancelable, uint charCodeValue);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern KeyboardEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern KeyboardEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern KeyboardEvent(Avm.String type);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.events.Event clone();

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();

        /// <summary>
        /// Instructs Flash Player to render after processing of this event completes, if the display
        /// list has been modified
        /// </summary>
        [PageFX.AbcInstanceTrait(16)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void updateAfterEvent();
    }
}
