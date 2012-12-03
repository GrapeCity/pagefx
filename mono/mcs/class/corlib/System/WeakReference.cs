//
// System.WeakReference.cs
//
// Author:
//   Ajay kumar Dwivedi (adwiv@yahoo.com)
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

#if NOT_PFX
using System.Runtime.Serialization;
#endif
using System.Runtime.InteropServices;

namespace System
{
	[Serializable]
#if NET_2_0
	[ComVisible (true)]
#endif
	public class WeakReference
#if NOT_PFX
        : ISerializable
#endif
	{
		//Fields
		private bool isLongReference;
		private GCHandle gcHandle;

		// Helper method for constructors
		//Should not be called from any other method.
		private void AllocateHandle (Object target)
		{
			if (isLongReference) {
				gcHandle = GCHandle.Alloc (target, GCHandleType.WeakTrackResurrection);
			}
			else {
				gcHandle = GCHandle.Alloc (target, GCHandleType.Weak);
			}
		}

		//Constructors
		public WeakReference (object target)
			: this (target, false)
		{
		}

		public WeakReference (object target, bool trackResurrection)
		{
			isLongReference = trackResurrection;
			AllocateHandle (target);
		}

#if NOT_PFX
protected WeakReference (SerializationInfo info, StreamingContext context)
		{
			if (info == null)
				throw new ArgumentNullException ("info");

			isLongReference = info.GetBoolean ("TrackResurrection");
			Object target = info.GetValue ("TrackedObject", typeof (System.Object));

			AllocateHandle (target);
		}
#endif

		// Properties
		public virtual bool IsAlive {
			get {
				//Target property takes care of the exception
				return (Target != null);		
			}
		}

		public virtual object Target {
			get {
				//Exception is thrown by gcHandle's Target
				return gcHandle.Target;
			}
			set
			{
				gcHandle.Target = value;
			}
		}

		public virtual bool TrackResurrection {
			get {
				return isLongReference;
			}
		}

		//Methods
		~WeakReference ()
		{
			gcHandle.Free ();
		}

#if NOT_PFX

		public virtual void GetObjectData (SerializationInfo info, StreamingContext context)
		{
			if (info == null)
				throw new ArgumentNullException ("info");

			info.AddValue ("TrackResurrection", TrackResurrection);

			try {
				info.AddValue ("TrackedObject", Target);
			} catch (Exception) {
				info.AddValue ("TrackedObject", null);
			}
		}
#endif
	}
}
