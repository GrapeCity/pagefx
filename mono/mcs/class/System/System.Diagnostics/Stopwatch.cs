//
// System.Diagnostics.Stopwatch.cs
//
// Authors:
//   Zoltan Varga (vargaz@gmail.com)
//   Atsushi Enomoto  <atsushi@ximian.com>
//
// (C) 2006 Novell, Inc.
//

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

#if NET_2_0

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace System.Diagnostics
{
	public class Stopwatch
	{
		[DllImport ("kernel32.dll")]
		static extern bool QueryPerformanceCounter (out long performance_count);

		[DllImport ("kernel32.dll")]
		static extern bool QueryPerformanceFrequency (out long frequency);

		public static readonly long Frequency;

		public static readonly bool IsHighResolution;

		public static long GetTimestamp ()
		{
			if (IsHighResolution) {
				long performance_count;
				QueryPerformanceCounter (out performance_count);
				return performance_count;
			}
			else
				return DateTime.Now.Ticks;
		}

		public static Stopwatch StartNew ()
		{
			Stopwatch s = new Stopwatch ();
			s.Start ();
			return s;
		}

		static Stopwatch ()
		{
			Frequency = TimeSpan.TicksPerSecond;
			IsHighResolution = false;
			int platform = (int) Environment.OSVersion.Platform;
			if ((platform != 4) && (platform != 128)) {
				// try to use high performance timer on Windows.
				IsHighResolution = QueryPerformanceFrequency (out Frequency);
			}
		}

		public Stopwatch ()
		{
		}

		long elapsed;
		long started;
		bool is_running;

		public TimeSpan Elapsed {
			get {
				if (IsHighResolution) {
					// convert our ticks to TimeSpace ticks, 100 nano second units
					return TimeSpan.FromTicks ((long)(TimeSpan.TicksPerSecond * ElapsedTicks / Frequency));
				}
				else {
					return TimeSpan.FromTicks (ElapsedTicks); 
				}
			}
		}

		public long ElapsedMilliseconds {
			get { 
				checked {
					if (IsHighResolution) {
						return (long)(1000 * ElapsedTicks / Frequency);
					}
					else {
						return (long) Elapsed.TotalMilliseconds;
					}
				} 
			}
		}

		public long ElapsedTicks {
			get { return is_running ? GetTimestamp () - started + elapsed : elapsed; }
		}

		public bool IsRunning {
			get { return is_running; }
		}

		public void Reset ()
		{
			elapsed = 0;
			is_running = false;
		}

		public void Start ()
		{
			if (is_running)
				return;
			started = GetTimestamp ();
			is_running = true;
		}

		public void Stop ()
		{
			if (!is_running)
				return;
			elapsed += GetTimestamp () - started;
			is_running = false;
		}
	}
}

#endif
