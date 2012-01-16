using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The LoaderInfo class provides information about a loaded SWF file or a loaded image file
    /// (JPEG, GIF, or PNG).  LoaderInfo objects are available for any display object.
    /// The information provided includes load progress, the URLs of the loader and
    /// loaded content, the number of bytes total for the media, and the nominal height and width of the
    /// media.
    /// </summary>
    [PageFX.AbcInstance(198)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class LoaderInfo : flash.events.EventDispatcher
    {
        /// <summary>
        /// The URL of the SWF file that initiated the loading of the media
        /// described by this LoaderInfo object.  For the instance of the main class of the SWF file, this
        /// URL is the same as the SWF file&apos;s own URL.
        /// </summary>
        public extern virtual Avm.String loaderURL
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The URL of the media being loaded.
        /// Before the first progress event is dispatched by this LoaderInfo
        /// object&apos;s corresponding Loader object, the value of the url property
        /// might reflect only the initial URL specified in the call to the load()
        /// method  of the Loader object.  After the first progress event, the
        /// url property reflects the media&apos;s final URL, after any redirects and relative
        /// URLs are resolved.
        /// </summary>
        public extern virtual Avm.String url
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool isURLInaccessible
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The number of bytes that are loaded for the media. When this number equals
        /// the value of bytesTotal, all of the bytes are loaded.
        /// </summary>
        public extern virtual uint bytesLoaded
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The number of compressed bytes in the entire media file.
        /// Before the first progress event is dispatched by
        /// this LoaderInfo object&apos;s corresponding Loader object, bytesTotal is 0.
        /// After the first progress event from the Loader object, bytesTotal
        /// reflects the actual number of bytes to be downloaded.
        /// </summary>
        public extern virtual uint bytesTotal
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// When an external SWF file is loaded, all ActionScript 3.0 definitions contained in the loaded
        /// class are stored in the applicationDomain property.
        /// All code in a SWF file is defined to exist in an application domain. The current application
        /// domain is where your main application runs. The system domain contains all application domains,
        /// including the current domain, which means that it contains all Flash Player classes.All application domains, except the system domain, have an associated parent domain.
        /// The parent domain of your main application&apos;s applicationDomain is the system domain.
        /// Loaded classes are defined only when their parent doesn&apos;t already define them.
        /// You cannot override a loaded class definition with a newer definition.For usage examples of application domains, see the &quot;Client System Environment&quot; chapter
        /// in Programming ActionScript 3.0.
        /// </summary>
        public extern virtual flash.system.ApplicationDomain applicationDomain
        {
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The file format version of the loaded SWF file.
        /// The file format is specified using the enumerations in the
        /// SWFVersion class, such as SWFVersion.FLASH7 and SWFVersion.FLASH9.
        /// </summary>
        public extern virtual uint swfVersion
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The ActionScript version of the loaded SWF file.
        /// The language version is specified by using the enumerations in the
        /// ActionScriptVersion class, such as ActionScriptVersion.ACTIONSCRIPT2
        /// and ActionScriptVersion.ACTIONSCRIPT3.
        /// Note: This property always has a value of either ActionScriptVersion.ACTIONSCRIPT2 or
        /// ActionScriptVersion.ACTIONSCRIPT3.  ActionScript 1.0 and 2.0 are
        /// both reported as ActionScriptVersion.ACTIONSCRIPT2 (version 2.0).  This property
        /// only distinguishes ActionScript 1.0 and 2.0 from ActionScript 3.0.
        /// </summary>
        public extern virtual uint actionScriptVersion
        {
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The nominal frame rate, in frames per second, of the loaded SWF file. This
        /// number is often an integer, but need not be.
        /// This value may differ from the actual frame rate in use.
        /// Flash Player only uses a single frame rate for all loaded SWF files at
        /// any one time, and this frame rate is determined by the nominal
        /// frame rate of the main SWF file. Also, Flash Player might not be able to
        /// achieve the main frame rate, depending on hardware, sound synchronization,
        /// and other factors.
        /// </summary>
        public extern virtual double frameRate
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// An object that contains name-value pairs that represent the parameters provided
        /// to the loaded SWF file.
        /// You can use a for-in loop to extract all the names and values
        /// from the parameters object.The two sources of parameters are: the query string in the
        /// URL of the main SWF file, and the value of the FlashVars HTML parameter (this affects
        /// only the main SWF file).The parameters property replaces the ActionScript 1.0 and 2.0 technique of
        /// providing SWF file parameters as properties of the main timeline.The value of the parameters property is null for Loader objects
        /// that contain SWF files that use ActionScript 1.0 or 2.0. It is only
        /// non-null for Loader objects that contain SWF files that use ActionScript 3.0.
        /// </summary>
        public extern virtual object parameters
        {
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The nominal width of the loaded content. This value might differ from the actual
        /// width at which the content is displayed, since the loaded content or its parent
        /// display objects might be scaled.
        /// </summary>
        public extern virtual int width
        {
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The nominal height of the loaded file. This value might differ from the actual
        /// height at which the content is displayed, since the loaded content or its parent
        /// display objects might be scaled.
        /// </summary>
        public extern virtual int height
        {
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The MIME type of the loaded file. The value is null if not enough of the file has loaded
        /// for Flash Player to determine the type. The following list gives the possible values:
        /// &quot;application/x-shockwave-flash&quot;&quot;image/jpeg&quot;&quot;image/gif&quot;&quot;image/png&quot;
        /// </summary>
        public extern virtual Avm.String contentType
        {
            [PageFX.AbcInstanceTrait(12)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// An EventDispatcher instance that can be used to exchange events across security boundaries.
        /// Even when the loader and the loadee do not trust one another, both can access sharedEvents.
        /// </summary>
        public extern virtual flash.events.EventDispatcher sharedEvents
        {
            [PageFX.AbcInstanceTrait(13)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Expresses the domain relationship between the loader and the content: true if they have
        /// the same origin domain; false otherwise.
        /// </summary>
        public extern virtual bool sameDomain
        {
            [PageFX.AbcInstanceTrait(15)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Expresses the trust relationship from content (child) to the Loader (parent).
        /// If the child has allowed the parent access, true; otherwise,
        /// false. This property is set to true if the child object
        /// has called the allowDomain() method to grant permission to the parent domain
        /// or if a cross-domain policy is loaded at the child domain that grants permission
        /// to the parent domain. If child and parent are in
        /// the same domain, this property is set to true.
        /// For more information, see the &quot;Flash Player Security&quot; chapter
        /// in Programming ActionScript 3.0.
        /// </summary>
        public extern virtual bool childAllowsParent
        {
            [PageFX.AbcInstanceTrait(16)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Expresses the trust relationship from Loader (parent) to the content (child).
        /// If the parent has allowed the child access, true; otherwise,
        /// false. This property is set to true if the parent object
        /// called the allowDomain() method to grant permission to the child domain
        /// or if a cross-domain policy file is loaded at the parent domain granting permission
        /// to the child domain. If child and parent are in
        /// the same domain, this property is set to true.
        /// For more information, see the &quot;Flash Player Security&quot; chapter
        /// in Programming ActionScript 3.0.
        /// </summary>
        public extern virtual bool parentAllowsChild
        {
            [PageFX.AbcInstanceTrait(17)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The Loader object associated with this LoaderInfo object. If this LoaderInfo object
        /// is the loaderInfo property of the instance of the main class of the SWF file, no
        /// Loader object is associated.
        /// </summary>
        public extern virtual flash.display.Loader loader
        {
            [PageFX.AbcInstanceTrait(18)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>The loaded object associated with this LoaderInfo object.</summary>
        public extern virtual flash.display.DisplayObject content
        {
            [PageFX.AbcInstanceTrait(19)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual flash.utils.ByteArray bytes
        {
            [PageFX.AbcInstanceTrait(20)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual flash.events.UncaughtErrorEvents uncaughtErrorEvents
        {
            [PageFX.AbcInstanceTrait(22)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [PageFX.Event("httpStatus")]
        public event flash.events.HTTPStatusEventHandler httpStatus
        {
            add { }
            remove { }
        }

        [PageFX.Event("unload")]
        public event flash.events.EventHandler unload
        {
            add { }
            remove { }
        }

        [PageFX.Event("progress")]
        public event flash.events.ProgressEventHandler progress
        {
            add { }
            remove { }
        }

        [PageFX.Event("open")]
        public event flash.events.EventHandler open
        {
            add { }
            remove { }
        }

        [PageFX.Event("ioError")]
        public event flash.events.IOErrorEventHandler ioError
        {
            add { }
            remove { }
        }

        [PageFX.Event("init")]
        public event flash.events.EventHandler init
        {
            add { }
            remove { }
        }

        [PageFX.Event("complete")]
        public event flash.events.EventHandler complete
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern LoaderInfo();

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override bool dispatchEvent(flash.events.Event @event);

        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.display.LoaderInfo getLoaderInfoByDefinition(object @object);
    }
}
