//
// System.Xml.XmlReaderSettingsTests.cs
//
// Authors:
//	Atsushi Enomoto <atsushi@ximian.com>
//
// (C)2004 Novell Inc.
//

#if NET_2_0
using System;
using System.IO;
#if NOT_PFX
using System.Net;
#endif
using System.Text;
using System.Xml;
using System.Xml.Schema;
using NUnit.Framework;

#if NOT_PFX
using ValidationFlags = System.Xml.Schema.XmlSchemaValidationFlags;
#endif
using AssertType = NUnit.Framework.Assert;

namespace MonoTests.System.Xml
{
	[TestFixture]
	public class XmlReaderSettingsTests : Assertion
	{
		public Stream CreateStream (string xml)
		{
			return new MemoryStream (Encoding.UTF8.GetBytes (xml));
		}

		[Test]
		public void DefaultValue ()
		{
			XmlReaderSettings s = new XmlReaderSettings ();
			AssertEquals ("CheckCharacters", true, s.CheckCharacters);
			AssertEquals ("ConformanceLevel", ConformanceLevel.Document,
				s.ConformanceLevel);
			AssertEquals ("ValidationType", ValidationType.None, s.ValidationType);
			AssertEquals ("IgnoreComments", false, s.IgnoreComments);

#if NOT_PFX
Assert ("ProcessInlineSchema", 0 == (s.ValidationFlags &
				ValidationFlags.ProcessInlineSchema));

#endif
			AssertEquals ("IgnorePI", false, s.IgnoreProcessingInstructions);

#if NOT_PFX
            Assert("ProcessSchemaLocation", 0 == (s.ValidationFlags &
                ValidationFlags.ProcessSchemaLocation));
            Assert("ReportValidationWarnings", 0 == (s.ValidationFlags &
                ValidationFlags.ReportValidationWarnings));
            Assert("ProcessIdentityConstraints", 0 != (s.ValidationFlags &
                ValidationFlags.ProcessIdentityConstraints));
#endif

			// No one should use this flag BTW if someone wants
			// code to be conformant to W3C XML Schema standard.
#if NOT_PFX
Assert ("AllowXmlAttributes", 0 != (s.ValidationFlags &
				ValidationFlags.AllowXmlAttributes));
#endif
			AssertEquals ("IgnoreWhitespace", false, s.IgnoreWhitespace);
			AssertEquals ("LineNumberOffset", 0, s.LineNumberOffset);
			AssertEquals ("LinePositionOffset", 0, s.LinePositionOffset);
			AssertNull ("NameTable", s.NameTable);
#if NOT_PFX
AssertEquals ("Schemas.Count", 0, s.Schemas.Count);
#endif
		}

#if NOT_PFX
		[Test]
		public void SetSchemas ()
		{
			XmlReaderSettings s = new XmlReaderSettings ();
			s.Schemas = new XmlSchemaSet ();
		}

		[Test]
		public void SetSchemasNull ()
		{
			XmlReaderSettings s = new XmlReaderSettings ();
			s.Schemas = null;
		}
#endif

		[Test]
		public void CloseInput ()
		{
			StringReader sr = new StringReader ("<root/><root/>");
			XmlReader xtr = XmlReader.Create (sr); // default false
			xtr.Read ();
			xtr.Close ();
			// It should without error, unlike usual XmlTextReader.
			sr.ReadLine ();
		}

		[Test]
		public void CreateAndNormalization ()
		{
			StringReader sr = new StringReader (
				"<root attr='   value   '>test\rstring</root>");
			XmlReaderSettings settings = new XmlReaderSettings ();
			settings.CheckCharacters = false;
			XmlReader xtr = XmlReader.Create (
				sr, settings);
			xtr.Read ();
			xtr.MoveToFirstAttribute ();
			AssertEquals ("   value   ", xtr.Value);
			xtr.Read ();
			// Text string is normalized
			AssertEquals ("test\nstring", xtr.Value);
		}

		[Test]
		public void CheckCharactersAndNormalization ()
		{
			// It should *not* raise an error (even Normalization
			// is set by default).
			StringReader sr = new StringReader (
				"<root attr='&#0;'>&#x0;</root>");
			XmlReaderSettings settings = new XmlReaderSettings ();
			settings.CheckCharacters = false;
			XmlReader xtr = XmlReader.Create (
				sr, settings);
			// After creation, changes on source XmlReaderSettings
			// does not matter.
			settings.CheckCharacters = false;
			xtr.Read ();
			xtr.MoveToFirstAttribute ();
			AssertEquals ("\0", xtr.Value);
			xtr.Read ();
			AssertEquals ("\0", xtr.Value);
		}

#if NOT_PFX
		// Hmm, does it really make sense? :-/
		[Test]
		public void CheckCharactersForNonTextReader ()
		{
			// It should *not* raise an error (even Normalization
			// is set by default).
			StringReader sr = new StringReader (
				"<root attr='&#0;'>&#x0;</root>");
			XmlReaderSettings settings = new XmlReaderSettings ();
			settings.CheckCharacters = false;
			XmlReader xr = XmlReader.Create (
				sr, settings);

			// Enable character checking for XmlNodeReader.
			settings.CheckCharacters = true;

			XmlDocument doc = new XmlDocument ();
			doc.Load (xr);
			xr = XmlReader.Create (new XmlNodeReader (doc), settings);

            xr = XmlReader.Create (xr, settings);

			// But it won't work against XmlNodeReader.
			xr.Read ();
			xr.MoveToFirstAttribute ();
			AssertEquals ("\0", xr.Value);
			xr.Read ();
			AssertEquals ("\0", xr.Value);
		}

		[Test]
		public void CreateAndSettings ()
		{
			AssertNotNull (XmlReader.Create (CreateStream ("<xml/>")).Settings);
			AssertNotNull (XmlReader.Create ("Test/XmlFiles/simple.xml").Settings);
		}
#endif

		[Test]
		public void CreateAndNameTable ()
		{
			// By default NameTable is null, but some of
			// XmlReader.Create() should not result in null
			// reference exceptions.
			XmlReaderSettings s = new XmlReaderSettings ();
			XmlReader.Create (new StringReader ("<root/>"), s, String.Empty)
				.Read ();
			XmlReader.Create (new StringReader ("<root/>"), s, (XmlParserContext) null)
				.Read ();
			XmlReader.Create (CreateStream ("<root/>"), s, String.Empty)
				.Read ();
			XmlReader.Create (CreateStream ("<root/>"), s, (XmlParserContext) null)
				.Read ();
		}

		#region ConformanceLevel

		[Test]
		public void InferConformanceLevel ()
		{
			XmlReader xr = XmlReader.Create (new StringReader ("<foo/><bar/>"));
			
			AssertType.AreEqual (ConformanceLevel.Document, xr.Settings.ConformanceLevel);
		}

		[Test]
		public void InferWrappedReaderConformance ()
		{
			// Actually this test is weird, since XmlTextReader
			// instance here does not have XmlReaderSettings.
			XmlReaderSettings settings = new XmlReaderSettings ();
			settings.ConformanceLevel = ConformanceLevel.Auto;
			XmlReader xr = XmlReader.Create (
				XmlReader.Create (new StringReader ("<foo/><bar/>")),
				settings);
			AssertType.AreEqual (ConformanceLevel.Document, xr.Settings.ConformanceLevel);
		}

		[Test]
		[ExpectedException (typeof (XmlException))]
		public void CreateConformanceDocument ()
		{
			XmlReaderSettings s = new XmlReaderSettings ();
			s.ConformanceLevel = ConformanceLevel.Document;
			XmlReader xr = XmlReader.Create (new StringReader (
				"<foo/><bar/>"), s);
			while (!xr.EOF)
				xr.Read ();
		}

		[Test]
		public void CreateConformanceFragment ()
		{
			XmlReaderSettings settings = new XmlReaderSettings ();
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			XmlReader xr = XmlReader.Create (new StringReader (
				"<foo/><bar/>"), settings);
			while (!xr.EOF)
				xr.Read ();
		}

		[Test]
		[ExpectedException (typeof (InvalidOperationException))]
		public void CreateConformanceChangeToDocument ()
		{
			// Actually this test is weird, since XmlTextReader
			// instance here does not have XmlReaderSettings.
			XmlReaderSettings settings = new XmlReaderSettings ();
			settings.ConformanceLevel = ConformanceLevel.Document;
			XmlReader xr = XmlReader.Create (
				new XmlTextReader ("<foo/><bar/>", XmlNodeType.Element, null),
				settings);
			while (!xr.EOF)
				xr.Read ();
		}

		[Test]
		[ExpectedException (typeof (InvalidOperationException))]
		public void CreateConformanceChangeToFragment ()
		{
			// Actually this test is weird, since XmlTextReader
			// instance here does not have XmlReaderSettings.
			XmlReaderSettings settings = new XmlReaderSettings ();
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			XmlReader xr = XmlReader.Create (
				new XmlTextReader ("<foo/>", XmlNodeType.Document, null),
				settings);
			while (!xr.EOF)
				xr.Read ();
		}

		[Test]
		public void CreateConformanceLevelExplicitAuto ()
		{
			// Even if we specify ConformanceLevel.Auto explicitly,
			// XmlTextReader's ConformanceLevel becomes .Document.
			XmlReaderSettings settings = new XmlReaderSettings ();
			settings.ConformanceLevel = ConformanceLevel.Auto;
			XmlReader xr = XmlReader.Create (
				new XmlTextReader ("<foo/>", XmlNodeType.Document, null),
				settings);
			AssertType.AreEqual (ConformanceLevel.Document, xr.Settings.ConformanceLevel);
		}

		[Test]
		public void CreateKeepConformance ()
		{
			XmlReaderSettings settings;
			XmlReader xr;

			// Fragment -> Fragment
			settings = new XmlReaderSettings ();
			settings.ConformanceLevel = ConformanceLevel.Fragment;
			xr = XmlReader.Create (
				XmlReader.Create (new StringReader ("<foo/>"), settings),
				settings);
			while (!xr.EOF)
				xr.Read ();

			// Document -> Document
			settings.ConformanceLevel = ConformanceLevel.Document;
			xr = XmlReader.Create (
				XmlReader.Create (new StringReader ("<foo/>"), settings),
				settings);
			while (!xr.EOF)
				xr.Read ();
		}

		#endregion

		[Test]
		public void CreateClonesSettings ()
		{
			XmlReaderSettings settings = new XmlReaderSettings ();
			XmlReader xr = XmlReader.Create (new StringReader ("<doc/>"), settings);
			AssertType.IsFalse (Object.ReferenceEquals (settings, xr.Settings));
		}

		[Test]
		public void CreateValidatorFromNonIXmlNamespaceResolver ()
		{
			XmlReaderSettings settings = new XmlReaderSettings ();
#if NOT_PFX
settings.Schemas.Add (null, "Test/XmlFiles/xsd/xml.xsd");
#endif
			settings.ValidationType = ValidationType.Schema;
			XmlReader xr = XmlReader.Create (new StringReader ("<root/>"));
			XmlReader dr = new Commons.Xml.XmlDefaultReader (xr);
			// XmlDefaultReader does not implement IXmlNamespaceResolver
			// but don't reject because of that fact.
			XmlReader r = XmlReader.Create (dr, settings);
		}

#if NOT_PFX
		[Test]
		public void NullResolver ()
		{
			XmlReaderSettings settings = new XmlReaderSettings ();
			settings.XmlResolver = null;
			using (XmlReader xr = XmlReader.Create ("Test/XmlFiles/simple.xml", settings)) {
				while (!xr.EOF)
					xr.Read ();
			}
        }

		class ThrowExceptionResolver : XmlResolver
		{


            public override ICredentials Credentials {
				set { }
			}

			public override object GetEntity (Uri uri, string type, Type expected)
			{
				throw new ApplicationException ("error");
			}
		}


        [Test]
		[ExpectedException (typeof (ApplicationException))]
		public void CustomResolverUsedForXmlStream ()
		{
			XmlReaderSettings settings = new XmlReaderSettings ();
			settings.XmlResolver = new ThrowExceptionResolver ();
			using (XmlReader xr = XmlReader.Create ("Test/XmlFiles/simple.xml", settings)) {
				while (!xr.EOF)
					xr.Read ();
			}
		}

		[Test]
		[ExpectedException (typeof (ApplicationException))]
		public void ValidationEventHandler ()
		{
			XmlReaderSettings settings = new XmlReaderSettings ();
            settings.Schemas.Add (new XmlSchema ());
			settings.ValidationType = ValidationType.Schema;
			settings.ValidationEventHandler += delegate (object o, ValidationEventArgs e) {
				throw new ApplicationException ();
			};
			XmlReader r = XmlReader.Create (
				new StringReader ("<root/>"), settings);
			while (!r.EOF)
				r.Read ();
		}

        [Test]
        [ExpectedException (typeof (XmlSchemaValidationException))]
		// make sure that Create(string,XmlReaderSettings) returns
		// validating XmlReader.
		public void CreateFromUrlWithValidation ()
		{
			XmlReaderSettings settings = new XmlReaderSettings();
			XmlSchema xs = new XmlSchema ();

            settings.Schemas.Add (xs);

			settings.ValidationType = ValidationType.Schema;
			using (XmlReader r = XmlReader.Create ("Test/XmlFiles/simple.xml", settings)) {
				r.Read ();
			}
        }



		[Test]
		public void ResolveEntities () // bug #81000
		{
			XmlReaderSettings s = new XmlReaderSettings ();
			s.ProhibitDtd = false;
			s.XmlResolver = new XmlResolver81000 ();

			string xml = "<!DOCTYPE root SYSTEM \"foo.dtd\"><root>&alpha;</root>";
			XmlReader r = XmlReader.Create (new StringReader (xml), s);
			r.Read ();
			r.Read ();
			r.Read ();
			// not EntityReference but Text
			AssertEquals ("#1", XmlNodeType.Text, r.NodeType);
			r.Read ();
			AssertEquals ("#2", XmlNodeType.EndElement, r.NodeType);
		}


		public class XmlResolver81000 : XmlResolver
		{

public override ICredentials Credentials { set {} }


			public override object GetEntity (Uri uri, string role, Type type)
			{
				return new MemoryStream (Encoding.UTF8.GetBytes ("<!ENTITY alpha \"bravo\">"));
			}
		}
#endif

        [Test]
		public void IgnoreComments () // Bug #82062.
		{
			string xml = "<root><!-- ignore --></root>";
			XmlReaderSettings s = new XmlReaderSettings ();
			s.IgnoreComments = true;
			XmlReader r = XmlReader.Create (new StringReader (xml), s);
			r.Read ();
			r.Read ();
			AssertEquals (String.Empty, r.Value); // should not be at the comment node.
		}
	}
}
#endif
