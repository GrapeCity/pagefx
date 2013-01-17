using System;
using System.Runtime.CompilerServices;
using __AS3__.vec;

namespace Avm
{
    [PageFX.FP("10")]
    [PageFX.GenericVector]
    public class Vector<T> : Object
    {
        #region ctors
        /// <summary>
        /// Creates a <see cref="Vector{T}"/> instance. 
        /// </summary>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector();

        /// <summary>
        /// Creates a <see cref="Vector{T}"/> instance. 
        /// </summary>
        /// <param name="length">The initial length (number of elements) of the Vector.</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector(uint length);

        /// <summary>
        /// Creates a <see cref="Vector{T}"/> instance. 
        /// </summary>
        /// <param name="length">The initial length (number of elements) of the Vector. If this parameter is greater than zero, the specified number of Vector elements are created and populated with the default value appropriate to the base type (null for reference types).</param>
        /// <param name="fixed">Whether the Vector's length is fixed (true) or can be changed (false). This value can also be set using the <see cref="fixed"/> property.</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector(uint length, bool @fixed);
        #endregion

        public static implicit operator Vector(Vector<T> v)
        {
            throw new NotImplementedException();
        }

        public extern T this[int index]
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Gets the range of valid indices available in the <see cref="Vector{T}"/>.
        /// </summary>
        public virtual extern uint length
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Indicates if the length property of the <see cref="Vector{T}"/> can be changed.
        /// </summary>
        public virtual extern bool @fixed
        {
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        /// <summary>
        /// Removes the first element from the <see cref="Vector{T}"/> and returns that element.
        /// </summary>
        /// <returns>The first element in the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern T shift();

        /// <summary>
        /// Reverses the order of the elements in the <see cref="Vector{T}"/>.
        /// </summary>
        /// <returns>The <see cref="Vector{T}"/> with the elements in reverse order.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern Vector<T> reverse();

        /// <summary>
        /// Removes the last element from the <see cref="Vector{T}"/> and returns that element.
        /// </summary>
        /// <returns>The value of the last element in the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern T pop();

        #region unshift
        //[QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(T item);

        //[QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(T item1, T item2);

        //[QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(T item1, T item2, T item3);

        //[QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(T item1, T item2, T item3, T item4);

        //[QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(T item1, T item2, T item3, T item4, T item5);

        //[QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(T item1, T item2, T item3, T item4, T item5, T item6);

        //[QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(T item1, T item2, T item3, T item4, T item5, T item6, T item7);

        //[QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8);

        //[QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9);

        //[QName("unshift", "http://adobe.com/AS3/2006/builtin", "public")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint unshift(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9,
                                   T item10);
        #endregion

        #region indexOf, lastIndexOf
        /// <summary>
        /// Searches for an item in the <see cref="Vector{T}"/> and returns the index position of the item.
        /// </summary>
        /// <param name="searchElement">The item to find in the <see cref="Vector{T}"/>.</param>
        /// <param name="fromIndex">
        /// The location in the <see cref="Vector{T}"/> from which to start searching for the item.
        /// If this parameter is negative, it is treated as length + fromIndex, meaning the search starts -fromIndex items from the end and searches from that position forward to the end of the <see cref="Vector{T}"/>.
        /// </param>
        /// <returns>A zero-based index position of the item in the <see cref="Vector{T}"/>. If the searchElement argument is not found, the return value is -1.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern int indexOf(T searchElement, int fromIndex);

        /// <summary>
        /// Searches for an item in the <see cref="Vector{T}"/> and returns the index position of the item.
        /// </summary>
        /// <param name="searchElement">The item to find in the <see cref="Vector{T}"/>.</param>
        /// <returns>A zero-based index position of the item in the <see cref="Vector{T}"/>. If the searchElement argument is not found, the return value is -1.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int indexOf(T searchElement);

        /// <summary>
        /// Searches for an item in the <see cref="Vector{T}"/>, working backward from the specified index position, and returns the index position of the matching item.
        /// </summary>
        /// <param name="searchElement">The item to find in the <see cref="Vector{T}"/>.</param>
        /// <param name="fromIndex">The location in the <see cref="Vector{T}"/> from which to start searching for the item.</param>
        /// <returns>A zero-based index position of the item in the <see cref="Vector{T}"/>. If the searchElement argument is not found, the return value is -1.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern int lastIndexOf(T searchElement, int fromIndex);

        /// <summary>
        /// Searches for an item in the <see cref="Vector{T}"/>, working backward from the specified index position, and returns the index position of the matching item.
        /// </summary>
        /// <param name="searchElement">The item to find in the <see cref="Vector{T}"/>.</param>
        /// <returns>A zero-based index position of the item in the <see cref="Vector{T}"/>. If the searchElement argument is not found, the return value is -1.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern int lastIndexOf(T searchElement);
        #endregion

        #region slice
        /// <summary>
        /// Returns a new <see cref="Vector{T}"/> that consists of a range of elements from the original <see cref="Vector{T}"/>, without modifying the original <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="startIndex">A number specifying the index of the starting point for the slice. If startIndex is a negative number, the starting point begins at the end of the <see cref="Vector{T}"/>, where -1 is the last element.</param>
        /// <param name="endIndex">A number specifying the index of the ending point for the slice.</param>
        /// <returns>The <see cref="Vector{T}"/> that consists of a range of elements from the original <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern Vector<T> slice(int startIndex, int endIndex);

        /// <summary>
        /// Returns a new <see cref="Vector{T}"/> that consists of a range of elements from the original <see cref="Vector{T}"/>, without modifying the original <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="startIndex">A number specifying the index of the starting point for the slice. If startIndex is a negative number, the starting point begins at the end of the <see cref="Vector{T}"/>, where -1 is the last element.</param>
        /// <returns>The <see cref="Vector{T}"/> that consists of a range of elements from the original <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> slice(int startIndex);
        #endregion

        #region concat
        /// <summary>
        /// Concatenates the elements specified in the parameters with the elements in the <see cref="Vector{T}"/> and creates a new <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> concat(T item1);

        /// <summary>
        /// Concatenates the elements specified in the parameters with the elements in the <see cref="Vector{T}"/> and creates a new <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> concat(T item1, T item2);

        /// <summary>
        /// Concatenates the elements specified in the parameters with the elements in the <see cref="Vector{T}"/> and creates a new <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="item3"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> concat(T item1, T item2, T item3);

        /// <summary>
        /// Concatenates the elements specified in the parameters with the elements in the <see cref="Vector{T}"/> and creates a new <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="item3"></param>
        /// <param name="item4"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> concat(T item1, T item2, T item3, T item4);

        /// <summary>
        /// Concatenates the elements specified in the parameters with the elements in the <see cref="Vector{T}"/> and creates a new <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="item3"></param>
        /// <param name="item4"></param>
        /// <param name="item5"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> concat(T item1, T item2, T item3, T item4, T item5);

        /// <summary>
        /// Concatenates the elements specified in the parameters with the elements in the <see cref="Vector{T}"/> and creates a new <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="item3"></param>
        /// <param name="item4"></param>
        /// <param name="item5"></param>
        /// <param name="item6"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> concat(T item1, T item2, T item3, T item4, T item5, T item6);

        /// <summary>
        /// Concatenates the elements specified in the parameters with the elements in the <see cref="Vector{T}"/> and creates a new <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="item3"></param>
        /// <param name="item4"></param>
        /// <param name="item5"></param>
        /// <param name="item6"></param>
        /// <param name="item7"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> concat(T item1, T item2, T item3, T item4, T item5, T item6, T item7);

        /// <summary>
        /// Concatenates the elements specified in the parameters with the elements in the <see cref="Vector{T}"/> and creates a new <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="item3"></param>
        /// <param name="item4"></param>
        /// <param name="item5"></param>
        /// <param name="item6"></param>
        /// <param name="item7"></param>
        /// <param name="item8"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> concat(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8);

        /// <summary>
        /// Concatenates the elements specified in the parameters with the elements in the <see cref="Vector{T}"/> and creates a new <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="item3"></param>
        /// <param name="item4"></param>
        /// <param name="item5"></param>
        /// <param name="item6"></param>
        /// <param name="item7"></param>
        /// <param name="item8"></param>
        /// <param name="item9"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> concat(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9);

        /// <summary>
        /// Concatenates the elements specified in the parameters with the elements in the <see cref="Vector{T}"/> and creates a new <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1"></param>
        /// <param name="item2"></param>
        /// <param name="item3"></param>
        /// <param name="item4"></param>
        /// <param name="item5"></param>
        /// <param name="item6"></param>
        /// <param name="item7"></param>
        /// <param name="item8"></param>
        /// <param name="item9"></param>
        /// <param name="item10"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> concat(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9,
                                       T item10);
        #endregion

        #region push
        /// <summary>
        /// Adds elements to the end of the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item">item to add</param>
        /// <returns>The new length of the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item);

        /// <summary>
        /// Adds item to the end of the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1">item to add</param>
        /// <param name="item2">item to add</param>
        /// <returns>The new length of the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2);

        /// <summary>
        /// Adds elements to the end of the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1">item to add</param>
        /// <param name="item2">item to add</param>
        /// <param name="item3">item to add</param>
        /// <returns>The new length of the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3);

        /// <summary>
        /// Adds elements to the end of the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1">item to add</param>
        /// <param name="item2">item to add</param>
        /// <param name="item3">item to add</param>
        /// <param name="item4">item to add</param>
        /// <returns>The new length of the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4);

        /// <summary>
        /// Adds elements to the end of the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1">item to add</param>
        /// <param name="item2">item to add</param>
        /// <param name="item3">item to add</param>
        /// <param name="item4">item to add</param>
        /// <param name="item5">item to add</param>
        /// <returns>The new length of the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5);

        /// <summary>
        /// Adds elements to the end of the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1">item to add</param>
        /// <param name="item2">item to add</param>
        /// <param name="item3">item to add</param>
        /// <param name="item4">item to add</param>
        /// <param name="item5">item to add</param>
        /// <param name="item6">item to add</param>
        /// <returns>The new length of the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6);

        /// <summary>
        /// Adds elements to the end of the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1">item to add</param>
        /// <param name="item2">item to add</param>
        /// <param name="item3">item to add</param>
        /// <param name="item4">item to add</param>
        /// <param name="item5">item to add</param>
        /// <param name="item6">item to add</param>
        /// <param name="item7">item to add</param>
        /// <returns>The new length of the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7);

        /// <summary>
        /// Adds elements to the end of the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1">item to add</param>
        /// <param name="item2">item to add</param>
        /// <param name="item3">item to add</param>
        /// <param name="item4">item to add</param>
        /// <param name="item5">item to add</param>
        /// <param name="item6">item to add</param>
        /// <param name="item7">item to add</param>
        /// <param name="item8">item to add</param>
        /// <returns>The new length of the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8);

        /// <summary>
        /// Adds elements to the end of the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1">item to add</param>
        /// <param name="item2">item to add</param>
        /// <param name="item3">item to add</param>
        /// <param name="item4">item to add</param>
        /// <param name="item5">item to add</param>
        /// <param name="item6">item to add</param>
        /// <param name="item7">item to add</param>
        /// <param name="item8">item to add</param>
        /// <param name="item9">item to add</param>
        /// <returns>The new length of the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9);

        /// <summary>
        /// Adds elements to the end of the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1">item to add</param>
        /// <param name="item2">item to add</param>
        /// <param name="item3">item to add</param>
        /// <param name="item4">item to add</param>
        /// <param name="item5">item to add</param>
        /// <param name="item6">item to add</param>
        /// <param name="item7">item to add</param>
        /// <param name="item8">item to add</param>
        /// <param name="item9">item to add</param>
        /// <param name="item10">item to add</param>
        /// <returns>The new length of the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9,
                                T item10);

        /// <summary>
        /// Adds elements to the end of the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1">item to add</param>
        /// <param name="item2">item to add</param>
        /// <param name="item3">item to add</param>
        /// <param name="item4">item to add</param>
        /// <param name="item5">item to add</param>
        /// <param name="item6">item to add</param>
        /// <param name="item7">item to add</param>
        /// <param name="item8">item to add</param>
        /// <param name="item9">item to add</param>
        /// <param name="item10">item to add</param>
        /// <param name="item11">item to add</param>
        /// <returns>The new length of the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9,
                                T item10, T item11);

        /// <summary>
        /// Adds elements to the end of the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1">item to add</param>
        /// <param name="item2">item to add</param>
        /// <param name="item3">item to add</param>
        /// <param name="item4">item to add</param>
        /// <param name="item5">item to add</param>
        /// <param name="item6">item to add</param>
        /// <param name="item7">item to add</param>
        /// <param name="item8">item to add</param>
        /// <param name="item9">item to add</param>
        /// <param name="item10">item to add</param>
        /// <param name="item11">item to add</param>
        /// <param name="item12">item to add</param>
        /// <returns>The new length of the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9,
                                T item10, T item11, T item12);

        /// <summary>
        /// Adds elements to the end of the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1">item to add</param>
        /// <param name="item2">item to add</param>
        /// <param name="item3">item to add</param>
        /// <param name="item4">item to add</param>
        /// <param name="item5">item to add</param>
        /// <param name="item6">item to add</param>
        /// <param name="item7">item to add</param>
        /// <param name="item8">item to add</param>
        /// <param name="item9">item to add</param>
        /// <param name="item10">item to add</param>
        /// <param name="item11">item to add</param>
        /// <param name="item12">item to add</param>
        /// <param name="item13">item to add</param>
        /// <returns>The new length of the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9,
                                T item10, T item11, T item12, T item13);

        /// <summary>
        /// Adds elements to the end of the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1">item to add</param>
        /// <param name="item2">item to add</param>
        /// <param name="item3">item to add</param>
        /// <param name="item4">item to add</param>
        /// <param name="item5">item to add</param>
        /// <param name="item6">item to add</param>
        /// <param name="item7">item to add</param>
        /// <param name="item8">item to add</param>
        /// <param name="item9">item to add</param>
        /// <param name="item10">item to add</param>
        /// <param name="item11">item to add</param>
        /// <param name="item12">item to add</param>
        /// <param name="item13">item to add</param>
        /// <param name="item14">item to add</param>
        /// <returns>The new length of the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9,
                                T item10, T item11, T item12, T item13, T item14);

        /// <summary>
        /// Adds elements to the end of the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1">item to add</param>
        /// <param name="item2">item to add</param>
        /// <param name="item3">item to add</param>
        /// <param name="item4">item to add</param>
        /// <param name="item5">item to add</param>
        /// <param name="item6">item to add</param>
        /// <param name="item7">item to add</param>
        /// <param name="item8">item to add</param>
        /// <param name="item9">item to add</param>
        /// <param name="item10">item to add</param>
        /// <param name="item11">item to add</param>
        /// <param name="item12">item to add</param>
        /// <param name="item13">item to add</param>
        /// <param name="item14">item to add</param>
        /// <param name="item15">item to add</param>
        /// <returns>The new length of the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9,
                                T item10, T item11, T item12, T item13, T item14, T item15);

        /// <summary>
        /// Adds elements to the end of the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1">item to add</param>
        /// <param name="item2">item to add</param>
        /// <param name="item3">item to add</param>
        /// <param name="item4">item to add</param>
        /// <param name="item5">item to add</param>
        /// <param name="item6">item to add</param>
        /// <param name="item7">item to add</param>
        /// <param name="item8">item to add</param>
        /// <param name="item9">item to add</param>
        /// <param name="item10">item to add</param>
        /// <param name="item11">item to add</param>
        /// <param name="item12">item to add</param>
        /// <param name="item13">item to add</param>
        /// <param name="item14">item to add</param>
        /// <param name="item15">item to add</param>
        /// <param name="item16">item to add</param>
        /// <returns>The new length of the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9,
                                T item10, T item11, T item12, T item13, T item14, T item15, T item16);

        /// <summary>
        /// Adds elements to the end of the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1">item to add</param>
        /// <param name="item2">item to add</param>
        /// <param name="item3">item to add</param>
        /// <param name="item4">item to add</param>
        /// <param name="item5">item to add</param>
        /// <param name="item6">item to add</param>
        /// <param name="item7">item to add</param>
        /// <param name="item8">item to add</param>
        /// <param name="item9">item to add</param>
        /// <param name="item10">item to add</param>
        /// <param name="item11">item to add</param>
        /// <param name="item12">item to add</param>
        /// <param name="item13">item to add</param>
        /// <param name="item14">item to add</param>
        /// <param name="item15">item to add</param>
        /// <param name="item16">item to add</param>
        /// <param name="item17">item to add</param>
        /// <returns>The new length of the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9,
                                T item10, T item11, T item12, T item13, T item14, T item15, T item16, T item17);

        /// <summary>
        /// Adds elements to the end of the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1">item to add</param>
        /// <param name="item2">item to add</param>
        /// <param name="item3">item to add</param>
        /// <param name="item4">item to add</param>
        /// <param name="item5">item to add</param>
        /// <param name="item6">item to add</param>
        /// <param name="item7">item to add</param>
        /// <param name="item8">item to add</param>
        /// <param name="item9">item to add</param>
        /// <param name="item10">item to add</param>
        /// <param name="item11">item to add</param>
        /// <param name="item12">item to add</param>
        /// <param name="item13">item to add</param>
        /// <param name="item14">item to add</param>
        /// <param name="item15">item to add</param>
        /// <param name="item16">item to add</param>
        /// <param name="item17">item to add</param>
        /// <param name="item18">item to add</param>
        /// <returns>The new length of the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9,
                                T item10, T item11, T item12, T item13, T item14, T item15, T item16, T item17, T item18);

        /// <summary>
        /// Adds elements to the end of the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1">item to add</param>
        /// <param name="item2">item to add</param>
        /// <param name="item3">item to add</param>
        /// <param name="item4">item to add</param>
        /// <param name="item5">item to add</param>
        /// <param name="item6">item to add</param>
        /// <param name="item7">item to add</param>
        /// <param name="item8">item to add</param>
        /// <param name="item9">item to add</param>
        /// <param name="item10">item to add</param>
        /// <param name="item11">item to add</param>
        /// <param name="item12">item to add</param>
        /// <param name="item13">item to add</param>
        /// <param name="item14">item to add</param>
        /// <param name="item15">item to add</param>
        /// <param name="item16">item to add</param>
        /// <param name="item17">item to add</param>
        /// <param name="item18">item to add</param>
        /// <param name="item19">item to add</param>
        /// <returns>The new length of the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9,
                                T item10, T item11, T item12, T item13, T item14, T item15, T item16, T item17, T item18,
                                T item19);

        /// <summary>
        /// Adds elements to the end of the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="item1">item to add</param>
        /// <param name="item2">item to add</param>
        /// <param name="item3">item to add</param>
        /// <param name="item4">item to add</param>
        /// <param name="item5">item to add</param>
        /// <param name="item6">item to add</param>
        /// <param name="item7">item to add</param>
        /// <param name="item8">item to add</param>
        /// <param name="item9">item to add</param>
        /// <param name="item10">item to add</param>
        /// <param name="item11">item to add</param>
        /// <param name="item12">item to add</param>
        /// <param name="item13">item to add</param>
        /// <param name="item14">item to add</param>
        /// <param name="item15">item to add</param>
        /// <param name="item16">item to add</param>
        /// <param name="item17">item to add</param>
        /// <param name="item18">item to add</param>
        /// <param name="item19">item to add</param>
        /// <param name="item20">item to add</param>
        /// <returns>The new length of the <see cref="Vector{T}"/>.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern uint push(T item1, T item2, T item3, T item4, T item5, T item6, T item7, T item8, T item9,
                                T item10, T item11, T item12, T item13, T item14, T item15, T item16, T item17, T item18,
                                T item19, T item20);
        #endregion

        #region every, map, sort, forEach, filter, some
        /// <summary>
        /// Executes a test function on each item in the Vector until an item is reached that returns false for the specified function.
        /// </summary>
        /// <param name="callback">
        /// The function to run on each item in the <see cref="Vector{T}"/>.
        /// This function is invoked with three arguments: the current item from the <see cref="Vector{T}"/>,
        /// the index of the item, and the <see cref="Vector{T}"/> object.
        /// </param>
        /// <param name="thisObject">The object that the identifer this in the callback function refers to when the function is called.</param>
        /// <returns>true if the specified function returns true when called on all items in the <see cref="Vector{T}"/>; otherwise, false.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern bool every(Function callback, object thisObject);

        /// <summary>
        /// Executes a test function on each item in the Vector until an item is reached that returns false for the specified function.
        /// </summary>
        /// <param name="callback">The function to run on each item in the <see cref="Vector{T}"/>.</param>
        /// <returns>true if the specified function returns true when called on all items in the <see cref="Vector{T}"/>; otherwise, false.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool every(Function callback);

        /// <summary>
        /// Executes a test function on each item in the <see cref="Vector{T}"/> and returns a new <see cref="Vector{T}"/> containing all items that return true for the specified function.
        /// </summary>
        /// <param name="callback">The function to run on each item in the <see cref="Vector{T}"/>.</param>
        /// <param name="thisObject">The object that the identifer this in the callback function refers to when the function is called.</param>
        /// <returns>A new <see cref="Vector{T}"/> that contains all items from the original <see cref="Vector{T}"/> for which the callback function returned true.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern Vector<T> filter(Function callback, object thisObject);

        /// <summary>
        /// Executes a test function on each item in the <see cref="Vector{T}"/> and returns a new <see cref="Vector{T}"/> containing all items that return true for the specified function.
        /// </summary>
        /// <param name="callback">The function to run on each item in the <see cref="Vector{T}"/>.</param>
        /// <returns>A new <see cref="Vector{T}"/> that contains all items from the original <see cref="Vector{T}"/> for which the callback function returned true.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> filter(Function callback);

        /// <summary>
        /// Executes a function on each item in the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="callback">The function to run on each item in the <see cref="Vector{T}"/>.</param>
        /// <param name="thisObject">The object that the identifer this in the callback function refers to when the function is called.</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern void forEach(Function callback, object thisObject);

        /// <summary>
        /// Executes a function on each item in the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="callback">The function to run on each item in the <see cref="Vector{T}"/>.</param>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void forEach(Function callback);

        /// <summary>
        /// Executes a function on each item in the <see cref="Vector{T}"/>, and returns a new <see cref="Vector{T}"/> of items corresponding to the results of calling the function on each item in this <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="callback">The function to run on each item in the <see cref="Vector{T}"/>.</param>
        /// <param name="thisObject">The object that the identifer this in the callback function refers to when the function is called. </param>
        /// <returns>
        /// A new <see cref="Vector{T}"/> that contains the results of calling the function on each item in this <see cref="Vector{T}"/>.
        /// The result <see cref="Vector{T}"/> has the same base type and length as the original.
        /// </returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern Vector<T> map(Function callback, object thisObject);

        /// <summary>
        /// Executes a function on each item in the <see cref="Vector{T}"/>, and returns a new <see cref="Vector{T}"/> of items corresponding to the results of calling the function on each item in this <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="callback">The function to run on each item in the <see cref="Vector{T}"/>.</param>
        /// <returns>
        /// A new <see cref="Vector{T}"/> that contains the results of calling the function on each item in this <see cref="Vector{T}"/>.
        /// The result <see cref="Vector{T}"/> has the same base type and length as the original.
        /// </returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> map(Function callback);

        /// <summary>
        /// Sorts the elements in the <see cref="Vector{T}"/>.
        /// </summary>
        /// <param name="compareFunction">A comparison method that determines the behavior of the sort.</param>
        /// <returns>This <see cref="Vector{T}"/>, with elements in the new order. </returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern Vector<T> sort(Function compareFunction);

        /// <summary>
        /// Executes a test function on each item in the <see cref="Vector{T}"/> until an item is reached that returns true.
        /// </summary>
        /// <param name="callback">The function to run on each item in the <see cref="Vector{T}"/>.</param>
        /// <param name="thisObject">The object that the identifer this in the callback function refers to when the function is called.</param>
        /// <returns>true if any items in the <see cref="Vector{T}"/> return true for the specified function; otherwise false.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern bool some(Function callback, object thisObject);

        /// <summary>
        /// Executes a test function on each item in the <see cref="Vector{T}"/> until an item is reached that returns true.
        /// </summary>
        /// <param name="callback">The function to run on each item in the <see cref="Vector{T}"/>.</param>
        /// <returns>true if any items in the <see cref="Vector{T}"/> return true for the specified function; otherwise false.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool some(Function callback);
        #endregion

        #region splice
        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern Vector<T> splice(int startIndex, uint deleteCount);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> splice(int startIndex, uint deleteCount, T item1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> splice(int startIndex, uint deleteCount, T item1, T item2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> splice(int startIndex, uint deleteCount, T item1, T item2, T item3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> splice(int startIndex, uint deleteCount, T item1, T item2, T item3, T item4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> splice(int startIndex, uint deleteCount, T item1, T item2, T item3, T item4, T item5);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> splice(int startIndex, uint deleteCount, T item1, T item2, T item3, T item4, T item5,
                                       T item6);
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> splice(int startIndex, uint deleteCount, T item1, T item2, T item3, T item4, T item5,
                                       T item6, T item7);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> splice(int startIndex, uint deleteCount, T item1, T item2, T item3, T item4, T item5,
                                       T item6, T item7, T item8);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> splice(int startIndex, uint deleteCount, T item1, T item2, T item3, T item4, T item5,
                                       T item6, T item7, T item8, T item9);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Vector<T> splice(int startIndex, uint deleteCount, T item1, T item2, T item3, T item4, T item5,
                                       T item6, T item7, T item8, T item9, T item10);
        #endregion

        /// <summary>
        /// Returns a string that represents the elements in the <see cref="Vector{T}"/>.
        /// </summary>
        /// <returns>A string of Vector elements.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern String toString();

        /// <summary>
        /// Returns a string that represents the elements in the <see cref="Vector{T}"/>.
        /// </summary>
        /// <returns>A string of <see cref="Vector{T}"/> elements.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern String toLocaleString();

        /// <summary>
        /// Converts the elements in the <see cref="Vector{T}"/> to strings, inserts the specified separator between the elements, concatenates them, and returns the resulting string.
        /// </summary>
        /// <param name="sep">A character or string that separates <see cref="Vector{T}"/> elements in the returned string.</param>
        /// <returns>A string consisting of the elements of the <see cref="Vector{T}"/> converted to strings and separated by the specified string.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public virtual extern String join(String sep);

        /// <summary>
        /// Converts the elements in the <see cref="Vector{T}"/> to strings, inserts the comma between the elements, concatenates them, and returns the resulting string.
        /// </summary>
        /// <returns>A string consisting of the elements of the <see cref="Vector{T}"/> converted to strings and separated by comma.</returns>
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern String join();
    }
}