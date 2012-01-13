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
    [PageFX.ABC]
    [PageFX.FP9]
    public class Sound : flash.events.EventDispatcher
    {
        public extern virtual Avm.String url
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual uint bytesLoaded
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double length
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual ID3Info id3
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual int bytesTotal
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool isBuffering
        {
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
        public extern Sound(flash.net.URLRequest arg0, SoundLoaderContext arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Sound(flash.net.URLRequest arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Sound();

        [PageFX.ABC]
        [PageFX.QName("extract", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double extract(flash.utils.ByteArray arg0, double arg1, double arg2);

        [PageFX.ABC]
        [PageFX.QName("extract", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double extract(flash.utils.ByteArray arg0, double arg1);

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
        /// <param name="arg0">A URL that points to an external MP3 file.</param>
        /// <param name="arg1">
        /// (default = null)   Minimum number of milliseconds of MP3 data
        /// to hold in the Sound object&apos;s buffer.  The Sound object waits
        /// until it has at least this much data before beginning playback and
        /// before resuming playback after a network stall.  The default value
        /// is 1000 (one second).
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void load(flash.net.URLRequest arg0, SoundLoaderContext arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void load(flash.net.URLRequest arg0);

        /// <summary>
        /// Closes the stream, causing any download of data to cease.
        /// No data may be read from the stream after the close()
        /// method is called.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void close();

        /// <summary>
        /// Generates a new SoundChannel object to play back the sound. This method
        /// returns a SoundChannel object, which you access to stop the sound and to monitor volume.
        /// (To control the volume, panning, and balance, access the SoundTransform object assigned
        /// to the sound channel.)
        /// </summary>
        /// <param name="arg0">
        /// (default = 0)  The initial position in milliseconds at which playback should
        /// start.
        /// </param>
        /// <param name="arg1">
        /// (default = 0)  Defines the number of times a sound loops before the sound
        /// channel stops playback.
        /// </param>
        /// <param name="arg2">(default = null)  The initial SoundTransform object assigned to the sound channel.</param>
        /// <returns>
        /// A SoundChannel object, which you use to control the sound.
        /// This method returns null if you have no sound card
        /// or if you run out of available sound channels. The maximum number of
        /// sound channels available at once is 32.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual SoundChannel play(double arg0, int arg1, SoundTransform arg2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SoundChannel play(double arg0, int arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SoundChannel play(double arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SoundChannel play();


    }
}
