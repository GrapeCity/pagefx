using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The MovieClip class inherits from the following classes: Sprite, DisplayObjectContainer,
    /// InteractiveObject, DisplayObject, and EventDispatcher.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class MovieClip : Sprite
    {
        public extern virtual Avm.String currentFrameLabel
        {
            [PageFX.ABC]
            [PageFX.QName("currentFrameLabel", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.Array scenes
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool enabled
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

        public extern virtual int totalFrames
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual int framesLoaded
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual int currentFrame
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Scene currentScene
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool trackAsMenu
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

        public extern virtual Avm.String currentLabel
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.Array currentLabels
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern MovieClip();

        /// <summary>
        /// Sends the playhead to the previous frame and stops it.  This happens after all
        /// remaining actions in the frame have finished executing.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void prevFrame();

        /// <summary>Stops the playhead in the movie clip.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void stop();

        /// <summary>
        /// Starts playing the SWF file at the specified frame.  This happens after all
        /// remaining actions in the frame have finished executing.  To specify a scene
        /// as well as a frame, specify a value for the scene parameter.
        /// </summary>
        /// <param name="arg0">
        /// A number representing the frame number, or a string representing the label of the
        /// frame, to which the playhead is sent. If you specify a number, it is relative to the
        /// scene you specify. If you do not specify a scene, Flash Player uses the current scene
        /// to determine the global frame number to play. If you do specify a scene, the playhead
        /// jumps to the frame number in the specified scene.
        /// </param>
        /// <param name="arg1">(default = null)  The name of the scene to play. This parameter is optional.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void gotoAndPlay(object arg0, Avm.String arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void gotoAndPlay(object arg0);

        /// <summary>
        /// Moves the playhead to the next scene of the MovieClip instance.  This happens after all
        /// remaining actions in the frame have finished executing.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void nextScene();

        /// <summary>
        /// Brings the playhead to the specified frame of the movie clip and stops it there.  This happens after all
        /// remaining actions in the frame have finished executing.  If you want to specify a scene in addition to a frame,
        /// specify a scene parameter.
        /// </summary>
        /// <param name="arg0">
        /// A number representing the frame number, or a string representing the label of the
        /// frame, to which the playhead is sent. If you specify a number, it is relative to the
        /// scene you specify. If you do not specify a scene, Flash Player uses the current scene
        /// to determine the global frame number at which to go to and stop. If you do specify a scene,
        /// the playhead goes to the frame number in the specified scene and stops.
        /// </param>
        /// <param name="arg1">(default = null)  The name of the scene. This parameter is optional.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void gotoAndStop(object arg0, Avm.String arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void gotoAndStop(object arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void addFrameScript();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addFrameScript(object rest0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addFrameScript(object rest0, object rest1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addFrameScript(object rest0, object rest1, object rest2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addFrameScript(object rest0, object rest1, object rest2, object rest3);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addFrameScript(object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addFrameScript(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addFrameScript(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addFrameScript(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addFrameScript(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void addFrameScript(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        /// <summary>
        /// Moves the playhead to the previous scene of the MovieClip instance.  This happens after all
        /// remaining actions in the frame have finished executing.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void prevScene();

        /// <summary>
        /// Sends the playhead to the next frame and stops it.  This happens after all
        /// remaining actions in the frame have finished executing.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void nextFrame();

        /// <summary>Moves the playhead in the timeline of the movie clip.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void play();


    }
}
