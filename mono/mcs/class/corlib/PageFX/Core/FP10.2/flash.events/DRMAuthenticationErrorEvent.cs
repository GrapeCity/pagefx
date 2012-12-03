using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    [PageFX.AbcInstance(271)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class DRMAuthenticationErrorEvent : flash.events.ErrorEvent
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String AUTHENTICATION_ERROR;

        public extern virtual int subErrorID
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String serverURL
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String domain
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

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticationErrorEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String inDetail, int inErrorID, int inSubErrorID, Avm.String inServerURL, Avm.String inDomain);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticationErrorEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String inDetail, int inErrorID, int inSubErrorID, Avm.String inServerURL);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticationErrorEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String inDetail, int inErrorID, int inSubErrorID);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticationErrorEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String inDetail, int inErrorID);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticationErrorEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String inDetail);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticationErrorEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticationErrorEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticationErrorEvent(Avm.String type);

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.events.Event clone();


    }
}
