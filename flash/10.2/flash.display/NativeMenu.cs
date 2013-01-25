using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The NativeMenu class represents any menu in an application, whether it be a pop-up menu,
    /// a context menu, window menu on Windows, or an application menu on Mac OS.
    /// </summary>
    [PageFX.AbcInstance(52)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class NativeMenu : flash.events.EventDispatcher
    {
        [PageFX.Event("preparing")]
        public event flash.events.EventHandler preparing
        {
            add { }
            remove { }
        }

        [PageFX.Event("displaying")]
        public event flash.events.EventHandler displaying
        {
            add { }
            remove { }
        }

        [PageFX.Event("select")]
        public event flash.events.EventHandler select
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern NativeMenu();
    }
}
