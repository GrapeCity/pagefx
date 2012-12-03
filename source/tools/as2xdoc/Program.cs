using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using DataDynamics;
using DataDynamics.PageFX.Common.Utilities;

namespace DataDynamics.Tools
{
    class Program
    {
        static CommandLine cl;

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                //args = new[] { @"/out:c:\xdoc.xml", @"E:\Work\Projects\PageFX\Source\tools\flexdoc\asdoc\flex3\mx.accessibility.AccImpl.xml" };
                //args = new[] { @"/out:c:\xdoc.xml", @"E:\Work\Projects\PageFX\Source\tools\flexdoc\asdoc\flex3\mx.controls.Button.xml" };
                //args = new[] { @"/out:c:\xdoc.xml", @"E:\Work\Projects\PageFX\Source\tools\flexdoc\flex3.asdoc.xml" };
                Usage();
                return;
            }

            cl = CommandLine.Parse(args);

            try
            {
                Core();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Environment.Exit(-1);
            }
        }

        static void Usage()
        {
        }

        static XmlDocument _doc;
        static readonly List<Member> _members = new List<Member>();
        static readonly Hashtable _nscache = new Hashtable();

        static void Core()
        {
            string[] files = cl.GetInputFiles();
            if (files.Length != 1)
            {
                Usage();
                Console.WriteLine("No input files.");
                return;
            }

            string input = files[0];
            
            _doc = new XmlDocument();
            _doc.Load(input);

            string outpath = cl.GetPath(input + ".xdoc.xml", "out", "output");
            string asm = cl.GetOption("", "asm", "assembly");
            
            LoadMembers(_doc.DocumentElement, null);

            var xws = new XmlWriterSettings { Indent = true, IndentChars = "  " };
            using (var writer = XmlWriter.Create(outpath, xws))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("doc");

                writer.WriteStartElement("assembly");
                writer.WriteElementString("name", asm);
                writer.WriteEndElement();

                writer.WriteStartElement("members");
                foreach (var member in _members)
                {
                    member.Write(writer);
                }
                writer.WriteEndElement();

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        static void LoadMembers(XmlNode parentElem, Member parent)
        {
            foreach (var child in Utils.GetElems(parentElem, false))
            {
                LoadMember(child, parent);
            }
        }

        static void LoadMember(XmlElement elem, Member parent)
        {
            switch (elem.LocalName)
            {
                case "asPackage":
                    {
                        string name = elem.GetAttribute("name");
                        var member = _nscache[name] as Member;
                        if (member == null)
                        {
                            member = new Member
                                         {
                                             Name = ("N:" + name),
                                             Summary = Utils.GetDesc(elem)
                                         };
                            _members.Add(member);
                            _nscache[name] = member;
                        }
                        LoadMembers(elem, member);
                    }
                    break;

                case "asClass":
                    {
                        string name = elem.GetAttribute("fullname");
                        name = Utils.FixTypeName(name);

                        var member = new Member
                                         {
                                             TypeName = name,
                                             Name = "T:" + name,
                                             Summary = Utils.GetDesc(elem)
                                         };
                        _members.Add(member);
                        LoadMembers(elem, member);
                    }
                    break;

                case "constructor":
                    {
                        var member = new Method
                                         {
                                             Name = ("M:" + parent.TypeName + ".#ctor"),
                                             Summary = Utils.GetDesc(elem)
                                         };
                        _members.Add(member);
                        LoadMembers(elem, member);
                    }
                    break;

                case "method":
                    {
                        string name = elem.GetAttribute("name");
                        var member = new Method
                                         {
                                             Name = ("M:" + parent.TypeName + "." + name),
                                             Summary = Utils.GetDesc(elem)
                                         };
                        _members.Add(member);
                        LoadMembers(elem, member);
                    }
                    break;

                case "param":
                    {
                        var method = parent as Method;
                        if (method == null)
                            throw new InvalidOperationException();

                        string name = elem.GetAttribute("name");
                        string type = elem.GetAttribute("type");
                        type = Utils.FixTypeName(type);
                        var param = new Param
                                        {
                                            Name = name,
                                            Type = type,
                                            Summary = Utils.GetDesc(elem),
                                        };
                        method.Params.Add(param);
                    }
                    break;

                case "result":
                    {
                        var method = parent as Method;
                        if (method == null)
                            throw new InvalidOperationException();
                        method.Returns = Utils.GetDesc(elem);
                    }
                    break;

                case "field":
                    {
                        string name = elem.GetAttribute("name");
                        string fullname = elem.GetAttribute("fullname");
                        //Console.WriteLine(fullname);
                        string prefix = "F:";
                        if (!string.IsNullOrEmpty(fullname)
                            && (fullname.EndsWith("/get") || fullname.EndsWith("/set")))
                            prefix = "P:";
                        var member = new Member
                                         {
                                             Name = prefix + parent.TypeName + "." + name,
                                             Summary = Utils.GetDesc(elem),
                                         };
                        _members.Add(member);
                    }
                    break;

                case "event":
                    {
                        string name = elem.GetAttribute("name");
                        var member = new Member
                                         {
                                             Name = "E:" + parent.TypeName + "." + name,
                                             Summary = Utils.GetDesc(elem),
                                         };
                        _members.Add(member);
                        LoadMembers(elem, member);
                    }
                    break;

                case "eventObject":
                    {
                        string label = elem.GetAttribute("label");
                        parent.Type = label;
                    }
                    break;

                default:
                    LoadMembers(elem, parent);
                    break;
            }
        }
    }
}
