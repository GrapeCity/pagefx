//CHANGED
//
// System.Xml.XmlReaderCommonTests
//
// Authors:
//   Atsushi Enomoto <ginga@kit.hi-ho.ne.jp>
//
// (C) 2003 Atsushi Enomoto
//  Note: Most of testcases are moved from XmlTextReaderTests.cs and
//  XmlNodeReaderTests.cs.
//

using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
#if NOT_PFX
using System.Xml.XPath;
#endif

using NUnit.Framework;

namespace MonoTests.System.Xml
{
    [TestFixture]
    public class XmlReaderTests : Assertion
    {
#if NOT_PFX
		[SetUp]
		public void GetReady ()
		{
			document = new XmlDocument ();
			document.LoadXml (xml1);
		}
#endif

#if NOT_PFX
		XmlDocument document;
        XmlNodeReader xnr;
#endif

        const string xml1 = "<root attr1='value1'><child /></root>";
        const string xml2 = "<root><foo/><bar>test.</bar></root>";
        const string xml3 = "<root>  test of <b>mixed</b> string.<![CDATA[ cdata string.]]></root>";
        const string xml4 = "<root>test of <b>mixed</b> string.</root>";
        XmlTextReader xtr;


        // copy from XmlTextReaderTests
        private void AssertStartDocument(XmlReader xmlReader)
        {
            Assert(xmlReader.ReadState == ReadState.Initial);
            Assert(xmlReader.NodeType == XmlNodeType.None);
            Assert(xmlReader.Depth == 0);
            Assert(!xmlReader.EOF);
        }

        private void AssertNode(
            XmlReader xmlReader,
            XmlNodeType nodeType,
            int depth,
            bool isEmptyElement,
            string name,
            string prefix,
            string localName,
            string namespaceURI,
            string value,
            int attributeCount)
        {
            AssertNode("", xmlReader, nodeType, depth,
                isEmptyElement, name, prefix, localName,
                namespaceURI, value, attributeCount);
        }

        private void AssertNode(
            string label,
            XmlReader xmlReader,
            XmlNodeType nodeType,
            int depth,
            bool isEmptyElement,
            string name,
            string prefix,
            string localName,
            string namespaceURI,
            string value,
            int attributeCount)
        {
            Assert(label + " Read() return value", xmlReader.Read());
            Assert(label + " ReadState", xmlReader.ReadState == ReadState.Interactive);
            Assert(label + " !EOF", !xmlReader.EOF);
            AssertNodeValues(label, xmlReader, nodeType, depth,
                isEmptyElement, name, prefix, localName,
                namespaceURI, value, value != String.Empty,
                attributeCount, attributeCount > 0);
        }

        private void AssertNodeValues(
            string label,
            XmlReader xmlReader,
            XmlNodeType nodeType,
            int depth,
            bool isEmptyElement,
            string name,
            string prefix,
            string localName,
            string namespaceURI,
            string value,
            int attributeCount)
        {
            AssertNodeValues(label, xmlReader, nodeType, depth,
                isEmptyElement, name, prefix, localName,
                namespaceURI, value, value != String.Empty,
                attributeCount, attributeCount > 0);
        }

        private void AssertNodeValues(
            string label,
            XmlReader xmlReader,
            XmlNodeType nodeType,
            int depth,
            bool isEmptyElement,
            string name,
            string prefix,
            string localName,
            string namespaceURI,
            string value,
            bool hasValue,
            int attributeCount,
            bool hasAttributes)
        {
            label = String.Concat(label, "(", xmlReader.GetType().Name, ")");
            AssertEquals(label + ": NodeType", nodeType, xmlReader.NodeType);
            AssertEquals(label + ": IsEmptyElement", isEmptyElement, xmlReader.IsEmptyElement);

            AssertEquals(label + ": name", name, xmlReader.Name);

            AssertEquals(label + ": prefix", prefix, xmlReader.Prefix);

            AssertEquals(label + ": localName", localName, xmlReader.LocalName);

            AssertEquals(label + ": namespaceURI", namespaceURI, xmlReader.NamespaceURI);

            AssertEquals(label + ": Depth", depth, xmlReader.Depth);

            AssertEquals(label + ": hasValue", hasValue, xmlReader.HasValue);

            AssertEquals(label + ": Value", value, xmlReader.Value);

            AssertEquals(label + ": hasAttributes", hasAttributes, xmlReader.HasAttributes);

            AssertEquals(label + ": attributeCount", attributeCount, xmlReader.AttributeCount);
        }

        private void AssertAttribute(
            XmlReader xmlReader,
            string name,
            string prefix,
            string localName,
            string namespaceURI,
            string value)
        {
            AssertEquals("value", value, xmlReader[name]);

            Assert(xmlReader.GetAttribute(name) == value);

            if (namespaceURI != String.Empty)
            {
                Assert(xmlReader[localName, namespaceURI] == value);
                Assert(xmlReader.GetAttribute(localName, namespaceURI) == value);
            }
        }

        private void AssertEndDocument(XmlReader xmlReader)
        {
            Assert("could read", !xmlReader.Read());
            AssertEquals("NodeType is not XmlNodeType.None", XmlNodeType.None, xmlReader.NodeType);
            AssertEquals("Depth is not 0", 0, xmlReader.Depth);
            AssertEquals("ReadState is not ReadState.EndOfFile", ReadState.EndOfFile, xmlReader.ReadState);
            Assert("not EOF", xmlReader.EOF);

            xmlReader.Close();
            AssertEquals("ReadState is not ReadState.Cosed", ReadState.Closed, xmlReader.ReadState);
        }

        private delegate void TestMethod(XmlReader reader);

        private void RunTest(string xml, TestMethod method)
        {
            xtr = new XmlTextReader(new StringReader(xml));
            method(xtr);

#if NOT_PFX
			// DTD validation
			xtr = new XmlTextReader (new StringReader (xml));
			XmlValidatingReader xvr = new XmlValidatingReader (xtr);
			xvr.ValidationType = ValidationType.DTD;
			xvr.EntityHandling = EntityHandling.ExpandCharEntities;
			method (xvr);

			// XSD validation
			xtr = new XmlTextReader (new StringReader (xml));
			xvr = new XmlValidatingReader (xtr);
			xvr.EntityHandling = EntityHandling.ExpandCharEntities;
			method (xvr);

			document.XmlResolver = null;
			document.LoadXml (xml);
			xnr = new XmlNodeReader (document);
			method (xnr);

#if NET_2_0
/*
			// XPathNavigatorReader tests
			System.Xml.XPath.XPathDocument doc = new System.Xml.XPath.XPathDocument (new StringReader (xml));
			XmlReader xpr = doc.CreateNavigator ().ReadSubtree ();
			method (xpr);
*/
#endif
#endif
        }





        [Test]
        public void InitialState()
        {
            RunTest(xml1, new TestMethod(InitialState));
        }

        private void InitialState(XmlReader reader)
        {
            AssertEquals("Depth", 0, reader.Depth);
            AssertEquals("EOF", false, reader.EOF);
            AssertEquals("HasValue", false, reader.HasValue);
            AssertEquals("IsEmptyElement", false, reader.IsEmptyElement);
            AssertEquals("LocalName", String.Empty, reader.LocalName);
            AssertEquals("NodeType", XmlNodeType.None, reader.NodeType);
            AssertEquals("ReadState", ReadState.Initial, reader.ReadState);
        }

        [Test]
        public void Read()
        {
            RunTest(xml1, new TestMethod(Read));
        }

        public void Read(XmlReader reader)
        {
            reader.Read();
            AssertEquals("<root>.NodeType", XmlNodeType.Element, reader.NodeType);
            AssertEquals("<root>.Name", "root", reader.Name);
            AssertEquals("<root>.ReadState", ReadState.Interactive, reader.ReadState);
            AssertEquals("<root>.Depth", 0, reader.Depth);

            // move to 'child'
            reader.Read();
            AssertEquals("<child/>.Depth", 1, reader.Depth);
            AssertEquals("<child/>.NodeType", XmlNodeType.Element, reader.NodeType);
            AssertEquals("<child/>.Name", "child", reader.Name);

            reader.Read();
            AssertEquals("</root>.Depth", 0, reader.Depth);
            AssertEquals("</root>.NodeType", XmlNodeType.EndElement, reader.NodeType);
            AssertEquals("</root>.Name", "root", reader.Name);

            reader.Read();
            AssertEquals("end.EOF", true, reader.EOF);
            AssertEquals("end.NodeType", XmlNodeType.None, reader.NodeType);
        }

        [Test]
        [Category("NotDotNet")]
        public void ReadAttributeValue()
        {
            RunTest("<root attr=''/>", new TestMethod(ReadAttributeValue));
        }

        public void ReadAttributeValue(XmlReader reader)
        {
            reader.Read();	// root
            Assert(reader.MoveToFirstAttribute());
            // It looks like that MS.NET shows AttributeCount and
            // HasAttributes as the same as element node!
            this.AssertNodeValues("#1",
                reader, XmlNodeType.Attribute,
                1, false, "attr", "", "attr", "", "", true, 1, true);
            Assert(reader.ReadAttributeValue());
            // MS.NET XmlTextReader fails. Its Prefix returns 
            // null instead of "". It is fixed in MS.NET 2.0.
            this.AssertNodeValues("#2",
                reader, XmlNodeType.Text,
                2, false, "", "", "", "", "", true, 1, true);
            Assert(reader.MoveToElement());
            this.AssertNodeValues("#3",
                reader, XmlNodeType.Element,
                0, true, "root", "", "root", "", "", false, 1, true);
        }

        [Test]
        public void ReadEmptyElement()
        {
            RunTest(xml2, new TestMethod(ReadEmptyElement));
        }

        public void ReadEmptyElement(XmlReader reader)
        {
            reader.Read();	// root
            AssertEquals(false, reader.IsEmptyElement);
            reader.Read();	// foo
            AssertEquals("foo", reader.Name);
            AssertEquals(true, reader.IsEmptyElement);
            reader.Read();	// bar
            AssertEquals("bar", reader.Name);
            AssertEquals(false, reader.IsEmptyElement);
        }

        [Test]
        public void ReadStringFromElement()
        {
            RunTest(xml3, new TestMethod(ReadStringFromElement));
        }

        public void ReadStringFromElement(XmlReader reader)
        {
            // Note: ReadString() test works only when the reader is
            // positioned at the container element.
            // In case the reader is positioned at the first 
            // character node, XmlTextReader and XmlNodeReader works
            // different!!

            reader.Read();
            string s = reader.ReadString();
            AssertEquals("readString.1.ret_val", "  test of ", s);
            AssertEquals("readString.1.Name", "b", reader.Name);
            s = reader.ReadString();
            AssertEquals("readString.2.ret_val", "mixed", s);
            AssertEquals("readString.2.NodeType", XmlNodeType.EndElement, reader.NodeType);
            s = reader.ReadString();	// never proceeds.
            AssertEquals("readString.3.ret_val", String.Empty, s);
            AssertEquals("readString.3.NodeType", XmlNodeType.EndElement, reader.NodeType);
            reader.Read();
            AssertEquals("readString.4.NodeType", XmlNodeType.Text, reader.NodeType);
            AssertEquals("readString.4.Value", " string.", reader.Value);
            s = reader.ReadString();	// reads the same Text node.
            AssertEquals("readString.5.ret_val", " string. cdata string.", s);
            AssertEquals("readString.5.NodeType", XmlNodeType.EndElement, reader.NodeType);
        }

        [Test]
        public void ReadInnerXml()
        {
            const string xml = "<root><foo>test of <b>mixed</b> string.</foo><bar /></root>";
            RunTest(xml, new TestMethod(ReadInnerXml));
        }

        public void ReadInnerXml(XmlReader reader)
        {
            reader.Read();
            reader.Read();
            AssertEquals("initial.ReadState", ReadState.Interactive, reader.ReadState);
            AssertEquals("initial.EOF", false, reader.EOF);
            AssertEquals("initial.NodeType", XmlNodeType.Element, reader.NodeType);
            string s = reader.ReadInnerXml();
            AssertEquals("read_all", "test of <b>mixed</b> string.", s);
            AssertEquals("after.Name", "bar", reader.Name);
            AssertEquals("after.NodeType", XmlNodeType.Element, reader.NodeType);
        }


        [Test]
        public void EmptyElement()
        {
            RunTest("<foo/>", new TestMethod(EmptyElement));
        }

        public void EmptyElement(XmlReader xmlReader)
        {

            AssertStartDocument(xmlReader);

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                0, // depth
                true, // isEmptyElement
                "foo", // name
                String.Empty, // prefix
                "foo", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
                0 // attributeCount
            );

            AssertEndDocument(xmlReader);
        }

        [Test]
        public void NestedEmptyTag()
        {
            string xml = "<foo><bar/></foo>";
            RunTest(xml, new TestMethod(NestedEmptyTag));
        }

        public void NestedEmptyTag(XmlReader xmlReader)
        {
            AssertStartDocument(xmlReader);

            AssertNode(
                "#1",
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                0, //depth
                false, // isEmptyElement
                "foo", // name
                String.Empty, // prefix
                "foo", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
                0 // attributeCount
            );

            AssertNode(
                "#2",
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                1, //depth
                true, // isEmptyElement
                "bar", // name
                String.Empty, // prefix
                "bar", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
                0 // attributeCount
            );

            AssertNode(
                "#3",
                xmlReader, // xmlReader
                XmlNodeType.EndElement, // nodeType
                0, //depth
                false, // isEmptyElement
                "foo", // name
                String.Empty, // prefix
                "foo", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
                0 // attributeCount
            );

            AssertEndDocument(xmlReader);
        }

        [Test]
        public void NestedText()
        {
            string xml = "<foo>bar</foo>";
            RunTest(xml, new TestMethod(NestedText));
        }

        public void NestedText(XmlReader xmlReader)
        {
            AssertStartDocument(xmlReader);

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                0, //depth
                false, // isEmptyElement
                "foo", // name
                String.Empty, // prefix
                "foo", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
                0 // attributeCount
            );

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Text, // nodeType
                1, //depth
                false, // isEmptyElement
                String.Empty, // name
                String.Empty, // prefix
                String.Empty, // localName
                String.Empty, // namespaceURI
                "bar", // value
                0 // attributeCount
            );

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.EndElement, // nodeType
                0, //depth
                false, // isEmptyElement
                "foo", // name
                String.Empty, // prefix
                "foo", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
                0 // attributeCount
            );

            AssertEndDocument(xmlReader);
        }

        [Test]
        public void EmptyElementWithAttributes()
        {
            string xml = @"<foo bar=""baz"" quux='quuux' x:foo='x-foo' xmlns:x = 'urn:xfoo' />";
            RunTest(xml, new TestMethod(EmptyElementWithAttributes));
        }

        public void EmptyElementWithAttributes(XmlReader xmlReader)
        {

            AssertStartDocument(xmlReader);

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                0, //depth
                true, // isEmptyElement
                "foo", // name
                String.Empty, // prefix
                "foo", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
                4 // attributeCount
            );

            AssertAttribute(
                xmlReader, // xmlReader
                "bar", // name
                String.Empty, // prefix
                "bar", // localName
                String.Empty, // namespaceURI
                "baz" // value
            );

            AssertAttribute(
                xmlReader, // xmlReader
                "quux", // name
                String.Empty, // prefix
                "quux", // localName
                String.Empty, // namespaceURI
                "quuux" // value
            );

            AssertAttribute(
                xmlReader, // xmlReader
                "notexist", // name
                String.Empty, // prefix
                "notexist", // localName
                String.Empty, // namespaceURI
                null // value
            );

            AssertAttribute(
                xmlReader, // xmlReader
                "x:foo", // name
                "x", // prefix
                "foo", // localName
                "urn:xfoo", // namespaceURI
                "x-foo" // value
            );

            AssertAttribute(
                xmlReader, // xmlReader
                "x:bar", // name
                "x", // prefix
                "bar", // localName
                "urn:xfoo", // namespaceURI
                null // value
            );

            AssertEndDocument(xmlReader);
        }

        [Test]
        public void ProcessingInstructionBeforeDocumentElement()
        {
            string xml = "<?foo bar?><baz/>";
            RunTest(xml, new TestMethod(ProcessingInstructionBeforeDocumentElement));
        }

        public void ProcessingInstructionBeforeDocumentElement(XmlReader xmlReader)
        {
            AssertStartDocument(xmlReader);

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.ProcessingInstruction, // nodeType
                0, //depth
                false, // isEmptyElement
                "foo", // name
                String.Empty, // prefix
                "foo", // localName
                String.Empty, // namespaceURI
                "bar", // value
                0 // attributeCount
            );

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                0, //depth
                true, // isEmptyElement
                "baz", // name
                String.Empty, // prefix
                "baz", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
                0 // attributeCount
            );

            AssertEndDocument(xmlReader);
        }

        [Test]
        public void CommentBeforeDocumentElement()
        {
            string xml = "<!--foo--><bar/>";
            RunTest(xml, new TestMethod(CommentBeforeDocumentElement));
        }

        public void CommentBeforeDocumentElement(XmlReader xmlReader)
        {
            AssertStartDocument(xmlReader);

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Comment, // nodeType
                0, //depth
                false, // isEmptyElement
                String.Empty, // name
                String.Empty, // prefix
                String.Empty, // localName
                String.Empty, // namespaceURI
                "foo", // value
                0 // attributeCount
            );

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                0, //depth
                true, // isEmptyElement
                "bar", // name
                String.Empty, // prefix
                "bar", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
                0 // attributeCount
            );

            AssertEndDocument(xmlReader);
        }

        [Test]
        public void PredefinedEntities()
        {
            string xml = "<foo>&lt;&gt;&amp;&apos;&quot;</foo>";
            RunTest(xml, new TestMethod(PredefinedEntities));
        }

        public void PredefinedEntities(XmlReader xmlReader)
        {
            AssertStartDocument(xmlReader);

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                0, //depth
                false, // isEmptyElement
                "foo", // name
                String.Empty, // prefix
                "foo", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
                0 // attributeCount
            );

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Text, // nodeType
                1, //depth
                false, // isEmptyElement
                String.Empty, // name
                String.Empty, // prefix
                String.Empty, // localName
                String.Empty, // namespaceURI
                "<>&'\"", // value
                0 // attributeCount
            );

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.EndElement, // nodeType
                0, //depth
                false, // isEmptyElement
                "foo", // name
                String.Empty, // prefix
                "foo", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
                0 // attributeCount
            );

            AssertEndDocument(xmlReader);
        }

        [Test]
        public void CharacterReferences()
        {
            string xml = "<foo>&#70;&#x4F;&#x4f;</foo>";
            RunTest(xml, new TestMethod(CharacterReferences));
        }

        public void CharacterReferences(XmlReader xmlReader)
        {
            AssertStartDocument(xmlReader);

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                0, //depth
                false, // isEmptyElement
                "foo", // name
                String.Empty, // prefix
                "foo", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
                0 // attributeCount
            );

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Text, // nodeType
                1, //depth
                false, // isEmptyElement
                String.Empty, // name
                String.Empty, // prefix
                String.Empty, // localName
                String.Empty, // namespaceURI
                "FOO", // value
                0 // attributeCount
            );

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.EndElement, // nodeType
                0, //depth
                false, // isEmptyElement
                "foo", // name
                String.Empty, // prefix
                "foo", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
                0 // attributeCount
            );

            AssertEndDocument(xmlReader);
        }

        [Test]
        public void PredefinedEntitiesInAttribute()
        {
            string xml = "<foo bar='&lt;&gt;&amp;&apos;&quot;'/>";
            RunTest(xml, new TestMethod(PredefinedEntitiesInAttribute));
        }

        public void PredefinedEntitiesInAttribute(XmlReader xmlReader)
        {
            AssertStartDocument(xmlReader);

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                0, //depth
                true, // isEmptyElement
                "foo", // name
                String.Empty, // prefix
                "foo", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
                1 // attributeCount
            );

            AssertAttribute(
                xmlReader, // xmlReader
                "bar", // name
                String.Empty, // prefix
                "bar", // localName
                String.Empty, // namespaceURI
                "<>&'\"" // value
            );

            AssertEndDocument(xmlReader);
        }

        [Test]
        public void CharacterReferencesInAttribute()
        {
            string xml = "<foo bar='&#70;&#x4F;&#x4f;'/>";
            RunTest(xml, new TestMethod(CharacterReferencesInAttribute));
        }

        public void CharacterReferencesInAttribute(XmlReader xmlReader)
        {
            AssertStartDocument(xmlReader);

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                0, //depth
                true, // isEmptyElement
                "foo", // name
                String.Empty, // prefix
                "foo", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
                1 // attributeCount
            );

            AssertAttribute(
                xmlReader, // xmlReader
                "bar", // name
                String.Empty, // prefix
                "bar", // localName
                String.Empty, // namespaceURI
                "FOO" // value
            );

            AssertEndDocument(xmlReader);
        }

        [Test]
        public void CDATA()
        {
            string xml = "<foo><![CDATA[<>&]]></foo>";
            RunTest(xml, new TestMethod(CDATA));
        }

        public void CDATA(XmlReader xmlReader)
        {
            AssertStartDocument(xmlReader);

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                0, //depth
                false, // isEmptyElement
                "foo", // name
                String.Empty, // prefix
                "foo", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
                0 // attributeCount
            );

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.CDATA, // nodeType
                1, //depth
                false, // isEmptyElement
                String.Empty, // name
                String.Empty, // prefix
                String.Empty, // localName
                String.Empty, // namespaceURI
                "<>&", // value
                0 // attributeCount
            );

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.EndElement, // nodeType
                0, //depth
                false, // isEmptyElement
                "foo", // name
                String.Empty, // prefix
                "foo", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
                0 // attributeCount
            );

            AssertEndDocument(xmlReader);
        }

        [Test]
        public void EmptyElementInDefaultNamespace()
        {
            string xml = @"<foo xmlns='http://foo/' />";
            RunTest(xml, new TestMethod(EmptyElementInDefaultNamespace));
        }

        public void EmptyElementInDefaultNamespace(XmlReader xmlReader)
        {
            AssertStartDocument(xmlReader);

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                0, // depth
                true, // isEmptyElement
                "foo", // name
                String.Empty, // prefix
                "foo", // localName
                "http://foo/", // namespaceURI
                String.Empty, // value
                1 // attributeCount
            );

            AssertAttribute(
                xmlReader, // xmlReader
                "xmlns", // name
                String.Empty, // prefix
                "xmlns", // localName
                "http://www.w3.org/2000/xmlns/", // namespaceURI
                "http://foo/" // value
            );

            AssertEquals("http://foo/", xmlReader.LookupNamespace(String.Empty));

            AssertEndDocument(xmlReader);
        }

        [Test]
        public void ChildElementInNamespace()
        {
            string xml = @"<foo:bar xmlns:foo='http://foo/'><baz:quux xmlns:baz='http://baz/' /></foo:bar>";
            RunTest(xml, new TestMethod(ChildElementInNamespace));
        }

        public void ChildElementInNamespace(XmlReader xmlReader)
        {
            AssertStartDocument(xmlReader);

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                0, // depth
                false, // isEmptyElement
                "foo:bar", // name
                "foo", // prefix
                "bar", // localName
                "http://foo/", // namespaceURI
                String.Empty, // value
                1 // attributeCount
            );

            AssertAttribute(
                xmlReader, // xmlReader
                "xmlns:foo", // name
                "xmlns", // prefix
                "foo", // localName
                "http://www.w3.org/2000/xmlns/", // namespaceURI
                "http://foo/" // value
            );

            AssertEquals("http://foo/", xmlReader.LookupNamespace("foo"));

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                1, // depth
                true, // isEmptyElement
                "baz:quux", // name
                "baz", // prefix
                "quux", // localName
                "http://baz/", // namespaceURI
                String.Empty, // value
                1 // attributeCount
            );

            AssertAttribute(
                xmlReader, // xmlReader
                "xmlns:baz", // name
                "xmlns", // prefix
                "baz", // localName
                "http://www.w3.org/2000/xmlns/", // namespaceURI
                "http://baz/" // value
            );

            AssertEquals("http://foo/", xmlReader.LookupNamespace("foo"));
            AssertEquals("http://baz/", xmlReader.LookupNamespace("baz"));

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.EndElement, // nodeType
                0, // depth
                false, // isEmptyElement
                "foo:bar", // name
                "foo", // prefix
                "bar", // localName
                "http://foo/", // namespaceURI
                String.Empty, // value
                0 // attributeCount
            );

            AssertEquals("http://foo/", xmlReader.LookupNamespace("foo"));
            AssertNull(xmlReader.LookupNamespace("baz"));

            AssertEndDocument(xmlReader);
        }

        [Test]
        public void ChildElementInDefaultNamespace()
        {
            string xml = @"<foo:bar xmlns:foo='http://foo/'><baz xmlns='http://baz/' /></foo:bar>";
            RunTest(xml, new TestMethod(ChildElementInDefaultNamespace));
        }

        public void ChildElementInDefaultNamespace(XmlReader xmlReader)
        {
            AssertStartDocument(xmlReader);

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                0, // depth
                false, // isEmptyElement
                "foo:bar", // name
                "foo", // prefix
                "bar", // localName
                "http://foo/", // namespaceURI
                String.Empty, // value
                1 // attributeCount
            );

            AssertAttribute(
                xmlReader, // xmlReader
                "xmlns:foo", // name
                "xmlns", // prefix
                "foo", // localName
                "http://www.w3.org/2000/xmlns/", // namespaceURI
                "http://foo/" // value
            );

            AssertEquals("http://foo/", xmlReader.LookupNamespace("foo"));

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                1, // depth
                true, // isEmptyElement
                "baz", // name
                String.Empty, // prefix
                "baz", // localName
                "http://baz/", // namespaceURI
                String.Empty, // value
                1 // attributeCount
            );

            AssertAttribute(
                xmlReader, // xmlReader
                "xmlns", // name
                String.Empty, // prefix
                "xmlns", // localName
                "http://www.w3.org/2000/xmlns/", // namespaceURI
                "http://baz/" // value
            );

            AssertEquals("http://foo/", xmlReader.LookupNamespace("foo"));
            AssertEquals("http://baz/", xmlReader.LookupNamespace(String.Empty));

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.EndElement, // nodeType
                0, // depth
                false, // isEmptyElement
                "foo:bar", // name
                "foo", // prefix
                "bar", // localName
                "http://foo/", // namespaceURI
                String.Empty, // value
                0 // attributeCount
            );

            AssertEquals("http://foo/", xmlReader.LookupNamespace("foo"));

            AssertEndDocument(xmlReader);
        }

        [Test]
        public void AttributeInNamespace()
        {
            string xml = @"<foo bar:baz='quux' xmlns:bar='http://bar/' />";
            RunTest(xml, new TestMethod(AttributeInNamespace));
        }

        public void AttributeInNamespace(XmlReader xmlReader)
        {
            AssertStartDocument(xmlReader);

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                0, // depth
                true, // isEmptyElement
                "foo", // name
                String.Empty, // prefix
                "foo", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
                2 // attributeCount
            );

            AssertAttribute(
                xmlReader, // xmlReader
                "bar:baz", // name
                "bar", // prefix
                "baz", // localName
                "http://bar/", // namespaceURI
                "quux" // value
            );

            AssertAttribute(
                xmlReader, // xmlReader
                "xmlns:bar", // name
                "xmlns", // prefix
                "bar", // localName
                "http://www.w3.org/2000/xmlns/", // namespaceURI
                "http://bar/" // value
            );

            AssertEquals("http://bar/", xmlReader.LookupNamespace("bar"));

            AssertEndDocument(xmlReader);
        }

        [Test]
        public void MoveToElementFromAttribute()
        {
            string xml = @"<foo bar=""baz"" />";
            RunTest(xml, new TestMethod(MoveToElementFromAttribute));
        }

        public void MoveToElementFromAttribute(XmlReader xmlReader)
        {
            Assert(xmlReader.Read());
            AssertEquals(XmlNodeType.Element, xmlReader.NodeType);
            Assert(xmlReader.MoveToFirstAttribute());
            AssertEquals(XmlNodeType.Attribute, xmlReader.NodeType);
            Assert(xmlReader.MoveToElement());
            AssertEquals(XmlNodeType.Element, xmlReader.NodeType);
        }

        [Test]
        public void MoveToElementFromElement()
        {
            string xml = @"<foo bar=""baz"" />";
            RunTest(xml, new TestMethod(MoveToElementFromElement));
        }

        public void MoveToElementFromElement(XmlReader xmlReader)
        {
            Assert(xmlReader.Read());
            AssertEquals(XmlNodeType.Element, xmlReader.NodeType);
            Assert(!xmlReader.MoveToElement());
            AssertEquals(XmlNodeType.Element, xmlReader.NodeType);
        }

        [Test]
        public void MoveToFirstAttributeWithNoAttributes()
        {
            string xml = @"<foo />";
            RunTest(xml, new TestMethod(MoveToFirstAttributeWithNoAttributes));
        }

        public void MoveToFirstAttributeWithNoAttributes(XmlReader xmlReader)
        {
            Assert(xmlReader.Read());
            AssertEquals(XmlNodeType.Element, xmlReader.NodeType);
            Assert(!xmlReader.MoveToFirstAttribute());
            AssertEquals(XmlNodeType.Element, xmlReader.NodeType);
        }

        [Test]
        public void MoveToNextAttributeWithNoAttributes()
        {
            string xml = @"<foo />";
            RunTest(xml, new TestMethod(MoveToNextAttributeWithNoAttributes));
        }

        public void MoveToNextAttributeWithNoAttributes(XmlReader xmlReader)
        {
            Assert(xmlReader.Read());
            AssertEquals(XmlNodeType.Element, xmlReader.NodeType);
            Assert(!xmlReader.MoveToNextAttribute());
            AssertEquals(XmlNodeType.Element, xmlReader.NodeType);
        }

        [Test]
        public void MoveToNextAttribute()
        {
            string xml = @"<foo bar=""baz"" quux='quuux'/>";
            RunTest(xml, new TestMethod(MoveToNextAttribute));
        }

        public void MoveToNextAttribute(XmlReader xmlReader)
        {
            AssertStartDocument(xmlReader);

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                0, //depth
                true, // isEmptyElement
                "foo", // name
                String.Empty, // prefix
                "foo", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
                2 // attributeCount
            );

            AssertAttribute(
                xmlReader, // xmlReader
                "bar", // name
                String.Empty, // prefix
                "bar", // localName
                String.Empty, // namespaceURI
                "baz" // value
            );

            AssertAttribute(
                xmlReader, // xmlReader
                "quux", // name
                String.Empty, // prefix
                "quux", // localName
                String.Empty, // namespaceURI
                "quuux" // value
            );

            Assert(xmlReader.MoveToNextAttribute());
            AssertEquals("bar", xmlReader.Name);
            AssertEquals("baz", xmlReader.Value);

            Assert(xmlReader.MoveToNextAttribute());
            AssertEquals("quux", xmlReader.Name);
            AssertEquals("quuux", xmlReader.Value);

            Assert(!xmlReader.MoveToNextAttribute());

            Assert(xmlReader.MoveToElement());

            AssertNodeValues(
                "#1",
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                0, //depth
                true, // isEmptyElement
                "foo", // name
                String.Empty, // prefix
                "foo", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
                2 // attributeCount
            );

            AssertEndDocument(xmlReader);
        }

        [Test]
        //		[Category ("NotDotNet")] // MS XmlNodeReader never moves to xml declaration.
        [Ignore("Too inconsistent reference implementations to determine which is correct behavior.")]
        public void MoveToXmlDeclAttributes()
        {
            string xml = "<?xml version=\"1.0\" standalone=\"yes\"?><root/>";
            RunTest(xml, new TestMethod(MoveToXmlDeclAttributes));
        }

        public void MoveToXmlDeclAttributes(XmlReader xmlReader)
        {
            xmlReader.Read();
            this.AssertNodeValues("#1", xmlReader,
                XmlNodeType.XmlDeclaration,
                0,
                false,
                "xml",
                String.Empty,
                "xml",
                String.Empty,
                "version=\"1.0\" standalone=\"yes\"",
                2);
            Assert("MoveToFirstAttribute",
                xmlReader.MoveToFirstAttribute());
            this.AssertNodeValues("#2", xmlReader,
                XmlNodeType.Attribute,
                0, // FIXME: might be 1
                false,
                "version",
                String.Empty,
                "version",
                String.Empty,
                "1.0",
                2);
            xmlReader.ReadAttributeValue();
            this.AssertNodeValues("#3", xmlReader,
                XmlNodeType.Text,
                1, // FIXME might be 2
                false,
                String.Empty,
                null, // FIXME: should be String.Empty,
                String.Empty,
                null, // FIXME: should be String.Empty,
                "1.0",
                2);
            xmlReader.MoveToNextAttribute();
            this.AssertNodeValues("#4", xmlReader,
                XmlNodeType.Attribute,
                0, // FIXME: might be 1
                false,
                "standalone",
                String.Empty,
                "standalone",
                String.Empty,
                "yes",
                2);
            xmlReader.ReadAttributeValue();
            this.AssertNodeValues("#5", xmlReader,
                XmlNodeType.Text,
                1, // FIXME: might be 2
                false,
                String.Empty,
                null, // FIXME: should be String.Empty,
                String.Empty,
                null, // FIXME: should be String.Empty,
                "yes",
                2);
        }

        [Test]
        public void AttributeOrder()
        {
            string xml = @"<foo _1='1' _2='2' _3='3' />";
            RunTest(xml, new TestMethod(AttributeOrder));
        }

        public void AttributeOrder(XmlReader xmlReader)
        {
            Assert(xmlReader.Read());
            AssertEquals(XmlNodeType.Element, xmlReader.NodeType);

            Assert(xmlReader.MoveToFirstAttribute());
            AssertEquals("_1", xmlReader.Name);
            Assert(xmlReader.MoveToNextAttribute());
            AssertEquals("_2", xmlReader.Name);
            Assert(xmlReader.MoveToNextAttribute());
            AssertEquals("_3", xmlReader.Name);

            Assert(!xmlReader.MoveToNextAttribute());
        }

        [Test]
        [Category("NotDotNet")]
        public void IndexerAndAttributes()
        {
            string xml = @"<?xml version='1.0' standalone='no'?><foo _1='1' _2='2' _3='3' />";
            RunTest(xml, new TestMethod(IndexerAndAttributes));
        }

        public void IndexerAndAttributes(XmlReader xmlReader)
        {
            Assert(xmlReader.Read());
            AssertEquals("1.0", xmlReader["version"]);
            AssertEquals("1.0", xmlReader.GetAttribute("version"));
            // .NET 1.1 BUG. XmlTextReader returns null, while XmlNodeReader returns "".
            AssertEquals(null, xmlReader["encoding"]);
            AssertEquals(null, xmlReader.GetAttribute("encoding"));
            AssertEquals("no", xmlReader["standalone"]);
            AssertEquals("no", xmlReader.GetAttribute("standalone"));
            AssertEquals("1.0", xmlReader[0]);
            AssertEquals("1.0", xmlReader.GetAttribute(0));
            AssertEquals("no", xmlReader[1]);
            AssertEquals("no", xmlReader.GetAttribute(1));

            Assert(xmlReader.Read());
            AssertEquals(XmlNodeType.Element, xmlReader.NodeType);
            AssertEquals("1", xmlReader["_1"]);

            Assert(xmlReader.MoveToFirstAttribute());
            AssertEquals("_1", xmlReader.Name);
            AssertEquals("1", xmlReader["_1"]);
            Assert(xmlReader.MoveToNextAttribute());
            AssertEquals("_2", xmlReader.Name);
            AssertEquals("1", xmlReader["_1"]);
            Assert(xmlReader.MoveToNextAttribute());
            AssertEquals("_3", xmlReader.Name);
            AssertEquals("1", xmlReader["_1"]);

            Assert(!xmlReader.MoveToNextAttribute());
        }

        [Test]
        public void ProhibitedMultipleAttributes()
        {
            string xml = @"<foo _1='1' _1='1' />";
            try
            {
                RunTest(xml, new TestMethod(ReadAll));
            }
            catch (XmlException)
            {
            }
            xml = @"<foo _1='1' _1='2' />";
            try
            {
                RunTest(xml, new TestMethod(ReadAll));
            }
            catch (XmlException)
            {
            }
        }

        public void ReadAll(XmlReader xmlReader)
        {
            while (!xmlReader.EOF)
                xmlReader.Read();
        }

        [Test]
        public void SurrogatePairContent()
        {
            string xml = "<root xmlns='&#x10100;'/>";
            RunTest(xml, new TestMethod(SurrogatePairContent));
        }

        public void SurrogatePairContent(XmlReader xmlReader)
        {
            xmlReader.Read();
            AssertEquals(true, xmlReader.MoveToAttribute("xmlns"));
            AssertEquals("xmlns", xmlReader.Name);
            AssertEquals(2, xmlReader.Value.Length);
            AssertEquals(0xD800, (int)xmlReader.Value[0]);
            AssertEquals(0xDD00, (int)xmlReader.Value[1]);
        }

        [Test]
        public void ReadOuterXmlOnEndElement()
        {
            string xml = "<root><foo></foo></root>";
            RunTest(xml, new TestMethod(ReadOuterXmlOnEndElement));
        }

        public void ReadOuterXmlOnEndElement(XmlReader xmlReader)
        {
            xmlReader.Read();
            xmlReader.Read();
            xmlReader.Read();
            AssertEquals(String.Empty, xmlReader.ReadOuterXml());
        }

        [Test]
        public void ReadInnerXmlOnEndElement()
        {
            string xml = "<root><foo></foo></root>";
            RunTest(xml, new TestMethod(ReadInnerXmlOnEndElement));
        }

        private void ReadInnerXmlOnEndElement(XmlReader xmlReader)
        {
            xmlReader.Read();
            xmlReader.Read();
            xmlReader.Read();
            AssertEquals(String.Empty, xmlReader.ReadInnerXml());
        }

        [Test]
        public void LookupEmptyPrefix()
        {
            string xml = "<root><foo></foo></root>";
            RunTest(xml, new TestMethod(LookupEmptyPrefix));
        }

        void LookupEmptyPrefix(XmlReader xmlReader)
        {
            xmlReader.Read();
            AssertNull(xmlReader.LookupNamespace(String.Empty));
        }

        [Test]
        public void ReadStartElement()
        {
            string xml = "<root>test</root>";
            RunTest(xml, new TestMethod(ReadStartElement));
        }

        void ReadStartElement(XmlReader xr)
        {
            xr.Read();
            xr.ReadStartElement();
            // consume Element node.
            AssertEquals(XmlNodeType.Text, xr.NodeType);
        }

        [Test]
        public void LookupNamespaceAtEndElement()
        {
            string xml = "<root xmlns:x='urn:foo'><foo/></root>";
            RunTest(xml, new TestMethod(LookupNamespaceAtEndElement));
        }

        void LookupNamespaceAtEndElement(XmlReader reader)
        {
            reader.Read();
            AssertEquals("#1", "urn:foo", reader.LookupNamespace("x"));
            reader.Read();
            AssertEquals("#2", "urn:foo", reader.LookupNamespace("x"));
            reader.Read();
            AssertEquals("#3", "urn:foo", reader.LookupNamespace("x"));
        }

        [Test]
        public void ReadClosedReader()
        {
            string xml = "<fin>aaa</fin>";
            RunTest(xml, new TestMethod(ReadClosedReader));
        }

        void ReadClosedReader(XmlReader reader)
        {
            reader.Read();
            reader.Close();
            reader.Read(); // silently returns false
        }


#if NET_2_0
#if NOT_PFX
		[Test]
		public void CreateSimple ()
		{
			XmlReaderSettings s = new XmlReaderSettings ();
			s.ProhibitDtd = false;
			XmlReader xr = XmlReader.Create ("Test/XmlFiles/nested-dtd-test.xml", s);
			xr.Read ();
			AssertEquals ("#1", XmlNodeType.DocumentType, xr.NodeType);
			xr.Read ();
			AssertEquals ("#2", XmlNodeType.Whitespace, xr.NodeType);
			xr.Read ();
			AssertEquals ("#3", XmlNodeType.Element, xr.NodeType);
		}

		[Test]
		[ExpectedException (typeof (XmlException))]
		public void CreateSimpleProhibitDtd ()
		{
			XmlReader xr = XmlReader.Create ("Test/XmlFiles/nested-dtd-test.xml");
			xr.Read ();
		}


[Test]
		// a bit revised version of bug #78706
		public void CreateFromUrlClose ()
		{
			string file = "Test/XmlFiles/78706.xml";
			try {
				if (!File.Exists (file))
					File.Create (file).Close ();
				XmlReaderSettings s = new XmlReaderSettings ();
				s.CloseInput = false; // explicitly
				XmlReader r = XmlReader.Create (file, s);
				r.Close ();
				XmlTextWriter w = new XmlTextWriter (file, null);
				w.Close ();
			} finally {
				if (File.Exists (file))
					File.Delete (file);
			}
		}
#endif

		[Test]
		public void ReadToDescendant ()
		{
			string xml = @"<root><foo/><bar/><foo> test text <bar><bar></bar></bar></foo></root>";
			RunTest (xml, new TestMethod (ReadToDescendant));
		}

		void ReadToDescendant (XmlReader xmlReader)
		{
			// move to first <bar/>
			Assert ("#1", xmlReader.ReadToDescendant ("bar"));
			// no children in <bar/>. It is empty.
			Assert ("#2", !xmlReader.ReadToDescendant ("bar"));
			AssertEquals ("#2-2", "bar", xmlReader.Name);

			// move to the second <foo>
			xmlReader.Read ();
			// move to the second <bar>
			Assert ("#3", xmlReader.ReadToDescendant ("bar"));
			// move to <bar> inside <bar>...</bar>
			Assert ("#4", xmlReader.ReadToDescendant ("bar"));
			// the next is EndElement of </bar>, so no move.
			Assert ("#5", !xmlReader.ReadToDescendant ("bar"));
			AssertEquals ("#5-2", XmlNodeType.EndElement, xmlReader.NodeType);
		}

		[Test]
		public void ReadToDescepdant2 ()
		{
			string xml = "<root/>";
			RunTest (xml, new TestMethod (ReadToDescendant2));
		}

		void ReadToDescendant2 (XmlReader xmlReader)
		{
			// make sure that it works when the reader is at Initial state.
			Assert (xmlReader.ReadToDescendant ("root"));
		}

		[Test]
		public void ReadToFollowing ()
		{
			string xml = @"<root><foo/><bar/><foo><bar><bar></bar></bar></foo></root>";
			RunTest (xml, new TestMethod (ReadToFollowing));
		}

		public void ReadToFollowing (XmlReader xmlReader)
		{
			Assert ("#1", xmlReader.ReadToFollowing ("bar"));
			Assert ("#2", xmlReader.ReadToFollowing ("bar"));
			AssertEquals ("#2-2", 2, xmlReader.Depth);
			Assert ("#3", xmlReader.ReadToFollowing ("bar"));
			AssertEquals ("#3-2", 3, xmlReader.Depth);
			Assert ("#4", !xmlReader.ReadToFollowing ("bar"));
		}

		[Test]
		[Category ("NotDotNet")]
		public void ReadToNextSiblingAtInitialState ()
		{
			string xml = @"<root></root>";
			RunTest (xml, new TestMethod (ReadToNextSiblingAtInitialState ));
		}

		void ReadToNextSiblingAtInitialState (XmlReader xmlReader)
		{
			Assert ("#1", !xmlReader.ReadToNextSibling ("bar"));
			Assert ("#2", !xmlReader.ReadToNextSibling ("root"));
		}

		[Test]
		public void ReadToNextSibling ()
		{
			string xml = @"<root><foo/><bar attr='value'/><foo><pooh/><bar></bar><foo></foo><bar/></foo></root>";
			RunTest (xml, new TestMethod (ReadToNextSibling));
		}

		void ReadToNextSibling (XmlReader xmlReader)
		{
			// It is funky, but without it MS.NET results in an infinite loop.
			xmlReader.Read (); // root

			xmlReader.Read (); // foo
			Assert ("#3", xmlReader.ReadToNextSibling ("bar"));

			AssertEquals ("#3-2", "value", xmlReader.GetAttribute ("attr"));
			xmlReader.Read (); // foo
			xmlReader.Read (); // pooh
			Assert ("#4", xmlReader.ReadToNextSibling ("bar"));
			Assert ("#4-2", !xmlReader.IsEmptyElement);
			Assert ("#5", xmlReader.ReadToNextSibling ("bar"));
			Assert ("#5-2", xmlReader.IsEmptyElement);
			Assert ("#6", xmlReader.Read ()); // /foo

			AssertNodeValues ("#7", xmlReader,
				XmlNodeType.EndElement,
				1,		// Depth
				false,		// IsEmptyElement
				"foo",		// Name
				String.Empty,	// Prefix
				"foo",		// LocalName
				String.Empty,	// NamespaceURI
				String.Empty,	// Value
				false,		// HasValue
				0,		// AttributeCount
				false);		// HasAttributes
		}

		// bug #81451
		[Test]
		public void ReadToNextSibling2 ()
		{
			string xml = @"<root><baz><bar><foo attr='value'/></bar><foo attr='value2'><bar><foo /></bar></foo></baz></root>";
			RunTest (xml, new TestMethod (ReadToNextSibling2));
		}

		void ReadToNextSibling2 (XmlReader r)
		{
			r.MoveToContent (); // ->root
			r.Read (); // root->baz
			r.Read (); // baz->bar
			Assert ("#1", r.ReadToNextSibling ("foo"));
			AssertEquals ("#2", "value2", r.GetAttribute ("attr"));
			r.Read (); // foo[@value='value2']->bar
			Assert ("#3", !r.ReadToNextSibling ("foo"));
			AssertEquals ("#4", XmlNodeType.EndElement, r.NodeType);
			AssertEquals ("#5", "foo", r.LocalName);
		}

		// bug #347768
		[Test]
		public void ReadToNextSibling3 ()
		{
			string xml = @" <books> <book> <name>Happy C Sharp</name> </book> </books>";
			XmlReader reader = XmlReader.Create (new StringReader (xml));

			reader.MoveToContent ();

			while (reader.Read ())
				reader.ReadToNextSibling ("book"); // should not result in an infinite loop
		}

		[Test]
		public void ReadSubtree ()
		{
			string xml = @"<root><foo/><bar attr='value'></bar></root>";
			RunTest (xml, new TestMethod (ReadSubtree));
		}

		void ReadSubtree (XmlReader reader)
		{
			reader.MoveToContent (); // root
			reader.Read (); // foo
			XmlReader st = reader.ReadSubtree (); // <foo/>

			// MS bug: IsEmptyElement should be false here.
			/*
			AssertNodeValues ("#1", st,
				XmlNodeType.None,
				0,		// Depth
				false,		// IsEmptyElement
				String.Empty,	// Name
				String.Empty,	// Prefix
				String.Empty,	// LocalName
				String.Empty,	// NamespaceURI
				String.Empty,	// Value
				false,		// HasValue
				0,		// AttributeCount
				false);		// HasAttributes
			*/
			AssertEquals ("#1", XmlNodeType.None, st.NodeType);

			st.Read ();
			AssertNodeValues ("#2", st,
				XmlNodeType.Element,
				0,
				true,		// IsEmptyElement
				"foo",		// Name
				String.Empty,	// Prefix
				"foo",		// LocalName
				String.Empty,	// NamespaceURI
				String.Empty,	// Value
				false,		// HasValue
				0,		// AttributeCount
				false);		// HasAttributes

			Assert ("#3", !st.Read ());

			// At this state, reader is not positioned on <bar> yet
			AssertNodeValues ("#3-2", reader,
				XmlNodeType.Element,
				1,		// Depth. It is 1 for main tree.
				true,		// IsEmptyElement
				"foo",		// Name
				String.Empty,	// Prefix
				"foo",		// LocalName
				String.Empty,	// NamespaceURI
				String.Empty,	// Value
				false,		// HasValue
				0,		// AttributeCount
				false);		// HasAttributes

			reader.Read ();

			AssertNodeValues ("#4", reader,
				XmlNodeType.Element,
				1,		// Depth. It is 1 for main tree.
				false,		// IsEmptyElement
				"bar",		// Name
				String.Empty,	// Prefix
				"bar",		// LocalName
				String.Empty,	// NamespaceURI
				String.Empty,	// Value
				false,		// HasValue
				1,		// AttributeCount
				true);		// HasAttributes

			st = reader.ReadSubtree ();
			st.Read (); // Initial -> Interactive
			AssertNodeValues ("#5", st,
				XmlNodeType.Element,
				0,		// Depth. It is 0 for subtree.
				false,		// IsEmptyElement
				"bar",		// Name
				String.Empty,	// Prefix
				"bar",		// LocalName
				String.Empty,	// NamespaceURI
				String.Empty,	// Value
				false,		// HasValue
				1,		// AttributeCount
				true);		// HasAttributes

			st.Read ();
			AssertNodeValues ("#6-1", st,
				XmlNodeType.EndElement,
				0,		// Depth. It is 0 for subtree.
				false,		// IsEmptyElement
				"bar",		// Name
				String.Empty,	// Prefix
				"bar",		// LocalName
				String.Empty,	// NamespaceURI
				String.Empty,	// Value
				false,		// HasValue
				0,		// AttributeCount
				false);		// HasAttributes

			AssertNodeValues ("#6-2", st,
				XmlNodeType.EndElement,
				0,		// Depth. It is 0 for subtree.
				false,		// IsEmptyElement
				"bar",		// Name
				String.Empty,	// Prefix
				"bar",		// LocalName
				String.Empty,	// NamespaceURI
				String.Empty,	// Value
				false,		// HasValue
				0,		// AttributeCount
				false);		// HasAttributes

			Assert ("#7", !st.Read ());
            }

#if NOT_PFX

		[Test]
		public void ReadInteger ()
		{
			string xml1 = "<root>1</root>";
			XmlReader xr;
			
			xr = XmlReader.Create (new StringReader (xml1));
			xr.Read ();
			AssertEquals ("#1", "1", xr.ReadElementContentAsString ());

			AssertNodeValues ("#1-2", xr,
				XmlNodeType.None,
				0,		// Depth. It is 0 for subtree.
				false,		// IsEmptyElement
				String.Empty,	// Name
				String.Empty,	// Prefix
				String.Empty,	// LocalName
				String.Empty,	// NamespaceURI
				String.Empty,	// Value
				false,		// HasValue
				0,		// AttributeCount
				false);		// HasAttributes

			xr = XmlReader.Create (new StringReader (xml1));
			xr.Read ();
			// this XmlReader has no schema, thus the value is untyped
			AssertEquals ("#2", "1", xr.ReadElementContentAsObject ());

			xr = XmlReader.Create (new StringReader (xml1));
			xr.Read ();
			xr.Read ();
			AssertEquals ("#3", "1", xr.ReadContentAsString ());
			xr = XmlReader.Create (new StringReader (xml1));
			xr.Read ();
			AssertEquals ("#4", 1, xr.ReadElementContentAsInt ());
			xr = XmlReader.Create (new StringReader (xml1));
			xr.Read ();
			AssertEquals ("#5", 1, xr.ReadElementContentAs (typeof (int), null));
        }

        [Test]
		[ExpectedException (typeof (XmlException))]
		public void ReadContentAsIntFail ()
		{
			XmlReader xr = XmlReader.Create (
				new StringReader ("<doc>1.0</doc>"));
			xr.Read ();
			xr.ReadElementContentAsInt ();
		}

        [Test]
		public void ReadDateTime ()
		{
			DateTime time = new DateTime (2006, 1, 2, 3, 4, 56);
			string xml1 = "<root>2006-01-02T03:04:56</root>";
			XmlReader xr;

			xr = XmlReader.Create (new StringReader (xml1));
			xr.Read ();
			// this XmlReader has no schema, thus the value is untyped
			AssertEquals ("#1", "2006-01-02T03:04:56",
				xr.ReadElementContentAsString ());
            xr = XmlReader.Create (new StringReader (xml1));
			xr.Read ();
			xr.Read ();
			AssertEquals ("#2", time, xr.ReadContentAsDateTime ());

			xr = XmlReader.Create (new StringReader (xml1));
			xr.Read ();
			AssertEquals ("#3", time, xr.ReadElementContentAsDateTime ());
            xr = XmlReader.Create (new StringReader (xml1));
			xr.Read ();
			AssertEquals ("#4", time, xr.ReadElementContentAs (typeof (DateTime), null));
		}

        [Test]
		[ExpectedException (typeof (XmlException))]
		public void ReadContentAsDateTimeFail ()
		{
			XmlReader xr = XmlReader.Create (
				new StringReader ("<doc>P1Y2M3D</doc>"));
			xr.Read ();
			xr.ReadElementContentAsDateTime ();
		}
#endif

		[Test]
		public void ReadContentAs_QNameEmptyNSResolver ()
		{
			XmlReader xr = XmlReader.Create (
				new StringReader ("<doc xmlns:x='urn:foo'>x:el</doc>"));
			xr.Read ();
			object o = xr.ReadElementContentAs (
				typeof (XmlQualifiedName), null);
			// without IXmlNamespaceResolver, it still resolves
			// x:el as valid QName.
			AssertNotNull ("#1", o);
			XmlQualifiedName q = o as XmlQualifiedName;
			AssertEquals ("#2 : " + o.GetType (),
				new XmlQualifiedName ("el", "urn:foo"), q);
		}

		[Test]
		[ExpectedException (typeof (InvalidOperationException))]
		public void ReadContentStringOnElementFail ()
		{
			XmlReader xr = XmlReader.Create (new StringReader ("<a>test</a>"));
			xr.Read ();
			xr.ReadContentAsString ();
		}

		[Test]
		[ExpectedException (typeof (XmlException))]
		public void ReadElementContentStringMixedContent ()
		{
			XmlReader xr = XmlReader.Create (
				new StringReader ("<doc>123<child>456</child>789</doc>"));
			xr.Read ();
			// "child" is regarded as an invalid node.
			string s = xr.ReadElementContentAsString ();
		}

		[Test]
		public void ReadContentStringMixedContent ()
		{
			XmlReader xr = XmlReader.Create (
				new StringReader ("<doc>123<child>456</child>789</doc>"));
			xr.Read ();
			xr.Read (); // from Text "123"
			string s = xr.ReadContentAsString ();
			AssertEquals ("#1", "123", s);
			AssertEquals ("#2", XmlNodeType.Element, xr.NodeType);
		}

		[Test]
		public void ReadElementContentAsString ()
		{
			XmlTextReader r = new XmlTextReader (
				"<root/>", XmlNodeType.Document, null);
			r.Read ();
			AssertEquals ("#1",
				String.Empty, r.ReadElementContentAsString ());
			AssertEquals ("#2",
				XmlNodeType.None, r.NodeType);
		}

		[Test]
		public void ReadElementContentAs ()
		{
			// as System.Object

			XmlTextReader r = new XmlTextReader (
				"<root/>", XmlNodeType.Document, null);
			r.Read ();
			AssertEquals ("#1",
				String.Empty, r.ReadElementContentAs (typeof (object), null));
			AssertEquals ("#2",
				XmlNodeType.None, r.NodeType);

			// regardless of its value, the return value is string.
			r = new XmlTextReader ("<root>1</root>", XmlNodeType.Document, null);
			r.Read ();
			AssertEquals ("#3",
				"1", r.ReadElementContentAs (typeof (object), null));
			AssertEquals ("#4",
				XmlNodeType.None, r.NodeType);
		}

		[Test]
		public void ReadContentStringOnAttribute ()
		{
			string xml = @"<root id='myId'><child /></root>";
			RunTest (xml, new TestMethod (ReadContentStringOnAttribute));
		}

		void ReadContentStringOnAttribute (XmlReader reader)
		{
			reader.Read ();
			Assert (reader.MoveToAttribute ("id"));
			AssertEquals ("myId", reader.ReadContentAsString ());
		}

		[Test]
		public void ReadElementContentAsStringEmpty ()
		{
			string xml = "<root><sample/></root>";
			RunTest (xml, new TestMethod (ReadElementContentAsStringEmpty));
		}

		void ReadElementContentAsStringEmpty (XmlReader reader)
		{
			reader.MoveToContent ();
			reader.Read ();
			AssertEquals (String.Empty, reader.ReadElementContentAsString ("sample", ""));
			AssertEquals (XmlNodeType.EndElement, reader.NodeType);
		}

		[Test]
		public void ReadSubtreeClose ()
		{
			// bug #334752
			string xml = @"<root><item-list><item id='a'/><item id='b'/></item-list></root>";
			RunTest (xml, new TestMethod (ReadSubtreeClose));
		}

		void ReadSubtreeClose (XmlReader reader)
		{
			reader.ReadToFollowing ("item-list");
			XmlReader sub = reader.ReadSubtree ();
			sub.ReadToDescendant ("item");
			sub.Close ();
			AssertEquals ("#1", XmlNodeType.EndElement, reader.NodeType);
			AssertEquals ("#2", "item-list", reader.Name);
		}
#endif
    }
}
