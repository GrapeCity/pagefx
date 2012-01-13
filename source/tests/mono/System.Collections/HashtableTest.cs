// HashtableTest.cs - NUnit Test Cases for the System.Collections.Hashtable class
//
//
// (C) Ximian, Inc.  http://www.ximian.com
// 


using System;
using System.Collections;

using System.IO;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters;
//using System.Runtime.Serialization.Formatters.Binary;

using NUnit.Framework;

namespace MonoTests.System.Collections
{
    /// <summary>Hashtable test.</summary>
    [TestFixture]
    public class HashtableTest : Assertion
    {
        [Test]
        public void TestCtor1()
        {
            Hashtable h = new Hashtable();
            AssertNotNull("No hash table", h);
        }

        [Test]
        public void TestCtor2()
        {
            {
                bool errorThrown = false;
                try
                {
                    Hashtable h = new Hashtable((IDictionary)null);
                }
                catch (ArgumentNullException)
                {
                    errorThrown = true;
                }
                Assert("null hashtable error not thrown",
                       errorThrown);
            }
            {
                string[] keys = { "this", "is", "a", "test" };
                char[] values = { 'a', 'b', 'c', 'd' };
                Hashtable h1 = new Hashtable();
                for (int i = 0; i < keys.Length; i++)
                {
                    h1[keys[i]] = values[i];
                }
                Hashtable h2 = new Hashtable(h1);
                for (int i = 0; i < keys.Length; i++)
                {
                    AssertEquals("No match for key " + keys[i],
                             values[i], h2[keys[i]]);
                }
            }
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void TestCtor3()
        {
            Hashtable h = new Hashtable();
            Hashtable hh = new Hashtable(h, Single.NaN);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void TestCtor4()
        {
            Hashtable ht = new Hashtable(Int32.MaxValue, 0.1f, null, null);
        }

        [Test]
        public void TestCtor5()
        {
            // tests if negative capacity throws exception
            try
            {
                Hashtable ht = new Hashtable(-10, 0.1f, null, null);
                Assert("must throw ArgumentOutOfRange exception, param: capacity", false);
            }
            catch (ArgumentOutOfRangeException e)
            {
            }

            // tests if loadFactor out of range throws exception (low)
            try
            {
                Hashtable ht = new Hashtable(100, 0.01f, null, null);
                Assert("must throw ArgumentOutOfRange exception, param: loadFactor, too low value", false);
            }
            catch (ArgumentOutOfRangeException e)
            {
            }

            // tests if loadFactor out of range throws exception (high)
            try
            {
                Hashtable ht = new Hashtable(100, 2f, null, null);
                Assert("must throw ArgumentOutOfRange exception, param: loadFactor, too high value", false);
            }
            catch (ArgumentOutOfRangeException e)
            {
            }

        }

        // TODO - Ctors for capacity and load (how to test? any access?)
        // TODO - Ctors with IComparer, IHashCodeProvider, Serialization

        [Test]
        public void TestCount()
        {
            Hashtable h = new Hashtable();
            AssertEquals("new table - count zero", 0, h.Count);
            int max = 100;
            for (int i = 1; i <= max; i++)
            {
                h[i] = i;
                AssertEquals("Count wrong for " + i,
                         i, h.Count);
            }
            for (int i = 1; i <= max; i++)
            {
                h[i] = i * 2;
                AssertEquals("Count shouldn't change at " + i,
                         max, h.Count);
            }
        }

        [Test]
        public void TestIsFixedSize()
        {
            Hashtable h = new Hashtable();
            AssertEquals("hashtable not fixed by default",
                     false, h.IsFixedSize);
            // TODO - any way to get a fixed-size hashtable?
        }

        public void TestIsReadOnly()
        {
            Hashtable h = new Hashtable();
            AssertEquals("hashtable not read-only by default",
                     false, h.IsReadOnly);
            // TODO - any way to get a read-only hashtable?
        }

        [Test]
        public void TestIsSynchronized()
        {
            Hashtable h = new Hashtable();
            Assert("hashtable not synched by default", !h.IsSynchronized);

            Hashtable h2 = Hashtable.Synchronized(h);
            Assert("hashtable should by synched", h2.IsSynchronized);

            Hashtable h3 = (Hashtable)h2.Clone();
            Assert("Cloned Hashtable should by synched", h3.IsSynchronized);
        }

        [Test]
        public void TestItem()
        {
            {
                bool errorThrown = false;
                try
                {
                    Hashtable h = new Hashtable();
                    Object o = h[null];
                }
                catch (ArgumentNullException e)
                {
                    errorThrown = true;
                }
                Assert("null hashtable error not thrown",
                       errorThrown);
            }
            // TODO - if read-only and/or fixed-size is possible,
            //        test 'NotSupportedException' here

            {
                Hashtable h = new Hashtable();
                int max = 100;
                for (int i = 1; i <= max; i++)
                {
                    h[i] = i;
                    AssertEquals("value wrong for " + i,
                             i, h[i]);
                }
            }
        }

        [Test]
        public void TestKeys()
        {
            string[] keys = { "this", "is", "a", "test" };
            string[] keys2 = { "new", "keys" };
            char[] values1 = { 'a', 'b', 'c', 'd' };
            char[] values2 = { 'e', 'f', 'g', 'h' };
            ICollection keysReference, keysReference2;
            Hashtable h1 = new Hashtable();
            for (int i = 0; i < keys.Length; i++)
            {
                h1[keys[i]] = values1[i];
            }
            AssertEquals("keys wrong size",
                     keys.Length, h1.Keys.Count);
            for (int i = 0; i < keys.Length; i++)
            {
                h1[keys[i]] = values2[i];
            }
            AssertEquals("keys wrong size 2",
                     keys.Length, h1.Keys.Count);

            // MS .NET Always returns the same reference when calling Keys property
            keysReference = h1.Keys;
            keysReference2 = h1.Keys;
            AssertEquals("keys references differ", keysReference, keysReference2);

            for (int i = 0; i < keys2.Length; i++)
            {
                h1[keys2[i]] = values2[i];
            }
            AssertEquals("keys wrong size 3",
                keys.Length + keys2.Length, h1.Keys.Count);
            AssertEquals("keys wrong size 4",
                keys.Length + keys2.Length, keysReference.Count);
        }

        // TODO - SyncRoot
        [Test]
        public void TestValues()
        {
            string[] keys = { "this", "is", "a", "test" };
            char[] values1 = { 'a', 'b', 'c', 'd' };
            char[] values2 = { 'e', 'f', 'g', 'h' };
            Hashtable h1 = new Hashtable();
            for (int i = 0; i < keys.Length; i++)
            {
                h1[keys[i]] = values1[i];
            }
            AssertEquals("values wrong size",
                     keys.Length, h1.Values.Count);
            for (int i = 0; i < keys.Length; i++)
            {
                h1[keys[i]] = values2[i];
            }
            AssertEquals("values wrong size 2",
                     keys.Length, h1.Values.Count);

            // MS .NET Always returns the same reference when calling Values property
            ICollection valuesReference1 = h1.Values;
            ICollection valuesReference2 = h1.Values;
            AssertEquals("values references differ", valuesReference1, valuesReference2);
        }

        [Test]
        public void TestAdd()
        {
            {
                bool errorThrown = false;
                try
                {
                    Hashtable h = new Hashtable();
                    h.Add(null, "huh?");
                }
                catch (ArgumentNullException e)
                {
                    errorThrown = true;
                }
                Assert("null add error not thrown",
                       errorThrown);
            }
            {
                bool errorThrown = false;
                try
                {
                    Hashtable h = new Hashtable();
                    h.Add('a', 1);
                    h.Add('a', 2);
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("re-add error not thrown",
                       errorThrown);
            }
            // TODO - hit NotSupportedException
            {
                Hashtable h = new Hashtable();
                int max = 100;
                for (int i = 1; i <= max; i++)
                {
                    h.Add(i, i);
                    AssertEquals("value wrong for " + i,
                             i, h[i]);
                }
            }
        }

        [Test]
        public void TestClear()
        {
            // TODO - hit NotSupportedException
            Hashtable h = new Hashtable();
            AssertEquals("new table - count zero", 0, h.Count);
            int max = 100;
            for (int i = 1; i <= max; i++)
            {
                h[i] = i;
            }
            Assert("table don't gots stuff", h.Count > 0);
            h.Clear();
            AssertEquals("Table should be cleared",
                     0, h.Count);
        }

        [Test]
        public void TestClone()
        {
            {
                char[] c1 = { 'a', 'b', 'c' };
                char[] c2 = { 'd', 'e', 'f' };
                Hashtable h1 = new Hashtable();
                for (int i = 0; i < c1.Length; i++)
                {
                    h1[c1[i]] = c2[i];
                }
                Hashtable h2 = (Hashtable)h1.Clone();
                AssertNotNull("got no clone!", h2);
                AssertNotNull("clone's got nothing!", h2[c1[0]]);
                for (int i = 0; i < c1.Length; i++)
                {
                    AssertEquals("Hashtable match",
                             h1[c1[i]], h2[c1[i]]);
                }
            }
            {
                char[] c1 = { 'a', 'b', 'c' };
                char[] c20 = { '1', '2' };
                char[] c21 = { '3', '4' };
                char[] c22 = { '5', '6' };
                char[][] c2 = { c20, c21, c22 };
                Hashtable h1 = new Hashtable();
                for (int i = 0; i < c1.Length; i++)
                {
                    h1[c1[i]] = c2[i];
                }
                Hashtable h2 = (Hashtable)h1.Clone();
                AssertNotNull("got no clone!", h2);
                AssertNotNull("clone's got nothing!", h2[c1[0]]);
                for (int i = 0; i < c1.Length; i++)
                {
                    AssertEquals("Hashtable match",
                             h1[c1[i]], h2[c1[i]]);
                }

                ((char[])h1[c1[0]])[0] = 'z';
                AssertEquals("shallow copy", h1[c1[0]], h2[c1[0]]);
            }
        }

        [Test]
        public void TestContains()
        {
            {
                bool errorThrown = false;
                try
                {
                    Hashtable h = new Hashtable();
                    bool result = h.Contains(null);
                }
                catch (ArgumentNullException e)
                {
                    errorThrown = true;
                }
                Assert("null add error not thrown",
                       errorThrown);
            }
            {
                Hashtable h = new Hashtable();
                for (int i = 0; i < 10000; i += 2)
                {
                    h[i] = i;
                }
                for (int i = 0; i < 10000; i += 2)
                {
                    Assert("hashtable must contain" + i.ToString(), h.Contains(i));
                    Assert("hashtable does not contain " + ((int)(i + 1)).ToString(), !h.Contains(i + 1));
                }
            }
        }

        [Test]
        public void TestContainsKey()
        {
            {
                bool errorThrown = false;
                try
                {
                    Hashtable h = new Hashtable();
                    bool result = h.Contains(null);
                }
                catch (ArgumentNullException e)
                {
                    errorThrown = true;
                }
                Assert("null add error not thrown",
                    errorThrown);
            }
            {
                Hashtable h = new Hashtable();
                for (int i = 0; i < 1000; i += 2)
                {
                    h[i] = i;
                }
                for (int i = 0; i < 1000; i += 2)
                {
                    Assert("hashtable must contain" + i.ToString(), h.Contains(i));
                    Assert("hashtable does not contain " + ((int)(i + 1)).ToString(), !h.Contains(i + 1));
                }
            }

        }

        [Test]
        public void TestContainsValue()
        {
            {
                Hashtable h = new Hashtable();
                h['a'] = "blue";
                Assert("blue? it's in there!",
                       h.ContainsValue("blue"));
                Assert("green? no way!",
                       !h.ContainsValue("green"));
                Assert("null? no way!",
                    !h.ContainsValue(null));
                h['b'] = null;
                Assert("null? it's in there!",
                    h.ContainsValue(null));

            }
        }

        [Test]
        public void TestCopyTo()
        {
            {
                bool errorThrown = false;
                try
                {
                    Hashtable h = new Hashtable();
                    h.CopyTo(null, 0);
                }
                catch (ArgumentNullException e)
                {
                    errorThrown = true;
                }
                Assert("#2: null hashtable error not thrown", errorThrown);
            }

            {
                bool errorThrown = false;
                try
                {
                    Hashtable h = new Hashtable();
                    Object[] o = new Object[1];
                    h.CopyTo(o, -1);
                }
                catch (ArgumentOutOfRangeException e)
                {
                    errorThrown = true;
                }
                Assert("#4: out of range error not thrown", errorThrown);
            }

            {
                bool errorThrown = false;
                try
                {
                    Hashtable h = new Hashtable();
                    Object[,] o = new Object[1, 1];
                    h.CopyTo(o, 1);
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("#5: multi-dim array error not thrown", errorThrown);
            }

            {
                bool errorThrown = false;
                try
                {
                    Hashtable h = new Hashtable();
                    h['a'] = 1; // no error if table is empty
                    Object[] o = new Object[5];
                    h.CopyTo(o, 5);
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("#6: no room in array error not thrown", errorThrown);
            }

            {
                bool errorThrown = false;
                try
                {
                    Hashtable h = new Hashtable();
                    h['a'] = 1;
                    h['b'] = 2;
                    h['c'] = 2;
                    Object[] o = new Object[2];
                    h.CopyTo(o, 0);
                }
                catch (ArgumentException)
                {
                    errorThrown = true;
                }
                Assert("#7: table too big error not thrown", errorThrown);
            }

            {
                bool errorThrown = false;
                try
                {
                    Hashtable h = new Hashtable();
                    h["blue"] = 1;
                    h["green"] = 2;
                    h["red"] = 3;
                    Char[] o = new Char[3];
                    h.CopyTo(o, 0);
                }
                catch (InvalidCastException)
                {
                    errorThrown = true;
                }
                Assert("#8: invalid cast error not thrown", errorThrown);
            }

            {
                Hashtable h = new Hashtable();
                h['a'] = 1;
                h['b'] = 2;
                DictionaryEntry[] o = new DictionaryEntry[2];
                h.CopyTo(o, 0);
#if TARGET_JVM // Hashtable is not an ordered collection!
            if (o[0].Key.Equals('b')) {
                DictionaryEntry v = o[0];
                o[0] = o[1];
                o[1] = v;
            }
#endif // TARGET_JVM
                AssertEquals("#9: first copy fine.", 'a', o[0].Key);
                AssertEquals("#10: first copy fine.", 1, o[0].Value);
                AssertEquals("#11: second copy fine.", 'b', o[1].Key);
                AssertEquals("#12: second copy fine.", 2, o[1].Value);
            }
        }

        [Test]
        public void TestGetEnumerator()
        {
            String[] s1 = { "this", "is", "a", "test" };
            Char[] c1 = { 'a', 'b', 'c', 'd' };
            Hashtable h1 = new Hashtable();
            for (int i = 0; i < s1.Length; i++)
            {
                h1[s1[i]] = c1[i];
            }
            IDictionaryEnumerator en = h1.GetEnumerator();
            AssertNotNull("No enumerator", en);

            for (int i = 0; i < s1.Length; i++)
            {
                en.MoveNext();
                Assert("Not enumerating for " + en.Key,
                       Array.IndexOf(s1, en.Key) >= 0);
                Assert("Not enumerating for " + en.Value,
                       Array.IndexOf(c1, en.Value) >= 0);
            }
        }

        //NOTE: Serialization is not supported
#if NOT_PFX
        [Test]
        public void TestSerialization()
        {
            Hashtable table1 = new Hashtable();
            Hashtable table2;
            Stream str = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            for (int i = 0; i < 100; i++)
                table1[i] = "TestString Key: " + i.ToString();

            formatter.Serialize(str, table1);
            str.Position = 0;
            table2 = (Hashtable)formatter.Deserialize(str);

            bool result;
            foreach (DictionaryEntry de in table1)
                AssertEquals(de.Value, table2[de.Key]);
        }

        [Test]
        [Category("TargetJvmNotWorking")]
        public void TestSerialization2()
        {
            // Test from bug #70570
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();

            Hashtable table = new Hashtable();
            table.Add(new Bug(), "Hello");

            formatter.Serialize(stream, table);
            stream.Position = 0;
            table = (Hashtable)formatter.Deserialize(stream);
            AssertEquals("#1", 1, table.Count);
        }
#endif

        [Test]
        public void TestRemove()
        {
            {
                bool errorThrown = false;
                try
                {
                    Hashtable h = new Hashtable();
                    h.Remove(null);
                }
                catch (ArgumentNullException e)
                {
                    errorThrown = true;
                }
                Assert("null hashtable error not thrown",
                       errorThrown);
            }
            {
                string[] keys = { "this", "is", "a", "test" };
                char[] values = { 'a', 'b', 'c', 'd' };
                Hashtable h = new Hashtable();
                for (int i = 0; i < keys.Length; i++)
                {
                    h[keys[i]] = values[i];
                }
                AssertEquals("not enough in table",
                         4, h.Count);
                h.Remove("huh?");
                AssertEquals("not enough in table",
                         4, h.Count);
                h.Remove("this");
                AssertEquals("Wrong count in table",
                         3, h.Count);
                h.Remove("this");
                AssertEquals("Wrong count in table",
                         3, h.Count);
            }
        }

        [Test]
        public void TestSynchronized()
        {
            {
                bool errorThrown = false;
                try
                {
                    Hashtable h = Hashtable.Synchronized(null);
                }
                catch (ArgumentNullException e)
                {
                    errorThrown = true;
                }
                Assert("null hashtable error not thrown",
                       errorThrown);
            }
            {
                Hashtable h = new Hashtable();
                Assert("hashtable not synced by default",
                       !h.IsSynchronized);
                Hashtable h2 = Hashtable.Synchronized(h);
                Assert("hashtable should by synced",
                       h2.IsSynchronized);
            }
        }


        protected Hashtable ht;
        private static Random rnd;

        [SetUp]
        public void SetUp()
        {
            ht = new Hashtable();
            rnd = new Random();
        }

        private void SetDefaultData()
        {
            ht.Clear();
            ht.Add("k1", "another");
            ht.Add("k2", "yet");
            ht.Add("k3", "hashtable");
        }

        [Test]
        public void TestAddRemoveClear()
        {
            ht.Clear();
            Assert(ht.Count == 0);

            SetDefaultData();
            Assert(ht.Count == 3);

            bool thrown = false;
            try
            {
                ht.Add("k2", "cool");
            }
            catch (ArgumentException) { thrown = true; }
            Assert("Must throw ArgumentException!", thrown);

            ht["k2"] = "cool";
            Assert(ht.Count == 3);
            Assert(ht["k2"].Equals("cool"));

        }

        [Test]
        public void TestCopyTo2()
        {
            SetDefaultData();
            Object[] entries = new Object[ht.Count];
            ht.CopyTo(entries, 0);
            Assert("Not an entry.", entries[0] is DictionaryEntry);
        }

        [Test]
        public void CopyTo_Empty()
        {
            Hashtable ht = new Hashtable();
            AssertEquals("Count", 0, ht.Count);
            object[] array = new object[ht.Count];
            ht.CopyTo(array, 0);
        }

        [Test]
        public void TestUnderHeavyLoad()
        {
            ht.Clear();
            //int max = 100000;
            int max = 100;
            String[] cache = new String[max * 2];
            int n = 0;

            for (int i = 0; i < max; i++)
            {
                int id = rnd.Next() & 0xFFFF;
                String key = "" + id + "-key-" + id;
                String val = "value-" + id;
                if (ht[key] == null)
                {
                    ht[key] = val;
                    cache[n] = key;
                    cache[n + max] = val;
                    n++;
                }
            }

            Assert(ht.Count == n);

            for (int i = 0; i < n; i++)
            {
                String key = cache[i];
                String val = ht[key] as String;
                String err = "ht[\"" + key + "\"]=\"" + val +
                    "\", expected \"" + cache[i + max] + "\"";
                Assert(err, val != null && val.Equals(cache[i + max]));
            }

            int r1 = (n / 3);
            int r2 = r1 + (n / 5);

            for (int i = r1; i < r2; i++)
            {
                ht.Remove(cache[i]);
            }


            for (int i = 0; i < n; i++)
            {
                if (i >= r1 && i < r2)
                {
                    Assert(ht[cache[i]] == null);
                }
                else
                {
                    String key = cache[i];
                    String val = ht[key] as String;
                    String err = "ht[\"" + key + "\"]=\"" + val +
                        "\", expected \"" + cache[i + max] + "\"";
                    Assert(err, val != null && val.Equals(cache[i + max]));
                }
            }

            ICollection keys = ht.Keys;
            int nKeys = 0;
            foreach (Object key in keys)
            {
                Assert((key as String) != null);
                nKeys++;
            }
            Assert(nKeys == ht.Count);


            ICollection vals = ht.Values;
            int nVals = 0;
            foreach (Object val in vals)
            {
                Assert((val as String) != null);
                nVals++;
            }
            Assert(nVals == ht.Count);

        }


        /// <summary>
        ///  Test hashtable with CaseInsensitiveHashCodeProvider
        ///  and CaseInsensitive comparer.
        /// </summary>
        [Test]
        public void TestCaseInsensitive()
        {
            // Not very meaningfull test, just to make
            // sure that hcp is set properly set.
            Hashtable ciHashtable = new Hashtable(11, 1.0f, CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
            ciHashtable["key1"] = "value";
            ciHashtable["key2"] = "VALUE";
            Assert(ciHashtable["key1"].Equals("value"));
            Assert(ciHashtable["key2"].Equals("VALUE"));

            ciHashtable["KEY1"] = "new_value";
            Assert(ciHashtable["key1"].Equals("new_value"));

        }

        [Test]
        public void TestCopyConstructor()
        {
            SetDefaultData();

            Hashtable htCopy = new Hashtable(ht);

            Assert(ht.Count == htCopy.Count);
        }

        [Test]
        public void TestEnumerator()
        {
            SetDefaultData();

            IEnumerator e = ht.GetEnumerator();

            while (e.MoveNext()) { }

            Assert(!e.MoveNext());

        }

        //NOTE: Serialization is not supported
#if NOT_PFX
        [Test]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetObjectData_NullSerializationInfo()
        {
            SetDefaultData();
            ht.GetObjectData(null, new StreamingContext());
        }
#endif

#if NOT_MSCLR
        // bug #75790
        [Test]
        [Category("NotDotNet")] // .NET raises InvalidOperationException.
        public void SyncHashtable_ICollectionsGetEnumerator()
        {
            Hashtable hashtable = Hashtable.Synchronized(new Hashtable());
            hashtable["a"] = 1;
            //IEnumerator e = (hashtable.Clone() as
            IEnumerator e = (hashtable as ICollection).GetEnumerator();
            //e.Reset();
            e.MoveNext();
            DictionaryEntry de = (DictionaryEntry)e.Current;
        }
#endif

        //NOTE: Serialization is not supported
#if NOT_PFX
        [Test]
        public void SerializableSubClasses()
        {
            Hashtable ht = new Hashtable();
            // see bug #76300
            Assert("Keys.IsSerializable", ht.Keys.GetType().IsSerializable);
            Assert("Values.IsSerializable", ht.Values.GetType().IsSerializable);
            Assert("GetEnumerator.IsSerializable", ht.GetEnumerator().GetType().IsSerializable);
            Assert("Synchronized.IsSerializable", Hashtable.Synchronized(ht).GetType().IsSerializable);
        }
#endif

        //https://bugzilla.novell.com/show_bug.cgi?id=324761
        [Test]
        public void TestHashtableWithCustomComparer()
        {
            // see bug #324761
            IDHashtable dd = new IDHashtable();
            Random r = new Random(1000);
            for (int n = 0; n < 10000; n++)
            {
                int v = r.Next(0, 1000);
                dd[v] = v;
                v = r.Next(0, 1000);
                dd.Remove(v);
            }
        }
    }

    class IDHashtable : Hashtable
    {

        class IDComparer : IComparer
        {
            public int Compare(object x, object y)
            {
                if ((int)x == (int)y)
                    return 0;
                else
                    return 1;
            }
        }

        class IDHashCodeProvider : IHashCodeProvider
        {
            public int GetHashCode(object o)
            {
                return (int)o;
            }
        }

        public IDHashtable()
            : base(new IDHashCodeProvider(),
                    new IDComparer())
        {
        }
    }

    //[Serializable]
    //public class Bug : ISerializable
    //{

    //    [Serializable]
    //    private sealed class InnerClassSerializationHelper : IObjectReference
    //    {
    //        public object GetRealObject(StreamingContext context)
    //        {
    //            return new Bug();
    //        }
    //    };

    //    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    //    {
    //        info.SetType(typeof(InnerClassSerializationHelper));
    //    }
    //};
}
