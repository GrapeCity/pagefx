//
// System.Xml.XmlWriterSettingsTests.cs
//
// Authors:
//	Atsushi Enomoto <atsushi@ximian.com>
//
// (C)2004 Novell Inc.
//

#if NET_2_0
using System;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Schema;
using NUnit.Framework;

namespace MonoTests.System.Xml
{
	[TestFixture]
	public class XmlWriterSettingsTests : Assertion
	{
		[Test]
		public void DefaultValue ()
		{
			XmlWriterSettings s = new XmlWriterSettings ();
			DefaultValue (s);
			s.Reset ();
			DefaultValue (s);
		}

		private void DefaultValue (XmlWriterSettings s)
		{
			AssertEquals (true, s.CheckCharacters);
			AssertEquals (false, s.CloseOutput);
			AssertEquals (ConformanceLevel.Document, s.ConformanceLevel);
			AssertEquals (Encoding.UTF8, s.Encoding);
			AssertEquals (false, s.Indent);
			AssertEquals ("  ", s.IndentChars);
			AssertEquals (Environment.NewLine, s.NewLineChars);
			AssertEquals (false, s.NewLineOnAttributes);
			AssertEquals (false, s.OmitXmlDeclaration);
		}

#if NOT_PFX
		[Test]
		public void EncodingTest ()
		{
			// For Stream it makes sense
			XmlWriterSettings s = new XmlWriterSettings ();
			s.Encoding = Encoding.GetEncoding ("shift_jis");
			MemoryStream ms = new MemoryStream ();
			XmlWriter w = XmlWriter.Create (ms, s);
			w.WriteStartElement ("root");
			w.WriteEndElement ();
			w.Close ();
			byte [] data = ms.ToArray ();
			Assert (data.Length != 0);
			string str = s.Encoding.GetString (data);
			AssertEquals ("<?xml version=\"1.0\" encoding=\"shift_jis\"?><root />", str);

			// For TextWriter it does not make sense
			StringWriter sw = new StringWriter ();
			w = XmlWriter.Create (sw, s);
			w.WriteStartElement ("root");
			w.WriteEndElement ();
			w.Close ();
			AssertEquals ("<?xml version=\"1.0\" encoding=\"utf-16\"?><root />", sw.ToString ());
		}
#endif

		[Test]
		[ExpectedException (typeof (ArgumentException))]
		public void CheckCharactersTest ()
		{
			XmlWriterSettings s = new XmlWriterSettings ();
			StringWriter sw = new StringWriter ();
			XmlWriter w = XmlWriter.Create (sw, s);
			w.WriteStartElement ("root");
			w.WriteString ("\0"); // invalid
			w.WriteEndElement ();
			w.Close ();
		}

		[Test]
		public void CheckCharactersFalseTest ()
		{
			XmlWriterSettings s = new XmlWriterSettings ();
			s.CheckCharacters = false;
			StringWriter sw = new StringWriter ();
			XmlWriter w = XmlWriter.Create (sw, s);
			w.WriteStartElement ("root");
			w.WriteString ("\0"); // invalid
			w.WriteEndElement ();
		}

		[Test]
		[ExpectedException (typeof (ObjectDisposedException))]
		public void CloseOutputTest ()
		{
			XmlWriterSettings s = new XmlWriterSettings ();
			s.CloseOutput = true;
			StringWriter sw = new StringWriter ();
			XmlWriter w = XmlWriter.Create (sw, s);
			w.WriteStartElement ("root");
			w.WriteEndElement ();
			w.Close ();
			sw.Write ("more"); // not allowed
		}

		[Test]
		[ExpectedException (typeof (InvalidOperationException))]
		public void ConformanceLevelFragmentAndWriteStartDocument ()
		{
			XmlWriterSettings s = new XmlWriterSettings ();
			s.ConformanceLevel = ConformanceLevel.Fragment;
			s.OmitXmlDeclaration = true;
			XmlWriter w = XmlWriter.Create (Console.Out, s);
			w.WriteStartDocument ();
		}

		[Test]
		public void ConformanceLevelAuto ()
		{
			XmlWriterSettings s = new XmlWriterSettings ();
			s.ConformanceLevel = ConformanceLevel.Auto;
			StringWriter sw = new StringWriter ();
			XmlWriter w = XmlWriter.Create (sw, s);
			w.WriteElementString ("foo", "");
			w.Close ();
			AssertEquals ("<foo />", sw.ToString ());
		}

		[Test]
		public void ConformanceLevelAuto_WriteStartDocument ()
		{
			XmlWriterSettings s = new XmlWriterSettings ();
			s.ConformanceLevel = ConformanceLevel.Auto;
			StringWriter sw = new StringWriter ();
			XmlWriter w = XmlWriter.Create (sw, s);
			w.WriteStartDocument ();
			w.WriteElementString ("foo", "");
			w.Close ();
			AssertEquals ("<?xml version=\"1.0\" encoding=\"utf-16\"?><foo />", sw.ToString ());
		}

		[Test]
		public void ConformanceLevelAuto_OmitXmlDecl_WriteStartDocument ()
		{
			XmlWriterSettings s = new XmlWriterSettings ();
			s.ConformanceLevel = ConformanceLevel.Auto;
			s.OmitXmlDeclaration = true;
			StringWriter sw = new StringWriter ();
			XmlWriter w = XmlWriter.Create (sw, s);
			w.WriteStartDocument ();
			w.WriteElementString ("foo", "");
			w.Close ();
			AssertEquals ("<foo />", sw.ToString ());
		}

		[Test]
		public void ConformanceLevelDocument_OmitXmlDeclDeclaration ()
		{
			XmlWriterSettings s = new XmlWriterSettings ();
			s.ConformanceLevel = ConformanceLevel.Document;
			// LAMESPEC:
			// On MSDN, XmlWriterSettings.OmitXmlDeclaration is documented as:
			// "The XML declaration is always written if
			//  ConformanceLevel is set to Document, even 
			//  if OmitXmlDeclaration is set to true. "
			// but it is incorrect. It does consider 
			// OmitXmlDeclaration property.
			s.OmitXmlDeclaration = true;
			StringWriter sw = new StringWriter ();
			XmlWriter w = XmlWriter.Create (sw, s);
			w.WriteElementString ("foo", "");
			w.Close ();
			AssertEquals ("<foo />", sw.ToString ());
		}

		[Test]
		public void IndentationAndFormatting ()
		{
			// Test for Indent, IndentChars, NewLineOnAttributes,
			// NewLineChars and OmitXmlDeclaration.
			string output = "<root\n    attr=\"value\"\n    attr2=\"value\">\n    <child>test</child>\n</root>";
			XmlWriterSettings s = new XmlWriterSettings ();
			s.OmitXmlDeclaration = true;
			s.Indent = true;
			s.IndentChars = "    ";
			s.NewLineChars = "\n";
			s.NewLineOnAttributes = true;
			StringWriter sw = new StringWriter ();
			XmlWriter w = XmlWriter.Create (sw, s);
			w.WriteStartElement ("root");
			w.WriteAttributeString ("attr", "value");
			w.WriteAttributeString ("attr2", "value");
			w.WriteStartElement ("child");
			w.WriteString ("test");
			w.WriteEndElement ();
			w.WriteEndElement ();
			w.Close ();
			AssertEquals (output, sw.ToString ());
		}

		[Test]
		public void SetEncodingNull ()
		{
			// null is allowed.
			new XmlWriterSettings ().Encoding = null;
		}

		[Test]
		[ExpectedException (typeof (ArgumentNullException))]
		public void NewLineCharsNull ()
		{
			new XmlWriterSettings ().NewLineChars = null;
		}

		[Test]
		public void CreateOmitXmlDeclaration ()
		{
			StringBuilder sb = new StringBuilder ();
			// Even if XmlWriter is allowed to write fragment,
			// DataContractSerializer never allows it to write
			// content in contentOnly mode.
			XmlWriterSettings settings = new XmlWriterSettings ();
			settings.OmitXmlDeclaration = true;
			//settings.ConformanceLevel = ConformanceLevel.Fragment;
			XmlWriter w = XmlWriter.Create (sb, settings);
			w.WriteStartElement ("root");
			w.WriteEndElement ();
			w.Flush ();
			AssertEquals ("<root />", sb.ToString ());
		}

		[Test]
		public void NewLineOnAttributesMixedContent ()
		{
			StringWriter sw = new StringWriter ();
			XmlWriterSettings s = new XmlWriterSettings ();
			s.NewLineOnAttributes = true;
			s.OmitXmlDeclaration = true;
			XmlWriter w = XmlWriter.Create (sw, s);
			w.WriteStartElement ("root");
			w.WriteAttributeString ("a", "v");
			w.WriteAttributeString ("b", "x");
			w.WriteString ("some string");
			w.WriteEndElement ();
			w.Close ();
			AssertEquals ("#1", "<root a=\"v\" b=\"x\">some string</root>", sw.ToString ());

			// mixed content: bug #81770
			string expected = "<root>some string<mixed\n    a=\"v\"\n    b=\"x\">some string</mixed></root>";
			s.Indent = true;
			sw = new StringWriter ();
			w = XmlWriter.Create (sw, s);
			w.WriteStartElement ("root");
			w.WriteString ("some string");
			w.WriteStartElement ("mixed");
			w.WriteAttributeString ("a", "v");
			w.WriteAttributeString ("b", "x");
			w.WriteString ("some string");
			w.WriteEndElement ();
			w.WriteEndElement ();
			w.Close ();
			AssertEquals ("#2", expected, sw.ToString ().Replace ("\r\n", "\n"));
		}
		
	}
}
#endif
