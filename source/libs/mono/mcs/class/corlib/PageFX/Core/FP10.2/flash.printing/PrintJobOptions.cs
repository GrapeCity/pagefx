using System;
using System.Runtime.CompilerServices;

namespace flash.printing
{
    /// <summary>
    /// The PrintJobOptions class contains properties to use with the
    /// options  parameter of the PrintJob.addPage()  method.
    /// For more information about addPage() , see the PrintJob class.
    /// </summary>
    [PageFX.AbcInstance(108)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class PrintJobOptions : Avm.Object
    {
        /// <summary>
        /// Specifies whether the content in the print job is printed as a bitmap or as a vector.
        /// The default value is false, for vector printing.
        /// If the content that you&apos;re printing includes a bitmap image,
        /// set printAsBitmap to true to include any
        /// alpha transparency and color effects.
        /// If the content does not include bitmap images, you should print
        /// the content in higher quality vector format (the default option).For example, to print your content as a bitmap, use the following syntax:
        /// var options:PrintJobOptions = new PrintJobOptions();
        /// myOption.printAsBitmap = true;
        /// myPrintJob.addPage(mySprite, null, myOption);
        /// </summary>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public bool printAsBitmap;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern PrintJobOptions(bool printAsBitmap);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern PrintJobOptions();
    }
}
