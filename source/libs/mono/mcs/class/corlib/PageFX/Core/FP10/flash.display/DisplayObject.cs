using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The DisplayObject class is the base class for all objects that can be placed on
    /// the display list. The display list manages all objects displayed in Flash Player.
    /// Use the DisplayObjectContainer class to arrange the display objects in the display
    /// list. DisplayObjectContainer objects can have child display objects, while other
    /// display objects, such as Shape and TextField objects, are &quot;leaf&quot; nodes that have
    /// only parents and siblings, no children.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class DisplayObject : flash.events.EventDispatcher, IBitmapDrawable
    {
        public extern virtual bool visible
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

        public extern virtual Stage stage
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String name
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

        public extern virtual double width
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

        public extern virtual Avm.String blendMode
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

        public extern virtual flash.geom.Rectangle scale9Grid
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

        public extern virtual double rotationX
        {
            [PageFX.ABC]
            [PageFX.QName("rotationX", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.QName("rotationX", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double rotationY
        {
            [PageFX.ABC]
            [PageFX.QName("rotationY", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.QName("rotationY", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double scaleX
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

        public extern virtual double scaleY
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

        public extern virtual double scaleZ
        {
            [PageFX.ABC]
            [PageFX.QName("scaleZ", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.QName("scaleZ", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual flash.accessibility.AccessibilityProperties accessibilityProperties
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

        public extern virtual flash.geom.Rectangle scrollRect
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

        public extern virtual double height
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

        public extern virtual bool cacheAsBitmap
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

        public extern virtual double rotationZ
        {
            [PageFX.ABC]
            [PageFX.QName("rotationZ", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.QName("rotationZ", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Shader blendShader
        {
            [PageFX.ABC]
            [PageFX.QName("blendShader", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual object opaqueBackground
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

        public extern virtual DisplayObjectContainer parent
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double alpha
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

        public extern virtual double mouseX
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double mouseY
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual DisplayObject mask
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

        public extern virtual flash.geom.Transform transform
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

        public extern virtual LoaderInfo loaderInfo
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual DisplayObject root
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual double x
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

        public extern virtual double y
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

        public extern virtual double z
        {
            [PageFX.ABC]
            [PageFX.QName("z", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.QName("z", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.Array filters
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

        public extern virtual double rotation
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

        [PageFX.Event("render")]
        public event flash.events.EventHandler render
        {
            add { }
            remove { }
        }

        [PageFX.Event("removedFromStage")]
        public event flash.events.EventHandler removedFromStage
        {
            add { }
            remove { }
        }

        [PageFX.Event("removed")]
        public event flash.events.EventHandler removed
        {
            add { }
            remove { }
        }

        [PageFX.Event("exitFrame")]
        public event flash.events.EventHandler exitFrame
        {
            add { }
            remove { }
        }

        [PageFX.Event("frameConstructed")]
        public event flash.events.EventHandler frameConstructed
        {
            add { }
            remove { }
        }

        [PageFX.Event("enterFrame")]
        public event flash.events.EventHandler enterFrame
        {
            add { }
            remove { }
        }

        [PageFX.Event("addedToStage")]
        public event flash.events.EventHandler addedToStage
        {
            add { }
            remove { }
        }

        [PageFX.Event("added")]
        public event flash.events.EventHandler added
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DisplayObject();

        /// <summary>
        /// Converts the point object from the display object&apos;s (local) coordinates
        /// to the Stage (global) coordinates.
        /// This method allows you to convert any given x and y coordinates from
        /// values that are relative to the origin (0,0) of a specific display object (local
        /// coordinates) to values that are relative to the origin of the Stage (global coordinates).
        /// To use this method, first create an instance of the Point class. The x and
        /// y values that you assign represent local coordinates because they relate
        /// to the origin of the display object.
        /// You then pass the Point instance that you created as the parameter to the localToGlobal()
        /// method. The method returns a new Point object with x and y values
        /// that relate to the origin of the Stage instead of the origin of the display object.
        /// </summary>
        /// <param name="arg0">
        /// The name or identifier of a point created with the Point class, specifying
        /// the x and y coordinates as properties.
        /// </param>
        /// <returns>
        /// A Point object
        /// with coordinates relative to the Stage.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Point localToGlobal(flash.geom.Point arg0);

        /// <summary>
        /// Converts the point object from the Stage (global) coordinates to the
        /// display object&apos;s (local) coordinates.
        /// To use this method, first create an instance of the Point class. The x and
        /// y values that you assign represent global coordinates because they relate
        /// to the origin (0,0) of the main display area. Then pass the Point instance as the
        /// parameter to the globalToLocal() method. The method returns a new Point
        /// object with x and y values that relate to the origin of the display
        /// object instead of the origin of the Stage.
        /// </summary>
        /// <param name="arg0">
        /// An object created with the Point class. The Point object specifies the x
        /// and y coordinates as properties.
        /// </param>
        /// <returns>
        /// A Point object
        /// with coordinates relative to the display object.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Point globalToLocal(flash.geom.Point arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double dyna1_NewFunc1();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double newFunc1();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual double newFunc2();

        /// <summary>
        /// Returns a rectangle that defines the area of the display object relative to the
        /// coordinate system of the targetCoordinateSpace object. Consider the
        /// following code, which shows how the rectangle returned can vary depending on the
        /// targetCoordinateSpace parameter that you pass to the method:
        /// var container:Sprite = new Sprite();
        /// container.x = 100;
        /// container.y = 100;
        /// this.addChild(container);
        /// var contents:Shape = new Shape();
        /// contents.graphics.drawCircle(0,0,100);
        /// container.addChild(contents);
        /// trace(contents.getBounds(container));
        /// // (x=-100, y=-100, w=200, h=200)
        /// trace(contents.getBounds(this));
        /// // (x=0, y=0, w=200, h=200)
        /// Note: Use the localToGlobal() and globalToLocal()
        /// methods to convert the display object&apos;s local coordinates to display coordinates,
        /// or display coordinates to local coordinates, respectively.
        /// The getBounds() method is similar to the getRect() method;
        /// however, the Rectangle returned by the getBounds() method includes
        /// any strokes on shapes, whereas the Rectangle returned by the getRect()
        /// method does not. For an example, see the description of the getRect()
        /// method.
        /// </summary>
        /// <param name="arg0">The display object that defines the coordinate system to use.</param>
        /// <returns>
        /// The
        /// rectangle that defines the area of the display object relative to the targetCoordinateSpace
        /// object&apos;s coordinate system.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Rectangle getBounds(DisplayObject arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object newFunc3(DisplayObject arg0);

        [PageFX.ABC]
        [PageFX.QName("local3DToGlobal", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Point local3DToGlobal(flash.geom.Vector3D arg0);

        [PageFX.ABC]
        [PageFX.QName("globalToLocal3D", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Vector3D globalToLocal3D(flash.geom.Point arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object dyna2_NewFunc1(DisplayObject arg0);

        /// <summary>
        /// Evaluates the display object to see if it overlaps or intersects with the point
        /// specified by the x and y parameters. The x
        /// and y parameters specify a point in the coordinate space of the Stage,
        /// not the display object container that contains the display object (unless that display
        /// object container is the Stage).
        /// </summary>
        /// <param name="arg0">The x coordinate to test against this object.</param>
        /// <param name="arg1">The y coordinate to test against this object.</param>
        /// <param name="arg2">
        /// (default = false)  Whether to check against the
        /// actual pixels of the object (true) or the bounding box (false).
        /// </param>
        /// <returns>
        /// true
        /// if the display object overlaps or intersects with the specified point; false
        /// otherwise.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool hitTestPoint(double arg0, double arg1, bool arg2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool hitTestPoint(double arg0, double arg1);

        /// <summary>
        /// Returns a rectangle that defines the boundary of the display object, based on the
        /// coordinate system defined by the targetCoordinateSpace parameter, excluding
        /// any strokes on shapes. The values that the getRect() method returns
        /// are the same or smaller than those returned by the getBounds() method.
        /// Note: Use localToGlobal() and globalToLocal() methods
        /// to convert the display object&apos;s local coordinates to Stage coordinates, or Stage
        /// coordinates to local coordinates, respectively.
        /// </summary>
        /// <param name="arg0">The display object that defines the coordinate system to use.</param>
        /// <returns>
        /// The
        /// rectangle that defines the area of the display object relative to the targetCoordinateSpace
        /// object&apos;s coordinate system.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Rectangle getRect(DisplayObject arg0);

        /// <summary>
        /// Evaluates the display object to see if it overlaps or intersects with the obj
        /// display object.
        /// </summary>
        /// <param name="arg0">The display object to test against.</param>
        /// <returns>
        /// true
        /// if the display objects intersect; false if not.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool hitTestObject(DisplayObject arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object dyna3_NewFunc1(DisplayObject arg0);
    }
}
