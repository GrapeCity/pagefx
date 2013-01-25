using System;
using System.Runtime.CompilerServices;

namespace flash.sensors
{
    [PageFX.AbcInstance(82)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class Accelerometer : flash.events.EventDispatcher
    {
        public extern virtual bool muted
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static bool isSupported
        {
            [PageFX.AbcClassTrait(0)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [PageFX.Event("status")]
        public event flash.events.StatusEventHandler status
        {
            add { }
            remove { }
        }

        [PageFX.Event("update")]
        public event flash.events.AccelerometerEventHandler update
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Accelerometer();

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setRequestedUpdateInterval(double interval);


    }
}
