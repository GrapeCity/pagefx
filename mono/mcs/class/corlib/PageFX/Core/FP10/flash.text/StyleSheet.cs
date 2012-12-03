using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    /// <summary>
    /// The StyleSheet class lets you create a StyleSheet object that contains text
    /// formatting rules for font size, color, and other styles. You can then apply
    /// styles defined by a style sheet to a TextField object that contains HTML- or
    /// XML-formatted text. The text in the TextField object is automatically
    /// formatted according to the tag styles defined by the StyleSheet object.
    /// You can use text styles to define new formatting tags, redefine built-in HTML
    /// tags, or create style classes that you can apply to certain HTML tags.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class StyleSheet : flash.events.EventDispatcher
    {
        public extern virtual Avm.Array styleNames
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern StyleSheet();

        /// <summary>
        /// Extends the CSS parsing capability. Advanced developers can override this method by extending the
        /// StyleSheet class.
        /// </summary>
        /// <param name="arg0">
        /// An object that describes the style, containing style rules as properties of the object,
        /// or null.
        /// </param>
        /// <returns>
        /// A TextFormat object containing the result of the mapping of CSS rules
        /// to text format properties.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual TextFormat transform(object arg0);

        /// <summary>Removes all styles from the style sheet object.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void clear();

        /// <summary>
        /// Adds a new style with the specified name to the style sheet object.
        /// If the named style does not already exist in the style sheet, it is added.
        /// If the named style already exists in the style sheet, it is replaced.
        /// If the styleObject parameter is null, the named style is removed.
        /// Flash Player creates a copy of the style object that you pass to this method.For a list of supported styles, see the table in the description for the StyleSheet class.
        /// </summary>
        /// <param name="arg0">A string that specifies the name of the style to add to the style sheet.</param>
        /// <param name="arg1">An object that describes the style, or null.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setStyle(Avm.String arg0, object arg1);

        /// <summary>
        /// Parses the CSS in CSSText and loads the style sheet with it. If a style in
        /// CSSText is already in styleSheet, the properties in
        /// styleSheet are retained, and only the ones in CSSText
        /// are added or changed in styleSheet.
        /// To extend the native CSS parsing capability, you can override this method by creating a subclass
        /// of the StyleSheet class.
        /// </summary>
        /// <param name="arg0">The CSS text to parse (a string).</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void parseCSS(Avm.String arg0);

        /// <summary>
        /// Returns a copy of the style object associated with the style named styleName.
        /// If there is no style object associated with styleName,
        /// null is returned.
        /// </summary>
        /// <param name="arg0">A string that specifies the name of the style to retrieve.</param>
        /// <returns>An object.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object getStyle(Avm.String arg0);
    }
}
