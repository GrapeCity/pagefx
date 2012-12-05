//CHANGED
//
// XmlTextReaderTests.cs
//
// Authors:
//   Jason Diamond (jason@injektilo.org)
//   Martin Willemoes Hansen (mwh@sysrq.dk)
//
// (C) 2001, 2002 Jason Diamond  http://injektilo.org/
//

using System;
using System.IO;
using System.Xml;
using System.Text;

using NUnit.Framework;

namespace MonoTests.System.Xml
{
    [TestFixture]
    public class XmlTextReaderTests : Assertion
    {
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
            Assert("#Read", xmlReader.Read());
            Assert("#ReadState", xmlReader.ReadState == ReadState.Interactive);
            Assert(!xmlReader.EOF);
            AssertNodeValues(xmlReader, nodeType, depth, isEmptyElement, name, prefix, localName, namespaceURI, value, attributeCount);
        }

        private void AssertNodeValues(
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
            AssertEquals("NodeType", nodeType, xmlReader.NodeType);
            AssertEquals("Depth", depth, xmlReader.Depth);
            AssertEquals("IsEmptyElement", isEmptyElement, xmlReader.IsEmptyElement);

            AssertEquals("name", name, xmlReader.Name);

            AssertEquals("prefix", prefix, xmlReader.Prefix);

            AssertEquals("localName", localName, xmlReader.LocalName);

            AssertEquals("namespaceURI", namespaceURI, xmlReader.NamespaceURI);

            AssertEquals("hasValue", (value != String.Empty), xmlReader.HasValue);

            AssertEquals("Value", value, xmlReader.Value);

            AssertEquals("hasAttributes", attributeCount > 0, xmlReader.HasAttributes);

            AssertEquals("attributeCount", attributeCount, xmlReader.AttributeCount);
        }

        private void AssertAttribute(
            XmlReader xmlReader,
            string name,
            string prefix,
            string localName,
            string namespaceURI,
            string value)
        {
            AssertEquals("value.Indexer", value, xmlReader[name]);

            AssertEquals("value.GetAttribute", value, xmlReader.GetAttribute(name));

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

        [Test]
        public void StartAndEndTagWithAttribute()
        {
            string xml = @"<foo bar='baz'></foo>";
            XmlReader xmlReader =
                new XmlTextReader(new StringReader(xml));

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
                1 // attributeCount
            );

            AssertAttribute(
                xmlReader, // xmlReader
                "bar", // name
                String.Empty, // prefix
                "bar", // localName
                String.Empty, // namespaceURI
                "baz" // value
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

        // expecting parser error
        [Test]
        public void EmptyElementWithBadName()
        {
            string xml = "<1foo/>";
            XmlReader xmlReader =
                new XmlTextReader(new StringReader(xml));

            bool caughtXmlException = false;

            try
            {
                xmlReader.Read();
            }
            catch (XmlException)
            {
                caughtXmlException = true;
            }

            Assert(caughtXmlException);
        }

        [Test]
        public void EmptyElementWithStartAndEndTag()
        {
            string xml = "<foo></foo>";
            XmlReader xmlReader =
                new XmlTextReader(new StringReader(xml));

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

        // checking parser
        [Test]
        public void EmptyElementWithStartAndEndTagWithWhitespace()
        {
            string xml = "<foo ></foo >";
            XmlReader xmlReader =
                new XmlTextReader(new StringReader(xml));

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
        public void EmptyElementWithAttribute()
        {
            string xml = @"<foo bar=""baz""/>";
            XmlReader xmlReader =
                new XmlTextReader(new StringReader(xml));

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
                "baz" // value
            );

            AssertEndDocument(xmlReader);
        }

        [Test]
        public void EmptyElementInNamespace()
        {
            string xml = @"<foo:bar xmlns:foo='http://foo/' />";
            XmlReader xmlReader =
                new XmlTextReader(new StringReader(xml));

            AssertStartDocument(xmlReader);

            AssertNode(
                xmlReader, // xmlReader
                XmlNodeType.Element, // nodeType
                0, // depth
                true, // isEmptyElement
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

            AssertEndDocument(xmlReader);
        }

        [Test]
        public void EntityReferenceInAttribute()
        {
            string xml = "<foo bar='&baz;'/>";
            XmlReader xmlReader =
                new XmlTextReader(new StringReader(xml));

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
                "&baz;" // value
            );

            AssertEndDocument(xmlReader);
        }

        [Test]
        public void IsName()
        {
            Assert(XmlReader.IsName("foo"));
            Assert(!XmlReader.IsName("1foo"));
            Assert(!XmlReader.IsName(" foo"));
        }

        [Test]
        public void IsNameToken()
        {
            Assert(XmlReader.IsNameToken("foo"));
            Assert(XmlReader.IsNameToken("1foo"));
            Assert(!XmlReader.IsNameToken(" foo"));
        }

#if NOT_PFX
		[Test]
		public void FragmentConstructor()
		{
			XmlDocument doc = new XmlDocument();
//			doc.LoadXml("<root/>");

			string xml = @"<foo><bar xmlns=""NSURI"">TEXT NODE</bar></foo>";
			MemoryStream ms = new MemoryStream(Encoding.Default.GetBytes(xml));

			XmlParserContext ctx = new XmlParserContext(doc.NameTable, new XmlNamespaceManager(doc.NameTable), "", "", "", "",
				doc.BaseURI, "", XmlSpace.Default, Encoding.Default);

			XmlTextReader xmlReader = new XmlTextReader(ms, XmlNodeType.Element, ctx);
			AssertNode(xmlReader, XmlNodeType.Element, 0, false, "foo", "", "foo", "", "", 0);

			AssertNode(xmlReader, XmlNodeType.Element, 1, false, "bar", "", "bar", "NSURI", "", 1);

			AssertNode(xmlReader, XmlNodeType.Text, 2, false, "", "", "", "", "TEXT NODE", 0);

			AssertNode(xmlReader, XmlNodeType.EndElement, 1, false, "bar", "", "bar", "NSURI", "", 0);

			AssertNode(xmlReader, XmlNodeType.EndElement, 0, false, "foo", "", "foo", "", "", 0);

			AssertEndDocument (xmlReader);
		}
#endif

        [Test]
        public void AttributeWithCharacterReference()
        {
            string xml = @"<a value='hello &amp; world' />";
            XmlReader xmlReader =
                new XmlTextReader(new StringReader(xml));
            xmlReader.Read();
            AssertEquals("hello & world", xmlReader["value"]);
        }

        [Test]
        public void AttributeWithEntityReference()
        {
            string xml = @"<a value='hello &ent; world' />";
            XmlReader xmlReader =
                new XmlTextReader(new StringReader(xml));
            xmlReader.Read();
            xmlReader.MoveToFirstAttribute();
            xmlReader.ReadAttributeValue();
            AssertEquals("hello ", xmlReader.Value);
            Assert(xmlReader.ReadAttributeValue());
            AssertEquals(XmlNodeType.EntityReference, xmlReader.NodeType);
            AssertEquals("ent", xmlReader.Name);
            AssertEquals(XmlNodeType.EntityReference, xmlReader.NodeType);
            Assert(xmlReader.ReadAttributeValue());
            AssertEquals(" world", xmlReader.Value);
            AssertEquals(XmlNodeType.Text, xmlReader.NodeType);
            Assert(!xmlReader.ReadAttributeValue());
            AssertEquals(" world", xmlReader.Value); // remains
            AssertEquals(XmlNodeType.Text, xmlReader.NodeType);
            xmlReader.ReadAttributeValue();
            AssertEquals(XmlNodeType.Text, xmlReader.NodeType);
        }

        [Test]
        public void QuoteChar()
        {
            string xml = @"<a value='hello &amp; world' value2="""" />";
            XmlReader xmlReader =
                new XmlTextReader(new StringReader(xml));
            xmlReader.Read();
            xmlReader.MoveToFirstAttribute();
            AssertEquals("First", '\'', xmlReader.QuoteChar);
            xmlReader.MoveToNextAttribute();
            AssertEquals("Next", '"', xmlReader.QuoteChar);
            xmlReader.MoveToFirstAttribute();
            AssertEquals("First.Again", '\'', xmlReader.QuoteChar);
        }

        [Test]
        public void ReadInnerXmlWrongInit()
        {
            // This behavior is different from XmlNodeReader.
            XmlReader reader = new XmlTextReader(new StringReader("<root>test of <b>mixed</b> string.</root>"));
            reader.ReadInnerXml();
            AssertEquals("initial.ReadState", ReadState.Initial, reader.ReadState);
            AssertEquals("initial.EOF", false, reader.EOF);
            AssertEquals("initial.NodeType", XmlNodeType.None, reader.NodeType);
        }

        [Test]
        public void EntityReference()
        {
            string xml = "<foo>&bar;</foo>";
            XmlReader xmlReader = new XmlTextReader(new StringReader(xml));
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
                XmlNodeType.EntityReference, // nodeType
                1, //depth
                false, // isEmptyElement
                "bar", // name
                String.Empty, // prefix
                "bar", // localName
                String.Empty, // namespaceURI
                String.Empty, // value
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
        public void EntityReferenceInsideText()
        {
            string xml = "<foo>bar&baz;quux</foo>";
            XmlReader xmlReader = new XmlTextReader(new StringReader(xml));
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
                XmlNodeType.EntityReference, // nodeType
                1, //depth
                false, // isEmptyElement
                "baz", // name
                String.Empty, // prefix
                "baz", // localName
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
                "quux", // value
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
        [ExpectedException(typeof(XmlException))]
        public void XmlDeclAfterWhitespace()
        {
            XmlTextReader xtr = new XmlTextReader(
                " <?xml version='1.0' ?><root />",
                XmlNodeType.Document,
                null);
            xtr.Read();	// ws
            xtr.Read();	// not-wf xmldecl
            xtr.Close();
        }

        [Test]
        [ExpectedException(typeof(XmlException))]
        public void XmlDeclAfterComment()
        {
            XmlTextReader xtr = new XmlTextReader(
                "<!-- comment --><?xml version='1.0' ?><root />",
                XmlNodeType.Document,
                null);
            xtr.Read();	// comment
            xtr.Read();	// not-wf xmldecl
            xtr.Close();
        }

        [Test]
        [ExpectedException(typeof(XmlException))]
        public void XmlDeclAfterProcessingInstruction()
        {
            XmlTextReader xtr = new XmlTextReader(
                "<?myPI let it go ?><?xml version='1.0' ?><root />",
                XmlNodeType.Document,
                null);
            xtr.Read();	// PI
            xtr.Read();	// not-wf xmldecl
            xtr.Close();
        }

        [Test]
        [ExpectedException(typeof(XmlException))]
        public void StartsFromEndElement()
        {
            XmlTextReader xtr = new XmlTextReader(
                "</root>",
                XmlNodeType.Document,
                null);
            xtr.Read();
            xtr.Close();
        }

        [Test]
        public void ReadAsElementContent()
        {
            XmlTextReader xtr = new XmlTextReader(
                "<foo /><bar />", XmlNodeType.Element, null);
            xtr.Read();
            xtr.Close();
        }

        [Test]
        public void ReadAsAttributeContent()
        {
            XmlTextReader xtr = new XmlTextReader(
                "test", XmlNodeType.Attribute, null);
            xtr.Read();
            xtr.Close();
        }

        //NOTE: Requires file IO support.
#if NOT_PFX
		[Test] 
		public void ExternalDocument ()
		{
			XmlDocument doc = new XmlDocument ();
			doc.Load ("Test/XmlFiles/nested-dtd-test.xml");
		}
#endif

        [Test]
        [ExpectedException(typeof(XmlException))]
        public void NotAllowedCharRef()
        {
            string xml = "<root>&#0;</root>";
            XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
            xtr.Normalization = true;
            xtr.Read();
            xtr.Read();
            xtr.Close();
        }

        [Test]
        public void NotAllowedCharRefButPassNormalizationFalse()
        {
            string xml = "<root>&#0;</root>";
            XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
            xtr.Read();
            xtr.Read();
            xtr.Close();
        }

        [Test]
        [ExpectedException(typeof(XmlException))]
        [Ignore("MS.NET 1.0 does not pass this test. The related spec is XML rec. 4.1")]
        public void UndeclaredEntityInIntSubsetOnlyXml()
        {
            string ent2 = "<!ENTITY ent2 '<foo/><foo/>'>]>";
            string dtd = "<!DOCTYPE root[<!ELEMENT root (#PCDATA|foo)*>" + ent2;
            string xml = dtd + "<root>&ent;&ent2;</root>";
            XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
            while (!xtr.EOF)
                xtr.Read();
            xtr.Close();
        }

        [Test]
        [ExpectedException(typeof(XmlException))]
        [Ignore("MS.NET 1.0 does not pass this test. The related spec is XML rec. 4.1")]
        public void UndeclaredEntityInStandaloneXml()
        {
            string ent2 = "<!ENTITY ent2 '<foo/><foo/>'>]>";
            string dtd = "<!DOCTYPE root[<!ELEMENT root (#PCDATA|foo)*>" + ent2;
            string xml = "<?xml version='1.0' standalone='yes' ?>"
                + dtd + "<root>&ent;</root>";
            XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
            while (!xtr.EOF)
                xtr.Read();
            xtr.Close();
        }

#if NOT_PFX
		[Test]
		public void ExpandParameterEntity ()
		{
			string ent = "<!ENTITY foo \"foo-def\">";
			string pe = "<!ENTITY % pe '" + ent + "'>";
			string eldecl = "<!ELEMENT root ANY>";
			string dtd = "<!DOCTYPE root[" + eldecl + pe + "%pe;]>";
			string xml = dtd + "<root/>";
			XmlDocument doc = new XmlDocument ();
			doc.LoadXml (xml);
			XmlEntity foo = doc.DocumentType.Entities.GetNamedItem ("foo") as XmlEntity;
			AssertNotNull (foo);
			AssertEquals ("foo-def", foo.InnerText);
		}
#endif

#if NOT_PFX
		[Test]
		public void IfNamespacesThenProhibitedAttributes ()
		{
			string xml = @"<foo _1='1' xmlns:x='urn:x' x:_1='1' />";
			XmlDocument doc = new XmlDocument ();
			doc.LoadXml (xml);
		}
#endif

        [Test]
        public void ReadBase64()
        {
            byte[] bytes = new byte[] { 4, 14, 54, 114, 134, 184, 254, 255 };

            string base64 = "<root><foo>BA42coa44</foo></root>";
            XmlTextReader xtr = new XmlTextReader(base64, XmlNodeType.Document, null);
            byte[] bytes2 = new byte[10];
            xtr.Read();	// root
            xtr.Read();	// foo
            this.AssertNodeValues(xtr, XmlNodeType.Element, 1, false, "foo", String.Empty,
                "foo", String.Empty, String.Empty, 0);
            AssertEquals(6, xtr.ReadBase64(bytes2, 0, 10));
            this.AssertNodeValues(xtr, XmlNodeType.EndElement, 0, false, "root", String.Empty,
                "root", String.Empty, String.Empty, 0);
            Assert(!xtr.Read());
            AssertEquals(4, bytes2[0]);
            AssertEquals(14, bytes2[1]);
            AssertEquals(54, bytes2[2]);
            AssertEquals(114, bytes2[3]);
            AssertEquals(134, bytes2[4]);
            AssertEquals(184, bytes2[5]);
            AssertEquals(0, bytes2[6]);
            xtr.Close();

            xtr = new XmlTextReader(base64, XmlNodeType.Document, null);
            bytes2 = new byte[10];
            xtr.Read();	// root
            xtr.Read();	// foo
            this.AssertNodeValues(xtr, XmlNodeType.Element, 1, false, "foo", String.Empty,
                "foo", String.Empty, String.Empty, 0);

            // Read less than 4 (i.e. one Base64 block)
            AssertEquals(1, xtr.ReadBase64(bytes2, 0, 1));
            this.AssertNodeValues(xtr, XmlNodeType.Element, 1, false, "foo", String.Empty,
                "foo", String.Empty, String.Empty, 0);
            AssertEquals(4, bytes2[0]);

            AssertEquals(5, xtr.ReadBase64(bytes2, 0, 10));
            this.AssertNodeValues(xtr, XmlNodeType.EndElement, 0, false, "root", String.Empty,
                "root", String.Empty, String.Empty, 0);
            Assert(!xtr.Read());
            AssertEquals(14, bytes2[0]);
            AssertEquals(54, bytes2[1]);
            AssertEquals(114, bytes2[2]);
            AssertEquals(134, bytes2[3]);
            AssertEquals(184, bytes2[4]);
            AssertEquals(0, bytes2[5]);
            while (!xtr.EOF)
                xtr.Read();
            xtr.Close();
        }

        [Test]
        public void ReadBase64Test2()
        {
            string xml = "<root/>";
            XmlTextReader xtr = new XmlTextReader(new StringReader(xml));
            xtr.Read();
            byte[] data = new byte[1];
            xtr.ReadBase64(data, 0, 1);
            while (!xtr.EOF)
                xtr.Read();

            xml = "<root></root>";
            xtr = new XmlTextReader(new StringReader(xml));
            xtr.Read();
            xtr.ReadBase64(data, 0, 1);
            while (!xtr.EOF)
                xtr.Read();
        }

        [Test]
        [ExpectedException(typeof(XmlException))]
        public void CheckNamespaceValidity1()
        {
            string xml = "<x:root />";
            XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
            xtr.Read();
        }

        [Test]
        [ExpectedException(typeof(XmlException))]
        public void CheckNamespaceValidity2()
        {
            string xml = "<root x:attr='val' />";
            XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
            xtr.Read();
        }

        [Test]
        public void NamespaceFalse()
        {
            string xml = "<x:root />";
            XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
            xtr.Namespaces = false;
            xtr.Read();
        }

        [Test]
        public void NormalizationLineEnd()
        {
            string s = "One\rtwo\nthree\r\nfour";
            string t = "<hi><![CDATA[" + s + "]]></hi>";

            XmlTextReader r = new XmlTextReader(new StringReader(t));
            r.WhitespaceHandling = WhitespaceHandling.Significant;

            r.Normalization = true;

            s = r.ReadElementString("hi");
            AssertEquals("One\ntwo\nthree\nfour", s);
        }

        [Test]
        public void NormalizationAttributes()
        {
            // does not normalize attribute values.
            StringReader sr = new StringReader("<!DOCTYPE root [<!ELEMENT root EMPTY><!ATTLIST root attr ID #IMPLIED>]><root attr='   value   '/>");
            XmlTextReader xtr = new XmlTextReader(sr);
            xtr.Normalization = true;
            xtr.Read();
            xtr.Read();
            xtr.MoveToFirstAttribute();
            AssertEquals("   value   ", xtr.Value);
        }

        [Test]
        public void CloseIsNotAlwaysEOF()
        {
            // See bug #63505
            XmlTextReader xtr = new XmlTextReader(
                new StringReader("<a></a><b></b>"));
            xtr.Close();
            Assert(!xtr.EOF); // Close() != EOF
        }

        [Test]
        public void CloseIsNotAlwaysEOF2()
        {
            //XmlTextReader xtr = new XmlTextReader ("Test/XmlFiles/simple.xml");
            XmlTextReader xtr = new XmlTextReader(new StringReader("<root/>"));
            xtr.Close();
            Assert(!xtr.EOF); // Close() != EOF
        }

        [Test]
        public void IXmlLineInfo()
        {
            // See bug #63507
            XmlTextReader aux = new XmlTextReader(
                new StringReader("<all><hello></hello><bug></bug></all>"));
            AssertEquals(0, aux.LineNumber);
            AssertEquals(0, aux.LinePosition);
            aux.MoveToContent();
            AssertEquals(1, aux.LineNumber);
            AssertEquals(2, aux.LinePosition);
            aux.Read();
            AssertEquals(1, aux.LineNumber);
            AssertEquals(7, aux.LinePosition);
            aux.ReadOuterXml();
            AssertEquals(1, aux.LineNumber);
            AssertEquals(22, aux.LinePosition);
            aux.ReadInnerXml();
            AssertEquals(1, aux.LineNumber);
            AssertEquals(34, aux.LinePosition);
            aux.Read();
            AssertEquals(1, aux.LineNumber);
            AssertEquals(38, aux.LinePosition);
            aux.Close();
            AssertEquals(0, aux.LineNumber);
            AssertEquals(0, aux.LinePosition);
        }

#if NOT_PFX
		[Test]
		public void AttributeNormalizationWrapped ()
		{
			// When XmlValidatingReader there used to be a problem.
			string xml = "<root attr=' value\nstring' />";
			XmlTextReader xtr = new XmlTextReader (xml,
				XmlNodeType.Document, null);
			xtr.Normalization = true;
			XmlValidatingReader xvr = new XmlValidatingReader (xtr);
			xvr.Read ();
			xvr.MoveToFirstAttribute ();
			AssertEquals (" value string", xvr.Value);
		}
#endif

        [Test]
        [ExpectedException(typeof(XmlException))]
        public void ProhibitDtd()
        {
            XmlTextReader xtr = new XmlTextReader("<!DOCTYPE root []><root/>", XmlNodeType.Document, null);
            xtr.ProhibitDtd = true;
            while (!xtr.EOF)
                xtr.Read();
        }

#if NET_2_0
		[Test]
		public void Settings ()
		{
			XmlTextReader xtr = new XmlTextReader ("<root/>", XmlNodeType.Document, null);
			AssertNull (xtr.Settings);
		}

#if NOT_PFX
		// Copied from XmlValidatingReaderTests.cs
		[Test]
		public void ExpandEntity ()
		{
			string intSubset = "<!ELEMENT root (#PCDATA)><!ATTLIST root foo CDATA 'foo-def' bar CDATA 'bar-def'><!ENTITY ent 'entity string'>";
			string dtd = "<!DOCTYPE root [" + intSubset + "]>";
			string xml = dtd + "<root foo='&ent;' bar='internal &ent; value'>&ent;</root>";
			XmlTextReader dvr = new XmlTextReader (xml, XmlNodeType.Document, null);
            dvr.EntityHandling = EntityHandling.ExpandEntities;
			dvr.Read ();	// DTD
			dvr.Read ();
			AssertEquals (XmlNodeType.Element, dvr.NodeType);
			AssertEquals ("root", dvr.Name);
			Assert (dvr.MoveToFirstAttribute ());
			AssertEquals ("foo", dvr.Name);
			AssertEquals ("entity string", dvr.Value);
			Assert (dvr.MoveToNextAttribute ());
			AssertEquals ("bar", dvr.Name);
			AssertEquals ("internal entity string value", dvr.Value);
			AssertEquals ("entity string", dvr.ReadString ());
		}
#endif

		[Test]
		public void PreserveEntity ()
		{
			string intSubset = "<!ELEMENT root EMPTY><!ATTLIST root foo CDATA 'foo-def' bar CDATA 'bar-def'><!ENTITY ent 'entity string'>";
			string dtd = "<!DOCTYPE root [" + intSubset + "]>";
			string xml = dtd + "<root foo='&ent;' bar='internal &ent; value' />";
			XmlTextReader dvr = new XmlTextReader (xml, XmlNodeType.Document, null);
#if NOT_PFX
dvr.EntityHandling = EntityHandling.ExpandCharEntities;
#endif
			dvr.Read ();	// DTD
			dvr.Read ();
			AssertEquals (XmlNodeType.Element, dvr.NodeType);
			AssertEquals ("root", dvr.Name);
			Assert (dvr.MoveToFirstAttribute ());
			AssertEquals ("foo", dvr.Name);
			// MS BUG: it returns "entity string", however, entity should not be exanded.
			AssertEquals ("&ent;", dvr.Value);
			//  ReadAttributeValue()
			Assert (dvr.ReadAttributeValue ());
			AssertEquals (XmlNodeType.EntityReference, dvr.NodeType);
			AssertEquals ("ent", dvr.Name);
			AssertEquals ("", dvr.Value);
			Assert (!dvr.ReadAttributeValue ());

			// bar
			Assert (dvr.MoveToNextAttribute ());
			AssertEquals ("bar", dvr.Name);
			AssertEquals ("internal &ent; value", dvr.Value);
			//  ReadAttributeValue()
			Assert (dvr.ReadAttributeValue ());
			AssertEquals (XmlNodeType.Text, dvr.NodeType);
			AssertEquals ("", dvr.Name);
			AssertEquals ("internal ", dvr.Value);
			Assert (dvr.ReadAttributeValue ());
			AssertEquals (XmlNodeType.EntityReference, dvr.NodeType);
			AssertEquals ("ent", dvr.Name);
			AssertEquals ("", dvr.Value);
			Assert (dvr.ReadAttributeValue ());
			AssertEquals (XmlNodeType.Text, dvr.NodeType);
			AssertEquals ("", dvr.Name);
			AssertEquals (" value", dvr.Value);

		}

#if NOT_PFX
		[Test]
		[ExpectedException (typeof (XmlException))]
		public void ExpandEntityRejectsUndeclaredEntityAttr ()
		{
			XmlTextReader xtr = new XmlTextReader ("<!DOCTYPE root SYSTEM 'foo.dtd'><root attr='&rnt;'>&rnt;</root>", XmlNodeType.Document, null);
            xtr.EntityHandling = EntityHandling.ExpandEntities;
			xtr.XmlResolver = null;
			xtr.Read ();
			xtr.Read (); // attribute entity 'rnt' is undeclared
		}

		[Test]
		[ExpectedException (typeof (XmlException))]
		public void ExpandEntityRejectsUndeclaredEntityContent ()
		{
			XmlTextReader xtr = new XmlTextReader ("<!DOCTYPE root SYSTEM 'foo.dtd'><root>&rnt;</root>", XmlNodeType.Document, null);
            xtr.EntityHandling = EntityHandling.ExpandEntities;
			xtr.XmlResolver = null;
			xtr.Read ();
			xtr.Read ();
			xtr.Read (); // content entity 'rnt' is undeclared
		}
#endif


#if NOT_PFX
		// mostly copied from XmlValidatingReaderTests.
		[Test]
		public void ResolveEntity ()
		{
			string ent1 = "<!ENTITY ent 'entity string'>";
			string ent2 = "<!ENTITY ent2 '<foo/><foo/>'>]>";
			string dtd = "<!DOCTYPE root[<!ELEMENT root (#PCDATA|foo)*>" + ent1 + ent2;
			string xml = dtd + "<root>&ent;&ent2;</root>";
			XmlTextReader dvr = new XmlTextReader (xml, XmlNodeType.Document, null);
            dvr.EntityHandling = EntityHandling.ExpandCharEntities;
			dvr.Read ();	// DTD
			dvr.Read ();	// root
			dvr.Read ();	// &ent;
			AssertEquals (XmlNodeType.EntityReference, dvr.NodeType);
			AssertEquals (1, dvr.Depth);
			dvr.ResolveEntity ();
			// It is still entity reference.
			AssertEquals (XmlNodeType.EntityReference, dvr.NodeType);
			dvr.Read ();
			AssertEquals (XmlNodeType.Text, dvr.NodeType);
			AssertEquals (2, dvr.Depth);
			AssertEquals ("entity string", dvr.Value);
			dvr.Read ();
			AssertEquals (XmlNodeType.EndEntity, dvr.NodeType);
			AssertEquals (1, dvr.Depth);
			AssertEquals ("", dvr.Value);

			dvr.Read ();	// &ent2;
			AssertEquals (XmlNodeType.EntityReference, dvr.NodeType);
			AssertEquals (1, dvr.Depth);
			dvr.ResolveEntity ();
			// It is still entity reference.
			AssertEquals (XmlNodeType.EntityReference, dvr.NodeType);
			// It now became element node.
			dvr.Read ();
			AssertEquals (XmlNodeType.Element, dvr.NodeType);
			AssertEquals (2, dvr.Depth);
		}
#endif

#if NOT_PFX
        // mostly copied from XmlValidatingReaderTests.
		[Test]
		public void ResolveEntity2 ()
		{
			string ent1 = "<!ENTITY ent 'entity string'>";
			string ent2 = "<!ENTITY ent2 '<foo/><foo/>'>]>";
			string dtd = "<!DOCTYPE root[<!ELEMENT root (#PCDATA|foo)*>" + ent1 + ent2;
			string xml = dtd + "<root>&ent3;&ent2;</root>";
			XmlTextReader dvr = new XmlTextReader (xml, XmlNodeType.Document, null);
            dvr.EntityHandling = EntityHandling.ExpandCharEntities;
			dvr.Read ();	// DTD
			dvr.Read ();	// root
			dvr.Read ();	// &ent3;
			AssertEquals (XmlNodeType.EntityReference, dvr.NodeType);
			// ent3 does not exists in this dtd.
			AssertEquals (XmlNodeType.EntityReference, dvr.NodeType);
			try {
				dvr.ResolveEntity ();
				Fail ("Attempt to resolve undeclared entity should fail.");
			} catch (XmlException) {
			}
		}
#endif
#endif

#if NOT_PFX
		[Test]
		public void SurrogatePair ()
		{
			string xml = @"<!DOCTYPE test [<!ELEMENT test ANY>
		<!ENTITY % a '<!ENTITY ref " + "\"\uF090\u8080\"" + @">'>
		%a;
	]>
	<test>&ref;</test>";
			XmlValidatingReader r = new XmlValidatingReader (xml, XmlNodeType.Document, null);
			r.Read ();
			r.Read ();
			r.Read ();
			r.Read ();
			AssertEquals ("#1", 0xf090, (int) r.Value [0]);
			AssertEquals ("#1", 0x8080, (int) r.Value [1]);
		}
#endif

        //NOTE: Need to fix
#if NOT_PFX
        [Test]
        [ExpectedException(typeof(XmlException))]
        public void EntityDeclarationNotWF()
        {
            string xml = @"<!DOCTYPE doc [
				<!ELEMENT doc (#PCDATA)>
				<!ENTITY e ''>
				<!ENTITY e '<foo&>'>
				]>
				<doc>&e;</doc> ";
            XmlTextReader xtr = new XmlTextReader(xml,
                XmlNodeType.Document, null);
            xtr.Read();
        }
#endif

        //NOTE: Requires file IO support.
#if NOT_PFX
		[Test] // bug #76102
		public void SurrogateAtReaderByteCache ()
		{
			XmlTextReader xtr = null;
			try {
				xtr = new XmlTextReader (File.OpenText ("Test/XmlFiles/76102.xml"));
				while (!xtr.EOF)
					xtr.Read ();
			} finally {
				if (xtr != null)
					xtr.Close ();
			}
		}
#endif

        [Test] // bug #76247
        public void SurrogateRoundtrip()
        {
            byte[] data = new byte[] {0x3c, 0x61, 0x3e, 0xf0,
				0xa8, 0xa7, 0x80, 0x3c, 0x2f, 0x61, 0x3e};
            XmlTextReader xtr = new XmlTextReader(
                new MemoryStream(data));
            xtr.Read();
            string line = xtr.ReadString();
            int[] arr = new int[line.Length];
            for (int i = 0; i < line.Length; i++)
                arr[i] = (int)line[i];
            AssertEquals(new int[] { 0xd862, 0xddc0 }, arr);
        }

        [Test]
        [ExpectedException(typeof(XmlException))]
        public void RejectEmptyNamespaceWithNonEmptyPrefix()
        {
            XmlTextReader xtr = new XmlTextReader("<root xmlns:my='' />",
                XmlNodeType.Document, null);
            xtr.Read();
        }

        [Test]
        public void EncodingProperty()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"iso-8859-1\"?>\n<root>\n<node>\nvalue\n</node>\n</root>";
            XmlTextReader xr = new XmlTextReader(xml, XmlNodeType.Document, null);
            AssertNull("#1", xr.Encoding);
            xr.Read();
            AssertEquals("#2", Encoding.Unicode, xr.Encoding);
        }

        [Test]
        public void WhitespaceHandlingSignificant()
        {
            XmlTextReader xtr = new XmlTextReader("<root>  <child xml:space='preserve'>    <descendant xml:space='default'>    </descendant>   </child><child xml:space='default'>   </child>  </root>",
                XmlNodeType.Document, null);
            xtr.WhitespaceHandling = WhitespaceHandling.Significant;

            xtr.Read(); // root
            xtr.Read(); // child. skip whitespaces
            AssertEquals("#1", XmlNodeType.Element, xtr.NodeType);
            xtr.Read(); // significant whitespaces
            AssertEquals("#2", XmlNodeType.SignificantWhitespace, xtr.NodeType);
            xtr.Read();
            AssertEquals("#3", "descendant", xtr.LocalName);
            xtr.Read(); // end of descendant. skip whitespaces
            AssertEquals("#4", XmlNodeType.EndElement, xtr.NodeType);
            xtr.Read(); // significant whitespaces
            AssertEquals("#5", XmlNodeType.SignificantWhitespace, xtr.NodeType);
            xtr.Read(); // end of child
            xtr.Read(); // child
            xtr.Read(); // end of child. skip whitespaces
            AssertEquals("#6", XmlNodeType.EndElement, xtr.NodeType);
            xtr.Read(); // end of root. skip whitespaces
            AssertEquals("#7", XmlNodeType.EndElement, xtr.NodeType);
        }

        [Test]
        public void WhitespaceHandlingNone()
        {
            XmlTextReader xtr = new XmlTextReader("<root>  <child xml:space='preserve'>    <descendant xml:space='default'>    </descendant>   </child><child xml:space='default'>   </child>  </root>",
                XmlNodeType.Document, null);
            xtr.WhitespaceHandling = WhitespaceHandling.None;

            xtr.Read(); // root
            xtr.Read(); // child. skip whitespaces
            AssertEquals("#1", XmlNodeType.Element, xtr.NodeType);
            xtr.Read(); // descendant. skip significant whitespaces
            AssertEquals("#2", "descendant", xtr.LocalName);
            xtr.Read(); // end of descendant. skip whitespaces
            AssertEquals("#3", XmlNodeType.EndElement, xtr.NodeType);
            xtr.Read(); // end of child. skip significant whitespaces
            xtr.Read(); // child
            xtr.Read(); // end of child. skip whitespaces
            AssertEquals("#6", XmlNodeType.EndElement, xtr.NodeType);
            xtr.Read(); // end of root. skip whitespaces
            AssertEquals("#7", XmlNodeType.EndElement, xtr.NodeType);
        }

        //NOTE: version problem
#if NOT_PFX
		[Test]
		public void WhitespacesAfterTextDeclaration ()
		{
			XmlTextReader xtr = new XmlTextReader (
				"<?xml version='1.0' encoding='utf-8' ?> <x/>",
				XmlNodeType.Element,
				null);
			xtr.Read ();
			AssertEquals ("#1", XmlNodeType.Whitespace, xtr.NodeType);
			AssertEquals ("#2", " ", xtr.Value);
		}
#endif

        //NOTE: Requires file IO support.
#if NOT_PFX
		// bug #79683
		[Test]
		public void NotationPERef ()
		{
			string xml = "<!DOCTYPE root SYSTEM 'Test/XmlFiles/79683.dtd'><root/>";
			XmlTextReader xtr = new XmlTextReader (xml, XmlNodeType.Document, null);
			while (!xtr.EOF)
				xtr.Read ();
		}
#endif

        [Test] // bug #80308
        public void ReadCharsNested()
        {
            char[] buf = new char[4];

            string xml = "<root><text>AAAA</text></root>";
            string[] strings = new string[] {
				"<tex", "t>AA", "AA</", "text", ">"};
            XmlTextReader r = new XmlTextReader(
                xml, XmlNodeType.Document, null);
            int c, n = 0;
            while (r.Read())
                if (r.NodeType == XmlNodeType.Element)
                    while ((c = r.ReadChars(buf, 0, buf.Length)) > 0)
                        AssertEquals("at " + n,
                            strings[n++],
                            new string(buf, 0, c));
            AssertEquals("total lines", 5, n);
        }

        [Test] // bug #81294
        public void DtdCommentContainsCloseBracket()
        {
            string xml = @"<!DOCTYPE kanjidic2 [<!ELEMENT kanjidic2 EMPTY> <!-- ] --> ]><kanjidic2 />";
            XmlTextReader xtr = new XmlTextReader(xml, XmlNodeType.Document, null);
            while (!xtr.EOF)
                xtr.Read();
        }
    }
}
