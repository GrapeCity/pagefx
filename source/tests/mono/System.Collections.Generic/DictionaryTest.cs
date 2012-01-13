//
// MonoTests.System.Collections.Generic.DictionaryTest
//
// Authors:
//	Sureshkumar T (tsureshkumar@novell.com)
//	Ankit Jain (radical@corewars.org)
//	David Waite (mass@akuma.org)
//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
// Copyright (C) 2005 David Waite (mass@akuma.org)
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
using System.IO;
//using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;

using NUnit.Framework;

namespace MonoTests.System.Collections.Generic {
	[TestFixture]
	public class DictionaryTest {
		class MyClass {
			int a;
			int b;
			public MyClass (int a, int b)
			{
				this.a = a;
				this.b = b;
			}
			public override int GetHashCode ()
			{
				return a + b;
			}
	
			public override bool Equals (object obj)
			{
				if (!(obj is MyClass))
					return false;
				return ((MyClass)obj).Value == a;
			}
	
	
			public int Value {
				get { return a; }
			}
	
		}
	
		Dictionary <string, object> _dictionary = null;
		Dictionary <MyClass, MyClass> _dictionary2 = null;
		Dictionary <int, int> _dictionary3 = null;
	
		[SetUp]
		public void SetUp ()
		{
			_dictionary = new Dictionary <string, object> ();
			_dictionary2 = new Dictionary <MyClass, MyClass> ();
			_dictionary3 = new Dictionary <int, int>();
		}
	
		[Test]
		public void AddTest ()
		{
			_dictionary.Add ("key1", "value");
			Assert.AreEqual ("value", _dictionary ["key1"].ToString (), "Add failed!");
		}
	
		[Test]
		public void AddTest2 ()
		{
			MyClass m1 = new MyClass (10,5);
			MyClass m2 = new MyClass (20,5);
			MyClass m3 = new MyClass (12,3);
			_dictionary2.Add (m1,m1);
			_dictionary2.Add (m2, m2);
			_dictionary2.Add (m3, m3);
			Assert.AreEqual (20, _dictionary2 [m2].Value, "#1");
			Assert.AreEqual (10, _dictionary2 [m1].Value, "#2");
			Assert.AreEqual (12, _dictionary2 [m3].Value, "#3");
		}

		[Test]
		public void AddTest3 ()
		{
			_dictionary3.Add (1, 2);
			_dictionary3.Add (2, 3);
			_dictionary3.Add (3, 4);
			Assert.AreEqual (2, _dictionary3[1], "#1");
			Assert.AreEqual (3, _dictionary3[2], "#2");
			Assert.AreEqual (4, _dictionary3[3], "#3");
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void AddNullTest ()
		{
			_dictionary.Add (null, "");
		}
	
		[Test, ExpectedException(typeof(ArgumentException))]
		public void AddDuplicateTest ()
		{
			_dictionary.Add("foo", "bar");
			_dictionary.Add("foo", "bar");
		}

		//Tests Add when resize takes place
		[Test]
		public void AddLargeTest ()
		{
			int i, numElems = 50;
	
			for (i = 0; i < numElems; i++)
			{
				_dictionary3.Add (i, i);
			}
	
			i = 0;
			foreach (KeyValuePair <int, int> entry in _dictionary3)
			{
				i++;
			}
	
			Assert.AreEqual (i, numElems, "Add with resize failed!");
		}
	
		[Test]
		public void IndexerGetExistingTest ()
		{
			_dictionary.Add ("key1", "value");
			Assert.AreEqual ("value", _dictionary ["key1"].ToString (), "Add failed!");
		}
		
		[Test, ExpectedException(typeof(KeyNotFoundException))]
		public void IndexerGetNonExistingTest ()
		{
			object foo = _dictionary ["foo"];
		}

		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void IndexerGetNullTest()
		{
			object s = _dictionary[null];
		}

		[Test]
		public void IndexerSetExistingTest ()
		{
			_dictionary.Add ("key1", "value1");
			_dictionary ["key1"] =  "value2";
			Assert.AreEqual (1, _dictionary.Count);
			Assert.AreEqual ("value2", _dictionary ["key1"]);
		}

		[Test]
		public void IndexerSetNonExistingTest ()
		{
			_dictionary ["key1"] =  "value1";
			Assert.AreEqual (1, _dictionary.Count);
			Assert.AreEqual ("value1", _dictionary ["key1"]);
		}
	
		[Test]
		public void RemoveTest ()
		{
			_dictionary.Add ("key1", "value1");
			_dictionary.Add ("key2", "value2");
			_dictionary.Add ("key3", "value3");
			_dictionary.Add ("key4", "value4");
			Assert.IsTrue (_dictionary.Remove ("key3"));
			Assert.IsFalse (_dictionary.Remove ("foo"));
			Assert.AreEqual (3, _dictionary.Count);
			Assert.IsFalse (_dictionary.ContainsKey ("key3"));
		}
	
		[Test]
		public void RemoveTest2 ()
		{
			MyClass m1 = new MyClass (10, 5);
			MyClass m2 = new MyClass (20, 5);
			MyClass m3 = new MyClass (12, 3);
			_dictionary2.Add (m1, m1);
			_dictionary2.Add (m2, m2);
			_dictionary2.Add (m3, m3);
			_dictionary2.Remove (m1); // m2 is in rehash path
			Assert.AreEqual (20, _dictionary2 [m2].Value, "#4");
			
		}

#if NOT_PFX
		[Test]
		[Category ("NotWorking")]
		public void Remove_ZeroOut ()
		{
			object key = new object ();
			object value = new object ();

			WeakReference wrKey = new WeakReference (key);
			WeakReference wrValue = new WeakReference (value);

			Dictionary <object, object> dictionary = new Dictionary <object, object> ();
			dictionary.Add (key, value);
			dictionary.Remove (key);

			key = null;
			value = null;
			GC.Collect ();
			Thread.Sleep (200);

			Assert.IsNull (wrKey.Target, "#1");
			Assert.IsNull (wrValue.Target, "#2");
		}
#endif
	
		[Test, ExpectedException(typeof(ArgumentNullException))]
		public void IndexerSetNullTest()
		{
			_dictionary[null] = "bar";
		}
	
		[Test]
		public void ClearTest ()
		{
			_dictionary.Add ("key1", "value1");
			_dictionary.Add ("key2", "value2");
			_dictionary.Add ("key3", "value3");
			_dictionary.Add ("key4", "value4");
			_dictionary.Clear ();
			Assert.AreEqual (0, _dictionary.Count, "Clear method failed!");
			Assert.IsFalse (_dictionary.ContainsKey ("key2"));
		}

#if NOT_PFX		
        [Test]
		[Category ("NotWorking")]
		public void Clear_ZeroOut ()
		{
			object key = new object ();
			object value = new object ();

			WeakReference wrKey = new WeakReference (key);
			WeakReference wrValue = new WeakReference (value);

			Dictionary <object, object> dictionary = new Dictionary <object, object> ();
			dictionary.Add (key, value);
			dictionary.Clear ();

			key = null;
			value = null;
			GC.Collect ();
			Thread.Sleep (200);

			Assert.IsNull (wrKey.Target, "#1");
			Assert.IsNull (wrValue.Target, "#2");
		}
#endif

		[Test]
		public void ContainsKeyTest ()
		{
			_dictionary.Add ("key1", "value1");
			_dictionary.Add ("key2", "value2");
			_dictionary.Add ("key3", "value3");
			_dictionary.Add ("key4", "value4");
			bool contains = _dictionary.ContainsKey ("key4");
			Assert.IsTrue (contains, "ContainsKey does not return correct value!");
			contains = _dictionary.ContainsKey ("key5");
			Assert.IsFalse (contains, "ContainsKey for non existant does not return correct value!");
		}
	
		[Test]
		public void ContainsValueTest ()
		{
			_dictionary.Add ("key1", "value1");
			_dictionary.Add ("key2", "value2");
			_dictionary.Add ("key3", "value3");
			_dictionary.Add ("key4", "value4");
			bool contains = _dictionary.ContainsValue ("value2");
			Assert.IsTrue(contains, "ContainsValue does not return correct value!");
			contains = _dictionary.ContainsValue ("@@daisofja@@");
			Assert.IsFalse (contains, "ContainsValue for non existant does not return correct value!");
		}
	
		[Test]
		public void TryGetValueTest()
		{
			_dictionary.Add ("key1", "value1");
			_dictionary.Add ("key2", "value2");
			_dictionary.Add ("key3", "value3");
			_dictionary.Add ("key4", "value4");
			object value = "";
			bool retrieved = _dictionary.TryGetValue ("key4", out value);
			Assert.IsTrue (retrieved);
			Assert.AreEqual ("value4", (string)value, "TryGetValue does not return value!");
	
			retrieved = _dictionary.TryGetValue ("key7", out value);
			Assert.IsFalse (retrieved);
			Assert.IsNull (value, "value for non existant value should be null!");
		}
	
		[Test]
		public void ValueTypeTest ()
		{
			Dictionary <int, float> dict = new Dictionary <int, float> ();
			dict.Add (10, 10.3f);
			dict.Add (11, 10.4f);
			dict.Add (12, 10.5f);
			Assert.AreEqual (10.4f, dict [11], "#5");
		}
	
		private class MyTest
		{
			public string Name;
			public int RollNo;
	
			public MyTest (string name, int number)
			{
				Name = name;
				RollNo = number;
			}

			public override int GetHashCode ()
			{
				return Name.GetHashCode () ^ RollNo;
			}

			public override bool Equals (object obj)
			{
				MyTest myt = obj as MyTest;
				return myt.Name.Equals (this.Name) &&
						myt.RollNo.Equals (this.RollNo);
			}
		}
	
		[Test]
		public void ObjectAsKeyTest ()
		{
			Dictionary <object, object> dict = new Dictionary <object, object> ();
			MyTest key1, key2, key3;
			dict.Add ( (key1 = new MyTest ("key1", 234)), "value1");
			dict.Add ( (key2 = new MyTest ("key2", 444)), "value2");
			dict.Add ( (key3 = new MyTest ("key3", 5655)), "value3");
	
			Assert.AreEqual ("value2", dict [key2], "value is not returned!");
			Assert.AreEqual ("value3", dict [key3], "neg: exception should not be thrown!");
		}
	
		[Test, ExpectedException (typeof (ArgumentException))]
		public void IDictionaryAddTest ()
		{
			IDictionary iDict = _dictionary as IDictionary;
			iDict.Add ("key1", "value1");
			iDict.Add ("key2", "value3");
			Assert.AreEqual (2, iDict.Count, "IDictioanry interface add is not working!");
	
			//Negative test case
			iDict.Add (12, "value");
			iDict.Add ("key", 34);
		}
	
		[Test]
		public void IEnumeratorTest ()
		{
			_dictionary.Add ("key1", "value1");
			_dictionary.Add ("key2", "value2");
			_dictionary.Add ("key3", "value3");
			_dictionary.Add ("key4", "value4");
			IEnumerator itr = ((IEnumerable)_dictionary).GetEnumerator ();
			while (itr.MoveNext ())	{
				object o = itr.Current;
				Assert.AreEqual (typeof (KeyValuePair<string,object>), o.GetType (), "Current should return a type of KeyValuePair");
				KeyValuePair<string,object> entry = (KeyValuePair<string,object>) itr.Current;
			}
			Assert.AreEqual ("value4", _dictionary ["key4"].ToString (), "");
		}
	
	
		[Test]
		public void IEnumeratorGenericTest ()
		{
			_dictionary.Add ("key1", "value1");
			_dictionary.Add ("key2", "value2");
			_dictionary.Add ("key3", "value3");
			_dictionary.Add ("key4", "value4");
			IEnumerator <KeyValuePair <string, object>> itr = ((IEnumerable <KeyValuePair <string, object>>)_dictionary).GetEnumerator ();
			while (itr.MoveNext ())	{
				object o = itr.Current;
				Assert.AreEqual (typeof (KeyValuePair <string, object>), o.GetType (), "Current should return a type of KeyValuePair<object,string>");
				KeyValuePair <string, object> entry = (KeyValuePair <string, object>)itr.Current;
			}
			Assert.AreEqual ("value4", _dictionary ["key4"].ToString (), "");
		}
	
		[Test]
		public void IDictionaryEnumeratorTest ()
		{
			_dictionary.Add ("key1", "value1");
			_dictionary.Add ("key2", "value2");
			_dictionary.Add ("key3", "value3");
			_dictionary.Add ("key4", "value4");
			IDictionaryEnumerator itr = ((IDictionary)_dictionary).GetEnumerator ();
			while (itr.MoveNext ()) {
				object o = itr.Current;
				Assert.AreEqual (typeof (DictionaryEntry), o.GetType (), "Current should return a type of DictionaryEntry");
				DictionaryEntry entry = (DictionaryEntry) itr.Current;
			}
			Assert.AreEqual ("value4", _dictionary ["key4"].ToString (), "");
		}
	
		[Test]
		public void ForEachTest ()
		{
			_dictionary.Add ("key1", "value1");
			_dictionary.Add ("key2", "value2");
			_dictionary.Add ("key3", "value3");
			_dictionary.Add ("key4", "value4");
	
			int i = 0;
			foreach (KeyValuePair <string, object> entry in _dictionary)
				i++;
			Assert.AreEqual(4, i, "fail1: foreach entry failed!");
	
			i = 0;
			foreach (KeyValuePair <string, object> entry in ((IEnumerable)_dictionary))
				i++;
			Assert.AreEqual(4, i, "fail2: foreach entry failed!");
	
			i = 0;
			foreach (DictionaryEntry entry in ((IDictionary)_dictionary))
				i++;
			Assert.AreEqual (4, i, "fail3: foreach entry failed!");
		}
	
		[Test]
		public void ResizeTest ()
		{
			Dictionary <string, object> dictionary = new Dictionary <string, object> (3);
			dictionary.Add ("key1", "value1");
			dictionary.Add ("key2", "value2");
			dictionary.Add ("key3", "value3");
	
			Assert.AreEqual (3, dictionary.Count);
	
			dictionary.Add ("key4", "value4");
			Assert.AreEqual (4, dictionary.Count);
			Assert.AreEqual ("value1", dictionary ["key1"].ToString (), "");
			Assert.AreEqual ("value2", dictionary ["key2"].ToString (), "");
			Assert.AreEqual ("value4", dictionary ["key4"].ToString (), "");
			Assert.AreEqual ("value3", dictionary ["key3"].ToString (), "");
		}
	
		[Test]
		public void KeyCollectionTest ()
		{
			_dictionary.Add ("key1", "value1");
			_dictionary.Add ("key2", "value2");
			_dictionary.Add ("key3", "value3");
			_dictionary.Add ("key4", "value4");
	
			ICollection <string> keys = ((IDictionary <string, object>)_dictionary).Keys;
			Assert.AreEqual (4, keys.Count);
			int i = 0;
			foreach (string key in keys)
			{
				i++;
			}
			Assert.AreEqual(4, i);
		}

		[Test]
		public void KeyValueEnumeratorTest ()
		{
			IDictionary<int, int> d = new Dictionary<int, int>();

			// Values are chosen such that two keys map to the same bucket.
			// Default dictionary table size == 10
			d [9] = 1;
			d [10] = 2;
			d [19] = 3;

			Assert.AreEqual (d.Count, d.Keys.Count, "d and d.Keys don't appear to match");
			Assert.AreEqual (d.Values.Count, d.Keys.Count, "d.Keys and d.Values don't appear to match");

			int count = 0;
			foreach (int i in d.Values)
				++count;
			Assert.AreEqual (count, d.Values.Count, "d.Values doesn't have the correct number of elements");
	
			count = 0;
			foreach (int i in d.Keys)
				++count;
			Assert.AreEqual (count, d.Keys.Count, "d.Keys doesn't have the correct number of elements");

			int nkeys = count;
			count = 0;
			foreach (int i in d.Keys) {
				int foo = d [i];
				if (count++ >= nkeys)
					Assert.Fail ("Reading a value appears to trash enumerator state");
			}
		}

		[Test] // bug 75073
		public void SliceCollectionsEnumeratorTest ()
		{
			Dictionary<string, int> values = new Dictionary<string, int> ();

			IEnumerator <string> ke = values.Keys.GetEnumerator ();
			IEnumerator <int>    ve = values.Values.GetEnumerator ();

			Assert.IsTrue (ke is Dictionary<string, int>.KeyCollection.Enumerator);
			Assert.IsTrue (ve is Dictionary<string, int>.ValueCollection.Enumerator);
		}

		[Test]
		public void PlainEnumeratorReturnTest ()
		{
			// Test that we return a KeyValuePair even for non-generic dictionary iteration
			_dictionary["foo"] = "bar";
			IEnumerator<KeyValuePair<string, object>> enumerator = _dictionary.GetEnumerator();
			Assert.IsTrue(enumerator.MoveNext(), "#1");
			Assert.AreEqual (typeof (KeyValuePair<string,object>), ((IEnumerator)enumerator).Current.GetType (), "#2");
			Assert.AreEqual (typeof (DictionaryEntry), ((IDictionaryEnumerator)enumerator).Entry.GetType (), "#3");
			Assert.AreEqual (typeof (KeyValuePair<string,object>), ((IDictionaryEnumerator)enumerator).Current.GetType (), "#4");
			Assert.AreEqual (typeof (KeyValuePair<string,object>), ((object) enumerator.Current).GetType (), "#5");
		}

		[Test, ExpectedException (typeof (InvalidOperationException))]
		public void FailFastTest1 ()
		{
			Dictionary<int, int> d = new Dictionary<int, int> ();
			d [1] = 1;
			int count = 0;
			foreach (KeyValuePair<int, int> kv in d) {
				d [kv.Key + 1] = kv.Value + 1;
				if (count++ != 0)
					Assert.Fail ("Should not be reached");
			}
			Assert.Fail ("Should not be reached");
		}

		[Test, ExpectedException (typeof (InvalidOperationException))]
		public void FailFastTest2 ()
		{
			Dictionary<int, int> d = new Dictionary<int, int> ();
			d [1] = 1;
			int count = 0;
			foreach (int i in d.Keys) {
				d [i + 1] = i + 1;
				if (count++ != 0)
					Assert.Fail ("Should not be reached");
			}
			Assert.Fail ("Should not be reached");
		}

		[Test, ExpectedException (typeof (InvalidOperationException))]
		public void FailFastTest3 ()
		{
			Dictionary<int, int> d = new Dictionary<int, int> ();
			d [1] = 1;
			int count = 0;
			foreach (int i in d.Keys) {
				d [i] = i;
				if (count++ != 0)
					Assert.Fail ("Should not be reached");
			}
			Assert.Fail ("Should not be reached");
		}

#if NOT_PFX
		[Test]
		[Category ("TargetJvmNotWorking")] // BUGBUG Very very slow on TARGET_JVM.
		public void SerializationTest()
		{
			for (int i = 0; i < 50; i++)
			{
				_dictionary3.Add(i, i);
			}

			BinaryFormatter formatter = new BinaryFormatter();
			MemoryStream stream = new MemoryStream();
			formatter.Serialize(stream, _dictionary3);

			stream.Position = 0;
			object deserialized = formatter.Deserialize(stream);

			Assert.IsNotNull(deserialized);
			Assert.IsFalse(deserialized == _dictionary3);

			Assert.IsTrue(deserialized is Dictionary<int, int>);
			Dictionary<int, int> d3 = deserialized as Dictionary<int, int>;

			Assert.AreEqual(50, d3.Count);
			for (int i = 0; i < 50; i++)
			{
				Assert.AreEqual(i, d3[i]);
			}
		}
#endif

		[Test]
		public void ZeroCapacity ()
		{
			Dictionary<int, int> x = new Dictionary <int, int> (0);
			x.Add (1, 2);
			
			x = new Dictionary <int, int> (0);
			x.Clear ();

			x = new Dictionary <int, int> (0);
			int aa = x.Count;
			
			x = new Dictionary <int, int> (0);
			try {
				int j = x [1];
			} catch (KeyNotFoundException){
			}

			bool b;
			b = x.ContainsKey (10);
			b = x.ContainsValue (10);

			x = new Dictionary <int, int> (0);
			x.Remove (10);
			
			x = new Dictionary <int, int> (0);
			int intv;
			x.TryGetValue (1, out intv);

			object oa = x.Keys;
			object ob = x.Values;
			foreach (KeyValuePair<int,int> a in x){
			}
		}

		[Test]
		public void Empty_KeysValues_CopyTo ()
		{
			Dictionary<int, int> d = new Dictionary<int, int> ();
			int[] array = new int[1];
			d.Keys.CopyTo (array, array.Length);
			d.Values.CopyTo (array, array.Length);
		}

		[Test]
		public void Empty_CopyTo ()
		{
			Dictionary<int, int> d = new Dictionary<int, int> ();
			ICollection c = (ICollection) d;
			DictionaryEntry [] array = new DictionaryEntry [1];
			c.CopyTo (array, array.Length);

			ICollection<KeyValuePair<int,int>> c2 = d;
			KeyValuePair<int,int> [] array2 = new KeyValuePair<int,int> [1];
			c2.CopyTo (array2, array2.Length);
		}

		[Test]
		public void IDictionary_Contains ()
		{
			IDictionary d = new Dictionary<int, int> ();
			d.Add (1, 2);
			Assert.IsTrue (d.Contains (1));
			Assert.IsFalse (d.Contains (2));
			Assert.IsFalse (d.Contains ("x"));
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void IDictionary_Contains2 ()
		{
			IDictionary d = new Dictionary<int, int> ();
			d.Contains (null);
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void IDictionary_Add1 ()
		{
			IDictionary d = new Dictionary<int, int> ();
			d.Add (null, 1);
		}

		[Test, ExpectedException (typeof (ArgumentException))]
		public void IDictionary_Add2 ()
		{
			IDictionary d = new Dictionary<int, int> ();
			d.Add ("bar", 1);
		}

		[Test, ExpectedException (typeof (ArgumentException))]
		public void IDictionary_Add3 ()
		{
			IDictionary d = new Dictionary<int, int> ();
			d.Add (1, "bar");
		}

        // TODO: we should investigate this case
#if NOT_PFX
		[Test]
		public void IDictionary_Add_Null ()
		{
			IDictionary d = new Dictionary<int, string> ();
			d.Add (1, null);
			d [2] = null;

			Assert.IsNull (d [1]);
			Assert.IsNull (d [2]);
		}
#endif

		[Test]
		[ExpectedException (typeof (ArgumentException))]
		public void IDictionary_Add_Null_2 ()
		{
			IDictionary d = new Dictionary<int, int> ();
			d.Add (1, null);
		}

		[Test]
		public void IDictionary_Remove1 ()
		{
			IDictionary d = new Dictionary<int, int> ();
			d.Add (1, 2);
			d.Remove (1);
			d.Remove (5);
			d.Remove ("foo");
		}

		[Test, ExpectedException (typeof (ArgumentNullException))]
		public void IDictionary_Remove2 ()
		{
			IDictionary d = new Dictionary<int, int> ();
			d.Remove (null);
		}
		
		[Test]
		public void IDictionary_IndexerGetNonExistingTest ()
		{
			IDictionary d = new Dictionary<int, int> ();
			d.Add(1, 2);
			Assert.IsNull(d[2]);
			Assert.IsNull(d["foo"]);
		}

		[Test] // bug #332534
		public void Dictionary_MoveNext ()
		{
			Dictionary<int,int> a = new Dictionary<int,int>();
			a.Add(3,1);
			a.Add(4,1);

			IEnumerator en = a.GetEnumerator();
			for (int i = 1; i < 10; i++)
				en.MoveNext();
		}

        // TODO: we should investigate problem with this case
#if NOT_PFX
		[Test]
		public void CopyToArray ()
		{
			Dictionary<string, string> test = new Dictionary<string, string> ();
			test.Add ("monkey", "singe");
			test.Add ("singe", "mono");
			test.Add ("mono", "monkey");
			Assert.AreEqual (3, test.Keys.Count, "Dictionary.Count");
			
			ArrayList list = new ArrayList (test.Keys);
			Assert.AreEqual (3, list.Count, "ArrayList.Count");
			Assert.IsTrue (list.Contains ("monkey"), "monkey");
			Assert.IsTrue (list.Contains ("singe"), "singe");
			Assert.IsTrue (list.Contains ("mono"), "mono");
		}
#endif

		[Test]
		public void KeyObjectMustNotGetChangedIfKeyAlreadyExists ()
		{
			Dictionary<String, int> d = new Dictionary<string, int> ();
			string s1 = "Test";
			string s2 = "Tes" + "T".ToLowerInvariant();
			d[s1] = 1;
			d[s2] = 2;
			string comp = String.Empty;
			foreach (String s in d.Keys)
				comp = s;
			Assert.IsTrue (Object.ReferenceEquals (s1, comp));
		}
	}
}

#endif // NET_2_0
