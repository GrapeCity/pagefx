using System;
using System.Runtime.CompilerServices;

namespace flash.accessibility
{
    [PageFX.AbcInstance(131)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public interface ISimpleTextSelection
    {
        int selectionAnchorIndex
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.QName("selectionAnchorIndex", "flash.accessibility:ISimpleTextSelection", "public")]
            [PageFX.FP("10.2")]
            get;
        }

        int selectionActiveIndex
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.QName("selectionActiveIndex", "flash.accessibility:ISimpleTextSelection", "public")]
            [PageFX.FP("10.2")]
            get;
        }


    }
}
