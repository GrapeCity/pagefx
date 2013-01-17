using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The Scene class includes properties for identifying the name, labels, and number of frames
    /// in a scene. The MovieClip class includes a currentScene  property, which is
    /// a Scene object that identifies the scene in which the playhead is located in the
    /// timeline of the MovieClip instance. The scenes  property of the
    /// MovieClip class is an array of Scene objects. Also, the gotoAndPlay()
    /// and gotoAndStop()  methods of the MovieClip class use Scene objects as
    /// parameters.
    /// </summary>
    [PageFX.AbcInstance(210)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class Scene : Avm.Object
    {
        /// <summary>The name of the scene.</summary>
        public extern virtual Avm.String name
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// An array of FrameLabel objects for the scene. Each FrameLabel object contains
        /// a frame property, which specifies the frame number corresponding to the
        /// label, and a name property.
        /// </summary>
        public extern virtual Avm.Array labels
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>The number of frames in the scene.</summary>
        public extern virtual int numFrames
        {
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Scene(Avm.String name, Avm.Array labels, int numFrames);


    }
}
