using System;
using System.Runtime.CompilerServices;

namespace flash.external
{
    /// <summary>
    /// The ExternalInterface class is the External API, an application programming interface
    /// that enables straightforward communication between ActionScript and the Flash Player
    /// container; for example, an HTML page with JavaScript, or a desktop application with
    /// Flash Player embedded. Use of ExternalInterface is recommended for all JavaScript-ActionScript
    /// communication.
    /// </summary>
    [PageFX.AbcInstance(69)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class ExternalInterface : Avm.Object
    {
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static bool marshallExceptions;

        /// <summary>
        /// Indicates whether this player is in a container that offers an external interface.
        /// If the external interface is available, this property is true; otherwise,
        /// it is false.
        /// Note: When using the External API with HTML, you should always check that
        /// the HTML has fully loaded before attempting to call any JavaScript methods.
        /// </summary>
        public extern static bool available
        {
            [PageFX.AbcClassTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>
        /// Returns the id attribute of the &lt;object&gt; tag in
        /// Internet Explorer, or the name attribute of the &lt;embed&gt;
        /// tag in Netscape.
        /// </summary>
        public extern static Avm.String objectID
        {
            [PageFX.AbcClassTrait(6)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern ExternalInterface();

        /// <summary>
        /// Registers an ActionScript method as callable from the container. After a successful
        /// invocation of addCallBack(), the registered function in Flash Player
        /// can be called by JavaScript or ActiveX code in the container.
        /// Security note: For local content running in a browser, calls to the
        /// ExternalInterface.addCallback() method will only work if the SWF file
        /// and the containing web page are in the local-trusted security sandbox. For more
        /// information, see the following:The security
        /// chapter in the Programming ActionScript 3.0 book and the latest comments
        /// on LiveDocsThe Flash Player
        /// 9 Security white paper
        /// </summary>
        /// <param name="functionName">The name by which the container can invoke the function.</param>
        /// <param name="closure">
        /// The function closure to invoke. This could be a free-standing function,
        /// or it could be a method closure referencing a method of an object instance. By passing
        /// a method closure, the callback can actually be directed at a method of a particular
        /// object instance.
        /// </param>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void addCallback(Avm.String functionName, Avm.Function closure);

        /// <summary>
        /// Calls a function exposed by the Flash Player container, passing zero or more arguments.
        /// If the desired function is not available, the call returns null; otherwise
        /// it returns the value provided by the function. Recursion is not permitted;
        /// a recursive call produces a null response.
        /// If the container is an HTML page, this method invokes a JavaScript function in a
        /// &lt;script&gt; element.
        /// If the container is some other ActiveX container, this method fires the FlashCall
        /// ActiveX event with the specified name, and the container processes the event.
        /// If the container is hosting the Netscape plug-in, you can either write custom support
        /// for the new NPRuntime interface or embed an HTML control and embed Flash Player
        /// within the HTML control. If you embed an HTML control, you can communicate with
        /// Flash Player through a JavaScript interface that talks to the native container application.Security note: For local content running in a browser, calls to the
        /// ExternalInterface.call() method are only permitted if the SWF file
        /// and the containing web page (if there is one) are in the local-trusted security
        /// sandbox. Also, you can prevent a SWF file from using this method by setting the
        /// allowNetworking parameter of the the object and embed
        /// tags in the HTML page that contains the SWF content. For more information, see the
        /// following:The security
        /// chapter in the Programming ActionScript 3.0 book and the latest comments
        /// on LiveDocsThe Flash Player
        /// 9 Security white paper
        /// </summary>
        /// <param name="name">The name of the function to call in the container.</param>
        /// <returns>
        /// The response received
        /// from the container. If the call failed -- for example, if there is no such function
        /// in the container, or the interface is not available, or a recursion occurred, or
        /// there is a security issue, null is returned and an error is thrown.
        /// </returns>
        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object call(Avm.String name);

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object call(Avm.String name, object arg0);

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object call(Avm.String name, object arg0, object arg1);

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object call(Avm.String name, object arg0, object arg1, object arg2);

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object call(Avm.String name, object arg0, object arg1, object arg2, object arg3);

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object call(Avm.String name, object arg0, object arg1, object arg2, object arg3, object arg4);

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object call(Avm.String name, object arg0, object arg1, object arg2, object arg3, object arg4, object arg5);

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object call(Avm.String name, object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6);

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object call(Avm.String name, object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7);

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object call(Avm.String name, object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8);

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static object call(Avm.String name, object arg0, object arg1, object arg2, object arg3, object arg4, object arg5, object arg6, object arg7, object arg8, object arg9);


    }
}
