using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// The FileReference class provides a means to upload and
    /// download files between a user&apos;s computer and a server. An operating-system
    /// dialog box prompts the user to select a file to upload or a location for
    /// download. Each FileReference object refers to a single file on the user&apos;s disk
    /// and has properties that contain information about
    /// the file&apos;s size, type, name, creation date, modification date, and creator type
    /// (Macintosh only).
    /// </summary>
    [PageFX.AbcInstance(109)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class FileReference : flash.events.EventDispatcher
    {
        /// <summary>
        /// The creation date of the file on the local disk. If the object is
        /// was not populated, a call to get the value of this property returns null.
        /// </summary>
        public extern virtual Avm.Date creationDate
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The Macintosh creator type of the file, which is only used in Mac OS versions
        /// prior to Mac OS X. In Windows, this property is null.
        /// If the FileReference object
        /// was not populated, a call to get the value of this property returns null.
        /// </summary>
        public extern virtual Avm.String creator
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The date that the file on the local disk was last modified. If the FileReference
        /// object was not populated, a call to get the value of this property returns null.
        /// </summary>
        public extern virtual Avm.Date modificationDate
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The name of the file on the local disk. If the FileReference object
        /// was not populated, a call to get the value of this property returns null.
        /// All the properties of a FileReference object are populated by calling the browse() method.
        /// Unlike other FileReference properties, if you call the download() method,
        /// the name property is populated when the select event is dispatched.
        /// </summary>
        public extern virtual Avm.String name
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The size of the file on the local disk in bytes.
        /// Note: In the initial version of ActionScript 3.0, the size property was
        /// defined as a uint object, which supported files with sizes up to about 4 GB. It is now implimented as a Number
        /// object to support larger files.
        /// </summary>
        public extern virtual double size
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The file type. In Windows, this property is the file extension. On the Macintosh, this property is
        /// the four-character file type, which is only used in Mac OS versions prior to Mac OS X. If the FileReference object
        /// was not populated, a call to get the value of this property returns null.
        /// For Windows and Mac OS X, the file extension  the portion of the name property that
        /// follows the last occurence of the dot (.) character  identifies the file type.
        /// </summary>
        public extern virtual Avm.String type
        {
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual flash.utils.ByteArray data
        {
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [PageFX.Event("uploadCompleteData")]
        public event flash.events.DataEventHandler uploadCompleteData
        {
            add { }
            remove { }
        }

        [PageFX.Event("httpResponseStatus")]
        public event flash.events.HTTPStatusEventHandler httpResponseStatus
        {
            add { }
            remove { }
        }

        [PageFX.Event("httpStatus")]
        public event flash.events.HTTPStatusEventHandler httpStatus
        {
            add { }
            remove { }
        }

        [PageFX.Event("select")]
        public event flash.events.EventHandler select
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

        [PageFX.Event("complete")]
        public event flash.events.EventHandler complete
        {
            add { }
            remove { }
        }

        [PageFX.Event("cancel")]
        public event flash.events.EventHandler OnCancel
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FileReference();

        /// <summary>
        /// Displays a file-browsing dialog box that lets the
        /// user select a file to upload. The dialog box is native to the user&apos;s
        /// operating system. The user can select a file on the local computer
        /// or from other systems, for example, through a UNC path on Windows.
        /// When you call this method and the user
        /// successfully selects a file, the properties of this FileReference object are populated with
        /// the properties of that file. Each subsequent time that the FileReference.browse() method
        /// is called, the FileReference
        /// object&apos;s properties are reset to the file that the user selects in the dialog box.
        /// Only one browse() or download() session
        /// can be performed at a time (because only one dialog box can be invoked at a time).Using the typeFilter parameter, you can determine which files the dialog box displays.
        /// </summary>
        /// <param name="typeFilter">
        /// (default = null)  An array of FileFilter instances used to filter the files that are
        /// displayed in the dialog box. If you omit this parameter,
        /// all files are displayed.
        /// For more information, see the FileFilter class.
        /// </param>
        /// <returns>
        /// Returns true if the parameters are valid and the file-browsing dialog box
        /// opens.
        /// </returns>
        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool browse(Avm.Array typeFilter);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool browse();

        /// <summary>
        /// Cancels any ongoing upload or download operation on this FileReference object.
        /// Calling this method does not dispatch the cancel event; that event
        /// is dispatched only when the user cancels the operation by dismissing the
        /// file upload or download dialog box.
        /// </summary>
        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void cancel();

        /// <summary>
        /// Opens a dialog box that lets the user download a file from a remote server.
        /// Although Flash Player has no restriction on the size of files you can upload or download,
        /// the player officially supports uploads or downloads of up to 100 MB.The download() method first opens
        /// an operating-system dialog box that asks the user to enter a filename and
        /// select a location on the local computer
        /// to save the file. When the user selects a location and confirms the download operation
        /// (for example, by clicking Save), the download from the remote server begins.
        /// Listeners receive events to indicate the progress, success, or
        /// failure of the download.
        /// To ascertain the status of the dialog box and the download operation after calling
        /// download(), your code must listen for events
        /// such as cancel, open,
        /// progress, and complete.
        /// The FileReference.upload() and FileReference.download() functions
        /// are nonblocking. These functions return after they are called, before the file transmission
        /// is complete. In addition, if the FileReference object goes out of scope, any upload or download
        /// that is not yet completed on that object is canceled upon leaving the scope.
        /// Be sure that your FileReference object remains in scope for as long as the
        /// upload or download is expected to continue.When the file is downloaded successfully, the
        /// properties of the FileReference object are populated with the properties
        /// of the local file. The complete event is dispatched if the
        /// download is successful.Only one browse() or download() session can
        /// be performed at a time (because only one dialog box can be invoked at a time).This method supports downloading of any file type, with either HTTP or HTTPS.Note: If your server requires user authentication, only
        /// SWF files running in a browser — that is, using the browser plug-in or ActiveX control —
        /// can provide a dialog box to prompt the user for a user name and password for authentication,
        /// and only for downloads. For uploads using the plug-in or ActiveX control, or for
        /// uploads and downloads using the stand-alone or external player, the file transfer fails.When you use this method in content in security sandboxes other
        /// than then application security sandbox, consider the Flash PlayerAIR security model: Loading operations are not allowed if the calling content is in an untrusted local sandbox.The default behavior is to deny access between sandboxes. A website can enable access to a
        /// resource by adding a cross-domain policy file.You can prevent a SWF file from using this method by setting the allowNetworking
        /// parameter of the the object and embed tags in the HTML
        /// page that contains the SWF content.However, in the Adobe Integrated Runtime (AIR),
        /// content in the application security sandbox (content
        /// installed with the AIR application) are not restricted by these security limitations.For more information, see the following:The security chapter in the
        /// Programming ActionScript 3.0 book and the latest comments on LiveDocsThe &quot;Understanding AIR Security&quot; section of the &quot;Getting started with Adobe AIR&quot; chapter
        /// in the Developing AIR Applications book.The &quot;Understanding AIR Security&quot; section of the &quot;Getting started with Adobe AIR&quot; chapter
        /// in the Developing AIR Applications book.The Flash Player 9 Security white paper
        /// </summary>
        /// <param name="request">
        /// The URLRequest object. The url property of the URLRequest object
        /// should contain the URL of the file to download to the local computer.
        /// If this parameter is null, an exception is thrown.
        /// To send POST or GET parameters to the server, set the value of URLRequest.data
        /// to your parameters, and set URLRequest.method to either URLRequestMethod.POST
        /// or URLRequestMethod.GET.
        /// On some browsers, URL strings are limited in length. Lengths greater than 256 characters may
        /// fail on some browsers or servers.
        /// </param>
        /// <param name="defaultFileName">
        /// (default = null)  The default filename displayed in the dialog box for the file
        /// to be downloaded. This string must not contain the following characters:
        /// / \ : * ? &quot; &lt; &gt; | %
        /// If you omit this parameter, the filename of the
        /// remote URL is parsed and used as the default.
        /// </param>
        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void download(flash.net.URLRequest request, Avm.String defaultFileName);

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void download(flash.net.URLRequest request);

        /// <summary>
        /// Starts the upload of a file selected by a user to a remote server. Although
        /// Flash Player has no restriction on the size of files you can upload or download,
        /// the player officially supports uploads or downloads of up to 100 MB.
        /// You must call the FileReference.browse() or FileReferenceList.browse()
        /// method before you call this method.
        /// Listeners receive events to indicate the progress, success, or
        /// failure of the upload. Although you can use the FileReferenceList object to let users
        /// select multiple files for upload, you must upload the files one by one; to do so, iterate through
        /// the FileReferenceList.fileList array of FileReference objects.The FileReference.upload() and FileReference.download() functions
        /// are nonblocking. These functions return after they are called, before the file transmission
        /// is complete. In addition, if the FileReference object goes out of scope, any upload or download
        /// that is not yet completed on that object is canceled upon leaving the scope.
        /// Be sure that your FileReference object remains in scope for as long as the
        /// upload or download is expected to continue.The file is uploaded to the URL passed in the url parameter. The URL
        /// must be a server script configured to accept uploads. Flash Player uploads files by using
        /// the HTTP POST method. The server script that handles the upload
        /// should expect a POST request with the following elements:Content-Type of multipart/form-dataContent-Disposition with a name attribute set to &quot;Filedata&quot; by default
        /// and a filename attribute set to the name of the original fileThe binary contents of the fileFor a sample POST request, see the description of the uploadDataFieldName
        /// parameter. You can send POST or GET parameters to the server with the upload()
        /// method; see the description of the request parameter.
        /// If the testUpload parameter is true,
        /// and the file to be uploaded is bigger than approximately 10 KB, Flash Player on Windows
        /// first sends a test upload POST operation with zero content before uploading the actual file,
        /// to verify that the transmission is likely to succeed. Flash Player then sends
        /// a second POST operation that contains the actual file content.
        /// For files smaller than 10 KB, Flash Player performs a single
        /// upload POST with the actual file content to be uploaded.
        /// Flash Player on Macintosh does not perform test upload POST operations.Note: If your server requires user authentication, only
        /// SWF files running in a browser — that is, using the browser plug-in or ActiveX control —
        /// can provide a dialog box to prompt the user for a username and password for authentication,
        /// and only for downloads. For uploads using the plug-in or ActiveX control, or for
        /// uploads and downloads using the stand-alone or external player, the file transfer fails.When you use this method in content in security sandboxes other
        /// than then application security sandbox, consider the Flash PlayerAIR security model: Loading operations are not allowed if the calling SWF file is in an untrusted local sandbox.The default behavior is to deny access between sandboxes. A website can enable access to a
        /// resource by adding a cross-domain policy file.You can prevent a SWF file from using this method by setting the  allowNetworking
        /// parameter of the the object and embed tags in the HTML
        /// page that contains the SWF content.However, in the Apollo runtime, content in the application security sandbox (content
        /// installed with the Apollo application) are not restricted by these security limitations.For more information, see the following:The security chapter in the
        /// Programming ActionScript 3.0 book and the latest comments on LiveDocsThe &quot;Understanding AIR Security&quot; section of the &quot;Getting started with Adobe AIR&quot; chapter
        /// in the Developing AIR Applications book.The Flash Player 9 Security white paper
        /// </summary>
        /// <param name="request">
        /// The URLRequest object; the url property of the URLRequest object
        /// should contain the URL of the server script
        /// configured to handle upload through HTTP POST calls.
        /// On some browsers, URL strings are limited in length.
        /// Lengths greater than 256 characters may fail on some browsers or servers.
        /// If this parameter is null, an exception is thrown.
        /// The URL can be HTTP or, for secure uploads, HTTPS.
        /// To use HTTPS, use an HTTPS url in the url parameter.
        /// If you do not specify a port number in the url
        /// parameter, port 80 is used for HTTP and port 443 us used for HTTPS, by default.To send POST or GET parameters to the server, set the data property
        /// of the URLRequest object to your parameters, and set the method property
        /// to either URLRequestMethod.POST or
        /// URLRequestMethod.GET.
        /// </param>
        /// <param name="uploadDataFieldName">
        /// (default = &quot;Filedata&quot;)  The field name that precedes the file data in the upload POST operation.
        /// The uploadDataFieldName value must be non-null and a non-empty String.
        /// By default, the value of uploadDataFieldName is &quot;Filedata&quot;,
        /// as shown in the following sample POST request:
        /// Content-Type: multipart/form-data; boundary=AaB03x
        /// --AaB03x
        /// Content-Disposition: form-data; name=&quot;Filedata&quot;; filename=&quot;example.jpg&quot;
        /// Content-Type: application/octet-stream
        /// ... contents of example.jpg ...
        /// --AaB03x--
        /// </param>
        /// <param name="testUpload">
        /// (default = false)  A setting to request a test file upload. If testUpload
        /// is true, for files larger than 10 KB, Flash Player attempts
        /// a test file upload POST with a Content-Length of 0. The test upload
        /// checks whether the actual file upload will be successful and that server
        /// authentication, if required, will succeed. A test upload
        /// is only available for Windows players.
        /// </param>
        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void upload(flash.net.URLRequest request, Avm.String uploadDataFieldName, bool testUpload);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void upload(flash.net.URLRequest request, Avm.String uploadDataFieldName);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void upload(flash.net.URLRequest request);

        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void load();

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void save(object data, Avm.String defaultFileName);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void save(object data);
    }
}
