namespace DataDynamics.PageFX.Common
{
	/// <summary>
	/// Predefined names used in the CLR.
	/// </summary>
	public static class CLRNames
	{
		/// <summary>
		/// name of constructors. 
		/// </summary>
		public const string Constructor = ".ctor";

		/// <summary>
		/// name of static constructors. 
		/// </summary>
		public const string StaticConstructor = ".cctor";

		/// <summary>
		/// name used for pointer types 
		/// </summary>
		public const string Ptr  = "*";

        /// <summary>
        /// name used for ref param types 
        /// </summary>
        public const string Ref = "&";

	    public static class Array
        {
	        public const string Getter = "Get";
	        public const string Setter = "Set";
	        public const string Address = "Address";
        }

        public static class Types
        {
            public const string NullableT = "System.Nullable`1";
            public const string IEnumerable = "System.Collections.IEnumerable";
            public const string IEnumerableT = "System.Collections.Generic.IEnumerable`1";
            public const string IEnumerator = "System.Collections.IEnumerator";
            public const string IEnumeratorT = "System.Collections.Generic.IEnumerator`1";
            public const string ICollection = "System.Collections.ICollection";
            public const string ICollectionT = "System.Collections.Generic.ICollection`1";
            public const string IList = "System.Collections.IList";
            public const string IListT = "System.Collections.Generic.IList`1";
            public const string IDictionary = "System.Collections.IDictionary";
            public const string IDictionaryT = "System.Collections.Generic.IDictionary`2";
        }
	}
}