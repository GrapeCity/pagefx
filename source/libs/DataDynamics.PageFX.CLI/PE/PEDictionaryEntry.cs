namespace DataDynamics.PE
{
	/// <summary>
	/// PE File dictionary entry
	/// </summary>
	public class PEDictionaryEntry
	{
		public const int ENTRY_EXPORT = 0;
		public const int ENTRY_IMPORT = 1;
		public const int ETRY_RESOURCE = 2;
		public const int ENTRY_EXCEPTION = 3;
		public const int ENTRY_SECURITY = 4;
		public const int ENTRY_BASERELOC = 5;
		public const int ENTRY_DEBUG = 6;
		public const int ENTRY_ARCHITECTURE = 7;
		public const int ENTRY_GLOBALPTR = 8;
		public const int ENTRY_TLS = 9;
		public const int ENTRY_LOAD_CONFIG = 10;
		public const int ENTRY_BOUND_IMPORT = 11;
		public const int ENTRY_IAT = 12;
		public const int DELAY_IMPORT = 13;
		public const int ENTRY_COM_DESCRIPTOR = 14;
		public const int ENTRY_UNKNOWN = 15;
	}
}
