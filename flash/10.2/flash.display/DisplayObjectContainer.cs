using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The DisplayObjectContainer class is the base class for all objects that can serve as display object containers on
    /// the display list. The display list manages all objects displayed in Flash Player.
    /// Use the DisplayObjectContainer class to arrange the display objects in the display list.
    /// Each DisplayObjectContainer object has its own child list for organizing the z-order of the objects.
    /// The z-order is the front-to-back order that determines which object is drawn in front, which is behind,
    /// and so on.
    /// </summary>
    [PageFX.AbcInstance(88)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class DisplayObjectContainer : flash.display.InteractiveObject
    {
        /// <summary>Returns the number of children of this object.</summary>
        public extern virtual int numChildren
        {
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>Returns a TextSnapshot object for this DisplayObjectContainer instance.</summary>
        public extern virtual flash.text.TextSnapshot textSnapshot
        {
            [PageFX.AbcInstanceTrait(10)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Determines whether the children of the object are tab enabled. Enables or disables tabbing for the
        /// children of the object. The default is true.
        /// </summary>
        public extern virtual bool tabChildren
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
        /// Determines whether or not the children of the object are mouse enabled.
        /// If an object is mouse enabled, a user can interact with it by using a mouse. The default is true.
        /// This property is useful when you create a button with an instance of the Sprite class
        /// (instead of using the SimpleButton class). When you use a Sprite instance to create a button,
        /// you can choose to decorate the button by using the addChild() method to add additional
        /// Sprite instances. This process can cause unexpected behavior with mouse events because
        /// the Sprite instances you add as children can become the target object of a mouse event
        /// when you expect the parent instance to be the target object. To ensure that the parent
        /// instance serves as the target objects for mouse events, you can set the
        /// mouseChildren property of the parent instance to false. No event is dispatched by setting this property. You must use the
        /// addEventListener() method to create interactive functionality.
        /// </summary>
        public extern virtual bool mouseChildren
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
        public extern DisplayObjectContainer();

        /// <summary>
        /// Adds a child DisplayObject instance to this DisplayObjectContainer instance. The child is added
        /// to the front (top) of all other children in this DisplayObjectContainer instance. (To add a child to a
        /// specific index position, use the addChildAt() method.)
        /// If you add a child object that already has a different display object container as
        /// a parent, the object is removed from the child list of the other display object container.
        /// </summary>
        /// <param name="child">The DisplayObject instance to add as a child of this DisplayObjectContainer instance.</param>
        /// <returns>
        /// The DisplayObject instance that you pass in the
        /// child parameter.
        /// </returns>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.display.DisplayObject addChild(flash.display.DisplayObject child);

        /// <summary>
        /// Adds a child DisplayObject instance to this DisplayObjectContainer
        /// instance.  The child is added
        /// at the index position specified. An index of 0 represents the back (bottom)
        /// of the display list for this DisplayObjectContainer object.
        /// For example, the following example shows three display objects, labeled a, b, and c, at
        /// index positions 0, 2, and 1, respectively:If you add a child object that already has a different display object container as
        /// a parent, the object is removed from the child list of the other display object container.
        /// </summary>
        /// <param name="child">
        /// The DisplayObject instance to add as a child of this
        /// DisplayObjectContainer instance.
        /// </param>
        /// <param name="index">
        /// The index position to which the child is added. If you specify a
        /// currently occupied index position, the child object that exists at that position and all
        /// higher positions are moved up one position in the child list.
        /// </param>
        /// <returns>
        /// The DisplayObject instance that you pass in the
        /// child parameter.
        /// </returns>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.display.DisplayObject addChildAt(flash.display.DisplayObject child, int index);

        /// <summary>
        /// Removes the specified child DisplayObject instance from the child list of the DisplayObjectContainer instance.
        /// The parent property of the removed child is set to null
        /// , and the object is garbage collected if no other
        /// references to the child exist. The index positions of any display objects above the child in the
        /// DisplayObjectContainer are decreased by 1.
        /// The garbage collector is the process by which Flash Player reallocates unused memory space. When a variable
        /// or object is no longer actively referenced or stored somewhere, the garbage collector sweeps
        /// through and wipes out the memory space it used to occupy if no other references to it exist.
        /// </summary>
        /// <param name="child">The DisplayObject instance to remove.</param>
        /// <returns>
        /// The DisplayObject instance that you pass in the
        /// child parameter.
        /// </returns>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.display.DisplayObject removeChild(flash.display.DisplayObject child);

        /// <summary>
        /// Removes a child DisplayObject from the specified index position in the child list of
        /// the DisplayObjectContainer. The parent property of the removed child is set to
        /// null, and the object is garbage collected if no other references to the child exist. The index
        /// positions of any display objects above the child in the DisplayObjectContainer are decreased by 1.
        /// The garbage collector is the process by which Flash Player reallocates unused memory space. When a variable or
        /// object is no longer actively referenced or stored somewhere, the garbage collector sweeps
        /// through and wipes out the memory space it used to occupy if no other references to it exist.
        /// </summary>
        /// <param name="index">The child index of the DisplayObject to remove.</param>
        /// <returns>The DisplayObject instance that was removed.</returns>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.display.DisplayObject removeChildAt(int index);

        /// <summary>Returns the index position of a child DisplayObject instance.</summary>
        /// <param name="child">The DisplayObject instance to identify.</param>
        /// <returns>The index position of the child display object to identify.</returns>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int getChildIndex(flash.display.DisplayObject child);

        /// <summary>
        /// Changes the  position of an existing child in the display object container.
        /// This affects the layering of child objects. For example, the following example shows three
        /// display objects, labeled a, b, and c, at index positions 0, 1, and 2, respectively:
        /// When you use the setChildIndex() method and specify an index position
        /// that is already occupied, the child that occupies that position and all objects higher in
        /// the child list move up one position in the list. For example, if the display object container
        /// in the previous example is named container, you can swap the position
        /// of the display objects labeled a and b by calling the following code:container.setChildIndex(container.getChildAt(1), 0);This code results in the following arrangement of objects:
        /// </summary>
        /// <param name="child">
        /// The child DisplayObject instance for which you want to change
        /// the index number.
        /// </param>
        /// <param name="index">The resulting index number for the child display object.</param>
        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setChildIndex(flash.display.DisplayObject child, int index);

        /// <summary>Returns the child display object instance that exists at the specified index.</summary>
        /// <param name="index">The index position of the child object.</param>
        /// <returns>The child display object at the specified index position.</returns>
        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.display.DisplayObject getChildAt(int index);

        /// <summary>
        /// Returns the child display object that exists with the specified name.
        /// If more that one child display object has the specified name,
        /// the method returns the first object in the child list.
        /// The getChildAt() method is faster than the
        /// getChildByName() method. The getChildAt() method accesses
        /// a child from a cached array, whereas the getChildByName() method
        /// has to traverse a linked list to access a child.
        /// </summary>
        /// <param name="name">The name of the child to return.</param>
        /// <returns>The child display object with the specified name.</returns>
        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.display.DisplayObject getChildByName(Avm.String name);

        /// <summary>
        /// Returns an array of objects that lie under the specified point and are children
        /// (or grandchildren, and so on) of this DisplayObjectContainer instance. Any child objects that
        /// are inaccessible for security reasons are omitted from the returned array. To determine whether
        /// this security restriction affects the returned array, call the
        /// areInaccessibleObjectsUnderPoint() method.
        /// The point parameter is in the coordinate space of the Stage,
        /// which may differ from the coordinate space of the display object container (unless the
        /// display object container is the Stage). You can use the globalToLocal() and
        /// the localToGlobal() methods to convert points between these coordinate
        /// spaces.
        /// </summary>
        /// <param name="point">The point under which to look.</param>
        /// <returns>
        /// An array of objects that lie under the specified point and are children
        /// (or grandchildren, and so on) of this DisplayObjectContainer instance.
        /// </returns>
        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Array getObjectsUnderPoint(flash.geom.Point point);

        /// <summary>
        /// Indicates whether the security restrictions
        /// would cause any display objects to be omitted from the list returned by calling
        /// the DisplayObjectContainer.getObjectsUnderPoint() method
        /// with the specified point point. By default, content from one domain cannot
        /// access objects from another domain unless they are permitted to do so with a call to the
        /// Security.allowDomain() method.
        /// For more information, see the following: The security chapter in the
        /// Programming ActionScript 3.0 book and the latest comments on LiveDocsThe point parameter is in the coordinate space of the Stage,
        /// which may differ from the coordinate space of the display object container (unless the
        /// display object container is the Stage). You can use the globalToLocal() and
        /// the localToGlobal() methods to convert points between these coordinate
        /// spaces.
        /// </summary>
        /// <param name="point">The point under which to look.</param>
        /// <returns>true if the point contains child display objects with security restrictions.</returns>
        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool areInaccessibleObjectsUnderPoint(flash.geom.Point point);

        /// <summary>
        /// Determines whether the specified display object is a child of the DisplayObjectContainer instance or
        /// the instance itself.
        /// The search includes the entire display list including this DisplayObjectContainer instance. Grandchildren,
        /// great-grandchildren, and so on each return true.
        /// </summary>
        /// <param name="child">The child object to test.</param>
        /// <returns>
        /// true if the child object is a child of the DisplayObjectContainer
        /// or the container itself; otherwise false.
        /// </returns>
        [PageFX.AbcInstanceTrait(17)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool contains(flash.display.DisplayObject child);

        /// <summary>
        /// Swaps the z-order (front-to-back order) of the child objects at the two specified index positions in the
        /// child list. All other child objects in the display object container remain in the same index positions.
        /// </summary>
        /// <param name="index1">The index position of the first child object.</param>
        /// <param name="index2">The index position of the second child object.</param>
        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void swapChildrenAt(int index1, int index2);

        /// <summary>
        /// Swaps the z-order (front-to-back order) of the two specified child objects.  All other child
        /// objects in the display object container remain in the same index positions.
        /// </summary>
        /// <param name="child1">The first child object.</param>
        /// <param name="child2">The second child object.</param>
        [PageFX.AbcInstanceTrait(19)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void swapChildren(flash.display.DisplayObject child1, flash.display.DisplayObject child2);
    }
}
