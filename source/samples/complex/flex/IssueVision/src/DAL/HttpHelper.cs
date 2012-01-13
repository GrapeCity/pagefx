using mx.controls;
using mx.rpc.events;
using mx.rpc.http;

namespace IssueVision
{
    public class HttpHelper
    {
        public static void LoadXml(string url, XmlStringHandler handler)
        {
            HTTPService loader = new HTTPService();
            loader.resultFormat = "e4x";
            loader.fault += loader_fault;

            ResultHandler rh = new ResultHandler();
            rh.handler = handler;

            loader.result += rh.OnResult;
            
            loader.url = url;
            loader.send();
        }

        static void loader_fault(FaultEvent e)
        {
            Alert.show(e.fault.toString());
        }

        private class ResultHandler
        {
            public XmlStringHandler handler;

            public void OnResult(ResultEvent e)
            {
                string xml = Utils.ToXmlString(e.result);
                if (handler != null)
                    handler(xml);
            }   
        }
    }
}