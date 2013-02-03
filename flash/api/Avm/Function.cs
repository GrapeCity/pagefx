using System;

namespace Avm
{
	public partial class Function
	{
		public static implicit operator Function(Delegate d)
		{
			return (Function)d.Function;
		}
	}
}