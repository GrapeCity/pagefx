using System;

namespace DataDynamics.PageFX.CodeModel
{
    public class MethodBody : IMethodBody
    {
        public MethodBody(IMethod method)
        {
            _method = method;
            method.Body = this;
            _statements = new StatementCollection();
        }
        readonly IMethod _method;

        #region IMethodBody Members
        public IMethod Method
        {
            get { return _method; }
        }

        public int MaxStackSize { get; set; }

        public IVariableCollection LocalVariables
        {
            get { return _vars; }
        }
        readonly VariableCollection _vars = new VariableCollection();

        public IStatementCollection Statements
        {
            get { return _statements; }
        }
        readonly StatementCollection _statements;

        /// <summary>
        /// Provides translator that can be used to translate this method body using specific <see cref="ICodeProvider"/>.
        /// </summary>
        /// <returns></returns>
        public ITranslator CreateTranslator()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets all calls that can be invocated in the method.
        /// </summary>
        /// <returns></returns>
        public IMethod[] GetCalls()
        {
            return null;
        }

        public int[] GetReferencedMetadataTokens()
        {
            return null;
        }
        #endregion
    }
}