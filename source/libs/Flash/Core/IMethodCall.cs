using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Flash.Abc;

namespace DataDynamics.PageFX.Flash.Core
{
	/// <summary>
	/// Defines common contract for units that present <see cref="IMethod"/>.
	/// </summary>
	internal interface IMethodCall
	{
		/// <summary>
		/// Gets method presented by the agent.
		/// </summary>
		IMethod Method { get; }

		/// <summary>
		/// Gets name by which method could be called.
		/// </summary>
		AbcMultiname Name { get; }
	}

	internal sealed class ExternalCall : IMethodCall
	{
		public ExternalCall(IMethod method, AbcMultiname name)
		{
			if (method == null)
				throw new ArgumentNullException("method");
			if (name == null)
				throw new ArgumentNullException("name");

			Method = method;
			Name = name;
		}

		public IMethod Method { get; private set; }

		public AbcMultiname Name { get; private set; }
	}
}
