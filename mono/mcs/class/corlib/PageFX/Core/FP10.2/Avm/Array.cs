using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// The Array class lets you access and manipulate arrays. Array indices are zero-based,
    /// which means that the first element in the array is [0] , the second
    /// element is [1] , and so on. To create an Array object, you use the
    /// new Array()  constructor . Array()  can also be invoked as
    /// a function. In addition, you can use the array access ( [] ) operator
    /// to initialize an array or access the elements of an array.
    /// </summary>
    [PageFX.AbcInstance(9)]
    [PageFX.ABC]
    [PageFX.QName("Array")]
    [PageFX.FP9]
    public class Array : Avm.Object
    {
        /// <summary>
        /// Specifies case-insensitive sorting for the Array class sorting methods. You can
        /// use this constant for the options parameter in the sort()
        /// or sortOn() method.
        /// The value of this constant is 1.
        /// </summary>
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint CASEINSENSITIVE;

        /// <summary>
        /// Specifies descending sorting for the Array class sorting methods. You can use this
        /// constant for the options parameter in the sort() or
        /// sortOn() method.
        /// The value of this constant is 2.
        /// </summary>
        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint DESCENDING;

        /// <summary>
        /// Specifies the unique sorting requirement for the Array class sorting methods. You
        /// can use this constant for the options parameter in the sort()
        /// or sortOn() method. The unique sorting option terminates the sort if
        /// any two elements or fields being sorted have identical values.
        /// The value of this constant is 4.
        /// </summary>
        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint UNIQUESORT;

        /// <summary>
        /// Specifies that a sort returns an array that consists of array indices. You can use
        /// this constant for the options parameter in the sort()
        /// or sortOn() method, so you have access to multiple views of the array
        /// elements while the original array is unmodified.
        /// The value of this constant is 8.
        /// </summary>
        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint RETURNINDEXEDARRAY;

        /// <summary>
        /// Specifies numeric (instead of character-string) sorting for the Array class sorting
        /// methods. Including this constant in the options parameter causes the
        /// sort() and sortOn() methods to sort numbers as numeric
        /// values, not as strings of numeric characters. Without the NUMERIC constant,
        /// sorting treats each array element as a character string and produces the results
        /// in Unicode order.
        /// For example, given the array of values [2005, 7, 35], if the NUMERIC
        /// constant is not included in the options parameter, the sorted
        /// array is [2005, 35, 7], but if the NUMERIC constant is
        /// included, the sorted array is [7, 35, 2005].
        /// This constant applies only to numbers in the array; it does not apply to strings
        /// that contain numeric data such as [&quot;23&quot;, &quot;5&quot;].
        /// The value of this constant is 16.
        /// </summary>
        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP9]
        public static uint NUMERIC;

        /// <summary>
        /// A non-negative integer specifying the number of elements in the array. This property
        /// is automatically updated when new elements are added to the array. When you assign
        /// a value to an array element (for example, my_array[index] = value),
        /// if index is a number, and index+1 is greater than the
        /// length property, the length property is updated to index+1.
        /// Note: If you assign a value to the length property that is shorter
        /// than the existing length, the array will be truncated.
        /// </summary>
        public extern virtual uint length
        {
            [PageFX.AbcInstanceTrait(0)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP9]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Array();

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Array(object rest0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Array(object rest0, object rest1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Array(object rest0, object rest1, object rest2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Array(object rest0, object rest1, object rest2, object rest3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Array(object rest0, object rest1, object rest2, object rest3, object rest4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Array(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Array(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Array(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Array(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Array(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        /// <summary>
        /// Converts the elements in an array to strings, inserts the specified separator between
        /// the elements, concatenates them, and returns the resulting string. A nested array
        /// is always separated by a comma (,), not by the separator passed to the join()
        /// method.
        /// </summary>
        /// <param name="sep">
        /// A character or string that separates array elements in the returned string.
        /// If you omit this parameter, a comma is used as the default separator.
        /// </param>
        /// <returns>
        /// A string consisting of the
        /// elements of an array converted to strings and separated by the specified parameter.
        /// </returns>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("join", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String join(object sep);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("join", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String join();

        /// <summary>Removes the last element from an array and returns the value of that element.</summary>
        /// <returns>
        /// The value of the last element
        /// (of any data type) in the specified array.
        /// </returns>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.QName("pop", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object pop();

        /// <summary>
        /// Adds one or more elements to the end of an array and returns the new length of the
        /// array.
        /// </summary>
        /// <returns>
        /// An integer representing the length
        /// of the new array.
        /// </returns>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint push();

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object rest0);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object rest0, object rest1);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object rest0, object rest1, object rest2);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        /// <summary>Reverses the array in place.</summary>
        /// <returns>The new array.</returns>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.QName("reverse", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Array reverse();

        /// <summary>
        /// Concatenates the elements specified in the parameters with the elements in an array
        /// and creates a new array. If the parameters specify an array, the elements of that
        /// array are concatenated.
        /// </summary>
        /// <returns>
        /// An array that contains the elements
        /// from this array followed by elements from the parameters.
        /// </returns>
        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Array concat();

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Array concat(object rest0);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Array concat(object rest0, object rest1);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Array concat(object rest0, object rest1, object rest2);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Array concat(object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Array concat(object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Array concat(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Array concat(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Array concat(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Array concat(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.QName("concat", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Array concat(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        /// <summary>
        /// Removes the first element from an array and returns that element. The remaining
        /// array elements are moved from their original position, i, to i-1.
        /// </summary>
        /// <returns>
        /// The first element (of any
        /// data type) in an array.
        /// </returns>
        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.QName("shift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object shift();

        /// <summary>
        /// Returns a new array that consists of a range of elements from the original array,
        /// without modifying the original array. The returned array includes the startIndex
        /// element and all elements up to, but not including, the endIndex element.
        /// If you don&apos;t pass any parameters, a duplicate of the original array is created.
        /// </summary>
        /// <param name="startIndex">
        /// (default
        /// = 0)  A number specifying the index of the starting
        /// point for the slice. If startIndex is a negative number, the starting
        /// point begins at the end of the array, where -1 is the last element.
        /// </param>
        /// <param name="endIndex">
        /// (default
        /// = 16777215)  A number specifying the index of the
        /// ending point for the slice. If you omit this parameter, the slice includes all elements
        /// from the starting point to the end of the array. If endIndex is a negative
        /// number, the ending point is specified from the end of the array, where -1 is the
        /// last element.
        /// </param>
        /// <returns>
        /// An array that consists of a
        /// range of elements from the original array.
        /// </returns>
        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.QName("slice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Array slice(object startIndex, object endIndex);

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.QName("slice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Array slice(object startIndex);

        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.QName("slice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Array slice();

        /// <summary>
        /// Adds one or more elements to the beginning of an array and returns the new length
        /// of the array. The other elements in the array are moved from their original position,
        /// i, to i+1.
        /// </summary>
        /// <returns>
        /// An integer representing the new
        /// length of the array.
        /// </returns>
        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint unshift();

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object rest0);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object rest0, object rest1);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object rest0, object rest1, object rest2);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        /// <summary>
        /// Adds elements to and removes elements from an array. This method modifies the array
        /// without making a copy.
        /// Note: To override this method in a subclass of Array, use ...args
        /// for the parameters, as this example shows:
        /// public override function splice(...args) {
        /// // your statements here
        /// }
        /// </summary>
        /// <returns>
        /// An array containing the elements
        /// that were removed from the original array.
        /// </returns>
        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object splice();

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object splice(object rest0);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object splice(object rest0, object rest1);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object splice(object rest0, object rest1, object rest2);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object splice(object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object splice(object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object splice(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object splice(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object splice(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object splice(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcInstanceTrait(10)]
        [PageFX.ABC]
        [PageFX.QName("splice", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object splice(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        /// <summary>
        /// Sorts the elements in an array. This method sorts according to Unicode values. (ASCII
        /// is a subset of Unicode.)
        /// By default, Array.sort() works in the following way:Sorting is case-sensitive (Z precedes a).Sorting is ascending (a precedes b). The array is modified to reflect the sort order; multiple elements that have identical
        /// sort fields are placed consecutively in the sorted array in no particular order.All elements, regardless of data type, are sorted as if they were strings, so 100
        /// precedes 99, because &quot;1&quot; is a lower string value than &quot;9&quot;.
        /// To sort an array by using settings that deviate from the default settings, you can
        /// either use one of the sorting options described in the sortOptions
        /// portion of the ...args parameter description, or you can create your
        /// own custom function to do the sorting. If you create a custom function, you call
        /// the sort() method, and use the name of your custom function as the
        /// first argument (compareFunction)
        /// </summary>
        /// <returns>
        /// The return value depends on
        /// whether you pass any arguments, as described in the following list:
        /// If you specify a value of 4 or Array.UNIQUESORT for the sortOptions
        /// argument of the ...args parameter and two or more elements being sorted
        /// have identical sort fields, Flash returns a value of 0 and does not modify the array.
        /// If you specify a value of 8 or Array.RETURNINDEXEDARRAY for the
        /// sortOptions argument of the ...args parameter, Flash returns
        /// a sorted numeric array of the indices that reflects the results of the sort and
        /// does not modify the array. Otherwise, Flash returns nothing and modifies the array to reflect the sort order.
        /// </returns>
        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("sort", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object sort();

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("sort", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sort(object rest0);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("sort", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sort(object rest0, object rest1);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("sort", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sort(object rest0, object rest1, object rest2);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("sort", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sort(object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("sort", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sort(object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("sort", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sort(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("sort", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sort(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("sort", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sort(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("sort", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sort(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcInstanceTrait(11)]
        [PageFX.ABC]
        [PageFX.QName("sort", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sort(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        /// <summary>
        /// Sorts the elements in an array according to one or more fields in the array. The
        /// array should have the following characteristics:
        /// The array is an indexed array, not an associative array.Each element of the array holds an object with one or more properties.All of the objects have at least one property in common, the values of which can
        /// be used to sort the array. Such a property is called a field.
        /// If you pass multiple fieldName parameters, the first field represents
        /// the primary sort field, the second represents the next sort field, and so on. Flash
        /// sorts according to Unicode values. (ASCII is a subset of Unicode.) If either of
        /// the elements being compared does not contain the field that is specified in the
        /// fieldName parameter, the field is assumed to be set to undefined,
        /// and the elements are placed consecutively in the sorted array in no particular order.
        /// By default, Array.sortOn() works in the following way:Sorting is case-sensitive (Z precedes a).Sorting is ascending (a precedes b). The array is modified to reflect the sort order; multiple elements that have identical
        /// sort fields are placed consecutively in the sorted array in no particular order.Numeric fields are sorted as if they were strings, so 100 precedes 99, because &quot;1&quot;
        /// is a lower string value than &quot;9&quot;.
        /// Flash Player 7 added the options parameter, which you can use to override
        /// the default sort behavior. To sort a simple array (for example, an array with only
        /// one field), or to specify a sort order that the options parameter doesn&apos;t
        /// support, use Array.sort().
        /// To pass multiple flags, separate them with the bitwise OR (|) operator:
        /// my_array.sortOn(someFieldName, Array.DESCENDING | Array.NUMERIC);
        /// Flash Player 8 added the ability to specify a different sorting option for each
        /// field when you sort by more than one field. In Flash Player 8 and later, the options
        /// parameter accepts an array of sort options such that each sort option corresponds
        /// to a sort field in the fieldName parameter. The following example sorts
        /// the primary sort field, a, using a descending sort; the secondary sort
        /// field, b, using a numeric sort; and the tertiary sort field, c,
        /// using a case-insensitive sort:
        /// Array.sortOn ([&quot;a&quot;, &quot;b&quot;, &quot;c&quot;], [Array.DESCENDING, Array.NUMERIC, Array.CASEINSENSITIVE]);
        /// Note: The fieldName and options arrays must have
        /// the same number of elements; otherwise, the options array is ignored.
        /// Also, the Array.UNIQUESORT and Array.RETURNINDEXEDARRAY
        /// options can be used only as the first element in the array; otherwise, they are
        /// ignored.
        /// </summary>
        /// <param name="fieldName">
        /// A string that identifies a field to be used as the sort value, or an array
        /// in which the first element represents the primary sort field, the second represents
        /// the secondary sort field, and so on.
        /// </param>
        /// <param name="options">
        /// (default = null)  One or more numbers or names
        /// of defined constants, separated by the bitwise OR (|) operator, that
        /// change the sorting behavior. The following values are acceptable for the options
        /// parameter:
        /// Array.CASEINSENSITIVE or 1Array.DESCENDING or 2Array.UNIQUESORT or 4Array.RETURNINDEXEDARRAY
        /// or 8Array.NUMERIC or 16
        /// Code hinting is enabled if you use the string form of the flag (for example, DESCENDING)
        /// rather than the numeric form (2).
        /// </param>
        /// <returns>
        /// The return value depends on
        /// whether you pass any parameters:
        /// If you specify a value of 4 or Array.UNIQUESORT for the options
        /// parameter, and two or more elements being sorted have identical sort fields, a value
        /// of 0 is returned and the array is not modified. If you specify a value of 8 or Array.RETURNINDEXEDARRAY for the
        /// options parameter, an array is returned that reflects the results of the
        /// sort and the array is not modified.Otherwise, nothing is returned and the array is modified to reflect the sort order.
        /// </returns>
        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.QName("sortOn", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object sortOn(object fieldName, object options);

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.QName("sortOn", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sortOn(object fieldName, object options, object rest0);

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.QName("sortOn", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sortOn(object fieldName, object options, object rest0, object rest1);

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.QName("sortOn", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sortOn(object fieldName, object options, object rest0, object rest1, object rest2);

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.QName("sortOn", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sortOn(object fieldName, object options, object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.QName("sortOn", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sortOn(object fieldName, object options, object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.QName("sortOn", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sortOn(object fieldName, object options, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.QName("sortOn", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sortOn(object fieldName, object options, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.QName("sortOn", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sortOn(object fieldName, object options, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.QName("sortOn", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sortOn(object fieldName, object options, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.QName("sortOn", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sortOn(object fieldName, object options, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        [PageFX.AbcInstanceTrait(12)]
        [PageFX.ABC]
        [PageFX.QName("sortOn", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object sortOn(object fieldName);

        /// <summary>
        /// Searches for an item in an array by using strict equality (===) and
        /// returns the index position of the item.
        /// </summary>
        /// <param name="searchElement">The item to find in the array.</param>
        /// <param name="fromIndex">
        /// (default
        /// = 0)  The location in the array from which to start
        /// searching for the item.
        /// </param>
        /// <returns>
        /// A zero-based index position of the
        /// item in the array. If the searchElement argument is not found, the
        /// return value is -1.
        /// </returns>
        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.QName("indexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int indexOf(object searchElement, object fromIndex);

        [PageFX.AbcInstanceTrait(13)]
        [PageFX.ABC]
        [PageFX.QName("indexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int indexOf(object searchElement);

        /// <summary>
        /// Searches for an item in an array, working backward from the last item, and returns
        /// the index position of the matching item using strict equality (===).
        /// </summary>
        /// <param name="searchElement">The item to find in the array.</param>
        /// <param name="fromIndex">
        /// (default
        /// = 0x7fffffff)  The location in the array from which
        /// to start searching for the item. The default is the maximum value allowed for an
        /// index. If you do not specify fromIndex, the search starts at the last
        /// item in the array.
        /// </param>
        /// <returns>
        /// A zero-based index position of the
        /// item in the array. If the searchElement argument is not found, the
        /// return value is -1.
        /// </returns>
        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("lastIndexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int lastIndexOf(object searchElement, object fromIndex);

        [PageFX.AbcInstanceTrait(14)]
        [PageFX.ABC]
        [PageFX.QName("lastIndexOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int lastIndexOf(object searchElement);

        /// <summary>
        /// Executes a test function on each item in the array until an item is reached that
        /// returns false for the specified function. You use this method to determine
        /// whether all items in an array meet a criterion, such as having values less than
        /// a particular number.
        /// For this method, the second parameter, thisObject, must be null
        /// if the first parameter, callback, is a method closure. Suppose you
        /// create a function in a movie clip called me:
        /// function myFunction(){
        /// //your code here
        /// }
        /// Suppose you then use the filter() method on an array called myArray:
        /// myArray.filter(myFunction, me);
        /// Because myFunction is a member of the Timeline class, which cannot
        /// be overridden by me, Flash Player will throw an exception. You can
        /// avoid this runtime error by assigning the function to a variable, as follows:
        /// var foo:Function = myFunction() {
        /// //your code here
        /// };
        /// myArray.filter(foo, me);
        /// </summary>
        /// <param name="callback">
        /// The function to run on each item in the array. This function can contain
        /// a simple comparison (for example, item &lt; 20) or a more complex operation,
        /// and is invoked with three arguments; the value of an item, the index of an item,
        /// and the Array object:
        /// function callback(item:*, index:int, array:Array):Boolean;
        /// </param>
        /// <param name="thisObject">
        /// (default = null)  An object to use as this
        /// for the function.
        /// </param>
        /// <returns>
        /// A Boolean value of true
        /// if all items in the array return true for the specified function; otherwise,
        /// false.
        /// </returns>
        [PageFX.AbcInstanceTrait(15)]
        [PageFX.ABC]
        [PageFX.QName("every", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool every(Avm.Function callback, object thisObject);

        [PageFX.AbcInstanceTrait(15)]
        [PageFX.ABC]
        [PageFX.QName("every", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool every(Avm.Function callback);

        /// <summary>
        /// Executes a test function on each item in the array and constructs a new array for
        /// all items that return true for the specified function. If an item returns
        /// false, it is not included in the new array.
        /// For this method, the second parameter, thisObject, must be null
        /// if the first parameter, callback, is a method closure. Suppose you
        /// create a function in a movie clip called me:
        /// function myFunction(){
        /// //your code here
        /// }
        /// Suppose you then use the filter() method on an array called myArray:
        /// myArray.filter(myFunction, me);
        /// Because myFunction is a member of the Timeline class, which cannot
        /// be overridden by me, Flash Player will throw an exception. You can
        /// avoid this runtime error by assigning the function to a variable, as follows:
        /// var foo:Function = myFunction() {
        /// //your code here
        /// };
        /// myArray.filter(foo, me);
        /// </summary>
        /// <param name="callback">
        /// The function to run on each item in the array. This function can contain
        /// a simple comparison (for example, item &lt; 20) or a more complex operation,
        /// and is invoked with three arguments; the value of an item, the index of an item,
        /// and the Array object:
        /// function callback(item:*, index:int, array:Array):Boolean;
        /// </param>
        /// <param name="thisObject">
        /// (default = null)  An object to use as this
        /// for the function.
        /// </param>
        /// <returns>
        /// A new array that contains all
        /// items from the original array that returned true.
        /// </returns>
        [PageFX.AbcInstanceTrait(16)]
        [PageFX.ABC]
        [PageFX.QName("filter", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Array filter(Avm.Function callback, object thisObject);

        [PageFX.AbcInstanceTrait(16)]
        [PageFX.ABC]
        [PageFX.QName("filter", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Array filter(Avm.Function callback);

        /// <summary>
        /// Executes a function on each item in the array.
        /// For this method, the second parameter, thisObject, must be null
        /// if the first parameter, callback, is a method closure. Suppose you
        /// create a function in a movie clip called me:
        /// function myFunction(){
        /// //your code here
        /// }
        /// Suppose you then use the filter() method on an array called myArray:
        /// myArray.filter(myFunction, me);
        /// Because myFunction is a member of the Timeline class, which cannot
        /// be overridden by me, Flash Player will throw an exception. You can
        /// avoid this runtime error by assigning the function to a variable, as follows:
        /// var foo:Function = myFunction() {
        /// //your code here
        /// };
        /// myArray.filter(foo, me);
        /// </summary>
        /// <param name="callback">
        /// The function to run on each item in the array. This function can contain
        /// a simple command (for example, a trace() statement) or a more complex
        /// operation, and is invoked with three arguments; the value of an item, the index
        /// of an item, and the Array object:
        /// function callback(item:*, index:int, array:Array):void;
        /// </param>
        /// <param name="thisObject">
        /// (default = null)  An object to use as this
        /// for the function.
        /// </param>
        [PageFX.AbcInstanceTrait(17)]
        [PageFX.ABC]
        [PageFX.QName("forEach", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void forEach(Avm.Function callback, object thisObject);

        [PageFX.AbcInstanceTrait(17)]
        [PageFX.ABC]
        [PageFX.QName("forEach", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void forEach(Avm.Function callback);

        /// <summary>
        /// Executes a function on each item in an array, and constructs a new array of items
        /// corresponding to the results of the function on each item in the original array.
        /// For this method, the second parameter, thisObject, must be null
        /// if the first parameter, callback, is a method closure. Suppose you
        /// create a function in a movie clip called me:
        /// function myFunction(){
        /// //your code here
        /// }
        /// Suppose you then use the filter() method on an array called myArray:
        /// myArray.filter(myFunction, me);
        /// Because myFunction is a member of the Timeline class, which cannot
        /// be overridden by me, Flash Player will throw an exception. You can
        /// avoid this runtime error by assigning the function to a variable, as follows:
        /// var foo:Function = myFunction() {
        /// //your code here
        /// };
        /// myArray.filter(foo, me);
        /// </summary>
        /// <param name="callback">
        /// The function to run on each item in the array. This function can contain
        /// a simple command (such as changing the case of an array of strings) or a more complex
        /// operation, and is invoked with three arguments; the value of an item, the index
        /// of an item, and the Array object:
        /// function callback(item:*, index:int, array:Array):void;
        /// </param>
        /// <param name="thisObject">
        /// (default = null)  An object to use as this
        /// for the function.
        /// </param>
        /// <returns>
        /// A new array that contains the
        /// results of the function on each item in the original array.
        /// </returns>
        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("map", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Array map(Avm.Function callback, object thisObject);

        [PageFX.AbcInstanceTrait(18)]
        [PageFX.ABC]
        [PageFX.QName("map", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.Array map(Avm.Function callback);

        /// <summary>
        /// Executes a test function on each item in the array until an item is reached that
        /// returns true. Use this method to determine whether any items in an
        /// array meet a criterion, such as having a value less than a particular number.
        /// For this method, the second parameter, thisObject, must be null
        /// if the first parameter, callback, is a method closure. Suppose you
        /// create a function in a movie clip called me:
        /// function myFunction() {
        /// //your code here
        /// }
        /// Suppose you then use the filter() method on an array called myArray:
        /// myArray.filter(myFunction, me);
        /// Because myFunction is a member of the Timeline class, which cannot
        /// be overridden by me, Flash Player will throw an exception. You can
        /// avoid this runtime error by assigning the function to a variable, as follows:
        /// var foo:Function = myFunction() {
        /// //your code here
        /// };
        /// myArray.filter(foo, me);
        /// </summary>
        /// <param name="callback">
        /// The function to run on each item in the array. This function can contain
        /// a simple comparison (for example item &lt; 20) or a more complex operation,
        /// and is invoked with three arguments; the value of an item, the index of an item,
        /// and the Array object:
        /// function callback(item:*, index:int, array:Array):Boolean;
        /// </param>
        /// <param name="thisObject">
        /// (default = null)  An object to use as this
        /// for the function.
        /// </param>
        /// <returns>
        /// A Boolean value of true
        /// if any items in the array return true for the specified function; otherwise
        /// false.
        /// </returns>
        [PageFX.AbcInstanceTrait(19)]
        [PageFX.ABC]
        [PageFX.QName("some", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool some(Avm.Function callback, object thisObject);

        [PageFX.AbcInstanceTrait(19)]
        [PageFX.ABC]
        [PageFX.QName("some", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool some(Avm.Function callback);

        #region Custom Members
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint push(int arg);
        
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint push(uint arg);
        
        [PageFX.ABC]
        [PageFX.QName("push", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint push(Avm.String arg);
        
        public extern int Length
        {
			[PageFX.InlineProperty("length")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }
        
        public extern object this[int index]
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }
        
        //Methods below can be used to Minimize CIL by avoiding unnecessary casting operations
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool GetBool(int index);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int GetInt32(int index);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint GetUInt32(int index);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern double GetDouble(int index);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Avm.String GetString(int index);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern char GetChar(int index);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetValue(int index, bool value);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetValue(int index, char value);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetValue(int index, byte value);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetValue(int index, sbyte value);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetValue(int index, short value);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetValue(int index, ushort value);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetValue(int index, int value);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetValue(int index, uint value);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetValue(int index, float value);
        
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetValue(int index, double value);
        #endregion



    }
}
