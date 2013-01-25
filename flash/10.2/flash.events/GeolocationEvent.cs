using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    [PageFX.AbcInstance(368)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class GeolocationEvent : flash.events.Event
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String UPDATE;

        public extern virtual double latitude
        {
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double longitude
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

        public extern virtual double altitude
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

        public extern virtual double horizontalAccuracy
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

        public extern virtual double verticalAccuracy
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

        public extern virtual double speed
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

        public extern virtual double heading
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

        public extern virtual double timestamp
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

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GeolocationEvent(Avm.String type, bool bubbles, bool cancelable, double latitude, double longitude, double altitude, double hAccuracy, double vAccuracy, double speed, double heading, double timestamp);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GeolocationEvent(Avm.String type, bool bubbles, bool cancelable, double latitude, double longitude, double altitude, double hAccuracy, double vAccuracy, double speed, double heading);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GeolocationEvent(Avm.String type, bool bubbles, bool cancelable, double latitude, double longitude, double altitude, double hAccuracy, double vAccuracy, double speed);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GeolocationEvent(Avm.String type, bool bubbles, bool cancelable, double latitude, double longitude, double altitude, double hAccuracy, double vAccuracy);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GeolocationEvent(Avm.String type, bool bubbles, bool cancelable, double latitude, double longitude, double altitude, double hAccuracy);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GeolocationEvent(Avm.String type, bool bubbles, bool cancelable, double latitude, double longitude, double altitude);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GeolocationEvent(Avm.String type, bool bubbles, bool cancelable, double latitude, double longitude);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GeolocationEvent(Avm.String type, bool bubbles, bool cancelable, double latitude);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GeolocationEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GeolocationEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GeolocationEvent(Avm.String type);

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.events.Event clone();

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();


    }
}
