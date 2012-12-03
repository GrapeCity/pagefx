using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class GroupElement : ContentElement
    {
        public extern virtual int elementCount
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GroupElement(Avm.Vector<flash.text.engine.ContentElement> arg0, ElementFormat arg1, flash.events.EventDispatcher arg2, Avm.String arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GroupElement(Avm.Vector<flash.text.engine.ContentElement> arg0, ElementFormat arg1, flash.events.EventDispatcher arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GroupElement(Avm.Vector<flash.text.engine.ContentElement> arg0, ElementFormat arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GroupElement(Avm.Vector<flash.text.engine.ContentElement> arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern GroupElement();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual ContentElement getElementAt(int arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual ContentElement getElementAtCharIndex(int arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int getElementIndex(ContentElement arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual TextElement splitTextElement(int arg0, int arg1);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual GroupElement groupElements(int arg0, int arg1);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setElements(Avm.Vector<flash.text.engine.ContentElement> arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Vector<flash.text.engine.ContentElement> replaceElements(int arg0, int arg1, Avm.Vector<flash.text.engine.ContentElement> arg2);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual TextElement mergeTextElements(int arg0, int arg1);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void ungroupElements(int arg0);
    }
}
