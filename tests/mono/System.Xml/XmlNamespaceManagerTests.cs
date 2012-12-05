//
// XmlNamespaceManagerTests.cs
//
// Authors:
//   Jason Diamond (jason@injektilo.org)
//   Martin Willemoes Hansen (mwh@sysrq.dk)
//
// (C) 2002 Jason Diamond  http://injektilo.org/
// (C) 2003 Martin Willemoes Hansen
//

using System;
using System.Xml;

using NUnit.Framework;

namespace MonoTests.System.Xml
{
    [TestFixture]
    public class XmlNamespaceManagerTests : Assertion
    {
        private XmlNameTable nameTable;
        private XmlNamespaceManager namespaceManager;

        [SetUp]
        public void GetReady()
        {
            nameTable = new NameTable();
            namespaceManager = new XmlNamespaceManager(nameTable);
        }

        [Test]
        public void NewNamespaceManager()
        {
            // make sure that you can call PopScope when there aren't any to pop.
            Assert(!namespaceManager.PopScope());

            // the following strings should have been added to the name table by the
            // namespace manager.
            string xmlnsPrefix = nameTable.Get("xmlns");
            string xmlPrefix = nameTable.Get("xml");
            string stringEmpty = nameTable.Get(String.Empty);
            string xmlnsNamespace = "http://www.w3.org/2000/xmlns/";
            string xmlNamespace = "http://www.w3.org/XML/1998/namespace";

            // none of them should be null.
            AssertNotNull("#1", xmlnsPrefix);
            AssertNotNull("#2", xmlPrefix);
            AssertNotNull("#3", stringEmpty);
            AssertNotNull("#4", xmlnsNamespace);
            AssertNotNull("#5", xmlNamespace);

            // Microsoft's XmlNamespaceManager reports that these three
            // namespaces aren't declared for some reason.
            Assert("#6", !namespaceManager.HasNamespace("xmlns"));
            Assert("#7", !namespaceManager.HasNamespace("xml"));
            Assert("#8", !namespaceManager.HasNamespace(String.Empty));

            // these three namespaces are declared by default.
            AssertEquals("#9", "http://www.w3.org/2000/xmlns/", namespaceManager.LookupNamespace("xmlns"));
            AssertEquals("#10", "http://www.w3.org/XML/1998/namespace", namespaceManager.LookupNamespace("xml"));
            AssertEquals("#11", String.Empty, namespaceManager.LookupNamespace(String.Empty));

            //NOTE: We can not use AssertSame is used ReferenceEquals
            // the namespaces should be the same references found in the name table.
            AssertEquals("#12", xmlnsNamespace, namespaceManager.LookupNamespace("xmlns"));
            AssertEquals("#13", xmlNamespace, namespaceManager.LookupNamespace("xml"));
            AssertEquals("#14", stringEmpty, namespaceManager.LookupNamespace(String.Empty));

            // looking up undeclared namespaces should return null.
            AssertNull("#15", namespaceManager.LookupNamespace("foo"));
        }

        [Test]
        public void AddNamespace()
        {
            // add a new namespace.
            namespaceManager.AddNamespace("foo", "http://foo/");
            // make sure the new namespace is there.
            Assert(namespaceManager.HasNamespace("foo"));
            AssertEquals("http://foo/", namespaceManager.LookupNamespace("foo"));
            // adding a different namespace with the same prefix
            // is allowed.
            namespaceManager.AddNamespace("foo", "http://foo1/");
            AssertEquals("http://foo1/", namespaceManager.LookupNamespace("foo"));
        }

        [Test]
        public void AddNamespaceWithNameTable()
        {
            // add a known reference to the name table.
            string fooNamespace = "http://foo/";
            nameTable.Add(fooNamespace);

            // create a new string with the same value but different address.
            string fooNamespace2 = "http://";
            fooNamespace2 += "foo/";

            // the references must be different in order for this test to prove anything.
            //Assert (!Equals (fooNamespace, fooNamespace2));

            // add the namespace with the reference that's not in the name table.
            namespaceManager.AddNamespace("foo", fooNamespace2);

            // the returned reference should be the same one that's in the name table.
            AssertSame(fooNamespace, namespaceManager.LookupNamespace("foo"));
        }

        [Test]
        public void AddNamespace_XmlPrefix()
        {
            namespaceManager.AddNamespace("xml", "http://www.w3.org/XML/1998/namespace");
            namespaceManager.AddNamespace("XmL", "http://foo/");
            namespaceManager.AddNamespace("xmlsomething", "http://foo/");
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void AddNamespace_XmlPrefix_Invalid()
        {
            namespaceManager.AddNamespace("xml", "http://foo/");
        }

        [Test]
        public void PushScope()
        {
            // add a new namespace.
            namespaceManager.AddNamespace("foo", "http://foo/");
            // make sure the new namespace is there.
            Assert(namespaceManager.HasNamespace("foo"));
            AssertEquals("http://foo/", namespaceManager.LookupNamespace("foo"));
            // push a new scope.
            namespaceManager.PushScope();
            // add a new namespace.
            namespaceManager.AddNamespace("bar", "http://bar/");
            // make sure the old namespace is not in this new scope.
            Assert(!namespaceManager.HasNamespace("foo"));
            // but we're still supposed to be able to lookup the old namespace.
            AssertEquals("http://foo/", namespaceManager.LookupNamespace("foo"));
            // make sure the new namespace is there.
            Assert(namespaceManager.HasNamespace("bar"));
            AssertEquals("http://bar/", namespaceManager.LookupNamespace("bar"));
        }

        [Test]
        public void PopScope()
        {
            // add some namespaces and a scope.
            PushScope();
            // pop the scope.
            Assert(namespaceManager.PopScope());
            // make sure the first namespace is still there.
            Assert(namespaceManager.HasNamespace("foo"));
            AssertEquals("http://foo/", namespaceManager.LookupNamespace("foo"));
            // make sure the second namespace is no longer there.
            Assert(!namespaceManager.HasNamespace("bar"));
            AssertNull(namespaceManager.LookupNamespace("bar"));
            // make sure there are no more scopes to pop.
            Assert(!namespaceManager.PopScope());
            // make sure that popping again doesn't cause an exception.
            Assert(!namespaceManager.PopScope());
        }

        [Test]
        public void PopScopeMustKeepAddedInScope()
        {
            namespaceManager = new XmlNamespaceManager(new NameTable()); // clear
            namespaceManager.AddNamespace("foo", "urn:foo");	// 0
            namespaceManager.AddNamespace("bar", "urn:bar");	// 0
            namespaceManager.PushScope();	// 1
            namespaceManager.PushScope();	// 2
            namespaceManager.PopScope();	// 2
            namespaceManager.PopScope();	// 1
            namespaceManager.PopScope();	// 0
            AssertEquals("urn:foo", namespaceManager.LookupNamespace("foo"));
            AssertEquals("urn:bar", namespaceManager.LookupNamespace("bar"));
        }

        [Test]
        public void AddPushPopRemove()
        {
            XmlNamespaceManager nsmgr =
                new XmlNamespaceManager(new NameTable());
            string ns = nsmgr.NameTable.Add("urn:foo");
            nsmgr.AddNamespace("foo", ns);
            AssertEquals("foo", nsmgr.LookupPrefix(ns));
            nsmgr.PushScope();
            AssertEquals("foo", nsmgr.LookupPrefix(ns));
            nsmgr.PopScope();
            AssertEquals("foo", nsmgr.LookupPrefix(ns));
            nsmgr.RemoveNamespace("foo", ns);
            AssertNull(nsmgr.LookupPrefix(ns));
        }

        [Test]
        public void LookupPrefix()
        {
            // This test should use an empty nametable.
            XmlNamespaceManager nsmgr =
                new XmlNamespaceManager(new NameTable());
            nsmgr.NameTable.Add("urn:hoge");
            nsmgr.NameTable.Add("urn:fuga");
            nsmgr.AddNamespace(string.Empty, "urn:hoge");
            AssertNull(nsmgr.LookupPrefix("urn:fuga"));
            AssertEquals(String.Empty, nsmgr.LookupPrefix("urn:hoge"));
        }

        string suffix = "oo";

        [Test]
        public void AtomizedLookup()
        {
            if (DateTime.Now.Year == 0)
                suffix = String.Empty;
            XmlNamespaceManager nsmgr =
                new XmlNamespaceManager(new NameTable());
            nsmgr.AddNamespace("foo", "urn:foo");
            AssertNotNull(nsmgr.LookupPrefix("urn:foo"));
            // FIXME: This returns registered URI inconsistently.
            //			AssertNull ("It is not atomized and thus should be failed", nsmgr.LookupPrefix ("urn:f" + suffix));
        }

#if NET_2_0
		XmlNamespaceScope l = XmlNamespaceScope.Local;
		XmlNamespaceScope x = XmlNamespaceScope.ExcludeXml;
		XmlNamespaceScope a = XmlNamespaceScope.All;

		[Test]
		[Category ("NotDotNet")] // MS bug
		public void GetNamespacesInScope ()
		{
			XmlNamespaceManager nsmgr =
				new XmlNamespaceManager (new NameTable ());

			AssertEquals ("#1", 0, nsmgr.GetNamespacesInScope (l).Count);
			AssertEquals ("#2", 0, nsmgr.GetNamespacesInScope (x).Count);
			AssertEquals ("#3", 1, nsmgr.GetNamespacesInScope (a).Count);

			nsmgr.AddNamespace ("foo", "urn:foo");
			AssertEquals ("#4", 1, nsmgr.GetNamespacesInScope (l).Count);
			AssertEquals ("#5", 1, nsmgr.GetNamespacesInScope (x).Count);
			AssertEquals ("#6", 2, nsmgr.GetNamespacesInScope (a).Count);

			// default namespace
			nsmgr.AddNamespace ("", "urn:empty");
			AssertEquals ("#7", 2, nsmgr.GetNamespacesInScope (l).Count);
			AssertEquals ("#8", 2, nsmgr.GetNamespacesInScope (x).Count);
			AssertEquals ("#9", 3, nsmgr.GetNamespacesInScope (a).Count);

			// PushScope
			nsmgr.AddNamespace ("foo", "urn:foo");
			nsmgr.PushScope ();
			AssertEquals ("#10", 0, nsmgr.GetNamespacesInScope (l).Count);
			AssertEquals ("#11", 2, nsmgr.GetNamespacesInScope (x).Count);
			AssertEquals ("#12", 3, nsmgr.GetNamespacesInScope (a).Count);

			// PopScope
			nsmgr.PopScope ();
			AssertEquals ("#13", 2, nsmgr.GetNamespacesInScope (l).Count);
			AssertEquals ("#14", 2, nsmgr.GetNamespacesInScope (x).Count);
			AssertEquals ("#15", 3, nsmgr.GetNamespacesInScope (a).Count);

			nsmgr.AddNamespace ("", "");
			// MS bug - it should return 1 for .Local but it returns 2 instead.
			AssertEquals ("#16", 1, nsmgr.GetNamespacesInScope (l).Count);
			AssertEquals ("#17", 1, nsmgr.GetNamespacesInScope (x).Count);
			AssertEquals ("#18", 2, nsmgr.GetNamespacesInScope (a).Count);
		}
#endif
    }
}
