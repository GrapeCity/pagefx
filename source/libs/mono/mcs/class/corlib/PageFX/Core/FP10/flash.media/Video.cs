using System;
using System.Runtime.CompilerServices;

namespace flash.media
{
    /// <summary>
    /// The Video class enables you to display live streaming video in an application without embedding
    /// it in your SWF file. You can capture and play live video using the Camera.getCamera()  method.
    /// You can also use the Video class to play back Flash ®  Video (FLV) files over HTTP
    /// or from the local file system.
    /// For more information, see the NetConnection class and NetStream class entries.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class Video : flash.display.DisplayObject
    {
        public extern virtual int videoHeight
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool smoothing
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

        public extern virtual int deblocking
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

        public extern virtual int videoWidth
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Video(int arg0, int arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Video(int arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Video();

        /// <summary>
        /// Specifies a video stream from a camera to be displayed
        /// within the boundaries of the Video object in the application window.
        /// </summary>
        /// <param name="arg0">
        /// A Camera object that is capturing video data. To drop the connection to the
        /// Video object, pass null.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void attachCamera(Camera arg0);

        /// <summary>
        /// Clears the image currently displayed in the Video object. This is useful when
        /// you want to display standby
        /// information without having to hide the Video object.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void clear();

        /// <summary>
        /// Specifies a video stream to be displayed within the boundaries of the Video object
        /// in the application window.
        /// The video stream is either an FLV file being displayed by means of the
        /// NetStream.play() command, a Camera object, or
        /// null. If the value of the netStream parameter is
        /// null, video is no longer played within the object.
        /// You don&apos;t need to use this method if the FLV file contains only audio; the audio
        /// portion of FLV files is played automatically
        /// when the NetStream.play() method is called. To control the audio
        /// associated with an FLV file, use the soundTransform property
        /// of the NetStream object that plays the FLV file.
        /// </summary>
        /// <param name="arg0">
        /// A NetStream object. To drop the connection to the Video object, pass
        /// null.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void attachNetStream(flash.net.NetStream arg0);
    }
}
