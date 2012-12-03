using System;
using System.Runtime.CompilerServices;

namespace flash.media
{
    /// <summary>The Camera class is primarily for use with Flash Media Server, but can be used in a limited way without the server.</summary>
    [PageFX.AbcInstance(151)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class Camera : flash.events.EventDispatcher
    {
        /// <summary>
        /// A numeric value that specifies the amount of motion the camera is detecting. Values range from 0 (no motion is being detected) to
        /// 100 (a large amount of motion is being detected). The value of this property can help you determine if you need to pass a setting
        /// to the setMotionLevel() method.
        /// If the camera is available but is not yet being used because the
        /// Video.attachCamera() method has not been called, this property
        /// is set to -1.If you are streaming only uncompressed local video, this property is set only if you have assigned a function to the  event
        /// handler. Otherwise, it is undefined.
        /// </summary>
        public extern virtual double activityLevel
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// An integer that specifies the maximum amount of bandwidth the current outgoing video feed can use, in bytes.
        /// A value of 0 means that Flash video can use as much bandwidth as needed to maintain the desired frame quality.
        /// To set this property, use the setQuality() method.
        /// </summary>
        public extern virtual int bandwidth
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The rate at which the camera is capturing data, in frames per second.
        /// This property cannot be set; however, you can use the setMode() method
        /// to set a related property—fps—which specifies the maximum
        /// frame rate at which you would like the camera to capture data.
        /// </summary>
        public extern virtual double currentFPS
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The maximum rate at which you want the camera to capture data, in frames per second.
        /// The maximum rate possible depends on the capabilities of the camera; that is, if the
        /// camera doesn&apos;t support the value you set here, this frame rate will not be achieved.
        /// To set a desired value for this property, use the setMode() method.To determine the rate at which the camera is currently capturing data, use the currentFPS property.
        /// </summary>
        public extern virtual double fps
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The current capture height, in pixels. To set a value for this property,
        /// use the setMode() method.
        /// </summary>
        public extern virtual int height
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// A zero-based integer that specifies the index of the camera, as reflected in
        /// the array returned by the names property.
        /// </summary>
        public extern virtual int index
        {
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies which video frames are transmitted in full (called keyframes)
        /// instead of being interpolated by the video compression algorithm.
        /// The default value is 15, which means that every 15th frame is a keyframe.
        /// </summary>
        public extern virtual int keyFrameInterval
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether a local view of what the camera is capturing is compressed
        /// and decompressed (true), as it would be for live transmission using
        /// Flash Media Server, or uncompressed (false). The default value is
        /// false.
        /// To set this value, use Camera.setLoopback(). To set the amount of
        /// compression used when this property is true, use Camera.setQuality().
        /// </summary>
        public extern virtual bool loopback
        {
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// A numeric value that specifies the amount of motion required to invoke the activity event. Acceptable values range from 0 to 100.
        /// The default value is 50.
        /// Video can be displayed regardless of the value of the motionLevel property. For more information, see
        /// setMotionLevel().
        /// </summary>
        public extern virtual int motionLevel
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The number of milliseconds between the time the camera stops detecting motion and the time the activity event is invoked. The
        /// default value is 2000 (2 seconds).
        /// To set this value, use setMotionLevel().
        /// </summary>
        public extern virtual int motionTimeout
        {
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// A Boolean value that specifies whether the user has denied access to the camera
        /// (true) or allowed access (false) in the Flash Player Privacy panel.
        /// When this value changes, the statusevent is dispatched.
        /// </summary>
        public extern virtual bool muted
        {
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>A string that specifies the name of the current camera, as returned by the camera hardware.</summary>
        public extern virtual Avm.String name
        {
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// An integer specifying the required level of picture quality, as determined by the amount of compression being applied to each video
        /// frame. Acceptable quality values range from 1 (lowest quality, maximum compression) to 100 (highest quality, no compression). The
        /// default value is 0, which means that picture quality can vary as needed to avoid exceeding available bandwidth.
        /// To set this property, use the setQuality() method.
        /// </summary>
        public extern virtual int quality
        {
            [PageFX.AbcInstanceTrait(12)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The current capture width, in pixels. To set a desired value for this property,
        /// use the setMode() method.
        /// </summary>
        public extern virtual int width
        {
            [PageFX.AbcInstanceTrait(13)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Retrieves an array of strings reflecting the names of all available cameras
        /// without displaying the Flash Player Privacy panel. This array behaves in the
        /// same way as any other ActionScript array, implicitly providing the zero-based
        /// index of each camera and the number of cameras on the system (by means of
        /// names.length). For more information, see the names Array class entry.
        /// Calling the names property requires an extensive examination of the hardware,
        /// and it may take several seconds to build the array. In most cases, you can just use the
        /// default camera.
        /// </summary>
        public extern static Avm.Array names
        {
            [PageFX.AbcClassTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static bool isSupported
        {
            [PageFX.AbcClassTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [PageFX.Event("status")]
        public event flash.events.StatusEventHandler status
        {
            add { }
            remove { }
        }

        [PageFX.Event("activity")]
        public event flash.events.ActivityEventHandler activity
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Camera();

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setCursor(bool value);

        /// <summary>
        /// Specifies which video frames are transmitted in full (called keyframes)
        /// instead of being interpolated by the video compression algorithm. This method
        /// is generally applicable only if you are transmitting video using Flash Media Server.
        /// The Flash Video compression algorithm compresses video by transmitting
        /// only what has changed since the last frame of the video; these portions are
        /// considered to be interpolated frames. Frames of a video can be interpolated according
        /// to the contents of the previous frame. A keyframe, however, is a video frame that
        /// is complete; it is not interpolated from prior frames.To determine how to set a value for the keyFrameInterval parameter,
        /// consider both bandwidth use and video playback accessibility. For example,
        /// specifying a higher value for keyFrameInterval (sending keyframes less frequently)
        /// reduces bandwidth use.
        /// However, this may increase the amount of time required to position the playhead
        /// at a particular point in the video; more prior video frames may have to be interpolated
        /// before the video can resume.Conversely, specifying a lower value for keyFrameInterval
        /// (sending keyframes more frequently) increases bandwidth use because entire video frames
        /// are transmitted more often, but may decrease the amount of time required to seek a
        /// particular video frame within a recorded video.
        /// </summary>
        /// <param name="keyFrameInterval">
        /// A value that specifies which video frames are transmitted in full
        /// (as keyframes) instead of being interpolated by the video compression algorithm.
        /// A value of 1 means that every frame is a keyframe, a value of 3 means that every third frame
        /// is a keyframe, and so on. Acceptable values are 1 through 48.
        /// </param>
        [PageFX.AbcInstanceTrait(15)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setKeyFrameInterval(int keyFrameInterval);

        /// <summary>
        /// Specifies whether to use a compressed video stream for a local view of the camera.
        /// This method is generally applicable only if you are transmitting video using Flash Media Server;
        /// setting compress to true lets you see more precisely how the video
        /// will appear to users when they view it in real time.
        /// Although a compressed stream is useful for testing purposes, such as previewing video
        /// quality settings, it has a significant processing cost, because the local view is not
        /// simply compressed; it is compressed, edited for transmission as it would be over a live
        /// connection, and then decompressed for local viewing.To set the amount of compression used when you set compress to true,
        /// use Camera.setQuality().
        /// </summary>
        /// <param name="compress">
        /// (default = false)  Specifies whether to use a compressed video stream (true)
        /// or an uncompressed stream (false) for a local view of what the camera
        /// is receiving.
        /// </param>
        [PageFX.AbcInstanceTrait(16)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setLoopback(bool compress);

        [PageFX.AbcInstanceTrait(16)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void setLoopback();

        /// <summary>
        /// Sets the camera capture mode to the native mode that best meets the specified requirements.
        /// If the camera does not have a native mode that matches all the parameters you pass,
        /// Flash selects a capture mode that most closely synthesizes the requested mode.
        /// This manipulation may involve cropping the image and dropping frames.
        /// By default, Flash drops frames as needed to maintain image size. To minimize the number
        /// of dropped frames, even if this means reducing the size of the image, pass false
        /// for the favorArea parameter.When choosing a native mode, Flash tries to maintain the requested aspect ratio
        /// whenever possible. For example, if you issue the command myCam.setMode(400, 400, 30),
        /// and the maximum width and height values available on the camera are 320 and 288, Flash sets
        /// both the width and height at 288; by setting these properties to the same value, Flash
        /// maintains the 1:1 aspect ratio you requested.To determine the values assigned to these properties after Flash selects the mode
        /// that most closely matches your requested values, use the width, height,
        /// and fps properties.
        /// </summary>
        /// <param name="width">The requested capture width, in pixels. The default value is 160.</param>
        /// <param name="height">The requested capture height, in pixels. The default value is 120.</param>
        /// <param name="fps">
        /// The requested rate at which the camera should capture data, in frames per second.
        /// The default value is 15.
        /// </param>
        /// <param name="favorArea">
        /// (default = true)  Specifies whether to manipulate the width, height, and frame rate if
        /// the camera does not have a native mode that meets the specified requirements.
        /// The default value is true, which means that maintaining capture size
        /// is favored; using this parameter selects the mode that most closely matches
        /// width and height values, even if doing so adversely affects
        /// performance by reducing the frame rate. To maximize frame rate at the expense
        /// of camera height and width, pass false for the favorArea parameter.
        /// </param>
        [PageFX.AbcInstanceTrait(17)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setMode(int width, int height, double fps, bool favorArea);

        [PageFX.AbcInstanceTrait(17)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void setMode(int width, int height, double fps);

        /// <summary>
        /// Specifies how much motion is required to dispatch the activity event.
        /// Optionally sets the number of milliseconds that must elapse without activity before
        /// Flash considers motion to have stopped and dispatches the event.
        /// Note: Video can be displayed regardless of the value of the
        /// motionLevel parameter. This parameter determines only when and under
        /// what circumstances the event is dispatchednot whether video is actually being
        /// captured or displayed.To prevent the camera from detecting motion at all, pass a value of 100 for the
        /// motionLevel parameter; the activity event is never dispatched.
        /// (You would probably use
        /// this value only for testing purposesfor example, to temporarily disable any
        /// handlers that would normally be triggered when the event is dispatched.)To determine the amount of motion the camera is currently detecting, use the
        /// activityLevel property.
        /// Motion sensitivity values correspond directly to activity values.
        /// Complete lack of motion is an activity value of 0. Constant motion is an activity value of 100.
        /// Your activity value is less than your motion sensitivity value when you&apos;re not moving;
        /// when you are moving, activity values frequently exceed your motion sensitivity value.
        /// This method is similar in purpose to the Microphone.setSilenceLevel() method;
        /// both methods are used to specify when the activity event
        /// should be dispatched. However, these methods have a significantly different impact
        /// on publishing streams:Microphone.setSilenceLevel() is designed to optimize bandwidth.
        /// When an audio stream is considered silent, no audio data is sent. Instead, a single message
        /// is sent, indicating that silence has started. setMotionLevel() is designed to detect motion and does not affect
        /// bandwidth usage. Even if a video stream does not detect motion, video is still sent.
        /// </summary>
        /// <param name="motionLevel">
        /// Specifies the amount of motion required to dispatch the
        /// activity event. Acceptable values range from 0 to 100. The default value is 50.
        /// </param>
        /// <param name="timeout">
        /// (default = 2000)  Specifies how many milliseconds must elapse without activity
        /// before Flash considers activity to have stopped and dispatches the activity event.
        /// The default value is 2000 milliseconds (2 seconds).
        /// </param>
        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setMotionLevel(int motionLevel, int timeout);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void setMotionLevel(int motionLevel);

        /// <summary>
        /// Sets the maximum amount of bandwidth per second or the required picture quality
        /// of the current outgoing video feed. This method is generally applicable only if
        /// you are transmitting video using Flash Media Server.
        /// Use this method to specify which element of the outgoing video feed is more
        /// important to your applicationbandwidth use or picture quality.To indicate that bandwidth use takes precedence, pass a value for bandwidth
        /// and 0 for quality. Flash will transmit video at the highest quality
        /// possible within the specified bandwidth. If necessary, Flash will reduce picture
        /// quality to avoid exceeding the specified bandwidth. In general, as motion increases,
        /// quality decreases.To indicate that quality takes precedence, pass 0 for bandwidth
        /// and a numeric value for quality. Flash will use as much bandwidth
        /// as required to maintain the specified quality. If necessary, Flash will reduce the frame
        /// rate to maintain picture quality. In general, as motion increases, bandwidth use also
        /// increases.To specify that both bandwidth and quality are equally important, pass numeric
        /// values for both parameters. Flash will transmit video that achieves the specified quality
        /// and that doesn&apos;t exceed the specified bandwidth. If necessary, Flash will reduce the
        /// frame rate to maintain picture quality without exceeding the specified bandwidth.
        /// </summary>
        /// <param name="bandwidth">
        /// Specifies the maximum amount of bandwidth that the current outgoing video
        /// feed can use, in bytes per second. To specify that Flash video can use as much bandwidth
        /// as needed to maintain the value of quality, pass 0 for
        /// bandwidth. The default value is 16384.
        /// </param>
        /// <param name="quality">
        /// An integer that specifies the required level of picture quality,
        /// as determined by the amount of compression being applied to each video frame.
        /// Acceptable values range from 1 (lowest quality, maximum compression) to 100 (highest
        /// quality, no compression). To specify that picture quality can vary as needed to avoid
        /// exceeding bandwidth, pass 0 for quality.
        /// </param>
        [PageFX.AbcInstanceTrait(19)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setQuality(int bandwidth, int quality);

        /// <summary>
        /// Returns a reference to a Camera object for capturing video. To actually begin capturing
        /// the video, you must attach the Camera object to a Video object (see Video.attachVideo()
        /// ).
        /// Multiple calls to the getCamera() method reference the same camera.
        /// Thus, if your code contains code like firstCam:Camera = getCamera()
        /// and secondCam:Camera = getCamera(),
        /// both firstCam and secondCam reference the same camera,
        /// which is the user&apos;s default camera.In general, you shouldn&apos;t pass a value for the name parameter; simply use
        /// getCamera() to return a reference to the default camera. By means of the Camera
        /// settings panel (discussed later in this section), the user can specify the default camera
        /// Flash should use. When a SWF file tries to access the camera returned by getCamera(),
        /// Flash Player displays a dialog box that lets the user choose whether to allow
        /// or deny access to the camera. (Make sure your application window size is at least 215
        /// x 138 pixels; this is the minimum size Flash requires to display the dialog box.)
        /// When the user responds to this dialog box, Flash Player returns an information
        /// object in the status event that indicates the user&apos;s response:
        /// Camera.muted
        /// indicates the user denied access to a camera;
        /// Camera.unmuted indicates the user
        /// allowed access to a camera. To determine whether the user has denied or
        /// allowed access to the camera without handling the status event, use the muted
        /// property.The user can also specify permanent privacy settings for a particular domain by right-clicking
        /// (Windows) or Control-clicking (Macintosh) while a SWF file is playing, selecting Settings,
        /// opening the Privacy panel, and selecting Remember.You can&apos;t use ActionScript to set the Allow or Deny value for a user, but you can display
        /// the Privacy panel for the user by calling Security.showSettings(SecurityPanel.PRIVACY).
        /// If the user selects Remember, Flash Player no longer asks the user whether to allow or deny
        /// SWF files from this domain access to your camera.If getCamera() returns null, either the camera is in use by another
        /// application, or there are no cameras installed on the system. To determine whether any cameras
        /// are installed, use the names.length property. To display the Flash Player Camera Settings panel,
        /// which lets the user choose the camera to be referenced by getCamera(), use
        /// System.showSettings(SecurityPanel.CAMERA). Scanning the hardware for cameras takes time. When Flash finds at least one camera,
        /// the hardware is not scanned again for the lifetime of the player instance. However, if
        /// Flash doesn&apos;t find any cameras, it will scan each time getCamera is called.
        /// This is helpful if a user has forgotten to connect the camera; if your SWF file provides a
        /// Try Again button that calls getCamera, Flash can find the camera without the
        /// user having to restart the SWF file.
        /// </summary>
        /// <param name="name">
        /// (default = null)  Specifies which camera to get, as determined from the array
        /// returned by the names property. For most applications, get the default camera
        /// by omitting this parameter.
        /// </param>
        /// <returns>
        /// If the name parameter is not specified, this method returns a reference
        /// to the default camera or, if it is in use by another application, to the first
        /// available camera. (If there is more than one camera installed, the user may specify
        /// the default camera in the Flash Player Camera Settings panel.) If no cameras are available
        /// or installed, the method returns null.
        /// </returns>
        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.media.Camera getCamera(Avm.String name);

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.media.Camera getCamera();
    }
}
