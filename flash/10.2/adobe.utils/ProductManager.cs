using System;
using System.Runtime.CompilerServices;

namespace adobe.utils
{
    [PageFX.AbcInstance(207)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class ProductManager : flash.events.EventDispatcher
    {
        public extern virtual bool running
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool installed
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String installedVersion
        {
            [PageFX.AbcInstanceTrait(4)]
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
        public extern ProductManager(Avm.String name, bool shared);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ProductManager(Avm.String name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ProductManager();

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool launch(Avm.String parameters);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool launch();

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool download(Avm.String caption, Avm.String fileName, Avm.Array pathElements);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool download(Avm.String caption, Avm.String fileName);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool download(Avm.String caption);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool download();

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool doSelfUpgrade(Avm.String os);
    }
}
