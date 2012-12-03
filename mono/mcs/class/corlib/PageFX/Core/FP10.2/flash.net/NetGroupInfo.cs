using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    [PageFX.AbcInstance(102)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class NetGroupInfo : Avm.Object
    {
        public extern virtual double postingSendDataBytesPerSecond
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double postingSendControlBytesPerSecond
        {
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double postingReceiveDataBytesPerSecond
        {
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double postingReceiveControlBytesPerSecond
        {
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double routingSendBytesPerSecond
        {
            [PageFX.AbcInstanceTrait(12)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double routingReceiveBytesPerSecond
        {
            [PageFX.AbcInstanceTrait(13)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double objectReplicationSendBytesPerSecond
        {
            [PageFX.AbcInstanceTrait(14)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double objectReplicationReceiveBytesPerSecond
        {
            [PageFX.AbcInstanceTrait(15)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetGroupInfo(double postingSendDataBytesPerSecond, double postingSendControlBytesPerSecond, double postingReceiveDataBytesPerSecond, double postingReceiveControlBytesPerSecond, double routingSendBytesPerSecond, double routingReceiveBytesPerSecond, double objectReplicationSendBytesPerSecond, double objectReplicationReceiveBytesPerSecond);

        [PageFX.AbcInstanceTrait(16)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();
    }
}
