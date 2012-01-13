using System;
using System.Runtime.CompilerServices;

namespace flash.system
{
    /// <summary>
    /// The System class contains properties related to certain operations that take place
    /// on the user&apos;s computer, such as operations with shared
    /// objects, local settings for cameras and microphones, and  the use of the Clipboard.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class System : Avm.Object
    {
        public extern static IME ime
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static bool useCodePage
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

        public extern static uint totalMemory
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static Avm.String vmVersion
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern System();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void resume();

        /// <summary>
        /// Replaces the contents of the Clipboard with a specified text string.
        /// Note: Because of security concerns, it is not possible to read the contents of the system Clipboard.
        /// In other words, there is no corresponding System.getClipboard() method.
        /// </summary>
        /// <param name="arg0">A plain-text string of characters to put on the system Clipboard, replacing its current contents (if any).</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void setClipboard(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void pause();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void gc();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void exit(uint arg0);
    }
}
