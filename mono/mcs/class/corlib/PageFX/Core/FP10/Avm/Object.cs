using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    /// <summary>
    /// The Object class is at the root of the ActionScript runtime
    /// class hierarchy. Objects are created by constructors using the new
    /// operator syntax, and can have properties assigned to them dynamically. Objects
    /// can also be created by assigning an object literal, as in: var obj:Object = {a:&quot;foo&quot;, b:&quot;bar&quot;}
    /// </summary>
    [PageFX.ABC]
    [PageFX.QName("Object")]
    [PageFX.FP9]
    public class Object
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Object();

        /// <summary>
        /// Indicates whether an object has a specified property defined. This method returns
        /// true if the target object has a property that matches the string specified
        /// by the name parameter, and false otherwise. The following
        /// types of properties cause this method to return true for objects that
        /// are instances of a class (as opposed to class objects):
        /// Fixed instance properties—variables, constants, or methods defined by the
        /// object&apos;s class that are not static;Inherited fixed instance properties—variables, constants, or methods inherited
        /// by the object&apos;s class;Dynamic properties—properties added to an object after it is instantiated
        /// (outside of its class definition). To add dynamic properties, the object&apos;s defining
        /// class must be declared with the dynamic keyword.
        /// The following types of properties cause this method to return false
        /// for objects that are instances of a class:Static properties—variables, constants, or methods defined with the static
        /// keyword in an object&apos;s defining class or any of its superclasses;Prototype properties—properties defined on a prototype object that is part
        /// of the object&apos;s prototype chain. In ActionScript 3.0, the prototype chain is not
        /// used for class inheritance, but still exists as an alternative form of inheritance.
        /// For example, an instance of the Array class can access the valueOf()
        /// method because it exists on Object.prototype, which is part of the
        /// prototype chain for the Array class. Although you can use valueOf()
        /// on an instance of Array, the return value of hasOwnProperty(&quot;valueOf&quot;)
        /// for that instance is false.
        /// ActionScript 3.0 also has class objects, which are direct representations of class
        /// definitions. When called on class objects, the hasOwnProperty() method
        /// returns true only if a property is a static property defined on that
        /// class object. For example, if you create a subclass of Array named CustomArray,
        /// and define a static property in CustomArray named foo, a call to
        /// CustomArray.hasOwnProperty(&quot;foo&quot;) returns true. For the
        /// static property DESCENDING defined in the Array class, however, a call
        /// to CustomArray.hasOwnProperty(&quot;DESCENDING&quot;) returns false.Note: Methods of the Object class are dynamically created on Object&apos;s prototype.
        /// To redefine this method in a subclass of Object, do not use the override
        /// keyword. For example, A subclass of Object implements function hasOwnProperty():Boolean
        /// instead of using an override of the base class.
        /// </summary>
        /// <param name="arg0">The property of the object.</param>
        /// <returns>
        /// If the target object has
        /// the property specified by the name parameter this value is true,
        /// otherwise false.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("hasOwnProperty", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool hasOwnProperty(object arg0);

        [PageFX.ABC]
        [PageFX.QName("hasOwnProperty", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool hasOwnProperty();

        /// <summary>
        /// Indicates whether the specified property exists and is enumerable. If true,
        /// then the property exists and can be enumerated in a for..in loop. The
        /// property must exist on the target object because this method does not check the
        /// target object&apos;s prototype chain.
        /// Properties that you create are enumerable, but built-in properties are generally
        /// not enumerable.Note: Methods of the Object class are dynamically created on Object&apos;s prototype.
        /// To redefine this method in a subclass of Object, do not use the override
        /// keyword. For example, A subclass of Object implements function propertyIsEnumerable():Boolean
        /// instead of using an override of the base class.
        /// </summary>
        /// <param name="arg0">The property of the object.</param>
        /// <returns>
        /// If the property specified
        /// by the name parameter is enumerable this value is true,
        /// otherwise false.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("propertyIsEnumerable", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool propertyIsEnumerable(object arg0);

        [PageFX.ABC]
        [PageFX.QName("propertyIsEnumerable", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool propertyIsEnumerable();

        /// <summary>
        /// Indicates whether an instance of the Object class is in the prototype chain of the
        /// object specified as the parameter. This method returns true if the
        /// object is in the prototype chain of the object specified by the theClass
        /// parameter. The method returns false if the target object is absent
        /// from the prototype chain of the theClass object, and also if the
        /// theClass parameter is not an object.
        /// Note: Methods of the Object class are dynamically created on Object&apos;s prototype.
        /// To redefine this method in a subclass of Object, do not use the override
        /// keyword. For example, A subclass of Object implements function isPrototypeOf():Boolean
        /// instead of using an override of the base class.
        /// </summary>
        /// <param name="arg0">The class to which the specified object may refer.</param>
        /// <returns>
        /// If the object is in the
        /// prototype chain of the object specified by the theClass parameter this
        /// value is true, otherwise false.
        /// </returns>
        [PageFX.ABC]
        [PageFX.QName("isPrototypeOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool isPrototypeOf(object arg0);

        [PageFX.ABC]
        [PageFX.QName("isPrototypeOf", "http://adobe.com/AS3/2006/builtin", "public")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool isPrototypeOf();

        [PageFX.ABC]
        [PageFX.QName("_dontEnumPrototype", "Object", "static protected")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        protected extern static void _dontEnumPrototype(object arg0);

        [PageFX.ABC]
        [PageFX.QName("_setPropertyIsEnumerable", "Object", "static protected")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        protected extern static void _setPropertyIsEnumerable(object arg0, String arg1, bool arg2);

        #region Custom Members
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object GetProperty(string name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object GetProperty(string ns, string name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object GetProperty(Namespace ns, string name);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetProperty(string name, object value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetProperty(string ns, string name, object value);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern void SetProperty(Namespace ns, string name, object value);
        #endregion
    }
}
