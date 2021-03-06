using DataDynamics.PageFX.Common.CodeModel.Statements;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common.CodeModel
{
    /// <summary>
    /// Represents method body.
    /// </summary>
    public interface IMethodBody
    {
        /// <summary>
        /// Gets owner method.
        /// </summary>
        IMethod Method { get; }

        int MaxStackSize { get; }

        /// <summary>
        /// Gets local variables used in the method body.
        /// </summary>
        IVariableCollection LocalVariables { get; }

        /// <summary>
        /// Gets method body statements.
        /// </summary>
        IStatementCollection Statements { get; }

        /// <summary>
        /// Provides translator that can be used to translate this method body using specific <see cref="ICodeProvider"/>.
        /// </summary>
        /// <returns><see cref="ITranslator"/></returns>
        ITranslator CreateTranslator();

        /// <summary>
        /// Gets all calls that can be invocated in the method.
        /// </summary>
        /// <returns></returns>
        IMethod[] GetCalls();
    }
}