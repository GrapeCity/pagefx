using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The Sprite class is a basic display list building block: a display list node that can display
    /// graphics and can also contain children.
    /// </summary>
    [PageFX.AbcInstance(302)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class Sprite : flash.display.DisplayObjectContainer
    {
        /// <summary>
        /// Specifies the Graphics object that belongs to this sprite where vector
        /// drawing commands can occur.
        /// </summary>
        public extern virtual flash.display.Graphics graphics
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Specifies the button mode of this sprite. If true, this
        /// sprite behaves as a button, which means that it triggers the display
        /// of the hand cursor when the mouse passes over the sprite and can
        /// receive a click event if the enter or space keys are pressed
        /// when the sprite has focus. You can suppress the display of the hand cursor
        /// by setting the useHandCursor property to false,
        /// in which case the pointer is displayed.
        /// Although it is better to use the SimpleButton class to create buttons,
        /// you can use the buttonMode property to give a sprite
        /// some button-like functionality. To include a sprite in the tab order,
        /// set the tabEnabled property (inherited from the
        /// InteractiveObject class and false by default) to
        /// true. Additionally, consider whether you want
        /// the children of your sprite to be mouse enabled. Most buttons
        /// do not enable mouse interactivity for their child objects because
        /// it confuses the event flow. To disable mouse interactivity for all child
        /// objects, you must set the mouseChildren property (inherited
        /// from the DisplayObjectContainer class) to false.If you use the buttonMode property with the MovieClip class (which is a
        /// subclass of the Sprite class), your button might have some added
        /// functionality. If you include frames labeled _up, _over, and _down,
        /// Flash Player provides automatic state changes (functionality
        /// similar to that provided in previous versions of ActionScript for movie
        /// clips used as buttons). These automatic state changes are
        /// not available for sprites, which have no timeline, and thus no frames
        /// to label.
        /// </summary>
        public extern virtual bool buttonMode
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
        /// Specifies the display object over which the sprite is being dragged, or on
        /// which the sprite was dropped.
        /// </summary>
        public extern virtual flash.display.DisplayObject dropTarget
        {
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Designates another sprite to serve as the hit area for a sprite. If the hitArea
        /// property does not exist or the value is null or undefined, the
        /// sprite itself is used as the hit area. The value of the hitArea property can
        /// be a reference to a Sprite object.
        /// You can change the hitArea property at any time; the modified sprite immediately
        /// uses the new hit area behavior. The sprite designated as the hit area does not need to be
        /// visible; its graphical shape, although not visible, is still detected as the hit area.Note: You must set to false the mouseEnabled
        /// property of the sprite designated as the hit area. Otherwise, your sprite button might
        /// not work because the sprite designated as the hit area receives the mouse events instead
        /// of your sprite button.
        /// </summary>
        public extern virtual flash.display.Sprite hitArea
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
        /// A Boolean value that indicates whether the pointing hand (hand cursor) appears when the mouse rolls
        /// over a sprite in which the buttonMode property is set to true.
        /// The default value of the useHandCursor property is true.
        /// If useHandCursor is set to true, the pointing hand used for buttons
        /// appears when the mouse rolls over a button sprite. If useHandCursor is
        /// false, the arrow pointer is used instead.
        /// You can change the useHandCursor property at any time; the modified sprite
        /// immediately takes on the new cursor appearance. Note: If your sprite has child sprites, you might want to
        /// set the mouseChildren property to false. For example, if you want a hand
        /// cursor to appear over a Flex &lt;mx:Label&gt; control, set the useHandCursor and
        /// buttonMode properties to true, and the mouseChildren property
        /// to false.
        /// </summary>
        public extern virtual bool useHandCursor
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

        /// <summary>Controls sound within this sprite.</summary>
        public extern virtual flash.media.SoundTransform soundTransform
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

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Sprite();

        /// <summary>
        /// Lets the user drag the specified sprite. The sprite remains draggable until explicitly
        /// stopped through a call to the Sprite.stopDrag() method, or until
        /// another sprite is made draggable. Only one sprite is draggable at a time.
        /// </summary>
        /// <param name="lockCenter">
        /// (default = false)  Specifies whether the draggable sprite is locked to the center of
        /// the mouse position (true), or locked to the point where the user first clicked the
        /// sprite (false).
        /// </param>
        /// <param name="bounds">
        /// (default = null)  Value relative to the coordinates of the Sprite&apos;s parent that specify a constraint
        /// rectangle for the Sprite.
        /// </param>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void startDrag(bool lockCenter, flash.geom.Rectangle bounds);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void startDrag(bool lockCenter);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void startDrag();

        /// <summary>
        /// Ends the startDrag() method. A sprite that was made draggable with the
        /// startDrag() method remains draggable until a
        /// stopDrag() method is added, or until another
        /// sprite becomes draggable. Only one sprite is draggable at a time.
        /// </summary>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void stopDrag();

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void startTouchDrag(int touchPointID, bool lockCenter, flash.geom.Rectangle bounds);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void startTouchDrag(int touchPointID, bool lockCenter);

        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void startTouchDrag(int touchPointID);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void stopTouchDrag(int touchPointID);


    }
}
