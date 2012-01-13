namespace DataDynamics.PageFX.FLI.SWF
{
    public enum SwfCapStyle
    {
        Round,
        No,
        Sqare,
    }

    public enum SwfJoinStyle
    {
        Round,
        Bevel,
        Miter
    }

    public enum SwfLineFlags
    {
        /// <summary>
        /// If 1, stroke thickness will not scale if the object is scaled horizontally.
        /// </summary>
        NoHScale = 1,

        /// <summary>
        /// If 1, stroke thickness will not scale if the object is scaled vertically.
        /// </summary>
        NoVScale = 2,

        /// <summary>
        /// If 1, all anchors will be aligned to full pixels.
        /// </summary>
        PixelHinting = 4,

        /// <summary>
        /// If 1, stroke will not be closed if the stroke's last point matches its first point.
        /// Flash Player will apply caps instead of a join.
        /// </summary>
        NoClose = 8,
    }
}