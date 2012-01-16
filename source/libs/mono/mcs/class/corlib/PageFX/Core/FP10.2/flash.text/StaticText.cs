using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    /// <summary>
    /// This class represents StaticText objects on the display list.
    /// You cannot create a StaticText object using ActionScript. Only the authoring tool
    /// can create a StaticText object. An attempt to create a new StaticText object generates
    /// an ArgumentError .
    /// </summary>
    [PageFX.AbcInstance(253)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class StaticText : flash.display.DisplayObject
    {
        /// <summary>
        /// Returns the current text of the static text field. The authoring tool may export multiple text field
        /// objects comprising the complete text. For example, for vertical text, the authoring tool will create
        /// one text field per character.
        /// </summary>
        public extern virtual Avm.String text
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StaticText();


    }
}
