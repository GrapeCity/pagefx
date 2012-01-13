using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// The EventDispatcher class is the base class for all runtime  classes
    /// that dispatch events. The EventDispatcher class implements the IEventDispatcher
    /// interface and is the base class for the DisplayObject class. The EventDispatcher
    /// class allows any object on the display list to be an event target and as such, to
    /// use the methods of the IEventDispatcher interface.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class EventDispatcher : Avm.Object, IEventDispatcher
    {
        [PageFX.Event("deactivate")]
        public event EventHandler deactivate
        {
            add { }
            remove { }
        }

        [PageFX.Event("activate")]
        public event EventHandler activate
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EventDispatcher(IEventDispatcher arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern EventDispatcher();

        /// <summary>
        /// Dispatches an event into the event flow. The event target is the EventDispatcher
        /// object upon which the dispatchEvent() method is called.
        /// </summary>
        /// <param name="arg0">
        /// The Event object that is dispatched into the event flow. If the event is
        /// being redispatched, a clone of the event is created automatically. After an event
        /// is dispatched, its target property cannot be changed, so you must create
        /// a new copy of the event for redispatching to work.
        /// </param>
        /// <returns>
        /// A value of true
        /// if the event was successfully dispatched. A value of false indicates
        /// failure or that preventDefault() was called on the event.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool dispatchEvent(Event arg0);

        /// <summary>
        /// Checks whether an event listener is registered with this EventDispatcher object
        /// or any of its ancestors for the specified event type. This method returns true
        /// if an event listener is triggered during any phase of the event flow when an event
        /// of the specified type is dispatched to this EventDispatcher object or any of its
        /// descendants.
        /// The difference between the hasEventListener() and the willTrigger()
        /// methods is that hasEventListener() examines only the object to which
        /// it belongs, whereas the willTrigger() method examines the entire event
        /// flow for the event specified by the type parameter.
        /// When willTrigger() is called from a LoaderInfo object, only the listeners
        /// that the caller can access are considered.
        /// </summary>
        /// <param name="arg0">The type of event.</param>
        /// <returns>
        /// A value of true
        /// if a listener of the specified type will be triggered; false otherwise.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool willTrigger(Avm.String arg0);

        /// <summary>
        /// Removes a listener from the EventDispatcher object. If there is no matching listener
        /// registered with the EventDispatcher object, a call to this method has no effect.
        /// </summary>
        /// <param name="arg0">The type of event.</param>
        /// <param name="arg1">The listener object to remove.</param>
        /// <param name="arg2">
        /// (default = false)  This parameter applies to
        /// display objects in the ActionScript 3.0 display list architecture, used by SWF content.Specifies whether the listener was registered for the capture phase or the target
        /// and bubbling phases. If the listener was registered for both the capture phase and
        /// the target and bubbling phases, two calls to removeEventListener()
        /// are required to remove both, one call with useCapture() set to true,
        /// and another call with useCapture() set to false.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void removeEventListener(Avm.String arg0, Avm.Function arg1, bool arg2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void removeEventListener(Avm.String arg0, Avm.Function arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();

        /// <summary>
        /// Checks whether the EventDispatcher object has any listeners registered for a specific
        /// type of event. This allows you to determine where an EventDispatcher object has
        /// altered handling of an event type in the event flow hierarchy. To determine whether
        /// a specific event type actually triggers an event listener, use willTrigger().
        /// The difference between hasEventListener() and willTrigger()
        /// is that hasEventListener() examines only the object to which it belongs,
        /// whereas willTrigger() examines the entire event flow for the event
        /// specified by the type parameter.
        /// When hasEventListener() is called from a LoaderInfo object, only the
        /// listeners that the caller can access are considered.
        /// </summary>
        /// <param name="arg0">The type of event.</param>
        /// <returns>
        /// A value of true
        /// if a listener of the specified type is registered; false otherwise.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool hasEventListener(Avm.String arg0);

        /// <summary>
        /// Registers an event listener object with an EventDispatcher object so that the listener
        /// receives notification of an event. You can register event listeners on all nodes
        /// in the display list for a specific type of event, phase, and priority.
        /// After you successfully register an event listener, you cannot change its priority
        /// through additional calls to addEventListener(). To change a listener&apos;s
        /// priority, you must first call removeListener(). Then you can register
        /// the listener again with the new priority level.
        /// Keep in mind that after the listener is registered, subsequent calls to addEventListener()
        /// with a different type or useCapture value result in the
        /// creation of a separate listener registration. For example, if you first register
        /// a listener with useCapture set to true, it listens only
        /// during the capture phase. If you call addEventListener() again using
        /// the same listener object, but with useCapture set to false,
        /// you have two separate listeners: one that listens during the capture phase and another
        /// that listens during the target and bubbling phases.
        /// You cannot register an event listener for only the target phase or the bubbling
        /// phase. Those phases are coupled during registration because bubbling applies only
        /// to the ancestors of the target node.
        /// If you no longer need an event listener, remove it by calling removeEventListener(),
        /// or memory problems could result. Objects with registered event listeners are not
        /// automatically removed from memory because the garbage collector does not remove
        /// objects that still have references.
        /// Copying an EventDispatcher instance does not copy the event listeners attached to
        /// it. (If your newly created node needs an event listener, you must attach the listener
        /// after creating the node). However, if you move an EventDispatcher instance, the
        /// event listeners attached to it move along with it.
        /// If the event listener is being registered on a node while an event is being processed
        /// on this node, the event listener is not triggered during the current phase but can
        /// be triggered during a later phase in the event flow, such as the bubbling phase.
        /// If an event listener is removed from a node while an event is being processed on
        /// the node, it is still triggered by the current actions. After it is removed, the
        /// event listener is never invoked again (unless registered again for future processing).
        /// </summary>
        /// <param name="arg0">The type of event.</param>
        /// <param name="arg1">
        /// The listener function that processes the event. This function must accept
        /// an Event object as its only parameter and must return nothing, as this example
        /// shows:.function(evt:Event):void
        /// The function can have any name.
        /// </param>
        /// <param name="arg2">
        /// (default = false)  This parameter applies to
        /// display objects in the ActionScript 3.0 display list architecture, used by SWF content.Determines whether the listener works in the capture phase or the target and bubbling
        /// phases. If useCapture is set to true, the listener processes
        /// the event only during the capture phase and not in the target or bubbling phase.
        /// If useCapture is false, the listener processes the event
        /// only during the target or bubbling phase. To listen for the event in all three phases,
        /// call addEventListener twice, once with the useCapture
        /// set to true, then again with useCapture set to false.
        /// </param>
        /// <param name="arg3">
        /// (default = 0)  The priority level of the event
        /// listener. The priority is designated by a signed 32-bit integer. The higher the
        /// number, the higher the priority. All listeners with priority n are processed
        /// before listeners of priority n-1. If two or more listeners share the same
        /// priority, they are processed in the order in which they were added. The default
        /// priority is 0.
        /// </param>
        /// <param name="arg4">
        /// (default = false)  Determines whether the reference
        /// to the listener is strong or weak. A strong reference (the default) prevents your
        /// listener from being garbage-collected. A weak reference does not.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void addEventListener(Avm.String arg0, Avm.Function arg1, bool arg2, int arg3, bool arg4);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addEventListener(Avm.String arg0, Avm.Function arg1, bool arg2, int arg3);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addEventListener(Avm.String arg0, Avm.Function arg1, bool arg2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addEventListener(Avm.String arg0, Avm.Function arg1);
    }
}
