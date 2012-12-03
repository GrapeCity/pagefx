using System;
using System.UI;
using flash.events;
using flash.net;
using Flex.Utils;
using mx.core;
using mx.controls;
using mx.containers;

namespace URLStreamTest
{
    public class App : Application
    {
        private TextInput urlInput;
        private TabNavigator tabs;

        private class Download : URLStream
        {
            public TextArea text;
        }

        public App()
        {
            Init();
        }

        private void Init()
        {
            HBox locbar = new HBox();
            locbar.y = 2;
            Style.SetConstraints(locbar, Constraint.LR, 3);

            Label l = new Label();
            l.text = "URL:";
            locbar.addChild(l);

            urlInput = new TextInput();
            urlInput.percentWidth = 100;
            urlInput.keyDown += urlInput_keyDown;
            locbar.addChild(urlInput);

            addChild(locbar);

            tabs = new TabNavigator();
            Style.SetConstraints(tabs, Constraint.Top, 28);
            Style.SetConstraints(tabs, Constraint.LRB, 3);
            addChild(tabs);
        }

        void urlInput_keyDown(KeyboardEvent e)
        {
            KeyCode k = (KeyCode)e.keyCode;
            if (k == KeyCode.Enter)
            {
                string URL = urlInput.text;
                Download d = new Download();
                try
                {
                    d.complete += stream_complete;
                    d.ioError += stream_ioError;
                    d.securityError += stream_securityError;
                    d.progress += stream_progress;
                    d.httpStatus += stream_httpStatus;

                    TextArea text = new TextArea();
                    text.percentHeight = 100;
                    text.percentWidth = 100;
                    d.text = text;

                    Panel panel = new Panel();
                    panel.percentHeight = 100;
                    panel.percentWidth = 100;
                    panel.label = URL;
                    panel.addChild(text);
                    tabs.addChild(panel);

                    d.load(new URLRequest(URL));
                }
                catch (Exception)
                {
                    Alert.show("Unable to load given URL: " + URL);
                }
            }
        }

        private static void Complete(Download d)
        {
            Read(d);
        }

        private static void Read(Download d)
        {
            if (d == null) return;
            int n = (int)d.bytesAvailable;
            string s = d.text.text;
            for (int i = 0; i < n; ++i)
            {
                int b = d.readByte();
                //s += b.ToString("X2");
                s += (char)b;
            }
            d.text.text = s;
        }

        private static void Error(Download d)
        {
            if (d != null)
                d.text.parent.removeChild(d.text);
        }

        static void stream_progress(ProgressEvent e)
        {
            Download d = e.target as Download;
            if (d != null)
                Read(d);
        }

        static void stream_complete(Event e)
        {
            Download d = e.target as Download;
            if (d != null)
                Complete(d);
        }

        static void stream_ioError(IOErrorEvent e)
        {
            Download d = e.target as Download;
            if (d != null)
                Error(d);
            Alert.show("IOError: " + e.text);
        }

        static void stream_securityError(SecurityErrorEvent e)
        {
            if (e != null)
            {
                Download d = e.target as Download;
                if (d != null)
                    Error(d);
                Alert.show("SecurityError: " + e.text);
            }
            else
            {
                Alert.show("SecurityError");
            }
        }

        static void stream_httpStatus(HTTPStatusEvent e)
        {
            //Alert.show("HTTPStatus:" + e.status);
        }
    }
}
