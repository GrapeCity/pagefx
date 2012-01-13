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
    [PageFX.ABC]
    [PageFX.FP9]
    public class FrameLabel : Avm.Object
    {
        public extern virtual Avm.String name
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual int frame
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FrameLabel(Avm.String arg0, int arg1);


    }
}
