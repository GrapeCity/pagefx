using System;

namespace DataDynamics.PageFX.CodeModel
{
    public class ParameterizedMemberCollection<T> : MultiMemberCollection<T>
        where T: ITypeMember, IParameterizedMember
    {
        public ParameterizedMemberCollection(IType owner) : base(owner)
        {
        }

        public T this[string name, int argc]
        {
            get
            {
                return base[name, m => m.Parameters.Count == argc];
            }
        }

        public T this[string name, IType arg1]
        {
            get
            {
                return base[name,
                            m =>
                                {
                                    var p = m.Parameters;
                                    if (p.Count != 1) return false;
                                    return Signature.TypeEquals(p[0].Type, arg1);
                                }];
            }
        }

        public T this[string name, IType arg1, IType arg2]
        {
            get
            {
                return base[name,
                            m =>
                                {
                                    var p = m.Parameters;
                                    if (p.Count != 2) return false;
                                    return Signature.TypeEquals(p[0].Type, arg1)
										&& Signature.TypeEquals(p[1].Type, arg2);
                                }];
            }
        }

        public T this[string name, IType arg1, IType arg2, IType arg3]
        {
            get
            {
            	return base[name,
            	            m =>
            	            	{
            	            		var p = m.Parameters;
            	            		if (p.Count != 3) return false;
            	            		return Signature.TypeEquals(p[0].Type, arg1)
            	            		       && (Signature.TypeEquals(p[1].Type, arg2)
										   && Signature.TypeEquals(p[2].Type, arg3));
            	            	}];
            }
        }

        public T this[string name, params IType[] types]
        {
            get
            {
                return base[name, m => Signature.Equals(m.Parameters, types)];
            }
        }

        public T this[string name, Predicate<IParameterCollection> predicate]
        {
            get
            {
                return base[name, m => predicate(m.Parameters)];
            }
        }

        public T this[string name, Predicate<IType> arg1]
        {
            get
            {
                return base[name,
                            m =>
                                {
                                    var p = m.Parameters;
                                    if (p.Count != 1) return false;
                                    return arg1(p[0].Type);
                                }];
            }
        }

        public T this[string name, Predicate<IType> arg1, Predicate<IType> arg2]
        {
            get
            {
                return base[name,
                            m =>
                                {
                                    var p = m.Parameters;
                                    if (p.Count != 2) return false;
                                    return arg1(p[0].Type) && arg2(p[1].Type);
                                }];
            }
        }

        public T this[string name, Predicate<IType> arg1, Predicate<IType> arg2, Predicate<IType> arg3]
        {
            get
            {
                return base[name,
                            m =>
                                {
                                    var p = m.Parameters;
                                    if (p.Count != 2) return false;
                                    return arg1(p[0].Type) && arg2(p[1].Type) && arg3(p[2].Type);
                                }];
            }
        }
    }
}