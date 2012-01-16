using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    internal sealed class LateMethodBody : IMethodBody
    {
        private readonly uint _rva;
        private readonly IMethod _method;
        private MethodBody _body;
        private readonly AssemblyLoader _loader;

        public LateMethodBody(AssemblyLoader loader, IMethod method, uint rva)
        {
            _loader = loader;
            _method = method;
            _rva = rva;
        }

        public MethodBody RealBody
        {
            get { return _body ?? (_body = _loader.Decompile(_method, _rva)); }
        }

        #region IMethodBody Members
        public IMethod Method
        {
            get { return _method; }
        }

        public int MaxStackSize
        {
            get
            {
                return RealBody.MaxStackSize;
            }
        }

        public IVariableCollection LocalVariables
        {
            get
            {
                return RealBody.LocalVariables;
            }
        }

        public IStatementCollection Statements
        {
            get
            {
                return RealBody.Statements;
            }
        }

        /// <summary>
        /// Provides translator that can be used to translate this method body using specific <see cref="ICodeProvider"/>.
        /// </summary>
        /// <returns><see cref="ITranslator"/></returns>
        public ITranslator CreateTranslator()
        {
            return RealBody.CreateTranslator();
        }

        /// <summary>
        /// Gets all calls that can be invocated in the method.
        /// </summary>
        /// <returns></returns>
        public IMethod[] GetCalls()
        {
            return RealBody.GetCalls();
        }

        public int[] GetReferencedMetadataTokens()
        {
            return RealBody.GetReferencedMetadataTokens();
        }
        #endregion

        public override string ToString()
        {
            return _body != null ? _body.ToString() : base.ToString();
        }
    }
}