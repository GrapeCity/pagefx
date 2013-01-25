using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    [PageFX.AbcInstance(329)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class DRMAuthenticationCompleteEvent : flash.events.Event
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String AUTHENTICATION_COMPLETE;

        public extern virtual Avm.String serverURL
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String domain
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

        public extern virtual flash.utils.ByteArray token
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticationCompleteEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String inServerURL, Avm.String inDomain, flash.utils.ByteArray inToken);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticationCompleteEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String inServerURL, Avm.String inDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticationCompleteEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String inServerURL);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticationCompleteEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticationCompleteEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticationCompleteEvent(Avm.String type);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.events.Event clone();


    }
}
