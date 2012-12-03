using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// An object dispatches a KeyboardEvent object in response to user input through a keyboard.
    /// There are two types of keyboard events: KeyboardEvent.KEY_DOWN  and
    /// KeyboardEvent.KEY_UP
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class KeyboardEvent : Event
    {
        /// <summary>
        /// Defines the value of the type property of a keyDown event object.
        /// This event has the following properties:PropertyValuebubblestruecancelablefalse; there is no default behavior to cancel.charCodeThe character code value of the key pressed or released.ctrlKeytrue if the Control key is active; false if it is inactive.currentTargetThe object that is actively processing the Event
        /// object with an event listener.keyCodeThe key code value of the key pressed or released.keyLocationThe location of the key on the keyboard.shiftKeytrue if the Shift key is active; false if it is inactive.targetThe InteractiveObject instance with focus.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
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
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String KEY_UP;

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

        public extern virtual uint keyLocation
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

        public extern virtual uint charCode
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

        public extern virtual bool ctrlKey
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

        public extern virtual bool altKey
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
        public extern KeyboardEvent(Avm.String arg0, bool arg1, bool arg2, uint arg3, uint arg4, uint arg5, bool arg6, bool arg7, bool arg8);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern KeyboardEvent(Avm.String arg0, bool arg1, bool arg2, uint arg3, uint arg4, uint arg5, bool arg6, bool arg7);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern KeyboardEvent(Avm.String arg0, bool arg1, bool arg2, uint arg3, uint arg4, uint arg5, bool arg6);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern KeyboardEvent(Avm.String arg0, bool arg1, bool arg2, uint arg3, uint arg4, uint arg5);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern KeyboardEvent(Avm.String arg0, bool arg1, bool arg2, uint arg3, uint arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern KeyboardEvent(Avm.String arg0, bool arg1, bool arg2, uint arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern KeyboardEvent(Avm.String arg0, bool arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern KeyboardEvent(Avm.String arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern KeyboardEvent(Avm.String arg0);

        /// <summary>
        /// Instructs Flash Player to render after processing of this event completes, if the display
        /// list has been modified
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void updateAfterEvent();

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
