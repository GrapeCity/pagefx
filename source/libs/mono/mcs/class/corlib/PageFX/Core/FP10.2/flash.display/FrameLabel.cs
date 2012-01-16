using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The FrameLabel object contains properties that specify a frame number and the
    /// corresponding label name.
    /// The Scene class includes a labels  property, which is an array
    /// of FrameLabel objects for the scene.
    /// </summary>
    [PageFX.AbcInstance(206)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class FrameLabel : Avm.Object
    {
        /// <summary>The name of the label.</summary>
        public extern virtual Avm.String name
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>The frame number containing the label.</summary>
        public extern virtual int frame
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FrameLabel(Avm.String name, int frame);


    }
}
