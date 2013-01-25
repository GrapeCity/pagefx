using System;
using System.Runtime.CompilerServices;

namespace flash.net.drm
{
    [PageFX.AbcInstance(60)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class DRMManager : flash.events.EventDispatcher
    {
        public extern static bool isSupported
        {
            [PageFX.AbcClassTrait(3)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [PageFX.Event("drmError")]
        public event flash.events.DRMErrorEventHandler drmError
        {
            add { }
            remove { }
        }

        [PageFX.Event("drmStatus")]
        public event flash.events.DRMStatusEventHandler drmStatus
        {
            add { }
            remove { }
        }

        [PageFX.Event("authenticationError")]
        public event flash.events.DRMAuthenticationErrorEventHandler authenticationError
        {
            add { }
            remove { }
        }

        [PageFX.Event("authenticationComplete")]
        public event flash.events.DRMAuthenticationCompleteEventHandler authenticationComplete
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DRMManager();

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void authenticate(Avm.String serverURL, Avm.String domain, Avm.String username, Avm.String password);

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setAuthenticationToken(Avm.String serverUrl, Avm.String domain, flash.utils.ByteArray token);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void loadVoucher(flash.net.drm.DRMContentData contentData, Avm.String setting);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void loadPreviewVoucher(flash.net.drm.DRMContentData contentData);

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static flash.net.drm.DRMManager getDRMManager();


    }
}
