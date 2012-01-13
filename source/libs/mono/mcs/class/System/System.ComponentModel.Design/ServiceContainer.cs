//
// System.ComponentModel.Design.ServiceContainer.cs
//
// Authors:
//   Martin Willemoes Hansen (mwh@sysrq.dk)
//   Andreas Nahr (ClassDevelopment@A-SoftTech.com)
//   Ivan N. Zlatev (contact i-nZ.net)
//
// (C) 2003 Martin Willemoes Hansen
// (C) 2003 Andreas Nahr
// (C) 2006 Ivan N. Zlatev

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

namespace System.ComponentModel.Design
{

#if NET_2_0
	public class ServiceContainer : IServiceContainer, IServiceProvider, IDisposable
#else
	public sealed class ServiceContainer : IServiceContainer, IServiceProvider
#endif
	{

		private IServiceProvider parentProvider;
		private Hashtable services = new Hashtable ();
#if NET_2_0
		private bool _disposed = false;
#endif
		
		public ServiceContainer()
			: this (null)
		{
		}

		public ServiceContainer (IServiceProvider parentProvider)
		{
			this.parentProvider = parentProvider;
		}

		public void AddService (Type serviceType, object serviceInstance)
		{
			AddService (serviceType, serviceInstance, false);
		}

		public void AddService (Type serviceType, ServiceCreatorCallback callback)
		{
			AddService (serviceType, callback, false);
		}

#if NET_2_0
		public virtual
#else
		public
#endif
		void AddService (Type serviceType, 
					object serviceInstance,
					bool promote)
		{
			if (serviceType == null)
				throw new ArgumentNullException ("serviceType", "Cannot be null");
			if (promote)
				if (parentProvider != null)
					((IServiceContainer)parentProvider.GetService(typeof(IServiceContainer))).AddService (serviceType, serviceInstance, promote);
			if (services.Contains (serviceType)) {
					throw new ArgumentException (string.Format ("The service {0} already exists in the service container.", serviceType.ToString()));
			}
			services.Add (serviceType, serviceInstance);
		}

#if NET_2_0
		public virtual
#else
		public
#endif
		void AddService (Type serviceType,
					ServiceCreatorCallback callback,
					bool promote)
		{
			if (serviceType == null)
				throw new ArgumentNullException ("serviceType", "Cannot be null");
			if (promote)
				if (parentProvider != null)
					((IServiceContainer)parentProvider.GetService(typeof(IServiceContainer))).AddService (serviceType, callback, promote);
			if (services.Contains (serviceType)) {
					throw new ArgumentException (string.Format ("The service {0} already exists in the service container.", serviceType.ToString()));
			}
			services.Add (serviceType, callback);
		}

		public void RemoveService (Type serviceType)
		{
			RemoveService (serviceType, false);
		}
        
#if NET_2_0
		public virtual void RemoveService (Type serviceType, bool promote)
#else
        public void RemoveService (Type serviceType, bool promote)
#endif
		{
			if (serviceType == null)
				throw new ArgumentNullException ("serviceType", "Cannot be null");
			if (promote)
				if (parentProvider != null)
					((IServiceContainer)parentProvider.GetService(typeof(IServiceContainer))).RemoveService (serviceType, promote);
			else
				services.Remove (serviceType);
		}

#if NET_2_0
		public virtual
#else
		public
#endif
		object GetService (Type serviceType)
		{
#if NET_2_0
			object result = null;
			Type[] defaultServices = this.DefaultServices;
			for (int i=0; i < defaultServices.Length; i++) {
				if (defaultServices[i] == serviceType) {
					result = this;
					break;
				}
			}
			if (result == null)
				result = services[serviceType];
#else
			object result = services[serviceType];
#endif
			if (result == null && parentProvider != null)
				result = parentProvider.GetService (serviceType);
			if (result != null) {
				ServiceCreatorCallback	cb = result as ServiceCreatorCallback;
				if (cb != null) {
					result = cb (this, serviceType);
					services[serviceType] = result;
				}
				
			}
			return result;
		}

#if NET_2_0
		protected virtual Type [] DefaultServices {
			get { return new Type [] { typeof (IServiceContainer), typeof (ServiceContainer)}; }
		}

		public void Dispose ()
		{
			this.Dispose (true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose (bool disposing)
		{
			if (!_disposed) {
				if (disposing) {
					if (this.services != null) {
						foreach (object obj in this.services) {
							if (obj is IDisposable) {
								((IDisposable) obj).Dispose ();
							}
						}
						this.services = null;
					}
				}
				_disposed = true;
			}
		}
#endif
		
	}
}
