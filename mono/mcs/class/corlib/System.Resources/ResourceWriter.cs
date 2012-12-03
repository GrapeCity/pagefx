//
// System.Resources.ResourceWriter.cs
//
// Authors:
//	Duncan Mak <duncan@ximian.com>
//	Dick Porter <dick@ximian.com>
//
// (C) 2001, 2002 Ximian, Inc. 	http://www.ximian.com
//

//
// Copyright (C) 2004 Novell, Inc (http://www.novell.com)
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

using System.IO;
using System.Collections;
using System.Text;
using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;

namespace System.Resources
{
#if NET_2_0
	[System.Runtime.InteropServices.ComVisible (true)]
#endif
	public sealed class ResourceWriter : IResourceWriter, IDisposable
	{
		class TypeByNameObject
		{
			public readonly string TypeName;
			public readonly byte [] Value;

			public TypeByNameObject (string typeName, byte [] value)
			{
				TypeName = typeName;
				Value = (byte []) value.Clone ();
			}
		}

#if NET_2_0
		SortedList resources = new SortedList (StringComparer.Ordinal);
#else
		Hashtable resources = new Hashtable(CaseInsensitiveHashCodeProvider.Default, CaseInsensitiveComparer.Default);
#endif
		Stream stream;
		
		public ResourceWriter (Stream stream)
		{
			if (stream == null)
				throw new ArgumentNullException ("stream is null");
			if (stream.CanWrite == false)
				throw new ArgumentException ("stream is not writable.");

			this.stream=stream;
		}
		
		public ResourceWriter (String fileName)
		{
			if (fileName == null)
				throw new ArgumentNullException ("fileName is null.");

			stream=new FileStream(fileName, FileMode.Create, FileAccess.Write);
		}
		
		public void AddResource (string name, byte[] value)
		{
			if (name == null) {
				throw new ArgumentNullException ("name is null");
			}
			if (value == null) {
				throw new ArgumentNullException ("value is null");
			}
			if(resources==null) {
				throw new InvalidOperationException ("ResourceWriter has been closed");
			}
			if(resources[name]!=null) {
				throw new ArgumentException ("Resource already present: " + name);
			}

			resources.Add(name, value);
		}
		
		public void AddResource (string name, object value)
		{			 
			if (name == null) {
				throw new ArgumentNullException ("name is null");
			}
			if(resources==null) {
				throw new InvalidOperationException ("ResourceWriter has been closed");
			}
			if(resources[name]!=null) {
				throw new ArgumentException ("Resource already present: " + name);
			}

			resources.Add(name, value);
		}
		
		public void AddResource (string name, string value)
		{
			if (name == null) {
				throw new ArgumentNullException ("name is null");
			}
			if (value == null) {
				throw new ArgumentNullException ("value is null");
			}
			if(resources==null) {
				throw new InvalidOperationException ("ResourceWriter has been closed");
			}
			if(resources[name]!=null) {
				throw new ArgumentException ("Resource already present: " + name);
			}

			resources.Add(name, value);
		}

		public void Close () {
			Dispose(true);
		}
		
		public void Dispose ()
		{
			Dispose(true);
		}

		private void Dispose (bool disposing)
		{
			if(disposing) {
				if(resources.Count>0 && generated==false) {
					Generate();
				}
				if(stream!=null) {
					stream.Close();
				}
			}
			resources=null;
			stream=null;
		}

#if NET_2_0
		public void AddResourceData (string name, string typeName, byte [] serializedData)
		{
			if (name == null)
				throw new ArgumentNullException ("name");
			if (typeName == null)
				throw new ArgumentNullException ("typeName");
			if (serializedData == null)
				throw new ArgumentNullException ("serializedData");

			// shortcut
			AddResource (name, new TypeByNameObject (typeName, serializedData));
		}
#endif

		private bool generated=false;
		
		public void Generate ()
        {
//            BinaryWriter writer;
//            IFormatter formatter;

//            if(resources==null) {
//                throw new InvalidOperationException ("ResourceWriter has been closed");
//            }

//            if(generated) {
//                throw new InvalidOperationException ("ResourceWriter can only Generate() once");
//            }
//            generated=true;
			
//            writer=new BinaryWriter(stream, Encoding.UTF8);
//            formatter=new BinaryFormatter(null, new StreamingContext(StreamingContextStates.File|StreamingContextStates.Persistence));

//            /* The ResourceManager header */
			
//            writer.Write(ResourceManager.MagicNumber);
//            writer.Write(ResourceManager.HeaderVersionNumber);
			
//            /* Build the rest of the ResourceManager
//             * header in memory, because we need to know
//             * how long it is in advance
//             */
//            MemoryStream resman_stream=new MemoryStream();
//            BinaryWriter resman=new BinaryWriter(resman_stream,
//                                 Encoding.UTF8);

//            resman.Write(typeof(ResourceReader).AssemblyQualifiedName);
//#if NET_2_0
//            resman.Write(typeof(ResourceSet).FullName);
//#else
//            resman.Write(typeof(ResourceSet).AssemblyQualifiedName);
//#endif

//            /* Only space for 32 bits of header len in the
//             * resource file format
//             */
//            int resman_len=(int)resman_stream.Length;
//            writer.Write(resman_len);
//            writer.Write(resman_stream.GetBuffer(), 0, resman_len);

//            /* We need to build the ResourceReader name
//             * and data sections simultaneously
//             */
//            MemoryStream res_name_stream=new MemoryStream();
//            BinaryWriter res_name=new BinaryWriter(res_name_stream, Encoding.Unicode);

//            MemoryStream res_data_stream=new MemoryStream();
//            BinaryWriter res_data=new BinaryWriter(res_data_stream,
//                                   Encoding.UTF8);

//            /* Not sure if this is the best collection to
//             * use, I just want an unordered list of
//             * objects with fast lookup, but without
//             * needing a key.  (I suppose a hashtable with
//             * key==value would work, but I need to find
//             * the index of each item later)
//             */
//            ArrayList types=new ArrayList();
//            int[] hashes=new int[resources.Count];
//            int[] name_offsets=new int[resources.Count];
//            int count=0;
			
//            IDictionaryEnumerator res_enum=resources.GetEnumerator();
//            while(res_enum.MoveNext()) {
//                /* Hash the name */
//                hashes[count]=GetHash((string)res_enum.Key);

//                /* Record the offsets */
//                name_offsets[count]=(int)res_name.BaseStream.Position;

//                /* Write the name section */
//                res_name.Write((string)res_enum.Key);
//                res_name.Write((int)res_data.BaseStream.Position);

//                if (res_enum.Value == null) {
//                    Write7BitEncodedInt (res_data, -1);
//                    count++;
//                    continue;
//                }
//                // implementation note: TypeByNameObject is
//                // not used in 1.x profile.
//                TypeByNameObject tbn = res_enum.Value as TypeByNameObject;
//                Type type = tbn != null ? null : res_enum.Value.GetType();
//                object typeObj = tbn != null ? (object) tbn.TypeName : type;

//                /* Keep a list of unique types */
//#if NET_2_0
//                // do not output predefined ones.
//                switch (type != null ? Type.GetTypeCode (type) : TypeCode.Empty) {
//                case TypeCode.Decimal:
//                case TypeCode.Single:
//                case TypeCode.Double:
//                case TypeCode.SByte:
//                case TypeCode.Int16:
//                case TypeCode.Int32:
//                case TypeCode.Int64:
//                case TypeCode.Byte:
//                case TypeCode.UInt16:
//                case TypeCode.UInt32:
//                case TypeCode.UInt64:
//                case TypeCode.DateTime:
//                case TypeCode.String:
//                    break;
//                default:
//                    if (type == typeof (TimeSpan))
//                        break;
//                    if (type == typeof (MemoryStream))
//                        break;
//                    if (type==typeof(byte[]))
//                        break;

//                    if (!types.Contains (typeObj))
//                        types.Add (typeObj);
//                    /* Write the data section */
//                    Write7BitEncodedInt(res_data, (int) PredefinedResourceType.FistCustom + types.IndexOf(typeObj));
//                    break;
//                }
//#else
//                if (!types.Contains (typeObj))
//                    types.Add (typeObj);
//                /* Write the data section */
//                Write7BitEncodedInt(res_data, types.IndexOf(type));
//#endif

//                /* Strangely, Char is serialized
//                 * rather than just written out
//                 */
//#if NET_2_0
//                if (tbn != null)
//                    res_data.Write((byte []) tbn.Value);
//                else if (type==typeof(Byte)) {
//                    res_data.Write((byte) PredefinedResourceType.Byte);
//                    res_data.Write((Byte)res_enum.Value);
//                } else if (type==typeof(Decimal)) {
//                    res_data.Write((byte) PredefinedResourceType.Decimal);
//                    res_data.Write((Decimal)res_enum.Value);
//                } else if (type==typeof(DateTime)) {
//                    res_data.Write((byte) PredefinedResourceType.DateTime);
//                    res_data.Write(((DateTime)res_enum.Value).Ticks);
//                } else if (type==typeof(Double)) {
//                    res_data.Write((byte) PredefinedResourceType.Double);
//                    res_data.Write((Double)res_enum.Value);
//                } else if (type==typeof(Int16)) {
//                    res_data.Write((byte) PredefinedResourceType.Int16);
//                    res_data.Write((Int16)res_enum.Value);
//                } else if (type==typeof(Int32)) {
//                    res_data.Write((byte) PredefinedResourceType.Int32);
//                    res_data.Write((Int32)res_enum.Value);
//                } else if (type==typeof(Int64)) {
//                    res_data.Write((byte) PredefinedResourceType.Int64);
//                    res_data.Write((Int64)res_enum.Value);
//                } else if (type==typeof(SByte)) {
//                    res_data.Write((byte) PredefinedResourceType.SByte);
//                    res_data.Write((SByte)res_enum.Value);
//                } else if (type==typeof(Single)) {
//                    res_data.Write((byte) PredefinedResourceType.Single);
//                    res_data.Write((Single)res_enum.Value);
//                } else if (type==typeof(String)) {
//                    res_data.Write((byte) PredefinedResourceType.String);
//                    res_data.Write((String)res_enum.Value);
//                } else if (type==typeof(TimeSpan)) {
//                    res_data.Write((byte) PredefinedResourceType.TimeSpan);
//                    res_data.Write(((TimeSpan)res_enum.Value).Ticks);
//                } else if (type==typeof(UInt16)) {
//                    res_data.Write((byte) PredefinedResourceType.UInt16);
//                    res_data.Write((UInt16)res_enum.Value);
//                } else if (type==typeof(UInt32)) {
//                    res_data.Write((byte) PredefinedResourceType.UInt32);
//                    res_data.Write((UInt32)res_enum.Value);
//                } else if (type==typeof(UInt64)) {
//                    res_data.Write((byte) PredefinedResourceType.UInt64);
//                    res_data.Write((UInt64)res_enum.Value);
//                } else if (type==typeof(byte[])) {
//                    res_data.Write((byte) PredefinedResourceType.ByteArray);
//                    byte [] data = (byte[])res_enum.Value;
//                    res_data.Write((uint) data.Length);
//                    res_data.Write(data, 0, data.Length);
//                } else if (type==typeof(MemoryStream)) {
//                    res_data.Write((byte) PredefinedResourceType.Stream);
//                    byte [] data = ((MemoryStream)res_enum.Value).ToArray ();
//                    res_data.Write((uint) data.Length);
//                    res_data.Write(data, 0, data.Length);
//                } else {
//                    /* non-intrinsic types are
//                     * serialized
//                     */
//                    formatter.Serialize(res_data.BaseStream, res_enum.Value);
//                }
//#else
//                if(type==typeof(Byte)) {
//                    res_data.Write((Byte)res_enum.Value);
//                } else if (type==typeof(Decimal)) {
//                    res_data.Write((Decimal)res_enum.Value);
//                } else if (type==typeof(DateTime)) {
//                    res_data.Write(((DateTime)res_enum.Value).Ticks);
//                } else if (type==typeof(Double)) {
//                    res_data.Write((Double)res_enum.Value);
//                } else if (type==typeof(Int16)) {
//                    res_data.Write((Int16)res_enum.Value);
//                } else if (type==typeof(Int32)) {
//                    res_data.Write((Int32)res_enum.Value);
//                } else if (type==typeof(Int64)) {
//                    res_data.Write((Int64)res_enum.Value);
//                } else if (type==typeof(SByte)) {
//                    res_data.Write((SByte)res_enum.Value);
//                } else if (type==typeof(Single)) {
//                    res_data.Write((Single)res_enum.Value);
//                } else if (type==typeof(String)) {
//                    res_data.Write((String)res_enum.Value);
//                } else if (type==typeof(TimeSpan)) {
//                    res_data.Write(((TimeSpan)res_enum.Value).Ticks);
//                } else if (type==typeof(UInt16)) {
//                    res_data.Write((UInt16)res_enum.Value);
//                } else if (type==typeof(UInt32)) {
//                    res_data.Write((UInt32)res_enum.Value);
//                } else if (type==typeof(UInt64)) {
//                    res_data.Write((UInt64)res_enum.Value);
//                } else {
//                    /* non-intrinsic types are
//                     * serialized
//                     */
//                    formatter.Serialize(res_data.BaseStream, res_enum.Value);
//                }
//#endif

//                count++;
//            }

//            /* Sort the hashes, keep the name offsets
//             * matching up
//             */
//            Array.Sort(hashes, name_offsets);
			
//            /* now do the ResourceReader header */

//#if NET_2_0
//            writer.Write(2);
//#else
//            writer.Write(1);
//#endif
//            writer.Write(resources.Count);
//            writer.Write(types.Count);

//            /* Write all of the unique types */
//            foreach(object type in types) {
//                if (type is Type)
//                    writer.Write(((Type) type).AssemblyQualifiedName);
//                else
//                    writer.Write((string) type);
//            }

//            /* Pad the next fields (hash values) on an 8
//             * byte boundary, using the letters "PAD"
//             */
//            int pad_align=(int)(writer.BaseStream.Position & 7);
//            int pad_chars=0;

//            if(pad_align!=0) {
//                pad_chars=8-pad_align;
//            }

//            for(int i=0; i<pad_chars; i++) {
//                writer.Write((byte)"PAD"[i%3]);
//            }

//            /* Write the hashes */
//            for(int i=0; i<resources.Count; i++) {
//                writer.Write(hashes[i]);
//            }

//            /* and the name offsets */
//            for(int i=0; i<resources.Count; i++) {
//                writer.Write(name_offsets[i]);
//            }

//            /* Write the data section offset */
//            int data_offset=(int)writer.BaseStream.Position +
//                (int)res_name_stream.Length + 4;
//            writer.Write(data_offset);

//            /* The name section goes next */
//            writer.Write(res_name_stream.GetBuffer(), 0,
//                     (int)res_name_stream.Length);
//            /* The data section is last */
//            writer.Write(res_data_stream.GetBuffer(), 0,
//                     (int)res_data_stream.Length);

//            res_name.Close();
//            res_data.Close();

//            /* Don't close writer, according to the spec */
//            writer.Flush();
            throw new NotImplementedException();
		}

		// looks like it is (similar to) DJB hash
		private int GetHash(string name)
		{
			uint hash=5381;

			for(int i=0; i<name.Length; i++) {
				hash=((hash<<5)+hash)^name[i];
			}
			
			return((int)hash);
		}

		/* Cut and pasted from BinaryWriter, because it's
		 * 'protected' there.
		 */
		private void Write7BitEncodedInt(BinaryWriter writer,
						 int value)
		{
			do {
				int high = (value >> 7) & 0x01ffffff;
				byte b = (byte)(value & 0x7f);

				if (high != 0) {
					b = (byte)(b | 0x80);
				}

				writer.Write(b);
				value = high;
			} while(value != 0);
		}

		internal Stream Stream {
			get {
				return stream;
			}
		}
	}
}
