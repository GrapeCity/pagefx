using System;
using System.Linq;
using DataDynamics.PageFX.Common.Extensions;

namespace DataDynamics.PageFX.Common.TypeSystem
{
    public sealed class ArrayType : CompoundType
    {
		private readonly ArrayDimensionCollection _arrayDimensions;
		private MethodCollection _ctors;
		
        public ArrayType(IType elementType) : base(elementType)
        {
			_arrayDimensions = ArrayDimensionCollection.Single;
        }

        public ArrayType(IType elementType, ArrayDimensionCollection dim) : base(elementType)
        {
			_arrayDimensions = dim;
        }

        public override TypeKind TypeKind
        {
            get { return TypeKind.Array; }
        }

        public override string NameSuffix
        {
            get { return ArrayDimensions.ToString(); }
        }

        protected override string SigSuffix
        {
            get { return ArrayDimensionCollection.Format(ArrayDimensions, true); }
        }

        public override IType BaseType
        {
            get { return ElementType.SystemType(SystemTypeCode.Array); }
        }

	    public override ITypeCollection Interfaces
        {
            get { return BaseType.IfNotNull(x => x.Interfaces); }
        }

        public override IEventCollection Events
        {
            get { return BaseType.IfNotNull(x => x.Events); }
        }

        public override IFieldCollection Fields
        {
            get { return BaseType.IfNotNull(x => x.Fields); }
        }

        public override IPropertyCollection Properties
        {
            get { return BaseType.IfNotNull(x => x.Properties); }
        }

        public override IMethodCollection Methods
        {
            get { return BaseType.IfNotNull(x => x.Methods); }
        }

        public override ITypeMemberCollection Members
        {
            get { return BaseType.IfNotNull(x => x.Members); }
        }

        public override ITypeCollection Types
        {
            get { return BaseType.IfNotNull(x => x.Types); }
        }

        public override ClassLayout Layout
        {
            get { return BaseType.IfNotNull(x => x.Layout); }
        }

	    public override ArrayDimensionCollection ArrayDimensions
	    {
		    get { return _arrayDimensions; }
	    }

	    public IMethodCollection Constructors
        {
            get { return _ctors ?? (_ctors = new MethodCollection()); }
        }
        
	    public IMethod FindConstructor(IType[] types)
        {
            return Constructors.FirstOrDefault(ctor => Signature.Equals(ctor.Parameters, types));
        }

        public IMethod Getter { get; set; }

        public IMethod Setter { get; set; }

        public IMethod Address { get; set; }

	    #region Utils

		private static IType ResolveSystemType(IType arrayType, SystemTypeCode typeCode)
		{
			return arrayType.SystemType(typeCode);
		}

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
			        Type = ResolveSystemType(arrayType, SystemTypeCode.Void),
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

        private static IMethod ResolveAddress(IType arrayType, IMethod method)
        {
            var arrType = (ArrayType)arrayType;
            if (arrType.Address != null)
                return arrType.Address;

            var m = new Method
                        {
                            Name = CLRNames.Array.Address,
                            IsInternalCall = true,
                            DeclaringType = arrayType,
                            Type = arrayType.Assembly.TypeFactory.MakeReferenceType(arrType.ElementType),
                        };

            CopyParams(method, m);

            arrType.Address = m;

            return m;
        }

        private static IMethod ResolveSetter(IType arrayType, IMethod method)
        {
            var arrType = (ArrayType)arrayType;
            if (arrType.Setter != null)
                return arrType.Setter;

            var m = new Method
                        {
                            Name = CLRNames.Array.Setter,
                            IsInternalCall = true,
                            DeclaringType = arrayType,
                            Type = ResolveSystemType(arrayType, SystemTypeCode.Void),
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
}