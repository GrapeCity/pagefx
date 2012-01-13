using System;
using System.Runtime.CompilerServices;

namespace flash.system
{
    /// <summary>
    /// The Security class lets you specify how SWF files in different domains can communicate with
    /// each other.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class Security : Avm.Object
    {
        /// <summary>
        /// The SWF file is a local file and has been trusted by the user,
        /// using either the Settings Manager or a FlashPlayerTrust configuration
        /// file. The SWF file can read from local data sources and communicate
        /// with the Internet.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String LOCAL_TRUSTED;

        /// <summary>The SWF file is from an Internet URL and operates under domain-based sandbox rules.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String REMOTE;

        /// <summary>
        /// The SWF file is a local file, has not been trusted by the user,
        /// and was not published with a networking designation. The SWF file may
        /// read from local data sources but may not communicate with the Internet.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String LOCAL_WITH_FILE;

        /// <summary>
        /// The SWF file is a local file, has not been trusted by the user, and
        /// was published with a networking designation. The SWF file can
        /// communicate with the Internet but cannot read from local data sources.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String LOCAL_WITH_NETWORK;

        public extern static Avm.String sandboxType
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static bool exactSettings
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

        public extern static bool disableAVM1Loading
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

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Security();

        /// <summary>Displays the Security Settings panel in Flash Player.</summary>
        /// <param name="arg0">
        /// (default = &quot;default&quot;)  A value from the SecurityPanel class that specifies which Security Settings
        /// panel you want to display. If you omit this parameter, SecurityPanel.DEFAULT is used.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void showSettings(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void showSettings();

        /// <summary>
        /// Lets SWF files and HTML files in the identified domains access objects and variables in the
        /// SWF file that contains the allowDomain() call.
        /// If two SWF files are served from the same domain — for example, http://mysite.com/swfA.swf and
        /// http://mysite.com/swfB.swf — then swfA.swf can examine and modify variables, objects, properties,
        /// methods, and so on in swfB.swf, and swfB.swf can do the same for swfA.swf. This is called cross-movie
        /// scripting or cross-scripting.If two SWF files are served from different domains — for example, http://siteA.com/swfA.swf and
        /// http://siteB.com/siteB.swf — then, by default, Flash Player does not allow swfA.swf to script
        /// swfB.swf, nor swfB.swf to script swfA.swf. A SWF file gives SWF files from other domains
        /// by calling Security.allowDomain(). This is
        /// called cross-domain scripting. By calling Security.allowDomain(&quot;siteA.com&quot;), siteB.swf
        /// gives siteA.swf permission to script it.In any cross-domain situation, it is important to be clear about the two parties involved.
        /// For the purposes of this discussion, the side performing the cross-scripting
        /// is called the accessing party (usually the accessing SWF), and the other side is called the party being accessed
        /// (usually the SWF being accessed). When siteA.swf scripts siteB.swf,
        /// siteA.swf is the accessing party, and siteB.swf is the party being accessed.Cross-domain permissions that are established with allowDomain() are asymmetrical.
        /// In the previous example, siteA.swf can script siteB.swf, but siteB.swf cannot script siteA.swf,
        /// because siteA.swf has not called allowDomain() to give SWF files at siteB.com permission
        /// to script it. You can set up symmetrical permissions by having both SWF files call
        /// allowDomain().In addition to protecting SWF files from cross-domain scripting originated by other SWF files, Flash Player
        /// protects SWF files from cross-domain scripting originated by HTML files. HTML-to-SWF scripting can
        /// occur with older Flash browser functions such as SetVariable or callbacks
        /// established through ExternalInterface.addCallback(). When HTML-to-SWF scripting crosses
        /// domains, the SWF file being accessed must call allowDomain(),
        /// just as when the accessing party is a SWF file, or the operation will fail.Specifying an IP address as a parameter to allowDomain()
        /// does not permit access by all parties that originate at the specified IP address.
        /// Instead, it permits access only by a party that contains the specified IP address it its URL,
        /// rather than a domain name that maps to that IP address.Version-specific differencesFlash Player&apos;s cross-domain security rules have evolved from version to version.
        /// The following table summarizes the differences.Latest SWF version involved in cross-scriptingallowDomain() needed?allowInsecureDomain() needed?Which SWF must call allowDomain() or allowInsecureDomain()?What can be specified in allowDomain() or allowInsecureDomain()?5 or earlierNoNoN/A6Yes, if superdomains don&apos;t match The SWF file being accessed, or any SWF file with the same superdomain as the SWF file being accessedText-based domain (mysite.com)IP address (192.168.1.1)7Yes, if domains don&apos;t match exactlyYes, if performing HTTP-to-HTTPS access (even if domains match exactly)The SWF file being accessed, or any SWF file with exactly the same domain as the SWF file being accessed8 or laterSWF being accessedText-based domain (mysite.com)IP address (192.168.1.1)Wildcard (*)The versions that control the behavior of Flash Player are SWF versions
        /// (the published version of a SWF file), not the version of Flash Player itself.
        /// For example, when Flash Player 8 is playing a SWF file published for version 7, it
        /// applies behavior that is consistent with version 7. This practice ensures that player upgrades do not
        /// change the behavior of Security.allowDomain() in deployed SWF files.The version column in the previous table shows the latest SWF version involved in a cross-scripting
        /// operation. Flash Player determines its behavior according to either the accessing SWF file&apos;s
        /// version or the version of the SWF file that is being accessed, whichever is later.The following paragraphs provide more detail about Flash Player security changes involving
        /// Security.allowDomain().Version 5. There are no cross-domain scripting restrictions.Version 6. Cross-domain scripting security is introduced. By default, Flash Player forbids
        /// cross-domain scripting; Security.allowDomain() can permit it. To determine whether two files are
        /// in the same domain, Flash Player uses each file&apos;s superdomain, which is the exact host name from the
        /// file&apos;s URL, minus the first segment, down to a minimum of two segments. For example, the superdomain of
        /// www.mysite.com is mysite.com. SWF files from www.mysite.com and
        /// store.mysite.com to script each other without a call to Security.allowDomain().Version 7. Superdomain matching is changed to exact domain matching. Two files are
        /// permitted to script each other only if the host names in their URLs are identical; otherwise, a call to
        /// Security.allowDomain() is required. By default, files loaded from non-HTTPS URLs are no longer
        /// permitted to script files loaded from HTTPS URLs, even if the files are loaded from exactly the same
        /// domain. This restriction helps protect HTTPS files, because a non-HTTPS file is vulnerable to
        /// modification during download, and a maliciously modified non-HTTPS file could corrupt an HTTPS file,
        /// which is otherwise immune to such tampering. Security.allowInsecureDomain() is introduced to
        /// allow HTTPS SWF files that are being accessed to voluntarily disable this restriction, but the use of
        /// Security.allowInsecureDomain() is discouraged.Version 8. There are two major areas of change:Calling Security.allowDomain() now permits cross-scripting operations
        /// only if the SWF file being accessed is the SWF file that called Security.allowDomain().
        /// In other words, a SWF file that calls Security.allowDomain() now permits access only to itself.
        /// In previous versions, calling Security.allowDomain() permitted cross-scripting operations
        /// where the SWF file being accessed could be any SWF file in the same domain as the SWF file that called
        /// Security.allowDomain(). Calling Security.allowDomain() previously opened up
        /// the entire domain of the calling SWF file.Support has been added for wildcard values with Security.allowDomain(&quot;*&quot;) and
        /// Security.allowInsecureDomain(&quot;*&quot;).
        /// The wildcard (*) value permits cross-scripting operations where the accessing file is any file at all,
        /// loaded from anywhere. Think of the wildcard as a global permission. Wildcard permissions
        /// are required to enable certain kinds of operations
        /// under the local file security rules. Specifically,
        /// for a local SWF file with network-access permissions to script a SWF file on the
        /// Internet, the Internet SWF file being accessed must call Security.allowDomain(&quot;*&quot;),
        /// reflecting that the origin of a local SWF file is unknown. (If the Internet SWF file is loaded from an
        /// HTTPS URL, the Internet SWF file must instead call Security.allowInsecureDomain(&quot;*&quot;).)Occasionally, you may encounter the following situation: You load a child SWF file
        /// from a different domain and want to allow the child SWF file to script the parent SWF file,
        /// but you don&apos;t know the final domain of the child SWF file. This can happen, for
        /// example, when you use load-balancing redirects or third-party servers.In this situation, you can use the url property of the URLRequest object
        /// that you pass to Loader.load(). For example, if you load a child SWF file
        /// into a parent SWF, you can access the contentLoaderInfo property of the Loader
        /// object for the parent SWF: Security.allowDomain(loader.contentLoaderInfo.url)Make sure that you wait until the child SWF file begins loading to get the correct
        /// value of the url property. To determine when the child SWF has begun loading,
        /// use the progress event.The opposite situation can also occur; that is, you might create a child SWF file
        /// that wants to allow its parent to script it, but doesn&apos;t know what the domain of its parent
        /// will be. In this situation, you can access the loaderInfo property
        /// of the display object that is the SWF&apos;s root object. In the child SWF, call
        /// Security.allowDomain( this.root.loaderInfo.loaderURL).
        /// You don&apos;t have to wait for the parent SWF file to load; the parent will already be
        /// loaded by the time the child loads.If you are publishing for Flash Player 8 or later, you can also handle these situations by calling
        /// Security.allowDomain(&quot;*&quot;). However, this can sometimes be a dangerous shortcut,
        /// because it allows the calling SWF file to be accessed by any other SWF file from any domain.
        /// It is usually safer to use the _url property.For more information, see the following:The security chapter in the
        /// Programming ActionScript 3.0 book and the latest comments on LiveDocs
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowDomain();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowDomain(object rest0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowDomain(object rest0, object rest1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowDomain(object rest0, object rest1, object rest2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowDomain(object rest0, object rest1, object rest2, object rest3);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowDomain(object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowDomain(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowDomain(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowDomain(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowDomain(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowDomain(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        /// <summary>
        /// Lets SWF files and HTML files in the identified domains access objects and variables in the calling
        /// SWF file, which is hosted by means of the HTTPS protocol. This method is not recommended;
        /// see &quot;Security considerations,&quot; later in this entry.
        /// This method works in the same way as Security.allowDomain(), but it also
        /// permits operations in which the accessing party is loaded with a non-HTTPS protocol, and the
        /// party being accessed is loaded with HTTPS. In Flash Player 7 and later,
        /// non-HTTPS files are not allowed to script HTTPS files. The allowInsecureDomain() method lifts this
        /// restriction when the HTTPS SWF file being accessed uses it.Use allowInsecureDomain() only to enable scripting from non-HTTPS files
        /// to HTTPS files. Use it to enable scripting when the accessing non-HTTPS file and
        /// the HTTPS file being accessed are served from the same domain, for example, if a SWF file at
        /// http://mysite.com wants to script a SWF file at https://mysite.com. Do not use this method to enable
        /// scripting between non-HTTPS files, between HTTPS files, or from HTTPS files to non-HTTPS
        /// files. For those situations, use allowDomain() instead.Security considerations:
        /// Flash Player provides allowInsecureDomain() to maximize flexibility, but
        /// calling this method is not recommended. Serving a file over HTTPS provides several protections
        /// for you and your users, and calling allowInsecureDomain weakens one of those
        /// protections. The following scenario illustrates how allowInsecureDomain() can compromise security, if it is not used
        /// with careful consideration.Note that the following information is only one possible scenario, designed to
        /// help you understand allowInsecureDomain() through a real-world example
        /// of cross-scripting.
        /// It does not cover all issues with security architecture and should be used for background
        /// information only. The Flash Player Developer Center contains extensive information on Flash Player
        /// and security. For more information, see
        /// http://www.adobe.com/devnet/security/.Suppose you are building an e-commerce site that consists of two components:
        /// a catalog, which does not need to be secure, because it contains only public information;
        /// and a shopping cart/checkout component, which must be secure to protect users&apos; financial and
        /// personal information. Suppose you are considering serving the catalog from
        /// http://mysite.com/catalog.swf and the cart from https://mysite.com/cart.swf. One
        /// requirement for your site is that a third party should not be able to steal your
        /// users&apos; credit card numbers by taking advantage of a weakness in your security architecture.Suppose that a middle-party attacker intervenes between your server and your users, attempting
        /// to steal the credit card numbers that your users enter into your shopping cart application.
        /// A middle party might, for example, be an unscrupulous ISP used by some of your users, or a
        /// malicious administrator at a user&apos;s workplace — anyone who has the ability to view or alter
        /// network packets transmitted over the public Internet between your users and your servers.
        /// This situation is not uncommon.If cart.swf uses HTTPS to transmit credit card information to your servers, then the
        /// middle-party attacker can&apos;t directly steal this information from network packets, because the
        /// HTTPS transmission is encrypted. However, the attacker can use a different technique: altering the
        /// contents of one of your SWF files as it is delivered to the user, replacing your SWF file with an
        /// altered version that transmits the user&apos;s information to a different server, owned by the attacker.The HTTPS protocol, among other things, prevents this &quot;modification&quot; attack from working,
        /// because, in addition to being encrypted, HTTPS transmissions are tamper-resistant.
        /// If a middle-party attacker alters a packet, the receiving side detects the alteration
        /// and discards the packet. So the attacker in this situation can&apos;t alter cart.swf, because it
        /// is delivered over HTTPS.However, suppose that you want to allow buttons in catalog.swf, served over HTTP,
        /// to add items to the shopping cart in cart.swf, served over HTTPS. To accomplish this,
        /// cart.swf calls allowInsecureDomain(), which allows catalog.swf to script cart.swf.
        /// This action has an unintended consequence: Now the attacker can alter
        /// catalog.swf as it is initially being downloaded by
        /// the user, because catalog.swf is delivered with HTTP and is not tamper-resistant.
        /// The attacker&apos;s altered catalog.swf can now script cart.swf, because cart.swf contains
        /// a call to allowInsecureDomain(). The altered catalog.swf file can use ActionScript to access
        /// the variables in cart.swf, thus reading the user&apos;s credit card information and other
        /// sensitive data. The altered catalog.swf can then send this data to an attacker&apos;s server.Obviously, this implementation is not desired, but you still want to allow
        /// cross-scripting between the two SWF files on your site. Here are two possible ways to redesign
        /// this hypothetical e-commerce site to avoid allowInsecureDomain():Serve all SWF files in the application over HTTPS. This is by far the simplest and most
        /// reliable solution. In the scenario described, you would serve both catalog.swf and cart.swf
        /// over HTTPS. You might experience slightly higher bandwidth consumption and server CPU load
        /// when switching a file such as catalog.swf from HTTP to HTTPS, and your users might experience
        /// slightly longer application load times. You need to experiment with real servers to
        /// determine the severity of these effects; usually they are no worse than 10-20% each, and
        /// sometimes they are not present at all. You can usually improve results by using HTTPS-accelerating
        /// hardware or software on your servers. A major benefit of serving all
        /// cooperating SWF files over HTTPS is that you can use an HTTPS URL as the main URL
        /// in the user&apos;s browser without generating any mixed-content warnings from the browser.
        /// Also, the browser&apos;s padlock icon becomes visible, providing your users with
        /// a common and trusted indicator of security.Use HTTPS-to-HTTP scripting, rather than HTTP-to-HTTPS scripting. In the scenario described, you
        /// could store the contents of the user&apos;s shopping cart in catalog.swf, and have cart.swf manage
        /// only the checkout process. At checkout time, cart.swf could retrieve the cart contents from
        /// ActionScript variables in catalog.swf. The restriction on HTTP-to-HTTPS scripting is asymmetrical;
        /// although an HTTP-delivered catalog.swf file cannot safely be allowed to script an HTTPS-delivered cart.swf file,
        /// an HTTPS cart.swf file can script the HTTP catalog.swf file.
        /// This approach is more delicate than the all-HTTPS approach; you must be careful not to trust any
        /// SWF file delivered over HTTP, because of its vulnerability to tampering. For example, when cart.swf
        /// retrieves the ActionScript variable that describes the cart contents, the ActionScript code
        /// in cart.swf cannot trust that the value of this variable is in the format that you expect.
        /// You must verify that the cart contents do not contain invalid data that might
        /// lead cart.swf to take an undesired action. You must also accept the risk that a middle party,
        /// by altering catalog.swf, could supply valid but inaccurate data to cart.swf; for example, by placing
        /// items in the user&apos;s cart. The usual checkout process mitigates
        /// this risk somewhat by displaying the cart contents and total cost for final approval by the user,
        /// but the risk remains present.Web browsers have enforced separation between HTTPS and non-HTTPS files for years,
        /// and the scenario described illustrates one good reason for this restriction.
        /// Flash Player gives you the ability to work around this security restriction when you
        /// absolutely must, but be sure to consider the consequences carefully before doing so.For more information, see the following:The security chapter in the
        /// Programming ActionScript 3.0 book and the latest comments on LiveDocs
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowInsecureDomain();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowInsecureDomain(object rest0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowInsecureDomain(object rest0, object rest1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowInsecureDomain(object rest0, object rest1, object rest2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowInsecureDomain(object rest0, object rest1, object rest2, object rest3);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowInsecureDomain(object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowInsecureDomain(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowInsecureDomain(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowInsecureDomain(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowInsecureDomain(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void allowInsecureDomain(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        /// <summary>
        /// Loads a cross-domain policy file from a location specified by the url
        /// parameter. Flash Player uses policy files to determine whether to permit Flash movies to load
        /// data from servers other than their own.
        /// By default, Flash Player looks for policy files in only one location: /crossdomain.xml on the
        /// server to which a data-loading request is being made.With Security.loadPolicyFile(), Flash Player can
        /// load policy files from arbitrary locations, as shown in the following example:
        /// Security.loadPolicyFile(&quot;http://www.example.com/sub/dir/pf.xml&quot;);
        /// This causes Flash Player to retrieve a policy file from the specified URL. Any permissions
        /// granted by the policy file at that location will apply to all content at the same level or lower in
        /// the virtual directory hierarchy of the server. For example, following the previous code, these
        /// lines do not throw an exception: import flash.net.~~;
        /// var request:URLRequest = new URLRequest(&quot;http://www.example.com/sub/dir/vars.txt&quot;);
        /// var loader:URLLoader = new URLLoader();
        /// loader.load(request);
        /// var loader2:URLLoader = new URLLoader();
        /// var request2:URLRequest = new URLRequest(&quot;http://www.example.com/sub/dir/deep/vars2.txt&quot;);
        /// loader2.load(request2);
        /// However, the following code does throw a security exception: import flash.net.~~;
        /// var request3:URLRequest = new URLRequest(&quot;http://www.example.com/elsewhere/vars3.txt&quot;);
        /// var loader3:URLLoader = new URLLoader();
        /// loader3.load(request3);
        /// You can use loadPolicyFile() to load any number of policy files. When considering a
        /// request that requires a policy file, Flash Player always waits for the completion of any policy
        /// file downloads before denying a request. As a final fallback, if no policy file specified with
        /// loadPolicyFile() authorizes a request, Flash Player consults the original default
        /// location, /crossdomain.xml.Using the xmlsocket protocol along with a specific port number, lets you retrieve
        /// policy files directly from an XMLSocket server, as shown in the following example:
        /// Security.loadPolicyFile(&quot;xmlsocket://foo.com:414&quot;);
        /// This causes Flash Player to attempt to retrieve a policy file from the specified host and port.
        /// Any port can be used, not only ports 1024 and higher. Upon establishing a connection with the
        /// specified port, Flash Player transmits &lt;policy-file-request /&gt;, terminated by a
        /// null byte. An XMLSocket server can be configured to serve both policy files and normal
        /// XMLSocket connections over the same port, in which case the server should wait for
        /// &lt;policy-file-request /&gt; before transmitting a policy file. A server can also be
        /// set up to serve policy files over a separate port from standard connections, in which case it can
        /// send a policy file as soon as a connection is established on the dedicated policy file port. The
        /// server must send a null byte to terminate a policy file, and may thereafter close the connection;
        /// if the server does not close the connection, Flash Player does so upon receiving the terminating
        /// null byte.A policy file served by an XMLSocket server has the same syntax as any other policy file, except
        /// that it must also specify the ports to which access is granted. When a policy file comes from a
        /// port lower than 1024, it can grant access to any ports; when a policy file comes from port 1024 or
        /// higher, it can grant access only to other ports 1024 and higher. The allowed ports are specified in
        /// a &quot;to-ports&quot; attribute in the &lt;allow-access-from&gt; tag. Single port
        /// numbers, port ranges, and wildcards are all allowed. The following example shows an XMLSocket
        /// policy file:
        /// &lt;cross-domain-policy&gt;
        /// &lt;allow-access-from domain=&quot;*&quot; to-ports=&quot;507&quot; /&gt;
        /// &lt;allow-access-from domain=&quot;*.foo.com&quot; to-ports=&quot;507,516&quot; /&gt;
        /// &lt;allow-access-from domain=&quot;*.bar.com&quot; to-ports=&quot;516-523&quot; /&gt;
        /// &lt;allow-access-from domain=&quot;www.foo.com&quot; to-ports=&quot;507,516-523&quot; /&gt;
        /// &lt;allow-access-from domain=&quot;www.bar.com&quot; to-ports=&quot;*&quot; /&gt;
        /// &lt;/cross-domain-policy&gt;
        /// A policy file obtained from the old default location—/crossdomain.xml on an HTTP server on port
        /// 80—implicitly authorizes access to all ports 1024 and above. There is no way to retrieve a policy
        /// file to authorize XMLSocket operations from any other location on an HTTP server; any custom
        /// locations for XMLSocket policy files must be on an XMLSocket server.The ability to connect to ports lower than 1024 can be granted by a policy file loaded with
        /// loadPolicyFile() only.You can prevent a SWF file from using this method by setting the
        /// allowNetworking parameter of the the object and embed
        /// tags in the HTML page that contains the SWF content.For more information, see the following:The security chapter in the
        /// Programming ActionScript 3.0 book and the latest comments on LiveDocs
        /// </summary>
        /// <param name="arg0">The URL location of the cross-domain policy file to be loaded.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void loadPolicyFile(Avm.String arg0);


    }
}
