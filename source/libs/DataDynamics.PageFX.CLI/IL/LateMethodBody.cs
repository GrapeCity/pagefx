using System.Collections.Generic;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.IL
{
    internal sealed class LateMethodBody : IClrMethodBody
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

        private MethodBody Impl
        {
            get { return _body ?? (_body = _loader.LoadMethodBody(_method, _rva)); }
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
                return Impl.MaxStackSize;
            }
        }

        public IVariableCollection LocalVariables
        {
            get
            {
                return Impl.LocalVariables;
            }
        }

        public IStatementCollection Statements
        {
            get
            {
                return Impl.Statements;
            }
        }

        /// <summary>
        /// Provides translator that can be used to translate this method body using specific <see cref="ICodeProvider"/>.
        /// </summary>
        /// <returns><see cref="ITranslator"/></returns>
        public ITranslator CreateTranslator()
        {
            return Impl.CreateTranslator();
        }

        /// <summary>
        /// Gets all calls that can be invocated in the method.
        /// </summary>
        /// <returns></returns>
        public IMethod[] GetCalls()
        {
            return Impl.GetCalls();
        }

        public int[] GetReferencedMetadataTokens()
        {
            return Impl.GetReferencedMetadataTokens();
        }
        #endregion

        public override string ToString()
        {
            return _body != null ? _body.ToString() : base.ToString();
        }

    	#region Implementation of IClrMethodBody

    	public bool HasProtectedBlocks
    	{
			get { return Impl.HasProtectedBlocks; }
    	}

    	public IReadOnlyList<TryCatchBlock> ProtectedBlocks
    	{
			get { return Impl.ProtectedBlocks; }
    	}

    	public ILStream Code
    	{
			get { return Impl.Code; }
    	}

    	public bool HasGenerics
    	{
			get { return Impl.HasGenerics; }
    	}

    	public bool HasGenericVars
    	{
			get { return Impl.HasGenericVars; }
    	}

    	public bool HasGenericInstructions
    	{
			get { return Impl.HasGenericInstructions; }
    	}

    	public bool HasGenericExceptions
    	{
			get { return Impl.HasGenericExceptions; }
    	}

    	public int InstanceCount
    	{
			get { return Impl.InstanceCount; }
			set { Impl.InstanceCount = value; }
    	}

    	public FlowGraph FlowGraph
    	{
			get { return Impl.FlowGraph; }
			set { Impl.FlowGraph = value; }
    	}

    	#endregion
    }
}