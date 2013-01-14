using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;

namespace DataDynamics.PageFX.Ecma335.IL
{
	/// <summary>
	/// Represents try block.
	/// </summary>
	internal sealed class TryCatchBlock : Block, ISehTryBlock
	{
		public TryCatchBlock()
		{
			_handlers.Owner = this;
		}

		public override BlockType Type
		{
			get { return BlockType.Protected; }
		}

		public bool IsTryFinally
		{
			get { return _handlers.Count == 1 && ((HandlerBlock)_handlers[0]).Type == BlockType.Finally; }
		}

		public HandlerCollection Handlers
		{
			get { return _handlers; }
		}
		private readonly HandlerCollection _handlers = new HandlerCollection();

		ISehHandlerCollection ISehTryBlock.Handlers
		{
			get { return _handlers; }
		}

		public override string ToString()
		{
			if (IsTryFinally)
			{
				return string.Format("TryFinally[{0}-{1}]", EntryIndex, ExitIndex);
			}
			return base.ToString();
		}
	}

	internal sealed class HandlerCollection : ISehHandlerCollection
	{
		private readonly List<HandlerBlock> _list = new List<HandlerBlock>();

		public TryCatchBlock Owner { get; set; }

		public void Add(HandlerBlock block)
		{
			block.Owner = Owner;
			_list.Add(block);
		}

		public int Count
		{
			get { return _list.Count; }
		}

		public ISehHandlerBlock this[int index]
		{
			get { return _list[index]; }
		}

		public IEnumerator<ISehHandlerBlock> GetEnumerator()
		{
			return _list.Cast<ISehHandlerBlock>().GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return _list.GetEnumerator();
		}

		public int IndexOf(HandlerBlock handlerBlock)
		{
			return _list.IndexOf(handlerBlock);
		}
	}
}