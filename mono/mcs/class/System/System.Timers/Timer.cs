//
// System.Timers.Timer
//
// Authors:
//	Gonzalo Paniagua Javier (gonzalo@ximian.com)
//
// (C) 2002 Ximian, Inc (http://www.ximian.com)
// Copyright (C) 2005 Novell, Inc (http://www.novell.com)
//
// The docs talk about server timers and such...

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

using System.ComponentModel;
#if AVM
using flash.events;
using AvmTimer = flash.utils.Timer;
#else
using System.Threading;
#endif

namespace System.Timers
{
#if NOT_PFX
    [DefaultEventAttribute("Elapsed")]
    [DefaultProperty("Interval")]
#endif
    public class Timer
#if NOT_PFX
        : Component, ISupportInitialize
#endif
    {
        bool autoReset;
        bool enabled;
#if !AVM
        bool exiting;
#endif
        double interval;

#if AVM
        private readonly AvmTimer impl;
#else
        ISynchronizeInvoke so;
        ManualResetEvent wait;
        WeakReference weak_thread;
        readonly object locker = new object();
#endif

        [Category("Behavior")]
        [TimersDescription("Occurs when the Interval has elapsed.")]
        public event ElapsedEventHandler Elapsed;

        public Timer()
            : this(100)
        {
        }

        public Timer(double interval)
        {
#if NET_2_0
			// MSBUG: https://connect.microsoft.com/VisualStudio/feedback/ViewFeedback.aspx?FeedbackID=296761
			if (interval > 0x7FFFFFFF)
				throw new ArgumentException ("Invalid value: " + interval, "interval");
#endif

            autoReset = true;
#if AVM
            impl = new AvmTimer(interval);
            impl.timer += OnAvmTimer;
#else
            Interval = interval;
#endif
        }

#if AVM
        private void OnAvmTimer(TimerEvent e)
        {
            if (Elapsed != null)
            {
                Elapsed(null, new ElapsedEventArgs(DateTime.Now));
            }
        }
#endif

        [Category("Behavior")]
#if NOT_PFX
        [DefaultValue(true)]
#endif
        [TimersDescription("Indicates whether the timer will be restarted when it is enabled.")]
        public bool AutoReset
        {
            get { return autoReset; }
            set { autoReset = value; }
        }

        [Category("Behavior")]
#if NOT_PFX
        [DefaultValue(false)]
#endif
        [TimersDescription("Indicates whether the timer is enabled to fire events at a defined interval.")]
        public bool Enabled
        {
            get
            {
#if AVM
                return enabled;
#else
                return enabled && !exiting;
#endif
            }
            set
            {
                if (Enabled == value)
                    return;

                enabled = value;
                if (value)
                {
#if AVM
                    StartTimer();
#else
                    if (exiting)
                        StopTimer();
                    exiting = false;
                    wait = new ManualResetEvent(false);
                    Thread thread = new Thread(new ThreadStart(StartTimer));
                    weak_thread = new WeakReference(thread);

                    thread.IsBackground = true;
                    thread.Start();
#endif
                }
                else
                {
                    StopTimer();
                }
            }
        }

        [Category("Behavior")]
#if NOT_PFX        
        [DefaultValue(100)]
        [RecommendedAsConfigurable(true)]
#endif
        [TimersDescription("The number of milliseconds between timer events.")]
        public double Interval
        {
            get { return interval; }
            set
            {
                // The doc says 'less than 0', but 0 also throws the exception
                if (value <= 0)
                    throw new ArgumentException("Invalid value: " + interval, "interval");

                interval = value;
#if AVM
                impl.delay = value;
#endif
            }
        }

#if NOT_PFX
        public override ISite Site
        {
            get { return base.Site; }
            set { base.Site = value; }
        }
#endif

#if !AVM
        [DefaultValue(null)]
        [TimersDescriptionAttribute("The object used to marshal the event handler calls issued " +
                        "when an interval has elapsed.")]
#if NET_2_0
		[Browsable (false)]
#endif
        public ISynchronizeInvoke SynchronizingObject
        {
            get { return so; }
            set { so = value; }
        }
#endif

        public void BeginInit()
        {
            // Nothing to do
        }

        public void Close()
        {
            Enabled = false;
        }

        public void EndInit()
        {
            // Nothing to do
        }

        public void Start()
        {
            Enabled = true;
        }

        public void Stop()
        {
            Enabled = false;
        }

#if NOT_PFX
        protected override void Dispose(bool disposing)
        {
            Close();
            base.Dispose(disposing);
        }
#endif

#if !AVM
        static void Callback(object state)
        {
            Timer timer = (Timer)state;
            if (timer.Elapsed == null)
                return;

            ElapsedEventArgs arg = new ElapsedEventArgs(DateTime.Now);

            if (timer.so != null && timer.so.InvokeRequired)
            {
                timer.so.BeginInvoke(timer.Elapsed, new object[2] { timer, arg });
            }
            else
            {
                timer.Elapsed(timer, arg);
            }
        }
#endif

        void StartTimer()
        {
#if AVM
            impl.start();
#else
            WaitCallback wc = new WaitCallback(Callback);

            while (wait.WaitOne((int)interval, false) == false)
            {
                exiting = !autoReset;

                ThreadPool.QueueUserWorkItem(wc, this);

                if (exiting)
                    break;
            }

            lock (locker)
            {
                wait.Close();
                wait = null;
            }
#endif
        }

        void StopTimer()
        {
#if AVM
            impl.stop();
#else
            lock (locker)
            {
                if (wait != null)
                    wait.Set();
            }

            // the sleep speeds up the join under linux
            Thread.Sleep(0);

            Thread thread = (Thread)weak_thread.Target;

            if (thread != null)
                thread.Join();
#endif
        }
    }
}
