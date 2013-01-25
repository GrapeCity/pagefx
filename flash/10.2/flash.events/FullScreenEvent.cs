using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// Player dispatches a FullScreenEvent object whenever the Stage
    /// enters or leaves full-screen display mode. There is only one type of fullScreen
    /// event: FullScreenEvent.FULL_SCREEN .
    /// </summary>
    [PageFX.AbcInstance(341)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class FullScreenEvent : flash.events.ActivityEvent
    {
        /// <summary>
        /// The FullScreenEvent.FULL_SCREEN constant defines the value of the
        /// type property of a fullScreen event object.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String FULL_SCREEN;

        /// <summary>
        /// Indicates whether the Stage object is in full-screen mode (true) or
        /// not (false).
        /// </summary>
        public extern virtual bool fullScreen
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FullScreenEvent(Avm.String type, bool bubbles, bool cancelable, bool fullScreen);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FullScreenEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FullScreenEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FullScreenEvent(Avm.String type);

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.events.Event clone();

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();


    }
}
