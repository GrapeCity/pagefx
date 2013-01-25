using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    [PageFX.AbcInstance(364)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class UncaughtErrorEvent : flash.events.ErrorEvent
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String UNCAUGHT_ERROR;

        public extern virtual object error
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern UncaughtErrorEvent(Avm.String type, bool bubbles, bool cancelable, object error_in);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern UncaughtErrorEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern UncaughtErrorEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern UncaughtErrorEvent(Avm.String type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern UncaughtErrorEvent();

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.events.Event clone();

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();
    }
}
