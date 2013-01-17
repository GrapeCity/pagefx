using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// Player dispatches a FullScreenEvent object whenever the Stage
    /// enters or leaves full-screen display mode. There is only one type of fullScreen
    /// event: FullScreenEvent.FULL_SCREEN .
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP10]
    public class FullScreenEvent : ActivityEvent
    {
        /// <summary>
        /// The FullScreenEvent.FULL_SCREEN constant defines the value of the
        /// type property of a fullScreen event object.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String FULL_SCREEN;

        public extern virtual bool fullScreen
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FullScreenEvent(Avm.String arg0, bool arg1, bool arg2, bool arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FullScreenEvent(Avm.String arg0, bool arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FullScreenEvent(Avm.String arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FullScreenEvent(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Event clone();
    }
}
