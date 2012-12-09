using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataDynamics.PageFX.Common.CodeModel;
using DataDynamics.PageFX.Common.Collections;
using DataDynamics.PageFX.Common.Syntax;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Ecma335.Metadata;

namespace DataDynamics.PageFX.Ecma335.LoaderInternals.Collections
{
	internal sealed class ParamList : IParameterCollection
	{
		private readonly AssemblyLoader _loader;
		private readonly IMethod _owner;
		private readonly int _from;
		private readonly MethodSignature _signature;
		private IReadOnlyList<IParameter> _list;

		public ParamList(AssemblyLoader loader, IMethod owner, int from, MethodSignature signature)
		{
			_loader = loader;
			_owner = owner;
			_from = from;
			_signature = signature;
		}

		public IEnumerator<IParameter> GetEnumerator()
		{
			return List.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public int Count
		{
			get { return List.Count; }
		}

		public IParameter this[int index]
		{
			get { return List[index]; }
		}

		public string ToString(string format, IFormatProvider formatProvider)
		{
			return SyntaxFormatter.Format(this, format, formatProvider);
		}

		public IEnumerable<ICodeNode> ChildNodes
		{
			get { return this.Cast<ICodeNode>(); }
		}

		public object Tag { get; set; }

		public IParameter this[string name]
		{
			get { return this.FirstOrDefault(x => x.Name == name); }
		}

		public void Add(IParameter parameter)
		{
			throw new NotSupportedException();
		}

		private IReadOnlyList<IParameter> List
		{
			get { return _list ?? (_list = Populate().Memoize()); }
		}

		private IEnumerable<IParameter> Populate()
		{
			var context = ResolveContext();

			for (int i = _from, k = 0; k < _signature.Params.Length; ++i)
			{
				var type = GetParameterType(k++, context);

				if (i >= _loader.Parameters.Count)
				{
					var param = new Parameter
						{
							Index = i + 1,
							Name = "arg" + (i + 1)
						};
					yield return new TypedParameter(param, type);
				}
				else
				{
					var param = _loader.Parameters[i];
					if (param.Index == 0)
					{
						//0 refers to the owner method's return type;
						continue;
					}

					yield return new TypedParameter(param, type);
				}
			}
		}

		private LazyType GetParameterType(int index, Context context)
		{
			var typeSignature = _signature.Params[index];
			return new LazyType(_loader, typeSignature, context);
		}

		private Context ResolveContext()
		{
			var declType = _owner.DeclaringType;
			if (declType == null)
				throw new InvalidOperationException();
			return new Context(declType, _owner);
		}

		private sealed class TypedParameter : IParameter
		{
			private readonly IParameter _parameter;
			private LazyType _type;

			public TypedParameter(IParameter parameter, LazyType type)
			{
				_parameter = parameter;
				_type = type;
			}

			public int MetadataToken
			{
				get { return _parameter.MetadataToken; }
				set { _parameter.MetadataToken = value; }
			}

			public ICustomAttributeCollection CustomAttributes
			{
				get { return _parameter.CustomAttributes; }
			}

			public object Value
			{
				get { return _parameter.Value; }
				set { _parameter.Value = value; }
			}

			public IEnumerable<ICodeNode> ChildNodes
			{
				get { return null; }
			}

			public object Tag
			{
				get { return _parameter.Tag; }
				set { _parameter.Tag = value; }
			}

			public string Documentation
			{
				get { return _parameter.Documentation; }
				set { _parameter.Documentation = value; }
			}

			public object Clone()
			{
				return new TypedParameter((IParameter)_parameter.Clone(), _type);
			}

			public int Index
			{
				get { return _parameter.Index; }
				set { _parameter.Index = value; }
			}

			public string Name
			{
				get { return _parameter.Name; }
				set { _parameter.Name = value; }
			}

			public IType Type
			{
				get { return _parameter.Type ?? (_parameter.Type = ResolveType()); }
				set { _parameter.Type = value; }
			}

			private IType ResolveType()
			{
				var type = _type.ResolveType();
				_type = null;
				return type;
			}

			public bool IsIn
			{
				get { return _parameter.IsIn; }
			}

			public bool IsOut
			{
				get { return _parameter.IsOut; }
			}

			public bool HasParams
			{
				get { return _parameter.HasParams; }
				set { _parameter.HasParams = value; }
			}

			public bool IsAddressed
			{
				get { return _parameter.IsAddressed; }
				set { _parameter.IsAddressed = value; }
			}

			public IInstruction Instruction
			{
				get { return _parameter.Instruction; }
				set { _parameter.Instruction = value; }
			}

			public string ToString(string format, IFormatProvider formatProvider)
			{
				return SyntaxFormatter.Format(this, format, formatProvider);
			}

			public override string ToString()
			{
				return ToString(null, null);
			}
		}
	}
}
