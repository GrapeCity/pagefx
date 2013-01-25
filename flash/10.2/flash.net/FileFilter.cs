using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// The FileFilter class is used to indicate what files on the user&apos;s system are shown
    /// in the file-browsing dialog box that is displayed when the FileReference.browse()
    /// method, the FileReferenceList.browse()  method is called or a
    /// a browse method of a File, FileReference, or FileReferenceList object is called.
    /// FileFilter instances are passed as a value for the optional typeFilter  parameter to the method.
    /// If you use a FileFilter instance, extensions and file types that aren&apos;t specified in the FileFilter instance
    /// are filtered out; that is, they are not available to the user for selection.
    /// If no FileFilter object is passed to the method, all files are shown in the dialog box.
    /// </summary>
    [PageFX.AbcInstance(180)]
    [PageFX.ABC]
    [PageFX.FP9]
    public partial class FileFilter : Avm.Object
    {
        /// <summary>
        /// The description string for the filter. The description
        /// is visible to the user in the dialog box that opens
        /// when FileReference.browse()
        /// or FileReferenceList.browse() is called.
        /// The description string contains a string, such as
        /// &quot;Images (*.gif, *.jpg, *.png)&quot;, that can
        /// help instruct the user on what file types can be uploaded
        /// or downloaded. Note that the actual file types that are supported by
        /// this FileReference object are stored in the extension
        /// property.
        /// </summary>
        public extern virtual Avm.String description
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

        /// <summary>
        /// A list of file extensions. This list indicates the types of files
        /// that you want to show in the file-browsing dialog box. (The list
        /// is not visible to the user; the user sees only the value of the
        /// description property.) The extension property contains
        /// a semicolon-delimited list of Windows file extensions,
        /// with a wildcard (*) preceding each extension, as shown
        /// in the following string: &quot;*.jpg;*.gif;*.png&quot;.
        /// </summary>
        public extern virtual Avm.String extension
        {
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// A list of Macintosh file types. This list indicates the types of files
        /// that you want to show in the file-browsing dialog box. (This list
        /// itself is not visible to the user; the user sees only the value of the
        /// description property.) The macType property contains
        /// a semicolon-delimited list of Macintosh file types, as shown
        /// in the following string: &quot;JPEG;jp2_;GIFF&quot;.
        /// </summary>
        public extern virtual Avm.String macType
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FileFilter(Avm.String description, Avm.String extension, Avm.String macType);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FileFilter(Avm.String description, Avm.String extension);


    }
}
