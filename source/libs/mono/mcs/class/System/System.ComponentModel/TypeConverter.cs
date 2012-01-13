//
// System.ComponentModel.TypeConverter.cs
//
// Authors:
//   Gonzalo Paniagua Javier (gonzalo@ximian.com)
//   Andreas Nahr (ClassDevelopment@A-SoftTech.com)
//
// (C) 2002/2003 Ximian, Inc (http://www.ximian.com)
// (C) 2003 Andreas Nahr
//

//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using System;
using System.Collections;
#if NOT_PFX
using System.ComponentModel.Design.Serialization;
#endif
using System.Globalization;
using System.Runtime.InteropServices;

namespace System.ComponentModel
{
	[ComVisible (true)]
	public class TypeConverter
	{
		public bool CanConvertFrom (Type sourceType)
		{
			return CanConvertFrom (
#if NOT_PFX
                null, 
#endif
                sourceType);
		}

#if NOT_PFX
public virtual bool CanConvertFrom (ITypeDescriptorContext context, Type sourceType)
		{
			if (sourceType == typeof (InstanceDescriptor)) {
				return true;
			}

			return false;
		}

#endif
#if NOT_PFX
        public bool CanConvertTo (Type destinationType)
		{
			return CanConvertTo (

null, 

                destinationType);
		}
#endif
        public virtual bool CanConvertTo (
#if NOT_PFX
ITypeDescriptorContext context,
#endif
            Type destinationType)
		{
			return (destinationType == typeof (string));
		}

		public object ConvertFrom (object o)
		{
			return ConvertFrom (null, CultureInfo.CurrentCulture, o);
		}

#if NOT_PFX
		public virtual object ConvertFrom (
ITypeDescriptorContext context,
            CultureInfo culture, object value)
		{
			if (value is InstanceDescriptor) {
				return ((InstanceDescriptor) value).Invoke ();
			}
			return GetConvertFromException (value);
		}
#endif

#if NOT_PFX
        public object ConvertFromInvariantString (string text)
		{
			return ConvertFromInvariantString (null,
                text); 
		}
#endif
		
        public object ConvertFromInvariantString (
#if NOT_PFX            
            ITypeDescriptorContext context, 
#endif
            string text)
		{
			return ConvertFromString (
#if NOT_PFX                
                context, 
#endif
                CultureInfo.InvariantCulture, text);
		}

		public object ConvertFromString (string text)
		{
			return ConvertFrom (text);
		}

#if NOT_PFX
		public object ConvertFromString (

            ITypeDescriptorContext context, 

            string text)
		{
			return ConvertFromString (

                context, 

                CultureInfo.CurrentCulture, text);
		}
#endif
        
        public object ConvertFromString (
#if NOT_PFX
ITypeDescriptorContext context,
#endif
            CultureInfo culture, string text)
		{
			return ConvertFrom (
#if NOT_PFX
context, 
#endif
                culture, text);
		}

		public object ConvertTo (object value, Type destinationType)
		{
			return ConvertTo (
#if NOT_PFX
null, 
#endif
                null, value, destinationType);
		}

		public virtual object ConvertTo (
#if NOT_PFX
ITypeDescriptorContext context,
#endif
            CultureInfo culture, object value,
						 Type destinationType)
		{
			// context? culture?
			if (destinationType == null)
				throw new ArgumentNullException ("destinationType");

			if (destinationType == typeof (string)) {
				if (value != null)
					return value.ToString();
				return String.Empty;
			}

			return GetConvertToException (value, destinationType);
		}

#if NOT_PFX
		public string ConvertToInvariantString (object value)
		{
			return ConvertToInvariantString (null, value);
		}

#endif
		public string ConvertToInvariantString (
#if NOT_PFX
ITypeDescriptorContext context,
#endif
		    object value)
		{
			return (string) ConvertTo (
#if NOT_PFX
context, 
#endif
                CultureInfo.InvariantCulture, value, typeof (string));
		}

		public string ConvertToString (object value)
		{
			return (string) ConvertTo (
#if NOT_PFX
null, 
#endif
                CultureInfo.CurrentCulture, value, typeof (string));
		}

#if NOT_PFX
		public string ConvertToString (ITypeDescriptorContext context, object value)
		{
			return (string) ConvertTo (context, CultureInfo.CurrentCulture, value, typeof (string));
		}
#endif
		public string ConvertToString (
#if NOT_PFX
ITypeDescriptorContext context,
#endif
            CultureInfo culture, object value)
		{
			return (string) ConvertTo (
#if NOT_PFX
context,
#endif
                culture, value, typeof (string));
		}

		protected Exception GetConvertFromException (object value)
		{
			string destinationType;
			if (value == null)
				destinationType = "(null)";
			else
				destinationType = value.GetType ().FullName;

			throw new NotSupportedException (string.Format (CultureInfo.InvariantCulture,
				"{0} cannot convert from {1}.", this.GetType ().Name,
				destinationType));
		}

		protected Exception GetConvertToException (object value, Type destinationType)
		{
			string sourceType;
			if (value == null)
				sourceType = "(null)";
			else
				sourceType = value.GetType ().FullName;

			throw new NotSupportedException (string.Format (CultureInfo.InvariantCulture,
				"'{0}' is unable to convert '{1}' to '{2}'.", this.GetType ().Name,
				sourceType, destinationType.FullName));
		}

#if NOT_PFX
public object CreateInstance (IDictionary propertyValues)
		{
			return CreateInstance (null, propertyValues);
		}
#endif

		public virtual object CreateInstance (
#if NOT_PFX
ITypeDescriptorContext context,
#endif
            IDictionary propertyValues)
		{
			return null;
		}

#if NOT_PFX
		public bool GetCreateInstanceSupported ()
		{
			return GetCreateInstanceSupported (null);
		}

#endif
		public virtual bool GetCreateInstanceSupported (
#if NOT_PFX
ITypeDescriptorContext context
#endif
            )
		{
			return false;
		}

#if NOT_PFX
		public PropertyDescriptorCollection GetProperties (object value)
		{
			return GetProperties (null, value);
		}

		public PropertyDescriptorCollection GetProperties (ITypeDescriptorContext context, object value)
		{
			return GetProperties (context, value, new Attribute[1] { BrowsableAttribute.Yes });
		}

		public virtual PropertyDescriptorCollection GetProperties (ITypeDescriptorContext context,
									   object value, Attribute[] attributes)
		{
			return null;
		}


		public bool GetPropertiesSupported ()
		{
			return GetPropertiesSupported (null);
		}
#endif
        
        public virtual bool GetPropertiesSupported (
#if NOT_PFX
ITypeDescriptorContext context
#endif
            )
		{
			return false;
		}

#if NOT_PFX
		public ICollection GetStandardValues ()
		{
			return GetStandardValues (null);
		}

#endif
		public virtual StandardValuesCollection GetStandardValues (
#if NOT_PFX
ITypeDescriptorContext context
#endif
            )
		{
			return null;
		}

#if NOT_PFX
		public bool GetStandardValuesExclusive ()
		{
			return GetStandardValuesExclusive (null);
		}

#endif
		public virtual bool GetStandardValuesExclusive (
#if NOT_PFX
ITypeDescriptorContext context
#endif
            )
		{
			return false;
		}

#if NOT_PFX
public bool GetStandardValuesSupported ()
		{
			return GetStandardValuesSupported (null);
		}
#endif

		public virtual bool GetStandardValuesSupported (
#if NOT_PFX
ITypeDescriptorContext context
#endif
            )
		{
			return false;
		}

#if NOT_PFX
public bool IsValid (object value)
		{
			return IsValid (null, value);
		}
#endif

		public virtual bool IsValid (
#if NOT_PFX
ITypeDescriptorContext context,
#endif
            object value)
		{
			return true;
		}

#if NOT_PFX
protected PropertyDescriptorCollection SortProperties (PropertyDescriptorCollection props, string[] names)
		{
			props.Sort (names);
			return props; 
		}

#endif
		public class StandardValuesCollection : ICollection, IEnumerable
		{
			private ICollection values;

			public StandardValuesCollection (ICollection values)
			{
				this.values = values;
			}

			void ICollection.CopyTo (Array array, int index) {
				CopyTo (array, index);
			}

			public void CopyTo (Array array, int index)
			{
				values.CopyTo (array, index);
			}

			IEnumerator IEnumerable.GetEnumerator () {
				return GetEnumerator ();
			}

			public IEnumerator GetEnumerator ()
			{
				return values.GetEnumerator ();
			}

			bool ICollection.IsSynchronized {
				get { return false; }
			}

			object ICollection.SyncRoot {
				get { return null; }
			}

			int ICollection.Count {
				get { return this.Count; }
			}

			public int Count {
				get { return values.Count; }
			}

			public object this [int index] {
				get { return ((IList) values) [index]; }
			}
		}

		protected abstract class SimplePropertyDescriptor 
#if NOT_PFX
            : PropertyDescriptor
#endif

		{
			private Type componentType;
			private Type propertyType;

			public SimplePropertyDescriptor (Type componentType,
							 string name,
							 Type propertyType) :
				this (componentType, name, propertyType, null)
			{
			}

			public SimplePropertyDescriptor (Type componentType,
							 string name,
							 Type propertyType,
							 Attribute [] attributes) 
#if NOT_PFX                
                : base (name, attributes)
#endif
			{
				this.componentType = componentType;
				this.propertyType = propertyType;
			}

			public 
#if NOT_PFX
                override 
#endif
                Type ComponentType {
				get { return componentType; }
			}

			public
#if NOT_PFX
                override

#endif                
                Type PropertyType {
				get { return propertyType; }
                }

#if NOT_PFX
			public override bool IsReadOnly {
				get { return Attributes.Contains (ReadOnlyAttribute.Yes); }
			}
#endif

                public
#if NOT_PFX
                override
#endif
                bool ShouldSerializeValue (object component)
			{
					return false;
                }

#if NOT_PFX
			public override bool CanResetValue (object component)
			{
				DefaultValueAttribute Attrib = ((DefaultValueAttribute) Attributes[typeof (DefaultValueAttribute)]);
				if (Attrib == null) {
					return false; 
				}
				return (Attrib.Value == GetValue (component)); 
			}

			public override void ResetValue (object component)
			{
				DefaultValueAttribute Attrib = ((DefaultValueAttribute) Attributes[typeof (DefaultValueAttribute)]);
				if (Attrib != null) {
					SetValue (component, Attrib.Value); 
				}
 
			} 
#endif
            }
	}
}

