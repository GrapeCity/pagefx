using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// The Event class is used as the base class for the creation of Event objects,
    /// which are passed as parameters to event listeners when an event occurs.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class Event : Avm.Object
    {
        /// <summary>
        /// Defines the value of the type property of a cancel event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetA reference to the object on which the operation is canceled.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String CANCEL;

        /// <summary>
        /// Defines the value of the type property of an enterFrame event object.
        /// Note: This event does not go through a &quot;capture phase&quot; and is dispatched directly to the target, whether the target is on the display list or not.This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetAny DisplayObject instance with a listener registered for the ENTER_FRAME event.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ENTER_FRAME;

        /// <summary>
        /// Defines the value of the type property of a soundComplete event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe Sound object on which a sound has finished playing.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String SOUND_COMPLETE;

        /// <summary>
        /// Defines the value of the type property of an unload event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe LoaderInfo object associated with the SWF file being unloaded or replaced.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String UNLOAD;

        /// <summary>
        /// Defines the value of the type property of an init event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe LoaderInfo object associated with the SWF file being loaded.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String INIT;

        /// <summary>
        /// Defines the value of the type property of a render event object.
        /// Note: This event does not go through a &quot;capture phase&quot; and is dispatched directly to the target, whether the target is on the display list or not.This event has the following properties:PropertyValuebubblesfalsecancelablefalse; the default behavior cannot be canceled.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetAny DisplayObject instance with a listener registered for the RENDER event.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String RENDER;

        /// <summary>
        /// Defines the value of the type property of a tabEnabledChange event object.
        /// This event has the following properties:PropertyValuebubblestruecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe InteractiveObject whose tabEnabled flag has changed.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String TAB_ENABLED_CHANGE;

        /// <summary>
        /// Defines the value of the type property of an addedToStage event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe DisplayObject instance being added to the on stage display list,
        /// either directly or through the addition of a sub tree in which the DisplayObject instance is contained.
        /// If the DisplayObject instance is being directly added, the added event occurs before this event.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ADDED_TO_STAGE;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String SAMPLE_DATA;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String FRAME_CONSTRUCTED;

        /// <summary>
        /// Defines the value of the type property of a tabChildrenChange event object.
        /// This event has the following properties:PropertyValuebubblestruecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe object whose tabChildren flag has changed.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String TAB_CHILDREN_CHANGE;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String CUT;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String CLEAR;

        /// <summary>
        /// Defines the value of the type property of a change event object.
        /// This event has the following properties:PropertyValuebubblestruecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe object that has had its value modified.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String CHANGE;

        /// <summary>
        /// Defines the value of the type property of a resize event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe Stage object.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String RESIZE;

        /// <summary>
        /// Defines the value of the type property of a complete event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe network object that has completed loading.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String COMPLETE;

        /// <summary>
        /// Defines the value of the type property of a fullScreen event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe Stage object.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String FULLSCREEN;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String SELECT_ALL;

        /// <summary>
        /// Defines the value of the type property of a removed event object.
        /// This event has the following properties:PropertyValuebubblestruecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe DisplayObject instance to be removed from the display list.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String REMOVED;

        /// <summary>
        /// Defines the value of the type property of a connect event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe Socket or XMLSocket object that has established a network connection.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String CONNECT;

        /// <summary>
        /// Defines the value of the type property of a scroll event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe TextField object that has been scrolled.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String SCROLL;

        /// <summary>
        /// Defines the value of the type property of an open event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe network object that has opened a connection.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String OPEN;

        /// <summary>
        /// Defines the value of the type property of a close event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe object whose connection has been closed.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String CLOSE;

        /// <summary>
        /// Defines the value of the type property of a mouseLeave event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe Stage object.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String MOUSE_LEAVE;

        /// <summary>
        /// Defines the value of the type property of an added event object.
        /// This event has the following properties:PropertyValuebubblestruecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe DisplayObject instance being added to the display list.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ADDED;

        /// <summary>
        /// Defines the value of the type property of a removedFromStage event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe DisplayObject instance being removed from the on stage display list,
        /// either directly or through the removal of a sub tree in which the DisplayObject instance is contained.
        /// If the DisplayObject instance is being directly removed, the removed event occurs before this event.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String REMOVED_FROM_STAGE;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String EXIT_FRAME;

        /// <summary>
        /// Defines the value of the type property of a tabIndexChange event object.
        /// This event has the following properties:PropertyValuebubblestruecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe object whose tabIndex has changed.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String TAB_INDEX_CHANGE;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String PASTE;

        /// <summary>
        /// Defines the value of the type property of a deactivate event object.
        /// Note: This event does not go through a &quot;capture phase&quot; and is dispatched directly to the target, whether the target is on the display list or not.This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetAny DisplayObject instance with a listener registered for the DEACTIVATE event.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String DEACTIVATE;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String COPY;

        /// <summary>
        /// Defines the value of the type property of an id3 event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe Sound object loading the MP3 for which ID3 data is now available.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ID3;

        /// <summary>
        /// Defines the value of the type property of an activate event object.
        /// Note: This event does not go through a &quot;capture phase&quot; and is dispatched directly to the target, whether the target is on the display list or not.This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetAny DisplayObject instance with a listener registered for the ACTIVATE event.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ACTIVATE;

        /// <summary>
        /// Defines the value of the type property of a select event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe FileReference object on which an item has been selected.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String SELECT;

        public extern virtual uint eventPhase
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool bubbles
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual object target
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool cancelable
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual object currentTarget
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String type
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Event(Avm.String arg0, bool arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Event(Avm.String arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Event(Avm.String arg0);

        /// <summary>
        /// Checks whether the preventDefault() method has been called on the event. If the
        /// preventDefault() method has been called,
        /// returns true; otherwise, returns false.
        /// </summary>
        /// <returns>
        /// If preventDefault() has been called, returns true; otherwise,
        /// returns false.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool isDefaultPrevented();

        /// <summary>
        /// A utility function for implementing the
        /// toString() method in your custom Event class.
        /// Overriding the toString() method is recommended,
        /// but not required.
        /// class PingEvent extends Event {
        /// var URL:String;
        /// public override function toString():String {
        /// return formatToString(&quot;PingEvent&quot;, &quot;type&quot;, &quot;bubbles&quot;, &quot;cancelable&quot;, &quot;eventPhase&quot;, &quot;URL&quot;);
        /// }
        /// }
        /// This method is used in creating ActionScript 3.0 classes
        /// to be used in SWF content.
        /// </summary>
        /// <param name="arg0">
        /// The name of your custom Event class. In the previous example,
        /// the className parameter is PingEvent.
        /// </param>
        /// <returns>
        /// The name of your custom Event class and the String value of your ...arguments
        /// parameter.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String formatToString(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String formatToString(Avm.String arg0, object rest0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String formatToString(Avm.String arg0, object rest0, object rest1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String formatToString(Avm.String arg0, object rest0, object rest1, object rest2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String formatToString(Avm.String arg0, object rest0, object rest1, object rest2, object rest3);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String formatToString(Avm.String arg0, object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String formatToString(Avm.String arg0, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String formatToString(Avm.String arg0, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String formatToString(Avm.String arg0, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String formatToString(Avm.String arg0, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String formatToString(Avm.String arg0, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        /// <summary>
        /// Duplicates an instance of an Event subclass.
        /// Returns a new Event object that is a copy of the original instance of the Event object.
        /// You do not normally call clone(); the EventDispatcher class calls it automatically
        /// when you redispatch an event—that is, when you call dispatchEvent(event) from a handler
        /// that is handling event.The new Event object includes all the properties of the original.When creating your own custom Event class, you must override the
        /// inherited Event.clone() method in order for it to duplicate the
        /// properties of your custom class. If you do not set all the properties that you add
        /// in your event subclass, those properties will not have the correct values when listeners
        /// handle the redispatched event.In this example, PingEvent is a subclass of Event
        /// and therefore implements its own version of clone().
        /// class PingEvent extends Event {
        /// var URL:String;
        /// public override function clone():Event {
        /// return new PingEvent(type, bubbles, cancelable, URL);
        /// }
        /// }
        /// </summary>
        /// <returns>A new Event object that is identical to the original.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Event clone();

        /// <summary>
        /// Cancels an event&apos;s default behavior if that behavior can be canceled.
        /// Many events have associated behaviors that are carried out by default.
        /// For example, if a user types a character
        /// into a text field, the default behavior is that the character is
        /// displayed in the text field. Because the TextEvent.TEXT_INPUT
        /// event&apos;s default behavior can be canceled, you can use the preventDefault()
        /// method to prevent the character from appearing.For example, if a user clicks the close box of a window,
        /// the default behavior is that the window closes. Because the closing
        /// event&apos;s default behavior can be canceled, you can use the preventDefault()
        /// method to prevent the window from closing.An example of a behavior that is not cancelable is the default behavior associated with
        /// the Event.REMOVED event, which is generated whenever Flash Player is about to
        /// remove a display object from the display list. The default behavior (removing the element)
        /// cannot be canceled, so the preventDefault() method has no effect on this
        /// default behavior. You can use the Event.cancelable property to check whether you can prevent
        /// the default behavior associated with a particular event. If the value of
        /// Event.cancelable is true, then preventDefault() can
        /// be used to cancel the event; otherwise, preventDefault() has no effect.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void preventDefault();

        /// <summary>
        /// Prevents processing of any event listeners in nodes subsequent to the current node in the
        /// event flow. This method does not affect any event listeners in the current node
        /// (currentTarget). In contrast, the stopImmediatePropagation() method
        /// prevents processing of event listeners in both the current node and subsequent nodes.
        /// Additional calls to this method have no effect. This method can be called in any phase
        /// of the event flow.Note:  This method does not cancel the behavior associated with this event; see preventDefault() for that functionality.The event flow is defined for display objects in SWF content, which use
        /// the ActionScript 3.0 display object model.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void stopPropagation();

        /// <summary>
        /// Returns a string containing all the properties of the Event object. The string is in the following format:
        /// [Event type=value bubbles=value cancelable=value]
        /// </summary>
        /// <returns>A string containing all the properties of the Event object.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();

        /// <summary>
        /// Prevents processing of any event listeners in the current node and any subsequent nodes in
        /// the event flow. This method takes effect immediately, and it affects event listeners
        /// in the current node. In contrast, the stopPropagation() method doesn&apos;t take
        /// effect until all the event listeners in the current node finish processing.Note:  This method does not cancel the behavior associated with this event; see preventDefault() for that functionality.The event flow is defined for display objects in SWF content, which use
        /// the ActionScript 3.0 display object model.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void stopImmediatePropagation();
    }
}
