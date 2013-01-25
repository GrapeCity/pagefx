using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(303)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class GraphicElement : flash.text.engine.ContentElement
    {
        public extern virtual flash.display.DisplayObject graphic
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

        public extern virtual double elementHeight
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double elementWidth
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicElement(flash.display.DisplayObject graphic, double elementWidth, double elementHeight, flash.text.engine.ElementFormat elementFormat, flash.events.EventDispatcher eventMirror, Avm.String textRotation);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicElement(flash.display.DisplayObject graphic, double elementWidth, double elementHeight, flash.text.engine.ElementFormat elementFormat, flash.events.EventDispatcher eventMirror);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicElement(flash.display.DisplayObject graphic, double elementWidth, double elementHeight, flash.text.engine.ElementFormat elementFormat);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicElement(flash.display.DisplayObject graphic, double elementWidth, double elementHeight);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicElement(flash.display.DisplayObject graphic, double elementWidth);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicElement(flash.display.DisplayObject graphic);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GraphicElement();


    }
}
