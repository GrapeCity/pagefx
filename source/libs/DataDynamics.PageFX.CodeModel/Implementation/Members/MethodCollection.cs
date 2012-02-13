using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.Collections;
using DataDynamics.PageFX.CodeModel.Syntax;

namespace DataDynamics.PageFX.CodeModel
{
    [XmlElementName("Methods")]
    public class MethodCollection : ParameterizedMemberCollection<IMethod>, IMethodCollection
    {
        public MethodCollection(IType owner) : base(owner)
        {
        }

        #region IMethodCollection Members
        protected override void OnAdd(IMethod method)
        {
            if (method.IsConstructor)
            {
                _ctors.Add(method);
                if (method.IsStatic)
                    _cctor = method;
            }
        }

        public IEnumerable<IMethod> Constructors
        {
            get { return _ctors; }
        }
        readonly List<IMethod> _ctors = new List<IMethod>();

        public IMethod StaticConstructor
        {
            get { return _cctor; }
        }
        IMethod _cctor;
        #endregion

        #region ICodeNode Members
        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Methods; }
        }

        public object Tag { get; set; }
        #endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return SyntaxFormatter.Format(this, format, formatProvider);
        }
        #endregion

        public override string ToString()
        {
            return ToString(null, null);
        }
    }

    class EmptyMethodCollection : IMethodCollection
    {
        public static readonly IMethodCollection Instance = new EmptyMethodCollection();

        #region IMethodCollection Members
        public int Count
        {
            get { return 0; }
        }

        public IMethod this[int index]
        {
            get { return null; }
        }

        public void Add(IMethod method)
        {
        }

        public IEnumerable<IMethod> this[string name]
        {
            get { return Enumerable.Empty<IMethod>(); }
        }

        public IMethod this[string name, Predicate<IMethod> predicate]
        {
            get { return null; }
        }

        public IMethod this[string name, IType arg1]
        {
            get { return null; }
        }

        public IMethod this[string name, int argc]
        {
            get { return null; }
        }

        public IMethod this[string name, IType arg1, IType arg2]
        {
            get { return null; }
        }

        public IMethod this[string name, IType arg1, IType arg2, IType arg3]
        {
            get { return null; }
        }

        public IMethod this[string name, params IType[] args]
        {
            get { return null; }
        }

        public IMethod this[string name, Predicate<IParameterCollection> predicate]
        {
            get { return null; }
        }

        public IMethod this[string name, Predicate<IType> arg1]
        {
            get { return null; }
        }

        public IMethod this[string name, Predicate<IType> arg1, Predicate<IType> arg2]
        {
            get { return null; }
        }

        public IMethod this[string name, Predicate<IType> arg1, Predicate<IType> arg2, Predicate<IType> arg3]
        {
            get { return null; }
        }

        public IEnumerable<IMethod> Constructors
        {
            get { return Enumerable.Empty<IMethod>(); }
        }

        public IMethod StaticConstructor
        {
            get { return null; }
        }
        #endregion

        #region IEnumerable Members
        public IEnumerator<IMethod> GetEnumerator()
        {
            return new EmptyEnumerator<IMethod>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region ICodeNode Members
        public CodeNodeType NodeType
        {
            get { return CodeNodeType.Methods; }
        }

        public IEnumerable<ICodeNode> ChildNodes
        {
			get { return this.Cast<ICodeNode>(); }
        }

        public object Tag
        {
            get { return null; }
            set { }
        }
        #endregion

        #region IFormattable Members
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return "";
        }
        #endregion
    }
}