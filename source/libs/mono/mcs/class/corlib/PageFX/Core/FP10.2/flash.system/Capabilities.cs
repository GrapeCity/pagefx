using System;
using System.Runtime.CompilerServices;

namespace flash.system
{
    /// <summary>
    /// The Capabilities class provides properties that
    /// describe the system and player that are hosting a SWF file.
    /// For example, a mobile phone&apos;s screen might be 100 square
    /// pixels, black and white, whereas a PC screen might be 1000 square pixels, color.
    /// By using the Capabilities object to determine what type of device a user has,
    /// you can provide appropriate content to as many users as possible. When you know
    /// the device&apos;s capabilities, you can tell the server to send the appropriate SWF
    /// files or tell the SWF file to alter its presentation. The Capabilities class provides properties that describe
    /// the system and runtime that are hosting HTML (and SWF) content.
    /// By using the Capabilities object to determine what type of computer a user has,
    /// you can provide appropriate content to as many users as possible. When you know
    /// the computer&apos;s capabilities, you can load appropriate content or use code to
    /// alter its presentation.
    /// </summary>
    [PageFX.AbcInstance(265)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class Capabilities : Avm.Object
    {
        public extern static bool isEmbeddedInAcrobat
        {
            [PageFX.AbcClassTrait(0)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether the system supports
        /// (true) or does not support (false)
        /// embedded video. The server string is EV.
        /// </summary>
        public extern static bool hasEmbeddedVideo
        {
            [PageFX.AbcClassTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether the system has audio
        /// capabilities. This property is always true.  The server
        /// string is A.
        /// </summary>
        public extern static bool hasAudio
        {
            [PageFX.AbcClassTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether access to the user&apos;s camera and microphone has
        /// been administratively prohibited (true) or allowed (false).
        /// The server string is AVD.
        /// For content in the Adobe Integrated Runtime
        /// (AIR), this property applies only to content in security sandboxes other
        /// than the application security sandbox. Content in the application
        /// security sandbox can always access the user&apos;s camera and microphone.
        /// </summary>
        public extern static bool avHardwareDisable
        {
            [PageFX.AbcClassTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether the system supports
        /// (true) or does not support (false) communication
        /// with accessibility aids.
        /// The server string is ACC.
        /// </summary>
        public extern static bool hasAccessibility
        {
            [PageFX.AbcClassTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether the system can (true) or cannot (false)
        /// encode an audio stream, such as that coming from a microphone.
        /// The server string is AE.
        /// </summary>
        public extern static bool hasAudioEncoder
        {
            [PageFX.AbcClassTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether the system does (true)
        /// or does not (false) have an MP3 decoder.
        /// The server string is MP3.
        /// </summary>
        public extern static bool hasMP3
        {
            [PageFX.AbcClassTrait(6)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether the system does (true)
        /// or does not (false) support printing.
        /// The server string is PR.
        /// </summary>
        public extern static bool hasPrinting
        {
            [PageFX.AbcClassTrait(7)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether the system does (true) or does not (false)
        /// support the development of screen broadcast applications to be run through Flash Media
        /// Server.
        /// The server string is SB.
        /// </summary>
        public extern static bool hasScreenBroadcast
        {
            [PageFX.AbcClassTrait(8)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether the system does (true) or does not
        /// (false) support the playback of screen broadcast applications
        /// that are being run through Flash Media Server.
        /// The server string is SP.
        /// </summary>
        public extern static bool hasScreenPlayback
        {
            [PageFX.AbcClassTrait(9)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether the system can (true) or cannot (false)
        /// play streaming audio.
        /// The server string is SA.
        /// </summary>
        public extern static bool hasStreamingAudio
        {
            [PageFX.AbcClassTrait(10)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether the system can (true) or cannot
        /// (false) play streaming video.
        /// The server string is SV.
        /// </summary>
        public extern static bool hasStreamingVideo
        {
            [PageFX.AbcClassTrait(11)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether the system can (true) or cannot
        /// (false) encode a video stream, such as that coming
        /// from a web camera.
        /// The server string is VE.
        /// </summary>
        public extern static bool hasVideoEncoder
        {
            [PageFX.AbcClassTrait(12)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether the system is a special debugging version
        /// (true) or an officially released version (false).
        /// The server string is DEB.
        /// </summary>
        public extern static bool isDebugger
        {
            [PageFX.AbcClassTrait(13)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether read access to the user&apos;s hard disk has been
        /// administratively prohibited (true) or allowed
        /// (false). For content in the Adobe Integrated Runtime
        /// (AIR), this property applies only to content in security sandboxes other
        /// than the application security sandbox. (Content in the application
        /// security sandbox can always read from the file system.)
        /// If this property is true,
        /// Flash Player cannot read files (including the first file that
        /// Flash Player launches with) from the user&apos;s hard disk.
        /// If this property is true, AIR content outside of the
        /// application security sandbox cannot read files from the user&apos;s
        /// hard disk. For example, attempts to read a file on the user&apos;s
        /// hard disk using load methods will fail if this property
        /// is set to true.
        /// Reading runtime shared libraries is also blocked
        /// if this property is set to true, but reading local shared objects
        /// is allowed without regard to the value of this property.The server string is LFD.
        /// </summary>
        public extern static bool localFileReadDisable
        {
            [PageFX.AbcClassTrait(14)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies the language code of the system on which the content is running. The language is
        /// specified as a lowercase two-letter language code from ISO 639-1. For Chinese, an additional
        /// uppercase two-letter country code from ISO 3166 distinguishes between Simplified and
        /// Traditional Chinese. The languages codes are based on the English names of the language: for example,
        /// hu specifies Hungarian.
        /// On English systems, this property returns only the language code (en), not
        /// the country code. On Microsoft Windows systems, this property returns the user interface (UI)
        /// language, which refers to the language used for all menus, dialog boxes, error messages, and help
        /// files. The following table lists the possible values:
        /// LanguageValueCzechcsDanishdaDutchnlEnglishenFinnishfiFrenchfrGermandeHungarianhuItalianitJapanesejaKoreankoNorwegiannoOther/unknownxuPolishplPortugueseptRussianruSimplified Chinesezh-CNSpanishesSwedishsvTraditional Chinesezh-TWTurkishtrThe server string is L.
        /// </summary>
        public extern static Avm.String language
        {
            [PageFX.AbcClassTrait(15)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies the manufacturer of Flash Player, in the format &quot;AdobeOSName&quot;. The value for OSName
        /// could be &quot;Windows&quot;, &quot;Macintosh&quot;,
        /// &quot;Linux&quot;, or another operating system name. The server string is M.
        /// </summary>
        public extern static Avm.String manufacturer
        {
            [PageFX.AbcClassTrait(16)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies the current operating system. The os property
        /// can return the following strings: &quot;Windows XP&quot;, &quot;Windows 2000&quot;,
        /// &quot;Windows NT&quot;, &quot;Windows 98/ME&quot;, &quot;Windows 95&quot;,
        /// &quot;Windows CE&quot; (available only in Flash Player SDK, not in the desktop version),
        /// &quot;Linux&quot;, and &quot;MacOS&quot;.
        /// The server string is OS.
        /// </summary>
        public extern static Avm.String os
        {
            [PageFX.AbcClassTrait(17)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static Avm.String cpuArchitecture
        {
            [PageFX.AbcClassTrait(18)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies the type of player. This property can have one of the following
        /// values:
        /// &quot;ActiveX&quot; for the Flash Player ActiveX control used by Microsoft Internet Explorer&quot;Desktop&quot; for the Adobe Integrated Runtime (AIR)&quot;External&quot; for the external Flash Player or in test mode&quot;PlugIn&quot; for the Flash Player browser plug-in&quot;StandAlone&quot; for the stand-alone Flash PlayerThe server string is PT.
        /// </summary>
        public extern static Avm.String playerType
        {
            [PageFX.AbcClassTrait(19)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// A URL-encoded string that specifies values for each Capabilities
        /// property.
        /// The following example shows a URL-encoded string:
        /// A=t&amp;SA=t&amp;SV=t&amp;EV=t&amp;MP3=t&amp;AE=t&amp;VE=t&amp;ACC=f&amp;PR=t&amp;SP=t&amp;
        /// SB=f&amp;DEB=t&amp;V=WIN%208%2C5%2C0%2C208&amp;M=Adobe%20Windows&amp;
        /// R=1600x1200&amp;DP=72&amp;COL=color&amp;AR=1.0&amp;OS=Windows%20XP&amp;
        /// L=en&amp;PT=External&amp;AVD=f&amp;LFD=f&amp;WD=f
        /// </summary>
        public extern static Avm.String serverString
        {
            [PageFX.AbcClassTrait(20)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies the Flash Player or Adobe® Integrated Runtime (AIR™)
        /// platform and version information. The format of the version number is:
        /// platform majorVersion,minorVersion,buildNumber,internalBuildNumber.
        /// Possible values for platform are &quot;WIN&quot;,
        /// &quot;MAC&quot;, and &quot;UNIX&quot;. Here are some examples of
        /// version information:
        /// WIN 9,0,0,0  // Flash Player 9 for Windows
        /// MAC 7,0,25,0   // Flash Player 7 for Macintosh
        /// UNIX 5,0,55,0  // Flash Player 5 for UNIX
        /// The server string is V.
        /// </summary>
        public extern static Avm.String version
        {
            [PageFX.AbcClassTrait(21)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies the screen color. This property can have the value
        /// &quot;color&quot;, &quot;gray&quot; (for grayscale), or
        /// &quot;bw&quot; (for black and white).
        /// The server string is COL.
        /// </summary>
        public extern static Avm.String screenColor
        {
            [PageFX.AbcClassTrait(22)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies the pixel aspect ratio of the screen. The server string
        /// is AR.
        /// </summary>
        public extern static double pixelAspectRatio
        {
            [PageFX.AbcClassTrait(23)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies the dots-per-inch (dpi) resolution of the screen, in pixels.
        /// The server string is DP.
        /// </summary>
        public extern static double screenDPI
        {
            [PageFX.AbcClassTrait(24)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies the maximum horizontal resolution of the screen.
        /// The server string is R (which returns both the width and height of the screen).
        /// </summary>
        public extern static double screenResolutionX
        {
            [PageFX.AbcClassTrait(25)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies the maximum vertical resolution of the screen.
        /// The server string is R (which returns both the width and height of the screen).
        /// </summary>
        public extern static double screenResolutionY
        {
            [PageFX.AbcClassTrait(26)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static Avm.String touchscreenType
        {
            [PageFX.AbcClassTrait(27)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether the system does (true)
        /// or does not (false) have an input method editor (IME) installed.
        /// The server string is IME.
        /// </summary>
        public extern static bool hasIME
        {
            [PageFX.AbcClassTrait(28)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether the system supports native SSL sockets through NetConnection
        /// (true) or does not (false).
        /// The server string is TLS.
        /// </summary>
        public extern static bool hasTLS
        {
            [PageFX.AbcClassTrait(29)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static Avm.String maxLevelIDC
        {
            [PageFX.AbcClassTrait(30)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static bool supports32BitProcesses
        {
            [PageFX.AbcClassTrait(31)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static bool supports64BitProcesses
        {
            [PageFX.AbcClassTrait(32)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static uint _internal
        {
            [PageFX.AbcClassTrait(33)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Capabilities();


    }
}
