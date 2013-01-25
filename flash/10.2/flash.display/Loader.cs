using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The Loader class is used to load SWF files or image (JPG, PNG, or GIF) files. Use the
    /// load()  method to initiate loading. The loaded display object is added as a child
    /// of the Loader object.
    /// </summary>
    [PageFX.AbcInstance(340)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class Loader : flash.display.DisplayObjectContainer
    {
        /// <summary>
        /// Contains the root display object of the SWF file or image (JPG, PNG, or GIF)
        /// file that was loaded by using the load() or loadBytes() methods.
        /// </summary>
        public extern virtual flash.display.DisplayObject content
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Returns a LoaderInfo object corresponding to the object being loaded. LoaderInfo objects
        /// are shared between the Loader object and the loaded content object. The LoaderInfo object
        /// supplies loading progress information and statistics about the loaded file.
        /// Events related to the load are dispatched by the LoaderInfo object referenced by the
        /// contentLoaderInfo property of the Loader object. The contentLoaderInfo
        /// property is set to a valid LoaderInfo object, even before the content is loaded, so that you can add
        /// event listeners to the object prior to the load.
        /// </summary>
        public extern virtual flash.display.LoaderInfo contentLoaderInfo
        {
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual flash.events.UncaughtErrorEvents uncaughtErrorEvents
        {
            [PageFX.AbcInstanceTrait(15)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Loader();

        /// <summary>
        /// Loads a SWF, JPEG, progressive JPEG, unanimated GIF, or PNG file into an object that is a child of
        /// this Loader object. If you load an animated GIF file, only the first frame is displayed.
        /// As the Loader object can contain only a single child, issuing a subsequent load()
        /// request terminates the previous request, if still pending, and commences a new load.
        /// A SWF file or image loaded into a Loader object inherits the position, rotation, and scale
        /// properties of the parent display objects of the Loader object. Use the unload() method to remove movies or images loaded with this
        /// method, or to cancel a load operation that is in progress.When you use the load() method, consider the Flash Player security model: You can load content from any accessible source. Loading is not allowed if the calling SWF file is in a network sandbox and the file
        /// to be loaded is local. If the loaded content is a SWF file, it cannot be scripted by a SWF file in another
        /// security sandbox unless that cross-scripting arrangement was approved through a call to
        /// the Security.allowDomain() method in the loaded content file. SWF files written in ActionScript 1.0 or 2.0, which are loaded as AVM1Movie objects, cannot
        /// cross-script SWF files written in ActionScript 3.0, which are loaded as Sprite or MovieClip objects. You
        /// can use the LocalConnection class to have these files communicate with each other.If the loaded content is an image, its data cannot be accessed by a SWF file
        /// outside of the security sandbox, unless the domain of that SWF file was included a
        /// cross-domain policy file at the origin domain of the image.Movie clips in the local-with-file-system sandbox cannot cross-script movie clips in the
        /// local-with-networking sandbox, and the reverse is also prevented. You can prevent a SWF file from using this method by setting the  allowNetworking
        /// parameter of the the object and embed tags in the HTML
        /// page that contains the SWF content.However, in the Apollo runtime, content in the application security sandbox (content
        /// installed with the Apollo application) are not restricted by these security limitations.For more information, see the following:The security chapter in the
        /// Programming ActionScript 3.0 book and the latest comments on LiveDocs
        /// </summary>
        /// <param name="request">
        /// The absolute or relative URL of the SWF, JPEG, GIF, or PNG file to be loaded. A
        /// relative path must be relative to the main SWF file. Absolute URLs must include the
        /// protocol reference, such as http:// or file:///. Filenames cannot include disk drive
        /// specifications.
        /// </param>
        /// <param name="context">
        /// (default = null)  A LoaderContext object, which has properties that define the following:
        /// Whether or not Flash Player should check for the existence of a policy file
        /// upon loading the objectThe ApplicationDomain for the loaded objectThe SecurityDomain for the loaded objectFor complete details, see the description of the properties in the
        /// LoaderContext class.
        /// </param>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void load(flash.net.URLRequest request, flash.system.LoaderContext context);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void load(flash.net.URLRequest request);

        /// <summary>Loads from binary data stored in a ByteArray object.</summary>
        /// <param name="bytes">
        /// A ByteArray object. The contents of the ByteArray can be
        /// any of the file formats supported by the Loader class: SWF, GIF, JPEG, or PNG.
        /// </param>
        /// <param name="context">
        /// (default = null)  A LoaderContext object. Only the applicationDomain property
        /// of the LoaderContext object applies; the checkPolicyFile and securityDomain
        /// properties of the LoaderContext object do not apply.
        /// </param>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void loadBytes(flash.utils.ByteArray bytes, flash.system.LoaderContext context);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void loadBytes(flash.utils.ByteArray bytes);

        /// <summary>Cancels a load() method operation that is currently in progress for the Loader instance.</summary>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void close();

        /// <summary>
        /// Removes a child of this Loader object that was loaded by using the load() method.
        /// The property of the associated LoaderInfo object is reset to null.
        /// The child is not necessarily destroyed because other objects might have references to it; however,
        /// it is no longer a child of the Loader object.
        /// As a best practice, before you unload a child SWF file, you should explicitly
        /// close any streams in the child SWF file&apos;s objects, such as LocalConnection, NetConnection,
        /// NetStream, and Sound objects. Otherwise, audio in the child SWF file might continue to play, even
        /// though the child SWF file was unloaded. To close streams in the child SWF file, add an event listener
        /// to the child that listens for the unload event. When the parent calls
        /// Loader.unload(), the unload event is dispatched to the child.
        /// The following code shows how you might do this:
        /// function closeAllStreams(evt:Event) {
        /// myNetStream.close();
        /// mySound.close();
        /// myNetConnection.close();
        /// myLocalConnection.close();
        /// }
        /// myMovieClip.loaderInfo.addEventListener(Event.UNLOAD, closeAllStreams);
        /// </summary>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void unload();

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void unloadAndStop(bool gc);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void unloadAndStop();

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.display.DisplayObject addChild(flash.display.DisplayObject child);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.display.DisplayObject addChildAt(flash.display.DisplayObject child, int index);

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.display.DisplayObject removeChild(flash.display.DisplayObject child);

        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.display.DisplayObject removeChildAt(int index);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override void setChildIndex(flash.display.DisplayObject child, int index);


    }
}
