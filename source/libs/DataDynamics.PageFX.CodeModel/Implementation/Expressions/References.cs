using System;
using System.Collections.Generic;

namespace DataDynamics.PageFX.CodeModel
{
    public sealed class ThisReferenceExpression : Expression, IThisReferenceExpression, ITypeReferenceProvider
    {
        #region Constructors
        public ThisReferenceExpression(IType type)
        {
            _type = type;
        }
        #endregion

        #region IThisReferenceExpression Members
        public IType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        private IType _type;
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get { return _type; }
        }
        #endregion

        #region ITypeReferenceProvider Members
        public IEnumerable<IType> GetTypeReferences()
        {
            return new[] { _type };
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IThisReferenceExpression;
            if (e == null) return false;
            if (e.Type != _type) return false;
            return true;
        }

        private static readonly int _hs = typeof(IThisReferenceExpression).GetHashCode();

        public override int GetHashCode()
        {
            if (_type != null)
                return _type.GetHashCode() ^ _hs;
            return base.GetHashCode();
        }
        #endregion
    }

    public sealed class BaseReferenceExpression : Expression, IBaseReferenceExpression, ITypeReferenceProvider
    {
        #region Constructors
        public BaseReferenceExpression(IType type)
        {
            _type = type;
        }
        #endregion

        #region IBaseReferenceExpression Members
        public IType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        private IType _type;
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get { return _type; }
        }
        #endregion

        #region ITypeReferenceProvider Members
        public IEnumerable<IType> GetTypeReferences()
        {
            return new[] { _type };
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IBaseReferenceExpression;
            if (e == null) return false;
            if (e.Type != _type) return false;
            return true;
        }

        private static readonly int _hs = typeof(IBaseReferenceExpression).GetHashCode();

        public override int GetHashCode()
        {
            if (_type != null)
                return _type.GetHashCode() ^ _hs;
            return base.GetHashCode();
        }
        #endregion
    }

    public sealed class ArgumentReferenceExpression : Expression, IArgumentReferenceExpression, ITypeReferenceProvider
    {
        #region Constructors
        public ArgumentReferenceExpression(IParameter p)
        {
            _arg = p;
        }
        #endregion

        #region IArgumentReferenceExpression Members
        public IParameter Argument
        {
            get { return _arg; }
            set { _arg = value; }
        }
        private IParameter _arg;
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get
            {
                var type = _arg.Type;
                return type.UnwrapRef();
            }
        }
        #endregion

        #region ITypeReferenceProvider Members
        public IEnumerable<IType> GetTypeReferences()
        {
            return new[] { _arg.Type };
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IArgumentReferenceExpression;
            if (e == null) return false;
            if (e.Argument != _arg) return false;
            return true;
        }

        private static readonly int _hs = typeof(IArgumentReferenceExpression).GetHashCode();

        public override int GetHashCode()
        {
            if (_arg != null)
                return _arg.GetHashCode() ^ _hs;
            return base.GetHashCode();
        }
        #endregion
    }

    public abstract class MemberReferenceExpression : Expression, IMemberReferenceExpression, ITypeReferenceProvider
    {
        #region Constructors
        public MemberReferenceExpression()
        {
        }

        public MemberReferenceExpression(IExpression target)
        {
            _target = target;
        }
        #endregion

        #region IMemberReferenceExpression Members
        public IExpression Target
        {
            get { return _target; }
            set { _target = value; }
        }
        private IExpression _target;

        public abstract ITypeMember Member
        {
            get;
            set;
        }
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get
            {
                var m = Member;
                if (m != null) return m.Type;
                return null;
            }
        }
        #endregion

        #region ICodeNode Members
        public override IEnumerable<ICodeNode> ChildNodes
        {
            get { return new ICodeNode[] {_target}; }
        }
        #endregion

        #region ITypeReferenceProvider Members
        public virtual IEnumerable<IType> GetTypeReferences()
        {
            if (Member != null)
                return new[] { Member.Type };
            return null;
        }
        #endregion

        #region Object Override Methods
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IMemberReferenceExpression;
            if (e == null) return false;
            if (e.Member != Member) return false;
            if (!Equals(_target, e.Target)) return false;
            return true;
        }

        public override int GetHashCode()
        {
            int h = 0;
            if (_target != null)
                h ^= _target.GetHashCode();
            var m = Member;
            if (m != null)
                h ^= m.GetHashCode();
            return h;
        }
        #endregion
    }

    public class FieldReferenceExpression : MemberReferenceExpression, IFieldReferenceExpression
    {
        #region Constructors
        public FieldReferenceExpression(IExpression target, IField field)
            : base(target)
        {
            _field = field;
        }
        #endregion

        #region IFieldReferenceExpression Members
        public IField Field
        {
            get { return _field; }
            set { _field = value; }
        }
        private IField _field;
        #endregion

        #region IMemberReferenceExpression Members
        public override ITypeMember Member
        {
            get { return _field; }
            set
            {
                if (value != null)
                {
                    var f = value as IField;
                    if (f == null)
                        throw new InvalidOperationException();
                    _field = f;
                }
                else
                {
                    _field = null;
                }
            }
        }
        #endregion
    }

    public class PropertyReferenceExpression : MemberReferenceExpression, IPropertyReferenceExpression
    {
        #region Constructors
        public PropertyReferenceExpression(IExpression target, IProperty prop)
            : base(target)
        {
            _property = prop;
        }
        #endregion

        #region IPropertyReferenceExpression Members
        public IProperty Property
        {
            get { return _property; }
            set { _property = value; }
        }
        private IProperty _property;
        #endregion

        #region IMemberReferenceExpression Members
        public override ITypeMember Member
        {
            get { return _property; }
            set
            {
                if (value != null)
                {
                    var p = value as IProperty;
                    if (p == null)
                        throw new InvalidOperationException();
                    _property = p;
                }
                else
                {
                    _property = null;
                }
            }
        }
        #endregion
    }

    public class MethodReferenceExpression : MemberReferenceExpression, IMethodReferenceExpression
    {
        #region Constructors
        public MethodReferenceExpression(IExpression target, IMethod method)
            : base(target)
        {
            _method = method;
        }

        public MethodReferenceExpression(IMethod method)
        {
            _method = method;
        }
        #endregion

        #region IMethodReferenceExpression Members
        public IMethod Method
        {
            get { return _method; }
            set { _method = value; }
        }
        private IMethod _method;
        #endregion

        #region IMemberReferenceExpression Members
        public override ITypeMember Member
        {
            get { return _method; }
            set
            {
                if (value != null)
                {
                    var m = value as IMethod;
                    if (m == null)
                        throw new InvalidOperationException();
                    _method = m;
                }
                else
                {
                    _method = null;
                }
            }
        }
        #endregion

        #region ITypeReferenceProvider Members
        public override IEnumerable<IType> GetTypeReferences()
        {
            if (_method != null)
            {
                yield return _method.DeclaringType;
                yield return _method.Type;
                foreach (var p in _method.Parameters)
                {
                    yield return p.Type;
                }
            }
        }
        #endregion
    }

    public class EventReferenceExpression : MemberReferenceExpression, IEventReferenceExpression
    {
        #region Constructors
        public EventReferenceExpression(IExpression target, IEvent e)
            : base(target)
        {
            _event = e;
        }
        #endregion

        #region IEventReferenceExpression Members
        public IEvent Event
        {
            get { return _event; }
            set { _event = value; }
        }
        private IEvent _event;
        #endregion

        #region IMemberReferenceExpression Members
        public override ITypeMember Member
        {
            get { return _event; }
            set
            {
                if (value != null)
                {
                    var e = value as IEvent;
                    if (e == null)
                        throw new InvalidOperationException();
                    _event = e;
                }
                else
                {
                    _event = null;
                }
            }
        }
        #endregion
    }

    public class TypeReferenceExpression : Expression, ITypeReferenceExpression, ITypeReferenceProvider
    {
        #region Constructors
        public TypeReferenceExpression(IType type)
        {
            _type = type;
        }
        #endregion

        #region ITypeReferenceExpression Members
        public IType Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private IType _type;
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get { return null; }
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return false;
            var e = obj as ITypeReferenceExpression;
            if (e == null) return false;
            if (e.Type != _type) return false;
            return true;
        }

        private static readonly int _hs = typeof(ITypeReferenceExpression).GetHashCode();

        public override int GetHashCode()
        {
            if (_type != null)
                return _type.GetHashCode() ^ _hs;
            return base.GetHashCode();
        }
        #endregion

        #region ITypeReferenceProvider Members
        public IEnumerable<IType> GetTypeReferences()
        {
            return new[] { _type };
        }
        #endregion
    }

    public sealed class VariableReferenceExpression : Expression, IVariableReferenceExpression, ITypeReferenceProvider
    {
        #region Constructors
        public VariableReferenceExpression(IVariable var)
        {
            _var = var;
        }
        #endregion

        #region IVariableReferenceExpression Members
        public IVariable Variable
        {
            get { return _var; }
            set { _var = value; }
        }

        private IVariable _var;
        #endregion

        #region IExpression Members
        public override IType ResultType
        {
            get { return _var.Type; }
        }
        #endregion

        #region ITypeReferenceProvider Members
        public IEnumerable<IType> GetTypeReferences()
        {
            if (_var != null)
                return new[] { _var.Type };
            return null;
        }
        #endregion

        #region Object Override Members
        public override bool Equals(object obj)
        {
            if (obj == this) return true;
            var e = obj as IVariableReferenceExpression;
            if (e == null) return false;
            if (e.Variable != _var) return false;
            return true;
        }

        private static readonly int _hs = typeof(IVariableReferenceExpression).GetHashCode();

        public override int GetHashCode()
        {
            if (_var != null)
                return _var.GetHashCode() ^ _hs;
            return base.GetHashCode();
        }
        #endregion
    }
}