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
        [PageFX.ABC]
        [PageFX.FP9]
        public String message;

        /// <summary>Contains the name of the Error object. By default, the value of this property is &quot;Error&quot;.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        public String name;

        public extern virtual int errorID
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Error(String message, int id);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Error(String message);

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
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual String getStackTrace();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Class arg0, uint arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Class arg0, uint arg1, object rest0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Class arg0, uint arg1, object rest0, object rest1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Class arg0, uint arg1, object rest0, object rest1, object rest2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Class arg0, uint arg1, object rest0, object rest1, object rest2, object rest3);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Class arg0, uint arg1, object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Class arg0, uint arg1, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Class arg0, uint arg1, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Class arg0, uint arg1, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Class arg0, uint arg1, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object throwError(Class arg0, uint arg1, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static String getErrorMessage(int arg0);
    }
}
