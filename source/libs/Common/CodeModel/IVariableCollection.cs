using DataDynamics.PageFX.Common.Collections;

namespace DataDynamics.PageFX.Common.CodeModel
{
	/// <summary>
	/// Represents collection of <see cref="IVariable"/>s.
	/// </summary>
	public interface IVariableCollection : IReadOnlyList<IVariable>, ICodeNode
	{
		/// <summary>
		/// Finds local variable by given name.
		/// </summary>
		/// <param name="name">name of variable to find.</param>
		/// <returns></returns>
		IVariable this[string name] { get; }
	}
}