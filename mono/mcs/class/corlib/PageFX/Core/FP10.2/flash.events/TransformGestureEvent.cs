using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    [PageFX.AbcInstance(241)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class TransformGestureEvent : flash.events.GestureEvent
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String GESTURE_ZOOM;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String GESTURE_PAN;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String GESTURE_ROTATE;

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String GESTURE_SWIPE;

        public extern virtual double scaleX
        {
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double scaleY
        {
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double rotation
        {
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(12)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double offsetX
        {
            [PageFX.AbcInstanceTrait(13)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(14)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double offsetY
        {
            [PageFX.AbcInstanceTrait(15)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(16)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TransformGestureEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String phase, double localX, double localY, double scaleX, double scaleY, double rotation, double offsetX, double offsetY, bool ctrlKey, bool altKey, bool shiftKey);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TransformGestureEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String phase, double localX, double localY, double scaleX, double scaleY, double rotation, double offsetX, double offsetY, bool ctrlKey, bool altKey);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TransformGestureEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String phase, double localX, double localY, double scaleX, double scaleY, double rotation, double offsetX, double offsetY, bool ctrlKey);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TransformGestureEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String phase, double localX, double localY, double scaleX, double scaleY, double rotation, double offsetX, double offsetY);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TransformGestureEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String phase, double localX, double localY, double scaleX, double scaleY, double rotation, double offsetX);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TransformGestureEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String phase, double localX, double localY, double scaleX, double scaleY, double rotation);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TransformGestureEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String phase, double localX, double localY, double scaleX, double scaleY);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TransformGestureEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String phase, double localX, double localY, double scaleX);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TransformGestureEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String phase, double localX, double localY);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TransformGestureEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String phase, double localX);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TransformGestureEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String phase);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TransformGestureEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TransformGestureEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TransformGestureEvent(Avm.String type);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.events.Event clone();

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();


    }
}
