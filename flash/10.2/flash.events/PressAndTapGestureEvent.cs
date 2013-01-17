using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    [PageFX.AbcInstance(220)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class PressAndTapGestureEvent : flash.events.GestureEvent
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String GESTURE_PRESS_AND_TAP;

        public extern virtual double tapLocalX
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

        public extern virtual double tapLocalY
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double tapStageX
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double tapStageY
        {
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern PressAndTapGestureEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String phase, double localX, double localY, double tapLocalX, double tapLocalY, bool ctrlKey, bool altKey, bool shiftKey);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern PressAndTapGestureEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String phase, double localX, double localY, double tapLocalX, double tapLocalY, bool ctrlKey, bool altKey);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern PressAndTapGestureEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String phase, double localX, double localY, double tapLocalX, double tapLocalY, bool ctrlKey);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern PressAndTapGestureEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String phase, double localX, double localY, double tapLocalX, double tapLocalY);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern PressAndTapGestureEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String phase, double localX, double localY, double tapLocalX);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern PressAndTapGestureEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String phase, double localX, double localY);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern PressAndTapGestureEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String phase, double localX);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern PressAndTapGestureEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String phase);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern PressAndTapGestureEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern PressAndTapGestureEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern PressAndTapGestureEvent(Avm.String type);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.events.Event clone();

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();


    }
}
