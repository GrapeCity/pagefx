using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// The Error class contains information about an error that occurred in a script. In developing ActionScript 3.0 applications,
    /// when you run your compiled code in the debugger version of Flash Player, a dialog box displays exceptions of type Error,
    /// or of a subclass, to help you troubleshoot the code.
    /// You create an Error object by using the Error  constructor function.
    /// Typically, you throw a new Error object from within a try
    /// code block that is caught by a catch  or finally  code block.
    /// </summary>
    [PageFX.AbcInstance(35)]
    [PageFX.ABC]
    [PageFX.QName("Error")]
    [PageFX.FP9]
    public class Error : System.Exception
    {
        /// <summary>
        /// Contains the message associated with the Error object. By default, the value of this property
        /// is &quot;Error&quot;. You can specify a message property when you create an
        /// Error object by passing the error string to the Error constructor function.
        /// </summary>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public object message;

        /// <summary>Contains the name of the Error object. By default, the value of this property is &quot;Error&quot;.</summary>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public object name;

        /// <summary>
        /// Contains the reference number associated with the specific error message. For a custom Error object,
        /// this number is the value from the id parameter supplied in the constructor.
        /// </summary>
        public extern virtual int errorID
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Error(Avm.String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Error(Avm.String message);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Error();

        /// <summary>
        /// Returns the call stack for an error as a string at the time of the error&apos;s construction (for the debugger version
        /// of Flash Player only). As shown in the following example, the first line of the return value is the
        /// string representation of the exception object, followed by the stack trace elements:
        /// TypeError: null cannot be converted to an object
        /// at com.xyz.OrderEntry.retrieveData(OrderEntry.as:995)
        /// at com.xyz.OrderEntry.init(OrderEntry.as:200)
        /// at com.xyz.OrderEntry.$construct(OrderEntry.as:148)
        /// </summary>
        /// <returns>A string representation of the call stack.</returns>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String getStackTrace();

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String getErrorMessage(int index);

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Avm.Class type, uint index);

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Avm.Class type, uint index, object rest0);

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Avm.Class type, uint index, object rest0, object rest1);

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Avm.Class type, uint index, object rest0, object rest1, object rest2);

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Avm.Class type, uint index, object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Avm.Class type, uint index, object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Avm.Class type, uint index, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Avm.Class type, uint index, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Avm.Class type, uint index, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Avm.Class type, uint index, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Avm.Class type, uint index, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);
    }
}
