using System;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.TypeSystem;

namespace DataDynamics.PageFX.Core.Execution
{
	internal sealed class DelegateInvoker : NativeInvoker
	{
		private readonly VirtualMachine _engine;

		public DelegateInvoker(VirtualMachine engine) 
			: base(typeof(Delegate))
		{
			_engine = engine;
		}

		public override object Invoke(IMethod method, object instance, object[] args)
		{
			if (method.IsConstructor)
			{
				var f = args[1] as MethodPtr;
				if (f == null)
					throw new InvalidOperationException("Current instruction requires method pointer.");

				var closure = new Closure(_engine, args[0], f.Method);

				return new DelegateImpl(closure);
			}

			if (method.Name == "Invoke")
			{
				//var delegateInstance = instance as Instance;
				//var obj = delegateInstance.Fields[0].Value;
				//method = delegateInstance.Fields[1].Value as IMethod;
				//return _engine.Call(method, obj, args, false);

				var d = instance as DelegateImpl;
				return d.Invoke(args);
			}

			if (method.Name == "Combine")
			{
				//TODO: Combine array of delegates
				var x = args[0] as DelegateImpl;
				var y = args[1] as DelegateImpl;
				if (x == null) return y;
				return x.Combine(y);
			}

			if (method.Name == "Remove")
			{
				var x = args[0] as DelegateImpl;
				var y = args[1] as DelegateImpl;
				if (x == null) return null;
				if (y == null) return x;
				return x.Remove(y);
			}

			throw new NotImplementedException();
		}
	}

	internal sealed class DelegateImpl
	{
		private readonly List<Closure> _list = new List<Closure>();

		public DelegateImpl(Closure closure)
		{
			_list.Add(closure);
		}

		public bool Contains(Closure c)
		{
			return Enumerable.Contains(_list, c);
		}

		public DelegateImpl Combine(DelegateImpl other)
		{
			_list.AddRange(other._list.Where(x => !Contains(x)));
			return this;
		}

		public DelegateImpl Remove(DelegateImpl other)
		{
			foreach (var c in other._list)
			{
				_list.Remove(c);
			}
			return this;
		}

		public object Invoke(object[] args)
		{
			object result = null;
			for (int i = 0; i < _list.Count; i++)
			{
				result = _list[i].Invoke(args);
			}
			return result;
		}
	}

	internal sealed class Closure
	{
		private readonly VirtualMachine _engine;
		private readonly object _target;
		private readonly IMethod _method;
			
		public Closure(VirtualMachine engine, object target, IMethod method)
		{
			_engine = engine;
			_target = target;
			_method = method;
		}

		public object Invoke(object[] args)
		{
			return _engine.Call(_method, _target, args, false);
		}

		public override bool Equals(object obj)
		{
			var other = obj as Closure;
			if (other == null) return false;
			if (_method != other._method) return false;
			if (!Equals(_target, other._target)) return false;
			return true;
		}
	}
}
