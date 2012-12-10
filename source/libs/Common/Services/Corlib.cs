using System;
using DataDynamics.PageFX.Common.TypeSystem;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.PageFX.Common.Services
{
    #region class Corlib
    public static class Corlib
    {
	    /// <summary>
	    /// Finds corlib type.
	    /// </summary>
	    /// <param name="assembly"></param>
	    /// <param name="fullname"></param>
	    /// <returns></returns>
	    public static IType FindType(IAssembly assembly, string fullname)
	    {
		    var type = assembly.Corlib().FindType(fullname);
		    if (type == null)
			    throw new InvalidOperationException(
				    string.Format("Unable to find {0}. Invalid corlib.", fullname));
		    return type;
	    }

	    public static IGenericType FindGenericType(IAssembly assembly, string fullname)
	    {
		    var type = FindType(assembly, fullname) as IGenericType;
		    if (type == null)
			    throw new InvalidOperationException(
				    string.Format("Unable to find {0}. Invalid corlib.", fullname));
		    return type;
	    }

	    /// <summary>
        /// Provides type properties for types from corlib
        /// </summary>
        public static class Types
        {
		    /// <summary>
		    /// Returns System.Reflection.Assembly type.
		    /// </summary>
		    /// <param name="assembly"></param>
		    public static IType Assembly(IAssembly assembly)
	        {
		        return FindType(assembly, "System.Reflection.Assembly");
	        }

		    /// <summary>
		    /// Returns System.Console type.
		    /// </summary>
		    /// <param name="assembly"></param>
		    public static IType Console(IAssembly assembly)
	        {
		        return FindType(assembly, "System.Console");
	        }

		    /// <summary>
		    /// Return System.NullReferenceException
		    /// </summary>
		    /// <param name="assembly"></param>
		    public static IType NullReferenceException(IAssembly assembly)
	        {
		        return FindType(assembly, "System.NullReferenceException");
	        }

		    /// <summary>
		    /// Returns System.InvalidCastException type
		    /// </summary>
		    /// <param name="assembly"></param>
		    public static IType InvalidCastException(IAssembly assembly)
	        {
		        return FindType(assembly, "System.InvalidCastException");
	        }

	        public static IType NotImplementedException(IAssembly assembly)
	        {
		        return FindType(assembly, "System.NotImplementedException");
	        }

	        public static IType NotSupportedException(IAssembly assembly)
	        {
		        return FindType(assembly, "System.NotSupportedException");
	        }

	        public static IType ArgumentOutOfRangeException(IAssembly assembly)
	        {
		        return FindType(assembly, "System.ArgumentOutOfRangeException");
	        }

	        public static IType ExecutionEngineException(IAssembly assembly)
	        {
		        return FindType(assembly, "System.ExecutionEngineException");
	        }

	        public static IType TypeLoadException(IAssembly assembly)
	        {
		        return FindType(assembly, "System.TypeLoadException");
	        }

		    /// <summary>
		    /// Returns System.EnumInfo type
		    /// </summary>
		    /// <param name="assembly"></param>
		    public static IType EnumInfo(IAssembly assembly)
	        {
		        return FindType(assembly, "System.EnumInfo");
	        }

		    /// <summary>
		    /// Returns System.Reflection.FieldInfo type
		    /// </summary>
		    /// <param name="assembly"></param>
		    public static IType FieldInfo(IAssembly assembly)
	        {
		        return FindType(assembly, "System.Reflection.FieldInfo");
	        }

		    /// <summary>
		    /// Returns System.Reflection.MethoBase type
		    /// </summary>
		    /// <param name="assembly"></param>
		    public static IType MethodBase(IAssembly assembly)
	        {
		        return FindType(assembly, "System.Reflection.MethodBase");
	        }

		    /// <summary>
		    /// Returns System.Reflection.MethoInfo type
		    /// </summary>
		    /// <param name="assembly"></param>
		    public static IType MethodInfo(IAssembly assembly)
	        {
		        return FindType(assembly, "System.Reflection.MethodInfo");
	        }

		    /// <summary>
		    /// Returns System.Reflection.ParametrInfo type
		    /// </summary>
		    /// <param name="assembly"></param>
		    public static IType ParameterInfo(IAssembly assembly)
	        {
		        return FindType(assembly, "System.Reflection.ParameterInfo");
	        }


		    /// <summary>
		    /// Returns System.Reflection.PropertyInfo type
		    /// </summary>
		    /// <param name="assembly"></param>
		    public static IType PropertyInfo(IAssembly assembly)
	        {
		        return FindType(assembly, "System.Reflection.PropertyInfo");
	        }

		    /// <summary>
		    /// Returns System.Reflection.ConstructorInfo type
		    /// </summary>
		    /// <param name="assembly"></param>
		    public static IType ConstructorInfo(IAssembly assembly)
	        {
		        return FindType(assembly, "System.Reflection.ConstructorInfo");
	        }

	        public static IType Convert(IAssembly assembly)
	        {
		        return FindType(assembly, "System.Convert");
	        }

	        public static IType CharEnumerator(IAssembly assembly)
	        {
		        return FindType(assembly, "System.CharEnumerator");
	        }

	        public static IType Environment(IAssembly assembly)
	        {
		        return FindType(assembly, "System.Environment");
	        }

	        public static IType IEnumerable(IAssembly assembly)
	        {
		        return FindType(assembly, CLRNames.Types.IEnumerable);
	        }

	        public static IGenericType IEnumerableT(IAssembly assembly)
	        {
		        return FindGenericType(assembly, CLRNames.Types.IEnumerableT);
	        }

	        public static IType IEnumerator(IAssembly assembly)
	        {
		        return FindType(assembly, CLRNames.Types.IEnumerator);
	        }

	        public static IType IDictionary(IAssembly assembly)
	        {
		        return FindType(assembly, CLRNames.Types.IDictionary);
	        }

	        public static IGenericType IEnumeratorT(IAssembly assembly)
	        {
		        return FindGenericType(assembly, CLRNames.Types.IEnumeratorT);
	        }

	        public static IGenericType ICollectionT(IAssembly assembly)
	        {
		        return FindGenericType(assembly, CLRNames.Types.ICollectionT);
	        }

	        public static IGenericType IListT(IAssembly assembly)
	        {
		        return FindGenericType(assembly, CLRNames.Types.IListT);
	        }

	        public static IGenericType NullableT(IAssembly assembly)
	        {
		        return FindGenericType(assembly, CLRNames.Types.NullableT);
	        }

	        public static IGenericType ArrayEnumeratorT(IAssembly assembly)
	        {
		        return FindGenericType(assembly, "System.Array+Enumerator`1");
	        }
        }
    }
    #endregion

    #region CorlibTypeCache
    public enum CorlibTypeId
    {
        NullReferenceException,
        InvalidCastException,
        NotImplementedException,
        NotSupportedException,
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

	public sealed class CorlibTypeCache
	{
		private readonly LazyValue<IType>[] _types;
		private readonly LazyValue<IType>[] _generics;

		public CorlibTypeCache(IAssembly assembly)
		{
			if (assembly == null)
				throw new ArgumentNullException("assembly");

			assembly = assembly.Corlib();

			_types = new[]
				{
					new LazyValue<IType>(() => Corlib.Types.NullReferenceException(assembly)),
					new LazyValue<IType>(() => Corlib.Types.InvalidCastException(assembly)),
					new LazyValue<IType>(() => Corlib.Types.NotImplementedException(assembly)),
					new LazyValue<IType>(() => Corlib.Types.NotSupportedException(assembly)),
					new LazyValue<IType>(() => Corlib.Types.ArgumentOutOfRangeException(assembly)),
					new LazyValue<IType>(() => Corlib.Types.ExecutionEngineException(assembly)),
					new LazyValue<IType>(() => Corlib.Types.TypeLoadException(assembly)),
					new LazyValue<IType>(() => Corlib.Types.EnumInfo(assembly)),
					new LazyValue<IType>(() => Corlib.Types.Environment(assembly)),
					new LazyValue<IType>(() => Corlib.Types.Assembly(assembly)),
					new LazyValue<IType>(() => Corlib.Types.CharEnumerator(assembly)),
					new LazyValue<IType>(() => Corlib.Types.Convert(assembly)),
					new LazyValue<IType>(() => Corlib.Types.FieldInfo(assembly)),
					new LazyValue<IType>(() => Corlib.Types.IEnumerable(assembly)),
					new LazyValue<IType>(() => Corlib.Types.IEnumerator(assembly)),
					new LazyValue<IType>(() => Corlib.Types.IDictionary(assembly)),
					new LazyValue<IType>(() => Corlib.Types.MethodBase(assembly)),
					new LazyValue<IType>(() => Corlib.Types.MethodInfo(assembly)),
					new LazyValue<IType>(() => Corlib.Types.ConstructorInfo(assembly)),
					new LazyValue<IType>(() => Corlib.Types.ParameterInfo(assembly)),
					new LazyValue<IType>(() => Corlib.Types.PropertyInfo(assembly)),
					new LazyValue<IType>(() => Corlib.Types.Console(assembly)),
					new LazyValue<IType>(() => Corlib.FindType(assembly, "PageFX.CompilerUtils"))
				};

			_generics = new[]
				{
					new LazyValue<IType>(() => Corlib.Types.ICollectionT(assembly)),
					new LazyValue<IType>(() => Corlib.Types.IEnumerableT(assembly)),
					new LazyValue<IType>(() => Corlib.Types.IEnumeratorT(assembly)),
					new LazyValue<IType>(() => Corlib.Types.IListT(assembly)),
					new LazyValue<IType>(() => Corlib.Types.NullableT(assembly)),
					new LazyValue<IType>(() => Corlib.Types.ArrayEnumeratorT(assembly))
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

		public IGenericType this[GenericTypeId c]
		{
			get
			{
				var i = (int)c;
				return (IGenericType)_generics[i].Value;
			}
		}
	}

	#endregion
}