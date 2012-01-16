using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// The LocalConnection class lets you create a LocalConnection object that can invoke a method in another
    /// LocalConnection object. The communication can be:
    /// Within a single SWF fileBetween multiple SWF filesBetween content (SWF-based or HTML-based) in AIR applicationsBetween content (SWF-based or HTML-based) in an AIR application and SWF content running in a browser
    /// </summary>
    [PageFX.AbcInstance(125)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class LocalConnection : flash.events.EventDispatcher
    {
        /// <summary>
        /// A string representing the domain of the location of the current file.
        /// In content running in the application security sandbox in the Adobe Integrated Runtime (content
        /// installed with the AIR application), the runtime uses the string app# followed by the application
        /// ID for the AIR application (defined in the application descriptor file) in place of the superdomain.
        /// For example a connectionName for an application with the application ID com.example.apollo.MyAppconnectionName resolves to &quot;app#com.example.apollo.MyApp:connectionName&quot;.In SWF files published for Flash Player 9 or later, the returned string is the exact domain of
        /// the file, including subdomains. For example, if the file is located at www.adobe.com, this command
        /// returns &quot;www.adobe.com&quot;. If the current file is a local file residing on the client computer running in Flash Player,
        /// this command returns &quot;localhost&quot;.The most common ways to use this property are to include the domain name of the sending
        /// LocalConnection object as a parameter to the method you plan to invoke in the receiving
        /// LocalConnection object, or to use it with LocalConnection.allowDomain() to accept commands
        /// from a specified domain. If you are enabling communication only between LocalConnection objects
        /// that are located in the same domain, you probably don&apos;t need to use this property.
        /// </summary>
        public extern virtual Avm.String domain
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Indicates the object on which callback methods are invoked. The default object
        /// is this, the local connection being created. You can set the
        /// client property to another object, and callback methods are
        /// invoked on that other object.
        /// </summary>
        public extern virtual object client
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool isPerUser
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern static bool isSupported
        {
            [PageFX.AbcClassTrait(0)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [PageFX.Event("status")]
        public event flash.events.StatusEventHandler status
        {
            add { }
            remove { }
        }

        [PageFX.Event("securityError")]
        public event flash.events.SecurityErrorEventHandler securityError
        {
            add { }
            remove { }
        }

        [PageFX.Event("asyncError")]
        public event flash.events.AsyncErrorEventHandler asyncError
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern LocalConnection();

        /// <summary>
        /// Closes (disconnects) a LocalConnection object. Issue this command when you no longer want the object
        /// to accept commands — for example, when you want to issue a connect()
        /// command using the same connectionName parameter in another SWF file.
        /// </summary>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void close();

        /// <summary>
        /// Prepares a LocalConnection object to receive commands from a send() command
        /// (called the sending LocalConnection object). The object used with this command is
        /// called the receiving LocalConnection object. The receiving and sending objects
        /// must be running on the same client computer.
        /// To avoid a race condition, define the methods attached to the
        /// receiving LocalConnection object before
        /// calling this method, as shown in the LocalConnection class example. By default, the connectionName argument is resolved into a value of
        /// &quot;superdomain:connectionName&quot;,
        /// where superdomain is the superdomain of the file that contains the
        /// connect() command. For example, if the file that contains the
        /// receiving LocalConnection object is located at www.someDomain.com, connectionName
        /// resolves to &quot;someDomain.com:connectionName&quot;. (If a file running in Flash Player
        /// is located on the client computer, the value assigned to superdomain is
        /// &quot;localhost&quot;.)In content running in the application security sandbox in the Adobe Integrated Runtime (content
        /// installed with the AIR application), the runtime uses the string app# followed by the application
        /// ID for the AIR application (defined in the application descriptor file) in place of the superdomain.
        /// For example a connectionName for an application with the application ID com.example.apollo.MyAppconnectionName resolves to &quot;app#com.example.apollo.MyApp:connectionName&quot;.Also by default, Flash Player lets the receiving LocalConnection object accept commands only from
        /// sending LocalConnection objects whose connection name also resolves into a value of
        /// &quot;superdomain:connectionName&quot;. In this way, Flash Player makes
        /// it simple for files that are located in the same domain to communicate with each other.If you are implementing communication only between files in the same domain, specify a string
        /// for connectionName that does not begin with an underscore (_) and that does not specify
        /// a domain name (for example, &quot;myDomain:connectionName&quot;). Use the same string in the
        /// connect(connectionName) method.If you are implementing communication between files in different domains, specifying a string
        /// for connectionName that begins with an underscore (_) makes the file with the
        /// receiving LocalConnection object more portable between domains. Here are the two possible cases:If the string for connectionNamedoes not begin with an underscore (_),
        /// a prefix is added with the superdomain and a colon (for example,
        /// &quot;myDomain:connectionName&quot;). Although this ensures that your connection does not conflict
        /// with connections of the same name from other domains, any sending LocalConnection objects must
        /// specify this superdomain (for example, &quot;myDomain:connectionName&quot;).
        /// If the file with the receiving LocalConnection object is moved to another domain, the player changes
        /// the prefix to reflect the new superdomain (for example, &quot;anotherDomain:connectionName&quot;).
        /// All sending LocalConnection objects would have to be manually edited to point to the new superdomain.If the string for connectionNamebegins with an underscore (for example,
        /// &quot;_connectionName&quot;), a prefix is not added to the string. This means that
        /// the receiving and sending LocalConnection objects use identical strings for
        /// connectionName. If the receiving object uses allowDomain()
        /// to specify that connections from any domain will be accepted, the file with the receiving LocalConnection
        /// object can be moved to another domain without altering any sending LocalConnection objects.For more information, see the discussion in the class overview and the discussion
        /// of connectionName in send(), and also
        /// the allowDomain() and domain entries.Note: Colons are used as special characters to separate the superdomain from the
        /// connectionName string. A string for connectionName that contains a colon is
        /// not valid.When you use this method in content in security sandboxes other
        /// than then application security sandbox, consider the Flash PlayerAIR security model. By default, a LocalConnection object
        /// is associated with the sandbox of the  file that created it, and cross-domain calls to LocalConnection
        /// objects are not allowed unless you call the LocalConnection.allowDomain() method in the
        /// receiving file. You can prevent a file from using this method by setting the
        /// allowNetworking parameter of the the object and embed
        /// tags in the HTML page that contains the SWF content. However, in the Adobe Integrated Runtime,
        /// content in the application security sandbox (content installed with the AIR application)
        /// are not restricted by these security limitations.For more information, see the following:The security chapter in the
        /// Programming ActionScript 3.0 book and the latest comments on LiveDocsThe &quot;Understanding AIR Security&quot; section of the &quot;Getting started with Adobe AIR&quot; chapter
        /// in the Developing AIR Applications book.The Flash Player 9
        /// Security white paper
        /// </summary>
        /// <param name="connectionName">
        /// A string that corresponds to the connection name specified in the
        /// send() command that wants to communicate with the receiving LocalConnection object.
        /// </param>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void connect(Avm.String connectionName);

        /// <summary>
        /// Invokes the method named methodName on a connection opened with the
        /// connect(connectionName) method (the receiving LocalConnection
        /// object). The object used with this command is called the sending LocalConnection object.
        /// The SWF files that contain the sending and receiving objects must be running on the same client computer.
        /// There is a 40 kilobyte limit to the amount of data you can pass as parameters to this command.
        /// If send() throws an ArgumentError but your syntax is correct, try dividing the
        /// send() requests into multiple commands, each with less than 40K of data.As discussed in the connect() entry, the current superdomain in added to
        /// connectionName by default. If you are implementing communication between different domains,
        /// you need to define connectionName in both the sending and receiving LocalConnection
        /// objects in such a way that the current superdomain is not added to connectionName.
        /// You can do this in one of the following two ways:Use an underscore (_) at the beginning of connectionName in both the sending and
        /// receiving LocalConnection objects. In the file that contains the receiving object, use
        /// LocalConnection.allowDomain() to specify that connections from any domain will be accepted.
        /// This implementation lets you store your sending and receiving files in any domain.Include the superdomain in connectionName in the sending LocalConnection
        /// object — for example, myDomain.com:myConnectionName. In the receiving object, use
        /// LocalConnection.allowDomain() to specify that connections from the specified superdomain
        /// will be accepted (in this case, myDomain.com) or that connections from any domain will be accepted.Note: You cannot specify a superdomain in connectionName in the receiving
        /// LocalConnection object — you can do this in only the sending LocalConnection object.When you use this method in content in security sandboxes other
        /// than then application security sandbox, consider the Flash PlayerAIR security model. By default, a LocalConnection object
        /// is associated with the sandbox of the file that created it, and cross-domain calls to LocalConnection
        /// objects are not allowed unless you call the LocalConnection.allowDomain() method in the
        /// receiving file.  For SWF content running in the browser, ou can prevent a file from using this method by setting the
        /// allowNetworking parameter of the the object and embed
        /// tags in the HTML page that contains the SWF content. However, in the Adobe Integrated Runtime, content in the
        /// application security sandbox (content installed with the AIR application) are not
        /// restricted by these security limitations.For more information, see the following:The security chapter in the
        /// Programming ActionScript 3.0 book and the latest comments on LiveDocsThe &quot;Understanding AIR Security&quot; section of the &quot;Getting started with Adobe AIR&quot; chapter
        /// in the Developing AIR Applications book.The Flash Player 9 Security white paper
        /// </summary>
        /// <param name="connectionName">
        /// Corresponds to the connection name specified in the connect() command
        /// that wants to communicate with the sending LocalConnection object.
        /// </param>
        /// <param name="methodName">
        /// The name of the method to be invoked in the receiving LocalConnection object. The
        /// following method names cause the command to fail: send, connect,
        /// close, allowDomain, allowInsecureDomain,
        /// client, and domain.
        /// </param>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void send(Avm.String connectionName, Avm.String methodName);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(Avm.String connectionName, Avm.String methodName, object rest0);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(Avm.String connectionName, Avm.String methodName, object rest0, object rest1);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(Avm.String connectionName, Avm.String methodName, object rest0, object rest1, object rest2);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(Avm.String connectionName, Avm.String methodName, object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(Avm.String connectionName, Avm.String methodName, object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(Avm.String connectionName, Avm.String methodName, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(Avm.String connectionName, Avm.String methodName, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(Avm.String connectionName, Avm.String methodName, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(Avm.String connectionName, Avm.String methodName, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(Avm.String connectionName, Avm.String methodName, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        /// <summary>
        /// Specifies one or more domains that can send LocalConnection calls to this LocalConnection instance.
        /// When using this method, consider the Flash Player security model. By default, a LocalConnection object
        /// is associated with the sandbox of the file that created it, and cross-domain calls to LocalConnection
        /// objects are not allowed unless you call the LocalConnection.allowDomain() method in the
        /// receiving file. However, in the Adobe Integrated Runtime, content in the application security sandbox
        /// (content installed with the AIR application) are not restricted by these security limitations.For more information, see the following:The security chapter in the
        /// Programming ActionScript 3.0 book and the latest comments on LiveDocsThe &quot;Understanding AIR Security&quot; section of the &quot;Getting started with Adobe AIR&quot; chapter
        /// in the Developing AIR Applications book.The Flash Player 9 Security white paperNote: The allowDomain() method has changed
        /// from the form it had in ActionScript 1.0 and 2.0.  In those earlier versions,
        /// allowDomain was a callback method that you
        /// implemented.  In ActionScript 3.0, allowDomain() is a built-in
        /// method of LocalConnection that you call.  With this change, allowDomain()
        /// works in much the same way as flash.system.Security.allowDomain().
        /// </summary>
        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void allowDomain();

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void allowDomain(object rest0);

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void allowDomain(object rest0, object rest1);

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void allowDomain(object rest0, object rest1, object rest2);

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void allowDomain(object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void allowDomain(object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void allowDomain(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void allowDomain(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void allowDomain(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void allowDomain(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void allowDomain(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        /// <summary>
        /// Specifies one or more domains that can send LocalConnection calls to this LocalConnection object.
        /// The allowInsecureDomain() method works just like the allowDomain() method,
        /// except that the allowInsecureDomain() method additionally permits SWF files
        /// from non-HTTPS origins to send LocalConnection calls to files from HTTPS origins.  This difference
        /// is meaningful only if you call the allowInsecureDomain() method from a
        /// file that was loaded using HTTPS.  You must call the allowInsecureDomain() method even
        /// if you are crossing a non-HTTPS/HTTPS boundary within the same domain; by default, LocalConnection calls
        /// are never permitted from non-HTTPS SWF files to HTTPS SWF files, even within the same domain.Calling allowInsecureDomain() is not recommended,
        /// because it can compromise the security offered by HTTPS.  When you
        /// load a file over HTTPS, you can be reasonably sure that the file
        /// will not be tampered with during delivery over the network.  If you
        /// then permit a non-HTTPS file to make LocalConnection calls to the
        /// HTTPS file, you are accepting calls from a file that may in fact have
        /// been tampered with during delivery.  This generally requires extra
        /// vigilance because you cannot trust the authenticity of LocalConnection
        /// calls arriving at your HTTPS file.By default, files hosted using the HTTPS protocol can be accessed only by other files hosted
        /// using the HTTPS protocol. This implementation maintains the integrity provided by the HTTPS protocol.Using this method to override the default behavior is not recommended, because it compromises HTTPS security.
        /// However, you might need to do so, for example, if you need to permit access to HTTPS SWF files published for
        /// Flash Player 9 or later from HTTP files SWF published for Flash Player 6 or earlier.For more information, see the following:The security chapter in the
        /// Programming ActionScript 3.0 book and the latest comments on LiveDocsThe &quot;Understanding AIR Security&quot; section of the &quot;Getting started with Adobe AIR&quot; chapter
        /// in the Developing AIR Applications book.The Flash Player 9 Security white paper
        /// </summary>
        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void allowInsecureDomain();

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void allowInsecureDomain(object rest0);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void allowInsecureDomain(object rest0, object rest1);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void allowInsecureDomain(object rest0, object rest1, object rest2);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void allowInsecureDomain(object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void allowInsecureDomain(object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void allowInsecureDomain(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void allowInsecureDomain(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void allowInsecureDomain(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void allowInsecureDomain(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void allowInsecureDomain(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);


    }
}
