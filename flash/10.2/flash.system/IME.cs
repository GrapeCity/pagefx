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
    [PageFX.AbcInstance(304)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class IME : flash.events.EventDispatcher
    {
        public extern static bool constructOK
        {
            [PageFX.AbcClassTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates whether the system IME is enabled (true) or disabled (false).
        /// An enabled IME performs multibyte input; a disabled IME performs alphanumeric input.
        /// </summary>
        public extern static bool enabled
        {
            [PageFX.AbcClassTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcClassTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The conversion mode of the current IME.
        /// Possible values are IME mode string constants that indicate the conversion mode:
        /// ALPHANUMERIC_FULLALPHANUMERIC_HALFCHINESEJAPANESE_HIRAGANAJAPANESE_KATAKANA_FULLJAPANESE_KATAKANA_HALFKOREANUNKNOWN (read-only value; this value cannot be set)
        /// </summary>
        public extern static Avm.String conversionMode
        {
            [PageFX.AbcClassTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcClassTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern static bool isSupported
        {
            [PageFX.AbcClassTrait(9)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
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
        /// <param name="composition">The string to send to the IME.</param>
        [PageFX.AbcClassTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void setCompositionString(Avm.String composition);

        /// <summary>Instructs the IME to select the first candidate for the current composition string.</summary>
        [PageFX.AbcClassTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void doConversion();

        [PageFX.AbcClassTrait(7)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void compositionSelectionChanged(int start, int end);

        [PageFX.AbcClassTrait(8)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void compositionAbandoned();


    }
}
