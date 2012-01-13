using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class ContentElement : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP10]
        public object userData;

        [PageFX.ABC]
        [PageFX.FP10]
        public static uint GRAPHIC_ELEMENT;

        public extern virtual TextBlock textBlock
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual ElementFormat elementFormat
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

        public extern virtual int textBlockBeginIndex
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String textRotation
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

        public extern virtual Avm.String text
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual flash.events.EventDispatcher eventMirror
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

        public extern virtual Avm.String rawText
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual GroupElement groupElement
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContentElement(ElementFormat arg0, flash.events.EventDispatcher arg1, Avm.String arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContentElement(ElementFormat arg0, flash.events.EventDispatcher arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContentElement(ElementFormat arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContentElement();


    }
}
