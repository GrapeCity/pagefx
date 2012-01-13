using System;

namespace DataDynamics
{
    #region enum PathPainting
    /// <summary>
    /// Defines available operations of path painting.
    /// </summary>
    [Flags]
    public enum PathPainting
    {
        /// <summary>
        /// End the path object without filling or stroking it.
        /// </summary>
        None,

        /// <summary>
        /// Fill the path.
        /// </summary>
        Fill = 1,

        /// <summary>
        /// Stroke the path.
        /// </summary>
        Stroke = 2,

        /// <summary>
        /// Use the even-odd rule to determine the region to fill.
        /// </summary>
        EOR = 4,

        /// <summary>
        /// Close the path.
        /// </summary>
        Close = 8,

        /// <summary>
        /// Combination of Close and Stroke.
        /// </summary>
        CloseStroke = Close | Stroke,

        /// <summary>
        /// Combination of Fill and Stroke.
        /// </summary>
        FillStroke = Fill | Stroke,

        /// <summary>
        /// Combination of Close, Fill and Stroke.
        /// </summary>
        CloseFillStroke = Close | FillStroke,

        /// <summary>
        /// Combination of Fill, Stroke and EOR.
        /// </summary>
        FillStrokeEOR = FillStroke | EOR,

        /// <summary>
        /// Combination of Close, Fill, Stroke and EOR.
        /// </summary>
        CloseFillStrokeEOR = Close | FillStrokeEOR,
    }
    #endregion
}