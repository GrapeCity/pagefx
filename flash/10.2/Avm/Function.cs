using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// A function is the basic unit of code that can be invoked in ActionScript.
    /// Both user-defined and built-in functions in ActionScript are represented by Function objects,
    /// which are instances of the Function class.
    /// </summary>
    [PageFX.AbcInstance(2)]
    [PageFX.ABC]
    [PageFX.QName("Function")]
    [PageFX.FP9]
    public partial class Function : Avm.Object
    {
        public extern virtual object prototype
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int length
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Function();

        /// <summary>
        /// Invokes the function represented by a Function object. Every function in ActionScript
        /// is represented by a Function object, so all functions support this method.
        /// In almost all cases, the function call (()) operator can be used instead of this method.
        /// The function call operator produces code that is concise and readable. This method is primarily useful
        /// when the thisObject parameter of the function invocation needs to be explicitly controlled.
        /// Normally, if a function is invoked as a method of an object within the body of the function, thisObject
        /// is set to myObject, as shown in the following example:
        /// myObject.myMethod(1, 2, 3);
        /// In some situations, you might want thisObject to point somewhere else;
        /// for example, if a function must be invoked as a method of an object, but is not actually stored as a method
        /// of that object:
        /// myObject.myMethod.call(myOtherObject, 1, 2, 3);
        /// You can pass the value null for the thisObject parameter to invoke a function as a
        /// regular function and not as a method of an object. For example, the following function invocations are equivalent:
        /// Math.sin(Math.PI / 4)
        /// Math.sin.call(null, Math.PI / 4)
        /// Returns the value that the called function specifies as the return value.
        /// </summary>
        /// <param name="thisObject">An object that specifies the value of thisObject within the function body.</param>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.QName("call", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object call(object thisObject);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.QName("call", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call(object thisObject, object arg0);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.QName("call", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call(object thisObject, object arg0, object arg1);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.QName("call", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call(object thisObject, object arg0, object arg1, object arg2);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.QName("call", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call(object thisObject, object arg0, object arg1, object arg2, object arg3);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.QName("call", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call(object thisObject, object arg0, object arg1, object arg2, object arg3, object arg4);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.QName("call", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call(object thisObject, object arg0, object arg1, object arg2, object arg3, object arg4, object arg5);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.QName("call", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call(object thisObject, object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.QName("call", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call(object thisObject, object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.QName("call", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call(object thisObject, object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.QName("call", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call(object thisObject, object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9);

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.QName("call", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call();

        /// <summary>
        /// Specifies the value of thisObject to be used within any function that ActionScript calls.
        /// This method also specifies the parameters to be passed to any called function. Because apply()
        /// is a method of the Function class, it is also a method of every Function object in ActionScript.
        /// The parameters are specified as an Array object, unlike Function.call(), which specifies
        /// parameters as a comma-delimited list. This is often useful when the number of parameters to be passed is not
        /// known until the script actually executes.Returns the value that the called function specifies as the return value.
        /// </summary>
        /// <param name="thisObject">The object to which the function is applied.</param>
        /// <param name="argArray">(default = null)  An array whose elements are passed to the function as parameters.</param>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("apply", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object apply(object thisObject, object argArray);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("apply", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object apply(object thisObject);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("apply", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object apply();
    }
}
