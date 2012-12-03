using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// The IEventDispatcher interface defines methods for adding or removing event listeners,
    /// checks whether specific types of event listeners are registered, and dispatches
    /// events.
    /// </summary>
    [PageFX.AbcInstance(23)]
    [PageFX.ABC]
    [PageFX.FP9]
    public interface IEventDispatcher
    {
        /// <summary>
        /// Registers an event listener object with an EventDispatcher object so that the listener
        /// receives notification of an event. You can register event listeners on all nodes
        /// in the display list for a specific type of event, phase, and priority.
        /// After you successfully register an event listener, you cannot change its priority
        /// through additional calls to addEventListener(). To change a listener&apos;s
        /// priority, you must first call removeListener(). Then you can register
        /// the listener again with the new priority level.
        /// After the listener is registered, subsequent calls to addEventListener()
        /// with a different value for either type or useCapture result
        /// in the creation of a separate listener registration. For example, if you first register
        /// a listener with useCapture set to true, it listens only
        /// during the capture phase. If you call addEventListener() again using
        /// the same listener object, but with useCapture set to false,
        /// you have two separate listeners: one that listens during the capture phase, and
        /// another that listens during the target and bubbling phases.
        /// You cannot register an event listener for only the target phase or the bubbling
        /// phase. Those phases are coupled during registration because bubbling applies only
        /// to the ancestors of the target node.
        /// When you no longer need an event listener, remove it by calling EventDispatcher.removeEventListener();
        /// otherwise, memory problems might result. Objects with registered event listeners
        /// are not automatically removed from memory because the garbage collector does not
        /// remove objects that still have references.
        /// Copying an EventDispatcher instance does not copy the event listeners attached to
        /// it. (If your newly created node needs an event listener, you must attach the listener
        /// after creating the node.) However, if you move an EventDispatcher instance, the
        /// event listeners attached to it move along with it.
        /// If the event listener is being registered on a node while an event is also being
        /// processed on this node, the event listener is not triggered during the current phase
        /// but may be triggered during a later phase in the event flow, such as the bubbling
        /// phase.
        /// If an event listener is removed from a node while an event is being processed on
        /// the node, it is still triggered by the current actions. After it is removed, the
        /// event listener is never invoked again (unless it is registered again for future
        /// processing).
        /// </summary>
        /// <param name="type">The type of event.</param>
        /// <param name="listener">
        /// The listener function that processes the event. This function must accept
        /// an Event object as its only parameter and must return nothing, as this example shows:
        /// function(evt:Event):void
        /// The function can have any name.
        /// </param>
        /// <param name="useCapture">
        /// (default = false)  Determines whether the listener
        /// works in the capture phase or the target and bubbling phases. If useCapture
        /// is set to true, the listener processes the event only during the capture
        /// phase and not in the target or bubbling phase. If useCapture is false,
        /// the listener processes the event only during the target or bubbling phase. To listen
        /// for the event in all three phases, call addEventListener() twice, once
        /// with useCapture set to true, then again with useCapture
        /// set to false.
        /// </param>
        /// <param name="priority">
        /// (default = 0)  The priority level of the event
        /// listener. Priorities are designated by a 32-bit integer. The higher the number,
        /// the higher the priority. All listeners with priority n are processed before
        /// listeners of priority n-1. If two or more listeners share the same priority,
        /// they are processed in the order in which they were added. The default priority is
        /// 0.
        /// </param>
        /// <param name="useWeakReference">
        /// (default = false)  Determines whether the reference
        /// to the listener is strong or weak. A strong reference (the default) prevents your
        /// listener from being garbage-collected. A weak reference does not.
        /// </param>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.QName("addEventListener", "flash.events:IEventDispatcher", "public")]
        [PageFX.FP9]
        void addEventListener(Avm.String type, Avm.Function listener, bool useCapture, int priority, bool useWeakReference);

        /// <summary>
        /// Removes a listener from the EventDispatcher object. If there is no matching listener
        /// registered with the EventDispatcher object, a call to this method has no effect.
        /// </summary>
        /// <param name="type">The type of event.</param>
        /// <param name="listener">The listener object to remove.</param>
        /// <param name="useCapture">
        /// (default = false)  Specifies whether the listener
        /// was registered for the capture phase or the target and bubbling phases. If the listener
        /// was registered for both the capture phase and the target and bubbling phases, two
        /// calls to removeEventListener() are required to remove both: one call
        /// with useCapture set to true, and another call with useCapture
        /// set to false.
        /// </param>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.QName("removeEventListener", "flash.events:IEventDispatcher", "public")]
        [PageFX.FP9]
        void removeEventListener(Avm.String type, Avm.Function listener, bool useCapture);

        /// <summary>
        /// Dispatches an event into the event flow. The event target is the EventDispatcher
        /// object upon which dispatchEvent() is called.
        /// </summary>
        /// <param name="@event">The Event object dispatched into the event flow.</param>
        /// <returns>
        /// A value of true
        /// unless preventDefault() is called on the event, in which case it returns
        /// false.
        /// </returns>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("dispatchEvent", "flash.events:IEventDispatcher", "public")]
        [PageFX.FP9]
        bool dispatchEvent(flash.events.Event @event);

        /// <summary>
        /// Checks whether the EventDispatcher object has any listeners registered for a specific
        /// type of event. This allows you to determine where an EventDispatcher object has
        /// altered handling of an event type in the event flow hierarchy. To determine whether
        /// a specific event type will actually trigger an event listener, use IEventDispatcher.willTrigger().
        /// The difference between hasEventListener() and willTrigger()
        /// is that hasEventListener() examines only the object to which it belongs,
        /// whereas willTrigger() examines the entire event flow for the event
        /// specified by the type parameter.
        /// </summary>
        /// <param name="type">The type of event.</param>
        /// <returns>
        /// A value of true
        /// if a listener of the specified type is registered; false otherwise.
        /// </returns>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.QName("hasEventListener", "flash.events:IEventDispatcher", "public")]
        [PageFX.FP9]
        bool hasEventListener(Avm.String type);

        /// <summary>
        /// Checks whether an event listener is registered with this EventDispatcher object
        /// or any of its ancestors for the specified event type. This method returns true
        /// if an event listener is triggered during any phase of the event flow when an event
        /// of the specified type is dispatched to this EventDispatcher object or any of its
        /// descendants.
        /// The difference between hasEventListener() and willTrigger()
        /// is that hasEventListener() examines only the object to which it belongs,
        /// whereas willTrigger() examines the entire event flow for the event
        /// specified by the type parameter.
        /// </summary>
        /// <param name="type">The type of event.</param>
        /// <returns>
        /// A value of true
        /// if a listener of the specified type will be triggered; false otherwise.
        /// </returns>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("willTrigger", "flash.events:IEventDispatcher", "public")]
        [PageFX.FP9]
        bool willTrigger(Avm.String type);
    }
}
