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
    [PageFX.AbcInstance(333)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class SimpleButton : flash.display.InteractiveObject
    {
        /// <summary>
        /// A Boolean value that, when set to true, indicates whether
        /// the hand cursor is shown when the mouse rolls over a button.
        /// If this property is set to false, the arrow pointer cursor is displayed
        /// instead. The default is true.
        /// You can change the useHandCursor property at any time;
        /// the modified button immediately uses the new cursor behavior.
        /// </summary>
        public extern virtual bool useHandCursor
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// A Boolean value that specifies whether a button is enabled. When a
        /// button is disabled (the enabled property is set to false),
        /// the button is visible but cannot be clicked. The default value is
        /// true. This property is useful if you want to
        /// disable part of your navigation; for example, you might want to disable a
        /// button in the currently displayed page so that it can&apos;t be clicked and
        /// the page cannot be reloaded.
        /// </summary>
        public extern virtual bool enabled
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates whether other display objects that are SimpleButton or MovieClip objects can receive
        /// mouse release events. The trackAsMenu property lets you create menus. You
        /// can set the trackAsMenu property on any SimpleButton or MovieClip object.
        /// If the trackAsMenu property does not exist, the default behavior is
        /// false.
        /// You can change the trackAsMenu property at any time; the
        /// modified button immediately takes on the new behavior.
        /// </summary>
        public extern virtual bool trackAsMenu
        {
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Specifies a display object that is used as the visual
        /// object for the button up state  the state that the button is in when
        /// the mouse is not positioned over the button.
        /// </summary>
        public extern virtual flash.display.DisplayObject upState
        {
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Specifies a display object that is used as the visual
        /// object for the button over state  the state that the button is in when
        /// the mouse is positioned over the button.
        /// </summary>
        public extern virtual flash.display.DisplayObject overState
        {
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Specifies a display object that is used as the visual
        /// object for the button &quot;Down&quot; state the state that the button is in when the user
        /// clicks the hitTestState object.
        /// </summary>
        public extern virtual flash.display.DisplayObject downState
        {
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(12)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Specifies a display object that is used as the hit testing object for the button. For a basic button, set the
        /// hitTestState property to the same display object as the overState
        /// property. If you do not set the hitTestState property, the SimpleButton
        /// is inactive  it does not respond to mouse and keyboard events.
        /// </summary>
        public extern virtual flash.display.DisplayObject hitTestState
        {
            [PageFX.AbcInstanceTrait(13)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(14)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The SoundTransform object assigned to this button. A SoundTransform object
        /// includes properties for setting volume, panning, left speaker assignment, and right
        /// speaker assignment. This SoundTransform object applies to all states of the button.
        /// This SoundTransform object affects only embedded sounds.
        /// </summary>
        public extern virtual flash.media.SoundTransform soundTransform
        {
            [PageFX.AbcInstanceTrait(15)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(16)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SimpleButton(flash.display.DisplayObject upState, flash.display.DisplayObject overState, flash.display.DisplayObject downState, flash.display.DisplayObject hitTestState);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SimpleButton(flash.display.DisplayObject upState, flash.display.DisplayObject overState, flash.display.DisplayObject downState);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SimpleButton(flash.display.DisplayObject upState, flash.display.DisplayObject overState);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SimpleButton(flash.display.DisplayObject upState);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern SimpleButton();


    }
}
