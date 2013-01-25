using System;
using System.Runtime.CompilerServices;

namespace flash.utils
{
    /// <summary>
    /// The Dictionary class lets you create a dynamic collection of properties, which uses strict equality
    /// ( === ) for key comparison on non-primitive object keys. When an object is used as a key, the object&apos;s
    /// identity is used to look up the object, and not the value returned from calling toString()  on it.
    /// Primitive (built-in) objects, like Numbers, in a Dictionary collection behave in the same manner as they do when
    /// they are the property of a regular object.
    /// </summary>
    [PageFX.AbcInstance(230)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class Dictionary : Avm.Object
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Dictionary(bool weakKeys);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Dictionary();
    }
}
