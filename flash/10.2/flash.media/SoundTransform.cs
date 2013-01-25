using System;
using System.Runtime.CompilerServices;

namespace flash.media
{
    /// <summary>
    /// The SoundTransform class contains properties for volume and panning.
    /// The following objects contain a soundTransform  property,
    /// the value of which is a SoundTransform object: Microphone, NetStream, SimpleButton,
    /// SoundChannel, SoundMixer, and Sprite.
    /// </summary>
    [PageFX.AbcInstance(122)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class SoundTransform : Avm.Object
    {
        /// <summary>The volume, ranging from 0 (silent) to 1 (full volume).</summary>
        public extern virtual double volume
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// A value, from 0 (none) to 1 (all), specifying how much of the left input is played in the
        /// left speaker.
        /// </summary>
        public extern virtual double leftToLeft
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// A value, from 0 (none) to 1 (all), specifying how much of the left input is played in the
        /// right speaker.
        /// </summary>
        public extern virtual double leftToRight
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// A value, from 0 (none) to 1 (all), specifying how much of the right input is played in the
        /// right speaker.
        /// </summary>
        public extern virtual double rightToRight
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// A value, from 0 (none) to 1 (all), specifying how much of the right input is played in the
        /// left speaker.
        /// </summary>
        public extern virtual double rightToLeft
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The left-to-right panning of the sound, ranging from -1 (full pan left)
        /// to 1 (full pan right). A value of 0 represents no panning (balanced center between
        /// right and left).
        /// </summary>
        public extern virtual double pan
        {
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SoundTransform(double vol, double panning);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SoundTransform(double vol);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SoundTransform();


    }
}
