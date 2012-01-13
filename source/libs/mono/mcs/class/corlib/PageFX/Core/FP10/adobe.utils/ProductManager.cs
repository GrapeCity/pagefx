using System;
using System.Runtime.CompilerServices;

namespace adobe.utils
{
    [PageFX.ABC]
    [PageFX.FP9]
    public class ProductManager : flash.events.EventDispatcher
    {
        public extern virtual bool installed
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String installedVersion
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool running
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [PageFX.Event("error")]
        public event flash.events.ErrorEventHandler error
        {
            add { }
            remove { }
        }

        [PageFX.Event("verifyError")]
        public event flash.events.IOErrorEventHandler verifyError
        {
            add { }
            remove { }
        }

        [PageFX.Event("diskError")]
        public event flash.events.IOErrorEventHandler diskError
        {
            add { }
            remove { }
        }

        [PageFX.Event("networkError")]
        public event flash.events.IOErrorEventHandler networkError
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

        [PageFX.Event("cancel")]
        public event flash.events.EventHandler cancel
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ProductManager(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool launch(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool launch();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool download(Avm.String arg0, Avm.String arg1, Avm.Array arg2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool download(Avm.String arg0, Avm.String arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool download(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool download();


    }
}
