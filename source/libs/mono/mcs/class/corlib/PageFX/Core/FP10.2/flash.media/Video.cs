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
    [PageFX.AbcInstance(292)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class Video : flash.display.DisplayObject
    {
        /// <summary>
        /// Indicates the type of filter applied to decoded video as part of post-processing.
        /// The default value is 0, which lets the video compressor apply a deblocking filter as needed.
        /// Compression of video can result in undesired artifacts. You can use the
        /// deblocking property to set filters that reduce blocking and,
        /// for video compressed using the On2 codec, ringing.Blocking refers to visible imperfections between the boundaries
        /// of the blocks that compose each video frame. Ringing refers to distorted
        /// edges around elements within a video image.Two deblocking filters are available: one in the Sorenson codec and one in the On2 VP6 codec.
        /// In addition, a deringing filter is available when you use the On2 VP6 codec.
        /// To set a filter, use one of the following values:0—Lets the video compressor apply the deblocking filter as needed.1—Does not use a deblocking filter.2—Uses the Sorenson deblocking filter.3—For On2 video only, uses the On2 deblocking filter but no deringing filter.4—For On2 video only, uses the On2 deblocking and deringing filter.5—For On2 video only, uses the On2 deblocking and a higher-performance
        /// On2 deringing filter.If a value greater than 2 is selected for video when you are using
        /// the Sorenson codec, the Sorenson decoder defaults to 2.Using a deblocking filter has an effect on overall playback performance, and it is usually
        /// not necessary for high-bandwidth video. If a user&apos;s system is not powerful enough,
        /// the user may experience difficulties playing back video with a deblocking filter enabled.
        /// </summary>
        public extern virtual int deblocking
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

        /// <summary>
        /// Specifies whether the video should be smoothed (interpolated) when it is scaled. For
        /// smoothing to work, the player must be in high quality mode. The default value
        /// is false (no smoothing).
        /// </summary>
        public extern virtual bool smoothing
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// An integer specifying the width of the video stream, in pixels. For live streams, this
        /// value is the same as the Camera.width
        /// property of the Camera object that is capturing the video stream. For FLV files, this
        /// value is the width of the file that was exported as
        /// an FLV file.
        /// You may want to use this property, for example, to ensure that the user is seeing the
        /// video at the same size at which it was captured,
        /// regardless of the actual size of the Video object on the Stage.
        /// </summary>
        public extern virtual int videoWidth
        {
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// An integer specifying the height of the video stream, in pixels. For live streams, this
        /// value is the same as the Camera.height
        /// property of the Camera object that is capturing the video stream. For FLV files, this
        /// value is the height of the file that was exported as FLV.
        /// You may want to use this property, for example, to ensure that the user is seeing the
        /// video at the same size at which it was captured,
        /// regardless of the actual size of the Video object on the Stage.
        /// </summary>
        public extern virtual int videoHeight
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Video(int width, int height);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Video(int width);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Video();

        /// <summary>
        /// Clears the image currently displayed in the Video object. This is useful when
        /// you want to display standby
        /// information without having to hide the Video object.
        /// </summary>
        [PageFX.AbcInstanceTrait(7)]
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
        /// <param name="netStream">
        /// A NetStream object. To drop the connection to the Video object, pass
        /// null.
        /// </param>
        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void attachNetStream(flash.net.NetStream netStream);

        /// <summary>
        /// Specifies a video stream from a camera to be displayed
        /// within the boundaries of the Video object in the application window.
        /// </summary>
        /// <param name="camera">
        /// A Camera object that is capturing video data. To drop the connection to the
        /// Video object, pass null.
        /// </param>
        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void attachCamera(flash.media.Camera camera);
    }
}
