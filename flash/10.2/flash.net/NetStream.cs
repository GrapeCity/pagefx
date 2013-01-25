using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// The NetStream class opens a one-way streaming connection between Flash Player and
    /// a server, such as Adobe&apos;s Macromedia Flash Media Server 2 or Adobe Flex, or between
    /// Flash Player and the local file system through a connection made available by a
    /// NetConnection object. A NetStream object is like a channel inside a NetConnection
    /// object; this channel can either publish audio and/or video data, using NetStream.publish() ,
    /// or subscribe to a published stream and receive data, using NetStream.play() .
    /// You can publish or play live (real-time) data and previously recorded data. You
    /// can also use NetStream objects to send text messages to all subscribed clients (see
    /// the NetStream.send()  method).
    /// </summary>
    [PageFX.AbcInstance(107)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class NetStream : flash.events.EventDispatcher
    {
        [PageFX.AbcClassTrait(36)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String DIRECT_CONNECTIONS;

        [PageFX.AbcClassTrait(37)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String CONNECT_TO_FMS;

        /// <summary>
        /// Specifies how long to buffer messages before starting to display the stream. For
        /// example, to make sure that the first 15 seconds of the stream play without interruption,
        /// set bufferTime to 15; Flash Player begins playing the stream only after
        /// 15 seconds of data are buffered.
        /// The default value is 0.1 (one-tenth of a second). To determine the number of seconds
        /// currently in the buffer, use the bufferLength property.Note: To avoid distortion when streaming pre-recorded (not live) content,
        /// do not set the value of Netstream.bufferTime to 0. By default, Flash
        /// Player uses an input buffer for pre-recorded content that queues the media data
        /// and plays the media properly. For pre-recorded content use the default setting or
        /// increase the buffer time.
        /// </summary>
        public extern virtual double bufferTime
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

        public extern virtual double maxPauseBufferTime
        {
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double backBufferTime
        {
            [PageFX.AbcInstanceTrait(12)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(13)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool inBufferSeek
        {
            [PageFX.AbcInstanceTrait(14)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(15)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double backBufferLength
        {
            [PageFX.AbcInstanceTrait(16)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double bufferTimeMax
        {
            [PageFX.AbcInstanceTrait(19)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(20)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual flash.net.NetStreamInfo info
        {
            [PageFX.AbcInstanceTrait(30)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual flash.net.NetStreamMulticastInfo multicastInfo
        {
            [PageFX.AbcInstanceTrait(31)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>The position of the playhead, in seconds.</summary>
        public extern virtual double time
        {
            [PageFX.AbcInstanceTrait(35)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The number of frames per second being displayed. If you are exporting FLV files
        /// to be played back on a number of systems, you can check this value during testing
        /// to help you determine how much compression to apply when exporting the file.
        /// </summary>
        public extern virtual double currentFPS
        {
            [PageFX.AbcInstanceTrait(36)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The number of seconds of data currently in the buffer. You can use this property
        /// with the bufferTime property to estimate how close the buffer is to
        /// being full — for example, to display feedback to a user who is waiting for
        /// data to be loaded into the buffer.
        /// </summary>
        public extern virtual double bufferLength
        {
            [PageFX.AbcInstanceTrait(37)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The number of seconds of data in the subscribing stream&apos;s buffer in live (unbuffered)
        /// mode. This property specifies the current network transmission delay (lag time).
        /// This property is intended primarily for use with a server, such as Flash Media Server;
        /// for more information, see the class description.
        /// You can get the value of this property to roughly gauge the transmission quality
        /// of the stream and communicate it to the user.
        /// </summary>
        public extern virtual double liveDelay
        {
            [PageFX.AbcInstanceTrait(38)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The number of bytes of data that have been loaded into the player. You can use this
        /// property with the bytesTotal property to estimate how close the buffer
        /// is to being full — for example, to display feedback to a user who is waiting
        /// for data to be loaded into the buffer.
        /// </summary>
        public extern virtual uint bytesLoaded
        {
            [PageFX.AbcInstanceTrait(39)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>The total size in bytes of the file being loaded into the player.</summary>
        public extern virtual uint bytesTotal
        {
            [PageFX.AbcInstanceTrait(40)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual uint decodedFrames
        {
            [PageFX.AbcInstanceTrait(41)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual uint videoCodec
        {
            [PageFX.AbcInstanceTrait(42)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual uint audioCodec
        {
            [PageFX.AbcInstanceTrait(43)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Controls sound in this NetStream object. For more information, see the SoundTransform
        /// class.
        /// </summary>
        public extern virtual flash.media.SoundTransform soundTransform
        {
            [PageFX.AbcInstanceTrait(44)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(45)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Specifies whether Flash Player should try to download a cross-domain policy file
        /// from the loaded FLV file&apos;s server before beginning to load the FLV file. This property
        /// applies when you are using a NetStream object for progressive video download (standalone
        /// FLV files), or when you are loading FLV files that are outside the calling SWF file&apos;s
        /// own domain. This property is ignored when you are using a NetStream object to get
        /// an RTMP asset.
        /// Set this property to true when you are loading an FLV file from outside
        /// the calling SWF file&apos;s domain and you need to use the BitmapData.draw()
        /// method for pixel-level access to the video. If you call BitmapData.draw()
        /// without setting the checkPolicyFile property to true at
        /// loading time, you may get a SecurityError exception because the required
        /// policy file was not downloaded.
        /// If you don&apos;t need pixel-level access to the video you are loading, avoid setting
        /// checkPolicyFile to true. Checking for a policy file consumes
        /// network bandwidth and may delay the start of your download.
        /// When you call the NetStream.play() method with checkPolicyFile
        /// set to true, Flash Player must either successfully download a relevant
        /// cross-domain policy file or determine that no such policy file exists before it
        /// begins downloading the object specified in your call to NetStream.play().
        /// To verify the existence of a policy file, Flash Player performs the following actions,
        /// in this order:Flash Player considers policy files that have already been downloaded.Flash Player tries to download any pending policy files specified in calls to the
        /// Security.loadPolicyFile() method.Flash Player tries to download a policy file from the default location that corresponds
        /// to the URL you passed to NetStream.play(), which is /crossdomain.xml
        /// on the same server as that URL.
        /// In all cases, Flash Player requires that an appropriate policy file exist on the
        /// video&apos;s server, that it provide access to the object at the URL you passed to
        /// play() based on the policy file&apos;s location, and that it allow the domain
        /// of the calling SWF to access the video, through one or more &lt;allow-access-from&gt;
        /// tags.
        /// If you set checkPolicyFile to true, Flash Player waits
        /// until the policy file is verified before downloading the video. Wait to perform
        /// any pixel-level operations on the video data, such as calling BitmapData.draw(),
        /// until you receive onMetaData or NetStatus events from
        /// your NetStream object.
        /// If you set checkPolicyFile to true but no relevant policy
        /// file is found, you won&apos;t receive an error until you perform an operation that requires
        /// a policy file, and then Flash Player throws a SecurityError exception.
        /// Be careful with checkPolicyFile if you are downloading an FLV file
        /// from a URL that uses server-side HTTP redirects. Flash Player tries to retrieve
        /// policy files that correspond to the initial URL that you specify in NetStream.play().
        /// If the final FLV file comes from a different URL because of HTTP redirects, the
        /// initially downloaded policy files might not be applicable to the FLV file&apos;s final
        /// URL, which is the URL that matters in security decisions.
        /// For more information on policy files, see the &quot;Flash Player Security&quot; chapter of
        /// Programming ActionScript 3.0.
        /// </summary>
        public extern virtual bool checkPolicyFile
        {
            [PageFX.AbcInstanceTrait(46)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(47)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Specifies the object on which callback methods are invoked. The default object is
        /// this, the NetStream object being created. If you set the client
        /// property to another object, callback methods will be invoked on that other object.
        /// </summary>
        public extern virtual object client
        {
            [PageFX.AbcInstanceTrait(48)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(49)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The object encoding (AMF version) for this NetStream object. The NetStream object
        /// inherits its objectEncoding value from the associated NetConnection
        /// object. It&apos;s important to understand this property if your ActionScript 3.0 SWF
        /// file needs to communicate with servers released prior to Flash Player 9. For more
        /// information, see the objectEncoding property description in the NetConnection
        /// class.
        /// The value of this property depends on whether the stream is local or remote. Local
        /// streams, where null was passed to the NetConnection.connect()
        /// method, return the value of NetConnection.defaultObjectEncoding. Remote
        /// streams, where you are connecting to a server, return the object encoding of the
        /// connection to the server.
        /// If you try to read this property when not connected, or if you try to change this
        /// property, Flash Player throws an exception.
        /// </summary>
        public extern virtual uint objectEncoding
        {
            [PageFX.AbcInstanceTrait(50)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double multicastPushNeighborLimit
        {
            [PageFX.AbcInstanceTrait(51)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(52)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double multicastWindowDuration
        {
            [PageFX.AbcInstanceTrait(53)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(54)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double multicastRelayMarginDuration
        {
            [PageFX.AbcInstanceTrait(55)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(56)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double multicastAvailabilityUpdatePeriod
        {
            [PageFX.AbcInstanceTrait(57)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(58)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double multicastFetchPeriod
        {
            [PageFX.AbcInstanceTrait(59)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(60)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool multicastAvailabilitySendToAll
        {
            [PageFX.AbcInstanceTrait(61)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(62)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String farID
        {
            [PageFX.AbcInstanceTrait(63)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String nearNonce
        {
            [PageFX.AbcInstanceTrait(64)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String farNonce
        {
            [PageFX.AbcInstanceTrait(65)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.Array peerStreams
        {
            [PageFX.AbcInstanceTrait(66)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool audioReliable
        {
            [PageFX.AbcInstanceTrait(68)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(69)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool videoReliable
        {
            [PageFX.AbcInstanceTrait(70)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(71)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool dataReliable
        {
            [PageFX.AbcInstanceTrait(72)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(73)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool audioSampleAccess
        {
            [PageFX.AbcInstanceTrait(74)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(75)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool videoSampleAccess
        {
            [PageFX.AbcInstanceTrait(76)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(77)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [PageFX.Event("onSeekPoint")]
        public event flash.events.EventHandler onSeekPoint
        {
            add { }
            remove { }
        }

        [PageFX.Event("drmStatus")]
        public event flash.events.DRMStatusEventHandler drmStatus
        {
            add { }
            remove { }
        }

        [PageFX.Event("drmError")]
        public event flash.events.DRMErrorEventHandler drmError
        {
            add { }
            remove { }
        }

        [PageFX.Event("drmAuthenticate")]
        public event flash.events.DRMAuthenticateEventHandler drmAuthenticate
        {
            add { }
            remove { }
        }

        [PageFX.Event("onDRMContentData")]
        public event flash.events.EventHandler onDRMContentData
        {
            add { }
            remove { }
        }

        [PageFX.Event("onPlayStatus")]
        public event flash.events.EventHandler onPlayStatus
        {
            add { }
            remove { }
        }

        [PageFX.Event("onCuePoint")]
        public event flash.events.EventHandler onCuePoint
        {
            add { }
            remove { }
        }

        [PageFX.Event("onTextData")]
        public event flash.events.EventHandler onTextData
        {
            add { }
            remove { }
        }

        [PageFX.Event("onImageData")]
        public event flash.events.EventHandler onImageData
        {
            add { }
            remove { }
        }

        [PageFX.Event("onMetaData")]
        public event flash.events.EventHandler onMetaData
        {
            add { }
            remove { }
        }

        [PageFX.Event("onXMPData")]
        public event flash.events.EventHandler onXMPData
        {
            add { }
            remove { }
        }

        [PageFX.Event("netStatus")]
        public event flash.events.NetStatusEventHandler netStatus
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

        [PageFX.Event("asyncError")]
        public event flash.events.AsyncErrorEventHandler asyncError
        {
            add { }
            remove { }
        }

        [PageFX.Event("status")]
        public event flash.events.StatusEventHandler status
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetStream(flash.net.NetConnection connection, Avm.String peerID);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NetStream(flash.net.NetConnection connection);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void attach(flash.net.NetConnection connection);

        /// <summary>
        /// Stops playing all data on the stream, sets the time property to 0,
        /// and makes the stream available for another use. This command also deletes the local
        /// copy of an FLV file that was downloaded through HTTP. Although Flash Player deletes
        /// the local copy of the FLV file that it creates, a copy of the video might persist
        /// in the browser&apos;s cache directory. If you must completely prevent caching or local
        /// storage of the FLV file, use Flash Media Server 2.
        /// </summary>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void close();

        /// <summary>
        /// Specifies an audio stream sent over the NetStream object, from a Microphone object
        /// passed as the source. This method is available only to the publisher of the specified
        /// stream.
        /// This method is intended primarily for use with a server, such as Flash Media Server;
        /// for more information, see the class description.
        /// You can call this method before or after you call the publish() method
        /// and actually begin transmitting. Subscribers who want to hear the audio must call
        /// the NetStream.play() method. You can control the sound properties of
        /// this audio stream through the soundTransform property of the specified
        /// Microphone object.
        /// </summary>
        /// <param name="microphone">The source of the audio stream to be transmitted.</param>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void attachAudio(flash.media.Microphone microphone);

        /// <summary>
        /// Starts capturing video from the specified source, or stops capturing if theCamera
        /// is set to null. This method is available only to the publisher of the
        /// specified stream.
        /// This method is intended primarily for use with a server, such as Flash Media Server;
        /// for more information, see the class description.
        /// After attaching the video source, you must call NetStream.publish()
        /// to begin transmitting. Subscribers who want to display the video must call the
        /// NetStream.play() and Video.attachCamera() methods to display
        /// the video on the Stage.
        /// You can use snapshotMilliseconds to send a single snapshot (by providing
        /// a value of 0) or a series of snapshots — in effect, time-lapse footage —
        /// by providing a positive number that adds a trailer of the specified number of milliseconds
        /// to the video feed. The trailer extends the display time of time the video message
        /// is displayed. By repeatedly calling attachCamera() with a positive
        /// value for snapshotMilliseconds, the sequence of alternating snapshots
        /// and trailers create time-lapse footage. For example, you could capture one frame
        /// per day and append it to a video file. When a subscriber plays the file, each frame
        /// remains onscreen for the specified number of milliseconds and then the next frame
        /// is displayed.
        /// The purpose of the snapshotMilliseconds parameter is different from
        /// the fps parameter you can set with Camera.setMode(). When
        /// you specify snapshotMilliseconds, you control how much time elapses
        /// between recorded frames. When you specify fps using Camera.setMode(),
        /// you are controlling how much time elapses during recording and playback.
        /// For example, suppose you want to take a snapshot every 5 minutes for a total of
        /// 100 snapshots. You can do this in two ways:You can issue a NetStream.attachCamera(myCamera, 500) command 100 times,
        /// once every 5 minutes. This takes 500 minutes to record, but the resulting file will
        /// play back in 50 seconds (100 frames with 500 milliseconds between frames).You can issue a Camera.setMode() command with an fps value
        /// of 1/300 (one per 300 seconds, or one every 5 minutes), and then issue a NetStream.attachCamera(source)
        /// command, letting the camera capture continuously for 500 minutes. The resulting
        /// file will play back in 500 minutes — the same length of time that it took
        /// to record — with each frame being displayed for 5 minutes.
        /// Both techniques capture the same 500 frames, and both approaches are useful; the
        /// approach to use depends primarily on your playback requirements. For example, in
        /// the second case, you could be recording audio the entire time. Also, both files
        /// would be approximately the same size.
        /// </summary>
        /// <param name="theCamera">
        /// The source of the video transmission. Valid values are a Camera object (which
        /// starts capturing video) and null. If you pass null, Flash
        /// Player stops capturing video, and any additional parameters you send are ignored.
        /// </param>
        /// <param name="snapshotMilliseconds">
        /// (default = -1)  Specifies whether the video stream
        /// is continuous, a single frame, or a series of single frames used to create time-lapse
        /// photography.
        /// If you omit this parameter, Flash Player captures all video until you pass a value
        /// of null to attachCamera.If you pass 0, Flash Player captures only a single video frame. Use this value to
        /// transmit &quot;snapshots&quot; within a preexisting stream. Flash interprets invalid, negative,
        /// or nonnumeric arguments as 0.If you pass a positive number, Flash captures a single video frame and then appends
        /// a pause of the specified length as a trailer on the snapshot. Use this value to
        /// create time-lapse photography effects.
        /// </param>
        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void attachCamera(flash.media.Camera theCamera, int snapshotMilliseconds);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void attachCamera(flash.media.Camera theCamera);

        /// <summary>
        /// Sends a message on the specified stream to all subscribing clients. This method
        /// is available only to the publisher of the specified stream.
        /// This method is intended primarily for use with a server, such as Flash Media Server
        /// 2; for more information, see the class description.
        /// To process and respond to the message, create a handler in the format myStream.HandlerName.
        /// Flash Player does not serialize methods or their data, object prototype variables,
        /// or non-enumerable variables. For display objects, Flash Player serializes the path
        /// but none of the data.
        /// </summary>
        /// <param name="handlerName">
        /// The message to be sent; also the name of the ActionScript handler to receive
        /// the message. The handler name can be only one level deep (that is, it can&apos;t be of
        /// the form parent/child) and is relative to the stream object. Do not use a reserved
        /// term for a handler name. For example, using &quot;close&quot; as a handler name
        /// will cause the method to fail.
        /// </param>
        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void send(Avm.String handlerName);

        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(Avm.String handlerName, object rest0);

        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(Avm.String handlerName, object rest0, object rest1);

        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(Avm.String handlerName, object rest0, object rest1, object rest2);

        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(Avm.String handlerName, object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(Avm.String handlerName, object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(Avm.String handlerName, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(Avm.String handlerName, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(Avm.String handlerName, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(Avm.String handlerName, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(Avm.String handlerName, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        [PageFX.AbcInstanceTrait(17)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void step(int frames);

        /// <summary>
        /// Specifies whether incoming audio plays on the stream. This method is available only
        /// to clients subscribed to the specified stream, not to the stream&apos;s publisher.
        /// This method is intended primarily for use with a server, such as Flash Media Server;
        /// for more information, see the class description.
        /// You can call this method before or after you call the NetStream.play()
        /// method and actually begin receiving the stream. For example, you can attach these
        /// methods to a button the user clicks to mute and unmute the incoming audio stream.
        /// If the specified stream contains only audio data, passing a value of false
        /// to this method stops NetStream.time from further incrementing.
        /// </summary>
        /// <param name="flag">
        /// Specifies whether incoming audio plays on the specified stream (true)
        /// or not (false). The default value is true.
        /// </param>
        [PageFX.AbcInstanceTrait(21)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void receiveAudio(bool flag);

        /// <summary>
        /// Specifies whether incoming video will play on the stream. This method is available
        /// only to clients subscribed to the specified stream, not to the stream&apos;s publisher.
        /// This method is intended primarily for use with a server, such as Flash Media Server
        /// 2; for more information, see the class description.
        /// You can call this method before or after you call the NetStream.play()
        /// method and actually begin receiving the stream. For example, you can attach these
        /// methods to a button the user presses to show or hide the incoming video stream.
        /// If the specified stream contains only video data, passing a value of false
        /// to this method stops NetStream.time from further incrementing.
        /// </summary>
        /// <param name="flag">
        /// Specifies whether incoming video plays on the specified stream (true)
        /// or not (false). The default value is true.
        /// </param>
        [PageFX.AbcInstanceTrait(22)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void receiveVideo(bool flag);

        [PageFX.AbcInstanceTrait(23)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void receiveVideoFPS(double FPS);

        /// <summary>
        /// Pauses playback of a video stream. Calling this method does nothing if the video
        /// is already paused.
        /// </summary>
        [PageFX.AbcInstanceTrait(24)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void pause();

        /// <summary>
        /// Resumes playback of a video stream that is paused. If the video is already playing,
        /// calling this method does nothing.
        /// </summary>
        [PageFX.AbcInstanceTrait(25)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void resume();

        /// <summary>
        /// Pauses or resumes playback of a stream. The first time you call this method, it
        /// pauses play; the next time, it resumes play. You could use this method to let users
        /// pause or resume playback by pressing a single button.
        /// </summary>
        [PageFX.AbcInstanceTrait(26)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void togglePause();

        /// <summary>
        /// Seeks the keyframe closest to the specified location (an offset, in seconds, from
        /// the beginning of the stream). The stream resumes playing when that location is reached.
        /// </summary>
        /// <param name="offset">
        /// The approximate time value, in seconds, to move to in an FLV file. The playhead
        /// moves to the video keyframe that&apos;s closest to offset.
        /// To return to the beginning of the stream, pass 0 for offset.To seek forward from the beginning of the stream, pass the number of seconds to
        /// advance. For example, to position the playhead at 15 seconds from the beginning,
        /// use myStream.seek(15).To seek relative to the current position, pass NetStream.time+
        /// n or NetStream.time - n to seek
        /// n seconds forward or backward, respectively, from the current
        /// position. For example, to rewind 20 seconds from the current position, use NetStream.seek(NetStream.time
        /// - 20).
        /// The precise location to which a video seeks depends on the frames per second (fps)
        /// setting at which it was exported. Therefore, if you export the same video at 6 fps
        /// and 30 fps, and you use myStream.seek(15) for both video objects, the
        /// videos seek to two different locations.
        /// </param>
        [PageFX.AbcInstanceTrait(27)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void seek(double offset);

        /// <summary>
        /// Begins playback of external audio or a video (FLV) file. To view video data, you
        /// must create a Video object and call the Video.attachNetStream() method;
        /// audio being streamed with the video, or an FLV file that contains only audio, is
        /// played automatically. To stream audio from a microphone, use the NetStream.attachAudio()
        /// method and control some aspects of the audio through the Microphone object.
        /// To control the audio associated with an FLV file, you can use the DisplayObjectContainer.addChild()
        /// method to route the audio to an object on the display list; you can then create
        /// a Sound object to control some aspects of the audio. For more information, see the
        /// DisplayObjectContainer.addChild() method.
        /// If the FLV file can&apos;t be found, the netStatus event is dispatched.
        /// To stop a stream that is currently playing, use the close() method.
        /// When you use this method, consider the Flash Player security model. A SWF file in
        /// the local-trusted or local-with-networking sandbox can load and play an FLV file
        /// from the remote sandbox, but cannot access that remoteFLV file&apos;s data without explicit
        /// permission in the form of a cross-domain policy file. Also, you can prevent a SWF
        /// file from using this method by setting the allowNetworking parameter
        /// of the the object and embed tags in the HTML page that
        /// contains the SWF content.
        /// For more information, see the following:The security
        /// chapter in the Programming ActionScript 3.0 book and the latest comments
        /// on LiveDocsThe Netstream.checkPolicyFile property.
        /// </summary>
        [PageFX.AbcInstanceTrait(28)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void play();

        [PageFX.AbcInstanceTrait(28)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void play(object rest0);

        [PageFX.AbcInstanceTrait(28)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void play(object rest0, object rest1);

        [PageFX.AbcInstanceTrait(28)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void play(object rest0, object rest1, object rest2);

        [PageFX.AbcInstanceTrait(28)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void play(object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcInstanceTrait(28)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void play(object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcInstanceTrait(28)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void play(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcInstanceTrait(28)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void play(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcInstanceTrait(28)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void play(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcInstanceTrait(28)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void play(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcInstanceTrait(28)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void play(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        [PageFX.AbcInstanceTrait(29)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void play2(flash.net.NetStreamPlayOptions param);

        /// <summary>
        /// Sends streaming audio, video, and text messages from a client to a server, such
        /// as Flash Media Server 2, optionally recording the stream during transmission. This
        /// method is available only to the publisher of the specified stream.
        /// This method is intended primarily for use with a server, such as Flash Media Server
        /// 2; for more information, see the class description.
        /// Don&apos;t use this method to let a client play a stream that has already been published
        /// and recorded. Instead, create a NetStream instance for that client and call the
        /// play() method:
        /// var subscribeNS:NetStream = new NetStream(myNetConnection);
        /// subscribeNS.play(&quot;streamToPlay&quot;);
        /// With Flash Media Server 2, when you record a stream, Flash creates an FLV file and
        /// stores it in a subdirectory of the applications directory on the server. Each stream
        /// is stored in a directory whose name matches the application instance name value
        /// passed to the command parameter of the NetConnection.connect()
        /// method. Flash Player creates these directories automatically; you don&apos;t have to
        /// create one for each instance name. For example, the following code shows how you
        /// would connect to a specific instance of an application that is stored in a directory
        /// named lectureSeries in your applications directory. A file named lecture.flv is
        /// stored in a subdirectory named /yourAppsFolder/lectureSeries/streams/Monday:
        /// var myNC:NetConnection = new NetConnection();
        /// myNC.connect(&quot;rtmp://server.domain.com/lectureSeries/Monday&quot;);
        /// var myNS:NetStream = new NetStream(myNC);
        /// myNS.publish(&quot;lecture&quot;, &quot;record&quot;);
        /// The following example shows how to connect to a different instance of the same application
        /// but issue an identical publish command. A file named lecture.flv is stored in a
        /// subdirectory named /yourAppsFolder/lectureSeries/streams/Tuesday.
        /// var myNC:NetConnection = new NetConnection();
        /// myNC.connect(&quot;rtmp://server.domain.com/lectureSeries/Tuesday&quot;);
        /// var myNS:NetStream = new NetStream(my_nc);
        /// myNS.publish(&quot;lecture&quot;, &quot;record&quot;);
        /// If you don&apos;t pass a value for the instance name, that matches the value passed to
        /// the name property is stored in a subdirectory named /yourAppsFolder/appName/streams/_definst_
        /// (for &quot;default instance&quot;). For more information on using instance names, see the
        /// NetConnection.connect() method. For information on playing back FLV
        /// files, see the NetStream.play() method.
        /// This method can dispatch a netStatus event with several different information
        /// objects. For example, if someone is already publishing on a stream with the specified
        /// name, the netStatus event is dispatched with a code property of NetStream.Publish.BadName.
        /// For more information, see the netStatus event.
        /// </summary>
        /// <param name="name">
        /// (default = null)  A string that identifies the
        /// stream. If you pass false, the publish operation stops. Clients that
        /// subscribe to this stream must pass this same name when they call NetStream.play().
        /// You don&apos;t need to include a file extension for the stream name.
        /// </param>
        /// <param name="type">
        /// (default = null)  A string that specifies how to
        /// publish the stream. Valid values are &quot;record&quot;, &quot;append&quot;,
        /// and &quot;live&quot;. The default value is &quot;live&quot;.
        /// If you pass &quot;record&quot;, Flash Player publishes and records live data,
        /// saving the recorded data to a new FLV file with a name matching the value passed
        /// to the name parameter. The file is stored on the server in a subdirectory
        /// within the directory that contains the server application. If the file already exists,
        /// it is overwritten.If you pass &quot;append&quot;, Flash Player publishes and records live data,
        /// appending the recorded data to an FLV file with a name that matches the value passed
        /// to the name parameter, stored on the server in a subdirectory within
        /// the directory that contains the server application. If no file with a matching name
        /// the name parameter is found, it is created. If you omit this parameter or pass &quot;live&quot;, Flash Player publishes live
        /// data without recording it. If a file with a name that matches the value passed to
        /// the name parameter exists, it is deleted.
        /// </param>
        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void publish(Avm.String name, Avm.String type);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void publish(Avm.String name);

        [PageFX.AbcInstanceTrait(32)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void publish();

        [PageFX.AbcInstanceTrait(67)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool onPeerConnect(flash.net.NetStream subscriber);

        [PageFX.AbcInstanceTrait(78)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void appendBytes(flash.utils.ByteArray bytes);

        [PageFX.AbcInstanceTrait(79)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void appendBytesAction(Avm.String netStreamAppendBytesAction);
    }
}
