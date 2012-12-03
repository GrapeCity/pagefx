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
    [PageFX.ABC]
    [PageFX.FP9]
    public class Transform : Avm.Object
    {
        public extern virtual Matrix matrix
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

        public extern virtual Matrix3D matrix3D
        {
            [PageFX.ABC]
            [PageFX.QName("matrix3D", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.QName("matrix3D", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual ColorTransform colorTransform
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

        public extern virtual PerspectiveProjection perspectiveProjection
        {
            [PageFX.ABC]
            [PageFX.QName("perspectiveProjection", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.QName("perspectiveProjection", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Matrix concatenatedMatrix
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual ColorTransform concatenatedColorTransform
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Rectangle pixelBounds
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Transform(flash.display.DisplayObject arg0);

        [PageFX.ABC]
        [PageFX.QName("getRelativeMatrix3D", "http://www.adobe.com/2008/actionscript/Flash10/", "public")]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Matrix3D getRelativeMatrix3D(flash.display.DisplayObject arg0);


    }
}
