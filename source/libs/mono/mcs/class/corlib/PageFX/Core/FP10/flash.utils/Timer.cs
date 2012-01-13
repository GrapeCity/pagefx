using System;
using System.Runtime.CompilerServices;

namespace flash.utils
{
    /// <summary>
    /// The Timer class is the interface to timers, which let you
    /// run code on a specified time sequence. Use the start()  method to start a timer.
    /// Add an event listener for the timer  event to set up code to be run on the timer interval.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class Timer : flash.events.EventDispatcher
    {
        public extern virtual double delay
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int repeatCount
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int currentCount
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual bool running
        {
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [PageFX.Event("timerComplete")]
        public event flash.events.TimerEventHandler timerComplete
        {
            add { }
            remove { }
        }

        [PageFX.Event("timer")]
        public event flash.events.TimerEventHandler timer
        {
            add { }
            remove { }
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Timer(double arg0, int arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Timer(double arg0);

        /// <summary>
        /// Stops the timer, if it is running, and sets the currentCount property back to 0,
        /// like the reset button of a stopwatch. Then, when start() is called,
        /// the timer instance runs for the specified number of repetitions,
        /// as set by the repeatCount value.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void reset();

        /// <summary>Starts the timer, if it is not already running.</summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void start();

        /// <summary>
        /// Stops the timer. When start() is called after stop(), the timer
        /// instance runs for the remaining number of repetitions, as set by the repeatCount property.
        /// </summary>
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void stop();


    }
}
