using System;
using System.Runtime.CompilerServices;

namespace flash.media
{
    /// <summary>
    /// The SoundLoaderContext class provides security checks for SWF files that load sound.
    /// SoundLoaderContext objects are passed as an argument to the constructor and the
    /// load()  method of the Sound class.
    /// </summary>
    [PageFX.AbcInstance(186)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class SoundLoaderContext : Avm.Object
    {
        /// <summary>
        /// The number of seconds to preload a streaming sound into a buffer
        /// before the sound starts to stream.
        /// Note that you cannot override the value of SoundLoaderContext.bufferTime
        /// by setting the global SoundMixer.bufferTime property.
        /// The SoundMixer.bufferTime property affects the buffer time
        /// for embedded streaming sounds in a SWF and is independent of dynamically created
        /// Sound objects (that is, Sound objects created in ActionScript).
        /// </summary>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public double bufferTime;

        /// <summary>
        /// Specifies whether Flash Player should try to download a cross-domain policy file from the
        /// loaded sound&apos;s server before beginning to load the sound. This property applies to
        /// sound that is loaded from outside
        /// the calling SWF file&apos;s own domain using the Sound.load() method.
        /// Set this property to true when you load a sound from outside the calling
        /// SWF file&apos;s own domain
        /// and you need low-level access to the sound&apos;s data from ActionScript. Examples of
        /// low-level access to a sound&apos;s data include referencing the Sound.id3 property to get an
        /// ID3Info object or calling the SoundMixer.computeSpectrum() method to get
        /// sound samples from the loaded sound. If you try to access sound data without
        /// setting the checkPolicyFile property to true at loading time,
        /// you may get a
        /// SecurityError exception because the required policy file has not been downloaded.If you don&apos;t need low-level access to the sound data that you are loading, avoid setting
        /// checkPolicyFile to true. Checking for a policy file consumes
        /// network bandwidth
        /// and might delay the start of your download, so it should only be done when necessary.When you call Sound.load() with SoundLoaderContext.checkPolicyFile set
        /// to true, Flash Player must either successfully download a relevant cross-domain
        /// policy file or determine that no such policy file exists before it
        /// begins downloading the specified sound.
        /// Flash Player performs the following
        /// actions, in this order, to verify the existence of a policy file:Flash Player considers policy files that have already been downloaded.Flash Player tries to download any pending policy files specified in calls to
        /// Security.loadPolicyFile().Flash Player tries to download a policy file from the default location
        /// that corresponds to the sound&apos;s URL, which is
        /// /crossdomain.xml on the same server as URLRequest.url.
        /// (The sound&apos;s URL is specified in
        /// the url property of the URLRequest object
        /// passed to Sound.load() or the Sound constructor.)In all cases,
        /// Flash Player requires that an appropriate policy file exist on the sound&apos;s server, that it provide access
        /// to the sound file at URLRequest.url by virtue of the policy file&apos;s location, and
        /// that it allow the domain of the calling SWF file to access the sound, through one or more
        /// &lt;allow-access-from&gt; tags.
        /// If you set checkPolicyFile to true, Flash Player waits until the policy file is verified
        /// before loading the sound. You should wait to perform
        /// any low-level operations on the sound data, such as calling Sound.id3 or
        /// SoundMixer.computeSpectrum(), until progress and complete
        /// events are dispatched from the Sound object.
        /// If you set checkPolicyFile to true but no appropriate policy file is found,
        /// you will not receive an error until you perform an operation that requires
        /// a policy file, and then Flash Player throws a
        /// SecurityError exception. After you receive a complete
        /// event, you can test whether a relevant policy file was found by getting the value
        /// of Sound.id3 within a try block and seeing if a
        /// SecurityError is thrown.Be careful with checkPolicyFile if you are downloading sound from a URL that
        /// uses server-side HTTP redirects. Flash Player tries to retrieve policy files that
        /// correspond to the url property of the URLRequest object
        /// passed to Sound.load(). If the final
        /// sound file comes from a different URL because of HTTP redirects, then the initially downloaded
        /// policy files might not be applicable to the sound&apos;s final URL, which is the URL that matters
        /// in security decisions.If you find yourself in this situation, here is one possible solution.
        /// After you receive a progress or complete event, you can examine the value of
        /// the Sound.url property, which contains the sound&apos;s final URL.
        /// Then call the Security.loadPolicyFile() method
        /// with a policy file URL that you calculate based on the sound&apos;s final URL. Finally, poll the value
        /// of Sound.id3 until no exception is thrown.For more information on policy files, see the &quot;Flash Player Security&quot; chapter in
        /// Programming ActionScript 3.0.
        /// </summary>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public bool checkPolicyFile;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SoundLoaderContext(double bufferTime, bool checkPolicyFile);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SoundLoaderContext(double bufferTime);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SoundLoaderContext();
    }
}
