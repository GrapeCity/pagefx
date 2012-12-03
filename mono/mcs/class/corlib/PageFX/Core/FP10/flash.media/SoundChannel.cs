using System;
using System.Runtime.CompilerServices;

namespace flash.media
{
    /// <summary>
    /// The SoundChannel class controls a sound in an application. Every sound
    /// is assigned to a sound channel, and the application can have multiple
    /// sound channels that are mixed together. The SoundChannel class contains a stop()  method,
    /// properties for monitoring the amplitude (volume) of the channel, and a property for assigning a
    /// SoundTransform object to the channel.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class SoundChannel : flash.events.EventDispatcher
    {
        public extern virtual double leftPeak
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double position
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual SoundTransform soundTransform
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double rightPeak
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [PageFX.Event("soundComplete")]
        public event flash.events.EventHandler soundComplete
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SoundChannel();

        /// <summary>Stops the sound playing in the channel.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void stop();


    }
}
