using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>NativeMenuItem is an abstract class that defines the interface for a single item in a menu.</summary>
    [PageFX.AbcInstance(179)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public class NativeMenuItem : flash.events.EventDispatcher
    {
        /// <summary>Describes whether the item is selectable.</summary>
        public extern virtual bool enabled
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

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
        public extern NativeMenuItem();


    }
}
