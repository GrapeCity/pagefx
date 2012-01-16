using System;
using System.Runtime.CompilerServices;

namespace flash.accessibility
{
    [PageFX.AbcInstance(105)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public interface ISearchableText
    {
        Avm.String searchText
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.QName("searchText", "flash.accessibility:ISearchableText", "public")]
            [PageFX.FP("10.2")]
            get;
        }


    }
}
