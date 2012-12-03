#region Constructors
//see: http://livedocs.adobe.com/flex/201/langref/index.html

//If you pass no arguments, the Date object is assigned the current date and time.
[MethodImpl(MethodImplOptions.InternalCall)]
public extern Date();

//If you pass one argument of data type Number, the Date object is assigned a time value based 
//on the number of milliseconds since January 1, 1970 0:00:000 GMT, as specified by the lone argument.
[MethodImpl(MethodImplOptions.InternalCall)]
public extern Date(int timevalue);

//If you pass one argument of data type String, and the string contains a valid date, 
//the Date object contains a time value based on that date.
[MethodImpl(MethodImplOptions.InternalCall)]
public extern Date(String timevalue);

//If you pass two or more arguments, the Date object is assigned a time value based on the argument values 
//passed, which represent the date's year, month, date, hour, minute, second, and milliseconds.
//Defaults: date = 1, hour = 0, minute = 0, second = 0, millisecond = 0
[MethodImpl(MethodImplOptions.InternalCall)]
public extern Date(int year, int month);

[MethodImpl(MethodImplOptions.InternalCall)]
public extern Date(int year, int month, int date);

[MethodImpl(MethodImplOptions.InternalCall)]
public extern Date(int year, int month, int date, int hour, int minute, int second);

[MethodImpl(MethodImplOptions.InternalCall)]
public extern Date(int year, int month, int date, int hour, int minute, int second, int millisecond);
#endregion

#region Custom Members
public extern virtual int DayOfMonth
{
    [PageFX.ABC]
    [PageFX.QName("date")]
    [MethodImpl(MethodImplOptions.InternalCall)]
    get;

    [PageFX.ABC]
    [PageFX.QName("date")]
    [MethodImpl(MethodImplOptions.InternalCall)]
    set;
}
#endregion