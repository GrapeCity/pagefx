using System;

namespace DataDynamics.Compression 
{
	
	/// <summary>
	/// Represents errors specific to Zip file handling
	/// </summary>
	internal class ZipException : ApplicationException
	{
		/// <summary>
		/// Initializes a new instance of the ZipException class.
		/// </summary>
		public ZipException()
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the ZipException class with a specified error message.
		/// </summary>
		public ZipException(string msg) : base(msg)
		{
		}
	}
}
