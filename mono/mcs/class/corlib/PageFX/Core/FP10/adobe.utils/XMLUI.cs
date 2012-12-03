using System;
using System.Runtime.CompilerServices;

namespace adobe.utils
{
    /// <summary>
    /// The XMLUI class enables communication with SWF files that are used as custom user interfaces for the Flash
    /// authoring tool&apos;s extensibility features.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class XMLUI : Avm.Object
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern XMLUI();

        /// <summary>
        /// Makes the current XMLUI dialog box close with an &quot;accept&quot; state. This is identical to the user
        /// clicking the OK button.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void accept();

        /// <summary>
        /// Makes the current XMLUI dialog box close with a &quot;cancel&quot; state. This is identical to
        /// the user clicking the Cancel button.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void cancel();

        /// <summary>Retrieves the value of the specified property of the current XMLUI dialog box.</summary>
        /// <param name="arg0">The name of the XMLUI property to retrieve.</param>
        /// <returns>The value of the property.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String getProperty(Avm.String arg0);

        /// <summary>Modifies the value of the specified property of the current XMLUI dialog.</summary>
        /// <param name="arg0">The name of the XMLUI property to modify.</param>
        /// <param name="arg1">The value to which the specified property will be set.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void setProperty(Avm.String arg0, Avm.String arg1);
    }
}
