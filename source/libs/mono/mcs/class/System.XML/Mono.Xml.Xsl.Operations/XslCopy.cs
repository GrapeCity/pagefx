//
// XslCopy.cs
//
// Authors:
//	Ben Maurer (bmaurer@users.sourceforge.net)
//	Atsushi Enomoto (ginga@kit.hi-ho.ne.jp)
//	
// (C) 2003 Ben Maurer
// (C) 2003 Atsushi Enomoto
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

using System;
using System.Collections;
using System.Xml;
using System.Xml.XPath;
using System.Xml.Xsl;

namespace Mono.Xml.Xsl.Operations {
	internal class XslCopy : XslCompiledElement {
		XslOperation children;
		XmlQualifiedName [] useAttributeSets;
		Hashtable nsDecls;
		
		public XslCopy (Compiler c) : base (c) {}
		
		protected override void Compile (Compiler c)
		{
			if (c.Debugger != null)
				c.Debugger.DebugCompile (c.Input);

			this.nsDecls = c.GetNamespacesToCopy ();
			if (nsDecls.Count == 0)
				nsDecls = null;

			c.CheckExtraAttributes ("copy", "use-attribute-sets");

			useAttributeSets = c.ParseQNameListAttribute ("use-attribute-sets");
			
			if (!c.Input.MoveToFirstChild ()) return;
			children = c.CompileTemplateContent();
			c.Input.MoveToParent ();
		}

		public override void Evaluate (XslTransformProcessor p)
		{
			if (p.Debugger != null)
				p.Debugger.DebugExecute (p, this.DebugInput);

			switch (p.CurrentNode.NodeType)
			{
			case XPathNodeType.Root:
				if (p.Out.CanProcessAttributes && useAttributeSets != null)
					foreach (XmlQualifiedName s in useAttributeSets) {
						XslAttributeSet attset = p.ResolveAttributeSet (s);
						if (attset == null)
							throw new XsltException ("Attribute set was not found", null, p.CurrentNode);
						attset.Evaluate (p);
					}

				if (children != null) children.Evaluate (p);
				break;
			case XPathNodeType.Element:
				bool isCData = p.InsideCDataElement;
				string prefix = p.CurrentNode.Prefix;
				p.PushElementState (prefix, p.CurrentNode.LocalName, p.CurrentNode.NamespaceURI, true);
				p.Out.WriteStartElement (prefix, p.CurrentNode.LocalName, p.CurrentNode.NamespaceURI);
				
				if (useAttributeSets != null)
					foreach (XmlQualifiedName s in useAttributeSets)
						p.ResolveAttributeSet (s).Evaluate (p);

				if (p.CurrentNode.MoveToFirstNamespace (XPathNamespaceScope.ExcludeXml)) {
					do {
						if (p.CurrentNode.LocalName == prefix)
							continue;
						p.Out.WriteNamespaceDecl (p.CurrentNode.LocalName, p.CurrentNode.Value);
					} while (p.CurrentNode.MoveToNextNamespace (XPathNamespaceScope.ExcludeXml));
					p.CurrentNode.MoveToParent ();
				}

				if (children != null) children.Evaluate (p);

				p.Out.WriteFullEndElement ();
				p.PopCDataState (isCData);
				break;
			case XPathNodeType.Attribute:
				p.Out.WriteAttributeString (p.CurrentNode.Prefix, p.CurrentNode.LocalName, p.CurrentNode.NamespaceURI, p.CurrentNode.Value);
				break;
			
			case XPathNodeType.SignificantWhitespace:
			case XPathNodeType.Whitespace:
				bool cdata = p.Out.InsideCDataSection;
				p.Out.InsideCDataSection = false;
				p.Out.WriteString (p.CurrentNode.Value);
				p.Out.InsideCDataSection = cdata;
				break;
			case XPathNodeType.Text:
				p.Out.WriteString (p.CurrentNode.Value);
				break;
			
			case XPathNodeType.Comment:
				p.Out.WriteComment (p.CurrentNode.Value);
				break;
			
			case XPathNodeType.ProcessingInstruction:
				p.Out.WriteProcessingInstruction (p.CurrentNode.Name, p.CurrentNode.Value);
				break;

			case XPathNodeType.Namespace:
				if (p.XPathContext.ElementPrefix != p.CurrentNode.Name)
					p.Out.WriteNamespaceDecl (p.CurrentNode.Name, p.CurrentNode.Value);
				break;

			default:
//				Console.WriteLine ("unhandled node type {0}", p.CurrentNode.NodeType);
				break;
			}
		}
	}
}
