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
    [PageFX.AbcInstance(204)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class IMEEvent : flash.events.TextEvent
    {
        /// <summary>
        /// Defines the value of the type property of an imeComposition event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe IME object.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String IME_COMPOSITION;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String IME_START_COMPOSITION;

        public extern virtual flash.text.ime.IIMEClient imeClient
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IMEEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String text, flash.text.ime.IIMEClient imeClient);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IMEEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String text);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IMEEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IMEEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IMEEvent(Avm.String type);

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.events.Event clone();

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();
    }
}
