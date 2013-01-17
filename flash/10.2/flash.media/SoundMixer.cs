using System;
using System.Runtime.CompilerServices;

namespace flash.media
{
    /// <summary>
    /// The SoundMixer class contains static properties and methods for global sound control
    /// in the SWF file. The SoundMixer class controls embedded streaming sounds in a SWF;
    /// it does not control dynamically created Sound objects (that is, Sound objects
    /// created in ActionScript).
    /// </summary>
    [PageFX.AbcInstance(137)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class SoundMixer : Avm.Object
    {
        /// <summary>
        /// The number of seconds to preload an embedded streaming sound into a buffer before it starts
        /// to stream. The data in a loaded sound, including its buffer time,
        /// cannot be accessed by a SWF file that is in a different domain
        /// unless you implement a cross-domain policy file.
        /// For more information about security and sound, see the Sound class description.
        /// The SoundMixer.bufferTime property only affects the buffer time
        /// for embedded streaming sounds in a SWF and is independent of dynamically created
        /// Sound objects (that is, Sound objects created in ActionScript).
        /// The value of SoundMixer.bufferTime cannot override
        /// or set the default of the buffer time specified in the SoundLoaderContext object
        /// that is passed to the Sound.load() method.
        /// </summary>
        public extern static int bufferTime
        {
            [PageFX.AbcClassTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcClassTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The SoundTransform object that controls global sound properties. A SoundTransform object
        /// includes properties for setting volume, panning, left speaker assignment, and right
        /// speaker assignment. This SoundTransform object only affects sounds embedded within a
        /// SWF file. The SoundTransform object used in this property provides final sound settings
        /// that are applied to all sounds after any individual sound settings are applied.
        /// </summary>
        public extern static flash.media.SoundTransform soundTransform
        {
            [PageFX.AbcClassTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcClassTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SoundMixer();

        /// <summary>
        /// Stops all sounds currently playing. This method does not stop the playhead.
        /// Sounds set to stream will resume playing as the playhead moves over the frames in which they
        /// are located.When using this property, consider the Flash Player security model: By default, calling the soundMixer.stopAll() method stops
        /// only sounds in the same security sandbox as the object that is calling the method.
        /// Any sounds whose playback was not started from the same sandbox as the calling object
        /// are not stopped.When you load the sound, using the load() method of the Sound class, you can
        /// specify a context parameter, which is a SoundLoaderContext object. If you set the
        /// checkPolicyFile  property of the SoundLoaderContext object to true, Flash Player
        /// checks for a cross-domain policy file on the server from which the sound is loaded. If the server has a
        /// cross-domain policy file, and the file permits the domain of a SWF file, then the file can stop the loaded
        /// sound by using the soundMixer.stopAll() method; otherwise it cannot.However, in the Adobe Integrated Runtime, content in the application security sandbox (content
        /// installed with the AIR application) are not restricted by these security limitations.For more information, see the following:The security chapter in the
        /// Programming ActionScript 3.0 book and the latest comments on LiveDocs
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void stopAll();

        /// <summary>
        /// Takes a snapshot of the current sound wave and places it into the specified ByteArray object.
        /// The values are formatted as normalized floating-point values, in the range -1.0 to 1.0.
        /// The ByteArray object passed to the outputArray parameter is overwritten with the new values.
        /// The size of the ByteArray object created is fixed to 512 floating-point values, where the
        /// first 256 values represent the left channel, and the second 256 values represent
        /// the right channel.
        /// Note: This method is subject to local file security restrictions and
        /// restrictions on cross-domain loading. If you are working with local SWF files or sounds loaded from a server in a
        /// different domain than the calling SWF, you might need to address sandbox restrictions
        /// through a cross-domain policy file. For more information, see the Sound class description.
        /// In addition, this method cannot be used to extract data from RTMP streams, even when
        /// it is called by SWF files that reside in the same domain as the RTMP server.
        /// </summary>
        /// <param name="outputArray">
        /// A ByteArray object that holds the values associated with the sound.
        /// If any sounds are not available due to security restrictions
        /// (areSoundsInaccessible == true), the outputArray object
        /// is left unchanged. If all sounds are stopped, the outputArray object is
        /// filled with zeros.
        /// </param>
        /// <param name="FFTMode">
        /// (default = false)  A Boolean value indicating whether a Fourier transformation is performed
        /// on the sound data first. Setting this parameter to true causes the method to return a
        /// frequency spectrum instead of the raw sound wave. In the frequency spectrum, low frequencies
        /// are represented on the left and high frequencies are on the right.
        /// </param>
        /// <param name="stretchFactor">
        /// (default = 0)  The resolution of the sound samples.
        /// If you set the stretchFactor value to 0, data is sampled at 44.1 KHz;
        /// with a value of 1, data is sampled at 22.05 KHz; with a value of 2, data is sampled 11.025 KHz;
        /// and so on.
        /// </param>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void computeSpectrum(flash.utils.ByteArray outputArray, bool FFTMode, int stretchFactor);

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void computeSpectrum(flash.utils.ByteArray outputArray, bool FFTMode);

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void computeSpectrum(flash.utils.ByteArray outputArray);

        /// <summary>
        /// Determines whether any sounds are not accessible due to security restrictions. For example,
        /// a sound loaded from a domain other than that of the SWF file is not accessible if
        /// the server for the sound has no cross-domain policy file that grants access to
        /// the domain of the SWF file. The sound can still be loaded and played, but low-level
        /// operations, such as getting ID3 metadata for the sound, cannot be performed on
        /// inaccessible sounds.
        /// </summary>
        /// <returns>Boolean</returns>
        [PageFX.AbcClassTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static bool areSoundsInaccessible();
    }
}
