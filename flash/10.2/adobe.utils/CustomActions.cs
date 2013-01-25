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
    [PageFX.AbcInstance(98)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class CustomActions : Avm.Object
    {
        /// <summary>
        /// Returns an Array object containing the names of all the custom actions that are registered
        /// with the Flash authoring tool. The elements of the array are simple names, without the .xml file
        /// extension, and without any directory separators (for example, &quot;:&quot;, &quot;/&quot;,
        /// or &quot;\&quot;). If there are no registered custom actions, actionsList()
        /// returns a zero-length array. If an error occurs, actionsList() returns the value
        /// undefined.
        /// </summary>
        public extern static Avm.Array actionsList
        {
            [PageFX.AbcClassTrait(2)]
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
        /// <param name="name">The name of the custom action definition to install.</param>
        /// <param name="data">The text of the XML definition to install.</param>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void installActions(Avm.String name, Avm.String data);

        /// <summary>
        /// Removes the Custom Actions XML definition file named name.
        /// The name of the definition file must be a simple filename, without the .xml file extension, and without any directory separators (&apos;:&apos;, &apos;/&apos; or &apos;\&apos;).
        /// </summary>
        /// <param name="name">The name of the custom action definition to uninstall.</param>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void uninstallActions(Avm.String name);

        /// <summary>
        /// Reads the contents of the custom action XML definition file named name.
        /// The name of the definition file must be a simple filename, without the .xml file extension, and without any directory separators (&apos;:&apos;, &apos;/&apos; or &apos;\&apos;). If the definition file specified by the name cannot be found, a value of undefined is returned. If the custom action XML definition specified by the name parameter is located, it is read in its entirety and returned as a string.
        /// </summary>
        /// <param name="name">The name of the custom action definition to retrieve.</param>
        /// <returns>If the custom action XML definition is located, returns a string; otherwise, returns undefined.</returns>
        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String getActions(Avm.String name);
    }
}
