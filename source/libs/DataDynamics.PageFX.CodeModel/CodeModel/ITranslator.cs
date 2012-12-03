using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Common.CodeModel
{
    /// <summary>
    /// Interfaces <see cref="IMethodBody"/> translator using specific <see cref="ICodeProvider"/>.
    /// </summary>
    public interface ITranslator
    {
        ///// <summary>
        ///// Gets or sets flag indicating whether translator should emit debug info.
        ///// </summary>
        //bool EmitDebugInfo { get; set; }

        /// <summary>
        /// Translates given <see cref="IMethodBody"/> using given  <see cref="ICodeProvider"/>.
        /// </summary>
        /// <param name="method"></param>
        /// <param name="body">body to translate.</param>
        /// <param name="provider">code provider to use.</param>
        /// <returns>translated code.</returns>
        IInstruction[] Translate(IMethod method, IMethodBody body, ICodeProvider provider);

#if DEBUG
        /// <summary>
        /// Dumps IL map in given format to specified file.
        /// </summary>
        /// <param name="format"></param>
        /// <param name="filename"></param>
        void DumpILMap(string format, string filename);
#endif
    }
}