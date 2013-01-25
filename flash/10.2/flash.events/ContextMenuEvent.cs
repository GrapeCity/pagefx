using System;
using System.Runtime.CompilerServices;

namespace flash.events
{
    /// <summary>
    /// An object dispatches a ContextMenuEvent object when the user generates or interacts with
    /// the context menu. Users generate the context menu by clicking the secondary button of
    /// the user&apos;s pointing device (usually a mouse or a trackball). There are two types of
    /// ContextMenuEvent objects:
    /// ContextMenuEvent.MENU_ITEM_SELECTContextMenuEvent.MENU_SELECT
    /// </summary>
    [PageFX.AbcInstance(373)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class ContextMenuEvent : flash.events.Event
    {
        /// <summary>
        /// Defines the value of the type property of a menuItemSelect event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.contextMenuOwnerThe display list object to which the menu is attached.currentTargetThe object that is actively processing the Event
        /// object with an event listener.mouseTargetThe display list object on which the user right-clicked to display the context menu.targetThe ContextMenuItem object that has been selected.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String MENU_ITEM_SELECT;

        /// <summary>
        /// Defines the value of the type property of a menuSelect event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.contextMenuOwnerThe display list object to which the menu is attached.currentTargetThe object that is actively processing the Event
        /// object with an event listener.mouseTargetThe display list object on which the user right-clicked to display the context menu.targetThe ContextMenu object that is about to be displayed.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String MENU_SELECT;

        /// <summary>The display list object on which the user right-clicked to display the context menu. This could be the display list object to which the menu is attached (contextMenuOwner) or one of its display list descendants.</summary>
        public extern virtual flash.display.InteractiveObject mouseTarget
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

        /// <summary>The display list object to which the menu is attached. This could be the mouse target (mouseTarget) or one of its ancestors in the display list.</summary>
        public extern virtual flash.display.InteractiveObject contextMenuOwner
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

        public extern virtual bool isMouseTargetInaccessible
        {
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenuEvent(Avm.String type, bool bubbles, bool cancelable, flash.display.InteractiveObject mouseTarget, flash.display.InteractiveObject contextMenuOwner);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenuEvent(Avm.String type, bool bubbles, bool cancelable, flash.display.InteractiveObject mouseTarget);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenuEvent(Avm.String type, bool bubbles, bool cancelable);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenuEvent(Avm.String type, bool bubbles);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenuEvent(Avm.String type);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.events.Event clone();

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();


    }
}
