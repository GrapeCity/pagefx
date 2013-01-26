using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// The Stage class represents the main drawing area. The Stage represents the entire area where Flash ®
    /// content is shown.
    /// </summary>
    [PageFX.AbcInstance(90)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class Stage : flash.display.DisplayObjectContainer
    {
        /// <summary>
        /// Gets and sets the frame rate of the stage. The frame rate is defined as frames per second.
        /// By default the rate is set to the frame rate of the first SWF file loaded. Valid range for the frame rate
        /// is from 0.01 to 1000 frames per second.
        /// Note: Flash Player might not be able to follow
        /// high frame rate settings, either because the target platform is not fast enough or the player is
        /// synchronized to the vertical blank timing of the display device (usually 60 Hz on LCD devices).
        /// In some cases, a target platform might also choose to lower the maximum frame rate if it
        /// anticipates high CPU usage.
        /// </summary>
        public extern virtual double frameRate
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// A value from the StageScaleMode class that specifies which scale mode to use.
        /// The following are valid values:
        /// StageScaleMode.EXACT_FITThe entire Flash application is
        /// visible in the specified area without distortion while maintaining the original aspect ratio
        /// of the application. Borders can appear on two sides of the application.
        /// StageScaleMode.SHOW_ALLThe entire Flash application is visible in
        /// the specified area without trying to preserve the original aspect ratio. Distortion can occur.
        /// StageScaleMode.NO_BORDERThe entire Flash application fills the specified area,
        /// without distortion but possibly with some cropping, while maintaining the original aspect ratio
        /// of the application.
        /// StageScaleMode.NO_SCALEThe entire Flash application is fixed, so that
        /// it remains unchanged even as the size of the player window changes. Cropping might
        /// occur if the player window is smaller than the content.
        /// </summary>
        public extern virtual Avm.String scaleMode
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
        /// A value from the StageAlign class that specifies the alignment of the stage in
        /// Flash Player or the browser. The following are valid values:
        /// ValueVertical AlignmentHorizontalStageAlign.TOPTopCenterStageAlign.BOTTOMBottomCenterStageAlign.LEFTCenterLeftStageAlign.RIGHTCenterRightStageAlign.TOP_LEFTTopLeftStageAlign.TOP_RIGHTTopRightStageAlign.BOTTOM_LEFTBottomLeftStageAlign.BOTTOM_RIGHTBottomRightThe align property is only available to an object that is in the same security sandbox
        /// as the Stage owner (the main SWF file).
        /// To avoid this, the Stage owner can grant permission to the domain of the
        /// calling object by calling the Security.allowDomain() method or the Security.alowInsecureDomain() method.
        /// For more information, see the &quot;Security&quot; chapter in Programming ActionScript 3.0.
        /// </summary>
        public extern virtual Avm.String align
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
        /// Specifies the current width, in pixels, of the Stage.
        /// When the value of the scaleMode property is set to
        /// StageScaleMode.NO_SCALE, the
        /// stageWidth property represents the width of Flash Player. This means that the
        /// stageWidth property varies as you resize the Flash Player window. When the
        /// value of the scaleMode property is not set to StageScaleMode.NO_SCALE, the
        /// stageWidth property represents the width of the SWF file as set during authoring in
        /// the Document Properties dialog box. This means that the value of the stageWidth
        /// property stays constant as you resize the Flash Player window.  This property cannot be set.
        /// </summary>
        public extern virtual int stageWidth
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
        /// The current height, in pixels, of the Stage.
        /// When the value of the Stage.scaleMode property is set to
        /// StageScaleMode.NO_SCALE, the
        /// stageHeight property represents the height of the Flash Player window. When the value of
        /// Stage.scaleMode is not set to StageScaleMode.NO_SCALE, stageHeight represents
        /// the height of the SWF file.  This property cannot be set.
        /// </summary>
        public extern virtual int stageHeight
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
        /// Specifies whether to show or hide the default items in the Flash Player
        /// context menu.
        /// If the showDefaultContextMenu property is set to true (the
        /// default), all context menu items appear. If the showDefaultContextMenu property
        /// is set to false, only the Settings and About Adobe Flash Player menu items appear.
        /// </summary>
        public extern virtual bool showDefaultContextMenu
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
        /// The interactive object with keyboard focus; or null if focus is not set
        /// or if the focused object belongs to a security sandbox to which the calling object does not
        /// have access.
        /// </summary>
        public extern virtual flash.display.InteractiveObject focus
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

        public extern virtual Avm.String colorCorrection
        {
            [PageFX.AbcInstanceTrait(15)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(16)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String colorCorrectionSupport
        {
            [PageFX.AbcInstanceTrait(17)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>Specifies whether or not objects display a glowing border when they have focus.</summary>
        public extern virtual bool stageFocusRect
        {
            [PageFX.AbcInstanceTrait(19)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(20)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// A value from the StageQuality class that specifies which rendering quality Flash Player uses.
        /// The following are valid values:
        /// StageQuality.LOWLow rendering quality. Graphics are not
        /// anti-aliased, and bitmaps are not smoothed.StageQuality.MEDIUMMedium rendering quality. Graphics are
        /// anti-aliased using a 2 x 2 pixel grid, but bitmaps are not
        /// smoothed. This setting is suitable for movies that do not contain text.StageQuality.HIGHHigh rendering quality. Graphics are anti-aliased
        /// using a 4 x 4 pixel grid, and bitmaps are smoothed if the movie
        /// is static. This is the default rendering quality setting that Flash Player uses.StageQuality.BESTVery high rendering quality. Graphics are
        /// anti-aliased using a 4 x 4 pixel grid and bitmaps are always
        /// smoothed.Note: The operating system draws the device fonts,
        /// which are therefore unaffected by the quality property.
        /// </summary>
        public extern virtual Avm.String quality
        {
            [PageFX.AbcInstanceTrait(21)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(22)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// A value from the StageDisplayState class that specifies which display state to use. The following
        /// are valid values:
        /// StageDisplayState.FULL_SCREEN Sets the Apollo runtime or Flash Player to expand the
        /// stage over the user&apos;s entire screen.StageDisplayState.NORMAL Sets the player back to the standard stage display mode.For SWF content running in the browser, the scaling behavior of the movie in
        /// full-screen mode is determined by the scaleMode
        /// setting (set using the Stage.scaleMode property or the SWF file&apos;s embed
        /// tag settings in the HTML file). If the scaleMode property is set to noScale
        /// while Flash Player transitions to full-screen mode, the Stage width and height
        /// properties are updated, and Flash Player dispatches the Stage.resize event.The following restrictions apply to SWF files that play within an HTML page (not those using the stand-alone
        /// Flash Player or not running in the Apollo runtime):To enable full-screen mode, add the allowFullScreen parameter to the object
        /// and embed tags in the HTML page that includes the SWF file, with allowFullScreen set
        /// to &quot;true&quot;, as shown in the following example:
        /// &lt;param name=&quot;allowFullScreen&quot; value=&quot;true&quot; /&gt;
        /// ...
        /// &lt;embed src=&quot;example.swf&quot; allowFullScreen=&quot;true&quot; ... &gt;An HTML page may also use a script to generate SWF-embedding tags. You need to alter the script
        /// so that it inserts the proper allowNetworking settings. HTML pages generated by Flash and
        /// FlexBuilder use the AC_FL_RunContent() function to embed references to SWF files, and you
        /// need to add the  allowNetworking parameter settings, as in the following:AC_FL_RunContent( ... &quot;allowFullScreen&quot;, &quot;true&quot;, ... )Full-screen mode is initiated in response to a mouse click or key press by the user; the movie cannot change
        /// Stage.displayState without user input. While Flash Player is in full-screen mode, all keyboard input is disabled
        /// (except keyboard shortcuts that take the user out of full-screen mode). A Flash Player dialog box appears over the movie
        /// when users enter full-screen mode to inform the users they are in full-screen mode and that they can press the Escape key
        /// to end full-screen mode.These restrictions are not present for SWF content running in the stand-alone Flash Player or in the
        /// Apollo runtime.
        /// </summary>
        public extern virtual Avm.String displayState
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

        public extern virtual flash.geom.Rectangle fullScreenSourceRect
        {
            [PageFX.AbcInstanceTrait(28)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(29)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.Vector<flash.media.StageVideo> stageVideos
        {
            [PageFX.AbcInstanceTrait(32)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual uint fullScreenWidth
        {
            [PageFX.AbcInstanceTrait(33)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual uint fullScreenHeight
        {
            [PageFX.AbcInstanceTrait(35)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool wmodeGPU
        {
            [PageFX.AbcInstanceTrait(37)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual flash.geom.Rectangle softKeyboardRect
        {
            [PageFX.AbcInstanceTrait(38)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern override Avm.String name
        {
            [PageFX.AbcInstanceTrait(39)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override flash.display.DisplayObject mask
        {
            [PageFX.AbcInstanceTrait(40)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override bool visible
        {
            [PageFX.AbcInstanceTrait(41)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override double x
        {
            [PageFX.AbcInstanceTrait(42)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override double y
        {
            [PageFX.AbcInstanceTrait(43)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override double z
        {
            [PageFX.AbcInstanceTrait(44)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override double scaleX
        {
            [PageFX.AbcInstanceTrait(45)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override double scaleY
        {
            [PageFX.AbcInstanceTrait(46)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override double scaleZ
        {
            [PageFX.AbcInstanceTrait(47)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override double rotation
        {
            [PageFX.AbcInstanceTrait(48)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override double rotationX
        {
            [PageFX.AbcInstanceTrait(49)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override double rotationY
        {
            [PageFX.AbcInstanceTrait(50)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override double rotationZ
        {
            [PageFX.AbcInstanceTrait(51)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override double alpha
        {
            [PageFX.AbcInstanceTrait(52)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override bool cacheAsBitmap
        {
            [PageFX.AbcInstanceTrait(53)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override object opaqueBackground
        {
            [PageFX.AbcInstanceTrait(54)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override flash.geom.Rectangle scrollRect
        {
            [PageFX.AbcInstanceTrait(55)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override Avm.Array filters
        {
            [PageFX.AbcInstanceTrait(56)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override Avm.String blendMode
        {
            [PageFX.AbcInstanceTrait(57)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override flash.geom.Transform transform
        {
            [PageFX.AbcInstanceTrait(58)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override flash.accessibility.AccessibilityProperties accessibilityProperties
        {
            [PageFX.AbcInstanceTrait(59)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override flash.geom.Rectangle scale9Grid
        {
            [PageFX.AbcInstanceTrait(60)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override bool tabEnabled
        {
            [PageFX.AbcInstanceTrait(61)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override int tabIndex
        {
            [PageFX.AbcInstanceTrait(62)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override object focusRect
        {
            [PageFX.AbcInstanceTrait(63)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override bool mouseEnabled
        {
            [PageFX.AbcInstanceTrait(64)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override flash.accessibility.AccessibilityImplementation accessibilityImplementation
        {
            [PageFX.AbcInstanceTrait(65)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>Indicates the width of the display object, in pixels.</summary>
        public extern override double width
        {
            [PageFX.AbcInstanceTrait(73)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(74)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>Indicates the height of the display object, in pixels.</summary>
        public extern override double height
        {
            [PageFX.AbcInstanceTrait(75)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(76)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>Returns a TextSnapshot object for this DisplayObjectContainer instance.</summary>
        public extern override flash.text.TextSnapshot textSnapshot
        {
            [PageFX.AbcInstanceTrait(77)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
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
        public extern override bool mouseChildren
        {
            [PageFX.AbcInstanceTrait(78)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(79)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>Returns the number of children of this object.</summary>
        public extern override int numChildren
        {
            [PageFX.AbcInstanceTrait(80)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Determines whether the children of the object are tab enabled. Enables or disables tabbing for the
        /// children of the object. The default is true.
        /// </summary>
        public extern override bool tabChildren
        {
            [PageFX.AbcInstanceTrait(81)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(82)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool allowsFullScreen
        {
            [PageFX.AbcInstanceTrait(83)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual uint color
        {
            [PageFX.AbcInstanceTrait(87)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(88)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern override flash.ui.ContextMenu contextMenu
        {
            [PageFX.AbcInstanceTrait(89)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [PageFX.Event("stageVideoAvailability")]
        public event flash.events.StageVideoAvailabilityEventHandler stageVideoAvailability
        {
            add { }
            remove { }
        }

        [PageFX.Event("orientationChange")]
        public event flash.events.EventHandler orientationChange
        {
            add { }
            remove { }
        }

        [PageFX.Event("orientationChanging")]
        public event flash.events.EventHandler orientationChanging
        {
            add { }
            remove { }
        }

        [PageFX.Event("fullScreen")]
        public event flash.events.FullScreenEventHandler fullScreen
        {
            add { }
            remove { }
        }

        [PageFX.Event("resize")]
        public event flash.events.EventHandler resize
        {
            add { }
            remove { }
        }

        [PageFX.Event("mouseLeave")]
        public event flash.events.EventHandler mouseLeave
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Stage();

        /// <summary>
        /// Calling the invalidate() method signals Flash Player to alert display objects
        /// on the next opportunity it has to render the display list (for example, when the playhead
        /// advances to a new frame). After you call the invalidate() method, when the display
        /// list is next rendered, Flash Player sends a render event to each display object that has
        /// registered to listen for the render event. You must call the invalidate()
        /// method each time you want Flash Player to send render events.
        /// The render event gives you an opportunity to make changes to the display list
        /// immediately before it is actually rendered. This lets you defer updates to the display list until the
        /// latest opportunity. This can increase performance by eliminating unnecessary screen updates.The render event is dispatched only to display objects that are in the same
        /// security domain as the code that calls the stage.invalidate() method,
        /// or to display objects from a security domain that has been granted permission via the
        /// Security.allowDomain() method.
        /// </summary>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void invalidate();

        /// <summary>
        /// Determines whether the Stage.focus property returns null for
        /// security reasons.
        /// In other words, isFocusInaccessible returns true if the
        /// object that has focus belongs to a security sandbox to which the SWF file does not have access.
        /// </summary>
        /// <returns>
        /// true if the object that has focus belongs to a security sandbox to which
        /// the SWF file does not have access.
        /// </returns>
        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool isFocusInaccessible();

        [PageFX.AbcInstanceTrait(66)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.display.DisplayObject addChild(flash.display.DisplayObject child);

        [PageFX.AbcInstanceTrait(67)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.display.DisplayObject addChildAt(flash.display.DisplayObject child, int index);

        [PageFX.AbcInstanceTrait(68)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override void setChildIndex(flash.display.DisplayObject child, int index);

        [PageFX.AbcInstanceTrait(69)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override void addEventListener(Avm.String type, Avm.Function listener, bool useCapture, int priority, bool useWeakReference);

        [PageFX.AbcInstanceTrait(70)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override bool dispatchEvent(flash.events.Event @event);

        [PageFX.AbcInstanceTrait(71)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override bool hasEventListener(Avm.String type);

        [PageFX.AbcInstanceTrait(72)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override bool willTrigger(Avm.String type);

        [PageFX.AbcInstanceTrait(84)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override flash.display.DisplayObject removeChildAt(int index);

        [PageFX.AbcInstanceTrait(85)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern override void swapChildrenAt(int index1, int index2);


    }
}
