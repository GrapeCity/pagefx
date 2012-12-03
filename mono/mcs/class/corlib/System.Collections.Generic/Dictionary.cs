//
// System.Collections.Generic.Dictionary
//
// Authors:
//	Sureshkumar T (tsureshkumar@novell.com)
//	Marek Safar (marek.safar@seznam.cz) (stubs)
//	Ankit Jain (radical@corewars.org)
//	David Waite (mass@akuma.org)
//	Juraj Skripsky (js@hotfeet.ch)
//
//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
// Copyright (C) 2005 David Waite
// Copyright (C) 2007 HotFeet GmbH (http://www.hotfeet.ch)
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

#if NET_2_0

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
//using System.Security.Permissions;
using System.Runtime.InteropServices;

namespace System.Collections.Generic {
	[ComVisible(true)]
	[Serializable]
	public class Dictionary<TKey, TValue> : IDictionary<TKey, TValue>,
		IDictionary,
		ICollection,
		ICollection<KeyValuePair<TKey, TValue>>,
		IEnumerable<KeyValuePair<TKey, TValue>>
#if NOT_PFX
,ISerializable
#endif
	{
		// The implementation of this class uses a hash table and linked lists
		// (see: http://msdn2.microsoft.com/en-us/library/ms379571(VS.80).aspx).
		//		
		// We use a kind of "mini-heap" instead of reference-based linked lists:
		// "keySlots" and "valueSlots" is the heap itself, it stores the data
		// "linkSlots" contains information about how the slots in the heap
		//             are connected into linked lists
		// "touchedSlots" and "emptySlot" manage the free space in the heap 

		const int INITIAL_SIZE = 10;
		const float DEFAULT_LOAD_FACTOR = (90f / 100);
		const int NO_SLOT = -1;
		
		private struct Link {
			public int HashCode;
			public int Next;
		}

		// The hash table contains indices into the linkSlots array
		int [] table;
		
		// All (key,value) pairs are chained into linked lists. The connection
		// information is stored in "linkSlots" along with the key's hash code
		// (for performance reasons).
		// TODO: get rid of the hash code in Link (this depends on a few
		// JIT-compiler optimizations)
		// Every link in "linkSlots" corresponds to the (key,value) pair
		// in "keySlots"/"valueSlots" with the same index.
		Link [] linkSlots;
		TKey [] keySlots;
		TValue [] valueSlots;

		// The number of slots in "linkSlots" and "keySlots"/"valueSlots" that
		// are in use (i.e. filled with data) or have been used and marked as
		// "empty" later on.
		int touchedSlots;
		
		// The index of the first slot in the "empty slots chain".
		// "Remove()" prepends the cleared slots to the empty chain.
		// "Add()" fills the first slot in the empty slots chain with the
		// added item (or increases "touchedSlots" if the chain itself is empty).
		int emptySlot;

		// The number of (key,value) pairs in this dictionary.
		int count;
		
		// The number of (key,value) pairs the dictionary can hold without
		// resizing the hash table and the slots arrays.
		int threshold;

		IEqualityComparer<TKey> hcp;
		SerializationInfo serialization_info;

		// The number of changes made to this dictionary. Used by enumerators
		// to detect changes and invalidate themselves.
		int generation;

		public int Count {
			get { return count; }
		}

		public TValue this [TKey key] {
			get {
				if (key == null)
					throw new ArgumentNullException ("key");

				// get first item of linked list corresponding to given key
				int hashCode = hcp.GetHashCode (key);
				int cur = table [(hashCode & int.MaxValue) % table.Length] - 1;
				
				// walk linked list until right slot is found or end is reached 
				while (cur != NO_SLOT) {
					// The ordering is important for compatibility with MS and strange
					// Object.Equals () implementations
					if (linkSlots [cur].HashCode == hashCode && hcp.Equals (keySlots [cur], key))
						return valueSlots [cur];
					cur = linkSlots [cur].Next;
				}
				throw new KeyNotFoundException ();
			}

			set {
				if (key == null)
					throw new ArgumentNullException ("key");
			
				// get first item of linked list corresponding to given key
				int hashCode = hcp.GetHashCode (key);
				int index = (hashCode & int.MaxValue) % table.Length;
				int cur = table [index] - 1;

				// walk linked list until right slot (and its predecessor) is
				// found or end is reached
				int prev = NO_SLOT;
				if (cur != NO_SLOT) {
					do {
						// The ordering is important for compatibility with MS and strange
						// Object.Equals () implementations
						if (linkSlots [cur].HashCode == hashCode && hcp.Equals (keySlots [cur], key))
							break;
						prev = cur;
						cur = linkSlots [cur].Next;
					} while (cur != NO_SLOT);
				}

				// is there no slot for the given key yet? 				
				if (cur == NO_SLOT) {
					// there is no existing slot for the given key,
					// allocate one and prepend it to its corresponding linked
					// list
				
					if (++count > threshold) {
						Resize ();
						index = (hashCode & int.MaxValue) % table.Length;
					}

					// find an empty slot
					cur = emptySlot;
					if (cur == NO_SLOT)
						cur = touchedSlots++;
					else 
						emptySlot = linkSlots [cur].Next;
					
					// prepend the added item to its linked list,
					// update the hash table
					linkSlots [cur].Next = table [index] - 1;
					table [index] = cur + 1;
				} else {
					// we already have a slot for the given key,
					// update the existing slot		

					// if the slot is not at the front of its linked list,
					// we move it there
					if (prev != NO_SLOT) {
						linkSlots [prev].Next = linkSlots [cur].Next;
						linkSlots [cur].Next = table [index] - 1;
						table [index] = cur + 1;
					}
				}

				// store the hash code of the new item and
				// the item's data itself
				linkSlots [cur].HashCode = hashCode;
				keySlots [cur] = key;
				valueSlots [cur] = value;
				
				generation++;
			}
		}

		public Dictionary ()
		{
			Init (INITIAL_SIZE, null);
		}

		public Dictionary (IEqualityComparer<TKey> comparer)
		{
			Init (INITIAL_SIZE, comparer);
		}

		public Dictionary (IDictionary<TKey, TValue> dictionary)
			: this (dictionary, null)
		{
		}

		public Dictionary (int capacity)
		{
			Init (capacity, null);
		}

		public Dictionary (IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer)
		{
			if (dictionary == null)
				throw new ArgumentNullException ("dictionary");
			int capacity = dictionary.Count;
			Init (capacity, comparer);
			foreach (KeyValuePair<TKey, TValue> entry in dictionary)
				this.Add (entry.Key, entry.Value);
		}

		public Dictionary (int capacity, IEqualityComparer<TKey> comparer)
		{
			Init (capacity, comparer);
		}

#if NOT_PFX
protected Dictionary (SerializationInfo info, StreamingContext context)
		{
			serialization_info = info;
		}
#endif

		private void Init (int capacity, IEqualityComparer<TKey> hcp)
		{
			if (capacity < 0)
				throw new ArgumentOutOfRangeException ("capacity");
			this.hcp = (hcp != null) ? hcp : EqualityComparer<TKey>.Default;
			if (capacity == 0)
				capacity = INITIAL_SIZE;

			/* Modify capacity so 'capacity' elements can be added without resizing */
			capacity = (int)(capacity / DEFAULT_LOAD_FACTOR) + 1;
			
			InitArrays (capacity);
			generation = 0;
		}
		
		private void InitArrays (int size) {
			table = new int [size];

			linkSlots = new Link [size];
			emptySlot = NO_SLOT;

			keySlots = new TKey [size];
			valueSlots = new TValue [size];
			touchedSlots = 0;

			threshold = (int)(table.Length * DEFAULT_LOAD_FACTOR);
			if (threshold == 0 && table.Length > 0)
				threshold = 1;
		}
		
		void CopyTo (KeyValuePair<TKey, TValue> [] array, int index)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			if (index < 0)
				throw new ArgumentOutOfRangeException ("index");
			// we want no exception for index==array.Length && Count == 0
			if (index > array.Length)
				throw new ArgumentException ("index larger than largest valid index of array");
			if (array.Length - index < Count)
				throw new ArgumentException ("Destination array cannot hold the requested elements!");

			for (int i = 0; i < table.Length; i++) {
				int cur = table [i] - 1;
				while (cur != NO_SLOT) {
					array [index++] = new KeyValuePair<TKey, TValue> (keySlots [cur], valueSlots [cur]);
					cur = linkSlots [cur].Next;
				}
			}
		}
		
		private void Resize ()
		{
			// From the SDK docs:
			//	 Hashtable is automatically increased
			//	 to the smallest prime number that is larger
			//	 than twice the current number of Hashtable buckets
			int newSize = Hashtable.ToPrime ((table.Length << 1) | 1);

			// allocate new hash table and link slots array
			int [] newTable = new int [newSize];
			Link [] newLinkSlots = new Link [newSize];

			for (int i = 0; i < table.Length; i++) {
				int cur = table [i] - 1;
				while (cur != NO_SLOT) {
					int hashCode = newLinkSlots [cur].HashCode = hcp.GetHashCode(keySlots [cur]);
					int index = (hashCode & int.MaxValue) % newSize;
					newLinkSlots [cur].Next = newTable [index] - 1;
					newTable [index] = cur + 1;
					cur = linkSlots [cur].Next;
				}
			}
			table = newTable;
			linkSlots = newLinkSlots;

			// allocate new data slots, copy data
			TKey [] newKeySlots = new TKey [newSize];
			TValue [] newValueSlots = new TValue [newSize];
			Array.Copy (keySlots, 0, newKeySlots, 0, touchedSlots);
			Array.Copy (valueSlots, 0, newValueSlots, 0, touchedSlots);
			keySlots = newKeySlots;
			valueSlots = newValueSlots;			

			threshold = (int)(newSize * DEFAULT_LOAD_FACTOR);
		}
		
		public void Add (TKey key, TValue value)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			// get first item of linked list corresponding to given key
			int hashCode = hcp.GetHashCode (key);
			int index = (hashCode & int.MaxValue) % table.Length;
			int cur = table [index] - 1;

			// walk linked list until end is reached (throw an exception if a
			// existing slot is found having an equivalent key)
			while (cur != NO_SLOT) {
				// The ordering is important for compatibility with MS and strange
				// Object.Equals () implementations
				if (linkSlots [cur].HashCode == hashCode && hcp.Equals (keySlots [cur], key))
					throw new ArgumentException ("An element with the same key already exists in the dictionary.");
				cur = linkSlots [cur].Next;
			}

			if (++count > threshold) {
				Resize ();
				index = (hashCode & int.MaxValue) % table.Length;
			}
			
			// find an empty slot
			cur = emptySlot;
			if (cur == NO_SLOT)
				cur = touchedSlots++;
			else 
				emptySlot = linkSlots [cur].Next;

			// store the hash code of the added item,
			// prepend the added item to its linked list,
			// update the hash table
			linkSlots [cur].HashCode = hashCode;
			linkSlots [cur].Next = table [index] - 1;
			table [index] = cur + 1;

			// store item's data 
			keySlots [cur] = key;
			valueSlots [cur] = value;

			generation++;
		}
		
		public IEqualityComparer<TKey> Comparer {
			get { return hcp; }
		}

		public void Clear ()
		{
			count = 0;
			// clear the hash table
			Array.Clear(table, 0, table.Length);

			// empty the "empty slots chain"
			emptySlot = NO_SLOT;
			
			touchedSlots = 0;
			generation++;
		}

		public bool ContainsKey (TKey key)
		{
			// get first item of linked list corresponding to given key
			int hashCode = hcp.GetHashCode (key);
			int cur = table [(hashCode & int.MaxValue) % table.Length] - 1;
			
			// walk linked list until right slot is found or end is reached
			while (cur != NO_SLOT) {
				// The ordering is important for compatibility with MS and strange
				// Object.Equals () implementations
				if (linkSlots [cur].HashCode == hashCode && hcp.Equals (keySlots [cur], key))
					return true;
				cur = linkSlots [cur].Next;
			}

			return false;
		}

		public bool ContainsValue (TValue value)
		{
			IEqualityComparer<TValue> cmp = EqualityComparer<TValue>.Default;

			for (int i = 0; i < table.Length; i++) {
				int cur = table [i] - 1;
				while (cur != NO_SLOT) {
					if (cmp.Equals (valueSlots [cur], value))
						return true;
					cur = linkSlots [cur].Next;
				}
			}
			return false;
        }
        
        #region Serialization stuff turned off
#if NOT_PFX
[SecurityPermission (SecurityAction.LinkDemand, Flags=SecurityPermissionFlag.SerializationFormatter)]
		public virtual void GetObjectData (SerializationInfo info, StreamingContext context)
		{
			if (info == null)
				throw new ArgumentNullException ("info");

			info.AddValue ("Version", generation);
			info.AddValue ("Comparer", hcp);
			KeyValuePair<TKey, TValue> [] data = null;
			if (count > 0) {
				data = new KeyValuePair<TKey,TValue> [count];
				CopyTo (data, 0);
			}
			info.AddValue ("HashSize", table.Length);
			info.AddValue ("KeyValuePairs", data);
		}

		public virtual void OnDeserialization (object sender)
		{
			if (serialization_info == null)
				return;

			generation = serialization_info.GetInt32 ("Version");
			hcp = (IEqualityComparer<TKey>) serialization_info.GetValue ("Comparer", typeof (IEqualityComparer<TKey>));

			int hashSize = serialization_info.GetInt32 ("HashSize");
			KeyValuePair<TKey, TValue> [] data =
				(KeyValuePair<TKey, TValue> [])
				serialization_info.GetValue ("KeyValuePairs", typeof (KeyValuePair<TKey, TValue> []));

			if (hashSize < INITIAL_SIZE)
				hashSize = INITIAL_SIZE;
			InitArrays (hashSize);
			count = 0;

			if (data != null) {
				for (int i = 0; i < data.Length; ++i)
					Add (data [i].Key, data [i].Value);
			}
			generation++;
			serialization_info = null;
		}
#endif
#endregion

        public bool Remove (TKey key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			// get first item of linked list corresponding to given key
			int hashCode = hcp.GetHashCode (key);
			int index = (hashCode & int.MaxValue) % table.Length;
			int cur = table [index] - 1;
			
			// if there is no linked list, return false
			if (cur == NO_SLOT)
				return false;
				
			// walk linked list until right slot (and its predecessor) is
			// found or end is reached
			int prev = NO_SLOT;
			do {
				// The ordering is important for compatibility with MS and strange
				// Object.Equals () implementations
				if (linkSlots [cur].HashCode == hashCode && hcp.Equals (keySlots [cur], key))
					break;
				prev = cur;
				cur = linkSlots [cur].Next;
			} while (cur != NO_SLOT);

			// if we reached the end of the chain, return false
			if (cur == NO_SLOT)
				return false;

			count--;
			// remove slot from linked list
			// is slot at beginning of linked list?
			if (prev == NO_SLOT)
				table [index] = linkSlots [cur].Next + 1;
			else
				linkSlots [prev].Next = linkSlots [cur].Next;

			// mark slot as empty and prepend it to "empty slots chain"				
			linkSlots [cur].Next = emptySlot;
			emptySlot = cur;
			
			generation++;
			return true;
		}

		public bool TryGetValue (TKey key, out TValue value)
		{
			if (key == null)
				throw new ArgumentNullException ("key");

			// get first item of linked list corresponding to given key
			int hashCode = hcp.GetHashCode (key);
			int cur = table [(hashCode & int.MaxValue) % table.Length] - 1;

			// walk linked list until right slot is found or end is reached
			while (cur != NO_SLOT) {
				// The ordering is important for compatibility with MS and strange
				// Object.Equals () implementations
				if (linkSlots [cur].HashCode == hashCode && hcp.Equals (keySlots [cur], key)) {
					value = valueSlots [cur];
					return true;
				}
				cur = linkSlots [cur].Next;
			}

			// we did not find the slot
			value = default (TValue);
			return false;
		}

		ICollection<TKey> IDictionary<TKey, TValue>.Keys {
			get { return Keys; }
		}

		ICollection<TValue> IDictionary<TKey, TValue>.Values {
			get { return Values; }
		}

		public KeyCollection Keys {
			get { return new KeyCollection (this); }
		}

		public ValueCollection Values {
			get { return new ValueCollection (this); }
		}

		ICollection IDictionary.Keys {
			get { return Keys; }
		}

		ICollection IDictionary.Values {
			get { return Values; }
		}

		bool IDictionary.IsFixedSize {
			get { return false; }
		}

		bool IDictionary.IsReadOnly {
			get { return false; }
		}

		TKey ToTKey (object key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			if (!(key is TKey))
				throw new ArgumentException ("not of type: " + typeof (TKey).ToString (), "key");
			return (TKey) key;
		}

		TValue ToTValue (object value)
		{
			if (!(value is TValue))
				throw new ArgumentException ("not of type: " + typeof (TValue).ToString (), "value");
			return (TValue) value;
		}

		object IDictionary.this [object key] {
			get {
				if (key is TKey && ContainsKey((TKey) key))
					return this [ToTKey (key)];
				return null;
			}
			set { this [ToTKey (key)] = ToTValue (value); }
		}

		void IDictionary.Add (object key, object value)
		{
			this.Add (ToTKey (key), ToTValue (value));
		}

		bool IDictionary.Contains (object key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			if (key is TKey)
				return ContainsKey ((TKey) key);
			return false;
		}

		void IDictionary.Remove (object key)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			if (key is TKey)
				Remove ((TKey) key);
		}

		bool ICollection.IsSynchronized {
			get { return false; }
		}

		object ICollection.SyncRoot {
			get { return this; }
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly {
			get { return false; }
		}

		void ICollection<KeyValuePair<TKey, TValue>>.Add (KeyValuePair<TKey, TValue> keyValuePair)
		{
			Add (keyValuePair.Key, keyValuePair.Value);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Contains (KeyValuePair<TKey, TValue> keyValuePair)
		{
			return this.ContainsKey (keyValuePair.Key);
		}

		void ICollection<KeyValuePair<TKey, TValue>>.CopyTo (KeyValuePair<TKey, TValue> [] array, int index)
		{
			this.CopyTo (array, index);
		}

		bool ICollection<KeyValuePair<TKey, TValue>>.Remove (KeyValuePair<TKey, TValue> keyValuePair)
		{
			return Remove (keyValuePair.Key);
		}

		void ICollection.CopyTo (Array array, int index)
		{
			if (array == null)
				throw new ArgumentNullException ("array");
			if (index < 0)
				throw new ArgumentOutOfRangeException ("index");
			// we want no exception for index==array.Length && Count == 0
			if (index > array.Length)
				throw new ArgumentException ("index larger than largest valid index of array");
			if (array.Length - index < count)
				throw new ArgumentException ("Destination array cannot hold the requested elements!");

			for (int i = 0; i < table.Length; i++) {
				int cur = table [i] - 1;
				while (cur != NO_SLOT) {
					array.SetValue (new DictionaryEntry (keySlots [cur], valueSlots [cur]), index++);
					cur = linkSlots [cur].Next;
				}
			}
		}

		IEnumerator IEnumerable.GetEnumerator ()
		{
			return new Enumerator (this);
		}

		IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator ()
		{
			return new Enumerator (this);
		}

		IDictionaryEnumerator IDictionary.GetEnumerator ()
		{
			return new ShimEnumerator (this);
		}

		public Enumerator GetEnumerator ()
		{
			return new Enumerator (this);
		}

		[Serializable]
		private class ShimEnumerator : IDictionaryEnumerator, IEnumerator
		{
			Enumerator host_enumerator;
			public ShimEnumerator (Dictionary<TKey, TValue> host)
			{
				host_enumerator = host.GetEnumerator ();
			}

			public void Dispose ()
			{
				host_enumerator.Dispose ();
			}

			public bool MoveNext ()
			{
				return host_enumerator.MoveNext ();
			}

			public DictionaryEntry Entry {
				get { return ((IDictionaryEnumerator) host_enumerator).Entry; }
			}

			public object Key {
				get { return host_enumerator.Current.Key; }
			}

			public object Value {
				get { return host_enumerator.Current.Value; }
			}

			// This is the raison d' etre of this $%!@$%@^@ class.
			// We want: IDictionary.GetEnumerator ().Current is DictionaryEntry
			public object Current {
				get { return Entry; }
			}

			public void Reset ()
			{
				((IEnumerator)host_enumerator).Reset ();
			}
		}

		[Serializable]
		public struct Enumerator : IEnumerator<KeyValuePair<TKey,TValue>>,
			IDisposable, IDictionaryEnumerator, IEnumerator
		{
			Dictionary<TKey, TValue> dictionary;
			int curTableItem;
			int cur;
			int stamp;

			internal Enumerator (Dictionary<TKey, TValue> dictionary)
			{
				this.dictionary = dictionary;
				stamp = dictionary.generation;

				// The following stanza is identical to IEnumerator.Reset (),
				// but because of the definite assignment rule, we cannot call it here.
				curTableItem = -1;
				cur = NO_SLOT;
			}

			public bool MoveNext ()
			{
				VerifyState ();
				
				do {
					if (cur == NO_SLOT) {
						//move to next item in table, check if we reached the end
                        //PFXFIX: >=
						if (++curTableItem >= dictionary.table.Length)
							return false;

						cur = dictionary.table [curTableItem] - 1;
					} else
						cur = dictionary.linkSlots [cur].Next;	
				} while (cur == NO_SLOT);
				
				return true;
			}

			public KeyValuePair<TKey, TValue> Current {
				get { 
					VerifyCurrent (); 
					return new KeyValuePair <TKey, TValue> (
						dictionary.keySlots [cur],
						dictionary.valueSlots [cur]
					);
				}
			}
			
			internal TKey CurrentKey {
				get {
					VerifyCurrent ();
					return dictionary.keySlots [cur];
				}
			}
			
			internal TValue CurrentValue {
				get {
					VerifyCurrent ();
					return dictionary.valueSlots [cur];
				}
			}

			object IEnumerator.Current {
				get { return Current; }
			}

			void IEnumerator.Reset ()
			{
				curTableItem = -1;
				cur = NO_SLOT;
			}

			DictionaryEntry IDictionaryEnumerator.Entry {
				get {
					VerifyCurrent ();
					return new DictionaryEntry (
						dictionary.keySlots [cur],
						dictionary.valueSlots [cur]
					);
				}
			}

			object IDictionaryEnumerator.Key {
				get {
					VerifyCurrent();
					return dictionary.keySlots [cur];
				}
			}

			object IDictionaryEnumerator.Value {
				get {
					VerifyCurrent();
					return dictionary.valueSlots [cur];
				}
			}

			void VerifyState ()
			{
				if (dictionary == null)
					throw new ObjectDisposedException (null);
				if (dictionary.generation != stamp)
					throw new InvalidOperationException ("out of sync");
			}

			void VerifyCurrent ()
			{
				VerifyState ();
				if (cur == NO_SLOT)
					throw new InvalidOperationException ("Current is not valid");
			}

			public void Dispose ()
			{
				dictionary = null;
			}
		}

		// This collection is a read only collection
		[Serializable]
		public sealed class KeyCollection : ICollection<TKey>, IEnumerable<TKey>, ICollection, IEnumerable {
			Dictionary<TKey, TValue> dictionary;

			public KeyCollection (Dictionary<TKey, TValue> dictionary)
			{
				if (dictionary == null)
					throw new ArgumentNullException ("dictionary");
				this.dictionary = dictionary;
			}

			public void CopyTo (TKey [] array, int index)
			{
				if (array == null)
					throw new ArgumentNullException ("array");
				if (index < 0)
					throw new ArgumentOutOfRangeException ("index");
				// we want no exception for index==array.Length && dictionary.Count == 0
				if (index > array.Length)
					throw new ArgumentException ("index larger than largest valid index of array");
				if (array.Length - index < dictionary.Count)
					throw new ArgumentException ("Destination array cannot hold the requested elements!");

				for (int i = 0; i < dictionary.table.Length; i++) {
					int cur = dictionary.table [i] - 1;
					while (cur != NO_SLOT) {
						array [index++] = dictionary.keySlots [cur];
						cur = dictionary.linkSlots [cur].Next;
					}
				}
			}

			public Enumerator GetEnumerator ()
			{
				return new Enumerator (dictionary);
			}

			void ICollection<TKey>.Add (TKey item)
			{
				throw new NotSupportedException ("this is a read-only collection");
			}

			void ICollection<TKey>.Clear ()
			{
				throw new NotSupportedException ("this is a read-only collection");
			}

			bool ICollection<TKey>.Contains (TKey item)
			{
				return dictionary.ContainsKey (item);
			}

			bool ICollection<TKey>.Remove (TKey item)
			{
				throw new NotSupportedException ("this is a read-only collection");
			}

			IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator ()
			{
				return this.GetEnumerator ();
			}

			void ICollection.CopyTo (Array array, int index)
			{
				CopyTo ((TKey []) array, index);
			}

			IEnumerator IEnumerable.GetEnumerator ()
			{
				return this.GetEnumerator ();
			}

			public int Count {
				get { return dictionary.Count; }
			}

			bool ICollection<TKey>.IsReadOnly {
				get { return true; }
			}

			bool ICollection.IsSynchronized {
				get { return false; }
			}

			object ICollection.SyncRoot {
				get { return ((ICollection) dictionary).SyncRoot; }
			}

			[Serializable]
			public struct Enumerator : IEnumerator<TKey>, IDisposable, IEnumerator {
				Dictionary<TKey, TValue>.Enumerator host_enumerator;

				internal Enumerator (Dictionary<TKey, TValue> host)
				{
					host_enumerator = host.GetEnumerator ();
				}

				public void Dispose ()
				{
					host_enumerator.Dispose ();
				}

				public bool MoveNext ()
				{
					return host_enumerator.MoveNext ();
				}

				public TKey Current {
					get { return host_enumerator.CurrentKey; }
				}

				object IEnumerator.Current {
					get { return host_enumerator.CurrentKey; }
				}

				void IEnumerator.Reset ()
				{
					((IEnumerator)host_enumerator).Reset ();
				}
			}
		}

		// This collection is a read only collection
		[Serializable]
		public sealed class ValueCollection : ICollection<TValue>, IEnumerable<TValue>, ICollection, IEnumerable {
			Dictionary<TKey, TValue> dictionary;

			public ValueCollection (Dictionary<TKey, TValue> dictionary)
			{
				if (dictionary == null)
					throw new ArgumentNullException ("dictionary");
				this.dictionary = dictionary;
			}

			public void CopyTo (TValue [] array, int index)
			{
				if (array == null)
					throw new ArgumentNullException ("array");
				if (index < 0)
					throw new ArgumentOutOfRangeException ("index");
				// we want no exception for index==array.Length && dictionary.Count == 0
				if (index > array.Length)
					throw new ArgumentException ("index larger than largest valid index of array");
				if (array.Length - index < dictionary.Count)
					throw new ArgumentException ("Destination array cannot hold the requested elements!");

				for (int i = 0; i < dictionary.table.Length; i++) {
					int cur = dictionary.table [i] - 1;
					while (cur != NO_SLOT) {
						array [index++] = dictionary.valueSlots [cur];
						cur = dictionary.linkSlots [cur].Next;
					}
				}
			}

			public Enumerator GetEnumerator ()
			{
				return new Enumerator (dictionary);
			}

			void ICollection<TValue>.Add (TValue item)
			{
				throw new NotSupportedException ("this is a read-only collection");
			}

			void ICollection<TValue>.Clear ()
			{
				throw new NotSupportedException ("this is a read-only collection");
			}

			bool ICollection<TValue>.Contains (TValue item)
			{
				return dictionary.ContainsValue (item);
			}

			bool ICollection<TValue>.Remove (TValue item)
			{
				throw new NotSupportedException ("this is a read-only collection");
			}

			IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator ()
			{
				return this.GetEnumerator ();
			}

			void ICollection.CopyTo (Array array, int index)
			{
				CopyTo ((TValue []) array, index);
			}

			IEnumerator IEnumerable.GetEnumerator ()
			{
				return this.GetEnumerator ();
			}

			public int Count {
				get { return dictionary.Count; }
			}

			bool ICollection<TValue>.IsReadOnly {
				get { return true; }
			}

			bool ICollection.IsSynchronized {
				get { return false; }
			}

			object ICollection.SyncRoot {
				get { return ((ICollection) dictionary).SyncRoot; }
			}

			[Serializable]
			public struct Enumerator : IEnumerator<TValue>, IDisposable, IEnumerator {
				Dictionary<TKey, TValue>.Enumerator host_enumerator;

				internal Enumerator (Dictionary<TKey,TValue> host)
				{
					host_enumerator = host.GetEnumerator ();
				}

				public void Dispose ()
				{
					host_enumerator.Dispose();
				}

				public bool MoveNext ()
				{
					return host_enumerator.MoveNext ();
				}

				public TValue Current {
					get { return host_enumerator.CurrentValue; }
				}

				object IEnumerator.Current {
					get { return host_enumerator.CurrentValue; }
				}

				void IEnumerator.Reset ()
				{
					((IEnumerator)host_enumerator).Reset ();
				}
			}
		}
	}
}
#endif
