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
    [PageFX.ABC]
    [PageFX.FP9]
    public class Scene : Avm.Object
    {
        public extern virtual int numFrames
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String name
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.Array labels
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Scene(Avm.String arg0, Avm.Array arg1, int arg2);


    }
}
