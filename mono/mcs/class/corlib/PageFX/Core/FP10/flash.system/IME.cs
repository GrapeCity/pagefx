using System;
using System.Runtime.CompilerServices;

namespace flash.system
{
    /// <summary>
    /// The IME class lets you directly manipulate the operating system&apos;s input method
    /// editor (IME) in the Flash Player application that is running on a client computer. You can
    /// determine whether an IME is installed, whether or not the IME is currently enabled, and which IME is
    /// enabled. You can disable or enable the IME in the Flash Player application, and you can perform other limited
    /// functions, depending on the operating system.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class IME : flash.events.EventDispatcher
    {
        public extern static bool enabled
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

        public extern static Avm.String conversionMode
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

        public extern static bool constructOK
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [PageFX.Event("imeComposition")]
        public event flash.events.IMEEventHandler imeComposition
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern IME();

        /// <summary>
        /// Sets the IME composition string. When this string is set, the user
        /// can select IME candidates before committing the result to the text
        /// field that currently has focus.
        /// If no text field has focus, this method fails and throws an error.
        /// </summary>
        /// <param name="arg0">The string to send to the IME.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void setCompositionString(Avm.String arg0);

        /// <summary>Instructs the IME to select the first candidate for the current composition string.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void doConversion();


    }
}
