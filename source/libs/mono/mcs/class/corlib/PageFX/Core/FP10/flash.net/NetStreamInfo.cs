using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class NetStreamInfo : Avm.Object
    {
        public extern virtual double videoBufferByteLength
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double droppedFrames
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double dataBytesPerSecond
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double dataBufferLength
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double audioLossRate
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double currentBytesPerSecond
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double videoBytesPerSecond
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double videoByteCount
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double SRTT
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double audioByteCount
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double audioBytesPerSecond
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double dataBufferByteLength
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double playbackBytesPerSecond
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double maxBytesPerSecond
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double dataByteCount
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double audioBufferLength
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double videoBufferLength
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double audioBufferByteLength
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double byteCount
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetStreamInfo(double arg0, double arg1, double arg2, double arg3, double arg4, double arg5, double arg6, double arg7, double arg8, double arg9, double arg10, double arg11, double arg12, double arg13, double arg14, double arg15, double arg16, double arg17, double arg18);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String toString();
    }
}
