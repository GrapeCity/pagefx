using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    [PageFX.AbcInstance(306)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class TouchEvent : flash.events.Event
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String TOUCH_BEGIN;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String TOUCH_END;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String TOUCH_MOVE;

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String TOUCH_OVER;

        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String TOUCH_OUT;

        [PageFX.AbcClassTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String TOUCH_ROLL_OVER;

        [PageFX.AbcClassTrait(6)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String TOUCH_ROLL_OUT;

        [PageFX.AbcClassTrait(7)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String TOUCH_TAP;

        public extern virtual double localX
        {
            [PageFX.AbcInstanceTrait(12)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(13)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double localY
        {
            [PageFX.AbcInstanceTrait(14)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(15)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int touchPointID
        {
            [PageFX.AbcInstanceTrait(16)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(17)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool isPrimaryTouchPoint
        {
            [PageFX.AbcInstanceTrait(18)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(19)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double sizeX
        {
            [PageFX.AbcInstanceTrait(20)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(21)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double sizeY
        {
            [PageFX.AbcInstanceTrait(22)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(23)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double pressure
        {
            [PageFX.AbcInstanceTrait(24)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(25)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual flash.display.InteractiveObject relatedObject
        {
            [PageFX.AbcInstanceTrait(26)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(27)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool ctrlKey
        {
            [PageFX.AbcInstanceTrait(28)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(29)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool altKey
        {
            [PageFX.AbcInstanceTrait(30)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(31)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool shiftKey
        {
            [PageFX.AbcInstanceTrait(32)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(33)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double stageX
        {
            [PageFX.AbcInstanceTrait(34)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double stageY
        {
            [PageFX.AbcInstanceTrait(35)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool isRelatedObjectInaccessible
        {
            [PageFX.AbcInstanceTrait(39)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(40)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TouchEvent(Avm.String type, bool bubbles, bool cancelable, int touchPointID, bool isPrimaryTouchPoint, double localX, double localY, double sizeX, double sizeY, double pressure, flash.display.InteractiveObject relatedObject, bool ctrlKey, bool altKey, bool shiftKey);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TouchEvent(Avm.String type, bool bubbles, bool cancelable, int touchPointID, bool isPrimaryTouchPoint, double localX, double localY, double sizeX, double sizeY, double pressure, flash.display.InteractiveObject relatedObject, bool ctrlKey, bool altKey);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TouchEvent(Avm.String type, bool bubbles, bool cancelable, int touchPointID, bool isPrimaryTouchPoint, double localX, double localY, double sizeX, double sizeY, double pressure, flash.display.InteractiveObject relatedObject, bool ctrlKey);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TouchEvent(Avm.String type, bool bubbles, bool cancelable, int touchPointID, bool isPrimaryTouchPoint, double localX, double localY, double sizeX, double sizeY, double pressure, flash.display.InteractiveObject relatedObject);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TouchEvent(Avm.String type, bool bubbles, bool cancelable, int touchPointID, bool isPrimaryTouchPoint, double localX, double localY, double sizeX, double sizeY, double pressure);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TouchEvent(Avm.String type, bool bubbles, bool cancelable, int touchPointID, bool isPrimaryTouchPoint, double localX, double localY, double sizeX, double sizeY);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TouchEvent(Avm.String type, bool bubbles, bool cancelable, int touchPointID, bool isPrimaryTouchPoint, double localX, double localY, double sizeX);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TouchEvent(Avm.String type, bool bubbles, bool cancelable, int touchPointID, bool isPrimaryTouchPoint, double localX, double localY);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TouchEvent(Avm.String type, bool bubbles, bool cancelable, int touchPointID, bool isPrimaryTouchPoint, double localX);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TouchEvent(Avm.String type, bool bubbles, bool cancelable, int touchPointID, bool isPrimaryTouchPoint);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TouchEvent(Avm.String type, bool bubbles, bool cancelable, int touchPointID);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TouchEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TouchEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TouchEvent(Avm.String type);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.events.Event clone();

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();

        [PageFX.AbcInstanceTrait(36)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void updateAfterEvent();


    }
}
