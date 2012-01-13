using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// An object dispatches an IMEEvent object when the user enters text using an input method editor
    /// (IME). IMEs are generally used to enter text from languages that have ideographs instead of
    /// letters, such as Japanese, Chinese, and Korean. There is only one IME event:
    /// IMEEvent.IME_COMPOSITION .
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class IMEEvent : TextEvent
    {
        /// <summary>
        /// Defines the value of the type property of an imeComposition event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe IME object.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String IME_COMPOSITION;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IMEEvent(Avm.String arg0, bool arg1, bool arg2, Avm.String arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IMEEvent(Avm.String arg0, bool arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IMEEvent(Avm.String arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IMEEvent(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Event clone();
    }
}
