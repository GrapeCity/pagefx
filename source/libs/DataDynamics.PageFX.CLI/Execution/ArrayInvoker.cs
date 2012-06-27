﻿using System;
using DataDynamics.PageFX.CLI.IL;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.CLI.Execution
{
	internal sealed class ArrayInvoker : NativeInvoker
	{
		private readonly VirtualMachine _engine;

		public ArrayInvoker(VirtualMachine engine)
			: base(typeof(Array))
		{
			_engine = engine;
		}

		public override object Invoke(IMethod method, object instance, object[] args)
		{
			if (method.IsConstructor)
			{
				int[] lengths = Array.ConvertAll<object, int>(args, Convert.ToInt32);

				var arrayType = (IArrayType)method.DeclaringType;
				return _engine.NewArray(arrayType.ElementType, lengths);
			}

			var array = (Array)instance;

			int[] index;
			switch (method.Name)
			{
				case "Get":
					index = Array.ConvertAll<object, int>(args, Convert.ToInt32);
					return array.GetValue(index);

				case "Address":
					index = Array.ConvertAll<object, int>(args, Convert.ToInt32);
					return new MdArrayElementPtr(array, index);

				case "Set":
					object value = args[args.Length - 1];
					var copy = new object[args.Length - 1];
					Array.Copy(args, copy, args.Length - 1);
					index = Array.ConvertAll<object, int>(copy, Convert.ToInt32);
					array.SetValue(value, index);
					return null;

				default:
					return base.Invoke(method, instance, args);
			}			
		}

		public static void InitializeArray(Array array, IField field)
		{
			var elemType = array.GetType().GetElementType();

			var vals = CLR.ReadArrayValues(field, Type.GetTypeCode(elemType));

			int n = vals.Count;
			for (int i = 0; i < n; ++i)
			{
				array.SetValue(vals[i], i);
			}
		}
	}
}
