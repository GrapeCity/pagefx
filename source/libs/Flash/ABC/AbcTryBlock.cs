using System.Collections.Generic;
using DataDynamics.PageFX.Flash.IL;

namespace DataDynamics.PageFX.Flash.Abc
{
	public sealed class AbcTryBlock
	{
		/// <summary>
		/// Gets or sets begin offset of protected block.
		/// </summary>
		public int From { get; set; }
        
		/// <summary>
		/// Gets or sets end offset of protected block.
		/// </summary>
		public int To { get; set; }

		/// <summary>
		/// Used to check BeginCatch/EndCatch balance
		/// </summary>
		internal AbcExceptionHandler CurrentHandler;

		internal Instruction SkipHandlers;

		public AbcExceptionHandlerCollection Handlers
		{
			get { return _handlers; }
		}
		private readonly AbcExceptionHandlerCollection _handlers = new AbcExceptionHandlerCollection();
	}

	public sealed class AbcTryBlockList : List<AbcTryBlock>
	{
	}
}