using System;
using System.Runtime.CompilerServices;

namespace flash.text
{
    /// <summary>
    /// The TextLineMetrics class contains information about the text position and measurements of a
    /// line of text  within a text field. All measurements are in pixels. Objects of this class are returned by the
    /// flash.text.TextField.getLineMetrics()  method.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class TextLineMetrics : Avm.Object
    {
        /// <summary>
        /// The ascent value of the text is the length from the baseline to the top of the line height in pixels. See the
        /// &quot;Ascent&quot; measurement in the overview diagram for this class.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public double ascent;

        /// <summary>
        /// The width value is the width of the text of the selected lines (not necessarily the complete text) in pixels. The width of the
        /// text line is not the same as the width of the text field. The width of the text line is relative to the
        /// text field width, minus the gutter width of 4 pixels (2 pixels on each side). See the &quot;Text Line width&quot;
        /// measurement in the overview diagram for this class.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public double width;

        /// <summary>
        /// The height value of the text of the selected lines (not necessarily the complete text) in pixels. The height of the
        /// text line does not include the gutter height. See the &quot;Line height&quot; measurement in the overview diagram
        /// for this class.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public double height;

        /// <summary>
        /// The leading value is the measurement of the vertical distance between the lines of text.
        /// See the &quot;Leading&quot; measurement in the overview diagram for this class.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public double leading;

        /// <summary>
        /// The x value is the left position of the first character in pixels. This value includes the margin,
        /// indent (if any), and gutter widths. See the &quot;Text Line x-position&quot; in the overview diagram for this class.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public double x;

        /// <summary>
        /// The descent value of the text is the length from the baseline to the bottom depth of the line in pixels.
        /// See the &quot;Descent&quot; measurement in the overview diagram for this class.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public double descent;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern TextLineMetrics(double arg0, double arg1, double arg2, double arg3, double arg4, double arg5);
    }
}
