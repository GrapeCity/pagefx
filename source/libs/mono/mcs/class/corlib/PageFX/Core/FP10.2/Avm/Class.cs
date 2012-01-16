using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// A Class object is created for each class definition in a program. Every Class object is an instance
    /// of the Class class. The Class object contains the static properties and methods of the class. The
    /// class object creates instances of the class when invoked using the new  operator.
    /// </summary>
    [PageFX.AbcInstance(1)]
    [PageFX.ABC]
    [PageFX.QName("Class")]
    [PageFX.FP9]
    public class Class : Avm.Object
    {
        public extern object prototype
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Class();



        #region Custom Members
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern Class Find(string ns, string name);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public static extern Class Find(Namespace ns, string name);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object CreateInstance();
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object CreateInstance(object arg1);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object CreateInstance(object arg1, object arg2);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object CreateInstance(object arg1, object arg2, object arg3);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object CreateInstance(object arg1, object arg2, object arg3, object arg4);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object CreateInstance(object arg1, object arg2, object arg3, object arg4, object arg5);
        #endregion



    }
}
