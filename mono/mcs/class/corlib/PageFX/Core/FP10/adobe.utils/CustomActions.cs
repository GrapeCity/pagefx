using System;
using System.Runtime.CompilerServices;

namespace adobe.utils
{
    /// <summary>
    /// The methods of the CustomActions class allow a SWF file playing in the Flash authoring
    /// tool to manage any custom actions that are registered with the authoring tool. A SWF
    /// file can install and uninstall custom actions, retrieve the XML definition of a custom
    /// action, and retrieve the list of registered custom actions.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class CustomActions : Avm.Object
    {
        public extern static Avm.Array actionsList
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern CustomActions();

        /// <summary>
        /// Installs a new custom action XML definition file indicated by the
        /// name parameter. The contents of the file is specified
        /// by the string data.
        /// The name of the definition file must be a simple filename, without the .xml
        /// file extension, and without any directory separators (&apos;:&apos;, &apos;/&apos; or
        /// &apos;\&apos;). If a custom actions file already exists with the name
        /// name, it is overwritten.If the Configuration/ActionsPanel/CustomActions directory does not exist when
        /// this method is invoked, the directory is created.
        /// </summary>
        /// <param name="arg0">The name of the custom action definition to install.</param>
        /// <param name="arg1">The text of the XML definition to install.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void installActions(Avm.String arg0, Avm.String arg1);

        /// <summary>
        /// Removes the Custom Actions XML definition file named name.
        /// The name of the definition file must be a simple filename, without the .xml file extension, and without any directory separators (&apos;:&apos;, &apos;/&apos; or &apos;\&apos;).
        /// </summary>
        /// <param name="arg0">The name of the custom action definition to uninstall.</param>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void uninstallActions(Avm.String arg0);

        /// <summary>
        /// Reads the contents of the custom action XML definition file named name.
        /// The name of the definition file must be a simple filename, without the .xml file extension, and without any directory separators (&apos;:&apos;, &apos;/&apos; or &apos;\&apos;). If the definition file specified by the name cannot be found, a value of undefined is returned. If the custom action XML definition specified by the name parameter is located, it is read in its entirety and returned as a string.
        /// </summary>
        /// <param name="arg0">The name of the custom action definition to retrieve.</param>
        /// <returns>If the custom action XML definition is located, returns a string; otherwise, returns undefined.</returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String getActions(Avm.String arg0);
    }
}
