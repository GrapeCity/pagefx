using System;
using System.Runtime.CompilerServices;

namespace flash.system
{
    /// <summary>
    /// The System class contains properties related to certain operations that take place
    /// on the user&apos;s computer, such as operations with shared
    /// objects, local settings for cameras and microphones, and  the use of the Clipboard.
    /// </summary>
    [PageFX.AbcInstance(261)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class System : Avm.Object
    {
        /// <summary>
        /// The currently installed system IME.
        /// To register for imeComposition events, call
        /// addEventListener() on this instance.
        /// </summary>
        public extern static flash.system.IME ime
        {
            [PageFX.AbcClassTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The amount of memory (in bytes) currently in use by Adobe®
        /// Flash® Player or the Adobe® Integrated Runtime (AIR™).
        /// </summary>
        public extern static uint totalMemory
        {
            [PageFX.AbcClassTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static double totalMemoryNumber
        {
            [PageFX.AbcClassTrait(4)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static double freeMemory
        {
            [PageFX.AbcClassTrait(5)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static double privateMemory
        {
            [PageFX.AbcClassTrait(6)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// A Boolean value that determines which code page to use to interpret external text files.
        /// When the property is set to false, external text files are interpretted as Unicode.
        /// (These files must be encoded as Unicode when you save them.) When the property is set to
        /// true, external text files are interpretted using the traditional code page of the
        /// operating system running the player. The default value of useCodePage is false.
        /// Text that you load as an external file (using Loader.load(), the URLLoader class or
        /// URLStream) must have been saved as Unicode in order for the application to recognize it
        /// as Unicode. To encode external files as Unicode, save the files in an application that
        /// supports Unicode, such as Notepad on Windows.If you load external text files that are not Unicode-encoded, set useCodePage to true.
        /// Add the following as the first line of code in the  first frame of the file that
        /// is loading the data:System.useCodePage = true;When this code is present, Flash Player interprets external text
        /// using the traditional code page of the operating system.
        /// This is generally CP1252 for an English Windows operating
        /// system and Shift-JIS for a Japanese operating system.
        /// If you set useCodePage to true,
        /// Flash Player 6 and later treat text as Flash Player 5 does. (Flash Player 5
        /// treated all text as if it were in the traditional code page of the operating
        /// system running the player.)If you set useCodePage to true, remember that the
        /// traditional code page of the operating system running the player must include
        /// the characters used in your external text file in order to display your text.
        /// For example, if you load an external text file that contains Chinese characters,
        /// those characters cannot display on a system that uses the CP1252 code page because
        /// that code page does not include Chinese characters.To ensure that users on all platforms can view external text files used in your
        /// application, you should encode all external text files as Unicode and leave
        /// useCodePage set to false. This way, the application
        /// (Flash Player 6 and later) interprets the text as Unicode.
        /// </summary>
        public extern static bool useCodePage
        {
            [PageFX.AbcClassTrait(7)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcClassTrait(8)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern static Avm.String vmVersion
        {
            [PageFX.AbcClassTrait(9)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern System();

        /// <summary>
        /// Replaces the contents of the Clipboard with a specified text string.
        /// Note: Because of security concerns, it is not possible to read the contents of the system Clipboard.
        /// In other words, there is no corresponding System.getClipboard() method.
        /// </summary>
        /// <param name="@string">A plain-text string of characters to put on the system Clipboard, replacing its current contents (if any).</param>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void setClipboard(Avm.String @string);

        [PageFX.AbcClassTrait(10)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void pause();

        [PageFX.AbcClassTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void resume();

        [PageFX.AbcClassTrait(12)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void exit(uint code);

        [PageFX.AbcClassTrait(13)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void gc();

        [PageFX.AbcClassTrait(14)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void nativeConstructionOnly(object @object);

        [PageFX.AbcClassTrait(15)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void disposeXML(Avm.XML node);
    }
}
