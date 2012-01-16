using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// The SharedObject class is used to read and store limited amounts of data on a user&apos;s computer
    /// or on a server.
    /// Shared objects offer real-time data sharing between multiple client SWF files and objects
    /// that are persistent on the local computer or remote server. Local shared objects are similar
    /// to browser cookies and remote shared objects are similar to real-time data transfer devices.
    /// To use remote shared objects, you need a server, such as Adobe Flex or Adobe&apos;s Macromedia Flash Media Server 2.
    /// </summary>
    [PageFX.AbcInstance(185)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class SharedObject : flash.events.EventDispatcher
    {
        /// <summary>
        /// The collection of attributes assigned to the data property of the object; these attributes can
        /// be shared and stored. Each attribute can be an object of any ActionScript or JavaScript
        /// type — Array, Number, Boolean, ByteArray, XML, and so on. For example, the following lines assign values to various aspects
        /// of a shared object:
        /// var items_array:Array = new Array(101, 346, 483);
        /// var currentUserIsAdmin:Boolean = true;
        /// var currentUserName:String = &quot;Ramona&quot;;
        /// var my_so:SharedObject = SharedObject.getLocal(&quot;superfoo&quot;);
        /// my_so.data.itemNumbers = items_array;
        /// my_so.data.adminPrivileges = currentUserIsAdmin;
        /// my_so.data.userName = currentUserName;
        /// for (var prop in my_so.data) {
        /// trace(prop+&quot;: &quot;+my_so.data[prop]);
        /// }
        /// All attributes of a shared object&apos;s data property are saved if the object is persistent, and
        /// the shared object contains the following information:
        /// userName: Ramona
        /// adminPrivileges: true
        /// itemNumbers: 101,346,483
        /// Note: Do not assign values directly to the data property of a shared
        /// object, as in so.data = someValue; Flash Player ignores these assignments.To delete attributes for local shared objects, use code such as
        /// delete so.data.attributeName; setting an attribute to
        /// null or undefined for a local shared object does not
        /// delete the attribute.To create private values for a shared object — values that are available only to the client
        /// instance while the object is in use and are not stored with the object when it is closed — create properties
        /// that are not named data to store them, as shown in the following example:
        /// var my_so:SharedObject = SharedObject.getLocal(&quot;superfoo&quot;);
        /// my_so.favoriteColor = &quot;blue&quot;;
        /// my_so.favoriteNightClub = &quot;The Bluenote Tavern&quot;;
        /// my_so.favoriteSong = &quot;My World is Blue&quot;;
        /// for (var prop in my_so) {
        /// trace(prop+&quot;: &quot;+my_so[prop]);
        /// }
        /// The shared object contains the following data:
        /// favoriteSong: My World is Blue
        /// favoriteNightClub: The Bluenote Tavern
        /// favoriteColor: blue
        /// data: [object Object]
        /// </summary>
        public extern virtual object data
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The current size of the shared object, in bytes.
        /// Flash calculates the size of a shared object by stepping through all of its data
        /// properties; the more data properties the object has, the longer it takes to estimate its size.
        /// Estimating object size can take significant processing time, so you
        /// may want to avoid using this method unless you have a specific need for it.
        /// </summary>
        public extern virtual uint size
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies the number of times per second that a client&apos;s changes to a
        /// shared object are sent to the server.
        /// Use this method when you want to control the amount of traffic between
        /// the client and the server. For example, if the connection between the client
        /// and server is relatively slow, you may want to set fps
        /// to a relatively low value. Conversely, if the client is connected to a multiuser
        /// application in which timing is important, you may want to set fps
        /// to a relatively high value.Setting fps will trigger a sync event and update all changes to the server.
        /// If you only want to update the server manually, set fps to 0.Changes are not sent to the server until the sync event has been dispatched.
        /// That is, if the response time from the server is slow, updates may be sent to
        /// the server less frequently than the value specified in this property.
        /// </summary>
        public extern virtual double fps
        {
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The object encoding (AMF version) for this shared object. When a local shared object is written to disk,
        /// the objectEncoding property indicates which Action
        /// Message Format version should be used: the ActionScript 3.0 format (AMF3)
        /// or the ActionScript 1.0 or 2.0 format (AMF0).
        /// Object encoding is handled differently depending if the shared object
        /// is local or remote.Local shared objects. You can get or set the value of the
        /// objectEncoding property for local shared objects.
        /// The value of objectEncoding
        /// affects what formatting is used for writing this local shared object.
        /// If this local shared object must be readable by
        /// ActionScript 2.0 or 1.0 SWF files, set objectEncoding to
        /// ObjectEncoding.AMF0.
        /// Even if object encoding is set to write AMF3, Flash Player can still read AMF0 local shared objects.
        /// That is, if you use the default value of this property, ObjectEncoding.AMF3,
        /// your SWF file can still read shared objects created by ActionScript 2.0 or 1.0 SWF files.
        /// Remote shared objects. When connected to the server, a remote shared object
        /// inherits its objectEncoding setting from the associated NetConnection
        /// instance (the instance used to connect to the remote shared object). When not connected
        /// to the server, a remote shared object inherits the defaultObjectEncoding
        /// setting from the associated NetConnection instance.
        /// Because the value of a remote shared object&apos;s objectEncoding property is
        /// determined by the NetConnection instance, this property is read-only for remote shared objects.
        /// </summary>
        public extern virtual uint objectEncoding
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates the object on which
        /// callback methods are invoked. The default object is this.
        /// You can set the client property to another object, and callback methods will be
        /// invoked on that other object.
        /// </summary>
        public extern virtual object client
        {
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The default object encoding (AMF version) for all local shared objects created in the SWF file.
        /// When local shared objects are written to disk, the
        /// SharedObject.defaultObjectEncoding property
        /// indicates which Action Message Format version should be used: the ActionScript 3.0 format (AMF3) or the ActionScript 1.0 or 2.0 format (AMF0).
        /// For more information about object encoding, including the difference between
        /// encoding in local and remote shared objects, see the description of the
        /// objectEncoding property.The default value of SharedObject.defaultObjectEncoding is set to use the
        /// ActionScript 3.0 format, AMF3.
        /// If you need to write local shared objects that ActionScript 2.0 or 1.0 SWF files can read,
        /// set SharedObject.defaultObjectEncoding to use the
        /// ActionScript 1.0 or ActionScript 2.0 format, flash.net.ObjectEncoding.AMF0,
        /// at the beginning of your script, before you create any local shared objects.
        /// All local shared objects
        /// created thereafter will use AMF0 encoding and can interact with older content.
        /// You cannot change the objectEncoding value of existing local shared objects
        /// by setting SharedObject.defaultObjectEncoding after the local shared
        /// objects have been created.To set the object encoding on a per-object basis, rather than for all shared objects
        /// created by the SWF file, set the objectEncoding property of the local shared object instead.
        /// </summary>
        public extern static uint defaultObjectEncoding
        {
            [PageFX.AbcClassTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcClassTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [PageFX.Event("sync")]
        public event flash.events.SyncEventHandler sync
        {
            add { }
            remove { }
        }

        [PageFX.Event("netStatus")]
        public event flash.events.NetStatusEventHandler netStatus
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
        public extern SharedObject();

        /// <summary>
        /// Connects to a remote shared object on the server through the specified connection.
        /// Use this method after issuing SharedObject.getRemote().
        /// After a successful connection, the sync event is dispatched.
        /// Before attempting to work with a remote shared object,
        /// first check for a return value of true, indicating a successful connection,
        /// and then wait until the sync event is dispatched.
        /// If you fail to do so, any changes you make to the object
        /// locally — before the sync event is dispatched — may be lost.Note: The sync event is not dispatched if this call returns
        /// false.
        /// </summary>
        /// <param name="myConnection">
        /// A NetConnection object (such as one used to communicate with Flash Media Server)
        /// that is using the Real-Time Messaging Protocol (RTMP).
        /// </param>
        /// <param name="@params">(default = null)</param>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void connect(flash.net.NetConnection myConnection, Avm.String @params);

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void connect(flash.net.NetConnection myConnection);

        /// <summary>
        /// Closes the connection between a remote shared object and the server.
        /// If a remote shared object is locally persistent, the user can make changes
        /// to the local copy of the object after this method is called. Any changes made
        /// to the local object are sent to the server the next time the user connects
        /// to the remote shared object.
        /// </summary>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void close();

        /// <summary>
        /// Immediately writes a locally persistent shared object to a local file. If you don&apos;t use this
        /// method, Flash Player writes the shared object to a file when the shared object session ends —
        /// that is, when the SWF file is closed, when the shared object is garbage-collected
        /// because it no longer has any references to it, or when you call SharedObject.clear()
        /// or SharedObject.close().
        /// If this method returns SharedObjectFlushStatus.PENDING,
        /// Flash Player displays a dialog box asking
        /// the user to increase the amount of disk space available to objects from this domain. To allow
        /// space for the shared object to  grow when it is saved in the future, which avoids return values
        /// of PENDING, pass a value for minDiskSpace. When Flash Player
        /// tries to write the file, it looks for the number of bytes passed to
        /// minDiskSpace, instead of looking for enough space to save the shared
        /// object at its current size. For example, if you expect a shared object to grow to a maximum size of 500 bytes, even
        /// though it might start out much smaller, pass 500 for minDiskSpace. If
        /// Flash asks the user to allot disk space for the shared object, it asks for 500 bytes. After
        /// the user allots the requested amount of space, Flash won&apos;t have to ask for more space on future
        /// attempts to flush the object (as long as its size doesn&apos;t exceed 500 bytes). After the user responds to the dialog box, this method is called again and the
        /// netStatus event is dispatched with a code property of
        /// SharedObject.Flush.Success or
        /// SharedObject.Flush.Failed.
        /// </summary>
        /// <param name="minDiskSpace">
        /// (default = 0)  The minimum disk space, in bytes,
        /// that must be allotted for this object.
        /// </param>
        /// <returns>
        /// Either of the following values:
        /// SharedObjectFlushStatus.PENDING: The user has permitted local information
        /// storage for objects from this domain, but the
        /// amount of space allotted is not sufficient to store the object. Flash Player prompts
        /// the user to allow more space. SharedObjectFlushStatus.FLUSHED: The shared object has been
        /// successfully written to a file on the local disk.
        /// </returns>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String flush(int minDiskSpace);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String flush();

        /// <summary>
        /// Broadcasts a message to all clients connected to the specified remote shared object,
        /// including the client that sent the message. To process and respond to the message,
        /// create a handler function to send to the shared object.
        /// </summary>
        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void send();

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(object rest0);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(object rest0, object rest1);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(object rest0, object rest1, object rest2);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void send(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        /// <summary>
        /// For local shared objects, purges all of the data and deletes the shared object from the disk.
        /// The reference to the shared object is still active, but its data properties are deleted.
        /// </summary>
        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void clear();

        /// <summary>
        /// Indicates to the server that the value of a property (defined with the data property)
        /// in the shared object has changed.
        /// This method marks properties as changed (dirty) and is used in server-side ActionScript.
        /// You create properties for a shared object
        /// using the SharedObject.data property or simply
        /// by creating new, uniquely named SharedObject properties.
        /// For more information about the difference between these two techniques,
        /// see the SharedObject.data property.The SharedObject.setProperty() method implements the setDirty() method.
        /// In most cases, such as when the value of a property is a primitive type like String or Number,
        /// you would use setProperty() instead of setDirty().
        /// However, when the value of a property is an object that contains its own properties,
        /// use setDirty() to indicate when a value within the object has changed.
        /// </summary>
        /// <param name="propertyName">The name of the property that has changed.</param>
        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setDirty(Avm.String propertyName);

        /// <summary>
        /// Updates the value of a property (defined with the data property)
        /// in a shared object and indicates to the server
        /// that the value of the property has changed. The setProperty() method
        /// explicitly marks properties as changed (dirty) and is used in server-side ActionScript.
        /// Some servers, such as Flash Media Server, require an explicit call to
        /// setProperty() to indicate when a property of the shared object has changed.
        /// If you call SharedObject.setProperty() on the server side,
        /// it invokes a change notification through the sync event
        /// on the client side for any Flash Player client that is using the shared object.
        /// The propertyName parameter on the server side
        /// is the same as an attribute of the data property on the client side.
        /// For example, the following lines of code are all equivalent:
        /// the first two lines are server-side ActionScript, and
        /// the third and fourth are client-side ActionScript:
        /// setProperty(&quot;nameVal&quot;, &quot;foo&quot;);
        /// setDirty(&quot;nameVal&quot;);
        /// data[&quot;nameVal&quot;] = &quot;foo&quot;;
        /// data.nameVal = &quot;foo&quot;;
        /// Users of Flash Media Server can find additional information on server-side
        /// ActionScript and remote shared objects in the
        /// Flash Media Server Server-Side ActionScript Language Reference.Note: The SharedObject.setProperty() method implements the setDirty() method.
        /// In most cases, such as when the value of a property is a primitive type like String or Number,
        /// you would use setProperty() instead of setDirty.
        /// However, when the value of a property is an object that contains its own properties,
        /// use setDirty() to indicate when a value within the object has changed.
        /// In general, it is a good idea to call setProperty() rather than setDirty(), because
        /// setProperty() updates a property value only when that value has changed, whereas setDirty()
        /// forces synchronization on all subscribed clients.
        /// </summary>
        /// <param name="propertyName">The name of the property in the shared object.</param>
        /// <param name="value">(default = null)  The value of the property (an ActionScript object), or null to delete the property.</param>
        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setProperty(Avm.String propertyName, object value);

        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void setProperty(Avm.String propertyName);

        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static int deleteAll(Avm.String url);

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static int getDiskUsage(Avm.String url);

        /// <summary>
        /// Returns a reference to a locally persistent shared object that is available only to the current
        /// client. If the shared object does not already exist, this method creates one. If any values
        /// passed to getLocal() are invalid or if the call fails, Flash Player throws an exception.
        /// The following code shows how you assign the returned shared object reference to a variable:var so:SharedObject = SharedObject.getLocal(&quot;savedData&quot;);Note: If the user has chosen to never allow local storage for this domain,
        /// the object is not saved locally, even if a value for localPath is specified. The
        /// exception to this rule is local content. Local content can always write shared objects
        /// from third-party domains (domains other than the domain in the current browser address bar)
        /// to disk, even if writing of third-party shared objects to disk is disallowed.
        /// To avoid name conflicts, Flash looks at the location of the SWF file that is creating the
        /// shared object. For example, if a SWF file at www.myCompany.com/apps/stockwatcher.swf creates a
        /// shared object named portfolio, that shared object does not conflict with another
        /// object named portfolio that was created by a SWF file at
        /// www.yourCompany.com/photoshoot.swf because the SWF files originate from different directories. Although the localPath parameter is optional, you should give some
        /// thought to its use, especially if other SWF files need to access the shared object. If the
        /// data in the shared object is specific to one SWF file that will not be moved to another location,
        /// then use of the default value makes sense. If other SWF files need access to the shared object, or
        /// if the SWF file that creates the shared object will later be moved, then the value of this parameter
        /// affects how accessible the shared object will be. For example, if you create a shared object with
        /// localPath set to the default value of the full path to the SWF file, no other SWF
        /// file can access that shared object. If you later move the original SWF file to another location,
        /// not even that SWF file can access the data already stored in the shared object.To avoid inadvertently restricting access to a shared object, use
        /// the localpath parameter. The most permissive approach is to set
        /// localPath to / (slash), which makes the shared object available to all SWF files
        /// in the domain, but increases the likelihood of name conflicts with other shared objects in
        /// the domain. A more restrictive approach is to append localPath with folder names that are in
        /// the full path to the SWF file. For example, for a portfolio shared object created by the SWF
        /// file at www.myCompany.com/apps/stockwatcher.swf, you could set the localPath parameter to
        /// /, /apps, or /apps/stockwatcher.swf. You must determine which
        /// approach provides optimal flexibility for your application.When using this method, consider the Flash Player security model:You cannot access shared objects across sandbox boundaries.Users can restrict shared object access by using the Flash Player Settings dialog box
        /// or the Settings Manager. By default, an application can create shared objects of up 100 KB of data per domain.
        /// Administrators and users can also place restrictions on the ability to write to the file system.Suppose you publish SWF file content to be played back as local files (either locally installed SWF files or
        /// EXE files), and you need to access a specific shared object from more than one local SWF file. In this situation,
        /// be aware that for local files, two different locations might be used to store shared objects. The domain that is
        /// used depends on the security permissions granted to the local file that created the shared object. Local files
        /// can have three different levels of permissions:Access to the local filesystem only.Access to the network only.Access to both the network and the local filesystem.Local files with access to the local filesystem (level 1 or 3) store their shared objects in one location.
        /// Local files without access to the local filesystem (level 2) store their shared objects in another location.You can prevent a SWF file from using this method by setting the
        /// allowNetworking parameter of the the object and embed
        /// tags in the HTML page that contains the SWF content.For more information, see the following:The security chapter in the
        /// Programming ActionScript 3.0 book and the latest comments on LiveDocs
        /// </summary>
        /// <param name="name">
        /// The name of the object. The name can include forward slashes (/); for example,
        /// work/addresses is a legal name. Spaces are not allowed in a shared
        /// object name, nor are the following characters:
        /// ~ % &amp; \ ; : &quot; &apos; , &lt; &gt; ? #
        /// </param>
        /// <param name="localPath">
        /// (default = null)  The full or partial path to the SWF file that created the shared object, and that
        /// determines where the shared object will be stored locally. If you do not specify this parameter, the
        /// full path is used.
        /// </param>
        /// <param name="secure">
        /// (default = false)  Determines whether access to this shared object
        /// is restricted to SWF files that are delivered over an HTTPS connection.
        /// If your SWF file is delivered over HTTPS, this parameter&apos;s value has the following effects:
        /// If this parameter is set to true, Flash Player creates a new secure shared object or
        /// gets a reference to an existing secure shared object. This secure shared object
        /// can be read from or written to only by SWF files delivered over HTTPS that call
        /// SharedObject.getLocal() with the secure parameter set to
        /// true.If this parameter is set to false, Flash Player creates a new shared object or
        /// gets a reference to an existing shared object that can be read from
        /// or written to by SWF files delivered over non-HTTPS connections.If your SWF file is delivered over a non-HTTPS connection and you try to set this parameter
        /// to true, the creation of a new shared object (or the access of a previously
        /// created secure shared object) fails and null is returned. Regardless of the
        /// value of this parameter, the created shared objects count toward the total amount
        /// of disk space allowed for a domain.The following diagram shows the use of the secure parameter:
        /// </param>
        /// <returns>
        /// A reference to a shared object that is persistent locally and is available only to the
        /// current client. If Flash Player can&apos;t create or find the shared object (for example, if
        /// localPath was
        /// specified but no such directory exists), this method throws an exception.
        /// </returns>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.net.SharedObject getLocal(Avm.String name, Avm.String localPath, bool secure);

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.net.SharedObject getLocal(Avm.String name, Avm.String localPath);

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.net.SharedObject getLocal(Avm.String name);

        /// <summary>
        /// Returns a reference to an object that can be shared across multiple clients
        /// by means of a server, such as Flash Media Server. If the shared object does not
        /// already exist, this method creates one.
        /// The following code shows how you assign the returned shared object reference
        /// to a variable:var myRemote_so:SharedObject = SharedObject.getRemote(name, remotePath, persistence);To create a shared object that is available only to the current client,
        /// use SharedObject.getLocal().After calling this method, use SharedObject.connect() to connect
        /// the object to the application server, as shown in the following code:
        /// var myNC:NetConnection = new NetConnection();
        /// myNC.connect(&quot;rtmp://[yourDomain].com/applicationName&quot;);
        /// var myRemoteSO:SharedObject = SharedObject.getRemote(&quot;mo&quot;, myNC.uri, false);
        /// myRemoteSO.connect(myNC);
        /// To confirm that the local and remote copies of the shared object are synchronized,
        /// use the sync event.All clients that want to share this object must pass the same values for
        /// the name and remotePath parameters. For additional
        /// information about remote shared objects, see
        /// SharedObject.getRemote()
        /// in the Flash Media Server documentation.
        /// </summary>
        /// <param name="name">
        /// The name of the object. The name can include forward slashes (/);
        /// for example, work/addresses is a legal name. Spaces are not allowed in a shared object name,
        /// nor are the following characters:
        /// ~ % &amp; \ ; :  &quot; &apos; , &gt; ? ? #
        /// </param>
        /// <param name="remotePath">
        /// (default = null)  The URI of the server on which the shared object will be stored.
        /// This URI must be identical to the URI of the NetConnection object passed to the
        /// SharedObject.connect() method.
        /// </param>
        /// <param name="persistence">
        /// (default = false)  Specifies whether the attributes of the shared
        /// object&apos;s data property are persistent locally, remotely, or both. This parameter can also specify
        /// where the shared object will be stored locally. Acceptable values are as follows:
        /// A value of false specifies that the shared object is not persistent
        /// on the client or server.A value of true specifies that the shared object is persistent only on the server.A full or partial local path to the shared object indicates that the shared
        /// object is persistent on the client and the server. On the client, it is stored in the
        /// specified path; on the server, it is stored in a subdirectory within the application
        /// directory.Note: If the user has chosen to never allow local storage
        /// for this domain, the object will not be saved locally, even if a local path is
        /// specified for persistence. For more information, see the class description.
        /// </param>
        /// <param name="secure">
        /// (default = false)  Determines whether access to this shared object is restricted to SWF
        /// files that are delivered over an HTTPS connection. For more information, see the
        /// description of the secure parameter in the
        /// getLocal method entry.
        /// </param>
        /// <returns>A reference to an object that can be shared across multiple clients.</returns>
        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.net.SharedObject getRemote(Avm.String name, Avm.String remotePath, object persistence, bool secure);

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.net.SharedObject getRemote(Avm.String name, Avm.String remotePath, object persistence);

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.net.SharedObject getRemote(Avm.String name, Avm.String remotePath);

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.net.SharedObject getRemote(Avm.String name);


    }
}
