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
    [PageFX.ABC]
    [PageFX.FP9]
    public class ContextMenuEvent : Event
    {
        /// <summary>
        /// Defines the value of the type property of a menuItemSelect event object.
        /// This event has the following properties:PropertyValuebubblesfalsecancelablefalse; there is no default behavior to cancel.contextMenuOwnerThe display list object to which the menu is attached.currentTargetThe object that is actively processing the Event
        /// object with an event listener.mouseTargetThe display list object on which the user right-clicked to display the context menu.targetThe ContextMenuItem object that has been selected.
        /// The target is not always the object in the display list
        /// that registered the event listener. Use the currentTarget
        /// property to access the object in the display list that is currently processing the event.
        /// </summary>
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
        [PageFX.ABC]
        [PageFX.FP9]
        public static Avm.String MENU_SELECT;

        public extern virtual flash.display.InteractiveObject contextMenuOwner
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

        public extern virtual flash.display.InteractiveObject mouseTarget
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
        public extern ContextMenuEvent(Avm.String arg0, bool arg1, bool arg2, flash.display.InteractiveObject arg3, flash.display.InteractiveObject arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenuEvent(Avm.String arg0, bool arg1, bool arg2, flash.display.InteractiveObject arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenuEvent(Avm.String arg0, bool arg1, bool arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenuEvent(Avm.String arg0, bool arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ContextMenuEvent(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Avm.String toString();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override Event clone();
    }
}
