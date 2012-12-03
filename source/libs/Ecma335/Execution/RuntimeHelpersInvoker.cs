using System;
using System.Runtime.CompilerServices;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Ecma335.IL;

namespace DataDynamics.PageFX.Ecma335.Execution
{
	internal sealed class RuntimeHelpersInvoker : NativeInvoker
	{
		private readonly VirtualMachine _engine;

		public RuntimeHelpersInvoker(VirtualMachine engine) 
			: base(typeof(RuntimeHelpers))
		{
			_engine = engine;
		}

		public override object Invoke(IMethod method, object instance, object[] args)
		{
			if (method.IsInitializeArray())
			{
				InitializeArray((Array)args[0], (IField)args[1]);
				return null;
			}

			if (method.IsGetTypeFromHandle())
			{
				var type = args[0] as IType;
				return _engine.TypeOf(type);
			}

			return base.Invoke(method, instance, args);
		}

		private static void InitializeArray(Array array, IField field)
		{
			var elemType = array.GetType().GetElementType();

			var vals = CLR.ReadArrayValues(field, Type.GetTypeCode(elemType));

			if (array.Rank == 1)
			{
				int n = vals.Count;
				for (int i = 0; i < n; ++i)
				{
					array.SetValue(vals[i], i);
				}
			}
			else
			{
				int i = 0;
				var it = new ArrayIterator(array);
				while (it.MoveNext())
				{
					array.SetValue(vals[i], it.Indices);
					i++;
				}
			}
		}
	}
}
