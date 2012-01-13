using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using Interop.Flash;

namespace DataDynamics.PageFX
{
    [Clsid("{d27cdb6e-ae6d-11cf-96b8-444553540000}"), DesignTimeVisible(true)]
    public class FlashControl : AxHost
    {
        #region Fields
        private ConnectionPointCookie cookie;
        private AxShockwaveFlashEventMulticaster eventMulticaster;
        private IFlashControl ocx;
        #endregion

        #region Events
        public event FlashCallEventHandler FlashCall;
        public event FlashCommandEventHandler FSCommand;
        public event OnProgressEventHandler OnProgress;
        public event OnReadyStateChangeEventHandler OnReadyStateChange;
        #endregion

        #region Properties
        [DispId(0x79), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int AlignMode
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("AlignMode", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.AlignMode;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("AlignMode", ActiveXInvokeKind.PropertySet);
                }
                ocx.AlignMode = value;
            }
        }

        [DispId(0xab), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string AllowScriptAccess
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("AllowScriptAccess", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.AllowScriptAccess;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("AllowScriptAccess", ActiveXInvokeKind.PropertySet);
                }
                ocx.AllowScriptAccess = value;
            }
        }

        [DispId(0x7b), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int BackgroundColor
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("BackgroundColor", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.BackgroundColor;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("BackgroundColor", ActiveXInvokeKind.PropertySet);
                }
                ocx.BackgroundColor = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DispId(0x88)]
        public virtual string Base
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("Base", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.Base;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("Base", ActiveXInvokeKind.PropertySet);
                }
                ocx.Base = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DispId(140)]
        public virtual string BGColor
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("BGColor", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.BGColor;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("BGColor", ActiveXInvokeKind.PropertySet);
                }
                ocx.BGColor = value;
            }
        }

        [DispId(0x89), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string CtlScale
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("CtlScale", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.Scale;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("CtlScale", ActiveXInvokeKind.PropertySet);
                }
                ocx.Scale = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DispId(0x8a)]
        public virtual bool DeviceFont
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("DeviceFont", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.DeviceFont;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("DeviceFont", ActiveXInvokeKind.PropertySet);
                }
                ocx.DeviceFont = value;
            }
        }

        [DispId(0x8b), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool EmbedMovie
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("EmbedMovie", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.EmbedMovie;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("EmbedMovie", ActiveXInvokeKind.PropertySet);
                }
                ocx.EmbedMovie = value;
            }
        }

        [DispId(170), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string FlashVars
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("FlashVars", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.FlashVars;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("FlashVars", ActiveXInvokeKind.PropertySet);
                }
                ocx.FlashVars = value;
            }
        }

        [DispId(0x6b), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int FrameNum
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("FrameNum", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.FrameNum;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("FrameNum", ActiveXInvokeKind.PropertySet);
                }
                ocx.FrameNum = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DispId(0xbf)]
        public virtual object InlineData
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("InlineData", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.InlineData;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("InlineData", ActiveXInvokeKind.PropertySet);
                }
                ocx.InlineData = value;
            }
        }

        [DispId(0x6a), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool Loop
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("Loop", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.Loop;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("Loop", ActiveXInvokeKind.PropertySet);
                }
                ocx.Loop = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DispId(0x87)]
        public virtual bool Menu
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("Menu", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.Menu;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("Menu", ActiveXInvokeKind.PropertySet);
                }
                ocx.Menu = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DispId(0x66)]
        public virtual string Movie
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("Movie", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.Movie;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("Movie", ActiveXInvokeKind.PropertySet);
                }
                ocx.Movie = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DispId(190)]
        public virtual string MovieData
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("MovieData", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.MovieData;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("MovieData", ActiveXInvokeKind.PropertySet);
                }
                ocx.MovieData = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DispId(0x7d)]
        public virtual bool Playing
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("Playing", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.Playing;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("Playing", ActiveXInvokeKind.PropertySet);
                }
                ocx.Playing = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DispId(0x69)]
        public virtual int Quality
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("Quality", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.Quality;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("Quality", ActiveXInvokeKind.PropertySet);
                }
                ocx.Quality = value;
            }
        }

        [DispId(0x8d), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string Quality2
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("Quality2", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.Quality2;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("Quality2", ActiveXInvokeKind.PropertySet);
                }
                ocx.Quality2 = value;
            }
        }

        [DispId(-525), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int ReadyState
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("ReadyState", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.ReadyState;
            }
        }

        [DispId(0x86), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string SAlign
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("SAlign", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.SAlign;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("SAlign", ActiveXInvokeKind.PropertySet);
                }
                ocx.SAlign = value;
            }
        }

        [DispId(120), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual int ScaleMode
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("ScaleMode", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.ScaleMode;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("ScaleMode", ActiveXInvokeKind.PropertySet);
                }
                ocx.ScaleMode = value;
            }
        }

        [DispId(0xc0), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual bool SeamlessTabbing
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("SeamlessTabbing", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.SeamlessTabbing;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("SeamlessTabbing", ActiveXInvokeKind.PropertySet);
                }
                ocx.SeamlessTabbing = value;
            }
        }

        [DispId(0x9f), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public virtual string SWRemote
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("SWRemote", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.SWRemote;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("SWRemote", ActiveXInvokeKind.PropertySet);
                }
                ocx.SWRemote = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DispId(0x7c)]
        public virtual int TotalFrames
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("TotalFrames", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.TotalFrames;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), DispId(0x85)]
        public virtual string WMode
        {
            get
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("WMode", ActiveXInvokeKind.PropertyGet);
                }
                return ocx.WMode;
            }
            set
            {
                if (ocx == null)
                {
                    throw new InvalidActiveXStateException("WMode", ActiveXInvokeKind.PropertySet);
                }
                ocx.WMode = value;
            }
        }
        #endregion

        #region Methods
        public FlashControl() : base("d27cdb6e-ae6d-11cf-96b8-444553540000")
        {
        }

        protected override void AttachInterfaces()
        {
            try
            {
                ocx = (IFlashControl)base.GetOcx();
            }
            catch (Exception)
            {
            }
        }

        public virtual void Back()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("Back", ActiveXInvokeKind.MethodInvoke);
            }
            ocx.Back();
        }

        protected override void CreateSink()
        {
            try
            {
                eventMulticaster = new AxShockwaveFlashEventMulticaster(this);
                cookie = new ConnectionPointCookie(ocx, eventMulticaster, typeof(IFlashEvents));
            }
            catch (Exception)
            {
            }
        }

        public virtual int CurrentFrame()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("CurrentFrame", ActiveXInvokeKind.MethodInvoke);
            }
            return ocx.CurrentFrame();
        }

        protected override void DetachSink()
        {
            try
            {
                cookie.Disconnect();
            }
            catch (Exception)
            {
            }
        }

        public virtual int FlashVersion()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("FlashVersion", ActiveXInvokeKind.MethodInvoke);
            }
            return ocx.FlashVersion();
        }

        public virtual void Forward()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("Forward", ActiveXInvokeKind.MethodInvoke);
            }
            ocx.Forward();
        }

        public virtual bool FrameLoaded(int frameNum)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("FrameLoaded", ActiveXInvokeKind.MethodInvoke);
            }
            return ocx.FrameLoaded(frameNum);
        }

        public virtual string GetVariable(string name)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GetVariable", ActiveXInvokeKind.MethodInvoke);
            }
            return ocx.GetVariable(name);
        }

        public virtual void GotoFrame(int frameNum)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("GotoFrame", ActiveXInvokeKind.MethodInvoke);
            }
            ocx.GotoFrame(frameNum);
        }

        public virtual bool IsPlaying()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("IsPlaying", ActiveXInvokeKind.MethodInvoke);
            }
            return ocx.IsPlaying();
        }

        public virtual void LoadMovie(int layer, string url)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("LoadMovie", ActiveXInvokeKind.MethodInvoke);
            }
            url = Path.GetFullPath(url);
            ocx.LoadMovie(layer, url);
        }

        public virtual void Pan(int x, int y, int mode)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("Pan", ActiveXInvokeKind.MethodInvoke);
            }
            ocx.Pan(x, y, mode);
        }

        public virtual int PercentLoaded()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("PercentLoaded", ActiveXInvokeKind.MethodInvoke);
            }
            return ocx.PercentLoaded();
        }

        public virtual void Play()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("Play", ActiveXInvokeKind.MethodInvoke);
            }
            ocx.Play();
        }

        internal void RaiseOnFSCommand(object sender, FlashCommandEventArgs e)
        {
            if (FSCommand != null)
            {
                FSCommand(sender, e);
            }
        }

        internal void RaiseFlashCall(object sender, FlashCallEventArgs e)
        {
            if (FlashCall != null)
            {
                FlashCall(sender, e);
            }
        }

        internal void RaiseOnOnProgress(object sender, OnProgressEventArgs e)
        {
            if (OnProgress != null)
            {
                OnProgress(sender, e);
            }
        }

        internal void RaiseOnOnReadyStateChange(object sender, OnReadyStateChangeEventArgs e)
        {
            if (OnReadyStateChange != null)
            {
                OnReadyStateChange(sender, e);
            }
        }

        public virtual void Rewind()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("Rewind", ActiveXInvokeKind.MethodInvoke);
            }
            ocx.Rewind();
        }

        public virtual void SetVariable(string name, string value)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("SetVariable", ActiveXInvokeKind.MethodInvoke);
            }
            ocx.SetVariable(name, value);
        }

        public virtual void SetZoomRect(int left, int top, int right, int bottom)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("SetZoomRect", ActiveXInvokeKind.MethodInvoke);
            }
            ocx.SetZoomRect(left, top, right, bottom);
        }

        public virtual void Stop()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("Stop", ActiveXInvokeKind.MethodInvoke);
            }
            ocx.Stop();
        }

        public virtual void StopPlay()
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("StopPlay", ActiveXInvokeKind.MethodInvoke);
            }
            ocx.StopPlay();
        }

        public virtual void TCallFrame(string target, int frameNum)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("TCallFrame", ActiveXInvokeKind.MethodInvoke);
            }
            ocx.TCallFrame(target, frameNum);
        }

        public virtual void TCallLabel(string target, string label)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("TCallLabel", ActiveXInvokeKind.MethodInvoke);
            }
            ocx.TCallLabel(target, label);
        }

        public virtual int TCurrentFrame(string target)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("TCurrentFrame", ActiveXInvokeKind.MethodInvoke);
            }
            return ocx.TCurrentFrame(target);
        }

        public virtual string TCurrentLabel(string target)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("TCurrentLabel", ActiveXInvokeKind.MethodInvoke);
            }
            return ocx.TCurrentLabel(target);
        }

        public virtual string TGetProperty(string target, int property)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("TGetProperty", ActiveXInvokeKind.MethodInvoke);
            }
            return ocx.TGetProperty(target, property);
        }

        public virtual double TGetPropertyAsNumber(string target, int property)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("TGetPropertyAsNumber", ActiveXInvokeKind.MethodInvoke);
            }
            return ocx.TGetPropertyAsNumber(target, property);
        }

        public virtual double TGetPropertyNum(string target, int property)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("TGetPropertyNum", ActiveXInvokeKind.MethodInvoke);
            }
            return ocx.TGetPropertyNum(target, property);
        }

        public virtual void TGotoFrame(string target, int frameNum)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("TGotoFrame", ActiveXInvokeKind.MethodInvoke);
            }
            ocx.TGotoFrame(target, frameNum);
        }

        public virtual void TGotoLabel(string target, string label)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("TGotoLabel", ActiveXInvokeKind.MethodInvoke);
            }
            ocx.TGotoLabel(target, label);
        }

        public virtual void TPlay(string target)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("TPlay", ActiveXInvokeKind.MethodInvoke);
            }
            ocx.TPlay(target);
        }

        public virtual void TSetProperty(string target, int property, string value)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("TSetProperty", ActiveXInvokeKind.MethodInvoke);
            }
            ocx.TSetProperty(target, property, value);
        }

        public virtual void TSetPropertyNum(string target, int property, double value)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("TSetPropertyNum", ActiveXInvokeKind.MethodInvoke);
            }
            ocx.TSetPropertyNum(target, property, value);
        }

        public virtual void TStopPlay(string target)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("TStopPlay", ActiveXInvokeKind.MethodInvoke);
            }
            ocx.TStopPlay(target);
        }

        public virtual void Zoom(int factor)
        {
            if (ocx == null)
            {
                throw new InvalidActiveXStateException("Zoom", ActiveXInvokeKind.MethodInvoke);
            }
            ocx.Zoom(factor);
        }

        // Flash8 :
        public virtual void EnforceLocalSecurity()
        {
            if (ocx == null)
                throw new InvalidActiveXStateException("EnforceLocalSecurity", ActiveXInvokeKind.MethodInvoke);
            ocx.EnforceLocalSecurity();
        }

        public bool Profile
        {
            get
            {
                if (ocx == null)
                    throw new InvalidActiveXStateException("Profile", ActiveXInvokeKind.MethodInvoke);
                return ocx.Profile;
            }
            set
            {
                if (ocx == null)
                    throw new InvalidActiveXStateException("Profile", ActiveXInvokeKind.MethodInvoke);
                ocx.Profile = value;
            }
        }

        public string ProfileAddress
        {
            get
            {
                if (ocx == null)
                    throw new InvalidActiveXStateException("ProfileAddress", ActiveXInvokeKind.MethodInvoke);
                return ocx.ProfileAddress;
            }
            set
            {
                if (ocx == null)
                    throw new InvalidActiveXStateException("ProfileAddress", ActiveXInvokeKind.MethodInvoke);
                ocx.ProfileAddress = value;
            }
        }

        public int ProfilePort
        {
            get
            {
                if (ocx == null)
                    throw new InvalidActiveXStateException("ProfilePort", ActiveXInvokeKind.MethodInvoke);
                return ocx.ProfilePort;
            }
            set
            {
                if (ocx == null)
                    throw new InvalidActiveXStateException("ProfilePort", ActiveXInvokeKind.MethodInvoke);
                ocx.ProfilePort = value;
            }
        }

        public virtual string CallFunction(string request)
        {
            if (ocx == null)
                throw new InvalidActiveXStateException("CallFunction", ActiveXInvokeKind.MethodInvoke);
            return ocx.CallFunction(request);
        }

        public virtual void SetReturnValue(string returnValue)
        {
            if (ocx == null)
                throw new InvalidActiveXStateException("SetReturnValue", ActiveXInvokeKind.MethodInvoke);
            ocx.SetReturnValue(returnValue);
        }

        public virtual void DisableLocalSecurity()
        {
            if (ocx == null)
                throw new InvalidActiveXStateException("DisableLocalSecurity", ActiveXInvokeKind.MethodInvoke);
            ocx.DisableLocalSecurity();
        }
        #endregion
    }

    public class AxShockwaveFlashEventMulticaster : IFlashEvents
    {
        #region Fields
        private readonly FlashControl parent;
        #endregion

        #region Methods
        public AxShockwaveFlashEventMulticaster(FlashControl parent)
        {
            this.parent = parent;
        }

        public virtual void FSCommand(string command, string args)
        {
            var event1 = new FlashCommandEventArgs(command, args);
            parent.RaiseOnFSCommand(parent, event1);
        }

        public virtual void FlashCall(string request)
        {
            var event1 = new FlashCallEventArgs(request);
            parent.RaiseFlashCall(parent, event1);
        }

        public virtual void OnProgress(int percentDone)
        {
            var event1 = new OnProgressEventArgs(percentDone);
            parent.RaiseOnOnProgress(parent, event1);
        }

        public virtual void OnReadyStateChange(int newState)
        {
            var event1 = new OnReadyStateChangeEventArgs(newState);
            parent.RaiseOnOnReadyStateChange(parent, event1);
        }
        #endregion
    }

    public delegate void FlashCommandEventHandler(object sender, FlashCommandEventArgs e);

    public class FlashCommandEventArgs
    {
        #region Fields
        public string args;
        public string command;
        #endregion

        #region Methods
        public FlashCommandEventArgs(string command, string args)
        {
            this.command = command;
            this.args = args;
        }
        #endregion
    }

    public delegate void FlashCallEventHandler(object sender, FlashCallEventArgs e);

    public class FlashCallEventArgs
    {
        #region Fields
        public string request;
        #endregion

        #region Methods
        public FlashCallEventArgs(string request)
        {
            this.request = request;
        }
        #endregion
    }

    public delegate void OnProgressEventHandler(object sender, OnProgressEventArgs e);

    public class OnProgressEventArgs
    {
        // Fields
        public int percentDone;

        // Methods
        public OnProgressEventArgs(int percentDone)
        {
            this.percentDone = percentDone;
        }
    }


    public delegate void OnReadyStateChangeEventHandler(object sender, OnReadyStateChangeEventArgs e);

    public class OnReadyStateChangeEventArgs
    {
        // Fields
        public int newState;

        // Methods
        public OnReadyStateChangeEventArgs(int newState)
        {
            this.newState = newState;
        }
    }
}