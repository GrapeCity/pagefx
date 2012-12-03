using System;
using System.Runtime.CompilerServices;

namespace flash.media
{
    /// <summary>
    /// The Sound class lets you work with sound in an application. The Sound class
    /// lets you create a new Sound object, load and play an external MP3 file into that object,
    /// close the sound stream, and access
    /// data about the sound, such as information about the number of bytes in the stream and
    /// ID3 metadata. More detailed control of the sound is performed through the sound source —
    /// the SoundChannel or Microphone object for the sound — and through the properties
    /// in the SoundTransform class that control the output of the sound to the computer&apos;s speakers.
    /// </summary>
    [PageFX.AbcInstance(331)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class Sound : flash.events.EventDispatcher
    {
        /// <summary>
        /// The URL from which this sound was loaded. This property is applicable only to Sound
        /// objects that were loaded using the Sound.load() method. For
        /// Sound objects that are associated with a sound asset from a SWF&apos;s library, the
        /// value of the url property is null.
        /// When you first call Sound.load(), the url property
        /// initially has a value of null, because the final URL is not yet known.
        /// The url property will have a non-null value as soon as an
        /// open event is dispatched from the Sound object.The url property contains the final, absolute URL from which a sound was
        /// loaded. The value of url is usually the same as the value passed to the
        /// stream parameter of Sound.load().
        /// However, if you passed a relative URL to Sound.load()
        /// the value of the url property will represent the absolute URL.
        /// Additionally, if the original URL request is redirected by an HTTP server, the value
        /// of the url property reflects the final URL from which the sound file was actually
        /// downloaded.  This reporting of an absolute, final URL is equivalent to the behavior of
        /// LoaderInfo.url.
        /// </summary>
        public extern virtual Avm.String url
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool isURLInaccessible
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>The length of the current sound in milliseconds.</summary>
        public extern virtual double length
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Returns the buffering state of external MP3 files. If the value is true,
        /// any playback is
        /// currently suspended while the object waits for more data.
        /// </summary>
        public extern virtual bool isBuffering
        {
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Returns the currently available number of bytes in this sound object. This is
        /// usually only useful for externally loaded files.
        /// </summary>
        public extern virtual uint bytesLoaded
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>Returns the total number of bytes in this sound object.</summary>
        public extern virtual int bytesTotal
        {
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Provides access to the metadata that is part of an MP3 file.
        /// MP3 sound files can contain ID3 tags, which provide metadata about the
        /// file. If an MP3 sound that you load using the Sound.load()
        /// method contains ID3 tags, you can query these properties. Only ID3 tags
        /// that use the UTF-8 character set are supported.Flash Player 9 and later supports ID3 2.0 tags,
        /// specifically 2.3 and 2.4. The following tables list the standard ID3 2.0 tags
        /// and the type of content the tags represent. The Sound.id3 property provides
        /// access to these tags through the format
        /// my_sound.id3.COMM, my_sound.id3.TIME, and so on. The first
        /// table describes tags that can be accessed either through the ID3 2.0 property name or
        /// the ActionScript property name. The second table describes ID3 tags that are supported but do not have
        /// predefined properties in ActionScript. ID3 2.0 tagCorresponding ActionScript propertyCOMMSound.id3.commentTALBSound.id3.album TCONSound.id3.genreTIT2Sound.id3.songName TPE1Sound.id3.artistTRCKSound.id3.track TYERSound.id3.year The following table describes ID3 tags that are supported but do not have
        /// predefined properties in ActionScript. You access them by calling
        /// mySound.id3.TFLT, mySound.id3.TIME, and so on.PropertyDescriptionTFLTFile typeTIMETimeTIT1Content group descriptionTIT2Title/song name/content descriptionTIT3Subtitle/description refinementTKEYInitial keyTLANLanguagesTLENLengthTMEDMedia typeTOALOriginal album/movie/show titleTOFNOriginal filenameTOLYOriginal lyricists/text writersTOPEOriginal artists/performersTORYOriginal release yearTOWNFile owner/licenseeTPE1Lead performers/soloistsTPE2Band/orchestra/accompanimentTPE3Conductor/performer refinementTPE4Interpreted, remixed, or otherwise modified byTPOSPart of a setTPUBPublisherTRCKTrack number/position in setTRDARecording datesTRSNInternet radio station nameTRSOInternet radio station ownerTSIZSizeTSRCISRC (international standard recording code)TSSESoftware/hardware and settings used for encodingTYERYearWXXXURL link frameWhen using this property, consider the Flash Player security model:The id3 property of a Sound object is always permitted for SWF files
        /// that are in the same security sandbox as the sound file. For files in other sandboxes, there
        /// are security checks.When you load the sound, using the load() method of the Sound class, you can
        /// specify a context parameter, which is a SoundLoaderContext object. If you set the
        /// checkPolicyFile  property of the SoundLoaderContext object to true, Flash Player
        /// checks for a cross-domain policy file on the server from which the sound is loaded. If there is a
        /// cross-domain policy file, and the file permits the domain of the loading SWF file, then the file is allowed
        /// to access the id3 property of the Sound object; otherwise it is not.However, in the Adobe Integrated Runtime, content in the application security sandbox (content
        /// installed with the AIR application) are not restricted by these security limitations.For more information, see the following:The security chapter in the
        /// Programming ActionScript 3.0 book and the latest comments on LiveDocs
        /// </summary>
        public extern virtual flash.media.ID3Info id3
        {
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [PageFX.Event("progress")]
        public event flash.events.ProgressEventHandler progress
        {
            add { }
            remove { }
        }

        [PageFX.Event("open")]
        public event flash.events.EventHandler open
        {
            add { }
            remove { }
        }

        [PageFX.Event("ioError")]
        public event flash.events.IOErrorEventHandler ioError
        {
            add { }
            remove { }
        }

        [PageFX.Event("id3")]
        public event flash.events.EventHandler OnId3
        {
            add { }
            remove { }
        }

        [PageFX.Event("complete")]
        public event flash.events.EventHandler complete
        {
            add { }
            remove { }
        }

        [PageFX.Event("sampleData")]
        public event flash.events.SampleDataEventHandler sampleData
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Sound(flash.net.URLRequest stream, flash.media.SoundLoaderContext context);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Sound(flash.net.URLRequest stream);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Sound();

        /// <summary>
        /// Initiates loading of an external MP3 file from the specified URL. If you provide
        /// a valid URLRequest object to the Sound constructor, the constructor calls
        /// Sound.load() for you. You only need to call Sound.load()
        /// yourself if you
        /// don&apos;t pass a valid URLRequest object to the Sound constructor or you pass a null
        /// value.
        /// Once load() is called on a Sound object, you can&apos;t later load
        /// a different sound file into that Sound object. To load a different sound file,
        /// create a new Sound object.
        /// When using this method, consider the Flash Player security model:
        /// Calling Sound.load() is not allowed if the calling SWF file is in the
        /// local-with-file-system sandbox and the sound is in a network sandbox.Access from the local-trusted or local-with-networking sandbox requires permission
        /// from a website through a cross-domain policy file.You can prevent a SWF file from using this method by setting the
        /// allowNetworking parameter of the the object and embed
        /// tags in the HTML page that contains the SWF content.However, in the Adobe Integrated Runtime, content in the application security sandbox (content
        /// installed with the AIR application) are not restricted by these security limitations.For more information, see the following:The security chapter in the
        /// Programming ActionScript 3.0 book and the latest comments on LiveDocs
        /// </summary>
        /// <param name="stream">A URL that points to an external MP3 file.</param>
        /// <param name="context">
        /// (default = null)   Minimum number of milliseconds of MP3 data
        /// to hold in the Sound object&apos;s buffer.  The Sound object waits
        /// until it has at least this much data before beginning playback and
        /// before resuming playback after a network stall.  The default value
        /// is 1000 (one second).
        /// </param>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void load(flash.net.URLRequest stream, flash.media.SoundLoaderContext context);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void load(flash.net.URLRequest stream);

        /// <summary>
        /// Generates a new SoundChannel object to play back the sound. This method
        /// returns a SoundChannel object, which you access to stop the sound and to monitor volume.
        /// (To control the volume, panning, and balance, access the SoundTransform object assigned
        /// to the sound channel.)
        /// </summary>
        /// <param name="startTime">
        /// (default = 0)  The initial position in milliseconds at which playback should
        /// start.
        /// </param>
        /// <param name="loops">
        /// (default = 0)  Defines the number of times a sound loops before the sound
        /// channel stops playback.
        /// </param>
        /// <param name="sndTransform">(default = null)  The initial SoundTransform object assigned to the sound channel.</param>
        /// <returns>
        /// A SoundChannel object, which you use to control the sound.
        /// This method returns null if you have no sound card
        /// or if you run out of available sound channels. The maximum number of
        /// sound channels available at once is 32.
        /// </returns>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.media.SoundChannel play(double startTime, int loops, flash.media.SoundTransform sndTransform);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern flash.media.SoundChannel play(double startTime, int loops);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern flash.media.SoundChannel play(double startTime);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern flash.media.SoundChannel play();

        /// <summary>
        /// Closes the stream, causing any download of data to cease.
        /// No data may be read from the stream after the close()
        /// method is called.
        /// </summary>
        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void close();

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double extract(flash.utils.ByteArray target, double length, double startPosition);

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double extract(flash.utils.ByteArray target, double length);
    }
}
