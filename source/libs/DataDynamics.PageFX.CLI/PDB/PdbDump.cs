using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.SymbolStore;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Xml;

namespace DataDynamics.PageFX.PDB
{
    //http://blogs.msdn.com/jmstall/articles/sample_pdb2xml.aspx

    public static class PdbDump
    {
        private static string GetOutPath(string path)
        {
            return Path.Combine(Path.GetDirectoryName(path),
                                Path.GetFileNameWithoutExtension(path) + ".pdb.xml");
        }

        public static void Write(string path)
        {
            Write(Assembly.ReflectionOnlyLoadFrom(path), GetOutPath(path));
        }

        public static void Write(Assembly assembly)
        {
            Write(assembly, GetOutPath(assembly.Location));
        }

        public static void Write(Assembly assembly, string path)
        {
            var xws = new XmlWriterSettings {Indent = true, IndentChars = "  "};
            using (var writer = XmlWriter.Create(path, xws))
            {
                Write(assembly, writer);
            }
        }

        public static void Write(Assembly assembly, XmlWriter writer)
        {
            var reader = SymbolUtil.GetPdbReader(assembly);
            if (reader == null)
            {
                return;
            }

            var impl = new Impl { m_assembly = assembly, m_writer = writer };
            impl.Dump(reader.SymReader);
        }

        private class Impl
        {
            public Assembly m_assembly;
            public XmlWriter m_writer;

            // Maps files to ids. 
            readonly Dictionary<string, int> m_fileMapping = new Dictionary<string, int>();

            public void Dump(ISymbolReader reader)
            {
                string path = m_assembly.Location;

                // Begin writing XML.
                m_writer.WriteStartDocument();
                m_writer.WriteComment("This is an XML file representing the PDB for '" + path + "'");
                m_writer.WriteStartElement("symbols");


                // Record what input file these symbols are for.
                m_writer.WriteAttributeString("file", path);

                WriteDocs(reader);
                WriteEntryPoint(reader);
                WriteMethods(reader);

                m_writer.WriteEndElement(); // "Symbols";
            }

            // Write all docs, and add to the m_fileMapping list.
            // Other references to docs will then just refer to this list.
            private void WriteDocs(ISymbolReader reader)
            {
                m_writer.WriteComment("This is a list of all source files referred by the PDB.");

                int id = 0;
                // Write doc list
                m_writer.WriteStartElement("files");
                {
                    var docs = reader.GetDocuments();
                    foreach (var doc in docs)
                    {
                        string url = doc.URL;

                        // Symbol store may give out duplicate documents. We'll fold them here
                        if (m_fileMapping.ContainsKey(url))
                        {
                            m_writer.WriteComment("There is a duplicate entry for: " + url);
                            continue;
                        }
                        id++;
                        m_fileMapping.Add(doc.URL, id);

                        m_writer.WriteStartElement("file");
                        m_writer.WriteAttributeString("id", id.ToString());
                        m_writer.WriteAttributeString("name", doc.URL);
                        //m_writer.WriteAttributeString("CheckSumAlgorithmId", doc.CheckSumAlgorithmId.ToString());
                        //m_writer.WriteAttributeString("DocumentType", doc.DocumentType.ToString());
                        //m_writer.WriteAttributeString("HasEmbeddedSource", doc.HasEmbeddedSource.ToString());
                        //m_writer.WriteAttributeString("Language", doc.Language.ToString());
                        //m_writer.WriteAttributeString("LanguageVendor", doc.LanguageVendor.ToString());
                        //m_writer.WriteAttributeString("SourceLength", doc.SourceLength.ToString());
                        m_writer.WriteEndElement(); // file
                    }
                }
                m_writer.WriteEndElement(); // files
            }

            // Write out a reference to the entry point method (if one exists)
            void WriteEntryPoint(ISymbolReader reader)
            {
                try
                {
                    // If there is no entry point token (such as in a dll), this will throw.
                    var token = reader.UserEntryPoint;
                    var m = reader.GetMethod(token);

                    Debug.Assert(m != null); // would have thrown by now.

                    // Should not throw past this point
                    m_writer.WriteComment(
                        "This is the token for the 'entry point' method, which is the method that will be called when the assembly is loaded." +
                        " This usually corresponds to 'Main'");

                    m_writer.WriteStartElement("EntryPoint");
                    WriteMethod(m);
                    m_writer.WriteEndElement();
                }
                catch (COMException)
                {
                    // If the Symbol APIs fail when looking for an entry point token, there is no entry point.
                    m_writer.WriteComment(
                        "There is no entry point token such as a 'Main' method. This module is probably a '.dll'");
                }
            }

            // Write out XML snippet to refer to the given method.
            void WriteMethod(ISymbolMethod method)
            {
                m_writer.WriteElementString("methodref", AsToken(method.Token.GetToken()));
            }

            // Dump all of the methods in the given ISymbolReader to the XmlWriter provided in the ctor.
            void WriteMethods(ISymbolReader reader)
            {
                m_writer.WriteComment("This is a list of all methods in the assembly that matches this PDB.");
                m_writer.WriteComment("For each method, we provide the sequence tables that map from IL offsets back to source.");

                m_writer.WriteStartElement("methods");

                // Use reflection to enumerate all methods            
                foreach (var t in m_assembly.GetTypes())
                {
                    foreach (var methodReflection in t.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static | BindingFlags.DeclaredOnly))
                    {
                        int token = methodReflection.MetadataToken;

                        m_writer.WriteStartElement("method");
                        m_writer.WriteAttributeString("name", t.FullName + "." + methodReflection.Name);
                        m_writer.WriteAttributeString("token", AsToken(token));
                        try
                        {
                            ISymbolMethod methodSymbol = reader.GetMethod(new SymbolToken(token));
                            WriteSequencePoints(methodSymbol);
                            WriteLocals(methodSymbol);
                        }
                        catch (COMException)
                        {
                            m_writer.WriteComment("No symbol info");
                        }
                        m_writer.WriteEndElement(); // method                    
                    }
                }
                m_writer.WriteEndElement();
            }


            // Write the sequence points for the given method
            // Sequence points are the map between IL offsets and source lines.
            // A single method could span multiple files (use C#'s #line directive to see for yourself).        
            private void WriteSequencePoints(ISymbolMethod method)
            {
                m_writer.WriteStartElement("sequence-points");

                int count = method.SequencePointCount;
                m_writer.WriteAttributeString("total", count.ToString());

                // Get the sequence points from the symbol store. 
                // We could cache these arrays and reuse them.
                var offsets = new int[count];
                var docs = new ISymbolDocument[count];
                var startColumn = new int[count];
                var endColumn = new int[count];
                var startRow = new int[count];
                var endRow = new int[count];
                method.GetSequencePoints(offsets, docs, startRow, startColumn, endRow, endColumn);

                // Write out sequence points
                for (int i = 0; i < count; i++)
                {
                    m_writer.WriteStartElement("entry");
                    m_writer.WriteAttributeString("il_offset", AsIlOffset(offsets[i]));

                    // If it's a special 0xFeeFee sequence point (eg, "hidden"), 
                    // place an attribute on it to make it very easy for tools to recognize.
                    // See http://blogs.msdn.com/jmstall/archive/2005/06/19/FeeFee_SequencePoints.aspx
                    if (startRow[i] == 0xFeeFee)
                    {
                        m_writer.WriteAttributeString("hidden", XmlConvert.ToString(true));
                    }
                    else
                    {
                        m_writer.WriteAttributeString("start_row", startRow[i].ToString());
                        m_writer.WriteAttributeString("start_column", startColumn[i].ToString());
                        m_writer.WriteAttributeString("end_row", endRow[i].ToString());
                        m_writer.WriteAttributeString("end_column", endColumn[i].ToString());
                        m_writer.WriteAttributeString("file_ref", m_fileMapping[docs[i].URL].ToString());
                    }
                    m_writer.WriteEndElement();
                }

                m_writer.WriteEndElement(); // sequencepoints
            }

            // Write all the locals in the given method out to an XML file.
            // Since the symbol store represents the locals in a recursive scope structure, we need to walk a tree.
            // Although the locals are technically a hierarchy (based off nested scopes), it's easiest for clients
            // if we present them as a linear list. We will provide the range for each local's scope so that somebody
            // could reconstruct an approximation of the scope tree. The reconstruction may not be exact.
            // (Note this would still break down if you had an empty scope nested in another scope.
            private void WriteLocals(ISymbolMethod method)
            {
                try
                {
                    var vars = method.GetParameters();
                    if (vars != null)
                    {
                        m_writer.WriteStartElement("params");
                        WriteVars(vars);
                        m_writer.WriteEndElement();
                    }
                }
                catch (Exception e)
                {
					Trace.TraceError("{0}", e);
                }

                m_writer.WriteStartElement("locals");
                // If there are no locals, then this element will just be empty.
                WriteLocals(method.RootScope);
                m_writer.WriteEndElement();
            }

            // Helper method to write the local variables in the given scope.
            // Scopes match an IL range, and also have child scopes.
            private void WriteLocals(ISymbolScope scope)
            {
                m_writer.WriteStartElement("scope");
                // Provide scope range
                m_writer.WriteAttributeString("start-offset", AsIlOffset(scope.StartOffset));
                m_writer.WriteAttributeString("end-offset", AsIlOffset(scope.EndOffset));

                WriteVars(scope.GetLocals());

                foreach (var childScope in scope.GetChildren())
                    WriteLocals(childScope);

                m_writer.WriteEndElement();
            }

            private void WriteVars(IEnumerable<ISymbolVariable> vars)
            {
                if (vars == null) return;
                foreach (var var in vars)
                    WriteVar(var);
            }

            private void WriteVar(ISymbolVariable var)
            {
                m_writer.WriteStartElement("local");
                m_writer.WriteAttributeString("name", var.Name);

                // Each local maps to a unique "IL Index" or "slot" number.
                // This index is what you pass to ICorDebugILFrame::GetLocalVariable() to get
                // a specific local variable. 
                //Debug.Assert(l.AddressKind == SymAddressKind.ILOffset);
                int slot = var.AddressField1;
                m_writer.WriteAttributeString("il_index", slot.ToString());

                m_writer.WriteEndElement(); // local
            }

            // Format a token to a string. Tokens are in hex.
            public static string AsToken(int i)
            {
                return String.Format(CultureInfo.InvariantCulture, "0x{0:x}", i);
            }

            // Since we're spewing this to XML, spew as a decimal number.
            public static string AsIlOffset(int i)
            {
                return i.ToString();
            }
        }
    }
}