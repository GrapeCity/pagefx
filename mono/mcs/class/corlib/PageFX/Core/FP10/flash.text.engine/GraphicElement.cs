using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class GraphicElement : ContentElement
    {
        public extern virtual flash.display.DisplayObject graphic
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double elementHeight
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double elementWidth
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicElement(flash.display.DisplayObject arg0, double arg1, double arg2, ElementFormat arg3, flash.events.EventDispatcher arg4, Avm.String arg5);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicElement(flash.display.DisplayObject arg0, double arg1, double arg2, ElementFormat arg3, flash.events.EventDispatcher arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicElement(flash.display.DisplayObject arg0, double arg1, double arg2, ElementFormat arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicElement(flash.display.DisplayObject arg0, double arg1, double arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicElement(flash.display.DisplayObject arg0, double arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicElement(flash.display.DisplayObject arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicElement();


    }
}
