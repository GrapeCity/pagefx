using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class TextElement : ContentElement
    {
        public extern virtual Avm.String text
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextElement(Avm.String arg0, ElementFormat arg1, flash.events.EventDispatcher arg2, Avm.String arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextElement(Avm.String arg0, ElementFormat arg1, flash.events.EventDispatcher arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextElement(Avm.String arg0, ElementFormat arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextElement(Avm.String arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextElement();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void replaceText(int arg0, int arg1, Avm.String arg2);


    }
}
