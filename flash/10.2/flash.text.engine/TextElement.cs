using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(51)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class TextElement : flash.text.engine.ContentElement
    {
        public extern virtual Avm.String text
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextElement(Avm.String text, flash.text.engine.ElementFormat elementFormat, flash.events.EventDispatcher eventMirror, Avm.String textRotation);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextElement(Avm.String text, flash.text.engine.ElementFormat elementFormat, flash.events.EventDispatcher eventMirror);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextElement(Avm.String text, flash.text.engine.ElementFormat elementFormat);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextElement(Avm.String text);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextElement();

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void replaceText(int beginIndex, int endIndex, Avm.String newText);
    }
}
