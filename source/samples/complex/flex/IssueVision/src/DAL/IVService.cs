using System;
using System.Data;
using System.IO;
using System.Xml;
using mx.controls;
using mx.rpc.soap;
using mx.rpc.events;

namespace IssueVision
{
    public class IVService
    {
        #region Const
        public const string TargetXmlNamespace = "http://tempuri.org/IssueVision.Web/IssueVisionServices";
        public const string RootUrl = "http://localhost/IssueVisionWebCS/";
        public const string AsmxUrl = RootUrl + "IssueVisionServices.asmx";
        public const string WSDL = AsmxUrl + "?wsdl";
        public const string XmlnsDiffgram = "urn:schemas-microsoft-com:xml-diffgram-v1";
        #endregion

        private readonly WebService _service;
        private readonly SOAPHeader _header;
        private OpCode _curop;

        public IVService()
        {
            _service = new WebService();
            _service.wsdl = WSDL;
            _service.useProxy = false;
            _service.fault += OnFault;
            _service.result += OnResult;
            _service.loadWSDL();
            
            CredentialSoapHeader h = new CredentialSoapHeader("demo", "demo");
            _header = new SOAPHeader(new Avm.QName(TargetXmlNamespace, "CredentialSoapHeader"), h);
        }

        public Operation GetOperation(string name)
        {
            Operation op = (Operation)_service.getOperation(name);
            if (op == null)
                throw new InvalidOperationException("No operation " + name);
            op.headers.push(_header);
            return op;
        }

        private Operation GetOperationE4X(string name)
        {
            Operation op = GetOperation(name);
            op.resultFormat = "e4x";
            return op;
        }

        #region Operations
        private static Avm.Date GetLastAccessedDate()
        {
            //return new Avm.Date(2008, 2, 25);
            return new Avm.Date(1900, 0, 1);
        }

        public void GetIssuesXml()
        {
            Operation op = GetOperationE4X(OpNames.SendReceiveIssues);
            _curop = OpCode.GetIssuesXml;
            op.send(null, GetLastAccessedDate());
        }

        public void GetIssues()
        {
            Operation op = GetOperationE4X(OpNames.SendReceiveIssues);
            _curop = OpCode.GetIssues;
            op.send(null, GetLastAccessedDate());
        }

        public void GetLookupTablesXml()
        {
            Operation op = GetOperationE4X(OpNames.GetLookupTables);
            _curop = OpCode.GetLookupTablesXml;
            op.send();
        }

        public void GetLookupTables()
        {
            Operation op = GetOperationE4X(OpNames.GetLookupTables);
            _curop = OpCode.GetLookupTables;
            op.send();
        }
        #endregion

        #region Events
        public event XmlStringHandler IssuesLoadedXml;

        public event IVDataSetHandler IssuesLoaded;

        public event XmlStringHandler LookupLoadedXml;

        public event IVDataSetHandler LookupLoaded;
        #endregion

        private static void OnFault(FaultEvent e)
        {
            Alert.show(e.fault.toString());
        }

        #region Result Handler
        private void OnResult(ResultEvent e)
        {
            if (e.result == null)
                return;

            OpCode op = _curop;
            _curop = OpCode.None;
            switch (op)
            {
                case OpCode.GetIssuesXml:
                    {
                        string xml = Utils.ToXmlString(e.result);
                        //string xml = GetDifgramString(e.result);
                        if (IssuesLoadedXml != null)
                            IssuesLoadedXml(xml);
                    }
                    break;

                case OpCode.GetIssues:
                    {
                        string xml = GetDifgramString(e.result, "SendReceiveIssuesResult");
                        if (!string.IsNullOrEmpty(xml))
                        {
                            StringReader reader = new StringReader(xml);
                            IVDataSet data = new IVDataSet();
                            data.ReadXml(reader, XmlReadMode.DiffGram);
                            if (IssuesLoaded != null)
                                IssuesLoaded(data);
                        }
                    }
                    break;

                case OpCode.GetLookupTablesXml:
                    {
                        string xml = Utils.ToXmlString(e.result);
                        if (LookupLoadedXml != null)
                            LookupLoadedXml(xml);
                    }
                    break;

                case OpCode.GetLookupTables:
                    {
                        string xml = GetDifgramString(e.result, "GetLookupTablesResult");
                        if (!string.IsNullOrEmpty(xml))
                        {
                            StringReader reader = new StringReader(xml);
                            IVDataSet data = new IVDataSet();
                            data.ReadXml(reader, XmlReadMode.DiffGram);
                            if (LookupLoaded != null)
                                LookupLoaded(data);
                        }
                    }
                    break;
            }
        }
        #endregion

        #region Utils
        private static Avm.XML GetDifgram(object result, string resultTag)
        {
            Avm.XML xml = null;
            Avm.XMLList xl = result as Avm.XMLList;

            if (xl != null)
                xml = xl[0];

            if (xml == null)
                xml = result as Avm.XML;

            if (xml == null)
                return null;

            xl = xml[TargetXmlNamespace, resultTag];
            if (xl != null && xl.length() == 1)
            {
                xl = xl[0][XmlnsDiffgram, "diffgram"];
                if (xl != null && xl.length() == 1)
                    return xl[0];
            }

            return xml;
        }

        private static string GetDifgramString(object result, string resultTag)
        {
            Avm.XML x = GetDifgram(result, resultTag);
            if (x == null)
                return "";
            return x.toXMLString();
        }
        #endregion
    }

    internal class OpNames
    {
        public const string SendReceiveIssues = "SendReceiveIssues";
        public const string GetLookupTables = "GetLookupTables";
    }

    internal enum OpCode
    {
        None,
        GetIssuesXml,
        GetIssues,
        GetLookupTablesXml,
        GetLookupTables
    }
}