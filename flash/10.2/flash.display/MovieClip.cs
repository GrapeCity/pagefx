using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The MovieClip class inherits from the following classes: Sprite, DisplayObjectContainer,
    /// InteractiveObject, DisplayObject, and EventDispatcher.
    /// </summary>
    [PageFX.AbcInstance(370)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class MovieClip : flash.display.Sprite
    {
        /// <summary>
        /// Specifies the number of the frame in which the playhead is located in the timeline of
        /// the MovieClip instance. If the movie clip has multiple scenes, this value is the
        /// frame number in the current scene.
        /// </summary>
        public extern virtual int currentFrame
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The number of frames that are loaded from a streaming SWF file. You can use the frameLoaded
        /// property to determine whether the contents of a specific frame and all the frames before it
        /// loaded and are available locally in the browser. You can also use it to monitor the downloading
        /// of large SWF files. For example, you might want to display a message to users indicating that
        /// the SWF file is loading until a specified frame in the SWF file finishes loading.
        /// If the movie clip contains multiple scenes, the framesLoaded property returns the number
        /// of frames loaded for all scenes in the movie clip.
        /// </summary>
        public extern virtual int framesLoaded
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The total number of frames in the MovieClip instance.
        /// If the movie clip contains multiple frames, the totalFrames property returns
        /// the total number of frames in all scenes in the movie clip.
        /// </summary>
        public extern virtual int totalFrames
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Indicates whether other display objects that are SimpleButton or MovieClip objects can receive
        /// mouse release events. The trackAsMenu property lets you create menus. You
        /// can set the trackAsMenu property on any SimpleButton or MovieClip object.
        /// The default value of the trackAsMenu property is false.
        /// You can change the trackAsMenu property at any time; the modified movie
        /// clip immediately uses the new behavior.
        /// </summary>
        public extern virtual bool trackAsMenu
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
        /// An array of Scene objects, each listing the name, the number of frames,
        /// and the frame labels for a scene in the MovieClip instance.
        /// </summary>
        public extern virtual Avm.Array scenes
        {
            [PageFX.AbcInstanceTrait(12)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>The current scene in which the playhead is located in the timeline of the MovieClip instance.</summary>
        public extern virtual flash.display.Scene currentScene
        {
            [PageFX.AbcInstanceTrait(13)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The current label in which the playhead is located in the timeline of the MovieClip instance.
        /// If the current frame has no label, currentLabel is set to the name of the previous frame
        /// that includes a label. If the current frame and previous frames do not include a label,
        /// currentLabel returns null.
        /// </summary>
        public extern virtual Avm.String currentLabel
        {
            [PageFX.AbcInstanceTrait(14)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String currentFrameLabel
        {
            [PageFX.AbcInstanceTrait(15)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Returns an array of FrameLabel objects from the current scene. If the MovieClip instance does
        /// not use scenes, the array includes all frame labels from the entire MovieClip instance.
        /// </summary>
        public extern virtual Avm.Array currentLabels
        {
            [PageFX.AbcInstanceTrait(16)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// A Boolean value that indicates whether a movie clip is enabled. The default value of enabled
        /// is true. If enabled is set to false, the movie clip&apos;s
        /// Over, Down, and Up frames are disabled. The movie clip
        /// continues to receive events (for example, mouseDown,
        /// mouseUp, keyDown, and keyUp).
        /// The enabled property governs only the button-like properties of a movie clip. You
        /// can change the enabled property at any time; the modified movie clip is immediately
        /// enabled or disabled. If enabled is set to false, the object is not
        /// included in automatic tab ordering.
        /// </summary>
        public extern virtual bool enabled
        {
            [PageFX.AbcInstanceTrait(19)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(20)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern MovieClip();

        /// <summary>Moves the playhead in the timeline of the movie clip.</summary>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void play();

        /// <summary>Stops the playhead in the movie clip.</summary>
        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void stop();

        /// <summary>
        /// Sends the playhead to the next frame and stops it.  This happens after all
        /// remaining actions in the frame have finished executing.
        /// </summary>
        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void nextFrame();

        /// <summary>
        /// Sends the playhead to the previous frame and stops it.  This happens after all
        /// remaining actions in the frame have finished executing.
        /// </summary>
        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void prevFrame();

        /// <summary>
        /// Starts playing the SWF file at the specified frame.  This happens after all
        /// remaining actions in the frame have finished executing.  To specify a scene
        /// as well as a frame, specify a value for the scene parameter.
        /// </summary>
        /// <param name="frame">
        /// A number representing the frame number, or a string representing the label of the
        /// frame, to which the playhead is sent. If you specify a number, it is relative to the
        /// scene you specify. If you do not specify a scene, Flash Player uses the current scene
        /// to determine the global frame number to play. If you do specify a scene, the playhead
        /// jumps to the frame number in the specified scene.
        /// </param>
        /// <param name="scene">(default = null)  The name of the scene to play. This parameter is optional.</param>
        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void gotoAndPlay(object frame, Avm.String scene);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void gotoAndPlay(object frame);

        /// <summary>
        /// Brings the playhead to the specified frame of the movie clip and stops it there.  This happens after all
        /// remaining actions in the frame have finished executing.  If you want to specify a scene in addition to a frame,
        /// specify a scene parameter.
        /// </summary>
        /// <param name="frame">
        /// A number representing the frame number, or a string representing the label of the
        /// frame, to which the playhead is sent. If you specify a number, it is relative to the
        /// scene you specify. If you do not specify a scene, Flash Player uses the current scene
        /// to determine the global frame number at which to go to and stop. If you do specify a scene,
        /// the playhead goes to the frame number in the specified scene and stops.
        /// </param>
        /// <param name="scene">(default = null)  The name of the scene. This parameter is optional.</param>
        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void gotoAndStop(object frame, Avm.String scene);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void gotoAndStop(object frame);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void addFrameScript();

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addFrameScript(object rest0);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addFrameScript(object rest0, object rest1);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addFrameScript(object rest0, object rest1, object rest2);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addFrameScript(object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addFrameScript(object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addFrameScript(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addFrameScript(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addFrameScript(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addFrameScript(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addFrameScript(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        /// <summary>
        /// Moves the playhead to the previous scene of the MovieClip instance.  This happens after all
        /// remaining actions in the frame have finished executing.
        /// </summary>
        [PageFX.AbcInstanceTrait(17)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void prevScene();

        /// <summary>
        /// Moves the playhead to the next scene of the MovieClip instance.  This happens after all
        /// remaining actions in the frame have finished executing.
        /// </summary>
        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void nextScene();


    }
}
