using System;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.FLI
{
    #region class Corlib
    static class Corlib
    {
        /// <summary>
        /// Returns corlib assembly
        /// </summary>
        public static IAssembly Assembly
        {
            get { return SystemTypes.Object != null ? SystemTypes.Object.Assembly : null; }
        }

        /// <summary>
        /// Finds corlib type
        /// </summary>
        /// <param name="fullname"></param>
        /// <returns></returns>
        public static IType FindType(string fullname)
        {
            var t = Assembly.FindType(fullname);
            if (t == null)
                throw new InvalidOperationException(
                    string.Format("Unable to find {0}. Invalid corlib.", fullname));
            return t;
        }

        public static IGenericType FindGenericType(string fullname)
        {
            foreach (var type in Assembly.Types)
            {
                var gt = type as IGenericType;
                if (gt == null) continue;
                if (gt.FullName == fullname)
                    return gt;
            }
            throw new InvalidOperationException(
                string.Format("Unable to find {0}. Invalid corlib.", fullname));
        }

        /// <summary>
        /// Provides type properties for types from corlib
        /// </summary>
        public static class Types
        {
            /// <summary>
            /// Returns System.Reflection.Assembly type.
            /// </summary>
            public static IType Assembly
            {
                get { return FindType("System.Reflection.Assembly"); }
            }

            /// <summary>
            /// Returns System.Console type.
            /// </summary>
            public static IType Console
            {
                get { return FindType("System.Console"); }
            }

            /// <summary>
            /// Return System.NullReferenceException
            /// </summary>
            public static IType NullReferenceException
            {
                get { return FindType("System.NullReferenceException"); }
            }

            /// <summary>
            /// Returns System.InvalidCastException type
            /// </summary>
            public static IType InvalidCastException
            {
                get { return FindType("System.InvalidCastException"); }
            }

            public static IType NotImplementedException
            {
                get { return FindType("System.NotImplementedException"); }
            }

            public static IType NotSupportedException
            {
                get { return FindType("System.NotSupportedException"); }
            }

            public static IType ArgumentOutOfRangeException
            {
                get { return FindType("System.ArgumentOutOfRangeException"); }
            }

            public static IType ExecutionEngineException
            {
                get { return FindType("System.ExecutionEngineException"); }
            }

            public static IType TypeLoadException
            {
                get { return FindType("System.TypeLoadException"); }
            }

            /// <summary>
            /// Returns System.EnumInfo type
            /// </summary>
            public static IType EnumInfo
            {
                get { return FindType("System.EnumInfo"); }
            }

            /// <summary>
            /// Returns System.Reflection.FieldInfo type
            /// </summary>
            public static IType FieldInfo
            {
                get { return FindType("System.Reflection.FieldInfo"); }
            }

            /// <summary>
            /// Returns System.Reflection.MethoBase type
            /// </summary>
            public static IType MethodBase
            {
                get { return FindType("System.Reflection.MethodBase"); }
            }

            /// <summary>
            /// Returns System.Reflection.MethoInfo type
            /// </summary>
            public static IType MethodInfo
            {
                get { return FindType("System.Reflection.MethodInfo"); }
            }

            /// <summary>
            /// Returns System.Reflection.ParametrInfo type
            /// </summary>
            public static IType ParameterInfo
            {
                get { return FindType("System.Reflection.ParameterInfo"); }
            }


            /// <summary>
            /// Returns System.Reflection.PropertyInfo type
            /// </summary>
            public static IType PropertyInfo
            {
                get { return FindType("System.Reflection.PropertyInfo"); }
            }

            /// <summary>
            /// Returns System.Reflection.ConstructorInfo type
            /// </summary>
            public static IType ConstructorInfo
            {
                get { return FindType("System.Reflection.ConstructorInfo"); }
            }

            public static IType Convert
            {
                get { return FindType("System.Convert"); }
            }

            public static IType CharEnumerator
            {
                get { return FindType("System.CharEnumerator"); }
            }

            public static IType Environment
            {
                get { return FindType("System.Environment"); }
            }

            public static IType IEnumerable
            {
                get { return FindType(CLRNames.Types.IEnumerable); }
            }

            public static IGenericType IEnumerableT
            {
                get { return FindGenericType(CLRNames.Types.IEnumerableT); }
            }

            public static IType IEnumerator
            {
                get { return FindType(CLRNames.Types.IEnumerator); }
            }

            public static IType IDictionary
            {
                get { return FindType(CLRNames.Types.IDictionary); }
            }

            public static IGenericType IEnumeratorT
            {
                get { return FindGenericType(CLRNames.Types.IEnumeratorT); }
            }

            public static IGenericType ICollectionT
            {
                get { return FindGenericType(CLRNames.Types.ICollectionT); }
            }

            public static IGenericType IListT
            {
                get { return FindGenericType(CLRNames.Types.IListT); }
            }

            public static IGenericType NullableT
            {
                get { return FindGenericType(CLRNames.Types.NullableT); }
            }

            public static IGenericType ArrayEnumeratorT
            {
                get
                {
                    var arr = SystemTypes.Array;
                    foreach (var type in arr.Types)
                    {
                        var gt = type as IGenericType;
                        if (gt == null) continue;
                        if (gt.Name.StartsWith("Enumerator"))
                            return gt;
                    }
                    throw new InvalidOperationException(
                        "Unable to find Array.Enumerator<T>. Invalid corlib.");
                }
            }
        }
    }
    #endregion

    #region CorlibTypeCache
    enum CorlibTypeId
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

    enum GenericTypeId
    {
        ICollectionT,
        IEnumerableT,
        IEnumeratorT,
        IListT,
        NullableT,
        ArrayEnumeratorT
    }

    internal class CorlibTypeCache
    {
        private readonly LazyValue<IType>[] _types = new[]
            {
                new LazyValue<IType>(()=>Corlib.Types.NullReferenceException),
                new LazyValue<IType>(()=>Corlib.Types.InvalidCastException),
                new LazyValue<IType>(()=>Corlib.Types.NotImplementedException),
                new LazyValue<IType>(()=>Corlib.Types.NotSupportedException),
                new LazyValue<IType>(()=>Corlib.Types.ArgumentOutOfRangeException),
                new LazyValue<IType>(()=>Corlib.Types.ExecutionEngineException),
                new LazyValue<IType>(()=>Corlib.Types.TypeLoadException),
                new LazyValue<IType>(()=>Corlib.Types.EnumInfo),
                new LazyValue<IType>(()=>Corlib.Types.Environment),
                new LazyValue<IType>(()=>Corlib.Types.Assembly),
                new LazyValue<IType>(()=>Corlib.Types.CharEnumerator),
                new LazyValue<IType>(()=>Corlib.Types.Convert),
                new LazyValue<IType>(()=>Corlib.Types.FieldInfo),
                new LazyValue<IType>(()=>Corlib.Types.IEnumerable),
                new LazyValue<IType>(()=>Corlib.Types.IEnumerator),
                new LazyValue<IType>(()=>Corlib.Types.IDictionary),
                new LazyValue<IType>(()=>Corlib.Types.MethodBase),
                new LazyValue<IType>(()=>Corlib.Types.MethodInfo),
                new LazyValue<IType>(()=>Corlib.Types.ConstructorInfo),
                new LazyValue<IType>(()=>Corlib.Types.ParameterInfo),
                new LazyValue<IType>(()=>Corlib.Types.PropertyInfo),
                new LazyValue<IType>(()=>Corlib.Types.Console),
                new LazyValue<IType>(()=>Corlib.FindType("PageFX.CompilerUtils"))
            };

        private readonly LazyValue<IType>[] _generics = new[]
            {
                new LazyValue<IType>(()=>Corlib.Types.ICollectionT),
                new LazyValue<IType>(()=>Corlib.Types.IEnumerableT),
                new LazyValue<IType>(()=>Corlib.Types.IEnumeratorT),
                new LazyValue<IType>(()=>Corlib.Types.IListT),
                new LazyValue<IType>(()=>Corlib.Types.NullableT),
                new LazyValue<IType>(()=>Corlib.Types.ArrayEnumeratorT)
            };

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