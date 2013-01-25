using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// An object dispatches a SecurityErrorEvent object to report the occurrence of a
    /// security error. Security errors reported through this class are generally from asynchronous
    /// operations, such as loading data, in which security violations may not manifest immediately.
    /// Your event listener can access the object&apos;s text  property to determine what operation was
    /// attempted and any URLs that were involved. If there are no event listeners, the
    /// debugger version of Flash Player  or the AIR Debug Launcher (ADL) application
    /// automatically displays an error message that contains the contents of the text
    /// property. There is one type of security error event: SecurityErrorEvent.SECURITY_ERROR .
    /// </summary>
    [PageFX.AbcInstance(330)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class SecurityErrorEvent : flash.events.ErrorEvent
    {
        /// <summary>
        /// The SecurityErrorEvent.SECURITY_ERROR constant defines the value of the type property of a securityError event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.currentTargetThe object that is actively processing the Event
        /// object with an event listener.targetThe network object reporting the security error.textText to be displayed as an error message.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String SECURITY_ERROR;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SecurityErrorEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String text, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SecurityErrorEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String text);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SecurityErrorEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SecurityErrorEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SecurityErrorEvent(Avm.String type);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.events.Event clone();

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();
    }
}
