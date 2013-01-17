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
    [PageFX.AbcInstance(64)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class DisplayObject : flash.events.EventDispatcher, flash.display.IBitmapDrawable
    {
        /// <summary>
        /// For a display object in a loaded SWF file, the root property is the
        /// top-most display object in the portion of the display list&apos;s tree structure represented
        /// by that SWF file. For a Bitmap object representing a loaded image file, the root
        /// property is the Bitmap object itself. For the instance of the main class of the
        /// first SWF file loaded, the root property is the display object itself.
        /// The root property of the Stage object is the Stage object itself. The
        /// root property is set to null for any display object that
        /// has not been added to the display list, unless it has been added to a display object
        /// container that is off the display list but that is a child of the top-most display
        /// object in a loaded SWF file.
        /// For example, if you create a new Sprite object by calling the Sprite()
        /// constructor method, its root property is null until you
        /// add it to the display list (or to a display object container that is off the display
        /// list but that is a child of the top-most display object in a SWF file).
        /// For a loaded SWF file, even though the Loader object used to load the file may not
        /// be on the display list, the top-most display object in the SWF file has its root
        /// property set to itself. The Loader object does not have its root property
        /// set until it is added as a child of a display object for which the root
        /// property is set.
        /// </summary>
        public extern virtual flash.display.DisplayObject root
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The Stage of the display object. A Flash application has only one Stage object.
        /// For example, you can create and load multiple display objects into the display list,
        /// and the stage property of each display object refers to the same Stage
        /// object (even if the display object belongs to a loaded SWF file).
        /// If a display object is not added to the display list, its stage property
        /// is set to null.
        /// </summary>
        public extern virtual flash.display.Stage stage
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Indicates the instance name of the DisplayObject. The object can be identified in
        /// the child list of its parent display object container by calling the getChildByName()
        /// method of the display object container.
        /// </summary>
        public extern virtual Avm.String name
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates the DisplayObjectContainer object that contains this display object. Use
        /// the parent property to specify a relative path to display objects that
        /// are above the current display object in the display list hierarchy.
        /// You can use parent to move up multiple levels in the display list as
        /// in the following:
        /// this.parent.parent.alpha = 20;
        /// </summary>
        public extern virtual flash.display.DisplayObjectContainer parent
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The calling display object is masked by the specified mask object.
        /// To ensure that masking works when the Stage is scaled, the mask display
        /// object must be in an active part of the display list. The mask object
        /// itself is not drawn. Set mask to null to remove the mask.
        /// To be able to scale a mask object, it must be on the display list. To be able to
        /// drag a mask Sprite object (by calling its startDrag() method), it must
        /// be on the display list. To call the startDrag() method for a mask sprite
        /// based on a mouseDown event being dispatched by the sprite, set the
        /// sprite&apos;s buttonMode property to true.
        /// </summary>
        public extern virtual flash.display.DisplayObject mask
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
        /// Whether or not the display object is visible. Display objects that are not visible
        /// are disabled. For example, if visible=false for an InteractiveObject
        /// instance, it cannot be clicked.
        /// </summary>
        public extern virtual bool visible
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
        /// Indicates the x coordinate of the DisplayObject instance relative to the
        /// local coordinates of the parent DisplayObjectContainer. If the object is inside
        /// a DisplayObjectContainer that has transformations, it is in the local coordinate
        /// system of the enclosing DisplayObjectContainer. Thus, for a DisplayObjectContainer
        /// rotated 90° counterclockwise, the DisplayObjectContainer&apos;s children inherit
        /// a coordinate system that is rotated 90° counterclockwise. The object&apos;s coordinates
        /// refer to the registration point position.
        /// </summary>
        public extern virtual double x
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
        /// Indicates the y coordinate of the DisplayObject instance relative to the
        /// local coordinates of the parent DisplayObjectContainer. If the object is inside
        /// a DisplayObjectContainer that has transformations, it is in the local coordinate
        /// system of the enclosing DisplayObjectContainer. Thus, for a DisplayObjectContainer
        /// rotated 90° counterclockwise, the DisplayObjectContainer&apos;s children inherit
        /// a coordinate system that is rotated 90° counterclockwise. The object&apos;s coordinates
        /// refer to the registration point position.
        /// </summary>
        public extern virtual double y
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

        public extern virtual double z
        {
            [PageFX.AbcInstanceTrait(13)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(14)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates the horizontal scale (percentage) of the object as applied
        /// from the registration point. The default registration point is (0,0). 1.0 equals
        /// 100% scale.
        /// Scaling the local coordinate system affects the x and y
        /// property settings, which are defined in whole pixels.
        /// </summary>
        public extern virtual double scaleX
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

        /// <summary>
        /// Indicates the vertical scale (percentage) of an object as applied from
        /// the registration point of the object. The default registration point is (0,0). 1.0
        /// is 100% scale.
        /// Scaling the local coordinate system affects the x and y
        /// property settings, which are defined in whole pixels.
        /// </summary>
        public extern virtual double scaleY
        {
            [PageFX.AbcInstanceTrait(17)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(18)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double scaleZ
        {
            [PageFX.AbcInstanceTrait(19)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(20)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>Indicates the x coordinate of the mouse position, in pixels.</summary>
        public extern virtual double mouseX
        {
            [PageFX.AbcInstanceTrait(21)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>Indicates the y coordinate of the mouse position, in pixels.</summary>
        public extern virtual double mouseY
        {
            [PageFX.AbcInstanceTrait(22)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Indicates the rotation of the DisplayObject instance, in degrees, from its original
        /// orientation. Values from 0 to 180 represent clockwise rotation; values from 0 to
        /// -180 represent counterclockwise rotation. Values outside this range are added to
        /// or subtracted from 360 to obtain a value within the range. For example, the statement
        /// my_video.rotation = 450 is the same as my_video.rotation = 90.
        /// </summary>
        public extern virtual double rotation
        {
            [PageFX.AbcInstanceTrait(23)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(24)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double rotationX
        {
            [PageFX.AbcInstanceTrait(25)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(26)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double rotationY
        {
            [PageFX.AbcInstanceTrait(27)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(28)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double rotationZ
        {
            [PageFX.AbcInstanceTrait(29)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(30)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates the alpha transparency value of the object specified. Valid values are
        /// 0 (fully transparent) to 1 (fully opaque). The default value is 1. Display objects
        /// with alpha set to 0 are active, even though they are invisible.
        /// </summary>
        public extern virtual double alpha
        {
            [PageFX.AbcInstanceTrait(31)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(32)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>Indicates the width of the display object, in pixels.</summary>
        public extern virtual double width
        {
            [PageFX.AbcInstanceTrait(33)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(34)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>Indicates the height of the display object, in pixels.</summary>
        public extern virtual double height
        {
            [PageFX.AbcInstanceTrait(35)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(36)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// If set to true, Flash Player caches an internal bitmap representation
        /// of the display object. This caching can increase performance for display objects
        /// that contain complex vector content.
        /// All vector data for a display object that has a cached bitmap is drawn to the bitmap
        /// instead of the main display. The bitmap is then copied to the main display as unstretched,
        /// unrotated pixels snapped to the nearest pixel boundaries. Pixels are mapped 1 to
        /// 1 with the parent object. If the bounds of the bitmap change, the bitmap is recreated
        /// instead of being stretched.
        /// No internal bitmap is created unless the cacheAsBitmap property is
        /// set to true.
        /// After you set the cacheAsBitmap property to true, the
        /// rendering does not change, however the display object performs pixel snapping automatically.
        /// The animation speed can be significantly faster depending on the complexity of the
        /// vector content.
        /// The cacheAsBitmap property is automatically set to true
        /// whenever you apply a filter to a display object (when its filter array
        /// is not empty), and if a display object has a filter applied to it, cacheAsBitmap
        /// is reported as true for that display object, even if you set the property
        /// to false. If you clear all filters for a display object, the cacheAsBitmap
        /// setting changes to what it was last set to.
        /// A display object does not use a bitmap even if the cacheAsBitmap property
        /// is set to true and instead renders from vector data in the following
        /// cases:The bitmap is too large: greater than 2880 pixels in either direction. The bitmap fails to allocate (out of memory error).
        /// The cacheAsBitmap property is best used with movie clips that have
        /// mostly static content and that do not scale and rotate frequently. With such movie
        /// clips, cacheAsBitmap can lead to performance increases when the movie
        /// clip is translated (when its x and y position is changed).
        /// </summary>
        public extern virtual bool cacheAsBitmap
        {
            [PageFX.AbcInstanceTrait(37)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(38)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Specifies whether the display object is opaque with a certain background color.
        /// A transparent bitmap contains alpha channel data and is drawn transparently. An
        /// opaque bitmap has no alpha channel (and renders faster than a transparent bitmap).
        /// If the bitmap is opaque, you specify its own background color to use.
        /// If set to a number value, the surface is opaque (not transparent) with the RGB background
        /// color that the number specifies. If set to null (the default value),
        /// the display object has a transparent background.
        /// The opaqueBackground property is intended mainly for use with the
        /// cacheAsBitmap property, for rendering optimization. For display objects
        /// in which the cacheAsBitmap property is set to true, setting opaqueBackground
        /// can improve rendering performance.
        /// The opaque background region is not matched when calling the hitTestPoint()
        /// method with the shapeFlag parameter set to true.
        /// The opaque background region does not respond to mouse events.
        /// </summary>
        public extern virtual object opaqueBackground
        {
            [PageFX.AbcInstanceTrait(39)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(40)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The scroll rectangle bounds of the display object. The display object is cropped
        /// to the size defined by the rectangle, and it scrolls within the rectangle when you
        /// change the x and y properties of the scrollRect
        /// object.
        /// The properties of the scrollRect Rectangle object use the display object&apos;s
        /// coordinate space and are scaled just like the overall display object. The corner
        /// bounds of the cropped window on the scrolling display object are the origin of the
        /// display object (0,0) and the point defined by the width and height of the rectangle.
        /// They are not centered around the origin, but use the origin to define the upper-left
        /// corner of the area. A scrolled display object always scrolls in whole pixel increments.
        /// You can scroll an object left and right by setting the x property of
        /// the scrollRect Rectangle object. You can scroll an object up and down
        /// by setting the y property of the scrollRect Rectangle
        /// object. If the display object is rotated 90° and you scroll it left and right,
        /// the display object actually scrolls up and down.
        /// </summary>
        public extern virtual flash.geom.Rectangle scrollRect
        {
            [PageFX.AbcInstanceTrait(41)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(42)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// An indexed array that contains each filter object currently associated with the
        /// display object. The flash.filters package contains several classes that define specific
        /// filters you can use.
        /// Filters can be applied in the Flash authoring tool at design-time, or at runtime
        /// by using ActionScript code. To apply a filter by using ActionScript, you must make
        /// a temporary copy of the entire filters array, modify the temporary
        /// array, then assign the value of the temporary array back to the filters
        /// array. You cannot directly add a new filter object to the filters array.
        /// The following code has no effect on the target display object, named myDisplayObject:
        /// myDisplayObject.filters[0].push(myDropShadow);
        /// To add a filter by using ActionScript, perform the following steps (assume that
        /// the target display object is named myDisplayObject):Create a new filter object by using the constructor method of your chosen filter
        /// class.Assign the value of the myDisplayObject.filters array to a temporary
        /// array, such as one named myFilters.Add the new filter object to the myFilters temporary array.Assign the value of the temporary array to the myDisplayObject.filters
        /// array.
        /// If the filters array is undefined, you do not need to use a temporary
        /// array. Instead, you can directly assign an array literal that contains one or more
        /// filter objects that you create. The first example in the Examples section adds a
        /// drop shadow filter by using code that handles both defined and undefined filters
        /// arrays.
        /// To modify an existing filter object, you must use the technique of modifying a copy
        /// of the filters array:Assign the value of the filters array to a temporary array, such as
        /// one named myFilters.Modify the property by using the temporary array, myFilters. For example,
        /// to set the quality property of the first filter in the array, you could use the
        /// following code: myFilters[0].quality = 1;Assign the value of the temporary array to the filters array.
        /// At load time, if a display object has an associated filter, it is marked to cache
        /// itself as a transparent bitmap. From this point forward, as long as the display
        /// object has a valid filter list, the player caches the display object as a bitmap.
        /// This source bitmap is used as a source image for the filter effects. Each display
        /// object usually has two bitmaps: one with the original unfiltered source display
        /// object and another for the final image after filtering. The final image is used
        /// when rendering. As long as the display object does not change, the final image does
        /// not need updating.
        /// The flash.filters package includes classes for filters. For example, to create a
        /// DropShadow filter, you would write:
        /// import flash.filters.DropShadowFilter
        /// var myFilter:DropShadowFilter = new DropShadowFilter (distance, angle, color, alpha, blurX, blurY, quality, inner, knockout)
        /// You can use the is operator to determine the type of filter assigned
        /// to each index position in the filter array. For example, the following
        /// code shows how to determine the position of the first filter in the filters
        /// array that is a DropShadowFilter:
        /// import flash.text.TextField;
        /// import flash.filters.~~;
        /// var tf:TextField = new TextField();
        /// var filter1:DropShadowFilter = new DropShadowFilter();
        /// var filter2:GradientGlowFilter = new GradientGlowFilter();
        /// tf.filters = [filter1, filter2];
        /// tf.text = &quot;DropShadow index: &quot; + filterPosition(tf, DropShadowFilter).toString(); // 0
        /// addChild(tf)
        /// function filterPosition(displayObject:DisplayObject, filterClass:Class):int {
        /// for (var i:uint = 0; i &lt; displayObject.filters.length; i++) {
        /// if (displayObject.filters[i] is filterClass) {
        /// return i;
        /// }
        /// }
        /// return -1;
        /// }
        /// </summary>
        public extern virtual Avm.Array filters
        {
            [PageFX.AbcInstanceTrait(43)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(44)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// A value from the BlendMode class that specifies which blend mode to use. A bitmap
        /// can be drawn internally in two ways. If you have a blend mode enabled or an external
        /// clipping mask, the bitmap is drawn by adding a bitmap-filled square shape to the
        /// vector render. If you attempt to set this property to an invalid value, Flash Player
        /// sets the value to BlendMode.NORMAL.
        /// Flash Player applies the blendMode property on each pixel of the display
        /// object. Each pixel is composed of three constituent colors (red, green, and blue),
        /// and each constituent color has a value between 0x00 and 0xFF. Flash Player compares
        /// each constituent color of one pixel in the movie clip with the corresponding color
        /// of the pixel in the background. For example, if blendMode is set to
        /// BlendMode.LIGHTEN, Flash Player compares the red value of the display
        /// object with the red value of the background, and uses the lighter of the two as
        /// the value for the red component of the displayed color.
        /// The following table describes the blendMode settings. The BlendMode
        /// class defines string values you can use. The illustrations in the table show blendMode
        /// values applied to a circular display object (2) superimposed on another display
        /// object (1).
        /// BlendMode Constant
        /// Illustration
        /// DescriptionBlendMode.NORMAL
        /// The display object appears in front of the background. Pixel values of the display
        /// object override those of the background. Where the display object is transparent,
        /// the background is visible.BlendMode.LAYER
        /// Forces the creation of a transparency group for the display object. This means that
        /// the display object is pre-composed in a temporary buffer before it is processed
        /// further. This is done automatically if the display object is pre-cached using bitmap
        /// caching or if the display object is a display object container with at least one
        /// child object with a blendMode setting other than BlendMode.NORMAL.
        /// BlendMode.MULTIPLY
        /// Multiplies the values of the display object constituent colors by the colors of
        /// the background color, and then normalizes by dividing by 0xFF, resulting in darker
        /// colors. This setting is commonly used for shadows and depth effects.
        /// For example, if a constituent color (such as red) of one pixel in the display object
        /// and the corresponding color of the pixel in the background both have the value 0x88,
        /// the multiplied result is 0x4840. Dividing by 0xFF yields a value of 0x48 for that
        /// constituent color, which is a darker shade than the color of the display object
        /// or the color of the background.BlendMode.SCREEN
        /// Multiplies the complement (inverse) of the display object color by the complement
        /// of the background color, resulting in a bleaching effect. This setting is commonly
        /// used for highlights or to remove black areas of the display object.BlendMode.LIGHTEN
        /// Selects the lighter of the constituent colors of the display object and the color
        /// of the background (the colors with the larger values). This setting is commonly
        /// used for superimposing type.
        /// For example, if the display object has a pixel with an RGB value of 0xFFCC33, and
        /// the background pixel has an RGB value of 0xDDF800, the resulting RGB value for the
        /// displayed pixel is 0xFFF833 (because 0xFF &gt; 0xDD, 0xCC &lt; 0xF8, and 0x33 &gt;
        /// 0x00 = 33).BlendMode.DARKEN
        /// Selects the darker of the constituent colors of the display object and the colors
        /// of the background (the colors with the smaller values). This setting is commonly
        /// used for superimposing type.
        /// For example, if the display object has a pixel with an RGB value of 0xFFCC33, and
        /// the background pixel has an RGB value of 0xDDF800, the resulting RGB value for the
        /// displayed pixel is 0xDDCC00 (because 0xFF &gt; 0xDD, 0xCC &lt; 0xF8, and 0x33 &gt;
        /// 0x00 = 33).BlendMode.DIFFERENCE
        /// Compares the constituent colors of the display object with the colors of its background,
        /// and subtracts the darker of the values of the two constituent colors from the lighter
        /// value. This setting is commonly used for more vibrant colors.
        /// For example, if the display object has a pixel with an RGB value of 0xFFCC33, and
        /// the background pixel has an RGB value of 0xDDF800, the resulting RGB value for the
        /// displayed pixel is 0x222C33 (because 0xFF - 0xDD = 0x22, 0xF8 - 0xCC = 0x2C, and
        /// 0x33 - 0x00 = 0x33).BlendMode.ADD
        /// Adds the values of the constituent colors of the display object to the colors of
        /// its background, applying a ceiling of 0xFF. This setting is commonly used for animating
        /// a lightening dissolve between two objects.
        /// For example, if the display object has a pixel with an RGB value of 0xAAA633, and
        /// the background pixel has an RGB value of 0xDD2200, the resulting RGB value for the
        /// displayed pixel is 0xFFC833 (because 0xAA + 0xDD &gt; 0xFF, 0xA6 + 0x22 = 0xC8,
        /// and 0x33 + 0x00 = 0x33).BlendMode.SUBTRACT
        /// Subtracts the values of the constituent colors in the display object from the values
        /// of the background color, applying a floor of 0. This setting is commonly used for
        /// animating a darkening dissolve between two objects.
        /// For example, if the display object has a pixel with an RGB value of 0xAA2233, and
        /// the background pixel has an RGB value of 0xDDA600, the resulting RGB value for the
        /// displayed pixel is 0x338400 (because 0xDD - 0xAA = 0x33, 0xA6 - 0x22 = 0x84, and
        /// 0x00 - 0x33 &lt; 0x00).BlendMode.INVERT
        /// Inverts the background.BlendMode.ALPHA
        /// Applies the alpha value of each pixel of the display object to the background. This
        /// requires the blendMode setting of the parent display object to be set
        /// to BlendMode.LAYER. For example, in the illustration, the parent display
        /// object, which is a white background, has blendMode = BlendMode.LAYER.BlendMode.ERASE
        /// Erases the background based on the alpha value of the display object. This requires
        /// the blendMode of the parent display object to be set to BlendMode.LAYER.
        /// For example, in the illustration, the parent display object, which is a white background,
        /// has blendMode = BlendMode.LAYER.BlendMode.OVERLAY
        /// Adjusts the color of each pixel based on the darkness of the background. If the
        /// background is lighter than 50% gray, the display object and background colors are
        /// screened, which results in a lighter color. If the background is darker than 50%
        /// gray, the colors are multiplied, which results in a darker color. This setting is
        /// commonly used for shading effects.BlendMode.HARDLIGHT
        /// Adjusts the color of each pixel based on the darkness of the display object. If
        /// the display object is lighter than 50% gray, the display object and background colors
        /// are screened, which results in a lighter color. If the display object is darker
        /// than 50% gray, the colors are multiplied, which results in a darker color. This
        /// setting is commonly used for shading effects.
        /// </summary>
        public extern virtual Avm.String blendMode
        {
            [PageFX.AbcInstanceTrait(45)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(46)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// An object with properties pertaining to a display object&apos;s matrix, color transform,
        /// and pixel bounds. The specific properties  matrix, colorTransform, and three read-only
        /// properties (concatenatedMatrix, concatenatedColorTransform,
        /// and pixelBounds)  are described in the entry for the Transform class.
        /// Each of the transform object&apos;s properties is itself an object. This concept is important
        /// because the only way to set new values for the matrix or colorTransform objects
        /// is to create a new object and copy that object into the transform.matrix or transform.colorTransform
        /// property.
        /// For example, to increase the tx value of a display object&apos;s matrix,
        /// you must make a copy of the entire matrix object, then copy the new object into
        /// the matrix property of the transform object:var myMatrix:Object = myDisplayObject.transform.matrix; myMatrix.tx += 10;
        /// myDisplayObject.transform.matrix = myMatrix;
        /// You cannot directly set the tx property. The following code has no
        /// effect on myDisplayObject:
        /// myDisplayObject.transform.matrix.tx += 10;
        /// You can also copy an entire transform object and assign it to another display object&apos;s
        /// transform property. For example, the following code copies the entire transform
        /// object from myOldDisplayObj to myNewDisplayObj:myNewDisplayObj.transform = myOldDisplayObj.transform;
        /// The resulting display object, myNewDisplayObj, now has the same values
        /// for its matrix, color transform, and pixel bounds as the old display object, myOldDisplayObj.
        /// </summary>
        public extern virtual flash.geom.Transform transform
        {
            [PageFX.AbcInstanceTrait(47)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(48)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The current scaling grid that is in effect. If set to null, the entire
        /// display object is scaled normally when any scale transformation is applied.
        /// When you define the scale9Grid property, the display object is divided
        /// into a grid with nine regions based on the scale9Grid rectangle, which
        /// defines the center region of the grid. The eight other regions of the grid are the
        /// following areas:
        /// The upper-left corner outside of the rectangleThe area above the rectangle The upper-right corner outside of the rectangleThe area to the left of the rectangleThe area to the right of the rectangleThe lower-left corner outside of the rectangleThe area below the rectangleThe lower-right corner outside of the rectangle
        /// You can think of the eight regions outside of the center (defined by the rectangle)
        /// as being like a picture frame that has special rules applied to it when scaled.
        /// When the scale9Grid property is set and a display object is scaled,
        /// all text and gradients are scaled normally; however, for other types of objects
        /// the following rules apply:Content in the center region is scaled normally. Content in the corners is not scaled. Content in the top and bottom regions is scaled horizontally only. Content in the
        /// left and right regions is scaled vertically only.All fills (including bitmaps, video, and gradients) are stretched to fit their shapes.
        /// If a display object is rotated, all subsequent scaling is normal (and the scale9Grid
        /// property is ignored).
        /// For example, consider the following display object and a rectangle that is applied
        /// as the display object&apos;s scale9Grid:
        /// The display object.
        /// The red rectangle shows the scale9Grid.
        /// When the display object is scaled or stretched, the objects within the rectangle
        /// scale normally, but the objects outside of the rectangle scale according to the
        /// scale9Grid rules:
        /// Scaled to 75%:
        /// Scaled to 50%:
        /// Scaled to 25%:
        /// Stretched horizontally 150%:
        /// A common use for setting scale9Grid is to set up a display object to
        /// be used as a component, in which edge regions retain the same width when the component
        /// is scaled.
        /// </summary>
        public extern virtual flash.geom.Rectangle scale9Grid
        {
            [PageFX.AbcInstanceTrait(49)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(50)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Returns a LoaderInfo object containing information about loading the file to which
        /// this display object belongs. The loaderInfo property is defined only
        /// for the root display object of a SWF file or for a loaded Bitmap (not for a Bitmap
        /// that is drawn with ActionScript). To find the loaderInfo object associated
        /// with the SWF file that contains a display object named myDisplayObject,
        /// use myDisplayObject.root.loaderInfo.
        /// A large SWF file can monitor its download by calling this.root.loaderInfo.addEventListener(Event.COMPLETE,
        /// func).
        /// </summary>
        public extern virtual flash.display.LoaderInfo loaderInfo
        {
            [PageFX.AbcInstanceTrait(55)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// The current accessibility options for this display object. If you modify the accessibilityProperties
        /// property or any of the fields within accessibilityProperties, you must
        /// call the Accessibility.updateProperties() method to make your changes
        /// take effect.
        /// Note: For an object created in the Flash authoring environment, the value
        /// of accessibilityProperties is prepopulated with any information you
        /// entered in the Accessibility panel for that object.
        /// </summary>
        public extern virtual flash.accessibility.AccessibilityProperties accessibilityProperties
        {
            [PageFX.AbcInstanceTrait(59)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(60)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual flash.display.Shader blendShader
        {
            [PageFX.AbcInstanceTrait(63)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
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
        /// Converts the point object from the Stage (global) coordinates to the
        /// display object&apos;s (local) coordinates.
        /// To use this method, first create an instance of the Point class. The x and
        /// y values that you assign represent global coordinates because they relate
        /// to the origin (0,0) of the main display area. Then pass the Point instance as the
        /// parameter to the globalToLocal() method. The method returns a new Point
        /// object with x and y values that relate to the origin of the display
        /// object instead of the origin of the Stage.
        /// </summary>
        /// <param name="point">
        /// An object created with the Point class. The Point object specifies the x
        /// and y coordinates as properties.
        /// </param>
        /// <returns>
        /// A Point object
        /// with coordinates relative to the display object.
        /// </returns>
        [PageFX.AbcInstanceTrait(51)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Point globalToLocal(flash.geom.Point point);

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
        /// <param name="point">
        /// The name or identifier of a point created with the Point class, specifying
        /// the x and y coordinates as properties.
        /// </param>
        /// <returns>
        /// A Point object
        /// with coordinates relative to the Stage.
        /// </returns>
        [PageFX.AbcInstanceTrait(52)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Point localToGlobal(flash.geom.Point point);

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
        /// <param name="targetCoordinateSpace">The display object that defines the coordinate system to use.</param>
        /// <returns>
        /// The
        /// rectangle that defines the area of the display object relative to the targetCoordinateSpace
        /// object&apos;s coordinate system.
        /// </returns>
        [PageFX.AbcInstanceTrait(53)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Rectangle getBounds(flash.display.DisplayObject targetCoordinateSpace);

        /// <summary>
        /// Returns a rectangle that defines the boundary of the display object, based on the
        /// coordinate system defined by the targetCoordinateSpace parameter, excluding
        /// any strokes on shapes. The values that the getRect() method returns
        /// are the same or smaller than those returned by the getBounds() method.
        /// Note: Use localToGlobal() and globalToLocal() methods
        /// to convert the display object&apos;s local coordinates to Stage coordinates, or Stage
        /// coordinates to local coordinates, respectively.
        /// </summary>
        /// <param name="targetCoordinateSpace">The display object that defines the coordinate system to use.</param>
        /// <returns>
        /// The
        /// rectangle that defines the area of the display object relative to the targetCoordinateSpace
        /// object&apos;s coordinate system.
        /// </returns>
        [PageFX.AbcInstanceTrait(54)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Rectangle getRect(flash.display.DisplayObject targetCoordinateSpace);

        /// <summary>
        /// Evaluates the display object to see if it overlaps or intersects with the obj
        /// display object.
        /// </summary>
        /// <param name="obj">The display object to test against.</param>
        /// <returns>
        /// true
        /// if the display objects intersect; false if not.
        /// </returns>
        [PageFX.AbcInstanceTrait(56)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool hitTestObject(flash.display.DisplayObject obj);

        /// <summary>
        /// Evaluates the display object to see if it overlaps or intersects with the point
        /// specified by the x and y parameters. The x
        /// and y parameters specify a point in the coordinate space of the Stage,
        /// not the display object container that contains the display object (unless that display
        /// object container is the Stage).
        /// </summary>
        /// <param name="x">The x coordinate to test against this object.</param>
        /// <param name="y">The y coordinate to test against this object.</param>
        /// <param name="shapeFlag">
        /// (default = false)  Whether to check against the
        /// actual pixels of the object (true) or the bounding box (false).
        /// </param>
        /// <returns>
        /// true
        /// if the display object overlaps or intersects with the specified point; false
        /// otherwise.
        /// </returns>
        [PageFX.AbcInstanceTrait(57)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool hitTestPoint(double x, double y, bool shapeFlag);

        [PageFX.AbcInstanceTrait(57)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool hitTestPoint(double x, double y);

        [PageFX.AbcInstanceTrait(61)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Vector3D globalToLocal3D(flash.geom.Point point);

        [PageFX.AbcInstanceTrait(62)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Point local3DToGlobal(flash.geom.Vector3D point3d);


    }
}
