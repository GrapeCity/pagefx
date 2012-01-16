using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// The Event class is used as the base class for the creation of Event objects,
    /// which are passed as parameters to event listeners when an event occurs.
    /// </summary>
    [PageFX.AbcInstance(71)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class Event : Avm.Object
    {
        /// <summary>
        /// Defines the value of the type property of an activate event object.
        /// Note: This event does not go through a &quot;capture phase&quot; and is dispatched directly to the target, whether the target is on the display list or not.This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetAny DisplayObject instance with a listener registered for the ACTIVATE event.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ACTIVATE;

        /// <summary>
        /// Defines the value of the type property of an added event object.
        /// This event has the following properties:PropertyValuebubblestruecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe DisplayObject instance being added to the display list.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ADDED;

        /// <summary>
        /// Defines the value of the type property of an addedToStage event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe DisplayObject instance being added to the on stage display list,
        /// either directly or through the addition of a sub tree in which the DisplayObject instance is contained.
        /// If the DisplayObject instance is being directly added, the added event occurs before this event.
        /// </summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ADDED_TO_STAGE;

        /// <summary>
        /// Defines the value of the type property of a cancel event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetA reference to the object on which the operation is canceled.
        /// </summary>
        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String CANCEL;

        /// <summary>
        /// Defines the value of the type property of a change event object.
        /// This event has the following properties:PropertyValuebubblestruecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe object that has had its value modified.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String CHANGE;

        [PageFX.AbcClassTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String CLEAR;

        /// <summary>
        /// Defines the value of the type property of a close event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe object whose connection has been closed.
        /// </summary>
        [PageFX.AbcClassTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String CLOSE;

        /// <summary>
        /// Defines the value of the type property of a complete event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe network object that has completed loading.
        /// </summary>
        [PageFX.AbcClassTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String COMPLETE;

        /// <summary>
        /// Defines the value of the type property of a connect event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe Socket or XMLSocket object that has established a network connection.
        /// </summary>
        [PageFX.AbcClassTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String CONNECT;

        [PageFX.AbcClassTrait(9)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String COPY;

        [PageFX.AbcClassTrait(10)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String CUT;

        /// <summary>
        /// Defines the value of the type property of a deactivate event object.
        /// Note: This event does not go through a &quot;capture phase&quot; and is dispatched directly to the target, whether the target is on the display list or not.This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetAny DisplayObject instance with a listener registered for the DEACTIVATE event.
        /// </summary>
        [PageFX.AbcClassTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String DEACTIVATE;

        /// <summary>
        /// Defines the value of the type property of an enterFrame event object.
        /// Note: This event does not go through a &quot;capture phase&quot; and is dispatched directly to the target, whether the target is on the display list or not.This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetAny DisplayObject instance with a listener registered for the ENTER_FRAME event.
        /// </summary>
        [PageFX.AbcClassTrait(12)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ENTER_FRAME;

        [PageFX.AbcClassTrait(13)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String FRAME_CONSTRUCTED;

        [PageFX.AbcClassTrait(14)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String EXIT_FRAME;

        /// <summary>
        /// Defines the value of the type property of an id3 event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe Sound object loading the MP3 for which ID3 data is now available.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.AbcClassTrait(15)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String ID3;

        /// <summary>
        /// Defines the value of the type property of an init event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe LoaderInfo object associated with the SWF file being loaded.
        /// </summary>
        [PageFX.AbcClassTrait(16)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String INIT;

        /// <summary>
        /// Defines the value of the type property of a mouseLeave event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe Stage object.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.AbcClassTrait(17)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String MOUSE_LEAVE;

        /// <summary>
        /// Defines the value of the type property of an open event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe network object that has opened a connection.
        /// </summary>
        [PageFX.AbcClassTrait(18)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String OPEN;

        [PageFX.AbcClassTrait(19)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String PASTE;

        /// <summary>
        /// Defines the value of the type property of a removed event object.
        /// This event has the following properties:PropertyValuebubblestruecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe DisplayObject instance to be removed from the display list.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.AbcClassTrait(20)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String REMOVED;

        /// <summary>
        /// Defines the value of the type property of a removedFromStage event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe DisplayObject instance being removed from the on stage display list,
        /// either directly or through the removal of a sub tree in which the DisplayObject instance is contained.
        /// If the DisplayObject instance is being directly removed, the removed event occurs before this event.
        /// </summary>
        [PageFX.AbcClassTrait(21)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String REMOVED_FROM_STAGE;

        /// <summary>
        /// Defines the value of the type property of a render event object.
        /// Note: This event does not go through a &quot;capture phase&quot; and is dispatched directly to the target, whether the target is on the display list or not.This event has the following properties:PropertyValuebubblesfalsecancelablefalse; the default behavior cannot be canceled.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetAny DisplayObject instance with a listener registered for the RENDER event.
        /// </summary>
        [PageFX.AbcClassTrait(22)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String RENDER;

        /// <summary>
        /// Defines the value of the type property of a resize event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe Stage object.
        /// </summary>
        [PageFX.AbcClassTrait(23)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String RESIZE;

        /// <summary>
        /// Defines the value of the type property of a scroll event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe TextField object that has been scrolled.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.AbcClassTrait(24)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String SCROLL;

        [PageFX.AbcClassTrait(25)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String TEXT_INTERACTION_MODE_CHANGE;

        /// <summary>
        /// Defines the value of the type property of a select event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe FileReference object on which an item has been selected.
        /// </summary>
        [PageFX.AbcClassTrait(26)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String SELECT;

        [PageFX.AbcClassTrait(27)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String SELECT_ALL;

        /// <summary>
        /// Defines the value of the type property of a soundComplete event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe Sound object on which a sound has finished playing.
        /// </summary>
        [PageFX.AbcClassTrait(28)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String SOUND_COMPLETE;

        /// <summary>
        /// Defines the value of the type property of a tabChildrenChange event object.
        /// This event has the following properties:PropertyValuebubblestruecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe object whose tabChildren flag has changed.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.AbcClassTrait(29)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String TAB_CHILDREN_CHANGE;

        /// <summary>
        /// Defines the value of the type property of a tabEnabledChange event object.
        /// This event has the following properties:PropertyValuebubblestruecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe InteractiveObject whose tabEnabled flag has changed.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.AbcClassTrait(30)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String TAB_ENABLED_CHANGE;

        /// <summary>
        /// Defines the value of the type property of a tabIndexChange event object.
        /// This event has the following properties:PropertyValuebubblestruecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe object whose tabIndex has changed.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.AbcClassTrait(31)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String TAB_INDEX_CHANGE;

        /// <summary>
        /// Defines the value of the type property of an unload event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe LoaderInfo object associated with the SWF file being unloaded or replaced.
        /// </summary>
        [PageFX.AbcClassTrait(32)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String UNLOAD;

        /// <summary>
        /// Defines the value of the type property of a fullScreen event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe Stage object.
        /// </summary>
        [PageFX.AbcClassTrait(33)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String FULLSCREEN;

        /// <summary>The type of event. The type is case-sensitive.</summary>
        public extern virtual Avm.String type
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Indicates whether an event is a bubbling event. If the event can bubble,
        /// this value is true; otherwise it is false.
        /// Event bubbling is defined for display objects in SWF content,
        /// which uses the ActionScript 3.0 display object model.When an event occurs, it moves through the three phases of the event flow: the capture
        /// phase, which flows from the top of the display list hierarchy to the node just before the
        /// target node; the target phase, which comprises the target node; and the bubbling phase,
        /// which flows from the node subsequent to the target node back up the display list hierarchy.Some events, such as the activate and unload events, do not
        /// have a bubbling phase. The bubbles property has a value of
        /// false for events that do not have a bubbling phase.
        /// </summary>
        public extern virtual bool bubbles
        {
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Indicates whether the behavior associated with the event can be prevented.
        /// If the behavior can be canceled, this value is true; otherwise it is false.
        /// </summary>
        public extern virtual bool cancelable
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>The event target. This property contains the target node. For example, if a user clicks an OK button, the target node is the display list node containing that button.</summary>
        public extern virtual object target
        {
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>The object that is actively processing the Event object with an event listener. For example, if a user clicks an OK button, the current target could be the node containing that button or one of its ancestors that has registered an event listener for that event.</summary>
        public extern virtual object currentTarget
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The current phase in the event flow. This property can contain the following numeric values:
        /// 1 The capture phase (EventPhase.CAPTURING_PHASE). 2 The target phase (EventPhase.AT_TARGET). 3 The bubbling phase (EventPhase.BUBBLING_PHASE).The event flow is defined for display objects in SWF content, which use
        /// the ActionScript 3.0 display object model.
        /// </summary>
        public extern virtual uint eventPhase
        {
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Event(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Event(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Event(Avm.String type);

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
        /// <param name="className">
        /// The name of your custom Event class. In the previous example,
        /// the className parameter is PingEvent.
        /// </param>
        /// <returns>
        /// The name of your custom Event class and the String value of your ...arguments
        /// parameter.
        /// </returns>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String formatToString(Avm.String className);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String formatToString(Avm.String className, object rest0);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String formatToString(Avm.String className, object rest0, object rest1);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String formatToString(Avm.String className, object rest0, object rest1, object rest2);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String formatToString(Avm.String className, object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String formatToString(Avm.String className, object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String formatToString(Avm.String className, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String formatToString(Avm.String className, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String formatToString(Avm.String className, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String formatToString(Avm.String className, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String formatToString(Avm.String className, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

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
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.events.Event clone();

        /// <summary>
        /// Returns a string containing all the properties of the Event object. The string is in the following format:
        /// [Event type=value bubbles=value cancelable=value]
        /// </summary>
        /// <returns>A string containing all the properties of the Event object.</returns>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();

        /// <summary>
        /// Prevents processing of any event listeners in nodes subsequent to the current node in the
        /// event flow. This method does not affect any event listeners in the current node
        /// (currentTarget). In contrast, the stopImmediatePropagation() method
        /// prevents processing of event listeners in both the current node and subsequent nodes.
        /// Additional calls to this method have no effect. This method can be called in any phase
        /// of the event flow.Note:  This method does not cancel the behavior associated with this event; see preventDefault() for that functionality.The event flow is defined for display objects in SWF content, which use
        /// the ActionScript 3.0 display object model.
        /// </summary>
        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void stopPropagation();

        /// <summary>
        /// Prevents processing of any event listeners in the current node and any subsequent nodes in
        /// the event flow. This method takes effect immediately, and it affects event listeners
        /// in the current node. In contrast, the stopPropagation() method doesn&apos;t take
        /// effect until all the event listeners in the current node finish processing.Note:  This method does not cancel the behavior associated with this event; see preventDefault() for that functionality.The event flow is defined for display objects in SWF content, which use
        /// the ActionScript 3.0 display object model.
        /// </summary>
        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void stopImmediatePropagation();

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
        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void preventDefault();

        /// <summary>
        /// Checks whether the preventDefault() method has been called on the event. If the
        /// preventDefault() method has been called,
        /// returns true; otherwise, returns false.
        /// </summary>
        /// <returns>
        /// If preventDefault() has been called, returns true; otherwise,
        /// returns false.
        /// </returns>
        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool isDefaultPrevented();
    }
}
