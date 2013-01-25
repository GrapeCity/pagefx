using System;
using System.Runtime.CompilerServices;

namespace flash.globalization
{
    [PageFX.AbcInstance(66)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class DateTimeFormatter : Avm.Object
    {
        public extern virtual Avm.String lastOperationStatus
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String requestedLocaleIDName
        {
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern virtual Avm.String actualLocaleIDName
        {
            [PageFX.AbcInstanceTrait(6)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DateTimeFormatter(Avm.String requestedLocaleIDName, Avm.String dateStyle, Avm.String timeStyle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DateTimeFormatter(Avm.String requestedLocaleIDName, Avm.String dateStyle);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern DateTimeFormatter(Avm.String requestedLocaleIDName);

        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setDateTimeStyles(Avm.String dateStyle, Avm.String timeStyle);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String getTimeStyle();

        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String getDateStyle();

        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String format(Avm.Date dateTime);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String formatUTC(Avm.Date dateTime);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Vector<Avm.String> getMonthNames(Avm.String nameStyle, Avm.String context);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Vector<Avm.String> getMonthNames(Avm.String nameStyle);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Vector<Avm.String> getMonthNames();

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Vector<Avm.String> getWeekdayNames(Avm.String nameStyle, Avm.String context);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Vector<Avm.String> getWeekdayNames(Avm.String nameStyle);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Vector<Avm.String> getWeekdayNames();

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int getFirstWeekday();

        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String getDateTimePattern();

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setDateTimePattern(Avm.String pattern);

        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.Vector<Avm.String> getAvailableLocaleIDNames();
    }
}
