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
    [PageFX.AbcInstance(221)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class SoundChannel : flash.events.EventDispatcher
    {
        /// <summary>The current position of the playhead within the sound.</summary>
        public extern virtual double position
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The SoundTransform object assigned to the sound channel. A SoundTransform object
        /// includes properties for setting volume, panning, left speaker assignment, and right
        /// speaker assignment.
        /// </summary>
        public extern virtual flash.media.SoundTransform soundTransform
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>The current amplitude (volume) of the left channel, from 0 (silent) to 1 (full amplitude).</summary>
        public extern virtual double leftPeak
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>The current amplitude (volume) of the right channel, from 0 (silent) to 1 (full amplitude).</summary>
        public extern virtual double rightPeak
        {
            [PageFX.AbcInstanceTrait(5)]
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
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void stop();


    }
}
