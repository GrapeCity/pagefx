using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The SimpleButton class lets you control all instances of button symbols in a SWF
    /// file. After you create an instance of a button in the authoring tool, you can
    /// use the methods and properties of the SimpleButton class to manipulate buttons
    /// with ActionScript.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class SimpleButton : InteractiveObject
    {
        public extern virtual bool enabled
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual DisplayObject hitTestState
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual DisplayObject upState
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual DisplayObject downState
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual flash.media.SoundTransform soundTransform
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool useHandCursor
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual DisplayObject overState
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool trackAsMenu
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SimpleButton(DisplayObject arg0, DisplayObject arg1, DisplayObject arg2, DisplayObject arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SimpleButton(DisplayObject arg0, DisplayObject arg1, DisplayObject arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SimpleButton(DisplayObject arg0, DisplayObject arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SimpleButton(DisplayObject arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SimpleButton();


    }
}
