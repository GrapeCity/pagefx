using System;
using System.Runtime.CompilerServices;

namespace flash.utils
{
    /// <summary>
    /// The Timer class is the interface to timers, which let you
    /// run code on a specified time sequence. Use the start()  method to start a timer.
    /// Add an event listener for the timer  event to set up code to be run on the timer interval.
    /// </summary>
    [PageFX.AbcInstance(78)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class Timer : flash.events.EventDispatcher
    {
        /// <summary>
        /// The delay, in milliseconds, between timer
        /// events. If you set the delay interval while
        /// the timer is running, the timer will restart
        /// at the same repeatCount iteration.
        /// </summary>
        public extern virtual double delay
        {
            [PageFX.AbcInstanceTrait(3)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The total number of times the timer is set to run.
        /// If the repeat count is set to 0, the timer continues forever
        /// or until the stop() method is invoked or the program stops.
        /// If the repeat count is nonzero, the timer runs the specified number of times.
        /// If repeatCount is set to a total that is the same or less then currentCount
        /// the timer stops and will not fire again.
        /// </summary>
        public extern virtual int repeatCount
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// The total number of times the timer has fired since it started
        /// at zero. If the timer has been reset, only the fires since
        /// the reset are counted.
        /// </summary>
        public extern virtual int currentCount
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        /// <summary>The timer&apos;s current state; true if the timer is running, otherwise false.</summary>
        public extern virtual bool running
        {
            [PageFX.AbcInstanceTrait(7)]
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
        public extern Timer(double delay, int repeatCount);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Timer(double delay);

        /// <summary>Starts the timer, if it is not already running.</summary>
        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void start();

        /// <summary>
        /// Stops the timer, if it is running, and sets the currentCount property back to 0,
        /// like the reset button of a stopwatch. Then, when start() is called,
        /// the timer instance runs for the specified number of repetitions,
        /// as set by the repeatCount value.
        /// </summary>
        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void reset();

        /// <summary>
        /// Stops the timer. When start() is called after stop(), the timer
        /// instance runs for the remaining number of repetitions, as set by the repeatCount property.
        /// </summary>
        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void stop();
    }
}
