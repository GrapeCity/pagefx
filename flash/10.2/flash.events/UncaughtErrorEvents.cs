using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    [PageFX.AbcInstance(91)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class UncaughtErrorEvents : flash.events.EventDispatcher
    {
        [PageFX.Event("uncaughtError")]
        public event flash.events.UncaughtErrorEventHandler uncaughtError
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern UncaughtErrorEvents();
    }
}
