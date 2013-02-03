using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Mono.Xml;
using NUnit.Framework;

namespace DataDynamics.PageFX
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            flashPlayer = new FlashControl();
            flashPlayer.Width = 800;
            flashPlayer.Height = 600;
            flashPlayer.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top | AnchorStyles.Bottom;
            flashPlayer.Dock = DockStyle.Fill;
            flashPlayer.FSCommand += OnFSCommand;
            Controls.Add(flashPlayer);
        }

        private readonly FlashControl flashPlayer;

        #region Events
        public EventHandler Exception;

        private void RaiseException(string msg, bool unhandled)
        {
            if (unhandled)
            {
                Console.WriteLine("Unhandled Flash Exception: {0}", msg);
            }

            if (Exception != null)
                Exception(this, EventArgs.Empty);

            flashPlayer.Stop();
            Close();
        }

        public EventHandler Terminated;

        private void RaiseTerminated()
        {
            if (Terminated != null)
                Terminated(this, EventArgs.Empty);
            flashPlayer.Stop();
            Close();
        }
        #endregion

        #region Loading
        private void MainForm_Load(object sender, EventArgs e)
        {
            flashPlayer.AllowScriptAccess = "always";
            flashPlayer.FlashCall += OnCallFromFlash;
            flashPlayer.LoadMovie(0, Global.swfpath);
            flashPlayer.Play();

            if (!Global.nodie)
            {
                if (timer == null)
                    timer = new Timer();
                timer.Interval = 200;
                timer.Enabled = true;
                timer.Tick += timer_Tick;
            }
        }

        private Timer timer;

        private void timer_Tick(object sender, EventArgs e)
        {
            if (!flashPlayer.IsPlaying())
            {
                timer.Enabled = false;
                timer.Tick -= timer_Tick;
                RaiseTerminated();
            }
        }
        #endregion

        #region FSCommand
        // Used for FlashPlayer7 ActiveX to log debug messages , in Player8 ExternalInterface is used
        private void OnFSCommand(object sender, FlashCommandEventArgs e)
        {
            switch (e.command)
            {
                case "log":
                    Console.Write(e.args);
                    break;
                case "exception":
                    RaiseException(e.args, false);
                    break;
                case "uexception":
                    RaiseException(e.args, true);
                    break;
            }
        }
        #endregion

        #region External Interface
        public bool Call(string externalFunctionName)
        {
            try
            {
                string request = String.Format("<invoke name=\"{0}\" returntype=\"xml\"></invoke>",
                                               externalFunctionName);
                flashPlayer.CallFunction(request);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public bool Call(string externalFunctionName, string arg)
        {
            try
            {
                string request =
                    String.Format(
                        "<invoke name=\"{0}\" returntype=\"xml\"><arguments><string>{1}</string></arguments></invoke>",
                        externalFunctionName, arg);
                flashPlayer.CallFunction(request);
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        private readonly SmallXmlParser _xmlParser = new SmallXmlParser();

        private class Arg
        {
            public Arg(string type)
            {
                Type = type;
            }

            public readonly string Type;
            public string Value;
        }

        private class CallInfo : SmallXmlParser.IContentHandler
        {
            public string MethodName;
            public string ReturnType;
            public readonly List<Arg> Args = new List<Arg>();

            public string Error;

            public bool HasErrors
            {
                get { return _hasErrors; }
            }

            #region IContentHandler Members
            private bool _beginInvoke;
            private bool _beginArguments;
            private bool _hasErrors;
            private Arg _arg;

            private const string tagInvoke = "invoke";
            private const string tagArguments = "arguments";

            public void OnStartParsing(SmallXmlParser parser)
            {
            }

            public void OnEndParsing(SmallXmlParser parser)
            {
            }

            public void OnStartElement(string name, SmallXmlParser.IAttrList attrs)
            {
                if (_beginInvoke)
                {
                    if (_beginArguments)
                    {
                        _arg = new Arg(name);
                        //TODO: Check argument type
                    }
                    else
                    {
                        if (name != tagArguments)
                        {
                            _hasErrors = true;
                            Error = "No arguments";
                            return;
                        }
                        _beginArguments = true;
                    }
                }
                else
                {
                    if (name != tagInvoke)
                    {
                        _hasErrors = true;
                        Error = "Invalid request";
                        return;
                    }
                    _beginInvoke = true;
                    MethodName = attrs.GetValue("name");
                    ReturnType = attrs.GetValue("returntype");
                }
            }

            public void OnEndElement(string name)
            {
                if (_beginInvoke)
                {
                    if (name == tagInvoke)
                    {
                        _beginInvoke = false;
                        return;
                    }
                    if (name == tagArguments)
                    {
                        _beginArguments = false;
                        return;
                    }
                    if (_arg != null)
                    {
                        Args.Add(_arg);
                        _arg = null;
                    }
                }
                else
                {
                    _hasErrors = true;
                    Error = "Invalid request";
                }
            }

            public void OnProcessingInstruction(string name, string text)
            {
            }

            public void OnChars(string text)
            {
                if (_arg != null)
                {
                    _arg.Value = text;
                }
            }

            public void OnIgnorableWhitespace(string text)
            {
            }
            #endregion
        }

        [TestFixture]
        public class FlashCallParserTest
        {
            [Test]
            public void Test()
            {
                var call = new CallInfo();
                var parser = new SmallXmlParser();
                parser.Parse("<invoke name=\"test\" returntype=\"void\"><arguments><string>aaa</string></arguments></invoke>", call);
                Assert.AreEqual(call.MethodName, "test");
                Assert.AreEqual(call.Args[0].Type, "string");
                Assert.AreEqual(call.Args[0].Value, "aaa");
            }
        }

        private string Call(CallInfo call)
        {
            switch (call.MethodName)
            {
                //case "exception":
                //    RaiseException(call.Args[0].Value, false);
                //    break;
                //case "uexception":
                //    RaiseException(call.Args[0].Value, true);
                //    break;
                case "cout":
                case "log":
                    Console.Write(call.Args[0].Value);
                    break;
            }
            return String.Empty;
        }

        private void OnCallFromFlash(object sender, FlashCallEventArgs evt)
        {
            try
            {
                string request = evt.request;

                var call = new CallInfo();
                _xmlParser.Parse(new StringReader(request), call);

                if (call.HasErrors)
                {
                    Console.WriteLine("Flash Call Error: {0}", call.Error);
                    return;
                }

                string returnValue = Call(call);

                flashPlayer.SetReturnValue("<string>" + returnValue + "</string>");
            }
            catch (Exception e)
            {
                Console.WriteLine("Flash Call Unhandled Exception: {0}", e);
            }
        }
        #endregion

        private void MainForm_Resize(object sender, EventArgs e)
        {
            //Size cs = ClientSize;
            //flashPlayer.Width = cs.Width;
            //flashPlayer.Height = cs.Height;
        }
    }
}