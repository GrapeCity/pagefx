using System;
using System.Runtime.CompilerServices;

namespace flash.media
{
    /// <summary>
    /// The Microphone class lets you capture audio from a microphone attached to the computer
    /// that is running Flash Player.
    /// </summary>
    [PageFX.AbcInstance(81)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class Microphone : flash.events.EventDispatcher
    {
        /// <summary>
        /// The microphone gain—that is, the amount by which the microphone should multiply the signal before
        /// transmitting it. A value of 0 tells Flash to multiply by 0; that is, the microphone transmits no sound.
        /// You can think of this setting like a volume knob on a stereo: 0 is no volume and 50 is normal
        /// volume. Numbers below 50 specify lower than normal volume, while numbers above 50 specify higher than
        /// normal volume. Valid values are 0-100. The user can change this value in the
        /// Flash Player Microphone Settings panel.
        /// </summary>
        public extern virtual double gain
        {
            [PageFX.AbcInstanceTrait(16)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The rate at which the microphone is capturing sound, in kHz. The default value is 8 kHz if your sound capture device supports this
        /// value. Otherwise, the default value is the next available capture level above 8 kHz that your sound capture device supports,
        /// usually 11 kHz.
        /// </summary>
        public extern virtual int rate
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String codec
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int framesPerPacket
        {
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int encodeQuality
        {
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int noiseSuppressionLevel
        {
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool enableVAD
        {
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(12)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The amount of sound the microphone is detecting. Values range from
        /// 0 (no sound is detected) to 100 (very loud sound is detected). The value of this property can
        /// help you determine a good value to pass to the Microphone.setSilenceLevel() method.
        /// If the microphone is available but is not yet being used because Microphone.getMicrophone()
        /// has not been called, this property is set to -1.
        /// </summary>
        public extern virtual double activityLevel
        {
            [PageFX.AbcInstanceTrait(15)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The index of the microphone, as reflected in the array returned by
        /// Microphone.names.
        /// </summary>
        public extern virtual int index
        {
            [PageFX.AbcInstanceTrait(17)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies whether the user has denied access to the microphone (true)
        /// or allowed access (false). When this value changes, Microphone.onStatus is
        /// invoked. For more information, see Microphone.getMicrophone().
        /// </summary>
        public extern virtual bool muted
        {
            [PageFX.AbcInstanceTrait(18)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>The name of the current sound capture device, as returned by the sound capture hardware.</summary>
        public extern virtual Avm.String name
        {
            [PageFX.AbcInstanceTrait(19)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The amount of sound required to activate the microphone and dispatch
        /// the activity event. The default value is 10.
        /// </summary>
        public extern virtual double silenceLevel
        {
            [PageFX.AbcInstanceTrait(20)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The number of milliseconds between the time the microphone stops
        /// detecting sound and the time the activity event is dispatched. The default
        /// value is 2000 (2 seconds).
        /// To set this value, use the Microphone.setSilenceLevel() method.
        /// </summary>
        public extern virtual int silenceTimeout
        {
            [PageFX.AbcInstanceTrait(21)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Returns true if echo suppression is enabled; false otherwise. The default value is
        /// false unless the user has selected Reduce Echo in the Flash Player Microphone Settings panel.
        /// </summary>
        public extern virtual bool useEchoSuppression
        {
            [PageFX.AbcInstanceTrait(22)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>Controls the sound of this microphone object when it is in loopback mode.</summary>
        public extern virtual flash.media.SoundTransform soundTransform
        {
            [PageFX.AbcInstanceTrait(24)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(25)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The names of all available sound capture devices. The names are returned without
        /// having to display the Flash Player Privacy Settings panel to the user. This array
        /// provides the zero-based index of each sound capture device and the
        /// number of sound capture devices on the system, through the Microphone.names.length property.
        /// For more information, see the Array class entry.
        /// Calling Microphone.names requires an extensive examination of the hardware, and it
        /// may take several seconds to build the array. In most cases, you can just use the default microphone.Note: To determine the name of the current microphone,
        /// use the name property.
        /// </summary>
        public extern static Avm.Array names
        {
            [PageFX.AbcClassTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static bool isSupported
        {
            [PageFX.AbcClassTrait(2)]
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

        [PageFX.Event("sampleData")]
        public event flash.events.SampleDataEventHandler sampleData
        {
            add { }
            remove { }
        }

        [PageFX.Event("activity")]
        public event flash.events.ActivityEventHandler activity
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Microphone();

        /// <summary>
        /// Sets the minimum input level that should be considered sound and (optionally) the amount
        /// of silent time signifying that silence has actually begun.
        /// To prevent the microphone from detecting sound at all, pass a value of 100 for
        /// silenceLevel; the activity is never dispatched. To determine the amount of sound the microphone is currently detecting, use Microphone.activityLevel. Activity detection is the ability to detect when audio levels suggest that a person is talking.
        /// When someone is not talking, bandwidth can be saved because there is no need to send the associated
        /// audio stream. This information can also be used for visual feedback so that users know
        /// they (or others) are silent.Silence values correspond directly to activity values. Complete silence is an activity value of 0.
        /// Constant loud noise (as loud as can be registered based on the current gain setting) is an activity value
        /// of 100. After gain is appropriately adjusted, your activity value is less than your silence value when
        /// you&apos;re not talking; when you are talking, the activity value exceeds your silence value.This method is similar in purpose to Camera.setMotionLevel(); both methods are used to
        /// specify when the activity event is dispatched. However, these methods have
        /// a significantly different impact on publishing streams:Camera.setMotionLevel() is designed to detect motion and does not affect bandwidth
        /// usage. Even if a video stream does not detect motion, video is still sent.Microphone.setSilenceLevel() is designed to optimize bandwidth. When an audio
        /// stream is considered silent, no audio data is sent. Instead, a single message is sent, indicating
        /// that silence has started.
        /// </summary>
        /// <param name="silenceLevel">
        /// The amount of sound required to activate the microphone
        /// and dispatch the activity event. Acceptable values range from 0 to 100.
        /// </param>
        /// <param name="timeout">
        /// (default = -1)  The number of milliseconds that must elapse without
        /// activity before Flash Player considers sound to have stopped and dispatches the
        /// dispatch event. The default value is 2000 (2 seconds).
        /// (Note: The default value shown
        /// in the signature, -1, is an internal value that indicates to Flash Player to use 2000.)
        /// </param>
        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setSilenceLevel(double silenceLevel, int timeout);

        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void setSilenceLevel(double silenceLevel);

        /// <summary>
        /// Specifies whether to use the echo suppression feature of the audio codec. The default value is
        /// false unless the user has selected Reduce Echo in the Flash Player Microphone
        /// Settings panel.
        /// Echo suppression is an effort to reduce the effects of audio feedback, which is caused when
        /// sound going out the speaker is picked up by the microphone on the same computer. (This is different
        /// from echo cancellation, which completely removes the feedback.)Generally, echo suppression is advisable when the sound being captured is played through
        /// speakers — instead of a headset — on the same computer. If your SWF file allows users to specify the
        /// sound output device, you may want to call Microphone.setUseEchoSuppression(true)
        /// if they indicate they are using speakers and will be using the microphone as well. Users can also adjust these settings in the Flash Player Microphone Settings panel.
        /// </summary>
        /// <param name="useEchoSuppression">
        /// A Boolean value indicating whether echo suppression should be used
        /// (true) or not (false).
        /// </param>
        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setUseEchoSuppression(bool useEchoSuppression);

        /// <summary>
        /// Sets the microphone into loopback mode or turns it off. This method reroutes
        /// microphone sound to the local speaker.
        /// </summary>
        /// <param name="state">(default = true)</param>
        [PageFX.AbcInstanceTrait(23)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setLoopBack(bool state);

        [PageFX.AbcInstanceTrait(23)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void setLoopBack();

        /// <summary>
        /// To create or reference a Microphone object, use the
        /// Microphone.getMicrophone() method to get
        /// the index value of the current microphone. You can then
        /// pass this value to methods of the Microphone class.
        /// </summary>
        /// <param name="index">(default = 0)  The index value of the microphone.</param>
        /// <returns>Microphone</returns>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.media.Microphone getMicrophone(int index);

        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.media.Microphone getMicrophone();


    }
}
