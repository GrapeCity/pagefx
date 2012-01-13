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
    [PageFX.ABC]
    [PageFX.FP9]
    public class FileFilter : Avm.Object
    {
        public extern virtual Avm.String macType
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String description
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String extension
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FileFilter(Avm.String arg0, Avm.String arg1, Avm.String arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FileFilter(Avm.String arg0, Avm.String arg1);


    }
}
