using System;
using System.Runtime.CompilerServices;

namespace flash.text.ime
{
    [PageFX.AbcInstance(308)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public interface IIMEClient
    {
        int compositionStartIndex
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.QName("compositionStartIndex", "flash.text.ime:IIMEClient", "public")]
            [PageFX.FP("10.2")]
            get;
        }

        int compositionEndIndex
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.QName("compositionEndIndex", "flash.text.ime:IIMEClient", "public")]
            [PageFX.FP("10.2")]
            get;
        }

        bool verticalTextLayout
        {
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.QName("verticalTextLayout", "flash.text.ime:IIMEClient", "public")]
            [PageFX.FP("10.2")]
            get;
        }

        int selectionAnchorIndex
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.QName("selectionAnchorIndex", "flash.text.ime:IIMEClient", "public")]
            [PageFX.FP("10.2")]
            get;
        }

        int selectionActiveIndex
        {
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.QName("selectionActiveIndex", "flash.text.ime:IIMEClient", "public")]
            [PageFX.FP("10.2")]
            get;
        }

        [PageFX.Event("textInput")]
        event flash.events.TextEventHandler textInput;

        [PageFX.Event("imeStartComposition")]
        event flash.events.IMEEventHandler imeStartComposition;

        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.QName("updateComposition", "flash.text.ime:IIMEClient", "public")]
        [PageFX.FP("10.2")]
        void updateComposition(Avm.String text, Avm.Vector<flash.text.ime.CompositionAttributeRange> attributes, int compositionStartIndex, int compositionEndIndex);

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.QName("confirmComposition", "flash.text.ime:IIMEClient", "public")]
        [PageFX.FP("10.2")]
        void confirmComposition(Avm.String text, bool preserveSelection);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("getTextBounds", "flash.text.ime:IIMEClient", "public")]
        [PageFX.FP("10.2")]
        flash.geom.Rectangle getTextBounds(int startIndex, int endIndex);

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.QName("selectRange", "flash.text.ime:IIMEClient", "public")]
        [PageFX.FP("10.2")]
        void selectRange(int anchorIndex, int activeIndex);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("getTextInRange", "flash.text.ime:IIMEClient", "public")]
        [PageFX.FP("10.2")]
        Avm.String getTextInRange(int startIndex, int endIndex);
    }
}
