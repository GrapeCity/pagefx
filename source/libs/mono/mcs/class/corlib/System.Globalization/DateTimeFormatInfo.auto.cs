//WARNING: This file is auto generated

namespace System.Globalization
{
	public partial class DateTimeFormatInfo
	{
		private void ConstructFromLcid(int lcid)
		{
			switch (lcid)
			{
				case 127:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "MM/dd/yyyy";
						longDatePattern = "dddd, dd MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "yyyy MMMM";
						fullDateTimePattern = "dddd, dd MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
						monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						return;
					}

				case 1025:
					{
						amDesignator = "\u0635";
						pmDesignator = "\u0645";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yy";
						longDatePattern = "dd/MMMM/yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd/MMMM/yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 6;
						abbreviatedDayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						dayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						monthNames = new string[] { "\u0645\u062D\u0631\u0645", "\u0635\u0641\u0631", "\u0631\u0628\u064A\u0639\xa0\u0627\u0644\u0623\u0648\u0644", "\u0631\u0628\u064A\u0639\xa0\u0627\u0644\u062B\u0627\u0646\u064A", "\u062C\u0645\u0627\u062F\u0649\xa0\u0627\u0644\u0623\u0648\u0644\u0649", "\u062C\u0645\u0627\u062F\u0649\xa0\u0627\u0644\u062B\u0627\u0646\u064A\u0629", "\u0631\u062C\u0628", "\u0634\u0639\u0628\u0627\u0646", "\u0631\u0645\u0636\u0627\u0646", "\u0634\u0648\u0627\u0644", "\u0630\u0648\xa0\u0627\u0644\u0642\u0639\u062F\u0629", "\u0630\u0648\xa0\u0627\u0644\u062D\u062C\u0629", "" };
						abbreviatedMonthNames = new string[] { "\u0645\u062D\u0631\u0645", "\u0635\u0641\u0631", "\u0631\u0628\u064A\u0639\xa0\u0627\u0644\u0627\u0648\u0644", "\u0631\u0628\u064A\u0639\xa0\u0627\u0644\u062B\u0627\u0646\u064A", "\u062C\u0645\u0627\u062F\u0649\xa0\u0627\u0644\u0627\u0648\u0644\u0649", "\u062C\u0645\u0627\u062F\u0649\xa0\u0627\u0644\u062B\u0627\u0646\u064A\u0629", "\u0631\u062C\u0628", "\u0634\u0639\u0628\u0627\u0646", "\u0631\u0645\u0636\u0627\u0646", "\u0634\u0648\u0627\u0644", "\u0630\u0648\xa0\u0627\u0644\u0642\u0639\u062F\u0629", "\u0630\u0648\xa0\u0627\u0644\u062D\u062C\u0629", "" };
						calendar = new HijriCalendar();
						firstDayOfWeek = (int)DayOfWeek.Saturday;
						return;
					}

				case 1026:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.M.yyyy '\u0433.'";
						longDatePattern = "dd MMMM yyyy '\u0433.'";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy '\u0433.'";
						fullDateTimePattern = "dd MMMM yyyy '\u0433.' HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u041D\u0434", "\u041F\u043D", "\u0412\u0442", "\u0421\u0440", "\u0427\u0442", "\u041F\u0442", "\u0421\u0431" };
						dayNames = new string[] { "\u043D\u0435\u0434\u0435\u043B\u044F", "\u043F\u043E\u043D\u0435\u0434\u0435\u043B\u043D\u0438\u043A", "\u0432\u0442\u043E\u0440\u043D\u0438\u043A", "\u0441\u0440\u044F\u0434\u0430", "\u0447\u0435\u0442\u0432\u044A\u0440\u0442\u044A\u043A", "\u043F\u0435\u0442\u044A\u043A", "\u0441\u044A\u0431\u043E\u0442\u0430" };
						monthNames = new string[] { "\u042F\u043D\u0443\u0430\u0440\u0438", "\u0424\u0435\u0432\u0440\u0443\u0430\u0440\u0438", "\u041C\u0430\u0440\u0442", "\u0410\u043F\u0440\u0438\u043B", "\u041C\u0430\u0439", "\u042E\u043D\u0438", "\u042E\u043B\u0438", "\u0410\u0432\u0433\u0443\u0441\u0442", "\u0421\u0435\u043F\u0442\u0435\u043C\u0432\u0440\u0438", "\u041E\u043A\u0442\u043E\u043C\u0432\u0440\u0438", "\u041D\u043E\u0435\u043C\u0432\u0440\u0438", "\u0414\u0435\u043A\u0435\u043C\u0432\u0440\u0438", "" };
						abbreviatedMonthNames = new string[] { "\u042F\u043D\u0443\u0430\u0440\u0438", "\u0424\u0435\u0432\u0440\u0443\u0430\u0440\u0438", "\u041C\u0430\u0440\u0442", "\u0410\u043F\u0440\u0438\u043B", "\u041C\u0430\u0439", "\u042E\u043D\u0438", "\u042E\u043B\u0438", "\u0410\u0432\u0433\u0443\u0441\u0442", "\u0421\u0435\u043F\u0442\u0435\u043C\u0432\u0440\u0438", "\u041E\u043A\u0442\u043E\u043C\u0432\u0440\u0438", "\u041D\u043E\u0435\u043C\u0432\u0440\u0438", "\u0414\u0435\u043A\u0435\u043C\u0432\u0440\u0438", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1027:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, d' / 'MMMM' / 'yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' / 'yyyy";
						fullDateTimePattern = "dddd, d' / 'MMMM' / 'yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "dg.", "dl.", "dt.", "dc.", "dj.", "dv.", "ds." };
						dayNames = new string[] { "diumenge", "dilluns", "dimarts", "dimecres", "dijous", "divendres", "dissabte" };
						monthNames = new string[] { "gener", "febrer", "mar\xe7", "abril", "maig", "juny", "juliol", "agost", "setembre", "octubre", "novembre", "desembre", "" };
						abbreviatedMonthNames = new string[] { "gen", "feb", "mar\xe7", "abr", "maig", "juny", "jul", "ag", "set", "oct", "nov", "des", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1028:
					{
						amDesignator = "\u4E0A\u5348";
						pmDesignator = "\u4E0B\u5348";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "yyyy/M/d";
						longDatePattern = "yyyy'\u5E74'M'\u6708'd'\u65E5'";
						shortTimePattern = "tt hh:mm";
						longTimePattern = "tt hh:mm:ss";
						monthDayPattern = "M'\u6708'd'\u65E5'";
						yearMonthPattern = "yyyy'\u5E74'M'\u6708'";
						fullDateTimePattern = "yyyy'\u5E74'M'\u6708'd'\u65E5' tt hh:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "\u661F\u671F\u65E5", "\u661F\u671F\u4E00", "\u661F\u671F\u4E8C", "\u661F\u671F\u4E09", "\u661F\u671F\u56DB", "\u661F\u671F\u4E94", "\u661F\u671F\u516D" };
						dayNames = new string[] { "\u661F\u671F\u65E5", "\u661F\u671F\u4E00", "\u661F\u671F\u4E8C", "\u661F\u671F\u4E09", "\u661F\u671F\u56DB", "\u661F\u671F\u4E94", "\u661F\u671F\u516D" };
						monthNames = new string[] { "\u4E00\u6708", "\u4E8C\u6708", "\u4E09\u6708", "\u56DB\u6708", "\u4E94\u6708", "\u516D\u6708", "\u4E03\u6708", "\u516B\u6708", "\u4E5D\u6708", "\u5341\u6708", "\u5341\u4E00\u6708", "\u5341\u4E8C\u6708", "" };
						abbreviatedMonthNames = new string[] { "\u4E00\u6708", "\u4E8C\u6708", "\u4E09\u6708", "\u56DB\u6708", "\u4E94\u6708", "\u516D\u6708", "\u4E03\u6708", "\u516B\u6708", "\u4E5D\u6708", "\u5341\u6708", "\u5341\u4E00\u6708", "\u5341\u4E8C\u6708", "" };
						return;
					}

				case 1029:
					{
						amDesignator = "dop.";
						pmDesignator = "odp.";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "d.M.yyyy";
						longDatePattern = "d. MMMM yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d. MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "ne", "po", "\xfat", "st", "\u010Dt", "p\xe1", "so" };
						dayNames = new string[] { "ned\u011Ble", "pond\u011Bl\xed", "\xfater\xfd", "st\u0159eda", "\u010Dtvrtek", "p\xe1tek", "sobota" };
						monthNames = new string[] { "leden", "\xfanor", "b\u0159ezen", "duben", "kv\u011Bten", "\u010Derven", "\u010Dervenec", "srpen", "z\xe1\u0159\xed", "\u0159\xedjen", "listopad", "prosinec", "" };
						abbreviatedMonthNames = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1030:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "dd-MM-yyyy";
						longDatePattern = "d. MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "d. MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d. MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "s\xf8", "ma", "ti", "on", "to", "fr", "l\xf8" };
						dayNames = new string[] { "s\xf8ndag", "mandag", "tirsdag", "onsdag", "torsdag", "fredag", "l\xf8rdag" };
						monthNames = new string[] { "januar", "februar", "marts", "april", "maj", "juni", "juli", "august", "september", "oktober", "november", "december", "" };
						abbreviatedMonthNames = new string[] { "jan", "feb", "mar", "apr", "maj", "jun", "jul", "aug", "sep", "okt", "nov", "dec", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 1031:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "dddd, d. MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd, d. MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "So", "Mo", "Di", "Mi", "Do", "Fr", "Sa" };
						dayNames = new string[] { "Sonntag", "Montag", "Dienstag", "Mittwoch", "Donnerstag", "Freitag", "Samstag" };
						monthNames = new string[] { "Januar", "Februar", "M\xe4rz", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mrz", "Apr", "Mai", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dez", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 1032:
					{
						amDesignator = "\u03C0\u03BC";
						pmDesignator = "\u03BC\u03BC";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "d/M/yyyy";
						longDatePattern = "dddd, d MMMM yyyy";
						shortTimePattern = "h:mm tt";
						longTimePattern = "h:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd, d MMMM yyyy h:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u039A\u03C5\u03C1", "\u0394\u03B5\u03C5", "\u03A4\u03C1\u03B9", "\u03A4\u03B5\u03C4", "\u03A0\u03B5\u03BC", "\u03A0\u03B1\u03C1", "\u03A3\u03B1\u03B2" };
						dayNames = new string[] { "\u039A\u03C5\u03C1\u03B9\u03B1\u03BA\u03AE", "\u0394\u03B5\u03C5\u03C4\u03AD\u03C1\u03B1", "\u03A4\u03C1\u03AF\u03C4\u03B7", "\u03A4\u03B5\u03C4\u03AC\u03C1\u03C4\u03B7", "\u03A0\u03AD\u03BC\u03C0\u03C4\u03B7", "\u03A0\u03B1\u03C1\u03B1\u03C3\u03BA\u03B5\u03C5\u03AE", "\u03A3\u03AC\u03B2\u03B2\u03B1\u03C4\u03BF" };
						monthNames = new string[] { "\u0399\u03B1\u03BD\u03BF\u03C5\u03AC\u03C1\u03B9\u03BF\u03C2", "\u03A6\u03B5\u03B2\u03C1\u03BF\u03C5\u03AC\u03C1\u03B9\u03BF\u03C2", "\u039C\u03AC\u03C1\u03C4\u03B9\u03BF\u03C2", "\u0391\u03C0\u03C1\u03AF\u03BB\u03B9\u03BF\u03C2", "\u039C\u03AC\u03B9\u03BF\u03C2", "\u0399\u03BF\u03CD\u03BD\u03B9\u03BF\u03C2", "\u0399\u03BF\u03CD\u03BB\u03B9\u03BF\u03C2", "\u0391\u03CD\u03B3\u03BF\u03C5\u03C3\u03C4\u03BF\u03C2", "\u03A3\u03B5\u03C0\u03C4\u03AD\u03BC\u03B2\u03C1\u03B9\u03BF\u03C2", "\u039F\u03BA\u03C4\u03CE\u03B2\u03C1\u03B9\u03BF\u03C2", "\u039D\u03BF\u03AD\u03BC\u03B2\u03C1\u03B9\u03BF\u03C2", "\u0394\u03B5\u03BA\u03AD\u03BC\u03B2\u03C1\u03B9\u03BF\u03C2", "" };
						abbreviatedMonthNames = new string[] { "\u0399\u03B1\u03BD", "\u03A6\u03B5\u03B2", "\u039C\u03B1\u03C1", "\u0391\u03C0\u03C1", "\u039C\u03B1\u03CA", "\u0399\u03BF\u03C5\u03BD", "\u0399\u03BF\u03C5\u03BB", "\u0391\u03C5\u03B3", "\u03A3\u03B5\u03C0", "\u039F\u03BA\u03C4", "\u039D\u03BF\u03B5", "\u0394\u03B5\u03BA", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1033:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "M/d/yyyy";
						longDatePattern = "dddd, MMMM dd, yyyy";
						shortTimePattern = "h:mm tt";
						longTimePattern = "h:mm:ss tt";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dddd, MMMM dd, yyyy h:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
						monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						return;
					}

				case 1035:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "d.M.yyyy";
						longDatePattern = "d. MMMM'ta 'yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "d. MMMM'ta'";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d. MMMM'ta 'yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "su", "ma", "ti", "ke", "to", "pe", "la" };
						dayNames = new string[] { "sunnuntai", "maanantai", "tiistai", "keskiviikko", "torstai", "perjantai", "lauantai" };
						monthNames = new string[] { "tammikuu", "helmikuu", "maaliskuu", "huhtikuu", "toukokuu", "kes\xe4kuu", "hein\xe4kuu", "elokuu", "syyskuu", "lokakuu", "marraskuu", "joulukuu", "" };
						abbreviatedMonthNames = new string[] { "tammi", "helmi", "maalis", "huhti", "touko", "kes\xe4", "hein\xe4", "elo", "syys", "loka", "marras", "joulu", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 1036:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd d MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "d MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd d MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "dim.", "lun.", "mar.", "mer.", "jeu.", "ven.", "sam." };
						dayNames = new string[] { "dimanche", "lundi", "mardi", "mercredi", "jeudi", "vendredi", "samedi" };
						monthNames = new string[] { "janvier", "f\xe9vrier", "mars", "avril", "mai", "juin", "juillet", "ao\xfbt", "septembre", "octobre", "novembre", "d\xe9cembre", "" };
						abbreviatedMonthNames = new string[] { "janv.", "f\xe9vr.", "mars", "avr.", "mai", "juin", "juil.", "ao\xfbt", "sept.", "oct.", "nov.", "d\xe9c.", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1037:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd dd MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd dd MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "\u05D9\u05D5\u05DD\xa0\u05D0", "\u05D9\u05D5\u05DD\xa0\u05D1", "\u05D9\u05D5\u05DD\xa0\u05D2", "\u05D9\u05D5\u05DD\xa0\u05D3", "\u05D9\u05D5\u05DD\xa0\u05D4", "\u05D9\u05D5\u05DD\xa0\u05D5", "\u05E9\u05D1\u05EA" };
						dayNames = new string[] { "\u05D9\u05D5\u05DD\xa0\u05E8\u05D0\u05E9\u05D5\u05DF", "\u05D9\u05D5\u05DD\xa0\u05E9\u05E0\u05D9", "\u05D9\u05D5\u05DD\xa0\u05E9\u05DC\u05D9\u05E9\u05D9", "\u05D9\u05D5\u05DD\xa0\u05E8\u05D1\u05D9\u05E2\u05D9", "\u05D9\u05D5\u05DD\xa0\u05D7\u05DE\u05D9\u05E9\u05D9", "\u05D9\u05D5\u05DD\xa0\u05E9\u05D9\u05E9\u05D9", "\u05E9\u05D1\u05EA" };
						monthNames = new string[] { "\u05D9\u05E0\u05D5\u05D0\u05E8", "\u05E4\u05D1\u05E8\u05D5\u05D0\u05E8", "\u05DE\u05E8\u05E5", "\u05D0\u05E4\u05E8\u05D9\u05DC", "\u05DE\u05D0\u05D9", "\u05D9\u05D5\u05E0\u05D9", "\u05D9\u05D5\u05DC\u05D9", "\u05D0\u05D5\u05D2\u05D5\u05E1\u05D8", "\u05E1\u05E4\u05D8\u05DE\u05D1\u05E8", "\u05D0\u05D5\u05E7\u05D8\u05D5\u05D1\u05E8", "\u05E0\u05D5\u05D1\u05DE\u05D1\u05E8", "\u05D3\u05E6\u05DE\u05D1\u05E8", "" };
						abbreviatedMonthNames = new string[] { "\u05D9\u05E0\u05D5", "\u05E4\u05D1\u05E8", "\u05DE\u05E8\u05E5", "\u05D0\u05E4\u05E8", "\u05DE\u05D0\u05D9", "\u05D9\u05D5\u05E0", "\u05D9\u05D5\u05DC", "\u05D0\u05D5\u05D2", "\u05E1\u05E4\u05D8", "\u05D0\u05D5\u05E7", "\u05E0\u05D5\u05D1", "\u05D3\u05E6\u05DE", "" };
						return;
					}

				case 1038:
					{
						amDesignator = "de.";
						pmDesignator = "du.";
						dateSeparator = ". ";
						timeSeparator = ":";
						shortDatePattern = "yyyy. MM. dd.";
						longDatePattern = "yyyy. MMMM d.";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "MMMM d.";
						yearMonthPattern = "yyyy. MMMM";
						fullDateTimePattern = "yyyy. MMMM d. H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "V", "H", "K", "Sze", "Cs", "P", "Szo" };
						dayNames = new string[] { "vas\xe1rnap", "h\xe9tf\u0151", "kedd", "szerda", "cs\xfct\xf6rt\xf6k", "p\xe9ntek", "szombat" };
						monthNames = new string[] { "janu\xe1r", "febru\xe1r", "m\xe1rcius", "\xe1prilis", "m\xe1jus", "j\xfanius", "j\xfalius", "augusztus", "szeptember", "okt\xf3ber", "november", "december", "" };
						abbreviatedMonthNames = new string[] { "jan.", "febr.", "m\xe1rc.", "\xe1pr.", "m\xe1j.", "j\xfan.", "j\xfal.", "aug.", "szept.", "okt.", "nov.", "dec.", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1039:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "d.M.yyyy";
						longDatePattern = "d. MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "d. MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d. MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "sun.", "m\xe1n.", "\xferi.", "mi\xf0.", "fim.", "f\xf6s.", "lau." };
						dayNames = new string[] { "sunnudagur", "m\xe1nudagur", "\xferi\xf0judagur", "mi\xf0vikudagur", "fimmtudagur", "f\xf6studagur", "laugardagur" };
						monthNames = new string[] { "jan\xfaar", "febr\xfaar", "mars", "apr\xedl", "ma\xed", "j\xfan\xed", "j\xfal\xed", "\xe1g\xfast", "september", "okt\xf3ber", "n\xf3vember", "desember", "" };
						abbreviatedMonthNames = new string[] { "jan.", "feb.", "mar.", "apr.", "ma\xed", "j\xfan.", "j\xfal.", "\xe1g\xfa.", "sep.", "okt.", "n\xf3v.", "des.", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 1040:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "/";
						timeSeparator = ".";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd d MMMM yyyy";
						shortTimePattern = "H.mm";
						longTimePattern = "H.mm.ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd d MMMM yyyy H.mm.ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mer", "gio", "ven", "sab" };
						dayNames = new string[] { "domenica", "luned\xec", "marted\xec", "mercoled\xec", "gioved\xec", "venerd\xec", "sabato" };
						monthNames = new string[] { "gennaio", "febbraio", "marzo", "aprile", "maggio", "giugno", "luglio", "agosto", "settembre", "ottobre", "novembre", "dicembre", "" };
						abbreviatedMonthNames = new string[] { "gen", "feb", "mar", "apr", "mag", "giu", "lug", "ago", "set", "ott", "nov", "dic", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 1041:
					{
						amDesignator = "\u5348\u524D";
						pmDesignator = "\u5348\u5F8C";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "yyyy/MM/dd";
						longDatePattern = "yyyy'\u5E74'M'\u6708'd'\u65E5'";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "M'\u6708'd'\u65E5'";
						yearMonthPattern = "yyyy'\u5E74'M'\u6708'";
						fullDateTimePattern = "yyyy'\u5E74'M'\u6708'd'\u65E5' H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "\u65E5", "\u6708", "\u706B", "\u6C34", "\u6728", "\u91D1", "\u571F" };
						dayNames = new string[] { "\u65E5\u66DC\u65E5", "\u6708\u66DC\u65E5", "\u706B\u66DC\u65E5", "\u6C34\u66DC\u65E5", "\u6728\u66DC\u65E5", "\u91D1\u66DC\u65E5", "\u571F\u66DC\u65E5" };
						monthNames = new string[] { "1\u6708", "2\u6708", "3\u6708", "4\u6708", "5\u6708", "6\u6708", "7\u6708", "8\u6708", "9\u6708", "10\u6708", "11\u6708", "12\u6708", "" };
						abbreviatedMonthNames = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "" };
						return;
					}

				case 1042:
					{
						amDesignator = "\uC624\uC804";
						pmDesignator = "\uC624\uD6C4";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "yyyy-MM-dd";
						longDatePattern = "yyyy'\uB144' M'\uC6D4' d'\uC77C' dddd";
						shortTimePattern = "tt h:mm";
						longTimePattern = "tt h:mm:ss";
						monthDayPattern = "M'\uC6D4' d'\uC77C'";
						yearMonthPattern = "yyyy'\uB144' M'\uC6D4'";
						fullDateTimePattern = "yyyy'\uB144' M'\uC6D4' d'\uC77C' dddd tt h:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "\uC77C", "\uC6D4", "\uD654", "\uC218", "\uBAA9", "\uAE08", "\uD1A0" };
						dayNames = new string[] { "\uC77C\uC694\uC77C", "\uC6D4\uC694\uC77C", "\uD654\uC694\uC77C", "\uC218\uC694\uC77C", "\uBAA9\uC694\uC77C", "\uAE08\uC694\uC77C", "\uD1A0\uC694\uC77C" };
						monthNames = new string[] { "1\uC6D4", "2\uC6D4", "3\uC6D4", "4\uC6D4", "5\uC6D4", "6\uC6D4", "7\uC6D4", "8\uC6D4", "9\uC6D4", "10\uC6D4", "11\uC6D4", "12\uC6D4", "" };
						abbreviatedMonthNames = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "" };
						return;
					}

				case 1043:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "d-M-yyyy";
						longDatePattern = "dddd d MMMM yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd d MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "zo", "ma", "di", "wo", "do", "vr", "za" };
						dayNames = new string[] { "zondag", "maandag", "dinsdag", "woensdag", "donderdag", "vrijdag", "zaterdag" };
						monthNames = new string[] { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december", "" };
						abbreviatedMonthNames = new string[] { "jan", "feb", "mrt", "apr", "mei", "jun", "jul", "aug", "sep", "okt", "nov", "dec", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 1044:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "d. MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "d. MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d. MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "s\xf8", "ma", "ti", "on", "to", "fr", "l\xf8" };
						dayNames = new string[] { "s\xf8ndag", "mandag", "tirsdag", "onsdag", "torsdag", "fredag", "l\xf8rdag" };
						monthNames = new string[] { "januar", "februar", "mars", "april", "mai", "juni", "juli", "august", "september", "oktober", "november", "desember", "" };
						abbreviatedMonthNames = new string[] { "jan", "feb", "mar", "apr", "mai", "jun", "jul", "aug", "sep", "okt", "nov", "des", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 1045:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "yyyy-MM-dd";
						longDatePattern = "d MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "d MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "N", "Pn", "Wt", "\u015Ar", "Cz", "Pt", "So" };
						dayNames = new string[] { "niedziela", "poniedzia\u0142ek", "wtorek", "\u015Broda", "czwartek", "pi\u0105tek", "sobota" };
						monthNames = new string[] { "stycze\u0144", "luty", "marzec", "kwiecie\u0144", "maj", "czerwiec", "lipiec", "sierpie\u0144", "wrzesie\u0144", "pa\u017Adziernik", "listopad", "grudzie\u0144", "" };
						abbreviatedMonthNames = new string[] { "sty", "lut", "mar", "kwi", "maj", "cze", "lip", "sie", "wrz", "pa\u017A", "lis", "gru", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 1046:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "d/M/yyyy";
						longDatePattern = "dddd, d' de 'MMMM' de 'yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "dd' de 'MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, d' de 'MMMM' de 'yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "dom", "seg", "ter", "qua", "qui", "sex", "s\xe1b" };
						dayNames = new string[] { "domingo", "segunda-feira", "ter\xe7a-feira", "quarta-feira", "quinta-feira", "sexta-feira", "s\xe1bado" };
						monthNames = new string[] { "janeiro", "fevereiro", "mar\xe7o", "abril", "maio", "junho", "julho", "agosto", "setembro", "outubro", "novembro", "dezembro", "" };
						abbreviatedMonthNames = new string[] { "jan", "fev", "mar", "abr", "mai", "jun", "jul", "ago", "set", "out", "nov", "dez", "" };
						return;
					}

				case 1048:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "d MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "d MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "D", "L", "Ma", "Mi", "J", "V", "S" };
						dayNames = new string[] { "duminic\u0103", "luni", "mar\u0163i", "miercuri", "joi", "vineri", "s\xe2mb\u0103t\u0103" };
						monthNames = new string[] { "ianuarie", "februarie", "martie", "aprilie", "mai", "iunie", "iulie", "august", "septembrie", "octombrie", "noiembrie", "decembrie", "" };
						abbreviatedMonthNames = new string[] { "ian.", "feb.", "mar.", "apr.", "mai.", "iun.", "iul.", "aug.", "sep.", "oct.", "nov.", "dec.", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1049:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "d MMMM yyyy '\u0433.'";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM yyyy '\u0433.'";
						fullDateTimePattern = "d MMMM yyyy '\u0433.' H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u0412\u0441", "\u041F\u043D", "\u0412\u0442", "\u0421\u0440", "\u0427\u0442", "\u041F\u0442", "\u0421\u0431" };
						dayNames = new string[] { "\u0432\u043E\u0441\u043A\u0440\u0435\u0441\u0435\u043D\u044C\u0435", "\u043F\u043E\u043D\u0435\u0434\u0435\u043B\u044C\u043D\u0438\u043A", "\u0432\u0442\u043E\u0440\u043D\u0438\u043A", "\u0441\u0440\u0435\u0434\u0430", "\u0447\u0435\u0442\u0432\u0435\u0440\u0433", "\u043F\u044F\u0442\u043D\u0438\u0446\u0430", "\u0441\u0443\u0431\u0431\u043E\u0442\u0430" };
						monthNames = new string[] { "\u042F\u043D\u0432\u0430\u0440\u044C", "\u0424\u0435\u0432\u0440\u0430\u043B\u044C", "\u041C\u0430\u0440\u0442", "\u0410\u043F\u0440\u0435\u043B\u044C", "\u041C\u0430\u0439", "\u0418\u044E\u043D\u044C", "\u0418\u044E\u043B\u044C", "\u0410\u0432\u0433\u0443\u0441\u0442", "\u0421\u0435\u043D\u0442\u044F\u0431\u0440\u044C", "\u041E\u043A\u0442\u044F\u0431\u0440\u044C", "\u041D\u043E\u044F\u0431\u0440\u044C", "\u0414\u0435\u043A\u0430\u0431\u0440\u044C", "" };
						abbreviatedMonthNames = new string[] { "\u044F\u043D\u0432", "\u0444\u0435\u0432", "\u043C\u0430\u0440", "\u0430\u043F\u0440", "\u043C\u0430\u0439", "\u0438\u044E\u043D", "\u0438\u044E\u043B", "\u0430\u0432\u0433", "\u0441\u0435\u043D", "\u043E\u043A\u0442", "\u043D\u043E\u044F", "\u0434\u0435\u043A", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1050:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "d.M.yyyy";
						longDatePattern = "d. MMMM yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "d. MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "d. MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "ned", "pon", "uto", "sri", "\u010Det", "pet", "sub" };
						dayNames = new string[] { "nedjelja", "ponedjeljak", "utorak", "srijeda", "\u010Detvrtak", "petak", "subota" };
						monthNames = new string[] { "sije\u010Danj", "velja\u010Da", "o\u017Eujak", "travanj", "svibanj", "lipanj", "srpanj", "kolovoz", "rujan", "listopad", "studeni", "prosinac", "" };
						abbreviatedMonthNames = new string[] { "sij", "vlj", "o\u017Eu", "tra", "svi", "lip", "srp", "kol", "ruj", "lis", "stu", "pro", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1051:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ". ";
						timeSeparator = ":";
						shortDatePattern = "d. M. yyyy";
						longDatePattern = "d. MMMM yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d. MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "ne", "po", "ut", "st", "\u0161t", "pi", "so" };
						dayNames = new string[] { "nede\u013Ea", "pondelok", "utorok", "streda", "\u0161tvrtok", "piatok", "sobota" };
						monthNames = new string[] { "janu\xe1r", "febru\xe1r", "marec", "apr\xedl", "m\xe1j", "j\xfan", "j\xfal", "august", "september", "okt\xf3ber", "november", "december", "" };
						abbreviatedMonthNames = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "VIII", "IX", "X", "XI", "XII", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1052:
					{
						amDesignator = "PD";
						pmDesignator = "MD";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "yyyy-MM-dd";
						longDatePattern = "yyyy-MM-dd";
						shortTimePattern = "h:mm.tt";
						longTimePattern = "h:mm:ss.tt";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "yyyy-MM";
						fullDateTimePattern = "yyyy-MM-dd h:mm:ss.tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "Die", "H\xebn", "Mar", "M\xebr", "Enj", "Pre", "Sht" };
						dayNames = new string[] { "e diel", "e h\xebn\xeb", "e mart\xeb", "e m\xebrkur\xeb", "e enjte", "e premte", "e shtun\xeb" };
						monthNames = new string[] { "janar", "shkurt", "mars", "prill", "maj", "qershor", "korrik", "gusht", "shtator", "tetor", "n\xebntor", "dhjetor", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Shk", "Mar", "Pri", "Maj", "Qer", "Kor", "Gsh", "Sht", "Tet", "N\xebn", "Dhj", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1053:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "yyyy-MM-dd";
						longDatePattern = "'den 'd MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "'den 'd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "'den 'd MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "s\xf6", "m\xe5", "ti", "on", "to", "fr", "l\xf6" };
						dayNames = new string[] { "s\xf6ndag", "m\xe5ndag", "tisdag", "onsdag", "torsdag", "fredag", "l\xf6rdag" };
						monthNames = new string[] { "januari", "februari", "mars", "april", "maj", "juni", "juli", "augusti", "september", "oktober", "november", "december", "" };
						abbreviatedMonthNames = new string[] { "jan", "feb", "mar", "apr", "maj", "jun", "jul", "aug", "sep", "okt", "nov", "dec", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 1054:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "d/M/yyyy";
						longDatePattern = "d MMMM yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u0E2D\u0E32.", "\u0E08.", "\u0E2D.", "\u0E1E.", "\u0E1E\u0E24.", "\u0E28.", "\u0E2A." };
						dayNames = new string[] { "\u0E2D\u0E32\u0E17\u0E34\u0E15\u0E22\u0E4C", "\u0E08\u0E31\u0E19\u0E17\u0E23\u0E4C", "\u0E2D\u0E31\u0E07\u0E04\u0E32\u0E23", "\u0E1E\u0E38\u0E18", "\u0E1E\u0E24\u0E2B\u0E31\u0E2A\u0E1A\u0E14\u0E35", "\u0E28\u0E38\u0E01\u0E23\u0E4C", "\u0E40\u0E2A\u0E32\u0E23\u0E4C" };
						monthNames = new string[] { "\u0E21\u0E01\u0E23\u0E32\u0E04\u0E21", "\u0E01\u0E38\u0E21\u0E20\u0E32\u0E1E\u0E31\u0E19\u0E18\u0E4C", "\u0E21\u0E35\u0E19\u0E32\u0E04\u0E21", "\u0E40\u0E21\u0E29\u0E32\u0E22\u0E19", "\u0E1E\u0E24\u0E29\u0E20\u0E32\u0E04\u0E21", "\u0E21\u0E34\u0E16\u0E38\u0E19\u0E32\u0E22\u0E19", "\u0E01\u0E23\u0E01\u0E0E\u0E32\u0E04\u0E21", "\u0E2A\u0E34\u0E07\u0E2B\u0E32\u0E04\u0E21", "\u0E01\u0E31\u0E19\u0E22\u0E32\u0E22\u0E19", "\u0E15\u0E38\u0E25\u0E32\u0E04\u0E21", "\u0E1E\u0E24\u0E28\u0E08\u0E34\u0E01\u0E32\u0E22\u0E19", "\u0E18\u0E31\u0E19\u0E27\u0E32\u0E04\u0E21", "" };
						abbreviatedMonthNames = new string[] { "\u0E21.\u0E04.", "\u0E01.\u0E1E.", "\u0E21\u0E35.\u0E04.", "\u0E40\u0E21.\u0E22.", "\u0E1E.\u0E04.", "\u0E21\u0E34.\u0E22.", "\u0E01.\u0E04.", "\u0E2A.\u0E04.", "\u0E01.\u0E22.", "\u0E15.\u0E04.", "\u0E1E.\u0E22.", "\u0E18.\u0E04.", "" };
						calendar = new ThaiBuddhistCalendar();
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1055:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "dd MMMM yyyy dddd";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dd MMMM yyyy dddd HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "Paz", "Pzt", "Sal", "\xc7ar", "Per", "Cum", "Cmt" };
						dayNames = new string[] { "Pazar", "Pazartesi", "Sal\u0131", "\xc7ar\u015Famba", "Per\u015Fembe", "Cuma", "Cumartesi" };
						monthNames = new string[] { "Ocak", "\u015Eubat", "Mart", "Nisan", "May\u0131s", "Haziran", "Temmuz", "A\u011Fustos", "Eyl\xfcl", "Ekim", "Kas\u0131m", "Aral\u0131k", "" };
						abbreviatedMonthNames = new string[] { "Oca", "\u015Eub", "Mar", "Nis", "May", "Haz", "Tem", "A\u011Fu", "Eyl", "Eki", "Kas", "Ara", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1056:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM, yyyy";
						shortTimePattern = "h:mm tt";
						longTimePattern = "h:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM, yyyy h:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 1;
						abbreviatedDayNames = new string[] { "\u0627\u062A\u0648\u0627\u0631", "\u067E\u064A\u0631", "\u0645\u0646\u06AF\u0644", "\u0628\u062F\u06BE", "\u062C\u0645\u0639\u0631\u0627\u062A", "\u062C\u0645\u0639\u0647", "\u0647\u0641\u062A\u0647" };
						dayNames = new string[] { "\u0627\u062A\u0648\u0627\u0631", "\u067E\u064A\u0631", "\u0645\u0646\u06AF\u0644", "\u0628\u062F\u06BE", "\u062C\u0645\u0639\u0631\u0627\u062A", "\u062C\u0645\u0639\u0647", "\u0647\u0641\u062A\u0647" };
						monthNames = new string[] { "\u062C\u0646\u0648\u0631\u0649", "\u0641\u0631\u0648\u0631\u0649", "\u0645\u0627\u0631\u0686", "\u0627\u067E\u0631\u064A\u0644", "\u0645\u0626", "\u062C\u0648\u0646", "\u062C\u0648\u0644\u0627\u0678", "\u0627\u06AF\u0633\u062A", "\u0633\u062A\u0645\u0628\u0631", "\u0627\u06A9\u062A\u0648\u0628\u0631", "\u0646\u0648\u0645\u0628\u0631", "\u062F\u0633\u0645\u0628\u0631", "" };
						abbreviatedMonthNames = new string[] { "\u062C\u0646\u0648\u0631\u0649", "\u0641\u0631\u0648\u0631\u0649", "\u0645\u0627\u0631\u0686", "\u0627\u067E\u0631\u064A\u0644", "\u0645\u0626", "\u062C\u0648\u0646", "\u062C\u0648\u0644\u0627\u0678", "\u0627\u06AF\u0633\u062A", "\u0633\u062A\u0645\u0628\u0631", "\u0627\u06A9\u062A\u0648\u0628\u0631", "\u0646\u0648\u0645\u0628\u0631", "\u062F\u0633\u0645\u0628\u0631", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFullWeek;
						return;
					}

				case 1057:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dd MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "Minggu", "Sen", "Sel", "Rabu", "Kamis", "Jumat", "Sabtu" };
						dayNames = new string[] { "Minggu", "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu" };
						monthNames = new string[] { "Januari", "Februari", "Maret", "April", "Mei", "Juni", "Juli", "Agustus", "September", "Oktober", "Nopember", "Desember", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "Mei", "Jun", "Jul", "Agust", "Sep", "Okt", "Nop", "Des", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1058:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "d MMMM yyyy' \u0440.'";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "d MMMM";
						yearMonthPattern = "MMMM yyyy' \u0440.'";
						fullDateTimePattern = "d MMMM yyyy' \u0440.' H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u041D\u0434", "\u041F\u043D", "\u0412\u0442", "\u0421\u0440", "\u0427\u0442", "\u041F\u0442", "\u0421\u0431" };
						dayNames = new string[] { "\u043D\u0435\u0434\u0456\u043B\u044F", "\u043F\u043E\u043D\u0435\u0434\u0456\u043B\u043E\u043A", "\u0432\u0456\u0432\u0442\u043E\u0440\u043E\u043A", "\u0441\u0435\u0440\u0435\u0434\u0430", "\u0447\u0435\u0442\u0432\u0435\u0440", "\u043F'\u044F\u0442\u043D\u0438\u0446\u044F", "\u0441\u0443\u0431\u043E\u0442\u0430" };
						monthNames = new string[] { "\u0421\u0456\u0447\u0435\u043D\u044C", "\u041B\u044E\u0442\u0438\u0439", "\u0411\u0435\u0440\u0435\u0437\u0435\u043D\u044C", "\u041A\u0432\u0456\u0442\u0435\u043D\u044C", "\u0422\u0440\u0430\u0432\u0435\u043D\u044C", "\u0427\u0435\u0440\u0432\u0435\u043D\u044C", "\u041B\u0438\u043F\u0435\u043D\u044C", "\u0421\u0435\u0440\u043F\u0435\u043D\u044C", "\u0412\u0435\u0440\u0435\u0441\u0435\u043D\u044C", "\u0416\u043E\u0432\u0442\u0435\u043D\u044C", "\u041B\u0438\u0441\u0442\u043E\u043F\u0430\u0434", "\u0413\u0440\u0443\u0434\u0435\u043D\u044C", "" };
						abbreviatedMonthNames = new string[] { "\u0421\u0456\u0447", "\u041B\u044E\u0442", "\u0411\u0435\u0440", "\u041A\u0432\u0456", "\u0422\u0440\u0430", "\u0427\u0435\u0440", "\u041B\u0438\u043F", "\u0421\u0435\u0440", "\u0412\u0435\u0440", "\u0416\u043E\u0432", "\u041B\u0438\u0441", "\u0413\u0440\u0443", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1059:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "d MMMM yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "d MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u043D\u0434", "\u043F\u043D", "\u0430\u045E", "\u0441\u0440", "\u0447\u0446", "\u043F\u0442", "\u0441\u0431" };
						dayNames = new string[] { "\u043D\u044F\u0434\u0437\u0435\u043B\u044F", "\u043F\u0430\u043D\u044F\u0434\u0437\u0435\u043B\u0430\u043A", "\u0430\u045E\u0442\u043E\u0440\u0430\u043A", "\u0441\u0435\u0440\u0430\u0434\u0430", "\u0447\u0430\u0446\u0432\u0435\u0440", "\u043F\u044F\u0442\u043D\u0456\u0446\u0430", "\u0441\u0443\u0431\u043E\u0442\u0430" };
						monthNames = new string[] { "\u0421\u0442\u0443\u0434\u0437\u0435\u043D\u044C", "\u041B\u044E\u0442\u044B", "\u0421\u0430\u043A\u0430\u0432\u0456\u043A", "\u041A\u0440\u0430\u0441\u0430\u0432\u0456\u043A", "\u041C\u0430\u0439", "\u0427\u044D\u0440\u0432\u0435\u043D\u044C", "\u041B\u0456\u043F\u0435\u043D\u044C", "\u0416\u043D\u0456\u0432\u0435\u043D\u044C", "\u0412\u0435\u0440\u0430\u0441\u0435\u043D\u044C", "\u041A\u0430\u0441\u0442\u0440\u044B\u0447\u043D\u0456\u043A", "\u041B\u0456\u0441\u0442\u0430\u043F\u0430\u0434", "\u0421\u043D\u0435\u0436\u0430\u043D\u044C", "" };
						abbreviatedMonthNames = new string[] { "\u0421\u0442\u0443", "\u041B\u044E\u0442", "\u0421\u0430\u043A", "\u041A\u0440\u0430", "\u041C\u0430\u0439", "\u0427\u044D\u0440", "\u041B\u0456\u043F", "\u0416\u043D\u0456", "\u0412\u0435\u0440", "\u041A\u0430\u0441", "\u041B\u0456\u0441", "\u0421\u043D\u0435", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1060:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "d.M.yyyy";
						longDatePattern = "d. MMMM yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "d. MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d. MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "ned", "pon", "tor", "sre", "\u010Det", "pet", "sob" };
						dayNames = new string[] { "nedelja", "ponedeljek", "torek", "sreda", "\u010Detrtek", "petek", "sobota" };
						monthNames = new string[] { "januar", "februar", "marec", "april", "maj", "junij", "julij", "avgust", "september", "oktober", "november", "december", "" };
						abbreviatedMonthNames = new string[] { "jan", "feb", "mar", "apr", "maj", "jun", "jul", "avg", "sep", "okt", "nov", "dec", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1061:
					{
						amDesignator = "EL";
						pmDesignator = "PL";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "d.MM.yyyy";
						longDatePattern = "d. MMMM yyyy'. a.'";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "d. MMMM";
						yearMonthPattern = "MMMM yyyy'. a.'";
						fullDateTimePattern = "d. MMMM yyyy'. a.' H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "P", "E", "T", "K", "N", "R", "L" };
						dayNames = new string[] { "p\xfchap\xe4ev", "esmasp\xe4ev", "teisip\xe4ev", "kolmap\xe4ev", "neljap\xe4ev", "reede", "laup\xe4ev" };
						monthNames = new string[] { "jaanuar", "veebruar", "m\xe4rts", "aprill", "mai", "juuni", "juuli", "august", "september", "oktoober", "november", "detsember", "" };
						abbreviatedMonthNames = new string[] { "jaan", "veebr", "m\xe4rts", "apr", "mai", "juuni", "juuli", "aug", "sept", "okt", "nov", "dets", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 1062:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "yyyy.MM.dd.";
						longDatePattern = "dddd, yyyy'. gada 'd. MMMM";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "d. MMMM";
						yearMonthPattern = "yyyy. MMMM";
						fullDateTimePattern = "dddd, yyyy'. gada 'd. MMMM H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "Sv", "Pr", "Ot", "Tr", "Ce", "Pk", "Se" };
						dayNames = new string[] { "sv\u0113tdiena", "pirmdiena", "otrdiena", "tre\u0161diena", "ceturtdiena", "piektdiena", "sestdiena" };
						monthNames = new string[] { "janv\u0101ris", "febru\u0101ris", "marts", "apr\u012Blis", "maijs", "j\u016Bnijs", "j\u016Blijs", "augusts", "septembris", "oktobris", "novembris", "decembris", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "Mai", "J\u016Bn", "J\u016Bl", "Aug", "Sep", "Okt", "Nov", "Dec", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 1063:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "yyyy.MM.dd";
						longDatePattern = "yyyy 'm.' MMMM d 'd.'";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "MMMM d 'd.'";
						yearMonthPattern = "yyyy 'm.' MMMM";
						fullDateTimePattern = "yyyy 'm.' MMMM d 'd.' HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "Sk", "Pr", "An", "Tr", "Kt", "Pn", "\u0160t" };
						dayNames = new string[] { "sekmadienis", "pirmadienis", "antradienis", "tre\u010Diadienis", "ketvirtadienis", "penktadienis", "\u0161e\u0161tadienis" };
						monthNames = new string[] { "sausis", "vasaris", "kovas", "balandis", "gegu\u017E\u0117", "bir\u017Eelis", "liepa", "rugpj\u016Btis", "rugs\u0117jis", "spalis", "lapkritis", "gruodis", "" };
						abbreviatedMonthNames = new string[] { "Sau", "Vas", "Kov", "Bal", "Geg", "Bir", "Lie", "Rgp", "Rgs", "Spl", "Lap", "Grd", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1065:
					{
						amDesignator = "\u0642.\u0638";
						pmDesignator = "\u0628.\u0638";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "M/d/yyyy";
						longDatePattern = "dddd, MMMM dd, yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dddd, MMMM dd, yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
						monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						return;
					}

				case 1066:
					{
						amDesignator = "SA";
						pmDesignator = "CH";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM yyyy";
						shortTimePattern = "h:mm tt";
						longTimePattern = "h:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dd MMMM yyyy h:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "CN", "Hai", "Ba", "T\u01B0", "N\u0103m", "Sa\u0301u", "Ba\u0309y" };
						dayNames = new string[] { "Chu\u0309 Nh\xe2\u0323t", "Th\u01B0\u0301 Hai", "Th\u01B0\u0301 Ba", "Th\u01B0\u0301 T\u01B0", "Th\u01B0\u0301 N\u0103m", "Th\u01B0\u0301 Sa\u0301u", "Th\u01B0\u0301 Ba\u0309y" };
						monthNames = new string[] { "Tha\u0301ng Gi\xeang", "Tha\u0301ng Hai", "Tha\u0301ng Ba", "Tha\u0301ng T\u01B0", "Tha\u0301ng N\u0103m", "Tha\u0301ng Sa\u0301u", "Tha\u0301ng Ba\u0309y", "Tha\u0301ng Ta\u0301m", "Tha\u0301ng Chi\u0301n", "Tha\u0301ng M\u01B0\u01A1\u0300i", "Tha\u0301ng M\u01B0\u01A1\u0300i M\xf4\u0323t", "Tha\u0301ng M\u01B0\u01A1\u0300i Hai", "" };
						abbreviatedMonthNames = new string[] { "Thg1", "Thg2", "Thg3", "Thg4", "Thg5", "Thg6", "Thg7", "Thg8", "Thg9", "Thg10", "Thg11", "Thg12", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1067:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "d MMMM, yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "d MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "d MMMM, yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u053F\u056B\u0580", "\u0535\u0580\u056F", "\u0535\u0580\u0584", "\u0549\u0580\u0584", "\u0540\u0576\u0563", "\u0548\u0552\u0580", "\u0547\u0562\u0569" };
						dayNames = new string[] { "\u053F\u056B\u0580\u0561\u056F\u056B", "\u0535\u0580\u056F\u0578\u0582\u0577\u0561\u0562\u0569\u056B", "\u0535\u0580\u0565\u0584\u0577\u0561\u0562\u0569\u056B", "\u0549\u0578\u0580\u0565\u0584\u0577\u0561\u0562\u0569\u056B", "\u0540\u056B\u0576\u0563\u0577\u0561\u0562\u0569\u056B", "\u0548\u0552\u0580\u0562\u0561\u0569", "\u0547\u0561\u0562\u0561\u0569" };
						monthNames = new string[] { "\u0540\u0578\u0582\u0576\u057E\u0561\u0580", "\u0553\u0565\u057F\u0580\u057E\u0561\u0580", "\u0544\u0561\u0580\u057F", "\u0531\u057A\u0580\u056B\u056C", "\u0544\u0561\u0575\u056B\u057D", "\u0540\u0578\u0582\u0576\u056B\u057D", "\u0540\u0578\u0582\u056C\u056B\u057D", "\u0555\u0563\u0578\u057D\u057F\u0578\u057D", "\u054D\u0565\u057A\u057F\u0565\u0574\u0562\u0565\u0580", "\u0540\u0578\u056F\u057F\u0565\u0574\u0562\u0565\u0580", "\u0546\u0578\u0575\u0565\u0574\u0562\u0565\u0580", "\u0534\u0565\u056F\u057F\u0565\u0574\u0562\u0565\u0580", "" };
						abbreviatedMonthNames = new string[] { "\u0540\u0546\u054E", "\u0553\u054F\u054E", "\u0544\u0550\u054F", "\u0531\u054A\u0550", "\u0544\u0545\u054D", "\u0540\u0546\u054D", "\u0540\u053C\u054D", "\u0555\u0533\u054D", "\u054D\u0535\u054A", "\u0540\u0548\u053F", "\u0546\u0548\u0545", "\u0534\u0535\u053F", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1068:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "d MMMM yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "d MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "B", "Be", "\xc7a", "\xc7", "Ca", "C", "\u015E" };
						dayNames = new string[] { "Bazar", "Bazar\xa0ert\u0259si", "\xc7\u0259r\u015F\u0259nb\u0259\xa0ax\u015Fam\u0131", "\xc7\u0259r\u015F\u0259nb\u0259", "C\xfcm\u0259\xa0ax\u015Fam\u0131", "C\xfcm\u0259", "\u015E\u0259nb\u0259" };
						monthNames = new string[] { "Yanvar", "Fevral", "Mart", "Aprel", "May", "\u0130yun", "\u0130yul", "Avgust", "Sentyabr", "Oktyabr", "Noyabr", "Dekabr", "" };
						abbreviatedMonthNames = new string[] { "Yan", "Fev", "Mar", "Apr", "May", "\u0130yun", "\u0130yul", "Avg", "Sen", "Okt", "Noy", "Dek", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1069:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "yyyy/MM/dd";
						longDatePattern = "dddd, yyyy.'eko' MMMM'k 'd";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "yyyy.'eko' MMMM";
						fullDateTimePattern = "dddd, yyyy.'eko' MMMM'k 'd HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "ig.", "al.", "as.", "az.", "og.", "or.", "lr." };
						dayNames = new string[] { "igandea", "astelehena", "asteartea", "asteazkena", "osteguna", "ostirala", "larunbata" };
						monthNames = new string[] { "urtarrila", "otsaila", "martxoa", "apirila", "maiatza", "ekaina", "uztaila", "abuztua", "iraila", "urria", "azaroa", "abendua", "" };
						abbreviatedMonthNames = new string[] { "urt.", "ots.", "mar.", "api.", "mai.", "eka.", "uzt.", "abu.", "ira.", "urr.", "aza.", "abe.", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1071:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "dddd, dd MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd, dd MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "\u043D\u0435\u0434", "\u043F\u043E\u043D", "\u0432\u0442\u0440", "\u0441\u0440\u0434", "\u0447\u0435\u0442", "\u043F\u0435\u0442", "\u0441\u0430\u0431" };
						dayNames = new string[] { "\u043D\u0435\u0434\u0435\u043B\u0430", "\u043F\u043E\u043D\u0435\u0434\u0435\u043B\u043D\u0438\u043A", "\u0432\u0442\u043E\u0440\u043D\u0438\u043A", "\u0441\u0440\u0435\u0434\u0430", "\u0447\u0435\u0442\u0432\u0440\u0442\u043E\u043A", "\u043F\u0435\u0442\u043E\u043A", "\u0441\u0430\u0431\u043E\u0442\u0430" };
						monthNames = new string[] { "\u0458\u0430\u043D\u0443\u0430\u0440\u0438", "\u0444\u0435\u0432\u0440\u0443\u0430\u0440\u0438", "\u043C\u0430\u0440\u0442", "\u0430\u043F\u0440\u0438\u043B", "\u043C\u0430\u0458", "\u0458\u0443\u043D\u0438", "\u0458\u0443\u043B\u0438", "\u0430\u0432\u0433\u0443\u0441\u0442", "\u0441\u0435\u043F\u0442\u0435\u043C\u0432\u0440\u0438", "\u043E\u043A\u0442\u043E\u043C\u0432\u0440\u0438", "\u043D\u043E\u0435\u043C\u0432\u0440\u0438", "\u0434\u0435\u043A\u0435\u043C\u0432\u0440\u0438", "" };
						abbreviatedMonthNames = new string[] { "\u0458\u0430\u043D", "\u0444\u0435\u0432", "\u043C\u0430\u0440", "\u0430\u043F\u0440", "\u043C\u0430\u0458", "\u0458\u0443\u043D", "\u0458\u0443\u043B", "\u0430\u0432\u0433", "\u0441\u0435\u043F", "\u043E\u043A\u0442", "\u043D\u043E\u0435", "\u0434\u0435\u043A", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 1078:
					{
						amDesignator = "";
						pmDesignator = "nm";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "yyyy/MM/dd";
						longDatePattern = "dd MMMM yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dd MMMM yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "Son", "Maan", "Dins", "Woen", "Dond", "Vry", "Sat" };
						dayNames = new string[] { "Sondag", "Maandag", "Dinsdag", "Woensdag", "Donderdag", "Vrydag", "Saterdag" };
						monthNames = new string[] { "Januarie", "Februarie", "Maart", "April", "Mei", "Junie", "Julie", "Augustus", "September", "Oktober", "November", "Desember", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "Mei", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Des", "" };
						return;
					}

				case 1079:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "yyyy '\u10EC\u10DA\u10D8\u10E1' dd MM, dddd";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "dd MM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "yyyy '\u10EC\u10DA\u10D8\u10E1' dd MM, dddd H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u10D9\u10D5\u10D8\u10E0\u10D0", "\u10DD\u10E0\u10E8\u10D0\u10D1\u10D0\u10D7\u10D8", "\u10E1\u10D0\u10DB\u10E8\u10D0\u10D1\u10D0\u10D7\u10D8", "\u10DD\u10D7\u10EE\u10E8\u10D0\u10D1\u10D0\u10D7\u10D8", "\u10EE\u10E3\u10D7\u10E8\u10D0\u10D1\u10D0\u10D7\u10D8", "\u10DE\u10D0\u10E0\u10D0\u10E1\u10D9\u10D4\u10D5\u10D8", "\u10E8\u10D0\u10D1\u10D0\u10D7\u10D8" };
						dayNames = new string[] { "\u10D9\u10D5\u10D8\u10E0\u10D0", "\u10DD\u10E0\u10E8\u10D0\u10D1\u10D0\u10D7\u10D8", "\u10E1\u10D0\u10DB\u10E8\u10D0\u10D1\u10D0\u10D7\u10D8", "\u10DD\u10D7\u10EE\u10E8\u10D0\u10D1\u10D0\u10D7\u10D8", "\u10EE\u10E3\u10D7\u10E8\u10D0\u10D1\u10D0\u10D7\u10D8", "\u10DE\u10D0\u10E0\u10D0\u10E1\u10D9\u10D4\u10D5\u10D8", "\u10E8\u10D0\u10D1\u10D0\u10D7\u10D8" };
						monthNames = new string[] { "\u10D8\u10D0\u10DC\u10D5\u10D0\u10E0\u10D8", "\u10D7\u10D4\u10D1\u10D4\u10E0\u10D5\u10D0\u10DA\u10D8", "\u10DB\u10D0\u10E0\u10E2\u10D8", "\u10D0\u10DE\u10E0\u10D8\u10DA\u10D8", "\u10DB\u10D0\u10D8\u10E1\u10D8", "\u10D8\u10D5\u10DC\u10D8\u10E1\u10D8", "\u10D8\u10D5\u10DA\u10D8\u10E1\u10D8", "\u10D0\u10D2\u10D5\u10D8\u10E1\u10E2\u10DD", "\u10E1\u10D4\u10E5\u10E2\u10D4\u10DB\u10D1\u10D4\u10E0\u10D8", "\u10DD\u10E5\u10E2\u10DD\u10DB\u10D1\u10D4\u10E0\u10D8", "\u10DC\u10DD\u10D4\u10DB\u10D1\u10D4\u10E0\u10D8", "\u10D3\u10D4\u10D9\u10D4\u10DB\u10D1\u10D4\u10E0\u10D8", "" };
						abbreviatedMonthNames = new string[] { "\u10D8\u10D0\u10DC", "\u10D7\u10D4\u10D1", "\u10DB\u10D0\u10E0", "\u10D0\u10DE\u10E0", "\u10DB\u10D0\u10D8\u10E1", "\u10D8\u10D5\u10DC", "\u10D8\u10D5\u10DA", "\u10D0\u10D2\u10D5", "\u10E1\u10D4\u10E5", "\u10DD\u10E5\u10E2", "\u10DC\u10DD\u10D4\u10DB", "\u10D3\u10D4\u10D9", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1080:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "-";
						timeSeparator = ".";
						shortDatePattern = "dd-MM-yyyy";
						longDatePattern = "d. MMMM yyyy";
						shortTimePattern = "HH.mm";
						longTimePattern = "HH.mm.ss";
						monthDayPattern = "d. MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d. MMMM yyyy HH.mm.ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "sun", "m\xe1n", "t\xfds", "mik", "h\xf3s", "fr\xed", "leyg" };
						dayNames = new string[] { "sunnudagur", "m\xe1nadagur", "t\xfdsdagur", "mikudagur", "h\xf3sdagur", "fr\xedggjadagur", "leygardagur" };
						monthNames = new string[] { "januar", "februar", "mars", "apr\xedl", "mai", "juni", "juli", "august", "september", "oktober", "november", "desember", "" };
						abbreviatedMonthNames = new string[] { "jan", "feb", "mar", "apr", "mai", "jun", "jul", "aug", "sep", "okt", "nov", "des", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 1081:
					{
						amDesignator = "\u092A\u0942\u0930\u094D\u0935\u093E\u0939\u094D\u0928";
						pmDesignator = "\u0905\u092A\u0930\u093E\u0939\u094D\u0928";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "dd-MM-yyyy";
						longDatePattern = "dd MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u0930\u0935\u093F.", "\u0938\u094B\u092E.", "\u092E\u0902\u0917\u0932.", "\u092C\u0941\u0927.", "\u0917\u0941\u0930\u0941.", "\u0936\u0941\u0915\u094D\u0930.", "\u0936\u0928\u093F." };
						dayNames = new string[] { "\u0930\u0935\u093F\u0935\u093E\u0930", "\u0938\u094B\u092E\u0935\u093E\u0930", "\u092E\u0902\u0917\u0932\u0935\u093E\u0930", "\u092C\u0941\u0927\u0935\u093E\u0930", "\u0917\u0941\u0930\u0941\u0935\u093E\u0930", "\u0936\u0941\u0915\u094D\u0930\u0935\u093E\u0930", "\u0936\u0928\u093F\u0935\u093E\u0930" };
						monthNames = new string[] { "\u091C\u0928\u0935\u0930\u0940", "\u092B\u0930\u0935\u0930\u0940", "\u092E\u093E\u0930\u094D\u091A", "\u0905\u092A\u094D\u0930\u0948\u0932", "\u092E\u0908", "\u091C\u0942\u0928", "\u091C\u0941\u0932\u093E\u0908", "\u0905\u0917\u0938\u094D\u0924", "\u0938\u093F\u0924\u092E\u094D\u092C\u0930", "\u0905\u0915\u094D\u0924\u0942\u092C\u0930", "\u0928\u0935\u092E\u094D\u092C\u0930", "\u0926\u093F\u0938\u092E\u094D\u092C\u0930", "" };
						abbreviatedMonthNames = new string[] { "\u091C\u0928\u0935\u0930\u0940", "\u092B\u0930\u0935\u0930\u0940", "\u092E\u093E\u0930\u094D\u091A", "\u0905\u092A\u094D\u0930\u0948\u0932", "\u092E\u0908", "\u091C\u0942\u0928", "\u091C\u0941\u0932\u093E\u0908", "\u0905\u0917\u0938\u094D\u0924", "\u0938\u093F\u0924\u092E\u094D\u092C\u0930", "\u0905\u0915\u094D\u0924\u0942\u092C\u0930", "\u0928\u0935\u092E\u094D\u092C\u0930", "\u0926\u093F\u0938\u092E\u094D\u092C\u0930", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1086:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dd MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "Ahad", "Isnin", "Sel", "Rabu", "Khamis", "Jumaat", "Sabtu" };
						dayNames = new string[] { "Ahad", "Isnin", "Selasa", "Rabu", "Khamis", "Jumaat", "Sabtu" };
						monthNames = new string[] { "Januari", "Februari", "Mac", "April", "Mei", "Jun", "Julai", "Ogos", "September", "Oktober", "November", "Disember", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mac", "Apr", "Mei", "Jun", "Jul", "Ogos", "Sept", "Okt", "Nov", "Dis", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1087:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "d MMMM yyyy '\u0436.'";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "d MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d MMMM yyyy '\u0436.' H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u0416\u043A", "\u0414\u0441", "\u0421\u0441", "\u0421\u0440", "\u0411\u0441", "\u0416\u043C", "\u0421\u043D" };
						dayNames = new string[] { "\u0416\u0435\u043A\u0441\u0435\u043D\u0431\u0456", "\u0414\u04AF\u0439\u0441\u0435\u043D\u0431\u0456", "\u0421\u0435\u0439\u0441\u0435\u043D\u0431\u0456", "\u0421\u04D9\u0440\u0441\u0435\u043D\u0431\u0456", "\u0411\u0435\u0439\u0441\u0435\u043D\u0431\u0456", "\u0416\u04B1\u043C\u0430", "\u0421\u0435\u043D\u0431\u0456" };
						monthNames = new string[] { "\u049B\u0430\u04A3\u0442\u0430\u0440", "\u0430\u049B\u043F\u0430\u043D", "\u043D\u0430\u0443\u0440\u044B\u0437", "\u0441\u04D9\u0443\u0456\u0440", "\u043C\u0430\u043C\u044B\u0440", "\u043C\u0430\u0443\u0441\u044B\u043C", "\u0448\u0456\u043B\u0434\u0435", "\u0442\u0430\u043C\u044B\u0437", "\u049B\u044B\u0440\u043A\u04AF\u0439\u0435\u043A", "\u049B\u0430\u0437\u0430\u043D", "\u049B\u0430\u0440\u0430\u0448\u0430", "\u0436\u0435\u043B\u0442\u043E\u049B\u0441\u0430\u043D", "" };
						abbreviatedMonthNames = new string[] { "\u049A\u0430\u04A3", "\u0410\u049B\u043F", "\u041D\u0430\u0443", "\u0421\u04D9\u0443", "\u041C\u0430\u043C", "\u041C\u0430\u0443", "\u0428\u0456\u043B", "\u0422\u0430\u043C", "\u049A\u044B\u0440", "\u049A\u0430\u0437", "\u049A\u0430\u0440", "\u0416\u0435\u043B", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1088:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yy";
						longDatePattern = "d'-'MMMM yyyy'-\u0436.'";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "d MMMM";
						yearMonthPattern = "MMMM yyyy'-\u0436.'";
						fullDateTimePattern = "d'-'MMMM yyyy'-\u0436.' H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u0416\u0448", "\u0414\u0448", "\u0428\u0448", "\u0428\u0440", "\u0411\u0448", "\u0416\u043C", "\u0418\u0448" };
						dayNames = new string[] { "\u0416\u0435\u043A\u0448\u0435\u043C\u0431\u0438", "\u0414\u04AF\u0439\u0448\u04E9\u043C\u0431\u04AF", "\u0428\u0435\u0439\u0448\u0435\u043C\u0431\u0438", "\u0428\u0430\u0440\u0448\u0435\u043C\u0431\u0438", "\u0411\u0435\u0439\u0448\u0435\u043C\u0431\u0438", "\u0416\u0443\u043C\u0430", "\u0418\u0448\u0435\u043C\u0431\u0438" };
						monthNames = new string[] { "\u042F\u043D\u0432\u0430\u0440\u044C", "\u0424\u0435\u0432\u0440\u0430\u043B\u044C", "\u041C\u0430\u0440\u0442", "\u0410\u043F\u0440\u0435\u043B\u044C", "\u041C\u0430\u0439", "\u0418\u044E\u043D\u044C", "\u0418\u044E\u043B\u044C", "\u0410\u0432\u0433\u0443\u0441\u0442", "\u0421\u0435\u043D\u0442\u044F\u0431\u0440\u044C", "\u041E\u043A\u0442\u044F\u0431\u0440\u044C", "\u041D\u043E\u044F\u0431\u0440\u044C", "\u0414\u0435\u043A\u0430\u0431\u0440\u044C", "" };
						abbreviatedMonthNames = new string[] { "\u042F\u043D\u0432", "\u0424\u0435\u0432", "\u041C\u0430\u0440", "\u0410\u043F\u0440", "\u041C\u0430\u0439", "\u0418\u044E\u043D", "\u0418\u044E\u043B", "\u0410\u0432\u0433", "\u0421\u0435\u043D", "\u041E\u043A\u0442", "\u041D\u043E\u044F", "\u0414\u0435\u043A", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1089:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "M/d/yyyy";
						longDatePattern = "dddd, MMMM dd, yyyy";
						shortTimePattern = "h:mm tt";
						longTimePattern = "h:mm:ss tt";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dddd, MMMM dd, yyyy h:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
						monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						return;
					}

				case 1091:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM yyyy";
						longDatePattern = "yyyy 'yil' d-MMMM";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "d-MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "yyyy 'yil' d-MMMM HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "yak.", "dsh.", "sesh.", "chr.", "psh.", "jm.", "sh." };
						dayNames = new string[] { "yakshanba", "dushanba", "seshanba", "chorshanba", "payshanba", "juma", "shanba" };
						monthNames = new string[] { "yanvar", "fevral", "mart", "aprel", "may", "iyun", "iyul", "avgust", "sentyabr", "oktyabr", "noyabr", "dekabr", "" };
						abbreviatedMonthNames = new string[] { "yanvar", "fevral", "mart", "aprel", "may", "iyun", "iyul", "avgust", "sentyabr", "oktyabr", "noyabr", "dekabr", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1092:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "d MMMM yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "d MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u042F\u043A\u0448", "\u0414\u04AF\u0448", "\u0421\u0438\u0448", "\u0427\u04D9\u0440\u0448", "\u041F\u04D9\u043D\u0497", "\u0496\u043E\u043C", "\u0428\u0438\u043C" };
						dayNames = new string[] { "\u042F\u043A\u0448\u04D9\u043C\u0431\u0435", "\u0414\u04AF\u0448\u04D9\u043C\u0431\u0435", "\u0421\u0438\u0448\u04D9\u043C\u0431\u0435", "\u0427\u04D9\u0440\u0448\u04D9\u043C\u0431\u0435", "\u041F\u04D9\u043D\u0497\u0435\u0448\u04D9\u043C\u0431\u0435", "\u0496\u043E\u043C\u0433\u0430", "\u0428\u0438\u043C\u0431\u04D9" };
						monthNames = new string[] { "\u0413\u044B\u0439\u043D\u0432\u0430\u0440\u044C", "\u0424\u0435\u0432\u0440\u0430\u043B\u044C", "\u041C\u0430\u0440\u0442", "\u0410\u043F\u0440\u0435\u043B\u044C", "\u041C\u0430\u0439", "\u0418\u044E\u043D\u044C", "\u0418\u044E\u043B\u044C", "\u0410\u0432\u0433\u0443\u0441\u0442", "\u0421\u0435\u043D\u0442\u044F\u0431\u0440\u044C", "\u041E\u043A\u0442\u044F\u0431\u0440\u044C", "\u041D\u043E\u044F\u0431\u0440\u044C", "\u0414\u0435\u043A\u0430\u0431\u0440\u044C", "" };
						abbreviatedMonthNames = new string[] { "\u0413\u044B\u0439\u043D\u0432", "\u0424\u0435\u0432", "\u041C\u0430\u0440", "\u0410\u043F\u0440", "\u041C\u0430\u0439", "\u0418\u044E\u043D", "\u0418\u044E\u043B", "\u0410\u0432\u0433", "\u0421\u0435\u043D", "\u041E\u043A\u0442", "\u041D\u043E\u044F", "\u0414\u0435\u043A", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1094:
					{
						amDesignator = "\u0A38\u0A35\u0A47\u0A30\u0A47";
						pmDesignator = "\u0A36\u0A3E\u0A2E";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "dd-MM-yy";
						longDatePattern = "dd MMMM yyyy dddd";
						shortTimePattern = "tt hh:mm";
						longTimePattern = "tt hh:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM yyyy dddd tt hh:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u0A10\u0A24.", "\u0A38\u0A4B\u0A2E.", "\u0A2E\u0A70\u0A17\u0A32.", "\u0A2C\u0A41\u0A27.", "\u0A35\u0A40\u0A30.", "\u0A36\u0A41\u0A15\u0A30.", "\u0A36\u0A28\u0A40." };
						dayNames = new string[] { "\u0A10\u0A24\u0A35\u0A3E\u0A30", "\u0A38\u0A4B\u0A2E\u0A35\u0A3E\u0A30", "\u0A2E\u0A70\u0A17\u0A32\u0A35\u0A3E\u0A30", "\u0A2C\u0A41\u0A27\u0A35\u0A3E\u0A30", "\u0A35\u0A40\u0A30\u0A35\u0A3E\u0A30", "\u0A36\u0A41\u0A71\u0A15\u0A30\u0A35\u0A3E\u0A30", "\u0A36\u0A28\u0A40\u0A1A\u0A30\u0A35\u0A3E\u0A30" };
						monthNames = new string[] { "\u0A1C\u0A28\u0A35\u0A30\u0A40", "\u0A5E\u0A30\u0A35\u0A30\u0A40", "\u0A2E\u0A3E\u0A30\u0A1A", "\u0A05\u0A2A\u0A4D\u0A30\u0A48\u0A32", "\u0A2E\u0A08", "\u0A1C\u0A42\u0A28", "\u0A1C\u0A41\u0A32\u0A3E\u0A08", "\u0A05\u0A17\u0A38\u0A24", "\u0A38\u0A24\u0A70\u0A2C\u0A30", "\u0A05\u0A15\u0A24\u0A42\u0A2C\u0A30", "\u0A28\u0A35\u0A70\u0A2C\u0A30", "\u0A26\u0A38\u0A70\u0A2C\u0A30", "" };
						abbreviatedMonthNames = new string[] { "\u0A1C\u0A28\u0A35\u0A30\u0A40", "\u0A5E\u0A30\u0A35\u0A30\u0A40", "\u0A2E\u0A3E\u0A30\u0A1A", "\u0A05\u0A2A\u0A4D\u0A30\u0A48\u0A32", "\u0A2E\u0A08", "\u0A1C\u0A42\u0A28", "\u0A1C\u0A41\u0A32\u0A3E\u0A08", "\u0A05\u0A17\u0A38\u0A24", "\u0A38\u0A24\u0A70\u0A2C\u0A30", "\u0A05\u0A15\u0A24\u0A42\u0A2C\u0A30", "\u0A28\u0A35\u0A70\u0A2C\u0A30", "\u0A26\u0A38\u0A70\u0A2C\u0A30", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1095:
					{
						amDesignator = "\u0AAA\u0AC2\u0AB0\u0ACD\u0AB5\xa0\u0AAE\u0AA7\u0ACD\u0AAF\u0ABE\u0AB9\u0ACD\u0AA8";
						pmDesignator = "\u0A89\u0AA4\u0ACD\u0AA4\u0AB0\xa0\u0AAE\u0AA7\u0ACD\u0AAF\u0ABE\u0AB9\u0ACD\u0AA8";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "dd-MM-yy";
						longDatePattern = "dd MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u0AB0\u0AB5\u0ABF", "\u0AB8\u0ACB\u0AAE", "\u0AAE\u0A82\u0A97\u0AB3", "\u0AAC\u0AC1\u0AA7", "\u0A97\u0AC1\u0AB0\u0AC1", "\u0AB6\u0AC1\u0A95\u0ACD\u0AB0", "\u0AB6\u0AA8\u0ABF" };
						dayNames = new string[] { "\u0AB0\u0AB5\u0ABF\u0AB5\u0ABE\u0AB0", "\u0AB8\u0ACB\u0AAE\u0AB5\u0ABE\u0AB0", "\u0AAE\u0A82\u0A97\u0AB3\u0AB5\u0ABE\u0AB0", "\u0AAC\u0AC1\u0AA7\u0AB5\u0ABE\u0AB0", "\u0A97\u0AC1\u0AB0\u0AC1\u0AB5\u0ABE\u0AB0", "\u0AB6\u0AC1\u0A95\u0ACD\u0AB0\u0AB5\u0ABE\u0AB0", "\u0AB6\u0AA8\u0ABF\u0AB5\u0ABE\u0AB0" };
						monthNames = new string[] { "\u0A9C\u0ABE\u0AA8\u0ACD\u0AAF\u0AC1\u0A86\u0AB0\u0AC0", "\u0AAB\u0AC7\u0AAC\u0ACD\u0AB0\u0AC1\u0A86\u0AB0\u0AC0", "\u0AAE\u0ABE\u0AB0\u0ACD\u0A9A", "\u0A8F\u0AAA\u0ACD\u0AB0\u0ABF\u0AB2", "\u0AAE\u0AC7", "\u0A9C\u0AC2\u0AA8", "\u0A9C\u0AC1\u0AB2\u0ABE\u0A88", "\u0A91\u0A97\u0AB8\u0ACD\u0A9F", "\u0AB8\u0AAA\u0ACD\u0A9F\u0AC7\u0AAE\u0ACD\u0AAC\u0AB0", "\u0A91\u0A95\u0ACD\u0A9F\u0ACD\u0AAC\u0AB0", "\u0AA8\u0AB5\u0AC7\u0AAE\u0ACD\u0AAC\u0AB0", "\u0AA1\u0ABF\u0AB8\u0AC7\u0AAE\u0ACD\u0AAC\u0AB0", "" };
						abbreviatedMonthNames = new string[] { "\u0A9C\u0ABE\u0AA8\u0ACD\u0AAF\u0AC1", "\u0AAB\u0AC7\u0AAC\u0ACD\u0AB0\u0AC1", "\u0AAE\u0ABE\u0AB0\u0ACD\u0A9A", "\u0A8F\u0AAA\u0ACD\u0AB0\u0ABF\u0AB2", "\u0AAE\u0AC7", "\u0A9C\u0AC2\u0AA8", "\u0A9C\u0AC1\u0AB2\u0ABE\u0A88", "\u0A91\u0A97\u0AB8\u0ACD\u0A9F", "\u0AB8\u0AAA\u0ACD\u0A9F\u0AC7", "\u0A91\u0A95\u0ACD\u0A9F\u0ACB", "\u0AA8\u0AB5\u0AC7", "\u0AA1\u0ABF\u0AB8\u0AC7", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1097:
					{
						amDesignator = "\u0B95\u0BBE\u0BB2\u0BC8";
						pmDesignator = "\u0BAE\u0BBE\u0BB2\u0BC8";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "dd-MM-yyyy";
						longDatePattern = "dd MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dd MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u0B9E\u0BBE", "\u0BA4\u0BBF", "\u0B9A\u0BC6", "\u0BAA\u0BC1", "\u0BB5\u0BBF", "\u0BB5\u0BC6", "\u0B9A" };
						dayNames = new string[] { "\u0B9E\u0BBE\u0BAF\u0BBF\u0BB1\u0BC1", "\u0BA4\u0BBF\u0B99\u0BCD\u0B95\u0BB3\u0BCD", "\u0B9A\u0BC6\u0BB5\u0BCD\u0BB5\u0BBE\u0BAF\u0BCD", "\u0BAA\u0BC1\u0BA4\u0BA9\u0BCD", "\u0BB5\u0BBF\u0BAF\u0BBE\u0BB4\u0BA9\u0BCD", "\u0BB5\u0BC6\u0BB3\u0BCD\u0BB3\u0BBF", "\u0B9A\u0BA9\u0BBF" };
						monthNames = new string[] { "\u0B9C\u0BA9\u0BB5\u0BB0\u0BBF", "\u0BAA\u0BC6\u0BAA\u0BCD\u0BB0\u0BB5\u0BB0\u0BBF", "\u0BAE\u0BBE\u0BB0\u0BCD\u0B9A\u0BCD", "\u0B8F\u0BAA\u0BCD\u0BB0\u0BB2\u0BCD", "\u0BAE\u0BC7", "\u0B9C\u0BC2\u0BA9\u0BCD", "\u0B9C\u0BC2\u0BB2\u0BC8", "\u0B86\u0B95\u0BB8\u0BCD\u0B9F\u0BCD", "\u0B9A\u0BC6\u0BAA\u0BCD\u0B9F\u0BAE\u0BCD\u0BAA\u0BB0\u0BCD", "\u0B85\u0B95\u0BCD\u0B9F\u0BCB\u0BAA\u0BB0\u0BCD", "\u0BA8\u0BB5\u0BAE\u0BCD\u0BAA\u0BB0\u0BCD", "\u0B9F\u0BBF\u0B9A\u0BAE\u0BCD\u0BAA\u0BB0\u0BCD", "" };
						abbreviatedMonthNames = new string[] { "\u0B9C\u0BA9.", "\u0BAA\u0BC6\u0BAA\u0BCD.", "\u0BAE\u0BBE\u0BB0\u0BCD.", "\u0B8F\u0BAA\u0BCD.", "\u0BAE\u0BC7", "\u0B9C\u0BC2\u0BA9\u0BCD", "\u0B9C\u0BC2\u0BB2\u0BC8", "\u0B86\u0B95.", "\u0B9A\u0BC6\u0BAA\u0BCD.", "\u0B85\u0B95\u0BCD.", "\u0BA8\u0BB5.", "\u0B9F\u0BBF\u0B9A.", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1098:
					{
						amDesignator = "\u0C2A\u0C42\u0C30\u0C4D\u0C35\u0C3E\u0C39\u0C4D\u0C28";
						pmDesignator = "\u0C05\u0C2A\u0C30\u0C3E\u0C39\u0C4D\u0C28";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "dd-MM-yy";
						longDatePattern = "dd MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u0C06\u0C26\u0C3F.", "\u0C38\u0C4B\u0C2E.", "\u0C2E\u0C02\u0C17\u0C33.", "\u0C2C\u0C41\u0C27.", "\u0C17\u0C41\u0C30\u0C41.", "\u0C36\u0C41\u0C15\u0C4D\u0C30.", "\u0C36\u0C28\u0C3F." };
						dayNames = new string[] { "\u0C06\u0C26\u0C3F\u0C35\u0C3E\u0C30\u0C02", "\u0C38\u0C4B\u0C2E\u0C35\u0C3E\u0C30\u0C02", "\u0C2E\u0C02\u0C17\u0C33\u0C35\u0C3E\u0C30\u0C02", "\u0C2C\u0C41\u0C27\u0C35\u0C3E\u0C30\u0C02", "\u0C17\u0C41\u0C30\u0C41\u0C35\u0C3E\u0C30\u0C02", "\u0C36\u0C41\u0C15\u0C4D\u0C30\u0C35\u0C3E\u0C30\u0C02", "\u0C36\u0C28\u0C3F\u0C35\u0C3E\u0C30\u0C02" };
						monthNames = new string[] { "\u0C1C\u0C28\u0C35\u0C30\u0C3F", "\u0C2B\u0C3F\u0C2C\u0C4D\u0C30\u0C35\u0C30\u0C3F", "\u0C2E\u0C3E\u0C30\u0C4D\u0C1A\u0C3F", "\u0C0F\u0C2A\u0C4D\u0C30\u0C3F\u0C32\u0C4D", "\u0C2E\u0C47", "\u0C1C\u0C42\u0C28\u0C4D", "\u0C1C\u0C42\u0C32\u0C48", "\u0C06\u0C17\u0C38\u0C4D\u0C1F\u0C41", "\u0C38\u0C46\u0C2A\u0C4D\u0C1F\u0C46\u0C02\u0C2C\u0C30\u0C4D", "\u0C05\u0C15\u0C4D\u0C1F\u0C4B\u0C2C\u0C30\u0C4D", "\u0C28\u0C35\u0C02\u0C2C\u0C30\u0C4D", "\u0C21\u0C3F\u0C38\u0C46\u0C02\u0C2C\u0C30\u0C4D", "" };
						abbreviatedMonthNames = new string[] { "\u0C1C\u0C28\u0C35\u0C30\u0C3F", "\u0C2B\u0C3F\u0C2C\u0C4D\u0C30\u0C35\u0C30\u0C3F", "\u0C2E\u0C3E\u0C30\u0C4D\u0C1A\u0C3F", "\u0C0F\u0C2A\u0C4D\u0C30\u0C3F\u0C32\u0C4D", "\u0C2E\u0C47", "\u0C1C\u0C42\u0C28\u0C4D", "\u0C1C\u0C42\u0C32\u0C48", "\u0C06\u0C17\u0C38\u0C4D\u0C1F\u0C41", "\u0C38\u0C46\u0C2A\u0C4D\u0C1F\u0C46\u0C02\u0C2C\u0C30\u0C4D", "\u0C05\u0C15\u0C4D\u0C1F\u0C4B\u0C2C\u0C30\u0C4D", "\u0C28\u0C35\u0C02\u0C2C\u0C30\u0C4D", "\u0C21\u0C3F\u0C38\u0C46\u0C02\u0C2C\u0C30\u0C4D", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1099:
					{
						amDesignator = "\u0CAA\u0CC2\u0CB0\u0CCD\u0CB5\u0CBE\u0CB9\u0CCD\u0CA8";
						pmDesignator = "\u0C85\u0CAA\u0CB0\u0CBE\u0CB9\u0CCD\u0CA8";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "dd-MM-yy";
						longDatePattern = "dd MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u0CAD\u0CBE\u0CA8\u0CC1.", "\u0CB8\u0CCB\u0CAE.", "\u0CAE\u0C82\u0C97\u0CB3.", "\u0CAC\u0CC1\u0CA7.", "\u0C97\u0CC1\u0CB0\u0CC1.", "\u0CB6\u0CC1\u0C95\u0CCD\u0CB0.", "\u0CB6\u0CA8\u0CBF." };
						dayNames = new string[] { "\u0CAD\u0CBE\u0CA8\u0CC1\u0CB5\u0CBE\u0CB0", "\u0CB8\u0CCB\u0CAE\u0CB5\u0CBE\u0CB0", "\u0CAE\u0C82\u0C97\u0CB3\u0CB5\u0CBE\u0CB0", "\u0CAC\u0CC1\u0CA7\u0CB5\u0CBE\u0CB0", "\u0C97\u0CC1\u0CB0\u0CC1\u0CB5\u0CBE\u0CB0", "\u0CB6\u0CC1\u0C95\u0CCD\u0CB0\u0CB5\u0CBE\u0CB0", "\u0CB6\u0CA8\u0CBF\u0CB5\u0CBE\u0CB0" };
						monthNames = new string[] { "\u0C9C\u0CA8\u0CB5\u0CB0\u0CBF", "\u0CAB\u0CC6\u0CAC\u0CCD\u0CB0\u0CB5\u0CB0\u0CBF", "\u0CAE\u0CBE\u0CB0\u0CCD\u0C9A\u0CCD", "\u0C8E\u0CAA\u0CCD\u0CB0\u0CBF\u0CB2\u0CCD", "\u0CAE\u0CC7", "\u0C9C\u0CC2\u0CA8\u0CCD", "\u0C9C\u0CC1\u0CB2\u0CC8", "\u0C86\u0C97\u0CB8\u0CCD\u0C9F\u0CCD", "\u0CB8\u0CC6\u0CAA\u0CCD\u0C9F\u0C82\u0CAC\u0CB0\u0CCD", "\u0C85\u0C95\u0CCD\u0C9F\u0CCB\u0CAC\u0CB0\u0CCD", "\u0CA8\u0CB5\u0CC6\u0C82\u0CAC\u0CB0\u0CCD", "\u0CA1\u0CBF\u0CB8\u0CC6\u0C82\u0CAC\u0CB0\u0CCD", "" };
						abbreviatedMonthNames = new string[] { "\u0C9C\u0CA8\u0CB5\u0CB0\u0CBF", "\u0CAB\u0CC6\u0CAC\u0CCD\u0CB0\u0CB5\u0CB0\u0CBF", "\u0CAE\u0CBE\u0CB0\u0CCD\u0C9A\u0CCD", "\u0C8E\u0CAA\u0CCD\u0CB0\u0CBF\u0CB2\u0CCD", "\u0CAE\u0CC7", "\u0C9C\u0CC2\u0CA8\u0CCD", "\u0C9C\u0CC1\u0CB2\u0CC8", "\u0C86\u0C97\u0CB8\u0CCD\u0C9F\u0CCD", "\u0CB8\u0CC6\u0CAA\u0CCD\u0C9F\u0C82\u0CAC\u0CB0\u0CCD", "\u0C85\u0C95\u0CCD\u0C9F\u0CCB\u0CAC\u0CB0\u0CCD", "\u0CA8\u0CB5\u0CC6\u0C82\u0CAC\u0CB0\u0CCD", "\u0CA1\u0CBF\u0CB8\u0CC6\u0C82\u0CAC\u0CB0\u0CCD", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1102:
					{
						amDesignator = "\u092E.\u092A\u0942.";
						pmDesignator = "\u092E.\u0928\u0902.";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "dd-MM-yyyy";
						longDatePattern = "dd MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u0930\u0935\u093F.", "\u0938\u094B\u092E.", "\u092E\u0902\u0917\u0933.", "\u092C\u0941\u0927.", "\u0917\u0941\u0930\u0941.", "\u0936\u0941\u0915\u094D\u0930.", "\u0936\u0928\u093F." };
						dayNames = new string[] { "\u0930\u0935\u093F\u0935\u093E\u0930", "\u0938\u094B\u092E\u0935\u093E\u0930", "\u092E\u0902\u0917\u0933\u0935\u093E\u0930", "\u092C\u0941\u0927\u0935\u093E\u0930", "\u0917\u0941\u0930\u0941\u0935\u093E\u0930", "\u0936\u0941\u0915\u094D\u0930\u0935\u093E\u0930", "\u0936\u0928\u093F\u0935\u093E\u0930" };
						monthNames = new string[] { "\u091C\u093E\u0928\u0947\u0935\u093E\u0930\u0940", "\u092B\u0947\u092C\u094D\u0930\u0941\u0935\u093E\u0930\u0940", "\u092E\u093E\u0930\u094D\u091A", "\u090F\u092A\u094D\u0930\u093F\u0932", "\u092E\u0947", "\u091C\u0942\u0928", "\u091C\u0941\u0932\u0948", "\u0911\u0917\u0938\u094D\u091F", "\u0938\u092A\u094D\u091F\u0947\u0902\u092C\u0930", "\u0911\u0915\u094D\u091F\u094B\u092C\u0930", "\u0928\u094B\u0935\u094D\u0939\u0947\u0902\u092C\u0930", "\u0921\u093F\u0938\u0947\u0902\u092C\u0930", "" };
						abbreviatedMonthNames = new string[] { "\u091C\u093E\u0928\u0947.", "\u092B\u0947\u092C\u094D\u0930\u0941.", "\u092E\u093E\u0930\u094D\u091A", "\u090F\u092A\u094D\u0930\u093F\u0932", "\u092E\u0947", "\u091C\u0942\u0928", "\u091C\u0941\u0932\u0948", "\u0911\u0917\u0938\u094D\u091F", "\u0938\u092A\u094D\u091F\u0947\u0902.", "\u0911\u0915\u094D\u091F\u094B.", "\u0928\u094B\u0935\u094D\u0939\u0947\u0902.", "\u0921\u093F\u0938\u0947\u0902.", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1103:
					{
						amDesignator = "\u092A\u0942\u0930\u094D\u0935\u093E\u0939\u094D\u0928";
						pmDesignator = "\u0905\u092A\u0930\u093E\u0939\u094D\u0928";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "dd-MM-yyyy";
						longDatePattern = "dd MMMM yyyy dddd";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM yyyy dddd HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "\u0930\u0935\u093F\u0935\u093E\u0938\u0930\u0903", "\u0938\u094B\u092E\u0935\u093E\u0938\u0930\u0903", "\u092E\u0919\u094D\u0917\u0932\u0935\u093E\u0938\u0930\u0903", "\u092C\u0941\u0927\u0935\u093E\u0938\u0930\u0903", "\u0917\u0941\u0930\u0941\u0935\u093E\u0938\u0930\u0903", "\u0936\u0941\u0915\u094D\u0930\u0935\u093E\u0938\u0930\u0903", "\u0936\u0928\u093F\u0935\u093E\u0938\u0930\u0903" };
						dayNames = new string[] { "\u0930\u0935\u093F\u0935\u093E\u0938\u0930\u0903", "\u0938\u094B\u092E\u0935\u093E\u0938\u0930\u0903", "\u092E\u0919\u094D\u0917\u0932\u0935\u093E\u0938\u0930\u0903", "\u092C\u0941\u0927\u0935\u093E\u0938\u0930\u0903", "\u0917\u0941\u0930\u0941\u0935\u093E\u0938\u0930\u0903", "\u0936\u0941\u0915\u094D\u0930\u0935\u093E\u0938\u0930\u0903", "\u0936\u0928\u093F\u0935\u093E\u0938\u0930\u0903" };
						monthNames = new string[] { "\u091C\u0928\u0935\u0930\u0940", "\u092B\u0930\u0935\u0930\u0940", "\u092E\u093E\u0930\u094D\u091A", "\u0905\u092A\u094D\u0930\u0948\u0932", "\u092E\u0908", "\u091C\u0942\u0928", "\u091C\u0941\u0932\u093E\u0908", "\u0905\u0917\u0938\u094D\u0924", "\u0938\u093F\u0924\u092E\u094D\u092C\u0930", "\u0905\u0915\u094D\u0924\u0942\u092C\u0930", "\u0928\u0935\u092E\u094D\u092C\u0930", "\u0926\u093F\u0938\u092E\u094D\u092C\u0930", "" };
						abbreviatedMonthNames = new string[] { "\u091C\u0928\u0935\u0930\u0940", "\u092B\u0930\u0935\u0930\u0940", "\u092E\u093E\u0930\u094D\u091A", "\u0905\u092A\u094D\u0930\u0948\u0932", "\u092E\u0908", "\u091C\u0942\u0928", "\u091C\u0941\u0932\u093E\u0908", "\u0905\u0917\u0938\u094D\u0924", "\u0938\u093F\u0924\u092E\u094D\u092C\u0930", "\u0905\u0915\u094D\u0924\u0942\u092C\u0930", "\u0928\u0935\u092E\u094D\u092C\u0930", "\u0926\u093F\u0938\u092E\u094D\u092C\u0930", "" };
						return;
					}

				case 1104:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "yy.MM.dd";
						longDatePattern = "yyyy '\u043E\u043D\u044B' MMMM d";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "d MMMM";
						yearMonthPattern = "yyyy '\u043E\u043D' MMMM";
						fullDateTimePattern = "yyyy '\u043E\u043D\u044B' MMMM d H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u041D\u044F", "\u0414\u0430", "\u041C\u044F", "\u041B\u0445", "\u041F\u04AF", "\u0411\u0430", "\u0411\u044F" };
						dayNames = new string[] { "\u041D\u044F\u043C", "\u0414\u0430\u0432\u0430\u0430", "\u041C\u044F\u0433\u043C\u0430\u0440", "\u041B\u0445\u0430\u0433\u0432\u0430", "\u041F\u04AF\u0440\u044D\u0432", "\u0411\u0430\u0430\u0441\u0430\u043D", "\u0411\u044F\u043C\u0431\u0430" };
						monthNames = new string[] { "1\xa0\u0434\u04AF\u0433\u044D\u044D\u0440\xa0\u0441\u0430\u0440", "2\xa0\u0434\u0443\u0433\u0430\u0430\u0440\xa0\u0441\u0430\u0440", "3\xa0\u0434\u0443\u0433\u0430\u0430\u0440\xa0\u0441\u0430\u0440", "4\xa0\u0434\u04AF\u0433\u044D\u044D\u0440\xa0\u0441\u0430\u0440", "5\xa0\u0434\u0443\u0433\u0430\u0430\u0440\xa0\u0441\u0430\u0440", "6\xa0\u0434\u0443\u0433\u0430\u0430\u0440\xa0\u0441\u0430\u0440", "7\xa0\u0434\u0443\u0433\u0430\u0430\u0440\xa0\u0441\u0430\u0440", "8\xa0\u0434\u0443\u0433\u0430\u0430\u0440\xa0\u0441\u0430\u0440", "9\xa0\u0434\u04AF\u0433\u044D\u044D\u0440\xa0\u0441\u0430\u0440", "10\xa0\u0434\u0443\u0433\u0430\u0430\u0440\xa0\u0441\u0430\u0440", "11\xa0\u0434\u04AF\u0433\u044D\u044D\u0440\xa0\u0441\u0430\u0440", "12\xa0\u0434\u0443\u0433\u0430\u0430\u0440\xa0\u0441\u0430\u0440", "" };
						abbreviatedMonthNames = new string[] { "I", "II", "III", "IV", "V", "VI", "VII", "V\u0428", "IX", "X", "XI", "XII", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1110:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "dom", "luns", "mar", "m\xe9r", "xov", "ven", "sab" };
						dayNames = new string[] { "domingo", "luns", "martes", "m\xe9rcores", "xoves", "venres", "s\xe1bado" };
						monthNames = new string[] { "xaneiro", "febreiro", "marzo", "abril", "maio", "xu\xf1o", "xullo", "agosto", "setembro", "outubro", "novembro", "decembro", "" };
						abbreviatedMonthNames = new string[] { "xan", "feb", "mar", "abr", "maio", "xu\xf1", "xull", "ago", "set", "out", "nov", "dec", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1111:
					{
						amDesignator = "\u092E.\u092A\u0942.";
						pmDesignator = "\u092E.\u0928\u0902.";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "dd-MM-yyyy";
						longDatePattern = "dd MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u0906\u092F.", "\u0938\u094B\u092E.", "\u092E\u0902\u0917\u0933.", "\u092C\u0941\u0927.", "\u092C\u093F\u0930\u0947.", "\u0938\u0941\u0915\u094D\u0930.", "\u0936\u0947\u0928." };
						dayNames = new string[] { "\u0906\u092F\u0924\u093E\u0930", "\u0938\u094B\u092E\u093E\u0930", "\u092E\u0902\u0917\u0933\u093E\u0930", "\u092C\u0941\u0927\u0935\u093E\u0930", "\u092C\u093F\u0930\u0947\u0938\u094D\u0924\u093E\u0930", "\u0938\u0941\u0915\u094D\u0930\u093E\u0930", "\u0936\u0947\u0928\u0935\u093E\u0930" };
						monthNames = new string[] { "\u091C\u093E\u0928\u0947\u0935\u093E\u0930\u0940", "\u092B\u0947\u092C\u094D\u0930\u0941\u0935\u093E\u0930\u0940", "\u092E\u093E\u0930\u094D\u091A", "\u090F\u092A\u094D\u0930\u093F\u0932", "\u092E\u0947", "\u091C\u0942\u0928", "\u091C\u0941\u0932\u0948", "\u0911\u0917\u0938\u094D\u091F", "\u0938\u092A\u094D\u091F\u0947\u0902\u092C\u0930", "\u0911\u0915\u094D\u091F\u094B\u092C\u0930", "\u0928\u094B\u0935\u0947\u092E\u094D\u092C\u0930", "\u0921\u093F\u0938\u0947\u0902\u092C\u0930", "" };
						abbreviatedMonthNames = new string[] { "\u091C\u093E\u0928\u0947\u0935\u093E\u0930\u0940", "\u092B\u0947\u092C\u094D\u0930\u0941\u0935\u093E\u0930\u0940", "\u092E\u093E\u0930\u094D\u091A", "\u090F\u092A\u094D\u0930\u093F\u0932", "\u092E\u0947", "\u091C\u0942\u0928", "\u091C\u0941\u0932\u0948", "\u0911\u0917\u0938\u094D\u091F", "\u0938\u092A\u094D\u091F\u0947\u0902\u092C\u0930", "\u0911\u0915\u094D\u091F\u094B\u092C\u0930", "\u0928\u094B\u0935\u0947\u092E\u094D\u092C\u0930", "\u0921\u093F\u0938\u0947\u0902\u092C\u0930", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1114:
					{
						amDesignator = "\u0729.\u071B";
						pmDesignator = "\u0712.\u071B";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM, yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM, yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 6;
						abbreviatedDayNames = new string[] { "\u070F\u0710\xa0\u070F\u0712\u072B", "\u070F\u0712\xa0\u070F\u0712\u072B", "\u070F\u0713\xa0\u070F\u0712\u072B", "\u070F\u0715\xa0\u070F\u0712\u072B", "\u070F\u0717\xa0\u070F\u0712\u072B", "\u070F\u0725\u072A\u0718\u0712", "\u070F\u072B\u0712" };
						dayNames = new string[] { "\u071A\u0715\xa0\u0712\u072B\u0712\u0710", "\u072C\u072A\u071D\u0722\xa0\u0712\u072B\u0712\u0710", "\u072C\u0720\u072C\u0710\xa0\u0712\u072B\u0712\u0710", "\u0710\u072A\u0712\u0725\u0710\xa0\u0712\u072B\u0712\u0710", "\u071A\u0721\u072B\u0710\xa0\u0712\u072B\u0712\u0710", "\u0725\u072A\u0718\u0712\u072C\u0710", "\u072B\u0712\u072C\u0710" };
						monthNames = new string[] { "\u071F\u0722\u0718\u0722\xa0\u0710\u071A\u072A\u071D", "\u072B\u0712\u071B", "\u0710\u0715\u072A", "\u0722\u071D\u0723\u0722", "\u0710\u071D\u072A", "\u071A\u0719\u071D\u072A\u0722", "\u072C\u0721\u0718\u0719", "\u0710\u0712", "\u0710\u071D\u0720\u0718\u0720", "\u072C\u072B\u072A\u071D\xa0\u0729\u0715\u071D\u0721", "\u072C\u072B\u072A\u071D\xa0\u0710\u071A\u072A\u071D", "\u071F\u0722\u0718\u0722\xa0\u0729\u0715\u071D\u0721", "" };
						abbreviatedMonthNames = new string[] { "\u070F\u071F\u0722\xa0\u070F\u0712", "\u072B\u0712\u071B", "\u0710\u0715\u072A", "\u0722\u071D\u0723\u0722", "\u0710\u071D\u072A", "\u071A\u0719\u071D\u072A\u0722", "\u072C\u0721\u0718\u0719", "\u0710\u0712", "\u0710\u071D\u0720\u0718\u0720", "\u070F\u072C\u072B\xa0\u070F\u0710", "\u070F\u072C\u072B\xa0\u070F\u0712", "\u070F\u071F\u0722\xa0\u070F\u0710", "" };
						firstDayOfWeek = (int)DayOfWeek.Saturday;
						return;
					}

				case 1125:
					{
						amDesignator = "\u0789\u0786";
						pmDesignator = "\u0789\u078A";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yy";
						longDatePattern = "dd/MMMM/yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd/MMMM/yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						dayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						monthNames = new string[] { "\u0645\u062D\u0631\u0645", "\u0635\u0641\u0631", "\u0631\u0628\u064A\u0639\xa0\u0627\u0644\u0623\u0648\u0644", "\u0631\u0628\u064A\u0639\xa0\u0627\u0644\u062B\u0627\u0646\u064A", "\u062C\u0645\u0627\u062F\u0649\xa0\u0627\u0644\u0623\u0648\u0644\u0649", "\u062C\u0645\u0627\u062F\u0649\xa0\u0627\u0644\u062B\u0627\u0646\u064A\u0629", "\u0631\u062C\u0628", "\u0634\u0639\u0628\u0627\u0646", "\u0631\u0645\u0636\u0627\u0646", "\u0634\u0648\u0627\u0644", "\u0630\u0648\xa0\u0627\u0644\u0642\u0639\u062F\u0629", "\u0630\u0648\xa0\u0627\u0644\u062D\u062C\u0629", "" };
						abbreviatedMonthNames = new string[] { "\u0645\u062D\u0631\u0645", "\u0635\u0641\u0631", "\u0631\u0628\u064A\u0639\xa0\u0627\u0644\u0627\u0648\u0644", "\u0631\u0628\u064A\u0639\xa0\u0627\u0644\u062B\u0627\u0646\u064A", "\u062C\u0645\u0627\u062F\u0649\xa0\u0627\u0644\u0627\u0648\u0644\u0649", "\u062C\u0645\u0627\u062F\u0649\xa0\u0627\u0644\u062B\u0627\u0646\u064A\u0629", "\u0631\u062C\u0628", "\u0634\u0639\u0628\u0627\u0646", "\u0631\u0645\u0636\u0627\u0646", "\u0634\u0648\u0627\u0644", "\u0630\u0648\xa0\u0627\u0644\u0642\u0639\u062F\u0629", "\u0630\u0648\xa0\u0627\u0644\u062D\u062C\u0629", "" };
						calendar = new HijriCalendar();
						return;
					}

				case 2049:
					{
						amDesignator = "\u0635";
						pmDesignator = "\u0645";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM, yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM, yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 6;
						abbreviatedDayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						dayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						monthNames = new string[] { "\u0643\u0627\u0646\u0648\u0646\xa0\u0627\u0644\u062B\u0627\u0646\u064A", "\u0634\u0628\u0627\u0637", "\u0622\u0630\u0627\u0631", "\u0646\u064A\u0633\u0627\u0646", "\u0623\u064A\u0627\u0631", "\u062D\u0632\u064A\u0631\u0627\u0646", "\u062A\u0645\u0648\u0632", "\u0622\u0628", "\u0623\u064A\u0644\u0648\u0644", "\u062A\u0634\u0631\u064A\u0646\xa0\u0627\u0644\u0623\u0648\u0644", "\u062A\u0634\u0631\u064A\u0646\xa0\u0627\u0644\u062B\u0627\u0646\u064A", "\u0643\u0627\u0646\u0648\u0646\xa0\u0627\u0644\u0623\u0648\u0644", "" };
						abbreviatedMonthNames = new string[] { "\u0643\u0627\u0646\u0648\u0646\xa0\u0627\u0644\u062B\u0627\u0646\u064A", "\u0634\u0628\u0627\u0637", "\u0622\u0630\u0627\u0631", "\u0646\u064A\u0633\u0627\u0646", "\u0623\u064A\u0627\u0631", "\u062D\u0632\u064A\u0631\u0627\u0646", "\u062A\u0645\u0648\u0632", "\u0622\u0628", "\u0623\u064A\u0644\u0648\u0644", "\u062A\u0634\u0631\u064A\u0646\xa0\u0627\u0644\u0623\u0648\u0644", "\u062A\u0634\u0631\u064A\u0646\xa0\u0627\u0644\u062B\u0627\u0646\u064A", "\u0643\u0627\u0646\u0648\u0646\xa0\u0627\u0644\u0623\u0648\u0644", "" };
						firstDayOfWeek = (int)DayOfWeek.Saturday;
						return;
					}

				case 2052:
					{
						amDesignator = "\u4E0A\u5348";
						pmDesignator = "\u4E0B\u5348";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "yyyy/M/d";
						longDatePattern = "yyyy'\u5E74'M'\u6708'd'\u65E5'";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "M'\u6708'd'\u65E5'";
						yearMonthPattern = "yyyy'\u5E74'M'\u6708'";
						fullDateTimePattern = "yyyy'\u5E74'M'\u6708'd'\u65E5' H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "\u65E5", "\u4E00", "\u4E8C", "\u4E09", "\u56DB", "\u4E94", "\u516D" };
						dayNames = new string[] { "\u661F\u671F\u65E5", "\u661F\u671F\u4E00", "\u661F\u671F\u4E8C", "\u661F\u671F\u4E09", "\u661F\u671F\u56DB", "\u661F\u671F\u4E94", "\u661F\u671F\u516D" };
						monthNames = new string[] { "\u4E00\u6708", "\u4E8C\u6708", "\u4E09\u6708", "\u56DB\u6708", "\u4E94\u6708", "\u516D\u6708", "\u4E03\u6708", "\u516B\u6708", "\u4E5D\u6708", "\u5341\u6708", "\u5341\u4E00\u6708", "\u5341\u4E8C\u6708", "" };
						abbreviatedMonthNames = new string[] { "\u4E00\u6708", "\u4E8C\u6708", "\u4E09\u6708", "\u56DB\u6708", "\u4E94\u6708", "\u516D\u6708", "\u4E03\u6708", "\u516B\u6708", "\u4E5D\u6708", "\u5341\u6708", "\u5341\u4E00\u6708", "\u5341\u4E8C\u6708", "" };
						return;
					}

				case 2055:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "dddd, d. MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd, d. MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "So", "Mo", "Di", "Mi", "Do", "Fr", "Sa" };
						dayNames = new string[] { "Sonntag", "Montag", "Dienstag", "Mittwoch", "Donnerstag", "Freitag", "Samstag" };
						monthNames = new string[] { "Januar", "Februar", "M\xe4rz", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mrz", "Apr", "Mai", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dez", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 2057:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dd MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
						monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 2058:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mi\xe9", "jue", "vie", "s\xe1b" };
						dayNames = new string[] { "domingo", "lunes", "martes", "mi\xe9rcoles", "jueves", "viernes", "s\xe1bado" };
						monthNames = new string[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre", "" };
						abbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
						return;
					}

				case 2060:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "d/MM/yyyy";
						longDatePattern = "dddd d MMMM yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "d MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd d MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "dim.", "lun.", "mar.", "mer.", "jeu.", "ven.", "sam." };
						dayNames = new string[] { "dimanche", "lundi", "mardi", "mercredi", "jeudi", "vendredi", "samedi" };
						monthNames = new string[] { "janvier", "f\xe9vrier", "mars", "avril", "mai", "juin", "juillet", "ao\xfbt", "septembre", "octobre", "novembre", "d\xe9cembre", "" };
						abbreviatedMonthNames = new string[] { "janv.", "f\xe9vr.", "mars", "avr.", "mai", "juin", "juil.", "ao\xfbt", "sept.", "oct.", "nov.", "d\xe9c.", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 2064:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "dddd, d. MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "d. MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd, d. MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mer", "gio", "ven", "sab" };
						dayNames = new string[] { "domenica", "luned\xec", "marted\xec", "mercoled\xec", "gioved\xec", "venerd\xec", "sabato" };
						monthNames = new string[] { "gennaio", "febbraio", "marzo", "aprile", "maggio", "giugno", "luglio", "agosto", "settembre", "ottobre", "novembre", "dicembre", "" };
						abbreviatedMonthNames = new string[] { "gen", "feb", "mar", "apr", "mag", "gio", "lug", "ago", "set", "ott", "nov", "dic", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 2067:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "d/MM/yyyy";
						longDatePattern = "dddd d MMMM yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd d MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "zo", "ma", "di", "wo", "do", "vr", "za" };
						dayNames = new string[] { "zondag", "maandag", "dinsdag", "woensdag", "donderdag", "vrijdag", "zaterdag" };
						monthNames = new string[] { "januari", "februari", "maart", "april", "mei", "juni", "juli", "augustus", "september", "oktober", "november", "december", "" };
						abbreviatedMonthNames = new string[] { "jan", "feb", "mrt", "apr", "mei", "jun", "jul", "aug", "sep", "okt", "nov", "dec", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 2068:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "d. MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "d. MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d. MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "s\xf8", "m\xe5", "ty", "on", "to", "fr", "la" };
						dayNames = new string[] { "s\xf8ndag", "m\xe5ndag", "tysdag", "onsdag", "torsdag", "fredag", "laurdag" };
						monthNames = new string[] { "januar", "februar", "mars", "april", "mai", "juni", "juli", "august", "september", "oktober", "november", "desember", "" };
						abbreviatedMonthNames = new string[] { "jan", "feb", "mar", "apr", "mai", "jun", "jul", "aug", "sep", "okt", "nov", "des", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 2070:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "dd-MM-yyyy";
						longDatePattern = "dddd, d' de 'MMMM' de 'yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "d/M";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, d' de 'MMMM' de 'yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "dom", "seg", "ter", "qua", "qui", "sex", "s\xe1b" };
						dayNames = new string[] { "domingo", "segunda-feira", "ter\xe7a-feira", "quarta-feira", "quinta-feira", "sexta-feira", "s\xe1bado" };
						monthNames = new string[] { "Janeiro", "Fevereiro", "Mar\xe7o", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Fev", "Mar", "Abr", "Mai", "Jun", "Jul", "Ago", "Set", "Out", "Nov", "Dez", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 2074:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "d.M.yyyy";
						longDatePattern = "d. MMMM yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "d. MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d. MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "ned", "pon", "uto", "sre", "\u010Det", "pet", "sub" };
						dayNames = new string[] { "nedelja", "ponedeljak", "utorak", "sreda", "\u010Detvrtak", "petak", "subota" };
						monthNames = new string[] { "januar", "februar", "mart", "april", "maj", "jun", "jul", "avgust", "septembar", "oktobar", "novembar", "decembar", "" };
						abbreviatedMonthNames = new string[] { "jan", "feb", "mar", "apr", "maj", "jun", "jul", "avg", "sep", "okt", "nov", "dec", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 2077:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "d.M.yyyy";
						longDatePattern = "'den 'd MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "'den 'd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "'den 'd MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "s\xf6", "m\xe5", "ti", "on", "to", "fr", "l\xf6" };
						dayNames = new string[] { "s\xf6ndag", "m\xe5ndag", "tisdag", "onsdag", "torsdag", "fredag", "l\xf6rdag" };
						monthNames = new string[] { "januari", "februari", "mars", "april", "maj", "juni", "juli", "augusti", "september", "oktober", "november", "december", "" };
						abbreviatedMonthNames = new string[] { "jan", "feb", "mar", "apr", "maj", "jun", "jul", "aug", "sep", "okt", "nov", "dec", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 2092:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "d MMMM yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "d MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u0411", "\u0411\u0435", "\u0427\u0430", "\u0427", "\u04B8\u0430", "\u04B8", "\u0428" };
						dayNames = new string[] { "\u0411\u0430\u0437\u0430\u0440", "\u0411\u0430\u0437\u0430\u0440\xa0\u0435\u0440\u0442\u04D9\u0441\u0438", "\u0427\u04D9\u0440\u0448\u04D9\u043D\u0431\u04D9\xa0\u0430\u0445\u0448\u0430\u043C\u044B", "\u0427\u04D9\u0440\u0448\u04D9\u043D\u0431\u04D9", "\u04B8\u04AF\u043C\u04D9\xa0\u0430\u0445\u0448\u0430\u043C\u044B", "\u04B8\u04AF\u043C\u04D9", "\u0428\u04D9\u043D\u0431\u04D9" };
						monthNames = new string[] { "\u0408\u0430\u043D\u0432\u0430\u0440", "\u0424\u0435\u0432\u0440\u0430\u043B", "\u041C\u0430\u0440\u0442", "\u0410\u043F\u0440\u0435\u043B", "\u041C\u0430\u0458", "\u0418\u0458\u0443\u043D", "\u0418\u0458\u0443\u043B", "\u0410\u0432\u0433\u0443\u0441\u0442", "\u0421\u0435\u043D\u0442\u0458\u0430\u0431\u0440", "\u041E\u043A\u0442\u0458\u0430\u0431\u0440", "\u041D\u043E\u0458\u0430\u0431\u0440", "\u0414\u0435\u043A\u0430\u0431\u0440", "" };
						abbreviatedMonthNames = new string[] { "\u0408\u0430\u043D", "\u0424\u0435\u0432", "\u041C\u0430\u0440", "\u0410\u043F\u0440", "\u041C\u0430\u0458", "\u0418\u0458\u0443\u043D", "\u0418\u0458\u0443\u043B", "\u0410\u0432\u0433", "\u0421\u0435\u043D", "\u041E\u043A\u0442", "\u041D\u043E\u044F", "\u0414\u0435\u043A", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 2110:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dd MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "Ahad", "Isnin", "Sel", "Rabu", "Khamis", "Jumaat", "Sabtu" };
						dayNames = new string[] { "Ahad", "Isnin", "Selasa", "Rabu", "Khamis", "Jumaat", "Sabtu" };
						monthNames = new string[] { "Januari", "Februari", "Mac", "April", "Mei", "Jun", "Julai", "Ogos", "September", "Oktober", "November", "Disember", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mac", "Apr", "Mei", "Jun", "Jul", "Ogos", "Sept", "Okt", "Nov", "Dis", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 2115:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "yyyy '\u0439\u0438\u043B' d-MMMM";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "d-MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "yyyy '\u0439\u0438\u043B' d-MMMM HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u044F\u043A\u0448", "\u0434\u0448", "\u0441\u0448", "\u0447\u0448", "\u043F\u0448", "\u0436", "\u0448" };
						dayNames = new string[] { "\u044F\u043A\u0448\u0430\u043D\u0431\u0430", "\u0434\u0443\u0448\u0430\u043D\u0431\u0430", "\u0441\u0435\u0448\u0430\u043D\u0431\u0430", "\u0447\u043E\u0440\u0448\u0430\u043D\u0431\u0430", "\u043F\u0430\u0439\u0448\u0430\u043D\u0431\u0430", "\u0436\u0443\u043C\u0430", "\u0448\u0430\u043D\u0431\u0430" };
						monthNames = new string[] { "\u042F\u043D\u0432\u0430\u0440", "\u0424\u0435\u0432\u0440\u0430\u043B", "\u041C\u0430\u0440\u0442", "\u0410\u043F\u0440\u0435\u043B", "\u041C\u0430\u0439", "\u0418\u044E\u043D", "\u0418\u044E\u043B", "\u0410\u0432\u0433\u0443\u0441\u0442", "\u0421\u0435\u043D\u0442\u044F\u0431\u0440", "\u041E\u043A\u0442\u044F\u0431\u0440", "\u041D\u043E\u044F\u0431\u0440", "\u0414\u0435\u043A\u0430\u0431\u0440", "" };
						abbreviatedMonthNames = new string[] { "\u042F\u043D\u0432", "\u0424\u0435\u0432", "\u041C\u0430\u0440", "\u0410\u043F\u0440", "\u041C\u0430\u0439", "\u0418\u044E\u043D", "\u0418\u044E\u043B", "\u0410\u0432\u0433", "\u0421\u0435\u043D", "\u041E\u043A\u0442", "\u041D\u043E\u044F", "\u0414\u0435\u043A", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 3073:
					{
						amDesignator = "\u0635";
						pmDesignator = "\u0645";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM, yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM, yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 6;
						abbreviatedDayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						dayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						monthNames = new string[] { "\u064A\u0646\u0627\u064A\u0631", "\u0641\u0628\u0631\u0627\u064A\u0631", "\u0645\u0627\u0631\u0633", "\u0627\u0628\u0631\u064A\u0644", "\u0645\u0627\u064A\u0648", "\u064A\u0648\u0646\u064A\u0648", "\u064A\u0648\u0644\u064A\u0648", "\u0627\u063A\u0633\u0637\u0633", "\u0633\u0628\u062A\u0645\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0641\u0645\u0628\u0631", "\u062F\u064A\u0633\u0645\u0628\u0631", "" };
						abbreviatedMonthNames = new string[] { "\u064A\u0646\u0627\u064A\u0631", "\u0641\u0628\u0631\u0627\u064A\u0631", "\u0645\u0627\u0631\u0633", "\u0627\u0628\u0631\u064A\u0644", "\u0645\u0627\u064A\u0648", "\u064A\u0648\u0646\u064A\u0648", "\u064A\u0648\u0644\u064A\u0648", "\u0627\u063A\u0633\u0637\u0633", "\u0633\u0628\u062A\u0645\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0641\u0645\u0628\u0631", "\u062F\u064A\u0633\u0645\u0628\u0631", "" };
						firstDayOfWeek = (int)DayOfWeek.Saturday;
						return;
					}

				case 3076:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "d/M/yyyy";
						longDatePattern = "dddd, d MMMM, yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "d MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dddd, d MMMM, yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
						monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						return;
					}

				case 3079:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "dddd, dd. MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd, dd. MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "So", "Mo", "Di", "Mi", "Do", "Fr", "Sa" };
						dayNames = new string[] { "Sonntag", "Montag", "Dienstag", "Mittwoch", "Donnerstag", "Freitag", "Samstag" };
						monthNames = new string[] { "J\xe4nner", "Februar", "M\xe4rz", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember", "" };
						abbreviatedMonthNames = new string[] { "J\xe4n", "Feb", "M\xe4r", "Apr", "Mai", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dez", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 3081:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "d/MM/yyyy";
						longDatePattern = "dddd, d MMMM yyyy";
						shortTimePattern = "h:mm tt";
						longTimePattern = "h:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd, d MMMM yyyy h:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
						monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 3082:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mi\xe9", "jue", "vie", "s\xe1b" };
						dayNames = new string[] { "domingo", "lunes", "martes", "mi\xe9rcoles", "jueves", "viernes", "s\xe1bado" };
						monthNames = new string[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre", "" };
						abbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 3084:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "yyyy-MM-dd";
						longDatePattern = "d MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "d MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "d MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "dim.", "lun.", "mar.", "mer.", "jeu.", "ven.", "sam." };
						dayNames = new string[] { "dimanche", "lundi", "mardi", "mercredi", "jeudi", "vendredi", "samedi" };
						monthNames = new string[] { "janvier", "f\xe9vrier", "mars", "avril", "mai", "juin", "juillet", "ao\xfbt", "septembre", "octobre", "novembre", "d\xe9cembre", "" };
						abbreviatedMonthNames = new string[] { "janv.", "f\xe9vr.", "mars", "avr.", "mai", "juin", "juil.", "ao\xfbt", "sept.", "oct.", "nov.", "d\xe9c.", "" };
						return;
					}

				case 3098:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "d.M.yyyy";
						longDatePattern = "d. MMMM yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "d. MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d. MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u043D\u0435\u0434", "\u043F\u043E\u043D", "\u0443\u0442\u043E", "\u0441\u0440\u0435", "\u0447\u0435\u0442", "\u043F\u0435\u0442", "\u0441\u0443\u0431" };
						dayNames = new string[] { "\u043D\u0435\u0434\u0435\u0459\u0430", "\u043F\u043E\u043D\u0435\u0434\u0435\u0459\u0430\u043A", "\u0443\u0442\u043E\u0440\u0430\u043A", "\u0441\u0440\u0435\u0434\u0430", "\u0447\u0435\u0442\u0432\u0440\u0442\u0430\u043A", "\u043F\u0435\u0442\u0430\u043A", "\u0441\u0443\u0431\u043E\u0442\u0430" };
						monthNames = new string[] { "\u0458\u0430\u043D\u0443\u0430\u0440", "\u0444\u0435\u0431\u0440\u0443\u0430\u0440", "\u043C\u0430\u0440\u0442", "\u0430\u043F\u0440\u0438\u043B", "\u043C\u0430\u0458", "\u0458\u0443\u043D", "\u0458\u0443\u043B", "\u0430\u0432\u0433\u0443\u0441\u0442", "\u0441\u0435\u043F\u0442\u0435\u043C\u0431\u0430\u0440", "\u043E\u043A\u0442\u043E\u0431\u0430\u0440", "\u043D\u043E\u0432\u0435\u043C\u0431\u0430\u0440", "\u0434\u0435\u0446\u0435\u043C\u0431\u0430\u0440", "" };
						abbreviatedMonthNames = new string[] { "\u0458\u0430\u043D", "\u0444\u0435\u0431", "\u043C\u0430\u0440", "\u0430\u043F\u0440", "\u043C\u0430\u0458", "\u0458\u0443\u043D", "\u0458\u0443\u043B", "\u0430\u0432\u0433", "\u0441\u0435\u043F", "\u043E\u043A\u0442", "\u043D\u043E\u0432", "\u0434\u0435\u0446", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 4097:
					{
						amDesignator = "\u0635";
						pmDesignator = "\u0645";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM, yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM, yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 6;
						abbreviatedDayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						dayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						monthNames = new string[] { "\u064A\u0646\u0627\u064A\u0631", "\u0641\u0628\u0631\u0627\u064A\u0631", "\u0645\u0627\u0631\u0633", "\u0627\u0628\u0631\u064A\u0644", "\u0645\u0627\u064A\u0648", "\u064A\u0648\u0646\u064A\u0648", "\u064A\u0648\u0644\u064A\u0648", "\u0627\u063A\u0633\u0637\u0633", "\u0633\u0628\u062A\u0645\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0641\u0645\u0628\u0631", "\u062F\u064A\u0633\u0645\u0628\u0631", "" };
						abbreviatedMonthNames = new string[] { "\u064A\u0646\u0627\u064A\u0631", "\u0641\u0628\u0631\u0627\u064A\u0631", "\u0645\u0627\u0631\u0633", "\u0627\u0628\u0631\u064A\u0644", "\u0645\u0627\u064A\u0648", "\u064A\u0648\u0646\u064A\u0648", "\u064A\u0648\u0644\u064A\u0648", "\u0627\u063A\u0633\u0637\u0633", "\u0633\u0628\u062A\u0645\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0641\u0645\u0628\u0631", "\u062F\u064A\u0633\u0645\u0628\u0631", "" };
						firstDayOfWeek = (int)DayOfWeek.Saturday;
						return;
					}

				case 4100:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "d/M/yyyy";
						longDatePattern = "dddd, d MMMM, yyyy";
						shortTimePattern = "tt h:mm";
						longTimePattern = "tt h:mm:ss";
						monthDayPattern = "d MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dddd, d MMMM, yyyy tt h:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "\u661F\u671F\u65E5", "\u661F\u671F\u4E00", "\u661F\u671F\u4E8C", "\u661F\u671F\u4E09", "\u661F\u671F\u56DB", "\u661F\u671F\u4E94", "\u661F\u671F\u516D" };
						dayNames = new string[] { "\u661F\u671F\u65E5", "\u661F\u671F\u4E00", "\u661F\u671F\u4E8C", "\u661F\u671F\u4E09", "\u661F\u671F\u56DB", "\u661F\u671F\u4E94", "\u661F\u671F\u516D" };
						monthNames = new string[] { "\u4E00\u6708", "\u4E8C\u6708", "\u4E09\u6708", "\u56DB\u6708", "\u4E94\u6708", "\u516D\u6708", "\u4E03\u6708", "\u516B\u6708", "\u4E5D\u6708", "\u5341\u6708", "\u5341\u4E00\u6708", "\u5341\u4E8C\u6708", "" };
						abbreviatedMonthNames = new string[] { "\u4E00\u6708", "\u4E8C\u6708", "\u4E09\u6708", "\u56DB\u6708", "\u4E94\u6708", "\u516D\u6708", "\u4E03\u6708", "\u516B\u6708", "\u4E5D\u6708", "\u5341\u6708", "\u5341\u4E00\u6708", "\u5341\u4E8C\u6708", "" };
						return;
					}

				case 4103:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "dddd, d. MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd, d. MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "So", "Mo", "Di", "Mi", "Do", "Fr", "Sa" };
						dayNames = new string[] { "Sonntag", "Montag", "Dienstag", "Mittwoch", "Donnerstag", "Freitag", "Samstag" };
						monthNames = new string[] { "Januar", "Februar", "M\xe4rz", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mrz", "Apr", "Mai", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dez", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 4105:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "MMMM d, yyyy";
						shortTimePattern = "h:mm tt";
						longTimePattern = "h:mm:ss tt";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "MMMM d, yyyy h:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
						monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						return;
					}

				case 4106:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mi\xe9", "jue", "vie", "s\xe1b" };
						dayNames = new string[] { "domingo", "lunes", "martes", "mi\xe9rcoles", "jueves", "viernes", "s\xe1bado" };
						monthNames = new string[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre", "" };
						abbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
						return;
					}

				case 4108:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "dddd, d. MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "d MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd, d. MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "dim.", "lun.", "mar.", "mer.", "jeu.", "ven.", "sam." };
						dayNames = new string[] { "dimanche", "lundi", "mardi", "mercredi", "jeudi", "vendredi", "samedi" };
						monthNames = new string[] { "janvier", "f\xe9vrier", "mars", "avril", "mai", "juin", "juillet", "ao\xfbt", "septembre", "octobre", "novembre", "d\xe9cembre", "" };
						abbreviatedMonthNames = new string[] { "janv.", "f\xe9vr.", "mars", "avr.", "mai", "juin", "juil.", "ao\xfbt", "sept.", "oct.", "nov.", "d\xe9c.", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 5121:
					{
						amDesignator = "\u0635";
						pmDesignator = "\u0645";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "dd-MM-yyyy";
						longDatePattern = "dd MMMM, yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM, yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 6;
						abbreviatedDayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						dayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						monthNames = new string[] { "\u062C\u0627\u0646\u0641\u064A\u064A\u0647", "\u0641\u064A\u0641\u0631\u064A\u064A\u0647", "\u0645\u0627\u0631\u0633", "\u0623\u0641\u0631\u064A\u0644", "\u0645\u064A", "\u062C\u0648\u0627\u0646", "\u062C\u0648\u064A\u064A\u0647", "\u0623\u0648\u062A", "\u0633\u0628\u062A\u0645\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0641\u0645\u0628\u0631", "\u062F\u064A\u0633\u0645\u0628\u0631", "" };
						abbreviatedMonthNames = new string[] { "\u062C\u0627\u0646\u0641\u064A\u064A\u0647", "\u0641\u064A\u0641\u0631\u064A\u064A\u0647", "\u0645\u0627\u0631\u0633", "\u0623\u0641\u0631\u064A\u0644", "\u0645\u064A", "\u062C\u0648\u0627\u0646", "\u062C\u0648\u064A\u064A\u0647", "\u0623\u0648\u062A", "\u0633\u0628\u062A\u0645\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0641\u0645\u0628\u0631", "\u062F\u064A\u0633\u0645\u0628\u0631", "" };
						firstDayOfWeek = (int)DayOfWeek.Saturday;
						return;
					}

				case 5124:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "d/M/yyyy";
						longDatePattern = "dddd, d MMMM, yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "d MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dddd, d MMMM, yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "\u661F\u671F\u65E5", "\u661F\u671F\u4E00", "\u661F\u671F\u4E8C", "\u661F\u671F\u4E09", "\u661F\u671F\u56DB", "\u661F\u671F\u4E94", "\u661F\u671F\u516D" };
						dayNames = new string[] { "\u661F\u671F\u65E5", "\u661F\u671F\u4E00", "\u661F\u671F\u4E8C", "\u661F\u671F\u4E09", "\u661F\u671F\u56DB", "\u661F\u671F\u4E94", "\u661F\u671F\u516D" };
						monthNames = new string[] { "\u4E00\u6708", "\u4E8C\u6708", "\u4E09\u6708", "\u56DB\u6708", "\u4E94\u6708", "\u516D\u6708", "\u4E03\u6708", "\u516B\u6708", "\u4E5D\u6708", "\u5341\u6708", "\u5341\u4E00\u6708", "\u5341\u4E8C\u6708", "" };
						abbreviatedMonthNames = new string[] { "\u4E00\u6708", "\u4E8C\u6708", "\u4E09\u6708", "\u56DB\u6708", "\u4E94\u6708", "\u516D\u6708", "\u4E03\u6708", "\u516B\u6708", "\u4E5D\u6708", "\u5341\u6708", "\u5341\u4E00\u6708", "\u5341\u4E8C\u6708", "" };
						return;
					}

				case 5127:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "dddd, d. MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd, d. MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "So", "Mo", "Di", "Mi", "Do", "Fr", "Sa" };
						dayNames = new string[] { "Sonntag", "Montag", "Dienstag", "Mittwoch", "Donnerstag", "Freitag", "Samstag" };
						monthNames = new string[] { "Januar", "Februar", "M\xe4rz", "April", "Mai", "Juni", "Juli", "August", "September", "Oktober", "November", "Dezember", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mrz", "Apr", "Mai", "Jun", "Jul", "Aug", "Sep", "Okt", "Nov", "Dez", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 5129:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "d/MM/yyyy";
						longDatePattern = "dddd, d MMMM yyyy";
						shortTimePattern = "h:mm tt";
						longTimePattern = "h:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd, d MMMM yyyy h:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
						monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 5130:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mi\xe9", "jue", "vie", "s\xe1b" };
						dayNames = new string[] { "domingo", "lunes", "martes", "mi\xe9rcoles", "jueves", "viernes", "s\xe1bado" };
						monthNames = new string[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre", "" };
						abbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
						return;
					}

				case 5132:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd d MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "d MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd d MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "dim.", "lun.", "mar.", "mer.", "jeu.", "ven.", "sam." };
						dayNames = new string[] { "dimanche", "lundi", "mardi", "mercredi", "jeudi", "vendredi", "samedi" };
						monthNames = new string[] { "janvier", "f\xe9vrier", "mars", "avril", "mai", "juin", "juillet", "ao\xfbt", "septembre", "octobre", "novembre", "d\xe9cembre", "" };
						abbreviatedMonthNames = new string[] { "janv.", "f\xe9vr.", "mars", "avr.", "mai", "juin", "juil.", "ao\xfbt", "sept.", "oct.", "nov.", "d\xe9c.", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 6145:
					{
						amDesignator = "\u0635";
						pmDesignator = "\u0645";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "dd-MM-yyyy";
						longDatePattern = "dd MMMM, yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM, yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						dayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						monthNames = new string[] { "\u064A\u0646\u0627\u064A\u0631", "\u0641\u0628\u0631\u0627\u064A\u0631", "\u0645\u0627\u0631\u0633", "\u0627\u0628\u0631\u064A\u0644", "\u0645\u0627\u064A", "\u064A\u0648\u0646\u064A\u0648", "\u064A\u0648\u0644\u064A\u0648\u0632", "\u063A\u0634\u062A", "\u0634\u062A\u0646\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0646\u0628\u0631", "\u062F\u062C\u0646\u0628\u0631", "" };
						abbreviatedMonthNames = new string[] { "\u064A\u0646\u0627\u064A\u0631", "\u0641\u0628\u0631\u0627\u064A\u0631", "\u0645\u0627\u0631\u0633", "\u0627\u0628\u0631\u064A\u0644", "\u0645\u0627\u064A", "\u064A\u0648\u0646\u064A\u0648", "\u064A\u0648\u0644\u064A\u0648\u0632", "\u063A\u0634\u062A", "\u0634\u062A\u0646\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0646\u0628\u0631", "\u062F\u062C\u0646\u0628\u0631", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 6153:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dd MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
						monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 6154:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "MM/dd/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mi\xe9", "jue", "vie", "s\xe1b" };
						dayNames = new string[] { "domingo", "lunes", "martes", "mi\xe9rcoles", "jueves", "viernes", "s\xe1bado" };
						monthNames = new string[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre", "" };
						abbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
						return;
					}

				case 6156:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd d MMMM yyyy";
						shortTimePattern = "HH:mm";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "d MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd d MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "dim.", "lun.", "mar.", "mer.", "jeu.", "ven.", "sam." };
						dayNames = new string[] { "dimanche", "lundi", "mardi", "mercredi", "jeudi", "vendredi", "samedi" };
						monthNames = new string[] { "janvier", "f\xe9vrier", "mars", "avril", "mai", "juin", "juillet", "ao\xfbt", "septembre", "octobre", "novembre", "d\xe9cembre", "" };
						abbreviatedMonthNames = new string[] { "janv.", "f\xe9vr.", "mars", "avr.", "mai", "juin", "juil.", "ao\xfbt", "sept.", "oct.", "nov.", "d\xe9c.", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 7169:
					{
						amDesignator = "\u0635";
						pmDesignator = "\u0645";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "dd-MM-yyyy";
						longDatePattern = "dd MMMM, yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM, yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						dayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						monthNames = new string[] { "\u062C\u0627\u0646\u0641\u064A", "\u0641\u064A\u0641\u0631\u064A", "\u0645\u0627\u0631\u0633", "\u0627\u0641\u0631\u064A\u0644", "\u0645\u0627\u064A", "\u062C\u0648\u0627\u0646", "\u062C\u0648\u064A\u0644\u064A\u0629", "\u0627\u0648\u062A", "\u0633\u0628\u062A\u0645\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0641\u0645\u0628\u0631", "\u062F\u064A\u0633\u0645\u0628\u0631", "" };
						abbreviatedMonthNames = new string[] { "\u062C\u0627\u0646\u0641\u064A", "\u0641\u064A\u0641\u0631\u064A", "\u0645\u0627\u0631\u0633", "\u0627\u0641\u0631\u064A\u0644", "\u0645\u0627\u064A", "\u062C\u0648\u0627\u0646", "\u062C\u0648\u064A\u0644\u064A\u0629", "\u0627\u0648\u062A", "\u0633\u0628\u062A\u0645\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0641\u0645\u0628\u0631", "\u062F\u064A\u0633\u0645\u0628\u0631", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 7177:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "yyyy/MM/dd";
						longDatePattern = "dd MMMM yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dd MMMM yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
						monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						return;
					}

				case 7178:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mi\xe9", "jue", "vie", "s\xe1b" };
						dayNames = new string[] { "domingo", "lunes", "martes", "mi\xe9rcoles", "jueves", "viernes", "s\xe1bado" };
						monthNames = new string[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre", "" };
						abbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
						return;
					}

				case 8193:
					{
						amDesignator = "\u0635";
						pmDesignator = "\u0645";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM, yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM, yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 6;
						abbreviatedDayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						dayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						monthNames = new string[] { "\u064A\u0646\u0627\u064A\u0631", "\u0641\u0628\u0631\u0627\u064A\u0631", "\u0645\u0627\u0631\u0633", "\u0627\u0628\u0631\u064A\u0644", "\u0645\u0627\u064A\u0648", "\u064A\u0648\u0646\u064A\u0648", "\u064A\u0648\u0644\u064A\u0648", "\u0627\u063A\u0633\u0637\u0633", "\u0633\u0628\u062A\u0645\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0641\u0645\u0628\u0631", "\u062F\u064A\u0633\u0645\u0628\u0631", "" };
						abbreviatedMonthNames = new string[] { "\u064A\u0646\u0627\u064A\u0631", "\u0641\u0628\u0631\u0627\u064A\u0631", "\u0645\u0627\u0631\u0633", "\u0627\u0628\u0631\u064A\u0644", "\u0645\u0627\u064A\u0648", "\u064A\u0648\u0646\u064A\u0648", "\u064A\u0648\u0644\u064A\u0648", "\u0627\u063A\u0633\u0637\u0633", "\u0633\u0628\u062A\u0645\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0641\u0645\u0628\u0631", "\u062F\u064A\u0633\u0645\u0628\u0631", "" };
						firstDayOfWeek = (int)DayOfWeek.Saturday;
						return;
					}

				case 8201:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, MMMM dd, yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dddd, MMMM dd, yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
						monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						return;
					}

				case 8202:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mi\xe9", "jue", "vie", "s\xe1b" };
						dayNames = new string[] { "domingo", "lunes", "martes", "mi\xe9rcoles", "jueves", "viernes", "s\xe1bado" };
						monthNames = new string[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre", "" };
						abbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
						return;
					}

				case 9217:
					{
						amDesignator = "\u0635";
						pmDesignator = "\u0645";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM, yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM, yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 6;
						abbreviatedDayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						dayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						monthNames = new string[] { "\u064A\u0646\u0627\u064A\u0631", "\u0641\u0628\u0631\u0627\u064A\u0631", "\u0645\u0627\u0631\u0633", "\u0627\u0628\u0631\u064A\u0644", "\u0645\u0627\u064A\u0648", "\u064A\u0648\u0646\u064A\u0648", "\u064A\u0648\u0644\u064A\u0648", "\u0627\u063A\u0633\u0637\u0633", "\u0633\u0628\u062A\u0645\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0641\u0645\u0628\u0631", "\u062F\u064A\u0633\u0645\u0628\u0631", "" };
						abbreviatedMonthNames = new string[] { "\u064A\u0646\u0627\u064A\u0631", "\u0641\u0628\u0631\u0627\u064A\u0631", "\u0645\u0627\u0631\u0633", "\u0627\u0628\u0631\u064A\u0644", "\u0645\u0627\u064A\u0648", "\u064A\u0648\u0646\u064A\u0648", "\u064A\u0648\u0644\u064A\u0648", "\u0627\u063A\u0633\u0637\u0633", "\u0633\u0628\u062A\u0645\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0641\u0645\u0628\u0631", "\u062F\u064A\u0633\u0645\u0628\u0631", "" };
						firstDayOfWeek = (int)DayOfWeek.Saturday;
						return;
					}

				case 9225:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "MM/dd/yyyy";
						longDatePattern = "dddd, MMMM dd, yyyy";
						shortTimePattern = "h:mm tt";
						longTimePattern = "h:mm:ss tt";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dddd, MMMM dd, yyyy h:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
						monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 9226:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mi\xe9", "jue", "vie", "s\xe1b" };
						dayNames = new string[] { "domingo", "lunes", "martes", "mi\xe9rcoles", "jueves", "viernes", "s\xe1bado" };
						monthNames = new string[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre", "" };
						abbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
						return;
					}

				case 10241:
					{
						amDesignator = "\u0635";
						pmDesignator = "\u0645";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM, yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM, yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 6;
						abbreviatedDayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						dayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						monthNames = new string[] { "\u0643\u0627\u0646\u0648\u0646\xa0\u0627\u0644\u062B\u0627\u0646\u064A", "\u0634\u0628\u0627\u0637", "\u0622\u0630\u0627\u0631", "\u0646\u064A\u0633\u0627\u0646", "\u0623\u064A\u0627\u0631", "\u062D\u0632\u064A\u0631\u0627\u0646", "\u062A\u0645\u0648\u0632", "\u0622\u0628", "\u0623\u064A\u0644\u0648\u0644", "\u062A\u0634\u0631\u064A\u0646\xa0\u0627\u0644\u0623\u0648\u0644", "\u062A\u0634\u0631\u064A\u0646\xa0\u0627\u0644\u062B\u0627\u0646\u064A", "\u0643\u0627\u0646\u0648\u0646\xa0\u0627\u0644\u0623\u0648\u0644", "" };
						abbreviatedMonthNames = new string[] { "\u0643\u0627\u0646\u0648\u0646\xa0\u0627\u0644\u062B\u0627\u0646\u064A", "\u0634\u0628\u0627\u0637", "\u0622\u0630\u0627\u0631", "\u0646\u064A\u0633\u0627\u0646", "\u0623\u064A\u0627\u0631", "\u062D\u0632\u064A\u0631\u0627\u0646", "\u062A\u0645\u0648\u0632", "\u0622\u0628", "\u0623\u064A\u0644\u0648\u0644", "\u062A\u0634\u0631\u064A\u0646\xa0\u0627\u0644\u0623\u0648\u0644", "\u062A\u0634\u0631\u064A\u0646\xa0\u0627\u0644\u062B\u0627\u0646\u064A", "\u0643\u0627\u0646\u0648\u0646\xa0\u0627\u0644\u0623\u0648\u0644", "" };
						firstDayOfWeek = (int)DayOfWeek.Saturday;
						return;
					}

				case 10249:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd MMMM yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd, dd MMMM yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
						monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						return;
					}

				case 10250:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mi\xe9", "jue", "vie", "s\xe1b" };
						dayNames = new string[] { "domingo", "lunes", "martes", "mi\xe9rcoles", "jueves", "viernes", "s\xe1bado" };
						monthNames = new string[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre", "" };
						abbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
						return;
					}

				case 11265:
					{
						amDesignator = "\u0635";
						pmDesignator = "\u0645";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM, yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM, yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 6;
						abbreviatedDayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						dayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						monthNames = new string[] { "\u0643\u0627\u0646\u0648\u0646\xa0\u0627\u0644\u062B\u0627\u0646\u064A", "\u0634\u0628\u0627\u0637", "\u0622\u0630\u0627\u0631", "\u0646\u064A\u0633\u0627\u0646", "\u0623\u064A\u0627\u0631", "\u062D\u0632\u064A\u0631\u0627\u0646", "\u062A\u0645\u0648\u0632", "\u0622\u0628", "\u0623\u064A\u0644\u0648\u0644", "\u062A\u0634\u0631\u064A\u0646\xa0\u0627\u0644\u0623\u0648\u0644", "\u062A\u0634\u0631\u064A\u0646\xa0\u0627\u0644\u062B\u0627\u0646\u064A", "\u0643\u0627\u0646\u0648\u0646\xa0\u0627\u0644\u0623\u0648\u0644", "" };
						abbreviatedMonthNames = new string[] { "\u0643\u0627\u0646\u0648\u0646\xa0\u0627\u0644\u062B\u0627\u0646\u064A", "\u0634\u0628\u0627\u0637", "\u0622\u0630\u0627\u0631", "\u0646\u064A\u0633\u0627\u0646", "\u0623\u064A\u0627\u0631", "\u062D\u0632\u064A\u0631\u0627\u0646", "\u062A\u0645\u0648\u0632", "\u0622\u0628", "\u0623\u064A\u0644\u0648\u0644", "\u062A\u0634\u0631\u064A\u0646\xa0\u0627\u0644\u0623\u0648\u0644", "\u062A\u0634\u0631\u064A\u0646\xa0\u0627\u0644\u062B\u0627\u0646\u064A", "\u0643\u0627\u0646\u0648\u0646\xa0\u0627\u0644\u0623\u0648\u0644", "" };
						firstDayOfWeek = (int)DayOfWeek.Saturday;
						return;
					}

				case 11273:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd MMMM yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd, dd MMMM yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
						monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						return;
					}

				case 11274:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mi\xe9", "jue", "vie", "s\xe1b" };
						dayNames = new string[] { "domingo", "lunes", "martes", "mi\xe9rcoles", "jueves", "viernes", "s\xe1bado" };
						monthNames = new string[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre", "" };
						abbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
						return;
					}

				case 12289:
					{
						amDesignator = "\u0635";
						pmDesignator = "\u0645";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM, yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM, yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						dayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						monthNames = new string[] { "\u0643\u0627\u0646\u0648\u0646\xa0\u0627\u0644\u062B\u0627\u0646\u064A", "\u0634\u0628\u0627\u0637", "\u0622\u0630\u0627\u0631", "\u0646\u064A\u0633\u0627\u0646", "\u0623\u064A\u0627\u0631", "\u062D\u0632\u064A\u0631\u0627\u0646", "\u062A\u0645\u0648\u0632", "\u0622\u0628", "\u0623\u064A\u0644\u0648\u0644", "\u062A\u0634\u0631\u064A\u0646\xa0\u0627\u0644\u0623\u0648\u0644", "\u062A\u0634\u0631\u064A\u0646\xa0\u0627\u0644\u062B\u0627\u0646\u064A", "\u0643\u0627\u0646\u0648\u0646\xa0\u0627\u0644\u0623\u0648\u0644", "" };
						abbreviatedMonthNames = new string[] { "\u0643\u0627\u0646\u0648\u0646\xa0\u0627\u0644\u062B\u0627\u0646\u064A", "\u0634\u0628\u0627\u0637", "\u0622\u0630\u0627\u0631", "\u0646\u064A\u0633\u0627\u0646", "\u0623\u064A\u0627\u0631", "\u062D\u0632\u064A\u0631\u0627\u0646", "\u062A\u0645\u0648\u0632", "\u0622\u0628", "\u0623\u064A\u0644\u0648\u0644", "\u062A\u0634\u0631\u064A\u0646\xa0\u0627\u0644\u0623\u0648\u0644", "\u062A\u0634\u0631\u064A\u0646\xa0\u0627\u0644\u062B\u0627\u0646\u064A", "\u0643\u0627\u0646\u0648\u0646\xa0\u0627\u0644\u0623\u0648\u0644", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 12297:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "M/d/yyyy";
						longDatePattern = "dddd, MMMM dd, yyyy";
						shortTimePattern = "h:mm tt";
						longTimePattern = "h:mm:ss tt";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dddd, MMMM dd, yyyy h:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
						monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						return;
					}

				case 12298:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mi\xe9", "jue", "vie", "s\xe1b" };
						dayNames = new string[] { "domingo", "lunes", "martes", "mi\xe9rcoles", "jueves", "viernes", "s\xe1bado" };
						monthNames = new string[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre", "" };
						abbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
						return;
					}

				case 13313:
					{
						amDesignator = "\u0635";
						pmDesignator = "\u0645";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM, yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM, yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 6;
						abbreviatedDayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						dayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						monthNames = new string[] { "\u064A\u0646\u0627\u064A\u0631", "\u0641\u0628\u0631\u0627\u064A\u0631", "\u0645\u0627\u0631\u0633", "\u0627\u0628\u0631\u064A\u0644", "\u0645\u0627\u064A\u0648", "\u064A\u0648\u0646\u064A\u0648", "\u064A\u0648\u0644\u064A\u0648", "\u0627\u063A\u0633\u0637\u0633", "\u0633\u0628\u062A\u0645\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0641\u0645\u0628\u0631", "\u062F\u064A\u0633\u0645\u0628\u0631", "" };
						abbreviatedMonthNames = new string[] { "\u064A\u0646\u0627\u064A\u0631", "\u0641\u0628\u0631\u0627\u064A\u0631", "\u0645\u0627\u0631\u0633", "\u0627\u0628\u0631\u064A\u0644", "\u0645\u0627\u064A\u0648", "\u064A\u0648\u0646\u064A\u0648", "\u064A\u0648\u0644\u064A\u0648", "\u0627\u063A\u0633\u0637\u0633", "\u0633\u0628\u062A\u0645\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0641\u0645\u0628\u0631", "\u062F\u064A\u0633\u0645\u0628\u0631", "" };
						firstDayOfWeek = (int)DayOfWeek.Saturday;
						return;
					}

				case 13321:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "M/d/yyyy";
						longDatePattern = "dddd, MMMM dd, yyyy";
						shortTimePattern = "h:mm tt";
						longTimePattern = "h:mm:ss tt";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dddd, MMMM dd, yyyy h:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
						monthNames = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						return;
					}

				case 13322:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "dd-MM-yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "H:mm";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mi\xe9", "jue", "vie", "s\xe1b" };
						dayNames = new string[] { "domingo", "lunes", "martes", "mi\xe9rcoles", "jueves", "viernes", "s\xe1bado" };
						monthNames = new string[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre", "" };
						abbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
						return;
					}

				case 14337:
					{
						amDesignator = "\u0635";
						pmDesignator = "\u0645";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM, yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM, yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 6;
						abbreviatedDayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						dayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						monthNames = new string[] { "\u064A\u0646\u0627\u064A\u0631", "\u0641\u0628\u0631\u0627\u064A\u0631", "\u0645\u0627\u0631\u0633", "\u0627\u0628\u0631\u064A\u0644", "\u0645\u0627\u064A\u0648", "\u064A\u0648\u0646\u064A\u0648", "\u064A\u0648\u0644\u064A\u0648", "\u0627\u063A\u0633\u0637\u0633", "\u0633\u0628\u062A\u0645\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0641\u0645\u0628\u0631", "\u062F\u064A\u0633\u0645\u0628\u0631", "" };
						abbreviatedMonthNames = new string[] { "\u064A\u0646\u0627\u064A\u0631", "\u0641\u0628\u0631\u0627\u064A\u0631", "\u0645\u0627\u0631\u0633", "\u0627\u0628\u0631\u064A\u0644", "\u0645\u0627\u064A\u0648", "\u064A\u0648\u0646\u064A\u0648", "\u064A\u0648\u0644\u064A\u0648", "\u0627\u063A\u0633\u0637\u0633", "\u0633\u0628\u062A\u0645\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0641\u0645\u0628\u0631", "\u062F\u064A\u0633\u0645\u0628\u0631", "" };
						firstDayOfWeek = (int)DayOfWeek.Saturday;
						return;
					}

				case 14346:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mi\xe9", "jue", "vie", "s\xe1b" };
						dayNames = new string[] { "domingo", "lunes", "martes", "mi\xe9rcoles", "jueves", "viernes", "s\xe1bado" };
						monthNames = new string[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre", "" };
						abbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 15361:
					{
						amDesignator = "\u0635";
						pmDesignator = "\u0645";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM, yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM, yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 6;
						abbreviatedDayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						dayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						monthNames = new string[] { "\u064A\u0646\u0627\u064A\u0631", "\u0641\u0628\u0631\u0627\u064A\u0631", "\u0645\u0627\u0631\u0633", "\u0627\u0628\u0631\u064A\u0644", "\u0645\u0627\u064A\u0648", "\u064A\u0648\u0646\u064A\u0648", "\u064A\u0648\u0644\u064A\u0648", "\u0627\u063A\u0633\u0637\u0633", "\u0633\u0628\u062A\u0645\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0641\u0645\u0628\u0631", "\u062F\u064A\u0633\u0645\u0628\u0631", "" };
						abbreviatedMonthNames = new string[] { "\u064A\u0646\u0627\u064A\u0631", "\u0641\u0628\u0631\u0627\u064A\u0631", "\u0645\u0627\u0631\u0633", "\u0627\u0628\u0631\u064A\u0644", "\u0645\u0627\u064A\u0648", "\u064A\u0648\u0646\u064A\u0648", "\u064A\u0648\u0644\u064A\u0648", "\u0627\u063A\u0633\u0637\u0633", "\u0633\u0628\u062A\u0645\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0641\u0645\u0628\u0631", "\u062F\u064A\u0633\u0645\u0628\u0631", "" };
						firstDayOfWeek = (int)DayOfWeek.Saturday;
						return;
					}

				case 15370:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mi\xe9", "jue", "vie", "s\xe1b" };
						dayNames = new string[] { "domingo", "lunes", "martes", "mi\xe9rcoles", "jueves", "viernes", "s\xe1bado" };
						monthNames = new string[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre", "" };
						abbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 16385:
					{
						amDesignator = "\u0635";
						pmDesignator = "\u0645";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM, yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "dd MMMM, yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 6;
						abbreviatedDayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						dayNames = new string[] { "\u0627\u0644\u0627\u062D\u062F", "\u0627\u0644\u0627\u062B\u0646\u064A\u0646", "\u0627\u0644\u062B\u0644\u0627\u062B\u0627\u0621", "\u0627\u0644\u0627\u0631\u0628\u0639\u0627\u0621", "\u0627\u0644\u062E\u0645\u064A\u0633", "\u0627\u0644\u062C\u0645\u0639\u0629", "\u0627\u0644\u0633\u0628\u062A" };
						monthNames = new string[] { "\u064A\u0646\u0627\u064A\u0631", "\u0641\u0628\u0631\u0627\u064A\u0631", "\u0645\u0627\u0631\u0633", "\u0627\u0628\u0631\u064A\u0644", "\u0645\u0627\u064A\u0648", "\u064A\u0648\u0646\u064A\u0648", "\u064A\u0648\u0644\u064A\u0648", "\u0627\u063A\u0633\u0637\u0633", "\u0633\u0628\u062A\u0645\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0641\u0645\u0628\u0631", "\u062F\u064A\u0633\u0645\u0628\u0631", "" };
						abbreviatedMonthNames = new string[] { "\u064A\u0646\u0627\u064A\u0631", "\u0641\u0628\u0631\u0627\u064A\u0631", "\u0645\u0627\u0631\u0633", "\u0627\u0628\u0631\u064A\u0644", "\u0645\u0627\u064A\u0648", "\u064A\u0648\u0646\u064A\u0648", "\u064A\u0648\u0644\u064A\u0648", "\u0627\u063A\u0633\u0637\u0633", "\u0633\u0628\u062A\u0645\u0628\u0631", "\u0627\u0643\u062A\u0648\u0628\u0631", "\u0646\u0648\u0641\u0645\u0628\u0631", "\u062F\u064A\u0633\u0645\u0628\u0631", "" };
						firstDayOfWeek = (int)DayOfWeek.Saturday;
						return;
					}

				case 16394:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mi\xe9", "jue", "vie", "s\xe1b" };
						dayNames = new string[] { "domingo", "lunes", "martes", "mi\xe9rcoles", "jueves", "viernes", "s\xe1bado" };
						monthNames = new string[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre", "" };
						abbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
						return;
					}

				case 17418:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mi\xe9", "jue", "vie", "s\xe1b" };
						dayNames = new string[] { "domingo", "lunes", "martes", "mi\xe9rcoles", "jueves", "viernes", "s\xe1bado" };
						monthNames = new string[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre", "" };
						abbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
						return;
					}

				case 18442:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mi\xe9", "jue", "vie", "s\xe1b" };
						dayNames = new string[] { "domingo", "lunes", "martes", "mi\xe9rcoles", "jueves", "viernes", "s\xe1bado" };
						monthNames = new string[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre", "" };
						abbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
						return;
					}

				case 19466:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mi\xe9", "jue", "vie", "s\xe1b" };
						dayNames = new string[] { "domingo", "lunes", "martes", "mi\xe9rcoles", "jueves", "viernes", "s\xe1bado" };
						monthNames = new string[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre", "" };
						abbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
						return;
					}

				case 20490:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "hh:mm tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "dd MMMM";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "dom", "lun", "mar", "mi\xe9", "jue", "vie", "s\xe1b" };
						dayNames = new string[] { "domingo", "lunes", "martes", "mi\xe9rcoles", "jueves", "viernes", "s\xe1bado" };
						monthNames = new string[] { "enero", "febrero", "marzo", "abril", "mayo", "junio", "julio", "agosto", "septiembre", "octubre", "noviembre", "diciembre", "" };
						abbreviatedMonthNames = new string[] { "ene", "feb", "mar", "abr", "may", "jun", "jul", "ago", "sep", "oct", "nov", "dic", "" };
						return;
					}

				case 6203:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "MMMM d'. b. 'yyyy";
						shortTimePattern = "HH:mm:ss";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "MMMM d'. b. 'yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "aej", "m\xe5a", "d\xe6j", "gask", "duar", "bearj", "laav" };
						dayNames = new string[] { "aejlege", "m\xe5anta", "d\xe6jsta", "gaskev\xe5hkoe", "duarsta", "bearjadahke", "laavvardahke" };
						monthNames = new string[] { "ts\xefengele", "goevte", "njoktje", "voerhtje", "suehpede", "ruffie", "snjaltje", "m\xefetske", "sk\xeferede", "golke", "rahka", "goeve", "" };
						abbreviatedMonthNames = new string[] { "ts\xefen", "goevt", "njok", "voer", "sueh", "ruff", "snja", "m\xefet", "sk\xefer", "golk", "rahk", "goev", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 7194:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "d.M.yyyy";
						longDatePattern = "d. MMMM yyyy";
						shortTimePattern = "H:mm:ss";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "d. MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u043D\u0435\u0434", "\u043F\u043E\u043D", "\u0443\u0442\u043E", "\u0441\u0440\u0435", "\u0447\u0435\u0442", "\u043F\u0435\u0442", "\u0441\u0443\u0431" };
						dayNames = new string[] { "\u043D\u0435\u0434\u0435\u0459\u0430", "\u043F\u043E\u043D\u0435\u0434\u0435\u0459\u0430\u043A", "\u0443\u0442\u043E\u0440\u0430\u043A", "\u0441\u0440\u0435\u0434\u0430", "\u0447\u0435\u0442\u0432\u0440\u0442\u0430\u043A", "\u043F\u0435\u0442\u0430\u043A", "\u0441\u0443\u0431\u043E\u0442\u0430" };
						monthNames = new string[] { "\u0458\u0430\u043D\u0443\u0430\u0440", "\u0444\u0435\u0431\u0440\u0443\u0430\u0440", "\u043C\u0430\u0440\u0442", "\u0430\u043F\u0440\u0438\u043B", "\u043C\u0430\u0458", "\u0458\u0443\u043D", "\u0458\u0443\u043B", "\u0430\u0432\u0433\u0443\u0441\u0442", "\u0441\u0435\u043F\u0442\u0435\u043C\u0431\u0430\u0440", "\u043E\u043A\u0442\u043E\u0431\u0430\u0440", "\u043D\u043E\u0432\u0435\u043C\u0431\u0430\u0440", "\u0434\u0435\u0446\u0435\u043C\u0431\u0430\u0440", "" };
						abbreviatedMonthNames = new string[] { "\u0458\u0430\u043D", "\u0444\u0435\u0431", "\u043C\u0430\u0440", "\u0430\u043F\u0440", "\u043C\u0430\u0458", "\u0458\u0443\u043D", "\u0458\u0443\u043B", "\u0430\u0432\u0433", "\u0441\u0435\u043F", "\u043E\u043A\u0442", "\u043D\u043E\u0432", "\u0434\u0435\u0446", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 1077:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "yyyy/MM/dd";
						longDatePattern = "dd MMMM yyyy";
						shortTimePattern = "hh:mm:ss tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dd MMMM yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "iSonto", "uMsombuluko", "uLwesibili", "uLwesithathu", "uLwesine", "uLwesihlanu", "uMgqibelo" };
						monthNames = new string[] { "uJanuwari", "uFebuwari", "uMashi", "uAprhili", "uMeyi", "uJuni", "uJulayi", "uAgaste", "uSepthemba", "uOkthoba", "uNovemba", "uDisemba", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						return;
					}

				case 1076:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "yyyy/MM/dd";
						longDatePattern = "dd MMMM yyyy";
						shortTimePattern = "hh:mm:ss tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dd MMMM yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "iCawa", "uMvulo", "uLwesibini", "uLwesithathu", "uLwesine", "uLwesihlanu", "uMgqibelo" };
						monthNames = new string[] { "eyoMqungu", "eyoMdumba", "eyoKwindla", "Tshazimpuzi", "Canzibe", "eyeSilimela", "eyeKhala", "eyeThupha", "eyoMsintsi", "eyeDwara", "eyeNkanga", "eyoMnga", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						return;
					}

				case 1074:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "yyyy/MM/dd";
						longDatePattern = "dd MMMM yyyy";
						shortTimePattern = "hh:mm:ss tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dd MMMM yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "Latshipi", "Mosupologo", "Labobedi", "Laboraro", "Labone", "Labotlhano", "Lamatlhatso" };
						monthNames = new string[] { "Ferikgong", "Tlhakole", "Mopitloe", "Moranang", "Motsheganong", "Seetebosigo", "Phukwi", "Phatwe", "Lwetse", "Diphalane", "Ngwanatsele", "Sedimothole", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						return;
					}

				case 2107:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "yyyy-MM-dd";
						longDatePattern = "MMMM d'. b. 'yyyy";
						shortTimePattern = "HH:mm:ss";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "MMMM d'. b. 'yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "sotn", "m\xe1n", "dis", "gask", "duor", "bear", "l\xe1v" };
						dayNames = new string[] { "sotnabeaivi", "m\xe1nnodat", "disdat", "gaskavahkku", "duorastat", "bearjadat", "l\xe1vvardat" };
						monthNames = new string[] { "o\u0111\u0111ajagem\xe1nnu", "guovvam\xe1nnu", "njuk\u010Dam\xe1nnu", "cuo\u014Bom\xe1nnu", "miessem\xe1nnu", "geassem\xe1nnu", "suoidnem\xe1nnu", "borgem\xe1nnu", "\u010Dak\u010Dam\xe1nnu", "golggotm\xe1nnu", "sk\xe1bmam\xe1nnu", "juovlam\xe1nnu", "" };
						abbreviatedMonthNames = new string[] { "o\u0111\u0111j", "guov", "njuk", "cuo", "mies", "geas", "suoi", "borg", "\u010Dak\u010D", "golg", "sk\xe1b", "juov", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 7227:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "yyyy-MM-dd";
						longDatePattern = "MMMM d'. b. 'yyyy";
						shortTimePattern = "HH:mm:ss";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "MMMM d'. b. 'yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "aej", "m\xe5a", "d\xe6j", "gask", "duar", "bearj", "laav" };
						dayNames = new string[] { "aejlege", "m\xe5anta", "d\xe6jsta", "gaskev\xe5hkoe", "duarsta", "bearjadahke", "laavvardahke" };
						monthNames = new string[] { "ts\xefengele", "goevte", "njoktje", "voerhtje", "suehpede", "ruffie", "snjaltje", "m\xefetske", "sk\xeferede", "golke", "rahka", "goeve", "" };
						abbreviatedMonthNames = new string[] { "ts\xefen", "goevt", "njok", "voer", "sueh", "ruff", "snja", "m\xefet", "sk\xefer", "golk", "rahk", "goev", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 4122:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "d.M.yyyy";
						longDatePattern = "d. MMMM yyyy";
						shortTimePattern = "H:mm:ss";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM, yyyy";
						fullDateTimePattern = "d. MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "ned", "pon", "uto", "sri", "\u010Det", "pet", "sub" };
						dayNames = new string[] { "nedjelja", "ponedjeljak", "utorak", "srijeda", "\u010Detvrtak", "petak", "subota" };
						monthNames = new string[] { "sije\u010Danj", "velja\u010Da", "o\u017Eujak", "travanj", "svibanj", "lipanj", "srpanj", "kolovoz", "rujan", "listopad", "studeni", "prosinac", "" };
						abbreviatedMonthNames = new string[] { "sij", "vlj", "o\u017Eu", "tra", "svi", "lip", "srp", "kol", "ruj", "lis", "stu", "pro", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 9275:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "d.M.yyyy";
						longDatePattern = "MMMM d'. p. 'yyyy";
						shortTimePattern = "H:mm:ss";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "MMMM d'. p. 'yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "pa", "vu", "ma", "ko", "tu", "v\xe1", "l\xe1" };
						dayNames = new string[] { "pasepeivi", "vuossarg\xe2", "majebarg\xe2", "koskokko", "tuor\xe2st\xe2h", "v\xe1stuppeivi", "l\xe1v\xe1rd\xe2h" };
						monthNames = new string[] { "u\u0111\u0111\xe2ivem\xe1\xe1nu", "kuov\xe2m\xe1\xe1nu", "njuh\u010D\xe2m\xe1\xe1nu", "cu\xe1\u014Buim\xe1\xe1nu", "vyesim\xe1\xe1nu", "kesim\xe1\xe1nu", "syeinim\xe1\xe1nu", "porgem\xe1\xe1nu", "\u010Doh\u010D\xe2m\xe1\xe1nu", "roovv\xe2dm\xe1\xe1nu", "skamm\xe2m\xe1\xe1nu", "juovl\xe2m\xe1\xe1nu", "" };
						abbreviatedMonthNames = new string[] { "u\u0111iv", "kuov", "njuh", "cuo\u014B", "vyes", "kesi", "syei", "porg", "\u010Doh", "roov", "ska", "juov", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 3179:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "hh:mm:ss tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "int", "kil", "ati", "quy", "Ch\x92", "Ill", "k'u" };
						dayNames = new string[] { "intichaw", "killachaw", "atipachaw", "quyllurchaw", "Ch' askachaw", "Illapachaw", "k'uychichaw" };
						monthNames = new string[] { "Qulla puquy", "Hatun puquy", "Pauqar waray", "ayriwa", "Aymuray", "Inti raymi", "Anta Sitwa", "Qhapaq Sitwa", "Uma raymi", "Kantaray", "Ayamarq'a", "Kapaq Raymi", "" };
						abbreviatedMonthNames = new string[] { "Qul", "Hat", "Pau", "ayr", "Aym", "Int", "Ant", "Qha", "Uma", "Kan", "Aya", "Kap", "" };
						return;
					}

				case 3131:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "d.M.yyyy";
						longDatePattern = "MMMM d'. b. 'yyyy";
						shortTimePattern = "H:mm:ss";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "MMMM d'. b. 'yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "sotn", "vuos", "ma\u014B", "gask", "duor", "bear", "l\xe1v" };
						dayNames = new string[] { "sotnabeaivi", "vuoss\xe1rga", "ma\u014B\u014Beb\xe1rga", "gaskavahkku", "duorastat", "bearjadat", "l\xe1vvardat" };
						monthNames = new string[] { "o\u0111\u0111ajagem\xe1nnu", "guovvam\xe1nnu", "njuk\u010Dam\xe1nnu", "cuo\u014Bom\xe1nnu", "miessem\xe1nnu", "geassem\xe1nnu", "suoidnem\xe1nnu", "borgem\xe1nnu", "\u010Dak\u010Dam\xe1nnu", "golggotm\xe1nnu", "sk\xe1bmam\xe1nnu", "juovlam\xe1nnu", "" };
						abbreviatedMonthNames = new string[] { "o\u0111\u0111j", "guov", "njuk", "cuo", "mies", "geas", "suoi", "borg", "\u010Dak\u010D", "golg", "sk\xe1b", "juov", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 8251:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "d.M.yyyy";
						longDatePattern = "MMMM d'. p. 'yyyy";
						shortTimePattern = "H:mm:ss";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "MMMM d'. p. 'yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "p\xe2", "vu", "m\xe2", "se", "ne", "pi", "su" };
						dayNames = new string[] { "p\xe2\xb4sspei\xb4vv", "vu\xf5ssargg", "m\xe2\xe2ibargg", "se\xe4rad", "nelljdpei\xb4vv", "pi\xe2tn\xe2c", "sue\xb4vet" };
						monthNames = new string[] { "o\u0111\u0111ee\xb4jjm\xe4\xe4n", "t\xe4\xb4lvvm\xe4\xe4n", "p\xe2\xb4zzl\xe2\u0161ttamm\xe4\xe4n", "njuh\u010D\u010Dm\xe4\xe4n", "vue\xb4ssm\xe4\xe4n", "\u01E9ie\xb4ssm\xe4\xe4n", "suei\xb4nnm\xe4\xe4n", "p\xe5\xb4r\u01E7\u01E7m\xe4\xe4n", "\u010D\xf5h\u010D\u010Dm\xe4\xe4n", "k\xe5lggm\xe4\xe4n", "skamm\xb4m\xe4\xe4n", "rosttovm\xe4\xe4n", "" };
						abbreviatedMonthNames = new string[] { "o\u0111jm", "t\xe4\xb4lvv", "p\xe2zl", "njuh", "vue", "\u01E9ie", "suei", "p\xe5\xb4r", "\u010D\xf5h", "k\xe5lg", "ska", "rost", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 1106:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dd MMMM yyyy";
						shortTimePattern = "HH:mm:ss";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dd MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "Sul", "Llun", "Maw", "Mer", "Iau", "Gwe", "Sad" };
						dayNames = new string[] { "Dydd Sul", "Dydd Llun", "Dydd Mawrth", "Dydd Mercher", "Dydd Iau", "Dydd Gwener", "Dydd Sadwrn" };
						monthNames = new string[] { "Ionawr", "Chwefror", "Mawrth", "Ebrill", "Mai", "Mehefin", "Gorffennaf", "Awst", "Medi", "Hydref", "Tachwedd", "Rhagfyr", "" };
						abbreviatedMonthNames = new string[] { "Ion", "Chwe", "Maw", "Ebr", "Mai", "Meh", "Gor", "Aws", "Med", "Hyd", "Tach", "Rhag", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 5146:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "d.M.yyyy";
						longDatePattern = "d. MMMM yyyy";
						shortTimePattern = "H:mm:ss";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d. MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "ned", "pon", "uto", "sri", "\u010Det", "pet", "sub" };
						dayNames = new string[] { "nedjelja", "ponedjeljak", "utorak", "srijeda", "\u010Detvrtak", "petak", "subota" };
						monthNames = new string[] { "januar", "februar", "mart", "april", "maj", "jun", "jul", "avgust", "septembar", "oktobar", "novembar", "decembar", "" };
						abbreviatedMonthNames = new string[] { "jan", "feb", "mar", "apr", "maj", "jun", "jul", "avg", "sep", "okt", "nov", "dec", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 4155:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "MMMM d'. b. 'yyyy";
						shortTimePattern = "HH:mm:ss";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "MMMM d'. b. 'yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "s\xe5d", "m\xe1n", "dis", "gas", "duor", "bier", "l\xe1v" };
						dayNames = new string[] { "s\xe5dn\xe5biejvve", "m\xe1nnodahka", "dijstahka", "gasskavahkko", "duorastahka", "bierjjedahka", "l\xe1vvodahka" };
						monthNames = new string[] { "\xe5d\xe5jakm\xe1nno", "guovvam\xe1nno", "sjnjuktjam\xe1nno", "vuoratjism\xe1nno", "moarmesm\xe1nno", "biehtsem\xe1nno", "sjnjilltjam\xe1nno", "b\xe5rggem\xe1nno", "rag\xe1tm\xe1nno", "g\xe5lg\xe5dism\xe1nno", "bas\xe1dism\xe1nno", "javllam\xe1nno", "" };
						abbreviatedMonthNames = new string[] { "\xe5d\xe5j", "guov", "snju", "vuor", "moar", "bieh", "snji", "b\xe5rg", "rag\xe1", "g\xe5lg", "bas\xe1", "javl", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 1153:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "d/MM/yyyy";
						longDatePattern = "dddd, d MMMM yyyy";
						shortTimePattern = "h:mm:ss tt";
						longTimePattern = "h:mm:ss tt";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd, d MMMM yyyy h:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "Ta", "Ma", "T\u016B", "We", "T\u0101i", "Pa", "H\u0101" };
						dayNames = new string[] { "R\u0101tapu", "Mane", "T\u016Brei", "Wenerei", "T\u0101ite", "Paraire", "H\u0101tarei" };
						monthNames = new string[] { "Kohi-t\u0101tea", "Hui-tanguru", "Pout\u016B-te-rangi", "Paenga-wh\u0101wh\u0101", "Haratua", "Pipiri", "H\u014Dngoingoi", "Here-turi-k\u014Dk\u0101", "Mahuru", "Whiringa-\u0101-nuku", "Whiringa-\u0101-rangi", "Hakihea", "" };
						abbreviatedMonthNames = new string[] { "Kohi", "Hui", "Pou", "Pae", "Hara", "Pipi", "H\u014Dngoi", "Here", "Mahu", "Whi-nu", "Whi-ra", "Haki", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 2155:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "H:mm:ss";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "int", "kil", "ati", "quy", "Ch\x92", "Ill", "k'u" };
						dayNames = new string[] { "intichaw", "killachaw", "atipachaw", "quyllurchaw", "Ch' askachaw", "Illapachaw", "k'uychichaw" };
						monthNames = new string[] { "Qulla puquy", "Hatun puquy", "Pauqar waray", "ayriwa", "Aymuray", "Inti raymi", "Anta Sitwa", "Qhapaq Sitwa", "Uma raymi", "Kantaray", "Ayamarq'a", "Kapaq Raymi", "" };
						abbreviatedMonthNames = new string[] { "Qul", "Hat", "Pau", "ayr", "Aym", "Int", "Ant", "Qha", "Uma", "Kan", "Aya", "Kap", "" };
						return;
					}

				case 6170:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "d.M.yyyy";
						longDatePattern = "d. MMMM yyyy";
						shortTimePattern = "H:mm:ss";
						longTimePattern = "H:mm:ss";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "d. MMMM yyyy H:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "ned", "pon", "uto", "sre", "\u010Det", "pet", "sub" };
						dayNames = new string[] { "nedelja", "ponedeljak", "utorak", "sreda", "\u010Detvrtak", "petak", "subota" };
						monthNames = new string[] { "januar", "februar", "mart", "april", "maj", "jun", "jul", "avgust", "septembar", "oktobar", "novembar", "decembar", "" };
						abbreviatedMonthNames = new string[] { "jan", "feb", "mar", "apr", "maj", "jun", "jul", "avg", "sep", "okt", "nov", "dec", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				case 5179:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = "-";
						timeSeparator = ":";
						shortDatePattern = "yyyy-MM-dd";
						longDatePattern = "MMMM d'. b. 'yyyy";
						shortTimePattern = "HH:mm:ss";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "MMMM d'. b. 'yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "\xe1jl", "m\xe1n", "dis", "gas", "duor", "bier", "l\xe1v" };
						dayNames = new string[] { "\xe1jllek", "m\xe1nnodahka", "dijstahka", "gasskavahkko", "duorastahka", "bierjjedahka", "l\xe1vvodahka" };
						monthNames = new string[] { "\xe5d\xe5jakm\xe1nno", "guovvam\xe1nno", "sjnjuktjam\xe1nno", "vuoratjism\xe1nno", "moarmesm\xe1nno", "biehtsem\xe1nno", "sjnjilltjam\xe1nno", "b\xe5rggem\xe1nno", "rag\xe1tm\xe1nno", "g\xe5lg\xe5dism\xe1nno", "bas\xe1dism\xe1nno", "javllam\xe1nno", "" };
						abbreviatedMonthNames = new string[] { "\xe5d\xe5j", "guov", "snju", "vuor", "moar", "bieh", "snji", "b\xe5rg", "rag\xe1", "g\xe5lg", "bas\xe1", "javl", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 1132:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "yyyy/MM/dd";
						longDatePattern = "dd MMMM yyyy";
						shortTimePattern = "hh:mm:ss tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dd MMMM yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
						dayNames = new string[] { "Lamorena", "Mo\u0161upologo", "Labobedi", "Laboraro", "Labone", "Labohlano", "Mokibelo" };
						monthNames = new string[] { "Pherekgong", "Hlakola", "Mopitlo", "Moranang", "Mosegamanye", "Ngoatobo\u0161ego", "Phuphu", "Phato", "Lewedi", "Diphalana", "Dibatsela", "Manthole", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec", "" };
						return;
					}

				case 1131:
					{
						amDesignator = "a.m.";
						pmDesignator = "p.m.";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, dd' de 'MMMM' de 'yyyy";
						shortTimePattern = "hh:mm:ss tt";
						longTimePattern = "hh:mm:ss tt";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM' de 'yyyy";
						fullDateTimePattern = "dddd, dd' de 'MMMM' de 'yyyy hh:mm:ss tt";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						abbreviatedDayNames = new string[] { "int", "kil", "ati", "quy", "Ch\x92", "Ill", "k'u" };
						dayNames = new string[] { "intichaw", "killachaw", "atipachaw", "quyllurchaw", "Ch' askachaw", "Illapachaw", "k'uychichaw" };
						monthNames = new string[] { "Qulla puquy", "Hatun puquy", "Pauqar waray", "ayriwa", "Aymuray", "Inti raymi", "Anta Sitwa", "Qhapaq Sitwa", "Uma raymi", "Kantaray", "Ayamarq'a", "Kapaq Raymi", "" };
						abbreviatedMonthNames = new string[] { "Qul", "Hat", "Pau", "ayr", "Aym", "Int", "Ant", "Qha", "Uma", "Kan", "Aya", "Kap", "" };
						return;
					}

				case 1083:
					{
						amDesignator = "";
						pmDesignator = "";
						dateSeparator = ".";
						timeSeparator = ":";
						shortDatePattern = "dd.MM.yyyy";
						longDatePattern = "MMMM d'. b. 'yyyy";
						shortTimePattern = "HH:mm:ss";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "MMMM d'. b. 'yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						calendarWeekRule = 2;
						abbreviatedDayNames = new string[] { "sotn", "vuos", "ma\u014B", "gask", "duor", "bear", "l\xe1v" };
						dayNames = new string[] { "sotnabeaivi", "vuoss\xe1rga", "ma\u014B\u014Beb\xe1rga", "gaskavahkku", "duorastat", "bearjadat", "l\xe1vvardat" };
						monthNames = new string[] { "o\u0111\u0111ajagem\xe1nnu", "guovvam\xe1nnu", "njuk\u010Dam\xe1nnu", "cuo\u014Bom\xe1nnu", "miessem\xe1nnu", "geassem\xe1nnu", "suoidnem\xe1nnu", "borgem\xe1nnu", "\u010Dak\u010Dam\xe1nnu", "golggotm\xe1nnu", "sk\xe1bmam\xe1nnu", "juovlam\xe1nnu", "" };
						abbreviatedMonthNames = new string[] { "o\u0111\u0111j", "guov", "njuk", "cuo", "mies", "geas", "suoi", "borg", "\u010Dak\u010D", "golg", "sk\xe1b", "juov", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						calendarWeekRule = (int)CalendarWeekRule.FirstFourDayWeek;
						return;
					}

				case 1082:
					{
						amDesignator = "AM";
						pmDesignator = "PM";
						dateSeparator = "/";
						timeSeparator = ":";
						shortDatePattern = "dd/MM/yyyy";
						longDatePattern = "dddd, d' ta\\' 'MMMM yyyy";
						shortTimePattern = "HH:mm:ss";
						longTimePattern = "HH:mm:ss";
						monthDayPattern = "MMMM dd";
						yearMonthPattern = "MMMM yyyy";
						fullDateTimePattern = "dddd, d' ta\\' 'MMMM yyyy HH:mm:ss";
						_RFC1123Pattern = "ddd, dd MMM yyyy HH':'mm':'ss 'GMT'";
						_SortableDateTimePattern = "yyyy'-'MM'-'dd'T'HH':'mm':'ss";
						_UniversalSortableDateTimePattern = "yyyy'-'MM'-'dd HH':'mm':'ss'Z'";
						firstDayOfWeek = 1;
						abbreviatedDayNames = new string[] { "\u0126ad", "Tne", "Tli", "Erb", "\u0126am", "\u0120im", "Sib" };
						dayNames = new string[] { "Il-\u0126add", "It-Tnejn", "It-Tlieta", "L-Erbg\u0127a", "Il-\u0126amis", "Il-\u0120img\u0127a", "Is-Sibt" };
						monthNames = new string[] { "Jannar", "Frar", "Marzu", "April", "Mejju", "\u0120unju", "Lulju", "Awissu", "Settembru", "Ottubru", "Novembru", "Di\u010Bembru", "" };
						abbreviatedMonthNames = new string[] { "Jan", "Fra", "Mar", "Apr", "Mej", "\u0120un", "Lul", "Awi", "Set", "Ott", "Nov", "Di\u010B", "" };
						firstDayOfWeek = (int)DayOfWeek.Monday;
						return;
					}

				default:
					throw new NotSupportedException(String.Format("Culture ID {0} (0x{0:X4}) is not a supported culture.", lcid));
			}
		}
	}
}
