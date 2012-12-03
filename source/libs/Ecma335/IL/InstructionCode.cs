//
// WARNING: Automatically generated file. DO NOT EDIT
//

namespace DataDynamics.PageFX.Ecma335.IL
{
    /// <summary>
    /// Enumerates CIL instruction codes
    /// </summary>
    public enum InstructionCode
    {
        /// <summary>
        /// Fills space if opcodes are patched. No meaningful operation is performed although a processing cycle can be consumed.
        /// </summary>
        Nop = 0,

        /// <summary>
        /// Signals the Common Language Infrastructure (CLI) to inform the debugger that a break point has been tripped.
        /// </summary>
        Break = 1,

        /// <summary>
        /// Loads the argument at index 0 onto the evaluation stack.
        /// </summary>
        Ldarg_0 = 2,

        /// <summary>
        /// Loads the argument at index 1 onto the evaluation stack.
        /// </summary>
        Ldarg_1 = 3,

        /// <summary>
        /// Loads the argument at index 2 onto the evaluation stack.
        /// </summary>
        Ldarg_2 = 4,

        /// <summary>
        /// Loads the argument at index 3 onto the evaluation stack.
        /// </summary>
        Ldarg_3 = 5,

        /// <summary>
        /// Loads the local variable at index 0 onto the evaluation stack.
        /// </summary>
        Ldloc_0 = 6,

        /// <summary>
        /// Loads the local variable at index 1 onto the evaluation stack.
        /// </summary>
        Ldloc_1 = 7,

        /// <summary>
        /// Loads the local variable at index 2 onto the evaluation stack.
        /// </summary>
        Ldloc_2 = 8,

        /// <summary>
        /// Loads the local variable at index 3 onto the evaluation stack.
        /// </summary>
        Ldloc_3 = 9,

        /// <summary>
        /// Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index 0.
        /// </summary>
        Stloc_0 = 10,

        /// <summary>
        /// Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index 1.
        /// </summary>
        Stloc_1 = 11,

        /// <summary>
        /// Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index 2.
        /// </summary>
        Stloc_2 = 12,

        /// <summary>
        /// Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index 3.
        /// </summary>
        Stloc_3 = 13,

        /// <summary>
        /// Loads the argument (referenced by a specified short form index) onto the evaluation stack.
        /// </summary>
        Ldarg_S = 14,

        /// <summary>
        /// Load an argument address, in short form, onto the evaluation stack.
        /// </summary>
        Ldarga_S = 15,

        /// <summary>
        /// Stores the value on top of the evaluation stack in the argument slot at a specified index, short form.
        /// </summary>
        Starg_S = 16,

        /// <summary>
        /// Loads the local variable at a specific index onto the evaluation stack, short form.
        /// </summary>
        Ldloc_S = 17,

        /// <summary>
        /// Loads the address of the local variable at a specific index onto the evaluation stack, short form.
        /// </summary>
        Ldloca_S = 18,

        /// <summary>
        /// Pops the current value from the top of the evaluation stack and stores it in a the local variable list at index (short form).
        /// </summary>
        Stloc_S = 19,

        /// <summary>
        /// Pushes a null reference (type O) onto the evaluation stack.
        /// </summary>
        Ldnull = 20,

        /// <summary>
        /// Pushes the integer value of -1 onto the evaluation stack as an int32.
        /// </summary>
        Ldc_I4_M1 = 21,

        /// <summary>
        /// Pushes the integer value of 0 onto the evaluation stack as an int32.
        /// </summary>
        Ldc_I4_0 = 22,

        /// <summary>
        /// Pushes the integer value of 1 onto the evaluation stack as an int32.
        /// </summary>
        Ldc_I4_1 = 23,

        /// <summary>
        /// Pushes the integer value of 2 onto the evaluation stack as an int32.
        /// </summary>
        Ldc_I4_2 = 24,

        /// <summary>
        /// Pushes the integer value of 3 onto the evaluation stack as an int32.
        /// </summary>
        Ldc_I4_3 = 25,

        /// <summary>
        /// Pushes the integer value of 4 onto the evaluation stack as an int32.
        /// </summary>
        Ldc_I4_4 = 26,

        /// <summary>
        /// Pushes the integer value of 5 onto the evaluation stack as an int32.
        /// </summary>
        Ldc_I4_5 = 27,

        /// <summary>
        /// Pushes the integer value of 6 onto the evaluation stack as an int32.
        /// </summary>
        Ldc_I4_6 = 28,

        /// <summary>
        /// Pushes the integer value of 7 onto the evaluation stack as an int32.
        /// </summary>
        Ldc_I4_7 = 29,

        /// <summary>
        /// Pushes the integer value of 8 onto the evaluation stack as an int32.
        /// </summary>
        Ldc_I4_8 = 30,

        /// <summary>
        /// Pushes the supplied int8 value onto the evaluation stack as an int32, short form.
        /// </summary>
        Ldc_I4_S = 31,

        /// <summary>
        /// Pushes a supplied value of type int32 onto the evaluation stack as an int32.
        /// </summary>
        Ldc_I4 = 32,

        /// <summary>
        /// Pushes a supplied value of type int64 onto the evaluation stack as an int64.
        /// </summary>
        Ldc_I8 = 33,

        /// <summary>
        /// Pushes a supplied value of type float32 onto the evaluation stack as type F (float).
        /// </summary>
        Ldc_R4 = 34,

        /// <summary>
        /// Pushes a supplied value of type float64 onto the evaluation stack as type F (float).
        /// </summary>
        Ldc_R8 = 35,

        /// <summary>
        /// Copies the current topmost value on the evaluation stack, and then pushes the copy onto the evaluation stack.
        /// </summary>
        Dup = 37,

        /// <summary>
        /// Removes the value currently on top of the evaluation stack.
        /// </summary>
        Pop = 38,

        /// <summary>
        /// Exits current method and jumps to specified method.
        /// </summary>
        Jmp = 39,

        /// <summary>
        /// Calls the method indicated by the passed method descriptor.
        /// </summary>
        Call = 40,

        /// <summary>
        /// Calls the method indicated on the evaluation stack (as a pointer to an entry point) with arguments described by a calling convention.
        /// </summary>
        Calli = 41,

        /// <summary>
        /// Returns from the current method, pushing a return value (if present) from the callee&apos;s evaluation stack onto the caller&apos;s evaluation stack.
        /// </summary>
        Ret = 42,

        /// <summary>
        /// Unconditionally transfers control to a target instruction (short form).
        /// </summary>
        Br_S = 43,

        /// <summary>
        /// Transfers control to a target instruction if value is false, a null reference, or zero.
        /// </summary>
        Brfalse_S = 44,

        /// <summary>
        /// Transfers control to a target instruction (short form) if value is true, not null, or non-zero.
        /// </summary>
        Brtrue_S = 45,

        /// <summary>
        /// Transfers control to a target instruction (short form) if two values are equal.
        /// </summary>
        Beq_S = 46,

        /// <summary>
        /// Transfers control to a target instruction (short form) if the first value is greater than or equal to the second value.
        /// </summary>
        Bge_S = 47,

        /// <summary>
        /// Transfers control to a target instruction (short form) if the first value is greater than the second value.
        /// </summary>
        Bgt_S = 48,

        /// <summary>
        /// Transfers control to a target instruction (short form) if the first value is less than or equal to the second value.
        /// </summary>
        Ble_S = 49,

        /// <summary>
        /// Transfers control to a target instruction (short form) if the first value is less than the second value.
        /// </summary>
        Blt_S = 50,

        /// <summary>
        /// Transfers control to a target instruction (short form) when two unsigned integer values or unordered float values are not equal.
        /// </summary>
        Bne_Un_S = 51,

        /// <summary>
        /// Transfers control to a target instruction (short form) if the first value is greater than the second value, when comparing unsigned integer values or unordered float values.
        /// </summary>
        Bge_Un_S = 52,

        /// <summary>
        /// Transfers control to a target instruction (short form) if the first value is greater than the second value, when comparing unsigned integer values or unordered float values.
        /// </summary>
        Bgt_Un_S = 53,

        /// <summary>
        /// Transfers control to a target instruction (short form) if the first value is less than or equal to the second value, when comparing unsigned integer values or unordered float values.
        /// </summary>
        Ble_Un_S = 54,

        /// <summary>
        /// Transfers control to a target instruction (short form) if the first value is less than the second value, when comparing unsigned integer values or unordered float values.
        /// </summary>
        Blt_Un_S = 55,

        /// <summary>
        /// Unconditionally transfers control to a target instruction.
        /// </summary>
        Br = 56,

        /// <summary>
        /// Transfers control to a target instruction if value is false, a null reference (Nothing in Visual Basic), or zero.
        /// </summary>
        Brfalse = 57,

        /// <summary>
        /// Transfers control to a target instruction if value is true, not null, or non-zero.
        /// </summary>
        Brtrue = 58,

        /// <summary>
        /// Transfers control to a target instruction if two values are equal.
        /// </summary>
        Beq = 59,

        /// <summary>
        /// Transfers control to a target instruction if the first value is greater than or equal to the second value.
        /// </summary>
        Bge = 60,

        /// <summary>
        /// Transfers control to a target instruction if the first value is greater than the second value.
        /// </summary>
        Bgt = 61,

        /// <summary>
        /// Transfers control to a target instruction if the first value is less than or equal to the second value.
        /// </summary>
        Ble = 62,

        /// <summary>
        /// Transfers control to a target instruction if the first value is less than the second value.
        /// </summary>
        Blt = 63,

        /// <summary>
        /// Transfers control to a target instruction when two unsigned integer values or unordered float values are not equal.
        /// </summary>
        Bne_Un = 64,

        /// <summary>
        /// Transfers control to a target instruction if the first value is greater than the second value, when comparing unsigned integer values or unordered float values.
        /// </summary>
        Bge_Un = 65,

        /// <summary>
        /// Transfers control to a target instruction if the first value is greater than the second value, when comparing unsigned integer values or unordered float values.
        /// </summary>
        Bgt_Un = 66,

        /// <summary>
        /// Transfers control to a target instruction if the first value is less than or equal to the second value, when comparing unsigned integer values or unordered float values.
        /// </summary>
        Ble_Un = 67,

        /// <summary>
        /// Transfers control to a target instruction if the first value is less than the second value, when comparing unsigned integer values or unordered float values.
        /// </summary>
        Blt_Un = 68,

        /// <summary>
        /// Implements a jump table.
        /// </summary>
        Switch = 69,

        /// <summary>
        /// Loads a value of type int8 as an int32 onto the evaluation stack indirectly.
        /// </summary>
        Ldind_I1 = 70,

        /// <summary>
        /// Loads a value of type unsigned int8 as an int32 onto the evaluation stack indirectly.
        /// </summary>
        Ldind_U1 = 71,

        /// <summary>
        /// Loads a value of type int16 as an int32 onto the evaluation stack indirectly.
        /// </summary>
        Ldind_I2 = 72,

        /// <summary>
        /// Loads a value of type unsigned int16 as an int32 onto the evaluation stack indirectly.
        /// </summary>
        Ldind_U2 = 73,

        /// <summary>
        /// Loads a value of type int32 as an int32 onto the evaluation stack indirectly.
        /// </summary>
        Ldind_I4 = 74,

        /// <summary>
        /// Loads a value of type unsigned int32 as an int32 onto the evaluation stack indirectly.
        /// </summary>
        Ldind_U4 = 75,

        /// <summary>
        /// Loads a value of type int64 as an int64 onto the evaluation stack indirectly.
        /// </summary>
        Ldind_I8 = 76,

        /// <summary>
        /// Loads a value of type natural int as a natural int onto the evaluation stack indirectly.
        /// </summary>
        Ldind_I = 77,

        /// <summary>
        /// Loads a value of type float32 as a type F (float) onto the evaluation stack indirectly.
        /// </summary>
        Ldind_R4 = 78,

        /// <summary>
        /// Loads a value of type float64 as a type F (float) onto the evaluation stack indirectly.
        /// </summary>
        Ldind_R8 = 79,

        /// <summary>
        /// Loads an object reference as a type O (object reference) onto the evaluation stack indirectly.
        /// </summary>
        Ldind_Ref = 80,

        /// <summary>
        /// Stores a object reference value at a supplied address.
        /// </summary>
        Stind_Ref = 81,

        /// <summary>
        /// Stores a value of type int8 at a supplied address.
        /// </summary>
        Stind_I1 = 82,

        /// <summary>
        /// Stores a value of type int16 at a supplied address.
        /// </summary>
        Stind_I2 = 83,

        /// <summary>
        /// Stores a value of type int32 at a supplied address.
        /// </summary>
        Stind_I4 = 84,

        /// <summary>
        /// Stores a value of type int64 at a supplied address.
        /// </summary>
        Stind_I8 = 85,

        /// <summary>
        /// Stores a value of type float32 at a supplied address.
        /// </summary>
        Stind_R4 = 86,

        /// <summary>
        /// Stores a value of type float64 at a supplied address.
        /// </summary>
        Stind_R8 = 87,

        /// <summary>
        /// Adds two values and pushes the result onto the evaluation stack.
        /// </summary>
        Add = 88,

        /// <summary>
        /// Subtracts one value from another and pushes the result onto the evaluation stack.
        /// </summary>
        Sub = 89,

        /// <summary>
        /// Multiplies two values and pushes the result on the evaluation stack.
        /// </summary>
        Mul = 90,

        /// <summary>
        /// Divides two values and pushes the result as a floating-point (type F) or quotient (type int32) onto the evaluation stack.
        /// </summary>
        Div = 91,

        /// <summary>
        /// Divides two unsigned integer values and pushes the result (int32) onto the evaluation stack.
        /// </summary>
        Div_Un = 92,

        /// <summary>
        /// Divides two values and pushes the remainder onto the evaluation stack.
        /// </summary>
        Rem = 93,

        /// <summary>
        /// Divides two unsigned values and pushes the remainder onto the evaluation stack.
        /// </summary>
        Rem_Un = 94,

        /// <summary>
        /// Computes the bitwise AND of two values and pushes the result onto the evaluation stack.
        /// </summary>
        And = 95,

        /// <summary>
        /// Compute the bitwise complement of the two integer values on top of the stack and pushes the result onto the evaluation stack.
        /// </summary>
        Or = 96,

        /// <summary>
        /// Computes the bitwise XOR of the top two values on the evaluation stack, pushing the result onto the evaluation stack.
        /// </summary>
        Xor = 97,

        /// <summary>
        /// Shifts an integer value to the left (in zeroes) by a specified number of bits, pushing the result onto the evaluation stack.
        /// </summary>
        Shl = 98,

        /// <summary>
        /// Shifts an integer value (in sign) to the right by a specified number of bits, pushing the result onto the evaluation stack.
        /// </summary>
        Shr = 99,

        /// <summary>
        /// Shifts an unsigned integer value (in zeroes) to the right by a specified number of bits, pushing the result onto the evaluation stack.
        /// </summary>
        Shr_Un = 100,

        /// <summary>
        /// Negates a value and pushes the result onto the evaluation stack.
        /// </summary>
        Neg = 101,

        /// <summary>
        /// Computes the bitwise complement of the integer value on top of the stack and pushes the result onto the evaluation stack as the same type.
        /// </summary>
        Not = 102,

        /// <summary>
        /// Converts the value on top of the evaluation stack to int8, then extends (pads) it to int32.
        /// </summary>
        Conv_I1 = 103,

        /// <summary>
        /// Converts the value on top of the evaluation stack to int16, then extends (pads) it to int32.
        /// </summary>
        Conv_I2 = 104,

        /// <summary>
        /// Converts the value on top of the evaluation stack to int32.
        /// </summary>
        Conv_I4 = 105,

        /// <summary>
        /// Converts the value on top of the evaluation stack to int64.
        /// </summary>
        Conv_I8 = 106,

        /// <summary>
        /// Converts the value on top of the evaluation stack to float32.
        /// </summary>
        Conv_R4 = 107,

        /// <summary>
        /// Converts the value on top of the evaluation stack to float64.
        /// </summary>
        Conv_R8 = 108,

        /// <summary>
        /// Converts the value on top of the evaluation stack to unsigned int32, and extends it to int32.
        /// </summary>
        Conv_U4 = 109,

        /// <summary>
        /// Converts the value on top of the evaluation stack to unsigned int64, and extends it to int64.
        /// </summary>
        Conv_U8 = 110,

        /// <summary>
        /// Calls a late-bound method on an object, pushing the return value onto the evaluation stack.
        /// </summary>
        Callvirt = 111,

        /// <summary>
        /// Copies the value type located at the address of an object (type &amp;, * or natural int) to the address of the destination object (type &amp;, * or natural int).
        /// </summary>
        Cpobj = 112,

        /// <summary>
        /// Copies the value type object pointed to by an address to the top of the evaluation stack.
        /// </summary>
        Ldobj = 113,

        /// <summary>
        /// Pushes a new object reference to a string literal stored in the metadata.
        /// </summary>
        Ldstr = 114,

        /// <summary>
        /// Creates a new object or a new instance of a value type, pushing an object reference (type O) onto the evaluation stack.
        /// </summary>
        Newobj = 115,

        /// <summary>
        /// Attempts to cast an object passed by reference to the specified class.
        /// </summary>
        Castclass = 116,

        /// <summary>
        /// Tests whether an object reference (type O) is an instance of a particular class.
        /// </summary>
        Isinst = 117,

        /// <summary>
        /// Converts the unsigned integer value on top of the evaluation stack to float32.
        /// </summary>
        Conv_R_Un = 118,

        /// <summary>
        /// Converts the boxed representation of a value type to its unboxed form.
        /// </summary>
        Unbox = 121,

        /// <summary>
        /// Throws the exception object currently on the evaluation stack.
        /// </summary>
        Throw = 122,

        /// <summary>
        /// Finds the value of a field in the object whose reference is currently on the evaluation stack.
        /// </summary>
        Ldfld = 123,

        /// <summary>
        /// Finds the address of a field in the object whose reference is currently on the evaluation stack.
        /// </summary>
        Ldflda = 124,

        /// <summary>
        /// Replaces the value stored in the field of an object reference or pointer with a new value.
        /// </summary>
        Stfld = 125,

        /// <summary>
        /// Pushes the value of a static field onto the evaluation stack.
        /// </summary>
        Ldsfld = 126,

        /// <summary>
        /// Pushes the address of a static field onto the evaluation stack.
        /// </summary>
        Ldsflda = 127,

        /// <summary>
        /// Replaces the value of a static field with a value from the evaluation stack.
        /// </summary>
        Stsfld = 128,

        /// <summary>
        /// Copies a value of a specified type from the evaluation stack into a supplied memory address.
        /// </summary>
        Stobj = 129,

        /// <summary>
        /// Converts the unsigned value on top of the evaluation stack to signed int8 and extends it to int32, throwing  on overflow.
        /// </summary>
        Conv_Ovf_I1_Un = 130,

        /// <summary>
        /// Converts the unsigned value on top of the evaluation stack to signed int16 and extends it to int32, throwing  on overflow.
        /// </summary>
        Conv_Ovf_I2_Un = 131,

        /// <summary>
        /// Converts the unsigned value on top of the evaluation stack to signed int32, throwing  on overflow.
        /// </summary>
        Conv_Ovf_I4_Un = 132,

        /// <summary>
        /// Converts the unsigned value on top of the evaluation stack to signed int64, throwing  on overflow.
        /// </summary>
        Conv_Ovf_I8_Un = 133,

        /// <summary>
        /// Converts the unsigned value on top of the evaluation stack to unsigned int8 and extends it to int32, throwing  on overflow.
        /// </summary>
        Conv_Ovf_U1_Un = 134,

        /// <summary>
        /// Converts the unsigned value on top of the evaluation stack to unsigned int16 and extends it to int32, throwing  on overflow.
        /// </summary>
        Conv_Ovf_U2_Un = 135,

        /// <summary>
        /// Converts the unsigned value on top of the evaluation stack to unsigned int32, throwing  on overflow.
        /// </summary>
        Conv_Ovf_U4_Un = 136,

        /// <summary>
        /// Converts the unsigned value on top of the evaluation stack to unsigned int64, throwing  on overflow.
        /// </summary>
        Conv_Ovf_U8_Un = 137,

        /// <summary>
        /// Converts the unsigned value on top of the evaluation stack to signed natural int, throwing  on overflow.
        /// </summary>
        Conv_Ovf_I_Un = 138,

        /// <summary>
        /// Converts the unsigned value on top of the evaluation stack to unsigned natural int, throwing  on overflow.
        /// </summary>
        Conv_Ovf_U_Un = 139,

        /// <summary>
        /// Converts a value type to an object reference (type O).
        /// </summary>
        Box = 140,

        /// <summary>
        /// Pushes an object reference to a new zero-based, one-dimensional array whose elements are of a specific type onto the evaluation stack.
        /// </summary>
        Newarr = 141,

        /// <summary>
        /// Pushes the number of elements of a zero-based, one-dimensional array onto the evaluation stack.
        /// </summary>
        Ldlen = 142,

        /// <summary>
        /// Loads the address of the array element at a specified array index onto the top of the evaluation stack as type &amp; (managed pointer).
        /// </summary>
        Ldelema = 143,

        /// <summary>
        /// Loads the element with type int8 at a specified array index onto the top of the evaluation stack as an int32.
        /// </summary>
        Ldelem_I1 = 144,

        /// <summary>
        /// Loads the element with type unsigned int8 at a specified array index onto the top of the evaluation stack as an int32.
        /// </summary>
        Ldelem_U1 = 145,

        /// <summary>
        /// Loads the element with type int16 at a specified array index onto the top of the evaluation stack as an int32.
        /// </summary>
        Ldelem_I2 = 146,

        /// <summary>
        /// Loads the element with type unsigned int16 at a specified array index onto the top of the evaluation stack as an int32.
        /// </summary>
        Ldelem_U2 = 147,

        /// <summary>
        /// Loads the element with type int32 at a specified array index onto the top of the evaluation stack as an int32.
        /// </summary>
        Ldelem_I4 = 148,

        /// <summary>
        /// Loads the element with type unsigned int32 at a specified array index onto the top of the evaluation stack as an int32.
        /// </summary>
        Ldelem_U4 = 149,

        /// <summary>
        /// Loads the element with type int64 at a specified array index onto the top of the evaluation stack as an int64.
        /// </summary>
        Ldelem_I8 = 150,

        /// <summary>
        /// Loads the element with type natural int at a specified array index onto the top of the evaluation stack as a natural int.
        /// </summary>
        Ldelem_I = 151,

        /// <summary>
        /// Loads the element with type float32 at a specified array index onto the top of the evaluation stack as type F (float).
        /// </summary>
        Ldelem_R4 = 152,

        /// <summary>
        /// Loads the element with type float64 at a specified array index onto the top of the evaluation stack as type F (float).
        /// </summary>
        Ldelem_R8 = 153,

        /// <summary>
        /// Loads the element containing an object reference at a specified array index onto the top of the evaluation stack as type O (object reference).
        /// </summary>
        Ldelem_Ref = 154,

        /// <summary>
        /// Replaces the array element at a given index with the natural int value on the evaluation stack.
        /// </summary>
        Stelem_I = 155,

        /// <summary>
        /// Replaces the array element at a given index with the int8 value on the evaluation stack.
        /// </summary>
        Stelem_I1 = 156,

        /// <summary>
        /// Replaces the array element at a given index with the int16 value on the evaluation stack.
        /// </summary>
        Stelem_I2 = 157,

        /// <summary>
        /// Replaces the array element at a given index with the int32 value on the evaluation stack.
        /// </summary>
        Stelem_I4 = 158,

        /// <summary>
        /// Replaces the array element at a given index with the int64 value on the evaluation stack.
        /// </summary>
        Stelem_I8 = 159,

        /// <summary>
        /// Replaces the array element at a given index with the float32 value on the evaluation stack.
        /// </summary>
        Stelem_R4 = 160,

        /// <summary>
        /// Replaces the array element at a given index with the float64 value on the evaluation stack.
        /// </summary>
        Stelem_R8 = 161,

        /// <summary>
        /// Replaces the array element at a given index with the object ref value (type O) on the evaluation stack.
        /// </summary>
        Stelem_Ref = 162,

        /// <summary>
        /// Loads the element at a specified array index onto the top of the evaluation stack as the type specified in the instruction. 
        /// </summary>
        Ldelem = 163,

        /// <summary>
        /// Replaces the array element at a given index with the value on the evaluation stack, whose type is specified in the instruction.
        /// </summary>
        Stelem = 164,

        /// <summary>
        /// Converts the boxed representation of a type specified in the instruction to its unboxed form. 
        /// </summary>
        Unbox_Any = 165,

        /// <summary>
        /// Converts the signed value on top of the evaluation stack to signed int8 and extends it to int32, throwing  on overflow.
        /// </summary>
        Conv_Ovf_I1 = 179,

        /// <summary>
        /// Converts the signed value on top of the evaluation stack to unsigned int8 and extends it to int32, throwing  on overflow.
        /// </summary>
        Conv_Ovf_U1 = 180,

        /// <summary>
        /// Converts the signed value on top of the evaluation stack to signed int16 and extending it to int32, throwing  on overflow.
        /// </summary>
        Conv_Ovf_I2 = 181,

        /// <summary>
        /// Converts the signed value on top of the evaluation stack to unsigned int16 and extends it to int32, throwing  on overflow.
        /// </summary>
        Conv_Ovf_U2 = 182,

        /// <summary>
        /// Converts the signed value on top of the evaluation stack to signed int32, throwing  on overflow.
        /// </summary>
        Conv_Ovf_I4 = 183,

        /// <summary>
        /// Converts the signed value on top of the evaluation stack to unsigned int32, throwing  on overflow.
        /// </summary>
        Conv_Ovf_U4 = 184,

        /// <summary>
        /// Converts the signed value on top of the evaluation stack to signed int64, throwing  on overflow.
        /// </summary>
        Conv_Ovf_I8 = 185,

        /// <summary>
        /// Converts the signed value on top of the evaluation stack to unsigned int64, throwing  on overflow.
        /// </summary>
        Conv_Ovf_U8 = 186,

        /// <summary>
        /// Retrieves the address (type &amp;) embedded in a typed reference.
        /// </summary>
        Refanyval = 194,

        /// <summary>
        /// Throws  if value is not a finite number.
        /// </summary>
        Ckfinite = 195,

        /// <summary>
        /// Pushes a typed reference to an instance of a specific type onto the evaluation stack.
        /// </summary>
        Mkrefany = 198,

        /// <summary>
        /// Converts a metadata token to its runtime representation, pushing it onto the evaluation stack.
        /// </summary>
        Ldtoken = 208,

        /// <summary>
        /// Converts the value on top of the evaluation stack to unsigned int16, and extends it to int32.
        /// </summary>
        Conv_U2 = 209,

        /// <summary>
        /// Converts the value on top of the evaluation stack to unsigned int8, and extends it to int32.
        /// </summary>
        Conv_U1 = 210,

        /// <summary>
        /// Converts the value on top of the evaluation stack to natural int.
        /// </summary>
        Conv_I = 211,

        /// <summary>
        /// Converts the signed value on top of the evaluation stack to signed natural int, throwing  on overflow.
        /// </summary>
        Conv_Ovf_I = 212,

        /// <summary>
        /// Converts the signed value on top of the evaluation stack to unsigned natural int, throwing  on overflow.
        /// </summary>
        Conv_Ovf_U = 213,

        /// <summary>
        /// Adds two integers, performs an overflow check, and pushes the result onto the evaluation stack.
        /// </summary>
        Add_Ovf = 214,

        /// <summary>
        /// Adds two unsigned integer values, performs an overflow check, and pushes the result onto the evaluation stack.
        /// </summary>
        Add_Ovf_Un = 215,

        /// <summary>
        /// Multiplies two integer values, performs an overflow check, and pushes the result onto the evaluation stack.
        /// </summary>
        Mul_Ovf = 216,

        /// <summary>
        /// Multiplies two unsigned integer values, performs an overflow check, and pushes the result onto the evaluation stack.
        /// </summary>
        Mul_Ovf_Un = 217,

        /// <summary>
        /// Subtracts one integer value from another, performs an overflow check, and pushes the result onto the evaluation stack.
        /// </summary>
        Sub_Ovf = 218,

        /// <summary>
        /// Subtracts one unsigned integer value from another, performs an overflow check, and pushes the result onto the evaluation stack.
        /// </summary>
        Sub_Ovf_Un = 219,

        /// <summary>
        /// Transfers control from the fault or finally clause of an exception block back to the Common Language Infrastructure (CLI) exception handler.
        /// </summary>
        Endfinally = 220,

        /// <summary>
        /// Exits a protected region of code, unconditionally transferring control to a specific target instruction.
        /// </summary>
        Leave = 221,

        /// <summary>
        /// Exits a protected region of code, unconditionally transferring control to a target instruction (short form).
        /// </summary>
        Leave_S = 222,

        /// <summary>
        /// Stores a value of type natural int at a supplied address.
        /// </summary>
        Stind_I = 223,

        /// <summary>
        /// Converts the value on top of the evaluation stack to unsigned natural int, and extends it to natural int.
        /// </summary>
        Conv_U = 224,

        /// <summary>
        /// This is a reserved instruction.
        /// </summary>
        Prefix7 = 248,

        /// <summary>
        /// This is a reserved instruction.
        /// </summary>
        Prefix6 = 249,

        /// <summary>
        /// This is a reserved instruction.
        /// </summary>
        Prefix5 = 250,

        /// <summary>
        /// This is a reserved instruction.
        /// </summary>
        Prefix4 = 251,

        /// <summary>
        /// This is a reserved instruction.
        /// </summary>
        Prefix3 = 252,

        /// <summary>
        /// This is a reserved instruction.
        /// </summary>
        Prefix2 = 253,

        /// <summary>
        /// This is a reserved instruction.
        /// </summary>
        Prefix1 = 254,

        /// <summary>
        /// This is a reserved instruction.
        /// </summary>
        Prefixref = 255,

        /// <summary>
        /// Returns an unmanaged pointer to the argument list of the current method.
        /// </summary>
        Arglist = -512,

        /// <summary>
        /// Compares two values. If they are equal, the integer value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.
        /// </summary>
        Ceq = -511,

        /// <summary>
        /// Compares two values. If the first value is greater than the second, the integer value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.
        /// </summary>
        Cgt = -510,

        /// <summary>
        /// Compares two unsigned or unordered values. If the first value is greater than the second, the integer value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.
        /// </summary>
        Cgt_Un = -509,

        /// <summary>
        /// Compares two values. If the first value is less than the second, the integer value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.
        /// </summary>
        Clt = -508,

        /// <summary>
        /// Compares the unsigned or unordered values value1 and value2. If value1 is less than value2, then the integer value 1 (int32) is pushed onto the evaluation stack; otherwise 0 (int32) is pushed onto the evaluation stack.
        /// </summary>
        Clt_Un = -507,

        /// <summary>
        /// Pushes an unmanaged pointer (type natural int) to the native code implementing a specific method onto the evaluation stack.
        /// </summary>
        Ldftn = -506,

        /// <summary>
        /// Pushes an unmanaged pointer (type natural int) to the native code implementing a particular virtual method associated with a specified object onto the evaluation stack.
        /// </summary>
        Ldvirtftn = -505,

        /// <summary>
        /// Loads an argument (referenced by a specified index value) onto the stack.
        /// </summary>
        Ldarg = -503,

        /// <summary>
        /// Load an argument address onto the evaluation stack.
        /// </summary>
        Ldarga = -502,

        /// <summary>
        /// Stores the value on top of the evaluation stack in the argument slot at a specified index.
        /// </summary>
        Starg = -501,

        /// <summary>
        /// Loads the local variable at a specific index onto the evaluation stack.
        /// </summary>
        Ldloc = -500,

        /// <summary>
        /// Loads the address of the local variable at a specific index onto the evaluation stack.
        /// </summary>
        Ldloca = -499,

        /// <summary>
        /// Pops the current value from the top of the evaluation stack and stores it in a the local variable list at a specified index.
        /// </summary>
        Stloc = -498,

        /// <summary>
        /// Allocates a certain number of bytes from the local dynamic memory pool and pushes the address (a transient pointer, type *) of the first allocated byte onto the evaluation stack.
        /// </summary>
        Localloc = -497,

        /// <summary>
        /// Transfers control from the filter clause of an exception back to the Common Language Infrastructure (CLI) exception handler.
        /// </summary>
        Endfilter = -495,

        /// <summary>
        /// Indicates that an address currently atop the evaluation stack might not be aligned to the natural size of the immediately following ldind, stind, ldfld, stfld, ldobj, stobj, initblk, or cpblk instruction.
        /// </summary>
        Unaligned = -494,

        /// <summary>
        /// Specifies that an address currently atop the evaluation stack might be volatile, and the results of reading that location cannot be cached or that multiple stores to that location cannot be suppressed.
        /// </summary>
        Volatile = -493,

        /// <summary>
        /// Performs a postfixed method call instruction such that the current method&apos;s stack frame is removed before the actual call instruction is executed.
        /// </summary>
        Tailcall = -492,

        /// <summary>
        /// Initializes all the fields of the object at a specific address to a null reference or a 0 of the appropriate primitive type.
        /// </summary>
        Initobj = -491,

        /// <summary>
        /// Constrains the type on which a virtual method call is made.
        /// </summary>
        Constrained = -490,

        /// <summary>
        /// Copies a specified number bytes from a source address to a destination address.
        /// </summary>
        Cpblk = -489,

        /// <summary>
        /// Initializes a specified block of memory at a specific address to a given size and initial value.
        /// </summary>
        Initblk = -488,

        /// <summary>
        /// Rethrows the current exception.
        /// </summary>
        Rethrow = -486,

        /// <summary>
        /// Pushes the size, in bytes, of a supplied value type onto the evaluation stack.
        /// </summary>
        Sizeof = -484,

        /// <summary>
        /// Retrieves the type token embedded in a typed reference.
        /// </summary>
        Refanytype = -483,

        /// <summary>
        /// Specifies that the subsequent array address operation performs no type check at run time, and that it returns a managed pointer whose mutability is restricted.
        /// </summary>
        Readonly = -482,
    }
}

