//
// WARNING: Automatically generated file. DO NOT EDIT
//

namespace DataDynamics.PageFX.Flash.IL
{
    /// <summary>
    /// Enumerates AVM2 instruction codes
    /// </summary>
    public enum InstructionCode : byte
    {
        /// <summary>
        /// 
        /// </summary>
        OP_0x00 = 0x00,

        /// <summary>
        /// Debugger break point
        /// </summary>
        Bkpt = 0x01,

        /// <summary>
        /// Do nothing.
        /// </summary>
        Nop = 0x02,

        /// <summary>
        /// Throws an exception.
        /// </summary>
        Throw = 0x03,

        /// <summary>
        /// Gets a property from a base class.
        /// </summary>
        Getsuper = 0x04,

        /// <summary>
        /// Sets a property in a base class.
        /// </summary>
        Setsuper = 0x05,

        /// <summary>
        /// Sets the default XML namespace.
        /// </summary>
        Dxns = 0x06,

        /// <summary>
        /// Sets the default XML namespace with a value determined at runtime.
        /// </summary>
        Dxnslate = 0x07,

        /// <summary>
        /// Kills a local register.
        /// </summary>
        Kill = 0x08,

        /// <summary>
        /// Do nothing. Used to indicate that this location is the target of a branch.
        /// </summary>
        Label = 0x09,

        /// <summary>
        /// 
        /// </summary>
        OP_0x0A = 0x0A,

        /// <summary>
        /// 
        /// </summary>
        OP_0x0B = 0x0B,

        /// <summary>
        /// Branch if the first value is not less than the second value.
        /// </summary>
        Ifnlt = 0x0C,

        /// <summary>
        /// Branch if the first value is not less than or equal to the second value.
        /// </summary>
        Ifnle = 0x0D,

        /// <summary>
        /// Branch if the first value is not greater than the second value.
        /// </summary>
        Ifngt = 0x0E,

        /// <summary>
        /// Branch if the first value is not greater than or equal to the second value.
        /// </summary>
        Ifnge = 0x0F,

        /// <summary>
        /// Unconditional branch.
        /// </summary>
        Jump = 0x10,

        /// <summary>
        /// Branch if true.
        /// </summary>
        Iftrue = 0x11,

        /// <summary>
        /// Branch if false.
        /// </summary>
        Iffalse = 0x12,

        /// <summary>
        /// Branch if the first value is equal to the second value.
        /// </summary>
        Ifeq = 0x13,

        /// <summary>
        /// Branch if the first value is not equal to the second value.
        /// </summary>
        Ifne = 0x14,

        /// <summary>
        /// Branch if the first value is less than the second value.
        /// </summary>
        Iflt = 0x15,

        /// <summary>
        /// Branch if the first value is less than or equal to the second value.
        /// </summary>
        Ifle = 0x16,

        /// <summary>
        /// Branch if the first value is greater than the second value.
        /// </summary>
        Ifgt = 0x17,

        /// <summary>
        /// Branch if the first value is greater than or equal to the second value.
        /// </summary>
        Ifge = 0x18,

        /// <summary>
        /// Branch if the first value is equal to the second value.
        /// </summary>
        Ifstricteq = 0x19,

        /// <summary>
        /// Branch if the first value is not equal to the second value.
        /// </summary>
        Ifstrictne = 0x1A,

        /// <summary>
        /// Jump to different locations based on an index.
        /// </summary>
        Lookupswitch = 0x1B,

        /// <summary>
        /// Push a with scope onto the scope stack
        /// </summary>
        Pushwith = 0x1C,

        /// <summary>
        /// Pop a scope off of the scope stack
        /// </summary>
        Popscope = 0x1D,

        /// <summary>
        /// Get the name of the next property when iterating over an object.
        /// </summary>
        Nextname = 0x1E,

        /// <summary>
        /// Determine if the given object has any more properties.
        /// </summary>
        Hasnext = 0x1F,

        /// <summary>
        /// Push null.
        /// </summary>
        Pushnull = 0x20,

        /// <summary>
        /// Push undefined.
        /// </summary>
        Pushundefined = 0x21,

        /// <summary>
        /// 
        /// </summary>
        OP_0x22 = 0x22,

        /// <summary>
        /// Get the name of the next property when iterating over an object.
        /// </summary>
        Nextvalue = 0x23,

        /// <summary>
        /// Push a byte value.
        /// </summary>
        Pushbyte = 0x24,

        /// <summary>
        /// Push a short value.
        /// </summary>
        Pushshort = 0x25,

        /// <summary>
        /// Push true.
        /// </summary>
        Pushtrue = 0x26,

        /// <summary>
        /// Push false.
        /// </summary>
        Pushfalse = 0x27,

        /// <summary>
        /// Push NaN.
        /// </summary>
        Pushnan = 0x28,

        /// <summary>
        /// Pop the top value from the stack.
        /// </summary>
        Pop = 0x29,

        /// <summary>
        /// Duplicates the top value on the stack.
        /// </summary>
        Dup = 0x2A,

        /// <summary>
        /// Swap the top two operands on the stack
        /// </summary>
        Swap = 0x2B,

        /// <summary>
        /// Push a string value onto the stack.
        /// </summary>
        Pushstring = 0x2C,

        /// <summary>
        /// Push an int value onto the stack.
        /// </summary>
        Pushint = 0x2D,

        /// <summary>
        /// Push an unsigned int value onto the stack.
        /// </summary>
        Pushuint = 0x2E,

        /// <summary>
        /// Push a double value onto the stack.
        /// </summary>
        Pushdouble = 0x2F,

        /// <summary>
        /// Push an object onto the scope stack.
        /// </summary>
        Pushscope = 0x30,

        /// <summary>
        /// Push a namespace.
        /// </summary>
        Pushnamespace = 0x31,

        /// <summary>
        /// Determine if the given object has any more properties.
        /// </summary>
        Hasnext2 = 0x32,

        /// <summary>
        /// 
        /// </summary>
        OP_0x33 = 0x33,

        /// <summary>
        /// 
        /// </summary>
        OP_0x34 = 0x34,

        /// <summary>
        /// 
        /// </summary>
        OP_0x35 = 0x35,

        /// <summary>
        /// 
        /// </summary>
        OP_0x36 = 0x36,

        /// <summary>
        /// 
        /// </summary>
        OP_0x37 = 0x37,

        /// <summary>
        /// 
        /// </summary>
        OP_0x38 = 0x38,

        /// <summary>
        /// 
        /// </summary>
        OP_0x39 = 0x39,

        /// <summary>
        /// 
        /// </summary>
        OP_0x3A = 0x3A,

        /// <summary>
        /// 
        /// </summary>
        OP_0x3B = 0x3B,

        /// <summary>
        /// 
        /// </summary>
        OP_0x3C = 0x3C,

        /// <summary>
        /// 
        /// </summary>
        OP_0x3D = 0x3D,

        /// <summary>
        /// 
        /// </summary>
        OP_0x3E = 0x3E,

        /// <summary>
        /// 
        /// </summary>
        OP_0x3F = 0x3F,

        /// <summary>
        /// Create a new function object.
        /// </summary>
        Newfunction = 0x40,

        /// <summary>
        /// Call a closure.
        /// </summary>
        Call = 0x41,

        /// <summary>
        /// Construct an instance.
        /// </summary>
        Construct = 0x42,

        /// <summary>
        /// Call a method identified by index in the object’s method table.
        /// </summary>
        Callmethod = 0x43,

        /// <summary>
        /// Call a method identified by index in the abcFile method table.
        /// </summary>
        Callstatic = 0x44,

        /// <summary>
        /// Call a method on a base class.
        /// </summary>
        Callsuper = 0x45,

        /// <summary>
        /// Call a property.
        /// </summary>
        Callproperty = 0x46,

        /// <summary>
        /// Return from a method.
        /// </summary>
        Returnvoid = 0x47,

        /// <summary>
        /// Return from the currently executing method.
        /// </summary>
        Returnvalue = 0x48,

        /// <summary>
        /// Construct an instance of the base class.
        /// </summary>
        Constructsuper = 0x49,

        /// <summary>
        /// Construct a property.
        /// </summary>
        Constructprop = 0x4A,

        /// <summary>
        /// 
        /// </summary>
        Callsuperid = 0x4B,

        /// <summary>
        /// Call a property.
        /// </summary>
        Callproplex = 0x4C,

        /// <summary>
        /// 
        /// </summary>
        Callinterface = 0x4D,

        /// <summary>
        /// Call a method on a base class, discarding the return value.
        /// </summary>
        Callsupervoid = 0x4E,

        /// <summary>
        /// Call a property, discarding the return value.
        /// </summary>
        Callpropvoid = 0x4F,

        /// <summary>
        /// 
        /// </summary>
        OP_0x50 = 0x50,

        /// <summary>
        /// 
        /// </summary>
        OP_0x51 = 0x51,

        /// <summary>
        /// 
        /// </summary>
        OP_0x52 = 0x52,

        /// <summary>
        /// Apply type params onto thet stack to parametrized type
        /// </summary>
        Applytype = 0x53,

        /// <summary>
        /// 
        /// </summary>
        OP_0x54 = 0x54,

        /// <summary>
        /// Create a new object.
        /// </summary>
        Newobject = 0x55,

        /// <summary>
        /// Create a new array.
        /// </summary>
        Newarray = 0x56,

        /// <summary>
        /// Create a new activation object.
        /// </summary>
        Newactivation = 0x57,

        /// <summary>
        /// Create a new class.
        /// </summary>
        Newclass = 0x58,

        /// <summary>
        /// Get descendants.
        /// </summary>
        Getdescendants = 0x59,

        /// <summary>
        /// Create a new catch scope.
        /// </summary>
        Newcatch = 0x5A,

        /// <summary>
        /// 
        /// </summary>
        OP_0x5B = 0x5B,

        /// <summary>
        /// 
        /// </summary>
        OP_0x5C = 0x5C,

        /// <summary>
        /// Find a property.
        /// </summary>
        Findpropstrict = 0x5D,

        /// <summary>
        /// Search the scope stack for a property.
        /// </summary>
        Findproperty = 0x5E,

        /// <summary>
        /// Find definition
        /// </summary>
        Finddef = 0x5F,

        /// <summary>
        /// Find and get a property.
        /// </summary>
        Getlex = 0x60,

        /// <summary>
        /// Set a property.
        /// </summary>
        Setproperty = 0x61,

        /// <summary>
        /// Get a local register.
        /// </summary>
        Getlocal = 0x62,

        /// <summary>
        /// Set a local register.
        /// </summary>
        Setlocal = 0x63,

        /// <summary>
        /// Gets the global scope.
        /// </summary>
        Getglobalscope = 0x64,

        /// <summary>
        /// Get a scope object.
        /// </summary>
        Getscopeobject = 0x65,

        /// <summary>
        /// Get a property.
        /// </summary>
        Getproperty = 0x66,

        /// <summary>
        /// 
        /// </summary>
        OP_0x67 = 0x67,

        /// <summary>
        /// Initialize a property.
        /// </summary>
        Initproperty = 0x68,

        /// <summary>
        /// 
        /// </summary>
        OP_0x69 = 0x69,

        /// <summary>
        /// Delete a property.
        /// </summary>
        Deleteproperty = 0x6A,

        /// <summary>
        /// 
        /// </summary>
        OP_0x6B = 0x6B,

        /// <summary>
        /// Get the value of a slot.
        /// </summary>
        Getslot = 0x6C,

        /// <summary>
        /// Set the value of a slot.
        /// </summary>
        Setslot = 0x6D,

        /// <summary>
        /// Get the value of a slot on the global scope.
        /// </summary>
        Getglobalslot = 0x6E,

        /// <summary>
        /// Set the value of a slot on the global scope.
        /// </summary>
        Setglobalslot = 0x6F,

        /// <summary>
        /// Convert a value to a string.
        /// </summary>
        Convert_s = 0x70,

        /// <summary>
        /// Escape an xml element.
        /// </summary>
        Esc_xelem = 0x71,

        /// <summary>
        /// Escape an xml attribute.
        /// </summary>
        Esc_xattr = 0x72,

        /// <summary>
        /// Convert a value to an integer.
        /// </summary>
        Convert_i = 0x73,

        /// <summary>
        /// Convert a value to an unsigned integer.
        /// </summary>
        Convert_u = 0x74,

        /// <summary>
        /// Convert a value to a double.
        /// </summary>
        Convert_d = 0x75,

        /// <summary>
        /// Convert a value to a Boolean.
        /// </summary>
        Convert_b = 0x76,

        /// <summary>
        /// Convert a value to an Object.
        /// </summary>
        Convert_o = 0x77,

        /// <summary>
        /// Check to make sure an object can have a filter operation performed on it.
        /// </summary>
        Checkfilter = 0x78,

        /// <summary>
        /// 
        /// </summary>
        OP_0x79 = 0x79,

        /// <summary>
        /// 
        /// </summary>
        OP_0x7A = 0x7A,

        /// <summary>
        /// 
        /// </summary>
        OP_0x7B = 0x7B,

        /// <summary>
        /// 
        /// </summary>
        OP_0x7C = 0x7C,

        /// <summary>
        /// 
        /// </summary>
        OP_0x7D = 0x7D,

        /// <summary>
        /// 
        /// </summary>
        OP_0x7E = 0x7E,

        /// <summary>
        /// 
        /// </summary>
        OP_0x7F = 0x7F,

        /// <summary>
        /// Coerce a value to a specified type
        /// </summary>
        Coerce = 0x80,

        /// <summary>
        /// Coerce a value to a Boolean.
        /// </summary>
        Coerce_b = 0x81,

        /// <summary>
        /// Coerce a value to the any type.
        /// </summary>
        Coerce_a = 0x82,

        /// <summary>
        /// Coerce a value to a int.
        /// </summary>
        Coerce_i = 0x83,

        /// <summary>
        /// Coerce a value to a double.
        /// </summary>
        Coerce_d = 0x84,

        /// <summary>
        /// Coerce a value to a string.
        /// </summary>
        Coerce_s = 0x85,

        /// <summary>
        /// Return the same value, or null if not of the specified type
        /// </summary>
        Astype = 0x86,

        /// <summary>
        /// Return the same value, or null if not of the specified type
        /// </summary>
        Astypelate = 0x87,

        /// <summary>
        /// Coerce a value to a uint.
        /// </summary>
        Coerce_u = 0x88,

        /// <summary>
        /// Coerce a value to an Object.
        /// </summary>
        Coerce_o = 0x89,

        /// <summary>
        /// 
        /// </summary>
        OP_0x8A = 0x8A,

        /// <summary>
        /// 
        /// </summary>
        OP_0x8B = 0x8B,

        /// <summary>
        /// 
        /// </summary>
        OP_0x8C = 0x8C,

        /// <summary>
        /// 
        /// </summary>
        OP_0x8D = 0x8D,

        /// <summary>
        /// 
        /// </summary>
        OP_0x8E = 0x8E,

        /// <summary>
        /// 
        /// </summary>
        OP_0x8F = 0x8F,

        /// <summary>
        /// Negate a value.
        /// </summary>
        Negate = 0x90,

        /// <summary>
        /// Increment a value.
        /// </summary>
        Increment = 0x91,

        /// <summary>
        /// Increment a local register value.
        /// </summary>
        Inclocal = 0x92,

        /// <summary>
        /// Decrement a value.
        /// </summary>
        Decrement = 0x93,

        /// <summary>
        /// Decrement a local register value.
        /// </summary>
        Declocal = 0x94,

        /// <summary>
        /// Get the type name of a value.
        /// </summary>
        Typeof = 0x95,

        /// <summary>
        /// Boolean negation.
        /// </summary>
        Not = 0x96,

        /// <summary>
        /// Bitwise not
        /// </summary>
        Bitnot = 0x97,

        /// <summary>
        /// 
        /// </summary>
        OP_0x98 = 0x98,

        /// <summary>
        /// 
        /// </summary>
        OP_0x99 = 0x99,

        /// <summary>
        /// 
        /// </summary>
        Concat = 0x9A,

        /// <summary>
        /// 
        /// </summary>
        Add_d = 0x9B,

        /// <summary>
        /// 
        /// </summary>
        OP_0x9C = 0x9C,

        /// <summary>
        /// 
        /// </summary>
        OP_0x9D = 0x9D,

        /// <summary>
        /// 
        /// </summary>
        OP_0x9E = 0x9E,

        /// <summary>
        /// 
        /// </summary>
        OP_0x9F = 0x9F,

        /// <summary>
        /// Add two values
        /// </summary>
        Add = 0xA0,

        /// <summary>
        /// Subtract two values.
        /// </summary>
        Subtract = 0xA1,

        /// <summary>
        /// Multiply two values.
        /// </summary>
        Multiply = 0xA2,

        /// <summary>
        /// Divide two values.
        /// </summary>
        Divide = 0xA3,

        /// <summary>
        /// Perform modulo division on two values.
        /// </summary>
        Modulo = 0xA4,

        /// <summary>
        /// Bitwise left shift.
        /// </summary>
        Lshift = 0xA5,

        /// <summary>
        /// Signed bitwise right shift.
        /// </summary>
        Rshift = 0xA6,

        /// <summary>
        /// Unsigned bitwise right shift.
        /// </summary>
        Urshift = 0xA7,

        /// <summary>
        /// Bitwise and
        /// </summary>
        Bitand = 0xA8,

        /// <summary>
        /// Bitwise or
        /// </summary>
        Bitor = 0xA9,

        /// <summary>
        /// Bitwise exclusive or
        /// </summary>
        Bitxor = 0xAA,

        /// <summary>
        /// Compare two values.
        /// </summary>
        Equals = 0xAB,

        /// <summary>
        /// Compare two values strictly.
        /// </summary>
        Strictequals = 0xAC,

        /// <summary>
        /// Determine if one value is less than another.
        /// </summary>
        Lessthan = 0xAD,

        /// <summary>
        /// Determine if one value is less than or equal to another.
        /// </summary>
        Lessequals = 0xAE,

        /// <summary>
        /// Determine if one value is greater than another.
        /// </summary>
        Greaterthan = 0xAF,

        /// <summary>
        /// Determine if one value is greater than or equal to another.
        /// </summary>
        Greaterequals = 0xB0,

        /// <summary>
        /// Check the prototype chain of an object for the existence of a type.
        /// </summary>
        Instanceof = 0xB1,

        /// <summary>
        /// Check whether an Object is of a certain type.
        /// </summary>
        Istype = 0xB2,

        /// <summary>
        /// Check whether an Object is of a certain type.
        /// </summary>
        Istypelate = 0xB3,

        /// <summary>
        /// Determine whether an object has a named property.
        /// </summary>
        In = 0xB4,

        /// <summary>
        /// 
        /// </summary>
        OP_0xB5 = 0xB5,

        /// <summary>
        /// 
        /// </summary>
        OP_0xB6 = 0xB6,

        /// <summary>
        /// 
        /// </summary>
        OP_0xB7 = 0xB7,

        /// <summary>
        /// 
        /// </summary>
        OP_0xB8 = 0xB8,

        /// <summary>
        /// 
        /// </summary>
        OP_0xB9 = 0xB9,

        /// <summary>
        /// 
        /// </summary>
        OP_0xBA = 0xBA,

        /// <summary>
        /// 
        /// </summary>
        OP_0xBB = 0xBB,

        /// <summary>
        /// 
        /// </summary>
        OP_0xBC = 0xBC,

        /// <summary>
        /// 
        /// </summary>
        OP_0xBD = 0xBD,

        /// <summary>
        /// 
        /// </summary>
        OP_0xBE = 0xBE,

        /// <summary>
        /// 
        /// </summary>
        OP_0xBF = 0xBF,

        /// <summary>
        /// Increment an integer value.
        /// </summary>
        Increment_i = 0xC0,

        /// <summary>
        /// Decrement an integer value.
        /// </summary>
        Decrement_i = 0xC1,

        /// <summary>
        /// Increment a local register value.
        /// </summary>
        Inclocal_i = 0xC2,

        /// <summary>
        /// Decrement a local register value.
        /// </summary>
        Declocal_i = 0xC3,

        /// <summary>
        /// Negate an integer value.
        /// </summary>
        Negate_i = 0xC4,

        /// <summary>
        /// Add two integer values
        /// </summary>
        Add_i = 0xC5,

        /// <summary>
        /// Subtract two integer values.
        /// </summary>
        Subtract_i = 0xC6,

        /// <summary>
        /// Multiply two integer values.
        /// </summary>
        Multiply_i = 0xC7,

        /// <summary>
        /// 
        /// </summary>
        OP_0xC8 = 0xC8,

        /// <summary>
        /// 
        /// </summary>
        OP_0xC9 = 0xC9,

        /// <summary>
        /// 
        /// </summary>
        OP_0xCA = 0xCA,

        /// <summary>
        /// 
        /// </summary>
        OP_0xCB = 0xCB,

        /// <summary>
        /// 
        /// </summary>
        OP_0xCC = 0xCC,

        /// <summary>
        /// 
        /// </summary>
        OP_0xCD = 0xCD,

        /// <summary>
        /// 
        /// </summary>
        OP_0xCE = 0xCE,

        /// <summary>
        /// 
        /// </summary>
        OP_0xCF = 0xCF,

        /// <summary>
        /// Get local register at 0 index.
        /// </summary>
        Getlocal0 = 0xD0,

        /// <summary>
        /// Get local register at 1 index.
        /// </summary>
        Getlocal1 = 0xD1,

        /// <summary>
        /// Get local register at 2 index.
        /// </summary>
        Getlocal2 = 0xD2,

        /// <summary>
        /// Get local register at 3 index.
        /// </summary>
        Getlocal3 = 0xD3,

        /// <summary>
        /// Set local register at 0 index.
        /// </summary>
        Setlocal0 = 0xD4,

        /// <summary>
        /// Set local register at 1 index.
        /// </summary>
        Setlocal1 = 0xD5,

        /// <summary>
        /// Set local register at 2 index.
        /// </summary>
        Setlocal2 = 0xD6,

        /// <summary>
        /// Set local register at 3 index.
        /// </summary>
        Setlocal3 = 0xD7,

        /// <summary>
        /// 
        /// </summary>
        OP_0xD8 = 0xD8,

        /// <summary>
        /// 
        /// </summary>
        OP_0xD9 = 0xD9,

        /// <summary>
        /// 
        /// </summary>
        OP_0xDA = 0xDA,

        /// <summary>
        /// 
        /// </summary>
        OP_0xDB = 0xDB,

        /// <summary>
        /// 
        /// </summary>
        OP_0xDC = 0xDC,

        /// <summary>
        /// 
        /// </summary>
        OP_0xDD = 0xDD,

        /// <summary>
        /// 
        /// </summary>
        OP_0xDE = 0xDE,

        /// <summary>
        /// 
        /// </summary>
        OP_0xDF = 0xDF,

        /// <summary>
        /// 
        /// </summary>
        OP_0xE0 = 0xE0,

        /// <summary>
        /// 
        /// </summary>
        OP_0xE1 = 0xE1,

        /// <summary>
        /// 
        /// </summary>
        OP_0xE2 = 0xE2,

        /// <summary>
        /// 
        /// </summary>
        OP_0xE3 = 0xE3,

        /// <summary>
        /// 
        /// </summary>
        OP_0xE4 = 0xE4,

        /// <summary>
        /// 
        /// </summary>
        OP_0xE5 = 0xE5,

        /// <summary>
        /// 
        /// </summary>
        OP_0xE6 = 0xE6,

        /// <summary>
        /// 
        /// </summary>
        OP_0xE7 = 0xE7,

        /// <summary>
        /// 
        /// </summary>
        OP_0xE8 = 0xE8,

        /// <summary>
        /// 
        /// </summary>
        OP_0xE9 = 0xE9,

        /// <summary>
        /// 
        /// </summary>
        OP_0xEA = 0xEA,

        /// <summary>
        /// 
        /// </summary>
        OP_0xEB = 0xEB,

        /// <summary>
        /// 
        /// </summary>
        OP_0xEC = 0xEC,

        /// <summary>
        /// 
        /// </summary>
        OP_0xED = 0xED,

        /// <summary>
        /// vm-use only, not allowed in ABC files.
        /// </summary>
        Abs_jump = 0xEE,

        /// <summary>
        /// Debugging info.
        /// </summary>
        Debug = 0xEF,

        /// <summary>
        /// Debugging line number info.
        /// </summary>
        Debugline = 0xF0,

        /// <summary>
        /// Debugging line number info.
        /// </summary>
        Debugfile = 0xF1,

        /// <summary>
        /// Debugger break point line
        /// </summary>
        Bkptline = 0xF2,

        /// <summary>
        /// ???
        /// </summary>
        Timestamp = 0xF3,

        /// <summary>
        /// 
        /// </summary>
        OP_0xF4 = 0xF4,

        /// <summary>
        /// 
        /// </summary>
        Verifypass = 0xF5,

        /// <summary>
        /// 
        /// </summary>
        Alloc = 0xF6,

        /// <summary>
        /// 
        /// </summary>
        Mark = 0xF7,

        /// <summary>
        /// 
        /// </summary>
        Wb = 0xF8,

        /// <summary>
        /// 
        /// </summary>
        Prologue = 0xF9,

        /// <summary>
        /// 
        /// </summary>
        Sendenter = 0xFA,

        /// <summary>
        /// 
        /// </summary>
        Doubletoatom = 0xFB,

        /// <summary>
        /// 
        /// </summary>
        Sweep = 0xFC,

        /// <summary>
        /// 
        /// </summary>
        Codegenop = 0xFD,

        /// <summary>
        /// 
        /// </summary>
        Verifyop = 0xFE,

        /// <summary>
        /// 
        /// </summary>
        Decode = 0xFF,
    }
}

