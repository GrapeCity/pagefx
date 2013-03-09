using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.Common.Services
{
	using LazyType = LazyValue<IType>;

	public enum CorlibTypeId
    {
        NullReferenceException,
        InvalidCastException,
        NotImplementedException,
        NotSupportedException,
		IndexOutOfRangeException,
        ArgumentOutOfRangeException,
        ExecutionEngineException,
        TypeLoadException,
        EnumInfo,
        Environment,
        Assembly,
        CharEnumerator,
        Convert,
        FieldInfo,
        IEnumerable,
        IEnumerator,
        IDictionary,
        MethodBase,
        MethodInfo,
        ConstructorInfo,
        ParameterInfo,
        PropertyInfo,
        Console,
        CompilerUtils,
    }

    public enum GenericTypeId
    {
        ICollectionT,
        IEnumerableT,
        IEnumeratorT,
        IListT,
        NullableT,
        ArrayEnumeratorT
    }

	public sealed class CorlibTypes
	{
		private readonly LazyType[] _types;
		private readonly LazyType[] _generics;
		private readonly IAssembly _assembly;

		public CorlibTypes(IAssembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			_assembly = assembly.Corlib();

			_types = new[]
				{
					new LazyType(() => FindType("System.NullReferenceException")),
					new LazyType(() => FindType("System.InvalidCastException")),
					new LazyType(() => FindType("System.NotImplementedException")),
					new LazyType(() => FindType("System.NotSupportedException")),
					new LazyType(() => FindType("System.IndexOutOfRangeException")),
					new LazyType(() => FindType("System.ArgumentOutOfRangeException")),
					new LazyType(() => FindType("System.ExecutionEngineException")),
					new LazyType(() => FindType("System.TypeLoadException")),
					new LazyType(() => FindType("System.EnumInfo")),
					new LazyType(() => FindType("System.Environment")),
					new LazyType(() => FindType("System.Reflection.Assembly")),
					new LazyType(() => FindType("System.CharEnumerator")),
					new LazyType(() => FindType("System.Convert")),
					new LazyType(() => FindType("System.Reflection.FieldInfo")),
					new LazyType(() => FindType(CLRNames.Types.IEnumerable)),
					new LazyType(() => FindType(CLRNames.Types.IEnumerator)),
					new LazyType(() => FindType(CLRNames.Types.IDictionary)),
					new LazyType(() => FindType("System.Reflection.MethodBase")),
					new LazyType(() => FindType("System.Reflection.MethodInfo")),
					new LazyType(() => FindType("System.Reflection.ConstructorInfo")),
					new LazyType(() => FindType("System.Reflection.ParameterInfo")),
					new LazyType(() => FindType("System.Reflection.PropertyInfo")),
					new LazyType(() => FindType("System.Console")),
					new LazyType(() => FindType("PageFX.CompilerUtils"))
				};

			_generics = new[]
				{
					new LazyType(() => FindGenericType(CLRNames.Types.ICollectionT)),
					new LazyType(() => FindGenericType(CLRNames.Types.IEnumerableT)),
					new LazyType(() => FindGenericType(CLRNames.Types.IEnumeratorT)),
					new LazyType(() => FindGenericType(CLRNames.Types.IListT)),
					new LazyType(() => FindGenericType(CLRNames.Types.NullableT)),
					new LazyType(() => FindGenericType("System.Array+Enumerator`1"))
				};
		}

		public IType this[CorlibTypeId c]
		{
			get
			{
				var i = (int)c;
				return _types[i].Value;
			}
		}

		public IType this[GenericTypeId c]
		{
			get
			{
				var i = (int)c;
				return _generics[i].Value;
			}
		}

		private IType FindType(string fullname)
		{
			var type = _assembly.FindType(fullname);
			if (type == null)
				throw new InvalidOperationException(string.Format("Unable to find {0}. Invalid corlib.", fullname));
			return type;
		}

		private IType FindGenericType(string fullname)
		{
			var type = FindType(fullname);
			if (type == null)
				throw new InvalidOperationException(string.Format("Unable to find {0}. Invalid corlib.", fullname));
			return type;
		}
	}
}