using System.Runtime.CompilerServices;
using PageFX;

namespace Native
{
	[Native]
	[QName("Date")]
	internal sealed class Date
	{
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Date();

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Date(int year, int month, int day);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Date(int year, int month, int day, int hour, int minute, int second);

		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern Date(int year, int month, int day, int hour, int minute, int second, int millisecond);

		public extern int fullYear
		{
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int month
		{
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int date
		{
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		[InlineFunction]
		[MethodImpl(MethodImplOptions.InternalCall)]
		public extern double setDate(int day);

		public extern int hours
		{
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int minutes
		{
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int seconds
		{
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int milliseconds
		{
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int fullYearUTC
		{
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int monthUTC
		{
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int dateUTC
		{
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int hoursUTC
		{
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int minutesUTC
		{
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int secondsUTC
		{
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int millisecondsUTC
		{
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			set;
		}

		public extern int timezoneOffset
		{
			[InlineProperty]
			[MethodImpl(MethodImplOptions.InternalCall)]
			get;
		}
	}
}