//
// HashSetTest.cs
//
// Authors:
//  Jb Evain  <jbevain@novell.com>
//
// Copyright (C) 2007 Novell, Inc (http://www.novell.com)
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

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

namespace MonoTests.System.Collections.Generic {

	[TestFixture]
	public class HashSetTest {

		[Test]
		public void TestAdd ()
		{
			var set = new HashSet<int> ();

			Assert.IsTrue (set.Add (1));
			Assert.IsTrue (set.Add (2));
			Assert.IsTrue (set.Add (3));
			Assert.IsTrue (set.Add (4));
			Assert.IsFalse (set.Add (4));
			Assert.IsFalse (set.Add (3));
			Assert.IsFalse (set.Add (2));
			Assert.IsFalse (set.Add (1));
			Assert.IsTrue (set.Add (0));
			Assert.IsFalse (set.Add (0));
		}

		[Test]
		public void TestRemove ()
		{
			var set = new HashSet<int> ();

			Assert.IsTrue (set.Add (1));
			Assert.IsTrue (set.Add (2));
			Assert.IsTrue (set.Add (3));
			Assert.IsTrue (set.Add (4));

			Assert.IsTrue (set.Remove (2));
			Assert.IsTrue (set.Remove (3));

			AssertContainsOnly (new int [] {1, 4}, set);
		}

		[Test]
		public void TestMassiveAdd ()
		{
			var set = new HashSet<int> ();

			var massive = Enumerable.Range (0, 10000).ToArray ();
			foreach (var item in massive)
				Assert.IsTrue (set.Add (item));

			AssertContainsOnly (massive, set);
		}

		[Test]
		public void TestMassiveRemove ()
		{
			var massive = Enumerable.Range (0, 10000).ToArray ();
			var set = new HashSet<int> (massive);

			foreach (var item in massive)
				Assert.IsTrue (set.Remove (item));

			AssertIsEmpty (set);
		}

		[Test]
		[Category("TargetJvmNotWorking")]
		public void TestCopyTo ()
		{
			var data = new [] {1, 2, 3, 4, 5};
			var set = new HashSet<int> (data);

			var array = new int [set.Count];
			set.CopyTo (array, 0);

			AssertContainsOnly (data, array);
		}

		[Test]
		public void TestClear ()
		{
			var data = new [] {1, 2, 3, 4, 5, 6};
			var set = new HashSet<int> (data);

			Assert.AreEqual (data.Length, set.Count);
			set.Clear ();
			AssertIsEmpty (set);
		}

		[Test]
		public void TestContains ()
		{
			var data = new [] {1, 2, 3, 4, 5, 6};
			var set = new HashSet<int> (data);

			foreach (var item in data)
				Assert.IsTrue (set.Contains (item));
		}

		[Test, ExpectedException (typeof (InvalidOperationException))]
		public void TestModifySetWhileForeach ()
		{
			var set = new HashSet<int> (new [] {1, 2, 3, 4});
			foreach (var item in set)
				set.Add (item + 2);
		}

		[Test]
		public void TestRemoveWhere ()
		{
			var data = new [] {1, 2, 3, 4, 5, 6, 7, 8, 9};
			var result = new [] {2, 4, 6, 8};

			var set = new HashSet<int> (data);
			int removed = set.RemoveWhere (i => (i % 2) != 0);

			Assert.AreEqual (data.Length - result.Length, removed);
			AssertContainsOnly (result, set);
		}

		[Test]
		public void TestOverlaps ()
		{
			var set = new HashSet<int> (new [] {1, 2, 3, 4, 5});

			Assert.IsTrue (set.Overlaps (new [] {0, 2}));
		}

		[Test]
		public void TestIntersectWith ()
		{
			var data = new [] {1, 2, 3, 4};
			var other = new [] {2, 4, 5, 6};
			var result = new [] {2, 4};

			var set = new HashSet<int> (data);

			set.IntersectWith (other);

			AssertContainsOnly (result, set);
		}

		[Test]
		public void TestExceptWith ()
		{
			var data = new [] {1, 2, 3, 4, 5, 6};
			var other = new [] {2, 4, 6};
			var result = new [] {1, 3, 5};
			var set = new HashSet<int> (data);

			set.ExceptWith (other);

			AssertContainsOnly (result, set);
		}

		[Test]
		public void TestUnionWith ()
		{
			var data = new [] {1, 2, 3, 4, 5, 6};
			var other = new [] {4, 5, 6, 7, 8, 9};
			var result = new [] {1, 2, 3, 4, 5, 6, 7, 8, 9};

			var set = new HashSet<int> (data);
			set.UnionWith (other);

			AssertContainsOnly (result, set);
		}

		[Test]
		public void TestSymmetricExceptWith ()
		{
			var data = new [] {1, 2, 3, 4, 5};
			var other = new [] {4, 5, 6, 7, 8, 9};
			var result = new [] {1, 2, 3, 6, 7, 8, 9};

			var set = new HashSet<int> (data);
			set.SymmetricExceptWith (other);

			AssertContainsOnly (result, set);
		}

		[Test]
		public void TestEmptyHashSubsetOf ()
		{
			var set = new HashSet<int> ();

			Assert.IsTrue (set.IsSubsetOf (new int [0]));
			Assert.IsTrue (set.IsSubsetOf (new [] {1, 2}));
		}

		[Test]
		public void TestSubsetOf ()
		{
			var data = new [] {1, 2, 3};
			var other = new [] {1, 2, 3, 4, 5};
			var other2 = new [] {1, 2, 3};
			var other3 = new [] {0, 1, 2};

			var set = new HashSet<int> (data);

			Assert.IsTrue (set.IsSubsetOf (other));
			Assert.IsTrue (set.IsSubsetOf (other2));
			Assert.IsFalse (set.IsSubsetOf (other3));
		}

		[Test]
		public void TestProperSubsetOf ()
		{
			var data = new [] {1, 2, 3};
			var other = new [] {1, 2, 3, 4, 5};
			var other2 = new [] {1, 2, 3};
			var other3 = new [] {0, 1, 2};

			var set = new HashSet<int> (data);

			Assert.IsTrue (set.IsProperSubsetOf (other));
			Assert.IsFalse (set.IsProperSubsetOf (other2));
			Assert.IsFalse (set.IsProperSubsetOf (other3));
		}

		[Test]
		public void TestSupersetOf ()
		{
			var data = new [] {1, 2, 3, 4, 5};
			var other = new [] {2, 3, 4};
			var other2 = new [] {1, 2, 3, 4, 5};
			var other3 = new [] {4, 5, 6};

			var set = new HashSet<int> (data);

			Assert.IsTrue (set.IsSupersetOf (other));
			Assert.IsTrue (set.IsSupersetOf (other2));
			Assert.IsFalse (set.IsSupersetOf (other3));
		}

		[Test]
		public void TestProperSupersetOf ()
		{
			var data = new [] {1, 2, 3, 4, 5};
			var other = new [] {2, 3, 4};
			var other2 = new [] {1, 2, 3, 4, 5};
			var other3 = new [] {4, 5, 6};

			var set = new HashSet<int> (data);

			Assert.IsTrue (set.IsProperSupersetOf (other));
			Assert.IsFalse (set.IsProperSupersetOf (other2));
			Assert.IsFalse (set.IsProperSupersetOf (other3));
		}

		[Test]
		public void TestSetEquals ()
		{
			var data = new [] {1, 2, 3, 4};

			var other = new [] {1, 2, 3, 4};
			var other2 = new [] {1, 2, 2, 4};
			var other3 = new [] {1, 2};
			var other4 = new [] {1, 2, 3, 4, 5};
			var other5 = new [] {1, 1, 1, 1};

			var set = new HashSet<int> (data);

			Assert.IsTrue (set.SetEquals (other));
			Assert.IsFalse (set.SetEquals (other2));
			Assert.IsFalse (set.SetEquals (other3));
			Assert.IsFalse (set.SetEquals (other4));
			Assert.IsFalse (set.SetEquals (other5));
		}

		static void AssertContainsOnly<T> (IEnumerable<T> result, IEnumerable<T> data)
		{
			Assert.AreEqual (result.Count (), data.Count ());

			var store = new List<T> (result);
			foreach (var element in data) {
				Assert.IsTrue (store.Contains (element));
				store.Remove (element);
			}

			AssertIsEmpty (store);
		}

		static void AssertIsEmpty<T> (IEnumerable<T> source)
		{
			Assert.AreEqual (0, source.Count ());
		}
	}
}
