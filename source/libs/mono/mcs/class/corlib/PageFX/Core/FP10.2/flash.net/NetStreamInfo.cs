using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    [PageFX.AbcInstance(17)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class NetStreamInfo : Avm.Object
    {
        public extern virtual double currentBytesPerSecond
        {
            [PageFX.AbcInstanceTrait(20)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double byteCount
        {
            [PageFX.AbcInstanceTrait(21)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double maxBytesPerSecond
        {
            [PageFX.AbcInstanceTrait(22)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double audioBytesPerSecond
        {
            [PageFX.AbcInstanceTrait(23)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double audioByteCount
        {
            [PageFX.AbcInstanceTrait(24)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double videoBytesPerSecond
        {
            [PageFX.AbcInstanceTrait(25)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double videoByteCount
        {
            [PageFX.AbcInstanceTrait(26)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double dataBytesPerSecond
        {
            [PageFX.AbcInstanceTrait(27)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double dataByteCount
        {
            [PageFX.AbcInstanceTrait(28)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double playbackBytesPerSecond
        {
            [PageFX.AbcInstanceTrait(29)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double droppedFrames
        {
            [PageFX.AbcInstanceTrait(30)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double audioBufferByteLength
        {
            [PageFX.AbcInstanceTrait(31)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double videoBufferByteLength
        {
            [PageFX.AbcInstanceTrait(32)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double dataBufferByteLength
        {
            [PageFX.AbcInstanceTrait(33)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double audioBufferLength
        {
            [PageFX.AbcInstanceTrait(34)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double videoBufferLength
        {
            [PageFX.AbcInstanceTrait(35)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double dataBufferLength
        {
            [PageFX.AbcInstanceTrait(36)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double SRTT
        {
            [PageFX.AbcInstanceTrait(37)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double audioLossRate
        {
            [PageFX.AbcInstanceTrait(38)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double videoLossRate
        {
            [PageFX.AbcInstanceTrait(39)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetStreamInfo(double curBPS, double byteCount, double maxBPS, double audioBPS, double audioByteCount, double videoBPS, double videoByteCount, double dataBPS, double dataByteCount, double playbackBPS, double droppedFrames, double audioBufferByteLength, double videoBufferByteLength, double dataBufferByteLength, double audioBufferLength, double videoBufferLength, double dataBufferLength, double srtt, double audioLossRate, double videoLossRate);

        [PageFX.AbcInstanceTrait(40)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();
    }
}
