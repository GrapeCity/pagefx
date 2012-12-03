//
// System.Collections.Specialized.NameValueCollection.cs
//
// Author:
//   Gleb Novodran
//
// (C) Ximian, Inc.  http://www.ximian.com
// Copyright (C) 2004-2005 Novell (http://www.novell.com)
//

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

#if NOT_PFX
using System.Runtime.Serialization;
#endif
using System.Text;

namespace System.Collections.Specialized{

	[Serializable]
	public class NameValueCollection : NameObjectCollectionBase
	{
		string[] cachedAllKeys = null;
		string[] cachedAll = null;

		//--------------------- Constructors -----------------------------

		public NameValueCollection () : base ()
		{
		}
		
		public NameValueCollection (int capacity) : base (capacity)
		{
		}
#if NET_2_0
        public NameValueCollection (NameValueCollection col) : base (( col == null ) ? null : col.EqualityComparer ,
                                                                (col == null) ? null : col.Comparer, 
                                                                (col == null) ? null : col.HashCodeProvider)
        {
            if (col==null)
                throw new ArgumentNullException ("col");		
            Add(col);
        }
#else
		public NameValueCollection (NameValueCollection col) : base ((col == null) ? null : col.HashCodeProvider ,
																(col == null) ? null : col.Comparer)																
		{
			if (col==null)
				throw new NullReferenceException ();		
			Add(col);
		}
#endif

#if NET_2_0
		[Obsolete ("Use NameValueCollection (IEqualityComparer)")]
#endif
		public NameValueCollection (IHashCodeProvider hashProvider, IComparer comparer)
			: base (hashProvider, comparer)
		{
			
		}

		public NameValueCollection (int capacity, NameValueCollection col)
			: base (capacity, (col == null) ? null : col.HashCodeProvider, 
					(col == null) ? null : col.Comparer)
		{
			Add (col);			
		}

#if NOT_PFX
protected NameValueCollection (SerializationInfo info, StreamingContext context)
			:base (info, context)
		{
			
		}

#endif
#if NET_2_0
		[Obsolete ("Use NameValueCollection (IEqualityComparer)")]
#endif
		public NameValueCollection (int capacity, IHashCodeProvider hashProvider, IComparer comparer)
			:base (capacity, hashProvider, comparer)
		{
			
		}

#if NET_2_0
		public NameValueCollection (IEqualityComparer equalityComparer)
			: base (equalityComparer)
		{
		}

		public NameValueCollection (int capacity, IEqualityComparer equalityComparer)
			: base (capacity, equalityComparer)
		{
		}
#endif

#if NOT_PFX
		public virtual string[] AllKeys 
		{
			get {
				if (cachedAllKeys == null)
					cachedAllKeys = BaseGetAllKeys ();
				return this.cachedAllKeys;
			}
		}

#endif		
		public string this [int index] 
		{
			get{
				return this.Get (index);
			}
		}
		
		public string this [string name] {
			get{
				return this.Get (name);
			}
			set{
				this.Set (name,value);
			}
		}
		
		public void Add (NameValueCollection c)
		{
#if NOT_PFX
if (this.IsReadOnly)
				throw new NotSupportedException ("Collection is read-only");
#endif
#if NET_2_0
			if (c == null)
				throw new ArgumentNullException ("c");
#endif
// make sense - but it's not the exception thrown
//				throw new ArgumentNullException ();
			
			InvalidateCachedArrays ();
			int max = c.Count;
			for (int i=0; i < max; i++){
				string key = c.GetKey (i);
				ArrayList new_values = (ArrayList) c.BaseGet (i);
				ArrayList values = (ArrayList) BaseGet (key);
				if (values != null && new_values != null)
					values.AddRange (new_values);
				else if (new_values != null)
					values = new ArrayList (new_values);
				BaseSet (key, values);
			}
		}

		/// in SDK doc: If the same value already exists under the same key in the collection, 
		/// it just adds one more value in other words after
		/// <code>
		/// NameValueCollection nvc;
		/// nvc.Add ("LAZY","BASTARD")
		/// nvc.Add ("LAZY","BASTARD")
		/// </code>
		/// nvc.Get ("LAZY") will be "BASTARD,BASTARD" instead of "BASTARD"

		public virtual void Add (string name, string val)
		{
			if (this.IsReadOnly)
				throw new NotSupportedException ("Collection is read-only");
			
			InvalidateCachedArrays ();
			ArrayList values = (ArrayList)BaseGet (name);
			if (values == null){
				values = new ArrayList ();
				if (val != null)
					values.Add (val);
				BaseAdd (name, values);
			}
			else {
				if (val != null)
					values.Add (val);
			}

		}

#if NET_2_0
		public virtual void Clear ()
#else
		public void Clear ()
#endif
		{
			if (this.IsReadOnly)
				throw new NotSupportedException ("Collection is read-only");
			InvalidateCachedArrays ();
			BaseClear ();
		}

		public void CopyTo (Array dest, int index)
		{
			if (dest == null)
				throw new ArgumentNullException ("dest", "Null argument - dest");
			if (index < 0)
				throw new ArgumentOutOfRangeException ("index", "index is less than 0");
			if (dest.Rank > 1)
				throw new ArgumentException ("dest", "multidim");

			if (cachedAll == null)
				RefreshCachedAll ();
#if NET_2_0
			try {
#endif			
			cachedAll.CopyTo (dest, index);
#if NET_2_0
		        }
		        catch (ArrayTypeMismatchException)  
		        {
		        	throw new InvalidCastException();
		        }
#endif		        
		}

		private void RefreshCachedAll ()
		{
			this.cachedAll = null;
			int max = this.Count;
			cachedAll = new string [max];
			for (int i = 0; i < max; i++)
				cachedAll [i] = this.Get (i);
		}
		
		public virtual string Get (int index)
		{
			ArrayList values = (ArrayList)BaseGet (index);
			// if index is out of range BaseGet throws an ArgumentOutOfRangeException

			return AsSingleString (values);
		}
		
		public virtual string Get (string name)
		{
			ArrayList values = (ArrayList)BaseGet (name);
			return AsSingleString (values);
		}

		private static string AsSingleString (ArrayList values)
		{
			const char separator = ',';
			
			if (values == null)
				return null;
			int max = values.Count;
			
			if (max == 0)
				return null;
			//TODO: reimplement this
			StringBuilder sb = new StringBuilder ((string)values [0]);
			for (int i = 1; i < max; i++){
				sb.Append (separator);
				sb.Append (values [i]);
			}

			return sb.ToString ();			
		}
		
		
		public virtual string GetKey (int index)
		{
			return BaseGetKey (index);
		}
		
		
		public virtual string[] GetValues (int index)
		{
			ArrayList values = (ArrayList)BaseGet (index);
			
			return AsStringArray (values);
		}
		
		
		public virtual string[] GetValues (string name)
		{
			ArrayList values = (ArrayList)BaseGet (name);
			
			return AsStringArray (values);
		}
		
		private static string[] AsStringArray (ArrayList values)
		{
			if (values == null)
				return null;
			int max = values.Count;//get_Count ();
			if (max == 0)
				return null;
			
			string[] valArray = new string[max];
			values.CopyTo (valArray);
			return valArray;
		}

#if NOT_PFX
		public bool HasKeys ()
		{
			return BaseHasKeys ();
		}
		
		public virtual void Remove (string name)
		{
			if (this.IsReadOnly)
				throw new NotSupportedException ("Collection is read-only");
			InvalidateCachedArrays ();
			BaseRemove (name);
			
		}

#endif		
		public virtual void Set (string name, string value)
		{
			if (this.IsReadOnly)
				throw new NotSupportedException ("Collection is read-only");

			InvalidateCachedArrays ();
			
			ArrayList values = new ArrayList ();
			if (value != null) {
				values.Add (value);
				BaseSet (name,values);
			}
			else {
				// remove all entries
				BaseSet (name, null);
			}
		}
		
		protected void InvalidateCachedArrays ()
		{
			cachedAllKeys = null;
			cachedAll = null;
		}

	}
}
