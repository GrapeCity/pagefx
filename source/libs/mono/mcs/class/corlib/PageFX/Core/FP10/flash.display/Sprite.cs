using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The Sprite class is a basic display list building block: a display list node that can display
    /// graphics and can also contain children.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class Sprite : DisplayObjectContainer
    {
        public extern virtual DisplayObject dropTarget
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
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

        public extern virtual Sprite hitArea
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

        public extern virtual bool buttonMode
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

        public extern virtual Graphics graphics
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
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

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Sprite();

        /// <summary>
        /// Ends the startDrag() method. A sprite that was made draggable with the
        /// startDrag() method remains draggable until a
        /// stopDrag() method is added, or until another
        /// sprite becomes draggable. Only one sprite is draggable at a time.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void stopDrag();

        /// <summary>
        /// Lets the user drag the specified sprite. The sprite remains draggable until explicitly
        /// stopped through a call to the Sprite.stopDrag() method, or until
        /// another sprite is made draggable. Only one sprite is draggable at a time.
        /// </summary>
        /// <param name="arg0">
        /// (default = false)  Specifies whether the draggable sprite is locked to the center of
        /// the mouse position (true), or locked to the point where the user first clicked the
        /// sprite (false).
        /// </param>
        /// <param name="arg1">
        /// (default = null)  Value relative to the coordinates of the Sprite&apos;s parent that specify a constraint
        /// rectangle for the Sprite.
        /// </param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void startDrag(bool arg0, flash.geom.Rectangle arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void startDrag(bool arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void startDrag();
    }
}
