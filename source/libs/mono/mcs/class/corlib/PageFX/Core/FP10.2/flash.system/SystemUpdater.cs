using System;
using System.Runtime.CompilerServices;

namespace flash.system
{
    [PageFX.AbcInstance(289)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class SystemUpdater : flash.events.EventDispatcher
    {
        [PageFX.Event("cancel")]
        public event flash.events.EventHandler OnCancel
        {
            add { }
            remove { }
        }

        [PageFX.Event("complete")]
        public event flash.events.EventHandler complete
        {
            add { }
            remove { }
        }

        [PageFX.Event("securityError")]
        public event flash.events.SecurityErrorEventHandler securityError
        {
            add { }
            remove { }
        }

        [PageFX.Event("ioError")]
        public event flash.events.IOErrorEventHandler ioError
        {
            add { }
            remove { }
        }

        [PageFX.Event("progress")]
        public event flash.events.ProgressEventHandler progress
        {
            add { }
            remove { }
        }

        [PageFX.Event("status")]
        public event flash.events.StatusEventHandler status
        {
            add { }
            remove { }
        }

        [PageFX.Event("open")]
        public event flash.events.EventHandler open
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SystemUpdater();

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void update(Avm.String type);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void cancel();
    }
}
