/*
	GDI+ matrix takes 6 elements arranged in 3 rows by 2 columns. The identity matrix is
	
	[1, 0]					[1, 0, 0]
	[0, 1] that is a simplification of 	[0, 1, 0] 
	[0, 0] 					[0, 0, 1]

	Point v1, v2	Matrix: [m11, m12]
				            [m21, m22]
				            [m31, m32]
	
	Calcutation of X, Y using the previous matrix

	X = v1 * m11 + v2 * m12 + m31
	Y = v1 * m21 + v2 * m22 + m32

	M31 and M32 are used to do matrix translations

*/

//TODO: Verify it again
//         GDI+         flash       cairo
//scaleX   m11          a           xx
//scaleY   m22          d           yy
//offsetX  m31          tx          x0
//offsetY  m32          ty          y0
//shareY   m21          b           xy
//shareX   m12          c           yx

namespace System.Drawing.Drawing2D
{
    using FlashMatrix = flash.geom.Matrix;

    public sealed class Matrix : MarshalByRefObject, IDisposable
    {
        internal FlashMatrix native;

        internal static readonly Matrix IdentityTransform = new Matrix();

       internal static bool IsEmpty(Matrix m)
        {
            if (m == null) return true;
            return m.IsIdentity;
        }

        #region ctors
        public Matrix(FlashMatrix m)
        {
            if (m == null)
                m = new FlashMatrix();
            native = m;
        }

        public Matrix()
        {
            native = new FlashMatrix();
        }

        public Matrix(float m11, float m12, float m21, float m22, float dx, float dy)
        {
            native = new FlashMatrix(m11, m12, m21, m22, dx, dy);
        }

#if NOT_PFX //TODO
        public Matrix(Rectangle rect, Point[] plgpts)
        {
            if (plgpts == null)
                throw new ArgumentNullException("plgpts");
            if (plgpts.Length != 3)
                throw new ArgumentException("plgpts");
            //Status status = GDIPlus.GdipCreateMatrix3I(ref rect, plgpts, out nativeMatrix);
            //GDIPlus.CheckStatus(status);
            throw new NotImplementedException();
        }

        public Matrix(RectangleF rect, PointF[] plgpts)
        {
            if (plgpts == null)
                throw new ArgumentNullException("plgpts");
            if (plgpts.Length != 3)
                throw new ArgumentException("plgpts");
            //Status status = GDIPlus.GdipCreateMatrix3(ref rect, plgpts, out nativeMatrix);
            //GDIPlus.CheckStatus(status);
            throw new NotImplementedException();
        }
#endif
        #endregion

        #region Properties
        internal double A
        {
            get { return native.a; }
            set { native.a = value; }
        }

        internal double B
        {
            get { return native.b; }
            set { native.b = value; }
        }

        internal double C
        {
            get { return native.c; }
            set { native.c = value; }
        }

        internal double D
        {
            get { return native.d; }
            set { native.d = value; }
        }

        internal double Determinant
        {
            get { return A * D - B * C; }
        }

        internal double TX
        {
            get { return native.tx; }
            set { native.tx = value; }
        }

        internal double TY
        {
            get { return native.tx; }
            set { native.tx = value; }
        }

        public float OffsetX
        {
            get { return (float)native.tx; }
        }

        public float OffsetY
        {
            get { return (float)native.ty; }
        }
        
        // properties
        public float[] Elements
        {
            get
            {
                return new[] 
                { 
                    (float)native.a,
                    (float)native.b,
                    (float)native.c,
                    (float)native.d,
                    (float)native.tx,
                    (float)native.ty
                };
            }
        }

        /*
* In System.Drawing it is often impossible to specify a 'null' matrix. 
* Instead we supply an empty matrix (i.e. new Matrix ()). However this
* "empty" matrix can cause a lot of extra calculation in libgdiplus
* (e.g. invalidating the bitmap) unless we consider it as a special case.
*/
        public bool IsIdentity
        {
            get
            {
                return FloatHelper.NearOne((float)native.a)
                    && FloatHelper.NearZero((float)native.b)
                    && FloatHelper.NearZero((float)native.c)
                    && FloatHelper.NearOne((float)native.d)
                    && FloatHelper.NearZero((float)native.tx)
                    && FloatHelper.NearZero((float)native.ty);
            }
        }

        public bool IsInvertible
        {
            get
            {
                var d = Determinant;
                return d != 0 && !double.IsInfinity(d);
            }
        }
        #endregion

        public FlashMatrix Native
        {
            get { return native.clone(); }
        }

        public Matrix Clone()
        {
            return new Matrix(native.clone());
        }

        public void Dispose()
        {
            native = null;
            GC.SuppressFinalize(this);
        }

        public void Invert()
        {
            native.invert();
        }

        #region Operations
        public void Multiply(Matrix matrix)
        {
            Multiply(matrix, MatrixOrder.Prepend);
        }

        public void Multiply(Matrix matrix, MatrixOrder order)
        {
            if (matrix == null)
                throw new ArgumentNullException("matrix");
            if (order == MatrixOrder.Append)
            {
                native.concat(matrix.native);
            }
            else
            {
                var m = matrix.native.clone();
                m.concat(native);
                native = m;
            }
        }

        public void Reset()
        {
            native.identity();
        }

        public void Rotate(float angle)
        {
            Rotate(angle, MatrixOrder.Prepend);
        }

        public void Rotate(float angle, MatrixOrder order)
        {
            double rad = angle * Const.DEGTORAD;
            float cos = (float)Math.Cos(rad);
            float sin = (float)Math.Sin(rad);
            var m = new Matrix(cos, sin, -sin, cos, 0, 0);
            Multiply(m, order);

            //if (order == MatrixOrder.Append)
            //{
            //    _mat.rotate(angle);
            //}
            //else
            //{
            //    var m = new FlashMatrix();
            //    m.rotate(angle);
            //    m.concat(_mat);
            //    _mat = m;
            //}
        }

        public void RotateAt(float angle, PointF point)
        {
            RotateAt(angle, point, MatrixOrder.Prepend);
        }

        public void RotateAt(float angle, PointF point, MatrixOrder order)
        {
            if ((order < MatrixOrder.Prepend) || (order > MatrixOrder.Append))
                throw new ArgumentException("order");

            angle *= (float)(Math.PI / 180.0); // degrees to radians
            float cos = (float)Math.Cos(angle);
            float sin = (float)Math.Sin(angle);
            float e4 = -point.X * cos + point.Y * sin + point.X;
            float e5 = -point.X * sin - point.Y * cos + point.Y;
            float[] m = Elements;

            //Status status;
            if (order == MatrixOrder.Prepend)
            {
                native = new FlashMatrix(
                    cos * m[0] + sin * m[2],
                    cos * m[1] + sin * m[3],
                    -sin * m[0] + cos * m[2],
                    -sin * m[1] + cos * m[3],
                    e4 * m[0] + e5 * m[2] + m[4],
                    e4 * m[1] + e5 * m[3] + m[5]);
            }
            else
            {
                native = new FlashMatrix(
                    m[0] * cos + m[1] * -sin,
                    m[0] * sin + m[1] * cos,
                    m[2] * cos + m[3] * -sin,
                    m[2] * sin + m[3] * cos,
                    m[4] * cos + m[5] * -sin + e4,
                    m[4] * sin + m[5] * cos + e5);
            }
        }

        public void Scale(float scaleX, float scaleY)
        {
            Scale(scaleX, scaleY, MatrixOrder.Prepend);
        }

        public void Scale(float scaleX, float scaleY, MatrixOrder order)
        {
            var m = new Matrix(scaleX, 0, 0, scaleY, 0, 0);
            Multiply(m, order);
        }

        public void Shear(float shearX, float shearY)
        {
            Shear(shearX, shearY, MatrixOrder.Prepend);
        }

        public void Shear(float shearX, float shearY, MatrixOrder order)
        {
            var m = new Matrix(1, shearY, shearX, 1, 0, 0);
            Multiply(m, order);
        }

        public void Translate(float offsetX, float offsetY)
        {
            Translate(offsetX, offsetY, MatrixOrder.Prepend);
        }

        public void Translate(float offsetX, float offsetY, MatrixOrder order)
        {
            var m = new Matrix(1, 0, 0, 1, offsetX, offsetY);
            Multiply(m, order);

            //if (order == MatrixOrder.Prepend)
            //{
            //    _mat.translate(offsetX, offsetY);
            //}
            //else
            //{
            //    var m = new FlashMatrix();
            //    m.translate(offsetX, offsetY);
            //    m.concat(_mat);
            //    _mat = m;
            //}
        }
        #endregion

        #region TransformPoint, TransformPoints
        internal PointF TransformPoint(double x, double y)
        {
            var pt = new flash.geom.Point(x, y);
            pt = native.transformPoint(pt);
            return new PointF((float)pt.x, (float)pt.y);
        }

        internal PointF TransformPoint(PointF pt)
        {
            return TransformPoint(pt.X, pt.Y);
        }

        public void TransformPoints(PointF[] pts)
        {
            if (pts == null)
                throw new ArgumentNullException("pts");
            int n = pts.Length;
            if (n == 0)
                throw new ArgumentNullException("pts");
            for (int i = 0; i < n; i++)
            {
                var pt = pts[i];
                pts[i] = TransformPoint(pt.X, pt.Y);
            }
        }

        public void TransformPoints(Point[] pts)
        {
            if (pts == null)
                throw new ArgumentNullException("pts");
            int n = pts.Length;
            if (n == 0)
                throw new ArgumentNullException("pts");
            for (int i = 0; i < n; i++)
            {
                var pt = pts[i];
                var pt2 = TransformPoint(pt.X, pt.Y);
                pts[i] = new Point((int)pt2.X, (int)pt2.Y);
            }
        }

        public void TransformVectors(Point[] pts)
        {
            TransformPoints(pts);
        }

        public void TransformVectors(PointF[] pts)
        {
            TransformPoints(pts);
        }
        #endregion

        public void VectorTransformPoints(Point[] pts)
        {
            TransformVectors(pts);
        }

        #region Object Override Methods
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var m = obj as Matrix;
            if (m == null) return false;
            var m1 = native;
            var m2 = m.native;
            return m1.a == m2.a
                   && m1.b == m2.b
                   && m1.c == m2.c
                   && m1.d == m2.d
                   && m1.tx == m2.tx
                   && m1.ty == m2.ty;
        }

        public override int GetHashCode()
        {
            return native.a.GetHashCode()
                   ^ native.b.GetHashCode()
                   ^ native.c.GetHashCode()
                   ^ native.d.GetHashCode()
                   ^ native.tx.GetHashCode()
                   ^ native.ty.GetHashCode();
        }
        #endregion
    }
}
