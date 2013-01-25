using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    [PageFX.AbcInstance(177)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class DRMAuthenticateEvent : flash.events.Event
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String DRM_AUTHENTICATE;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String AUTHENTICATION_TYPE_DRM;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String AUTHENTICATION_TYPE_PROXY;

        public extern virtual Avm.String header
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String usernamePrompt
        {
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String passwordPrompt
        {
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String urlPrompt
        {
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String authenticationType
        {
            [PageFX.AbcInstanceTrait(12)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual flash.net.NetStream netstream
        {
            [PageFX.AbcInstanceTrait(13)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticateEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String header, Avm.String userPrompt, Avm.String passPrompt, Avm.String urlPrompt, Avm.String authenticationType, flash.net.NetStream netstream);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticateEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String header, Avm.String userPrompt, Avm.String passPrompt, Avm.String urlPrompt, Avm.String authenticationType);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticateEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String header, Avm.String userPrompt, Avm.String passPrompt, Avm.String urlPrompt);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticateEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String header, Avm.String userPrompt, Avm.String passPrompt);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticateEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String header, Avm.String userPrompt);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticateEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String header);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticateEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticateEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMAuthenticateEvent(Avm.String type);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.events.Event clone();

        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();


    }
}
