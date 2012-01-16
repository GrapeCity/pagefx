using System;
using System.Runtime.CompilerServices;

namespace flash.utils
{
    /// <summary>
    /// The Proxy class lets you override the default behavior of ActionScript operations
    /// (such as retrieving and modifying properties) on an object.
    /// </summary>
    [PageFX.AbcInstance(268)]
    [PageFX.ABC]
    [PageFX.FP9]
    public class Proxy : Avm.Object
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Proxy();

        /// <summary>
        /// Overrides any request for a property&apos;s value. If the property can&apos;t be found, the
        /// method returns undefined. For more information on this behavior, see
        /// the ECMA-262 Language Specification, 3rd Edition, section 8.6.2.1.
        /// </summary>
        /// <param name="name">The name of the property to retrieve.</param>
        /// <returns>
        /// The specified property
        /// or undefined if the property is not found.
        /// </returns>
        [PageFX.AbcInstanceTrait(0)]
        [PageFX.ABC]
        [PageFX.QName("getProperty", "http://www.adobe.com/2006/actionscript/flash/proxy", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object getProperty(object name);

        /// <summary>
        /// Overrides a call to change a property&apos;s value. If the property can&apos;t be found, this
        /// method creates a property with the specified name and value.
        /// </summary>
        /// <param name="name">The name of the property to modify.</param>
        /// <param name="value">The value to set the property to.</param>
        [PageFX.AbcInstanceTrait(1)]
        [PageFX.ABC]
        [PageFX.QName("setProperty", "http://www.adobe.com/2006/actionscript/flash/proxy", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void setProperty(object name, object value);

        /// <summary>
        /// Overrides the behavior of an object property that can be called as a function. When
        /// a method of the object is invoked, this method is called. While some objects can
        /// be called as functions, some object properties can also be called as functions.
        /// </summary>
        /// <param name="name">The name of the method being invoked.</param>
        /// <returns>
        /// The return value
        /// of the called method.
        /// </returns>
        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("callProperty", "http://www.adobe.com/2006/actionscript/flash/proxy", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object callProperty(object name);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("callProperty", "http://www.adobe.com/2006/actionscript/flash/proxy", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object callProperty(object name, object rest0);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("callProperty", "http://www.adobe.com/2006/actionscript/flash/proxy", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object callProperty(object name, object rest0, object rest1);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("callProperty", "http://www.adobe.com/2006/actionscript/flash/proxy", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object callProperty(object name, object rest0, object rest1, object rest2);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("callProperty", "http://www.adobe.com/2006/actionscript/flash/proxy", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object callProperty(object name, object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("callProperty", "http://www.adobe.com/2006/actionscript/flash/proxy", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object callProperty(object name, object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("callProperty", "http://www.adobe.com/2006/actionscript/flash/proxy", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object callProperty(object name, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("callProperty", "http://www.adobe.com/2006/actionscript/flash/proxy", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object callProperty(object name, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("callProperty", "http://www.adobe.com/2006/actionscript/flash/proxy", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object callProperty(object name, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("callProperty", "http://www.adobe.com/2006/actionscript/flash/proxy", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object callProperty(object name, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcInstanceTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("callProperty", "http://www.adobe.com/2006/actionscript/flash/proxy", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object callProperty(object name, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        /// <summary>Overrides a request to check whether an object has a particular property by name.</summary>
        /// <param name="name">The name of the property to check for.</param>
        /// <returns>
        /// If the property exists,
        /// true; otherwise false.
        /// </returns>
        [PageFX.AbcInstanceTrait(3)]
        [PageFX.ABC]
        [PageFX.QName("hasProperty", "http://www.adobe.com/2006/actionscript/flash/proxy", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool hasProperty(object name);

        /// <summary>
        /// Overrides the request to delete a property. When a property is deleted with the
        /// delete operator, this method is called to perform the deletion.
        /// </summary>
        /// <param name="name">The name of the property to delete.</param>
        /// <returns>
        /// If the property was
        /// deleted, true; otherwise false.
        /// </returns>
        [PageFX.AbcInstanceTrait(4)]
        [PageFX.ABC]
        [PageFX.QName("deleteProperty", "http://www.adobe.com/2006/actionscript/flash/proxy", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool deleteProperty(object name);

        /// <summary>
        /// Overrides the use of the descendant operator. When the descendant
        /// operator is used, this method is invoked.
        /// </summary>
        /// <param name="name">The name of the property to descend into the object and search for.</param>
        /// <returns>
        /// The results of the
        /// descendant operator.
        /// </returns>
        [PageFX.AbcInstanceTrait(5)]
        [PageFX.ABC]
        [PageFX.QName("getDescendants", "http://www.adobe.com/2006/actionscript/flash/proxy", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object getDescendants(object name);

        /// <summary>
        /// Allows enumeration of the proxied object&apos;s properties by index number. However,
        /// you cannot enumerate the properties of the Proxy class themselves. This function
        /// supports implementing for...in and for each..in loops
        /// on the object to retrieve property index values.
        /// For example:
        /// protected var _item:Array; // array of object&apos;s properties
        /// override flash_proxy function nextNameIndex (index:int):int {
        /// // initial call
        /// if (index == 0) {
        /// _item = new Array();
        /// for (var x:~~ in _target) {
        /// _item.push(x);
        /// }
        /// }
        /// if (index &lt; _item.length) {
        /// return index + 1;
        /// } else {
        /// return 0;
        /// }
        /// }
        /// override flash_proxy function nextName(index:int):String {
        /// return _item[index - 1];
        /// }
        /// </summary>
        /// <param name="index">The zero-based index value where the enumeration begins.</param>
        /// <returns>The property&apos;s index value.</returns>
        [PageFX.AbcInstanceTrait(6)]
        [PageFX.ABC]
        [PageFX.QName("nextNameIndex", "http://www.adobe.com/2006/actionscript/flash/proxy", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual int nextNameIndex(int index);

        /// <summary>
        /// Allows enumeration of the proxied object&apos;s properties by index number to retrieve
        /// property names. However, you cannot enumerate the properties of the Proxy class
        /// themselves. This function supports implementing for...in and for
        /// each..in loops on the object to retrieve the desired names.
        /// For example (with code from Proxy.nextNameIndex()):
        /// protected var _item:Array; // array of object&apos;s properties
        /// override flash_proxy function nextNameIndex (index:int):int {
        /// // initial call
        /// if (index == 0) {
        /// _item = new Array();
        /// for (var x:~~ in _target) {
        /// _item.push(x);
        /// }
        /// }
        /// if (index &lt; _item.length) {
        /// return index + 1;
        /// } else {
        /// return 0;
        /// }
        /// }
        /// override flash_proxy function nextName(index:int):String {
        /// return _item[index - 1];
        /// }
        /// </summary>
        /// <param name="index">The zero-based index value of the object&apos;s property.</param>
        /// <returns>
        /// String The property&apos;s
        /// name.
        /// </returns>
        [PageFX.AbcInstanceTrait(7)]
        [PageFX.ABC]
        [PageFX.QName("nextName", "http://www.adobe.com/2006/actionscript/flash/proxy", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String nextName(int index);

        /// <summary>
        /// Allows enumeration of the proxied object&apos;s properties by index number to retrieve
        /// property values. However, you cannot enumerate the properties of the Proxy class
        /// themselves. This function supports implementing for...in and for
        /// each..in loops on the object to retrieve the desired values.
        /// For example (with code from Proxy.nextNameIndex()):
        /// protected var _item:Array; // array of object&apos;s properties
        /// override flash_proxy function nextNameIndex (index:int):int {
        /// // initial call
        /// if (index == 0) {
        /// _item = new Array();
        /// for (var x:~~ in _target) {
        /// _item.push(x);
        /// }
        /// }
        /// if (index &lt; _item.length) {
        /// return index + 1;
        /// } else {
        /// return 0;
        /// }
        /// }
        /// override flash_proxy function nextName(index:int):String {
        /// return _item[index - 1];
        /// }
        /// </summary>
        /// <param name="index">The zero-based index value of the object&apos;s property.</param>
        /// <returns>The property&apos;s value.</returns>
        [PageFX.AbcInstanceTrait(8)]
        [PageFX.ABC]
        [PageFX.QName("nextValue", "http://www.adobe.com/2006/actionscript/flash/proxy", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object nextValue(int index);

        /// <summary>Checks whether a supplied QName is also marked as an attribute.</summary>
        /// <param name="name">The name of the property to check.</param>
        /// <returns>
        /// Returns true
        /// if the argument for name is a QName that is also marked as an attribute.
        /// </returns>
        [PageFX.AbcInstanceTrait(9)]
        [PageFX.ABC]
        [PageFX.QName("isAttribute", "http://www.adobe.com/2006/actionscript/flash/proxy", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool isAttribute(object name);
    }
}
