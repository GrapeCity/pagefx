using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// An object dispatches a TextEvent object when a user enters text in a text field or clicks
    /// a hyperlink in an HTML-enabled text field. There are two types of text events: TextEvent.LINK
    /// and TextEvent.TEXT_INPUT .
    /// </summary>
    [PageFX.AbcInstance(127)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class TextEvent : flash.events.Event
    {
        /// <summary>
        /// Defines the value of the type property of a link event object.
        /// This event has the following properties:PropertyValuebubblestruecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe text field containing the hyperlink that has been clicked.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.textThe remainder of the URL after &quot;event:&quot;
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String LINK;

        /// <summary>
        /// Defines the value of the type property of a textInput event object.
        /// This event has the following properties:PropertyValuebubblestruecancelabletrue; call the preventDefault() method
        /// to cancel default behavior.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe text field into which characters are being entered.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.textThe character or sequence of characters entered by the user.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String TEXT_INPUT;

        /// <summary>
        /// For a textInput event, the character or sequence of characters
        /// entered by the user. For a link event, the text
        /// of the event attribute of the href attribute of the
        /// &lt;a&gt; tag.
        /// </summary>
        public extern virtual Avm.String text
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

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String text);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextEvent(Avm.String type);

        [PageFX.AbcInstanceTrait(3)]
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
