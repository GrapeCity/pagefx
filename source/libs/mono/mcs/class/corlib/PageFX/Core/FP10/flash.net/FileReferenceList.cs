using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    /// <summary>
    /// The FileReferenceList class provides a means to let users select one or more files for uploading.
    /// A FileReferenceList object represents a group of one or more local files on the user&apos;s disk as
    /// an array of FileReference objects. For detailed information and important considerations about
    /// FileReference objects and the FileReference class, which you use with FileReferenceList,
    /// see the FileReference class.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class FileReferenceList : flash.events.EventDispatcher
    {
        public extern virtual Avm.Array fileList
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [PageFX.Event("select")]
        public event flash.events.EventHandler select
        {
            add { }
            remove { }
        }

        [PageFX.Event("cancel")]
        public event flash.events.EventHandler cancel
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FileReferenceList();

        /// <summary>
        /// Displays a file-browsing dialog box that lets the
        /// user select one or more local files to upload. The dialog box is native to the user&apos;s
        /// operating system.
        /// When you call this method and the user successfully selects files,
        /// the fileList property of this FileReferenceList object is populated with
        /// an array of FileReference objects, one for each file that the user selects.
        /// Each subsequent time that the FileReferenceList.browse() method is called, the
        /// FileReferenceList.fileList property is reset to the file(s) that the
        /// user selects in the dialog box.
        /// Using the typeFilter parameter, you can determine which files
        /// the dialog box displays.Only one FileReference.browse(), FileReference.download(),
        /// or FileReferenceList.browse() session can be performed at a time
        /// on a FileReferenceList object
        /// (because only one dialog box can be opened at a time).
        /// </summary>
        /// <param name="arg0">
        /// (default = null)  An array of FileFilter instances used to filter the files that are
        /// displayed in the dialog box. If you omit this parameter, all files are displayed.
        /// For more information, see the FileFilter class.
        /// </param>
        /// <returns>
        /// Returns true if the parameters are valid and the file-browsing dialog box
        /// opens.
        /// </returns>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool browse(Avm.Array arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool browse();


    }
}
