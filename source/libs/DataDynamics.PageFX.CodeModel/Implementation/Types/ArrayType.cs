using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class ArrayType : CompoundType, IArrayType
    {
        public ArrayType(IType elementType) : base(elementType)
        {
            _dim = ArrayDimensionCollection.Single;
        }

        public ArrayType(IType elementType, IArrayDimensionCollection dim)
            : base(elementType)
        {
            _dim = dim;
        }

        public override TypeKind TypeKind
        {
            get { return TypeKind.Array; }
        }

        public override string NameSuffix
        {
            get { return _dim.ToString(); }
        }

        protected override string SigSuffix
        {
            get { return ArrayDimensionCollection.Format(_dim, true); }
        }

        public override IType BaseType
        {
            get
            {
                return SystemTypes.Array;
            }
            set
            {
                throw new NotSupportedException();
            }
        }

        IType ImplType
        {
            get { return SystemTypes.Array; }
        }

        public override ITypeCollection Interfaces
        {
            get
            {
                var impl = ImplType;
                if (impl != null)
                    return impl.Interfaces;
                return null;
            }
        }

        public override IEventCollection Events
        {
            get
            {
                var impl = ImplType;
                if (impl != null)
                    return impl.Events;
                return null;
            }
        }

        public override IFieldCollection Fields
        {
            get
            {
                var impl = ImplType;
                if (impl != null)
                    return impl.Fields;
                return null;
            }
        }

        public override IPropertyCollection Properties
        {
            get
            {
                var impl = ImplType;
                if (impl != null)
                    return impl.Properties;
                return null;
            }
        }

        public override IMethodCollection Methods
        {
            get
            {
                var impl = ImplType;
                if (impl != null)
                    return impl.Methods;
                return null;
            }
        }

        public override ITypeMemberCollection Members
        {
            get
            {
                var impl = ImplType;
                if (impl != null)
                    return impl.Members;
                return null;
            }
        }

        public override ITypeCollection Types
        {
            get
            {
                var impl = ImplType;
                if (impl != null)
                    return impl.Types;
                return null;
            }
        }

        public override ClassLayout Layout
        {
            get
            {
                var impl = ImplType;
                if (impl != null)
                    return impl.Layout;
                return null;
            }
            set
            {
                base.Layout = value;
            }
        }

        #region IArrayType Members
        public int Rank
        {
            get { return _dim.Count + 1; }
        }

        public IArrayDimensionCollection Dimensions
        {
            get { return _dim; }
        }
        readonly IArrayDimensionCollection _dim;
        #endregion

        #region Runtime Methods

        public IMethodCollection Constructors
        {
            get { return _ctors ?? (_ctors = new MethodCollection(this)); }
        }
        private MethodCollection _ctors;

        public IMethod FindConstructor(IType[] types)
        {
            return Constructors.FirstOrDefault(ctor => Signature.Equals(ctor.Parameters, types));
        }

        public IMethod Getter { get; set; }

        public IMethod Setter { get; set; }

        public IMethod Address { get; set; }

        #endregion

        #region Utils
        public static IMethod ResolveMethod(IType arrayType, IMethod method)
        {
            if (method.IsConstructor)
                return ResolveConstructor(arrayType, method);
            return ResolveAccessor(arrayType, method);
        }

        public static IMethod ResolveConstructor(IType arrayType, IMethod method)
        {
            if (arrayType == null)
                throw new ArgumentNullException("arrayType");
            if (method == null)
                throw new ArgumentNullException("method");
            if (!arrayType.IsArray)
                throw new ArgumentException(string.Format("given type '{0}' is not array", arrayType.FullName));
            if (!method.IsConstructor)
                throw new ArgumentException("given method is not constructor");
            
            var arrType = (ArrayType)arrayType;
            var c2 = arrType.Constructors.FirstOrDefault(c => Signature.Equals(c.Parameters, method.Parameters));
            if (c2 != null) return c2;

        	var m = new Method
        	        	{
        	        		Name = CLRNames.Constructor,
        	        		Type = SystemTypes.Void,
        	        		DeclaringType = arrType
        	        	};

            CopyParams(method, m);

            m.IsSpecialName = true;
            m.IsInternalCall = true;
            arrType.Constructors.Add(m);

            return m;
        }

        static void CopyParams(IMethod from, IMethod to)
        {
            foreach (var p in from.Parameters)
            {
                to.Parameters.Add(new Parameter(p.Type, p.Name, p.Index));
            }
        }

        public static IMethod ResolveAccessor(IType arrayType, IMethod method)
        {
            if (arrayType == null)
                throw new ArgumentNullException("arrayType");
            if (method == null)
                throw new ArgumentNullException("method");
            if (!arrayType.IsArray)
                throw new ArgumentException(string.Format("given type '{0}' is not array", arrayType.FullName));

            switch (method.Name)
            {
                case CLRNames.Array.Getter:
                    return ResolveGetter(arrayType, method);

                case CLRNames.Array.Setter:
                    return ResolveSetter(arrayType, method);

                case CLRNames.Array.Address:
                    return ResolveAddress(arrayType, method);

                default:
                    return null;
            }
        }

        static IMethod ResolveGetter(IType arrayType, IMethod method)
        {
            var arrType = (ArrayType)arrayType;
            if (arrType.Getter != null)
                return arrType.Getter;

            var m = new Method
                        {
                            Name = CLRNames.Array.Getter,
                            IsInternalCall = true,
                            DeclaringType = arrayType,
                            Type = arrType.ElementType,
                        };

            CopyParams(method, m);

            arrType.Getter = m;

            return m;
        }

        static IMethod ResolveAddress(IType arrayType, IMethod method)
        {
            var arrType = (ArrayType)arrayType;
            if (arrType.Address != null)
                return arrType.Address;

            var m = new Method
                        {
                            Name = CLRNames.Array.Address,
                            IsInternalCall = true,
                            DeclaringType = arrayType,
                            Type = TypeFactory.MakeReferenceType(arrType.ElementType),
                        };

            CopyParams(method, m);

            arrType.Address = m;

            return m;
        }

        static IMethod ResolveSetter(IType arrayType, IMethod method)
        {
            var arrType = (ArrayType)arrayType;
            if (arrType.Setter != null)
                return arrType.Setter;

            var m = new Method
                        {
                            Name = CLRNames.Array.Setter,
                            IsInternalCall = true,
                            DeclaringType = arrayType,
                            Type = SystemTypes.Void,
                        };

            int n = method.Parameters.Count;
            for (int i = 0; i < n - 1; ++i)
            {
                var p = method.Parameters[i];
                m.Parameters.Add(new Parameter(p.Type, p.Name, p.Index));
            }
            m.Parameters.Add(new Parameter(arrType.ElementType, "value", n));
            
            arrType.Setter = m;

            return m;
        }
        #endregion
    }

    #region class ArrayDimension
    public sealed class ArrayDimension : IArrayDimension
    {
        int _l = -1;
        int _u = -1;

        #region IArrayDimension Members
        public int LowerBound
        {
            get { return _l; }
            set { _l = value; }
        }

        public int UpperBound
        {
            get { return _u; }
            set { _u = value; }
        }
        #endregion

        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var d = obj as IArrayDimension;
            if (d == null) return false;
            if (d.LowerBound != _l) return false;
            if (d.UpperBound != _u) return false;
            return true;
        }

        public override int GetHashCode()
        {
            return _l ^ _u;
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(string format, IFormatProvider formatProvider)
        {
            return ToString(false);
        }

        public string ToString(bool sig)
        {
            return Format(_l, _u, sig);
        }

        public static string Format(int l, int u, bool sig)
        {
            if (l < 0)
            {
                if (u < 0) return "";
                return u.ToString();
            }
            if (u < 0)
            {
                if (l == 0) return "";
                if (sig) return l + "___";
                return l + "...";
            }
            if (sig) return l + "___" + u;
            return l + "..." + u;
        }
    }
    #endregion

    #region class ArrayDimensionCollection
    public sealed class ArrayDimensionCollection : List<IArrayDimension>, IArrayDimensionCollection
    {
        public static readonly ArrayDimensionCollection Single = new ArrayDimensionCollection();

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            var s = new StringBuilder();
            s.Append("[");
            int n = Count;
            for (int i = 0; i < n; ++i)
            {
                if (i > 0) s.Append(",");
                s.Append(this[i].ToString());
            }
            s.Append("]");
            return s.ToString();
        }
        #endregion

        public override bool Equals(object obj)
        {
            if (obj == this) return false;
            var c = obj as IArrayDimensionCollection;
            if (c == null) return false;
            return Object2.Equals(this, c);
        }

        public override int GetHashCode()
        {
            return Object2.GetHashCode(this);
        }

        public override string ToString()
        {
            return ToString(false);
        }

        public string ToString(bool sig)
        {
            return Format(this, sig);
        }

        static bool AllDefaults(IArrayDimensionCollection dims)
        {
            int n = dims.Count;
            for (int i = 0; i < n; ++i)
            {
                var d = dims[i];
                if (!(d.LowerBound <= 0 && d.UpperBound < 0))
                    return false;
            }
            return true;
        }

        public static string Format(IArrayDimensionCollection dims, bool sig)
        {
            var sb = new StringBuilder();
            int n = dims.Count;
            if (sig)
            {
                if (n == 0) return "_array";
                sb.Append("_" + n + "D_array");
                if (!AllDefaults(dims))
                {
                    for (int i = 0; i < n; ++i)
                    {
                        if (i > 0) sb.Append('_');
                        var d = dims[i];
                        sb.Append(ArrayDimension.Format(d.LowerBound, d.UpperBound, true));
                    }
                }
            }
            else
            {
                sb.Append('[');
                for (int i = 0; i < n; ++i)
                {
                    if (i > 0) sb.Append(',');
                    var d = dims[i];
                    sb.Append(ArrayDimension.Format(d.LowerBound, d.UpperBound, sig));
                }
                sb.Append(']');
            }
            return sb.ToString();
        }
    }
    #endregion
}