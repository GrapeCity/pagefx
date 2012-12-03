using System.IO;

namespace System.Drawing.Text {

	public sealed class PrivateFontCollection : FontCollection {

		// constructors

		public PrivateFontCollection ()
		{
            throw new NotImplementedException();
		}
		
		// methods
		public void AddFontFile (string filename) 
		{
			if (filename == null)
				throw new ArgumentNullException ("filename");

			// this ensure the filename is valid (or throw the correct exception)
			string fname = Path.GetFullPath (filename);

            //if (!File.Exists (fname))
            //    throw new FileNotFoundException ();

			// note: MS throw the same exception FileNotFoundException if the file exists but isn't a valid font file
            //Status status = GDIPlus.GdipPrivateAddFontFile (nativeFontCollection, fname);
            //GDIPlus.CheckStatus (status);			
            throw new NotImplementedException();
		}
	}
}
