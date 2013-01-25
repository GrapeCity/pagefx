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
    public partial class Class : Avm.Object
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
    }
}
