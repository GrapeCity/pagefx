using System;
using System.Runtime.CompilerServices;

namespace flash.printing
{
    /// <summary>This class provides values that are used by the PrintJob.orientation  property for the image position of a printed page.</summary>
    [PageFX.AbcInstance(337)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class PrintJobOrientation : Avm.Object
    {
        /// <summary>
        /// The landscape (horizontal) image orientation for printing.
        /// This constant is used with the PrintJob.orientation property.
        /// Use the syntax PrintJobOrientation.LANDSCAPE.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String LANDSCAPE;

        /// <summary>
        /// The portrait (vertical) image orientation for printing.
        /// This constant is used with the PrintJob.orientation property.
        /// Use the syntax PrintJobOrientation.PORTRAIT.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String PORTRAIT;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern PrintJobOrientation();
    }
}
