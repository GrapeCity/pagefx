namespace DataDynamics.PageFX.FlashLand.Swf.Actions
{
    /// <summary>
    /// Enumerates SWF action codes
    /// </summary>
    public enum SwfActionCode : byte
    {
        #region 3.0
        /// <summary>
        /// End a record of actions
        /// </summary>
        [SwfVersion(3)]
        End = 0x00,

        /// <summary>
        /// Go to the next frame
        /// </summary>
        [SwfVersion(3)]
        NextFrame = 0x04,

        /// <summary>
        /// Go to the previous frame
        /// </summary>
        [SwfVersion(3)]
        PreviousFrame = 0x05,

        /// <summary>
        /// Enter the standard (default) auto-loop playback
        /// </summary>
        [SwfVersion(3)]
        Play = 0x06,

        /// <summary>
        /// Stop playing. Only a button (or the plugin menu) can be used to restart the movie
        /// </summary>
        [SwfVersion(3)]
        Stop = 0x07,

        /// <summary>
        /// Change the quality level from low to high and vice versa. At this time, not sure what happens if you use medium!
        /// </summary>
        [SwfVersion(3)]
        ToggleQuality = 0x08,

        /// <summary>
        /// Stop playing any sound effect.
        /// </summary>
        [SwfVersion(3)]
        StopSounds = 0x09,

        /// <summary>
        /// Instructs Flash Player to go to the specified frame in the current file.
        /// </summary>
        [SwfVersion(3)]
        GotoFrame = 0x81,

        /// <summary>
        /// Instructs Flash Player to change the context of subsequent actions, so they apply to a named object (TargetName) rather than the current file.
        /// </summary>
        [SwfVersion(3)]
        SetTarget = 0x8B,

        /// <summary>
        /// Instructs Flash Player to go to the frame associated with the specified label.
        /// </summary>
        [SwfVersion(3)]
        GotoLabel = 0x8C,
        #endregion

        #region 4.0
        #region Stack operations
        /// <summary>
        /// Push some immediate data on the stack.
        /// </summary>
        [SwfVersion(4)]
        Push = 0x96,

        /// <summary>
        /// Pops one value from the stack and forget it.
        /// </summary>
        [SwfVersion(4)]
        Pop = 0x17,
        #endregion

        #region Arithmetic Operations
        /// <summary>
        /// Pops two values, add them and put the result back on the stack.
        /// </summary>
        [SwfVersion(4)]
        Add = 0x0A,

        /// <summary>
        /// Pops two values, subtract them and put the result back on the stack.
        /// </summary>
        [SwfVersion(4)]
        Subtract = 0x0B,

        /// <summary>
        /// Pops two values, multiply them and put the result back on the stack.
        /// </summary>
        [SwfVersion(4)]
        Multiply = 0x0C,

        /// <summary>
        /// Pops two values, divide them and put the result back on the stack.
        /// </summary>
        [SwfVersion(4)]
        Divide = 0x0D,
        #endregion

        #region Numerical Comparison
        /// <summary>
        /// Pops two values, compare them for equality and put the boolean result back on the stack.
        /// </summary>
        [SwfVersion(4)]
        Equal = 0x0E,

        /// <summary>
        /// Pops two values, compare them for inequality and put the boolean result back on the stack.
        /// </summary>
        [SwfVersion(4)]
        Less = 0x0F,
        #endregion

        #region Logical Operators
        /// <summary>
        /// Pops two values, compute the Logical AND and put the boolean result back on the stack.
        /// </summary>
        [SwfVersion(4)]
        And = 0x10,

        /// <summary>
        /// Pops two values, compute the Logical OR and put the boolean result back on the stack.
        /// </summary>
        [SwfVersion(4)]
        Or = 0x11,

        /// <summary>
        /// Pops one value, compute the Logical NOT and put the result back on the stack.
        /// </summary>
        [SwfVersion(4)]
        Not = 0x12,
        #endregion

        #region String manipulation
        /// <summary>
        /// Pops two strings, compute the equality and put the boolean result back on the stack.
        /// </summary>
        [SwfVersion(4)]
        StringEqual = 0x13,

        /// <summary>
        /// Pops two strings, concatenate them, push the result on the stack.
        /// </summary>
        [SwfVersion(4)]
        StringAdd = 0x21,

        /// <summary>
        /// Pops two strings, compute the equality and put the boolean result back on the stack.
        /// </summary>
        [SwfVersion(4)]
        StringLength = 0x14,

        /// <summary>
        /// Pops two values and one string, the first value is the new string size 
        /// (at most that many characters) and the second value is the index (1 based) 
        /// of the first character to start the copy from. 
        /// The resulting string is pushed back on the stack.
        /// </summary>
        [SwfVersion(4)]
        StringExtract = 0x15,

        /// <summary>
        /// Pops two strings, compare them, push the result back on the stack.
        /// </summary>
        [SwfVersion(4)]
        StringLess = 0x29,

        [SwfVersion(4)]
        MBStringLength = 0x31,

        [SwfVersion(4)]
        MBStringExtract = 0x35,
        #endregion

        #region Type conversion
        /// <summary>
        /// Pops one value, transform in an integer, and push the result back on the stack.
        /// </summary>
        [SwfVersion(4)]
        ToInteger = 0x18,

        [SwfVersion(4)]
        CharToAscii = 0x32,

        [SwfVersion(4)]
        AsciiToChar = 0x33,

        [SwfVersion(4)]
        MBCharToAscii = 0x36,

        [SwfVersion(4)]
        MBAsciiToChar = 0x37,
        #endregion

        #region Control Flow
        /// <summary>
        /// Creates an unconditional branch.
        /// </summary>
        [SwfVersion(4)]
        Jump = 0x99,

        /// <summary>
        /// Creates a conditional test and branch.
        /// </summary>
        [SwfVersion(4)]
        If = 0x9D,

        /// <summary>
        /// Calls a subroutine.
        /// </summary>
        [SwfVersion(4)]
        Call = 0x9E,
        #endregion

        #region Variables
        /// <summary>
        /// Pops one string, search for a variable of that name, and push its value on the stack. 
        /// The variable name can include sprite names separated by slashes and finished by a colon as in: /Sprite1/Sprite2:MyVar
        /// </summary>
        [SwfVersion(4)]
        GetVariable = 0x1C,

        /// <summary>
        /// Pops one value and one string, set the variable of that name with that value. Nothing is pushed back on the stack.
        /// </summary>
        [SwfVersion(4)]
        SetVariable = 0x1D,
        #endregion

        #region Movie control
        /// <summary>
        /// Gets a URL and is stack based.
        /// </summary>
        [SwfVersion(4)]
        GetURL2 = 0x9A,

        /// <summary>
        /// Goes to a frame and is stack based.
        /// </summary>
        [SwfVersion(4)]
        GotoFrame2 = 0x9F,

        /// <summary>
        /// Pops the target off the stack and makes it the current active context.
        /// </summary>
        [SwfVersion(4)]
        SetTarget2 = 0x20,

        /// <summary>
        /// Query the property 'n1' of the object named 's2' (a field in a structure if you wish), and push the result on the stack.
        /// </summary>
        [SwfVersion(4)]
        GetProperty = 0x22,

        /// <summary>
        /// Pop a value from the stack and use it to set the specified named object at the specified field property.
        /// </summary>
        [SwfVersion(4)]
        SetProperty = 0x23,

        /// <summary>
        /// Clones a sprite.
        /// </summary>
        [SwfVersion(4)]
        CloneSprite = 0x24,

        /// <summary>
        /// Pop the string 's1' with the name of the sprite to be removed from view.
        /// </summary>
        [SwfVersion(4)]
        RemoveSprite = 0x25,

        /// <summary>
        /// Starts dragging a movie clip.
        /// </summary>
        [SwfVersion(4)]
        StartDrag = 0x27,

        /// <summary>
        /// Ends the drag operation in progress, if any.
        /// </summary>
        [SwfVersion(4)]
        EndDrag = 0x28,

        /// <summary>
        /// Waits for a frame to be loaded and is stack based.
        /// </summary>
        [SwfVersion(4)]
        WaitForFrame2 = 0x8D,
        #endregion

        #region Utilities
        /// <summary>
        /// Print out string s1 in debugger output window. Ignored otherwise.
        /// </summary>
        [SwfVersion(4)]
        Trace = 0x26,

        /// <summary>
        /// reports the milliseconds since the Macromedia Flash Player started.
        /// </summary>
        [SwfVersion(4)]
        GetTime = 0x34,

        /// <summary>
        /// Calculates a random number.
        /// </summary>
        [SwfVersion(4)]
        RandomNumber = 0x30,
        #endregion
        #endregion

        #region 5.0
        #region ScriptObject actions
        /// <summary>
        /// Executes a function.
        /// </summary>
        [SwfVersion(5)]
        CallFunction = 0x3D,

        [SwfVersion(5)]
        CallMethod = 0x52,

        /// <summary>
        /// Creates a new constant pool, and replaces the old constant pool if one already exists.
        /// </summary>
        [SwfVersion(5)]
        ConstantPool = 0x88,

        /// <summary>
        /// Defines a function with a given name and body size.
        /// </summary>
        [SwfVersion(5)]
        DefineFunction = 0x9B,

        /// <summary>
        /// Defines a local variable and sets its value.
        /// </summary>
        [SwfVersion(5)]
        DefineLocal = 0x3C,

        /// <summary>
        /// Defines a local variable without setting its value.
        /// </summary>
        [SwfVersion(5)]
        DefineLocal2 = 0x41,

        /// <summary>
        /// Deletes a named property from a ScriptObject.
        /// </summary>
        [SwfVersion(5)]
        Delete = 0x3A,

        /// <summary>
        /// Deletes a named property.
        /// </summary>
        [SwfVersion(5)]
        Delete2 = 0x3B,

        /// <summary>
        /// Obtains the names of all "slots" in use in an ActionScript object.
        /// </summary>
        [SwfVersion(5)]
        Enumerate = 0x46,

        /// <summary>
        /// Equals2 is similar to Equals, but Equals2 knows about types.
        /// </summary>
        [SwfVersion(5)]
        Equals2 = 0x49,

        /// <summary>
        /// Retrieves a named property from an object, and pushes the value of the property onto the stack.
        /// </summary>
        [SwfVersion(5)]
        GetMember = 0x4E,

        /// <summary>
        /// Initializes an array in a ScriptObject.
        /// </summary>
        [SwfVersion(5)]
        InitArray = 0x42,

        /// <summary>
        /// Initializes an object.
        /// </summary>
        [SwfVersion(5)]
        InitObject = 0x43,

        /// <summary>
        /// Invokes a constructor function to create a new object.
        /// </summary>
        [SwfVersion(5)]
        NewMethod = 0x53,

        /// <summary>
        /// Invokes a constructor function.
        /// </summary>
        [SwfVersion(5)]
        NewObject = 0x40,

        /// <summary>
        /// Sets a property of an object.
        /// </summary>
        [SwfVersion(5)]
        SetMember = 0x4F,

        [SwfVersion(5)]
        TargetPath = 0x45,

        /// <summary>
        /// Defines a With block of script.
        /// </summary>
        [SwfVersion(5)]
        With = 0x94,
        #endregion

        #region Type conversion
        /// <summary>
        /// Converts the object on the top of the stack into a number, and pushes the number back to the stack.
        /// </summary>
        [SwfVersion(5)]
        ToNumber = 0x4A,

        /// <summary>
        /// Converts the object on the top of the stack into a String, and pushes the string back to the stack.
        /// </summary>
        [SwfVersion(5)]
        ActionToString = 0x4B,

        /// <summary>
        /// Pushes the object type to the stack.
        /// </summary>
        [SwfVersion(5)]
        TypeOf = 0x44,
        #endregion

        #region Math
        [SwfVersion(5)]
        Modulo = 0x3F,

        [SwfVersion(5)]
        Add2 = 0x47,

        [SwfVersion(5)]
        Less2 = 0x48,

        [SwfVersion(5)]
        Increment = 0x50,

        [SwfVersion(5)]
        Decrement = 0x51,
        #endregion

        #region Bit Math
        [SwfVersion(5)]
        BitAnd = 0x60,

        [SwfVersion(5)]
        BitOr = 0x61,

        [SwfVersion(5)]
        BitXor = 0x62,

        [SwfVersion(5)]
        BitLShift = 0x63,

        [SwfVersion(5)]
        BitRShift = 0x64,

        [SwfVersion(5)]
        BitURShift = 0x65,
        #endregion

        [SwfVersion(5)]
        PushDuplicate = 0x4C,

        [SwfVersion(5)]
        Return = 0x3E,

        [SwfVersion(5)]
        Swap = 0x4D,

        [SwfVersion(5)]
        StoreRegister = 0x87,
        #endregion

        #region 6.0
        [SwfVersion(6)]
        InstanceOf = 0x54,

        [SwfVersion(6)]
        Enumerate2 = 0x55,

        [SwfVersion(6)]
        StrictEquals = 0x66,

        [SwfVersion(6)]
        Greater = 0x67,

        [SwfVersion(6)]
        StringGreater = 0x68,
        #endregion

        #region 7.0
        [SwfVersion(7)]
        DefineFunction2 = 0x8E,

        [SwfVersion(7)]
        Extends = 0x69,

        [SwfVersion(7)]
        CastOp = 0x2B,

        [SwfVersion(7)]
        ImplementsOp = 0x2C,

        [SwfVersion(7)]
        Try = 0x8F,

        [SwfVersion(7)]
        Throw = 0x2A,
        #endregion
    }
}