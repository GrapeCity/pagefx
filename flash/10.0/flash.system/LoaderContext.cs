using System;
using System.Runtime.CompilerServices;

namespace flash.system
{
    /// <summary>
    /// The LoaderContext class provides options for loading SWF files and other media by using the Loader class.
    /// The LoaderContext class is used as the context  parameter in the load()  and
    /// loadBytes()  methods of the Loader class.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class LoaderContext : Avm.Object
    {
        /// <summary>
        /// Specifies the application domain to use for the Loader.load() or
        /// Loader.loadBytes() method.  Use this property only when loading a SWF file
        /// written in ActionScript 3.0 (not an image or a SWF file written in ActionScript 1.0 or ActionScript 2.0).
        /// Every security domain is divided into one or more application domains, represented
        /// by ApplicationDomain objects.  Application domains are not for security
        /// purposes; they are for managing cooperating units of ActionScript code.  If you are
        /// loading a SWF file from another domain, and allowing it to be placed in a separate
        /// security domain, then you cannot control the choice of application domain into which the
        /// loaded SWF file is placed; and if you have specified a choice of application domain, it
        /// will be ignored.  However, if you are loading a SWF file into your own security domain —
        /// either because the SWF file comes from your own domain, or because you are importing it into
        /// your security domain — then you can control the choice of application domain for the
        /// loaded SWF file.You can pass an application domain only from your own security domain in
        /// LoaderContext.applicationDomain.  Attempting to pass an application domain
        /// from any other security domain results in a SecurityError exception.You have four choices for what kind of ApplicationDomain property to use:Child of loader&apos;s ApplicationDomain. The default. You can
        /// explicitly represent this choice with the syntax
        /// new ApplicationDomain(ApplicationDomain.currentDomain). This allows the
        /// loaded SWF file to use the parent&apos;s classes directly, for example by writing
        /// new MyClassDefinedInParent().  The parent, however, cannot use this syntax;
        /// if the parent wishes to use the child&apos;s classes, it must call
        /// ApplicationDomain.getDefinition() to retrieve them.  The advantage of
        /// this choice is that, if the child defines a class with the same name as a class already
        /// defined by the parent, no error results; the child simply inherits the parent&apos;s
        /// definition of that class, and the child&apos;s conflicting definition goes unused unless
        /// either child or parent calls the ApplicationDomain.getDefinition() method to retrieve
        /// it.Loader&apos;s own ApplicationDomain.  You use this application domain when using
        /// ApplicationDomain.currentDomain.  When the load is complete, parent and
        /// child can use each other&apos;s classes directly.  If the child attempts to define a
        /// class with the same name as a class already defined by the parent, an error results
        /// and the load is abandoned.Child of the system ApplicationDomain.  You use this application domain when using
        /// new ApplicationDomain(null).  This separates loader and loadee entirely,
        /// allowing them to define separate versions of classes with the same name without conflict
        /// or overshadowing.  The only way either side sees the other&apos;s classes is by calling the
        /// ApplicationDomain.getDefinition() method.Child of some other ApplicationDomain.  Occasionally you may have
        /// a more complex ApplicationDomain hierarchy.  You can load a SWF file into any
        /// ApplicationDomain from your own SecurityDomain.  For example,
        /// new ApplicationDomain(ApplicationDomain.currentDomain.parentDomain.parentDomain)
        /// loads a SWF file into a new child of the current domain&apos;s parent&apos;s parent.When a load is complete, either side (loading or loaded) may need to find its own
        /// ApplicationDomain, or the other side&apos;s ApplicationDomain, for the purpose of calling
        /// ApplicationDomain.getDefinition().  Either side can retrieve a reference to
        /// its own application domain by using ApplicationDomain.currentDomain.  The loading
        /// SWF file can retrieve a reference to the loaded SWF file&apos;s ApplicationDomain via
        /// Loader.contentLoaderInfo.applicationDomain.  If the loaded SWF file knows how it
        /// was loaded, it can find its way to the loading SWF file&apos;s ApplicationDomain object.  For example, if
        /// the child was loaded in the default way, it can find the loading SWF file&apos;s application domain
        /// by using ApplicationDomain.currentDomain.parentDomain.For more information, see the &quot;ApplicationDomain class&quot; section of the &quot;Client System
        /// Environment&quot; chapter of Programming ActionScript 3.0.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public ApplicationDomain applicationDomain;

        /// <summary>
        /// Specifies whether Flash Player should attempt to download a cross-domain policy file from the
        /// loaded object&apos;s server before beginning to load the object itself.  This flag is applicable to
        /// the Loader.load() method, but not to the Loader.loadBytes() method.
        /// Set this flag to true when you are loading an image (JPEG, GIF, or PNG) from outside the calling
        /// SWF file&apos;s own domain, and you expect to need access to the content of that image from ActionScript.
        /// Examples of accessing image content include referencing the Loader.content property
        /// to obtain a Bitmap object, and calling the BitmapData.draw() method to obtain a
        /// copy of the loaded image&apos;s pixels.  If you attempt one of these operations without having
        /// specified checkPolicyFile at loading time, you may get a SecurityError
        /// exception because the needed policy file has not been downloaded yet.When you call the Loader.load() method with LoaderContext.checkPolicyFile set to
        /// true, Flash Player does not begin downloading the specified object in URLRequest.url
        /// until it has either successfully downloaded a relevant cross-domain policy file or discovered
        /// that no such policy file exists.  Flash Player first considers policy files that have already
        /// been downloaded, then attempts to download any pending policy files specified in calls to
        /// the Security.loadPolicyFile() method, then attempts to download a policy file from the default
        /// location that corresponds to URLRequest.url, which is /crossdomain.xml
        /// on the same server as URLRequest.url.  In all cases, Flash Player requires
        /// that the given policy file exist on its server, that it provide access to the object at
        /// URLRequest.url by virtue of the policy file&apos;s location, and that it permit access
        /// by the domain of the calling SWF file by virtue of one or more &lt;allow-access-from&gt;
        /// tags.If you set checkPolicyFile to true, Flash Player waits until policy file completion
        /// to begin the main download that you specify in the Loader.load() method.  Therefore, as long as the
        /// policy file that you need exists, as soon as you have received any ProgressEvent.PROGRESS or
        /// Event.COMPLETE events from the contentLoaderInfo property of your Loader object,
        /// the policy file download is complete, and you can safely begin performing operations that require
        /// the policy file.If you set checkPolicyFile to true, and no relevant policy file is found,
        /// you will not receive any error indication until you attempt an operation that throws a
        /// SecurityError exception.  However, once the LoaderInfo object dispatches a
        /// ProgressEvent.PROGRESS or Event.COMPLETE event, you can test whether a relevant
        /// policy file was found by checking the value of the LoaderInfo.childAllowsParent property.If you will not need pixel-level access to the image that you are loading, you should not set the
        /// checkPolicyFile property to true.  Checking for a policy file in this case is
        /// wasteful, because it may delay the start of your download, and it may consume network bandwidth unnecessarily.Also try to avoid setting checkPolicyFile to true if you are using the
        /// Loader.load() method to download a SWF file.  This is because SWF-to-SWF permissions are not
        /// controlled by policy files, but rather by the Security.allowDomain() method, and thus
        /// checkPolicyFile has no effect when you load a SWF file.  Checking for a policy file in
        /// this case is wasteful, because it may delay the download of the SWF file, and it may consume
        /// network bandwidth unnecessarily.  (Flash Player cannot tell whether your main download will be a
        /// SWF file or an image, because the policy file download occurs before the main download.)Be careful with checkPolicyFile if you are downloading an object from a URL that
        /// may use server-side HTTP redirects.  Flash Player always attempts to retrieve policy files
        /// that correspond to the initial URL that you specify in URLRequest.url.  If the final
        /// object comes from a different URL because of HTTP redirects, then the initially downloaded policy
        /// files might not be applicable to the object&apos;s final URL, which is the URL that matters in
        /// security decisions.  If you find yourself in this situation, you can examine the value of
        /// LoaderInfo.url after you have received a ProgressEvent.PROGRESS
        /// or Event.COMPLETE event, which tells you the object&apos;s final URL. Then call the
        /// Security.loadPolicyFile() method with a policy file URL based on the object&apos;s final
        /// URL. Then poll the value of LoaderInfo.childAllowsParent until it becomes true.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public bool checkPolicyFile;

        /// <summary>
        /// Specifies the security domain to use for a Loader.load() operation. Use this property
        /// only when loading a SWF file (not an image).
        /// The choice of security domain is meaningful only if you are loading a SWF file that might
        /// come from a different domain (a different server) than the loading SWF file.  When you load a
        /// SWF file from your own domain, it is always placed into your security domain.  But when you
        /// load a SWF file from a different domain, you have two options.  You can allow the loaded SWF file to
        /// be placed in its &quot;natural&quot; security domain, which is different from that of the
        /// loading SWF file; this is the default.  The other option is to specify that you want to place the
        /// loaded SWF file placed into the same security domain as the loading SWF file, by setting
        /// myLoaderContext.securityDomain to be equal to SecurityDomain.currentDomain.  This is
        /// called import loading, and it is equivalent, for security purposes, to copying the
        /// loaded SWF file to your own server and loading it from there.  In order for import loading to
        /// succeed, the loaded SWF file&apos;s server must have a policy file trusting the domain of the
        /// loading SWF file.You can pass your own security domain only in LoaderContext.securityDomain.
        /// Attempting to pass any other security domain results in a SecurityError exception.For more information, see the &quot;Security&quot; chapter in Programming ActionScript 3.0.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public SecurityDomain securityDomain;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern LoaderContext(bool arg0, ApplicationDomain arg1, SecurityDomain arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern LoaderContext(bool arg0, ApplicationDomain arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern LoaderContext(bool arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern LoaderContext();
    }
}
