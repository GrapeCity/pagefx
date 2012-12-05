using System;
using System.IO;
using System.Xml;

class X
{
    static void PrintAttrs(XmlReader reader)
    {
        if (reader.HasAttributes)
        {
            while (reader.MoveToNextAttribute())
            {
                Console.WriteLine(" {0}=\"{1}\"", reader.Name, reader.Value);
            }
            // Move the reader back to the element node.
            reader.MoveToElement();
        }
    }

    static void Print(XmlReader reader)
    {
        while (reader.Read())
        {
            switch (reader.NodeType)
            {
                case XmlNodeType.Element:
                    {
                        bool empty = reader.IsEmptyElement;
                        Console.WriteLine("<{0}", reader.Name);
                        PrintAttrs(reader);
                        if (empty) Console.WriteLine("/>");
                        else Console.WriteLine(">");
                    }
                    break;

                case XmlNodeType.Text:
                    Console.WriteLine(reader.Value);
                    break;

                case XmlNodeType.CDATA:
                    Console.WriteLine("<![CDATA[{0}]]>", reader.Value);
                    break;

                case XmlNodeType.ProcessingInstruction:
                    Console.WriteLine("<?{0} {1}?>", reader.Name, reader.Value);
                    break;

                case XmlNodeType.Comment:
                    Console.WriteLine("<!--{0}-->", reader.Value);
                    break;

                case XmlNodeType.XmlDeclaration:
                    Console.WriteLine("<?xml version=\"1.0\"?>");
                    break;

                case XmlNodeType.Document:
                    break;

                case XmlNodeType.DocumentType:
                    Console.WriteLine("<!DOCTYPE {0} [{1}]", reader.Name, reader.Value);
                    break;

                case XmlNodeType.EntityReference:
                    Console.WriteLine(reader.Name);
                    break;

                case XmlNodeType.EndElement:
                    Console.WriteLine("</{0}>", reader.Name);
                    break;
            }
        }
    }

    static int n;

    static void Print(string xml)
    {
        ++n;
        Console.WriteLine("--- Sample #{0}.", n);
        try
        {
            XmlTextReader reader = new XmlTextReader(new StringReader(xml));
            Print(reader);
        }
        catch (XmlException exc)
        {
            Console.WriteLine("XmlException. Location: ({0}:{1}). Message: {2}", exc.LineNumber, exc.LinePosition, exc.Message);
        }
    }

    static void Test1()
    {
        Print("<root/>");
        Print("<set><table1><col1>sample text</col1><col2/></table1><table2 attr='value'><col3>sample text 2</col3></table2></set>");

        string xml = "<?xml version=\"1.0\"?>\n" +
"<!-- This is a sample XML document -->\n" +
"<!DOCTYPE Items [<!ENTITY number \"123\">]>\n" +
"<Items>\n" + 
  "<Item>Test with an entity: &number;</Item>\n" +
  "<Item>Test with a child element <more/> stuff</Item>\n" + 
  "<Item>Test with a CDATA section <![CDATA[<456>]]> def</Item>\n" +
  "<Item>Test with a char entity: &#65;</Item>\n" +
  "<!-- Fourteen chars in this element.-->\n" +
  "<Item>1234567890ABCD</Item>\n" +
"</Items>";
        Print(xml);
    }

    static void Test2()
    {
        string diffgram =
"<diffgr:diffgram xmlns:msdata=\"urn:schemas-microsoft-com:xml-msdata\" xmlns:diffgr=\"urn:schemas-microsoft-com:xml-diffgram-v1\">\n"
+ "  <IssueDetails xmlns=\"http://www.tempuri.org/IVDataSet.xsd\">\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory1\" msdata:rowOrder=\"0\">\n"
+ "      <IssueHistoryID>11</IssueHistoryID>\n"
+ "      <StafferID>1</StafferID>\n"
+ "      <IssueID>12</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2003-12-29T16:03:42.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Adam Barr</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory2\" msdata:rowOrder=\"1\">\n"
+ "      <IssueHistoryID>12</IssueHistoryID>\n"
+ "      <StafferID>2</StafferID>\n"
+ "      <IssueID>13</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2003-12-29T16:07:13.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Kim Abercrombie</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory3\" msdata:rowOrder=\"2\">\n"
+ "      <IssueHistoryID>13</IssueHistoryID>\n"
+ "      <StafferID>3</StafferID>\n"
+ "      <IssueID>14</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2003-12-29T16:09:54.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Rob Young</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory4\" msdata:rowOrder=\"3\">\n"
+ "      <IssueHistoryID>14</IssueHistoryID>\n"
+ "      <StafferID>2</StafferID>\n"
+ "      <IssueID>15</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2003-12-29T16:12:33.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Kim Abercrombie</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory5\" msdata:rowOrder=\"4\">\n"
+ "      <IssueHistoryID>15</IssueHistoryID>\n"
+ "      <StafferID>1</StafferID>\n"
+ "      <IssueID>16</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2003-12-29T16:14:08.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Adam Barr</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory6\" msdata:rowOrder=\"5\">\n"
+ "      <IssueHistoryID>16</IssueHistoryID>\n"
+ "      <StafferID>3</StafferID>\n"
+ "      <IssueID>17</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2003-12-29T16:15:19.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Rob Young</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory7\" msdata:rowOrder=\"6\">\n"
+ "      <IssueHistoryID>17</IssueHistoryID>\n"
+ "      <StafferID>2</StafferID>\n"
+ "      <IssueID>18</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2003-12-29T16:17:25.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Kim Abercrombie</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory8\" msdata:rowOrder=\"7\">\n"
+ "      <IssueHistoryID>18</IssueHistoryID>\n"
+ "      <StafferID>12</StafferID>\n"
+ "      <IssueID>13</IssueID>\n"
+ "      <Comment>test</Comment>\n"
+ "      <DateCreated>2004-01-10T10:09:11.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Demo</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory9\" msdata:rowOrder=\"8\">\n"
+ "      <IssueHistoryID>19</IssueHistoryID>\n"
+ "      <StafferID>1</StafferID>\n"
+ "      <IssueID>19</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:55.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Adam Barr</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory10\" msdata:rowOrder=\"9\">\n"
+ "      <IssueHistoryID>20</IssueHistoryID>\n"
+ "      <StafferID>2</StafferID>\n"
+ "      <IssueID>20</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Kim Abercrombie</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory11\" msdata:rowOrder=\"10\">\n"
+ "      <IssueHistoryID>21</IssueHistoryID>\n"
+ "      <StafferID>3</StafferID>\n"
+ "      <IssueID>21</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Rob Young</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory12\" msdata:rowOrder=\"11\">\n"
+ "      <IssueHistoryID>22</IssueHistoryID>\n"
+ "      <StafferID>4</StafferID>\n"
+ "      <IssueID>22</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Tom Youtsey</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory13\" msdata:rowOrder=\"12\">\n"
+ "      <IssueHistoryID>23</IssueHistoryID>\n"
+ "      <StafferID>5</StafferID>\n"
+ "      <IssueID>23</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Gary W. Yukish</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory14\" msdata:rowOrder=\"13\">\n"
+ "      <IssueHistoryID>24</IssueHistoryID>\n"
+ "      <StafferID>6</StafferID>\n"
+ "      <IssueID>24</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Rob Caron</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory15\" msdata:rowOrder=\"14\">\n"
+ "      <IssueHistoryID>25</IssueHistoryID>\n"
+ "      <StafferID>7</StafferID>\n"
+ "      <IssueID>25</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Karin Zimprich</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory16\" msdata:rowOrder=\"15\">\n"
+ "      <IssueHistoryID>26</IssueHistoryID>\n"
+ "      <StafferID>8</StafferID>\n"
+ "      <IssueID>26</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Randall Boseman</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory17\" msdata:rowOrder=\"16\">\n"
+ "      <IssueHistoryID>27</IssueHistoryID>\n"
+ "      <StafferID>9</StafferID>\n"
+ "      <IssueID>27</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Kevin Kennedy</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory18\" msdata:rowOrder=\"17\">\n"
+ "      <IssueHistoryID>28</IssueHistoryID>\n"
+ "      <StafferID>10</StafferID>\n"
+ "      <IssueID>28</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Diane Tibbott</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory19\" msdata:rowOrder=\"18\">\n"
+ "      <IssueHistoryID>29</IssueHistoryID>\n"
+ "      <StafferID>11</StafferID>\n"
+ "      <IssueID>29</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Garrett Young</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <IssueHistory diffgr:id=\"IssueHistory20\" msdata:rowOrder=\"19\">\n"
+ "      <IssueHistoryID>30</IssueHistoryID>\n"
+ "      <StafferID>12</StafferID>\n"
+ "      <IssueID>30</IssueID>\n"
+ "      <Comment>Issue created.</Comment>\n"
+ "      <DateCreated>2004-01-11T12:41:56.0000000+06:00</DateCreated>\n"
+ "      <DisplayName>Demo</DisplayName>\n"
+ "    </IssueHistory>\n"
+ "    <Issues diffgr:id=\"Issues1\" msdata:rowOrder=\"0\">\n"
+ "      <IssueID>12</IssueID>\n"
+ "      <StafferID>1</StafferID>\n"
+ "      <IssueTypeID>3</IssueTypeID>\n"
+ "      <Title>Computer won't shut down</Title>\n"
+ "      <Description>Computer just hangs after clicking Shut Down. User must manually turn computer off.</Description>\n"
+ "      <DateOpened>2003-12-29T16:03:42.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>abarr</UserName>\n"
+ "      <DisplayName>Adam Barr</DisplayName>\n"
+ "      <IssueType>Computer</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues2\" msdata:rowOrder=\"1\">\n"
+ "      <IssueID>13</IssueID>\n"
+ "      <StafferID>2</StafferID>\n"
+ "      <IssueTypeID>1</IssueTypeID>\n"
+ "      <Title>Voice mail password doesn't work</Title>\n"
+ "      <Description>The user went away on vacation and came back to find that his voice mail password didn't work.</Description>\n"
+ "      <DateOpened>2003-12-29T16:07:13.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>kabercrombie</UserName>\n"
+ "      <DisplayName>Kim Abercrombie</DisplayName>\n"
+ "      <IssueType>Telecommunications</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues3\" msdata:rowOrder=\"2\">\n"
+ "      <IssueID>14</IssueID>\n"
+ "      <StafferID>3</StafferID>\n"
+ "      <IssueTypeID>10</IssueTypeID>\n"
+ "      <Title>User can't log into network</Title>\n"
+ "      <Description>The user gets a message that her account has been locked.</Description>\n"
+ "      <DateOpened>2003-12-29T16:09:54.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>ryoung</UserName>\n"
+ "      <DisplayName>Rob Young</DisplayName>\n"
+ "      <IssueType>Network</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues4\" msdata:rowOrder=\"3\">\n"
+ "      <IssueID>15</IssueID>\n"
+ "      <StafferID>2</StafferID>\n"
+ "      <IssueTypeID>5</IssueTypeID>\n"
+ "      <Title>HERCULES printer driver not installed</Title>\n"
+ "      <Description>The user gets a message that the driver for printer HERCULES is not installed.</Description>\n"
+ "      <DateOpened>2003-12-29T16:12:33.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>kabercrombie</UserName>\n"
+ "      <DisplayName>Kim Abercrombie</DisplayName>\n"
+ "      <IssueType>Printer</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues5\" msdata:rowOrder=\"4\">\n"
+ "      <IssueID>16</IssueID>\n"
+ "      <StafferID>1</StafferID>\n"
+ "      <IssueTypeID>10</IssueTypeID>\n"
+ "      <Title>User's VPN access not working</Title>\n"
+ "      <Description>The user's home computer can't connect to the VPN server remotely.</Description>\n"
+ "      <DateOpened>2003-12-29T16:14:08.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>abarr</UserName>\n"
+ "      <DisplayName>Adam Barr</DisplayName>\n"
+ "      <IssueType>Network</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues6\" msdata:rowOrder=\"5\">\n"
+ "      <IssueID>17</IssueID>\n"
+ "      <StafferID>3</StafferID>\n"
+ "      <IssueTypeID>1</IssueTypeID>\n"
+ "      <Title>9 key not working</Title>\n"
+ "      <Description>The 9 key on the user's telephone does not work.</Description>\n"
+ "      <DateOpened>2003-12-29T16:15:19.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>ryoung</UserName>\n"
+ "      <DisplayName>Rob Young</DisplayName>\n"
+ "      <IssueType>Telecommunications</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues7\" msdata:rowOrder=\"6\">\n"
+ "      <IssueID>18</IssueID>\n"
+ "      <StafferID>2</StafferID>\n"
+ "      <IssueTypeID>3</IssueTypeID>\n"
+ "      <Title>Needs an upgrade to Office 2003</Title>\n"
+ "      <Description>Authorized and requested by department head.</Description>\n"
+ "      <DateOpened>2003-12-29T16:17:25.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>kabercrombie</UserName>\n"
+ "      <DisplayName>Kim Abercrombie</DisplayName>\n"
+ "      <IssueType>Computer</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues8\" msdata:rowOrder=\"7\">\n"
+ "      <IssueID>19</IssueID>\n"
+ "      <StafferID>1</StafferID>\n"
+ "      <IssueTypeID>1</IssueTypeID>\n"
+ "      <Title>Voice mail light does not work</Title>\n"
+ "      <Description/>\n"
+ "      <DateOpened>2004-01-11T12:41:55.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>abarr</UserName>\n"
+ "      <DisplayName>Adam Barr</DisplayName>\n"
+ "      <IssueType>Telecommunications</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues9\" msdata:rowOrder=\"8\">\n"
+ "      <IssueID>20</IssueID>\n"
+ "      <StafferID>2</StafferID>\n"
+ "      <IssueTypeID>10</IssueTypeID>\n"
+ "      <Title>Laptop in cubicle 119 can't access the WiFi network</Title>\n"
+ "      <Description>The laptop is 802.11a enabled.</Description>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>kabercrombie</UserName>\n"
+ "      <DisplayName>Kim Abercrombie</DisplayName>\n"
+ "      <IssueType>Network</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues10\" msdata:rowOrder=\"9\">\n"
+ "      <IssueID>21</IssueID>\n"
+ "      <StafferID>3</StafferID>\n"
+ "      <IssueTypeID>3</IssueTypeID>\n"
+ "      <Title>Docking station in cubicle 37 doesn't work</Title>\n"
+ "      <Description/>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>ryoung</UserName>\n"
+ "      <DisplayName>Rob Young</DisplayName>\n"
+ "      <IssueType>Computer</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues11\" msdata:rowOrder=\"10\">\n"
+ "      <IssueID>22</IssueID>\n"
+ "      <StafferID>4</StafferID>\n"
+ "      <IssueTypeID>1</IssueTypeID>\n"
+ "      <Title>Phone at cubicle 217 has an echo when dialing out</Title>\n"
+ "      <Description/>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>tyoutsey</UserName>\n"
+ "      <DisplayName>Tom Youtsey</DisplayName>\n"
+ "      <IssueType>Telecommunications</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues12\" msdata:rowOrder=\"11\">\n"
+ "      <IssueID>23</IssueID>\n"
+ "      <StafferID>5</StafferID>\n"
+ "      <IssueTypeID>5</IssueTypeID>\n"
+ "      <Title>Printer Zeus has run out of legal size paper</Title>\n"
+ "      <Description/>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>gyukish</UserName>\n"
+ "      <DisplayName>Gary W. Yukish</DisplayName>\n"
+ "      <IssueType>Printer</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues13\" msdata:rowOrder=\"12\">\n"
+ "      <IssueID>24</IssueID>\n"
+ "      <StafferID>6</StafferID>\n"
+ "      <IssueTypeID>10</IssueTypeID>\n"
+ "      <Title>Network load balancer is not seeing server #9</Title>\n"
+ "      <Description>Server #9 was re-imaged last week.</Description>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>rcaron</UserName>\n"
+ "      <DisplayName>Rob Caron</DisplayName>\n"
+ "      <IssueType>Network</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues14\" msdata:rowOrder=\"13\">\n"
+ "      <IssueID>25</IssueID>\n"
+ "      <StafferID>7</StafferID>\n"
+ "      <IssueTypeID>1</IssueTypeID>\n"
+ "      <Title>People on the other end of the line can't hear when the speaker phone is turned on</Title>\n"
+ "      <Description>It works fine using the handset or a headset.</Description>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>kzimprich</UserName>\n"
+ "      <DisplayName>Karin Zimprich</DisplayName>\n"
+ "      <IssueType>Telecommunications</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues15\" msdata:rowOrder=\"14\">\n"
+ "      <IssueID>26</IssueID>\n"
+ "      <StafferID>8</StafferID>\n"
+ "      <IssueTypeID>3</IssueTypeID>\n"
+ "      <Title>Graphics station in cubicle 79 needs an upgrade to a 20 inch monitor</Title>\n"
+ "      <Description>This has already been approved.</Description>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>rboseman</UserName>\n"
+ "      <DisplayName>Randall Boseman</DisplayName>\n"
+ "      <IssueType>Computer</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues16\" msdata:rowOrder=\"15\">\n"
+ "      <IssueID>27</IssueID>\n"
+ "      <StafferID>9</StafferID>\n"
+ "      <IssueTypeID>10</IssueTypeID>\n"
+ "      <Title>Firewall is blocking instant messenger</Title>\n"
+ "      <Description>Neither Yahoo nor AOL work. MSN does however.</Description>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>kkennedy</UserName>\n"
+ "      <DisplayName>Kevin Kennedy</DisplayName>\n"
+ "      <IssueType>Network</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues17\" msdata:rowOrder=\"16\">\n"
+ "      <IssueID>28</IssueID>\n"
+ "      <StafferID>10</StafferID>\n"
+ "      <IssueTypeID>5</IssueTypeID>\n"
+ "      <Title>Printer Apollo is low on toner</Title>\n"
+ "      <Description/>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>dtibbott</UserName>\n"
+ "      <DisplayName>Diane Tibbott</DisplayName>\n"
+ "      <IssueType>Printer</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues18\" msdata:rowOrder=\"17\">\n"
+ "      <IssueID>29</IssueID>\n"
+ "      <StafferID>11</StafferID>\n"
+ "      <IssueTypeID>3</IssueTypeID>\n"
+ "      <Title>Cubicle 597 needs a developer workstation</Title>\n"
+ "      <Description/>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>gyoung</UserName>\n"
+ "      <DisplayName>Garrett Young</DisplayName>\n"
+ "      <IssueType>Computer</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues19\" msdata:rowOrder=\"18\">\n"
+ "      <IssueID>30</IssueID>\n"
+ "      <StafferID>12</StafferID>\n"
+ "      <IssueTypeID>5</IssueTypeID>\n"
+ "      <Title>The printer in cubicle 18 needs to be moved to cubicle 67A</Title>\n"
+ "      <Description/>\n"
+ "      <DateOpened>2004-01-11T12:41:56.0000000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:08:47.2530000+06:00</DateModified>\n"
+ "      <UserName>demo</UserName>\n"
+ "      <DisplayName>Demo</DisplayName>\n"
+ "      <IssueType>Printer</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "    <Issues diffgr:id=\"Issues20\" msdata:rowOrder=\"19\">\n"
+ "      <IssueID>31</IssueID>\n"
+ "      <StafferID>1</StafferID>\n"
+ "      <IssueTypeID>10</IssueTypeID>\n"
+ "      <Title>The fucking system does not work</Title>\n"
+ "      <Description/>\n"
+ "      <DateOpened>2008-03-25T15:10:28.8930000+06:00</DateOpened>\n"
+ "      <IsOpen>true</IsOpen>\n"
+ "      <DateModified>2008-03-25T15:10:28.9100000+06:00</DateModified>\n"
+ "      <UserName>abarr</UserName>\n"
+ "      <DisplayName>Adam Barr</DisplayName>\n"
+ "      <IssueType>Network</IssueType>\n"
+ "      <HasConflicts>false</HasConflicts>\n"
+ "      <IsRead>false</IsRead>\n"
+ "    </Issues>\n"
+ "  </IssueDetails>\n"
+ "</diffgr:diffgram>\n"
;
        Print(diffgram);
    }

    static void Main()
    {
        Test1();
        Test2();
        Console.WriteLine("<%END%>");
    }
}