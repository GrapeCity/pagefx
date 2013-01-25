using System;
using System.Runtime.CompilerServices;

namespace flash.geom
{
    /// <summary>
    /// The Transform class collects data about color transformations and coordinate transformations that
    /// are applied to a display object. Apply transformations by creating a new Matrix
    /// and/or a new ColorTransform  and setting the appropriate properties of the transform  property
    /// of a display object.
    /// </summary>
    [PageFX.AbcInstance(111)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class Transform : Avm.Object
    {
        /// <summary>
        /// A Matrix object containing values that affect the scaling, rotation,
        /// and translation of the display object.
        /// </summary>
        public extern virtual flash.geom.Matrix matrix
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
        /// A ColorTransform object containing values that universally adjust the colors in
        /// the display object.
        /// </summary>
        public extern virtual flash.geom.ColorTransform colorTransform
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
        /// A Matrix object representing the combined transformation matrixes of the
        /// display object and all of its parent objects, back to the root level.
        /// If different transformation matrixes have been applied at different levels,
        /// all of those matrixes are concatenated into one matrix
        /// for this property.
        /// </summary>
        public extern virtual flash.geom.Matrix concatenatedMatrix
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// A ColorTransform object representing the combined color transformations applied to the display object
        /// and all of its parent objects, back to the root level.
        /// If different color transformations have been applied at different levels, all of those transformations are
        /// concatenated into one ColorTransform object
        /// for this property.
        /// </summary>
        public extern virtual flash.geom.ColorTransform concatenatedColorTransform
        {
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>A Rectangle object that defines the bounding rectangle of the display object on the Stage.</summary>
        public extern virtual flash.geom.Rectangle pixelBounds
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual flash.geom.Matrix3D matrix3D
        {
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(9)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual flash.geom.PerspectiveProjection perspectiveProjection
        {
            [PageFX.AbcInstanceTrait(11)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(12)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Transform(flash.display.DisplayObject displayObject);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual flash.geom.Matrix3D getRelativeMatrix3D(flash.display.DisplayObject relativeTo);


    }
}
