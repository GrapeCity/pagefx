#region SWC Catalog Schema
//<swc xmlns="http://www.adobe.com/flash/swccatalog/9">
//<versions>
//  <swc version="1.2" />
//  <flex version="3.0.0" build="477" />
//</versions>
//<features>
//  <feature-debug />
//  <feature-external-deps />
//  <feature-script-deps />
//  <feature-components />
//  <feature-files />
//</features>
//<components>
//  <component className="" name="" uri="" icon=""/>
//</components>
//<libraries>
//  <library path="library.swf">
//    <script name="" mod="" signatureChecksum="">
//      <def id=""/>
//      <dep id="" type=""/>
//      ...
//      <dep id="" type=""/>
//    </script>
//    <keep-as3-metadata>
//      <metadata name="" />
//      ...
//      <metadata name="" />
//    </keep-as3-metadata>
//    <digests>
//      <digest type="" signed="" value=""/>
//      ...
//      <digest type="" signed="" value=""/>
//    </digests>
//  </library>
//</libraries>
//<files>
//  <file path="" mod=""/>
//  ...
//  <file path="" mod=""/>
//</files>
//</swc>
#endregion

namespace DataDynamics.PageFX.FLI.SWC
{
    class SwcCatalog
    {
        public const string XmlNamespace = "http://www.adobe.com/flash/swccatalog/9";

        public class Elements
        {
            public const string Library = "library";
            public const string Def = "def";
            public const string Dep = "dep";
            public const string Digests = "digests";
            public const string Digest = "digest";
        }

        public static class DepTypes
        {
            public const string Inheritance = "i";
            public const string Namespace = "n";
            public const string Signature = "s";
            public const string Expression = "e";
        }
    }
}