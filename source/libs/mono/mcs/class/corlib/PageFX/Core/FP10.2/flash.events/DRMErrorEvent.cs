using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    [PageFX.AbcInstance(287)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class DRMErrorEvent : flash.events.ErrorEvent
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String DRM_ERROR;

        public extern virtual int subErrorID
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual flash.net.drm.DRMContentData contentData
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

        public extern virtual bool systemUpdateNeeded
        {
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool drmUpdateNeeded
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMErrorEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String inErrorDetail, int inErrorCode, int insubErrorID, flash.net.drm.DRMContentData inMetadata, bool inSystemUpdateNeeded, bool inDrmUpdateNeeded);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMErrorEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String inErrorDetail, int inErrorCode, int insubErrorID, flash.net.drm.DRMContentData inMetadata, bool inSystemUpdateNeeded);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMErrorEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String inErrorDetail, int inErrorCode, int insubErrorID, flash.net.drm.DRMContentData inMetadata);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMErrorEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String inErrorDetail, int inErrorCode, int insubErrorID);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMErrorEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String inErrorDetail, int inErrorCode);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMErrorEvent(Avm.String type, bool bubbles, bool cancelable, Avm.String inErrorDetail);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMErrorEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMErrorEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMErrorEvent(Avm.String type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMErrorEvent();

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.events.Event clone();

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();


    }
}
