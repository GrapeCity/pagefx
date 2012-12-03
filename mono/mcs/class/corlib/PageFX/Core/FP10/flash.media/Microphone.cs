using System;
using System.Runtime.CompilerServices;

namespace flash.media
{
    /// <summary>
    /// The Microphone class lets you capture audio from a microphone attached to the computer
    /// that is running Flash Player.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class Microphone : flash.events.EventDispatcher
    {
        public extern virtual int rate
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

        public extern virtual SoundTransform soundTransform
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

        public extern virtual double silenceLevel
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double gain
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

        public extern virtual bool muted
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String codec
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool useEchoSuppression
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual int silenceTimeout
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual int encodeQuality
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double activityLevel
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual int index
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String name
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual int framesPerPacket
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern static Avm.Array names
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [PageFX.Event("status")]
        public event flash.events.StatusEventHandler status
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
        /// <param name="arg0">
        /// The amount of sound required to activate the microphone
        /// and dispatch the activity event. Acceptable values range from 0 to 100.
        /// </param>
        /// <param name="arg1">
        /// (default = -1)  The number of milliseconds that must elapse without
        /// activity before Flash Player considers sound to have stopped and dispatches the
        /// dispatch event. The default value is 2000 (2 seconds).
        /// (Note: The default value shown
        /// in the signature, -1, is an internal value that indicates to Flash Player to use 2000.)
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setSilenceLevel(double arg0, int arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void setSilenceLevel(double arg0);

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
        /// <param name="arg0">
        /// A Boolean value indicating whether echo suppression should be used
        /// (true) or not (false).
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setUseEchoSuppression(bool arg0);

        /// <summary>
        /// Sets the microphone into loopback mode or turns it off. This method reroutes
        /// microphone sound to the local speaker.
        /// </summary>
        /// <param name="arg0">(default = true)</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setLoopBack(bool arg0);

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
        /// <param name="arg0">(default = 0)  The index value of the microphone.</param>
        /// <returns>Microphone</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Microphone getMicrophone(int arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Microphone getMicrophone();


    }
}
