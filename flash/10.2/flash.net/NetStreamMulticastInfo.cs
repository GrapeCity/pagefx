using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    [PageFX.AbcInstance(211)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class NetStreamMulticastInfo : Avm.Object
    {
        public extern virtual double sendDataBytesPerSecond
        {
            [PageFX.AbcInstanceTrait(19)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double sendControlBytesPerSecond
        {
            [PageFX.AbcInstanceTrait(20)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double receiveDataBytesPerSecond
        {
            [PageFX.AbcInstanceTrait(21)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double receiveControlBytesPerSecond
        {
            [PageFX.AbcInstanceTrait(22)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double bytesPushedToPeers
        {
            [PageFX.AbcInstanceTrait(23)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double fragmentsPushedToPeers
        {
            [PageFX.AbcInstanceTrait(24)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double bytesRequestedByPeers
        {
            [PageFX.AbcInstanceTrait(25)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double fragmentsRequestedByPeers
        {
            [PageFX.AbcInstanceTrait(26)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double bytesPushedFromPeers
        {
            [PageFX.AbcInstanceTrait(27)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double fragmentsPushedFromPeers
        {
            [PageFX.AbcInstanceTrait(28)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double bytesRequestedFromPeers
        {
            [PageFX.AbcInstanceTrait(29)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double fragmentsRequestedFromPeers
        {
            [PageFX.AbcInstanceTrait(30)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double sendControlBytesPerSecondToServer
        {
            [PageFX.AbcInstanceTrait(31)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double receiveDataBytesPerSecondFromServer
        {
            [PageFX.AbcInstanceTrait(32)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double bytesReceivedFromServer
        {
            [PageFX.AbcInstanceTrait(33)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double fragmentsReceivedFromServer
        {
            [PageFX.AbcInstanceTrait(34)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double receiveDataBytesPerSecondFromIPMulticast
        {
            [PageFX.AbcInstanceTrait(35)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double bytesReceivedFromIPMulticast
        {
            [PageFX.AbcInstanceTrait(36)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double fragmentsReceivedFromIPMulticast
        {
            [PageFX.AbcInstanceTrait(37)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetStreamMulticastInfo(double sendDataBytesPerSecond, double sendControlBytesPerSecond, double receiveDataBytesPerSecond, double receiveControlBytesPerSecond, double bytesPushedToPeers, double fragmentsPushedToPeers, double bytesRequestedByPeers, double fragmentsRequestedByPeers, double bytesPushedFromPeers, double fragmentsPushedFromPeers, double bytesRequestedFromPeers, double fragmentsRequestedFromPeers, double sendControlBytesPerSecondToServer, double receiveDataBytesPerSecondFromServer, double bytesReceivedFromServer, double fragmentsReceivedFromServer, double receiveDataBytesPerSecondFromIPMulticast, double bytesReceivedFromIPMulticast, double fragmentsReceivedFromIPMulticast);

        [PageFX.AbcInstanceTrait(38)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();
    }
}
