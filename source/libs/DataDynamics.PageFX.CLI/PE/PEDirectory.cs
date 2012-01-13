using System.IO;

namespace DataDynamics.PE
{
	/// <summary>
	/// Base class for PE directory
	/// </summary>
	public abstract class PEDirectory
	{
		/// <summary>
		/// Read directory
		/// </summary>
		/// <param name="reader">reader</param>
		/// <param name="size">Size of directory</param>
		public abstract void Read(BufferedBinaryReader reader, int size);
	}
}
