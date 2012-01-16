using System;
using System.Runtime.CompilerServices;

namespace flash.accessibility
{
    /// <summary>
    /// The AccessibilityProperties class lets you control the presentation of Flash objects to accessibility
    /// aids, such as screen readers.
    /// </summary>
    [PageFX.AbcInstance(112)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class AccessibilityProperties : Avm.Object
    {
        /// <summary>
        /// Provides a name for this display object in the accessible presentation.
        /// Applies to whole SWF files, containers, buttons, and text.  Do not confuse with
        /// DisplayObject.name, which is unrelated.
        /// </summary>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String name;

        /// <summary>
        /// Provides a description for this display object in the accessible presentation.
        /// If you have a lot of information to present about the object, it is
        /// best to choose a concise name and put most of your content in the
        /// description property.
        /// Applies to whole SWF files, containers, buttons, and text.
        /// </summary>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String description;

        /// <summary>
        /// Indicates a keyboard shortcut associated with this display object.
        /// Supply this string only for UI controls that you have associated with a shortcut key.
        /// Applies to containers, buttons, and text.
        /// Note: Assigning this property does not automatically assign the specified key
        /// combination to this object; you must do that yourself, for example, by
        /// listening for a KeyboardEvent.The syntax for this string uses long names for modifier keys, and
        /// the plus(+) character to indicate key combination. Examples of valid strings are
        /// &quot;Ctrl+F&quot;, &quot;Ctrl+Shift+Z&quot;, and so on.
        /// </summary>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public Avm.String shortcut;

        /// <summary>
        /// If true, excludes this display object from accessible presentation.
        /// The default is false. Applies to whole SWF files, containers, buttons, and text.
        /// </summary>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public bool silent;

        /// <summary>
        /// If true, causes Flash® Player to exclude child objects within this
        /// display object from the accessible presentation.
        /// The default is false. Applies to whole SWF files and containers.
        /// </summary>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        public bool forceSimple;

        /// <summary>
        /// If true, disables the Flash Player default auto-labeling system.
        /// Auto-labeling causes text objects inside buttons to be treated as button names,
        /// and text objects near text fields to be treated as text field names.
        /// The default is false. Applies only to whole SWF files.
        /// </summary>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.FP9]
        public bool noAutoLabeling;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern AccessibilityProperties();
    }
}
