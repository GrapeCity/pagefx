using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.AbcInstance(232)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class GroupElement : flash.text.engine.ContentElement
    {
        public extern virtual int elementCount
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GroupElement(Avm.Vector<flash.text.engine.ContentElement> elements, flash.text.engine.ElementFormat elementFormat, flash.events.EventDispatcher eventMirror, Avm.String textRotation);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GroupElement(Avm.Vector<flash.text.engine.ContentElement> elements, flash.text.engine.ElementFormat elementFormat, flash.events.EventDispatcher eventMirror);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GroupElement(Avm.Vector<flash.text.engine.ContentElement> elements, flash.text.engine.ElementFormat elementFormat);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GroupElement(Avm.Vector<flash.text.engine.ContentElement> elements);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GroupElement();

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.text.engine.ContentElement getElementAt(int index);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setElements(Avm.Vector<flash.text.engine.ContentElement> value);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.text.engine.GroupElement groupElements(int beginIndex, int endIndex);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void ungroupElements(int groupIndex);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.text.engine.TextElement mergeTextElements(int beginIndex, int endIndex);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.text.engine.TextElement splitTextElement(int elementIndex, int splitIndex);

        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Vector<flash.text.engine.ContentElement> replaceElements(int beginIndex, int endIndex, Avm.Vector<flash.text.engine.ContentElement> newElements);

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.text.engine.ContentElement getElementAtCharIndex(int charIndex);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int getElementIndex(flash.text.engine.ContentElement element);
    }
}
