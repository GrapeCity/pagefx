using System;
using System.Runtime.CompilerServices;

namespace flash.system
{
    /// <summary>
    /// The SecurityPanel class provides values for specifying
    /// which Security Settings panel you want to display.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class SecurityPanel : Avm.Object
    {
        /// <summary>
        /// When passed to Security.showSettings(), displays the panel
        /// that was open the last time the user closed the Flash Player Settings.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String DEFAULT;

        /// <summary>
        /// When passed to Security.showSettings(), displays the
        /// Camera panel in Flash Player Settings.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String CAMERA;

        /// <summary>
        /// When passed to Security.showSettings(), displays the
        /// Local Storage Settings panel in Flash Player Settings.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String LOCAL_STORAGE;

        /// <summary>
        /// When passed to Security.showSettings(), displays the
        /// Privacy Settings panel in Flash Player Settings.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String PRIVACY;

        /// <summary>
        /// When passed to Security.showSettings(), displays the
        /// Microphone panel in Flash Player Settings.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String MICROPHONE;

        /// <summary>
        /// When passed to Security.showSettings(), displays the
        /// Settings Manager (in a separate browser window).
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String SETTINGS_MANAGER;

        [PageFX.ABC]
        [PageFX.FP10]
        public static Avm.String DISPLAY;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SecurityPanel();
    }
}
