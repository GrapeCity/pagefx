using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    [PageFX.AbcInstance(192)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class DRMStatusEvent : flash.events.Event
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String DRM_STATUS;

        public extern virtual uint offlineLeasePeriod
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

        public extern virtual flash.net.drm.DRMVoucher voucher
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

        public extern virtual bool isLocal
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

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMStatusEvent(Avm.String type, bool bubbles, bool cancelable, flash.net.drm.DRMContentData inMetadata, flash.net.drm.DRMVoucher inVoucher, bool inLocal);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMStatusEvent(Avm.String type, bool bubbles, bool cancelable, flash.net.drm.DRMContentData inMetadata, flash.net.drm.DRMVoucher inVoucher);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMStatusEvent(Avm.String type, bool bubbles, bool cancelable, flash.net.drm.DRMContentData inMetadata);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMStatusEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMStatusEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMStatusEvent(Avm.String type);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMStatusEvent();

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
