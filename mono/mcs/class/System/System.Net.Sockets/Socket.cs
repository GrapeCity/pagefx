// System.Net.Sockets.Socket.cs
//
// Authors:
//	Phillip Pearson (pp@myelin.co.nz)
//	Dick Porter <dick@ximian.com>
//	Gonzalo Paniagua Javier (gonzalo@ximian.com)
//	Sridhar Kulkarni (sridharkulkarni@gmail.com)
//	Brian Nickel (brian.nickel@gmail.com)
//
// Copyright (C) 2001, 2002 Phillip Pearson and Ximian, Inc.
//    http://www.myelin.co.nz
// (c) 2004-2006 Novell, Inc. (http://www.novell.com)
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
using System.Net;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using System.Reflection;
using System.IO;
using System.Net.Configuration;
using System.Text;

#if NET_2_0
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Timers;
#endif

namespace System.Net.Sockets 
{
	public class Socket : IDisposable
	{
		enum SocketOperation {
			Accept,
			Connect,
			Receive,
			ReceiveFrom,
			Send,
			SendTo,
			UsedInManaged1,
			UsedInManaged2,
			UsedInProcess,
			UsedInConsole2,
			Disconnect,
			AcceptReceive
		}

		[StructLayout (LayoutKind.Sequential)]
		private sealed class SocketAsyncResult: IAsyncResult
		{
			/* Same structure in the runtime */
			public Socket Sock;
			public IntPtr handle;
			object state;
			AsyncCallback callback;
			WaitHandle waithandle;

			Exception delayedException;

			public EndPoint EndPoint;	// Connect,ReceiveFrom,SendTo
			public byte [] Buffer;		// Receive,ReceiveFrom,Send,SendTo
			public int Offset;		// Receive,ReceiveFrom,Send,SendTo
			public int Size;		// Receive,ReceiveFrom,Send,SendTo
			public SocketFlags SockFlags;	// Receive,ReceiveFrom,Send,SendTo
			public Socket AcceptSocket;	// AcceptReceive
			public IPAddress[] Addresses;	// Connect
			public int Port;		// Connect
#if NET_2_0
			public IList<ArraySegment<byte>> Buffers;	// Receive, Send
#else
			public object Buffers;		// Reserve this slot in older profiles
#endif
			public bool ReuseSocket;	// Disconnect

			// Return values
			Socket acc_socket;
			int total;

			bool completed_sync;
			bool completed;
			public bool blocking;
			internal int error;
			SocketOperation operation;
			public object ares;

			public SocketAsyncResult (Socket sock, object state, AsyncCallback callback, SocketOperation operation)
			{
				this.Sock = sock;
				this.blocking = sock.blocking;
				this.handle = sock.socket;
				this.state = state;
				this.callback = callback;
				this.operation = operation;
				SockFlags = SocketFlags.None;
			}

			public void CheckIfThrowDelayedException ()
			{
				if (delayedException != null) {
					throw delayedException;
				}

				if (error != 0)
					throw new SocketException (error);
			}

			void CompleteAllOnDispose (Queue queue)
			{
				object [] pending = queue.ToArray ();
				queue.Clear ();

				WaitCallback cb;
				for (int i = 0; i < pending.Length; i++) {
					SocketAsyncResult ares = (SocketAsyncResult) pending [i];
					cb = new WaitCallback (ares.CompleteDisposed);
					ThreadPool.QueueUserWorkItem (cb, null);
				}
			}

			void CompleteDisposed (object unused)
			{
				Complete ();
			}

			public void Complete ()
			{
				if (operation != SocketOperation.Receive && Sock.disposed)
					delayedException = new ObjectDisposedException (Sock.GetType ().ToString ());

				IsCompleted = true;

				Queue queue = null;
				if (operation == SocketOperation.Receive || operation == SocketOperation.ReceiveFrom) {
					queue = Sock.readQ;
				} else if (operation == SocketOperation.Send || operation == SocketOperation.SendTo) {
					queue = Sock.writeQ;
				}

				if (queue != null) {
					SocketAsyncCall sac = null;
					SocketAsyncResult req = null;
					lock (queue) {
						queue.Dequeue (); // remove ourselves
						if (queue.Count > 0) {
							req = (SocketAsyncResult) queue.Peek ();
							if (!Sock.disposed) {
								Worker worker = new Worker (req);
								sac = GetDelegate (worker, req.operation);
							} else {
								CompleteAllOnDispose (queue);
							}
						}
					}

					if (sac != null)
						sac.BeginInvoke (null, req);
				}

				if (callback != null)
					callback (this);
			}

			SocketAsyncCall GetDelegate (Worker worker, SocketOperation op)
			{
				switch (op) {
				case SocketOperation.Receive:
					return new SocketAsyncCall (worker.Receive);
				case SocketOperation.ReceiveFrom:
					return new SocketAsyncCall (worker.ReceiveFrom);
				case SocketOperation.Send:
					return new SocketAsyncCall (worker.Send);
				case SocketOperation.SendTo:
					return new SocketAsyncCall (worker.SendTo);
				default:
					return null; // never happens
				}
			}

			public void Complete (bool synch)
			{
				completed_sync = synch;
				Complete ();
			}

			public void Complete (int total)
			{
				this.total = total;
				Complete ();
			}

			public void Complete (Exception e, bool synch)
			{
				completed_sync = synch;
				delayedException = e;
				Complete ();
			}

			public void Complete (Exception e)
			{
				delayedException = e;
				Complete ();
			}

			public void Complete (Socket s)
			{
				acc_socket = s;
				Complete ();
			}

			public void Complete (Socket s, int total)
			{
				acc_socket = s;
				this.total = total;
				Complete ();
			}

			public object AsyncState {
				get {
					return state;
				}
			}

			public WaitHandle AsyncWaitHandle {
				get {
					lock (this) {
						if (waithandle == null)
							waithandle = new ManualResetEvent (completed);
					}

					return waithandle;
				}
				set {
					waithandle=value;
				}
			}

			public bool CompletedSynchronously {
				get {
					return(completed_sync);
				}
			}

			public bool IsCompleted {
				get {
					return(completed);
				}
				set {
					completed=value;
					lock (this) {
						if (waithandle != null && value) {
							((ManualResetEvent) waithandle).Set ();
						}
					}
				}
			}
			
			public Socket Socket {
				get {
					return acc_socket;
				}
			}

			public int Total {
				get { return total; }
				set { total = value; }
			}

			public SocketError ErrorCode
			{
				get {
#if NET_2_0
					SocketException ex = delayedException as SocketException;
					
					if (ex != null)
						return(ex.SocketErrorCode);

					if (error != 0)
						return((SocketError)error);
#endif
					return(SocketError.Success);
				}
			}
		}

		private sealed class Worker 
		{
			SocketAsyncResult result;

			public Worker (SocketAsyncResult ares)
			{
				this.result = ares;
			}

			public void Accept ()
			{
				Socket acc_socket = null;
				try {
					acc_socket = result.Sock.Accept ();
				} catch (Exception e) {
					result.Complete (e);
					return;
				}

				result.Complete (acc_socket);
			}

			/* only used in 2.0 profile and newer, but
			 * leave in older profiles to keep interface
			 * to runtime consistent
			 */
			public void AcceptReceive ()
			{
				Socket acc_socket = null;
				
				try {
					if (result.AcceptSocket == null) {
						acc_socket = result.Sock.Accept ();
					} else {
						acc_socket = result.AcceptSocket;
						result.Sock.Accept (acc_socket);
					}
				} catch (Exception e) {
					result.Complete (e);
					return;
				}

				int total = 0;
				try {
					SocketError error;
					
					total = acc_socket.Receive_nochecks (result.Buffer,
									     result.Offset,
									     result.Size,
									     result.SockFlags,
									     out error);
				} catch (Exception e) {
					result.Complete (e);
					return;
				}

				result.Complete (acc_socket, total);
			}

			public void Connect ()
			{
				/* If result.EndPoint is non-null,
				 * this is the standard one-address
				 * connect attempt.  Otherwise
				 * Addresses must be non-null and
				 * contain a list of addresses to try
				 * to connect to; the first one to
				 * succeed causes the rest of the list
				 * to be ignored.
				 */
				if (result.EndPoint != null) {
					try {
						if (!result.Sock.Blocking) {
							int success;
							result.Sock.Poll (-1, SelectMode.SelectWrite, out success);
							if (success == 0) {
								result.Sock.connected = true;
							} else {
								result.Complete (new SocketException (success));
								return;
							}
						} else {
							result.Sock.seed_endpoint = result.EndPoint;
							result.Sock.Connect (result.EndPoint);
							result.Sock.connected = true;
						}
					} catch (Exception e) {
						result.Complete (e);
						return;
					}

					result.Complete ();
				} else if (result.Addresses != null) {
					foreach(IPAddress address in result.Addresses) {
						IPEndPoint iep = new IPEndPoint (address, result.Port);
						SocketAddress serial = iep.Serialize ();
						int error = 0;
						
						Socket.Connect_internal (result.Sock.socket, serial, out error);
						if (error == 0) {
							result.Sock.connected = true;
							result.Sock.seed_endpoint = iep;
							result.Complete ();
							return;
						} else if (error != (int)SocketError.InProgress &&
							   error != (int)SocketError.WouldBlock) {
							continue;
						}

						if (!result.Sock.Blocking) {
							int success;
							result.Sock.Poll (-1, SelectMode.SelectWrite, out success);
							if (success == 0) {
								result.Sock.connected = true;
								result.Sock.seed_endpoint = iep;
								result.Complete ();
								return;
							}
						}
					}
					
					result.Complete (new SocketException ((int)SocketError.InProgress));
				} else {
					result.Complete (new SocketException ((int)SocketError.AddressNotAvailable));
				}
			}

			/* Also only used in 2.0 profile and newer */
			public void Disconnect ()
			{
#if NET_2_0
				try {
					result.Sock.Disconnect (result.ReuseSocket);
				} catch (Exception e) {
					result.Complete (e);
					return;
				}
				result.Complete ();
#else
				result.Complete (new SocketException ((int)SocketError.Fault));
#endif
			}

			public void Receive ()
			{
				// Actual recv() done in the runtime
				result.Complete ();
			}

			public void ReceiveFrom ()
			{
				int total = 0;
				try {
					total = result.Sock.ReceiveFrom_nochecks (result.Buffer,
									 result.Offset,
									 result.Size,
									 result.SockFlags,
									 ref result.EndPoint);
				} catch (Exception e) {
					result.Complete (e);
					return;
				}

				result.Complete (total);
			}

			int send_so_far;

			void UpdateSendValues (int last_sent)
			{
				if (result.error == 0) {
					send_so_far += last_sent;
					result.Offset += last_sent;
					result.Size -= last_sent;
				}
			}

			public void Send ()
			{
				// Actual send() done in the runtime
				if (result.error == 0) {
					UpdateSendValues (result.Total);
					if (result.Sock.disposed) {
						result.Complete ();
						return;
					}

					if (result.Size > 0) {
						SocketAsyncCall sac = new SocketAsyncCall (this.Send);
						sac.BeginInvoke (null, result);
						return; // Have to finish writing everything. See bug #74475.
					}
					result.Total = send_so_far;
				}
				result.Complete ();
			}

			public void SendTo ()
			{
				int total = 0;
				try {
					total = result.Sock.SendTo_nochecks (result.Buffer,
								    result.Offset,
								    result.Size,
								    result.SockFlags,
								    result.EndPoint);

					UpdateSendValues (total);
					if (result.Size > 0) {
						SocketAsyncCall sac = new SocketAsyncCall (this.SendTo);
						sac.BeginInvoke (null, result);
						return; // Have to finish writing everything. See bug #74475.
					}
					result.Total = send_so_far;
				} catch (Exception e) {
					result.Complete (e);
					return;
				}

				result.Complete ();
			}
		}
			
		/* the field "socket" is looked up by name by the runtime */
		private IntPtr socket;
		private AddressFamily address_family;
		private SocketType socket_type;
		private ProtocolType protocol_type;
		internal bool blocking=true;
		private Queue readQ = new Queue (2);
		private Queue writeQ = new Queue (2);

		delegate void SocketAsyncCall ();
		/*
		 *	These two fields are looked up by name by the runtime, don't change
		 *  their name without also updating the runtime code.
		 */
		private static int ipv4Supported = -1, ipv6Supported = -1;

		/* When true, the socket was connected at the time of
		 * the last IO operation
		 */
		private bool connected;
		/* true if we called Close_internal */
		private bool closed;
		internal bool disposed;
		

		/*
		 * This EndPoint is used when creating new endpoints. Because
		 * there are many types of EndPoints possible,
		 * seed_endpoint.Create(addr) is used for creating new ones.
		 * As such, this value is set on Bind, SentTo, ReceiveFrom,
		 * Connect, etc.
 		 */
		private EndPoint seed_endpoint = null;

#if NET_2_0
		private bool isbound;
		private bool islistening;
		private bool useoverlappedIO;
#endif
		

		static void AddSockets (ArrayList sockets, IList list, string name)
		{
			if (list != null) {
				foreach (Socket sock in list) {
					if (sock == null) // MS throws a NullRef
						throw new ArgumentNullException ("name", "Contains a null element");
					sockets.Add (sock);
				}
			}

			sockets.Add (null);
		}
#if !TARGET_JVM
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static void Select_internal (ref Socket [] sockets,
							int microSeconds,
							out int error);
#endif
		public static void Select (IList checkRead, IList checkWrite, IList checkError, int microSeconds)
		{
			ArrayList list = new ArrayList ();
			AddSockets (list, checkRead, "checkRead");
			AddSockets (list, checkWrite, "checkWrite");
			AddSockets (list, checkError, "checkError");

			if (list.Count == 3) {
				throw new ArgumentNullException ("checkRead, checkWrite, checkError",
								 "All the lists are null or empty.");
			}

			int error;
			/*
			 * The 'sockets' array contains: READ socket 0-n, null,
			 * 				 WRITE socket 0-n, null,
			 *				 ERROR socket 0-n, null
			 */
			Socket [] sockets = (Socket []) list.ToArray (typeof (Socket));
			Select_internal (ref sockets, microSeconds, out error);

			if (error != 0)
				throw new SocketException (error);

			if (checkRead != null)
				checkRead.Clear ();

			if (checkWrite != null)
				checkWrite.Clear ();

			if (checkError != null)
				checkError.Clear ();

			if (sockets == null)
				return;

			int mode = 0;
			int count = sockets.Length;
			IList currentList = checkRead;
			for (int i = 0; i < count; i++) {
				Socket sock = sockets [i];
				if (sock == null) { // separator
					currentList = (mode == 0) ? checkWrite : checkError;
					mode++;
					continue;
				}

				if (currentList != null) {
					if (currentList == checkWrite && !sock.connected) {
						if ((int) sock.GetSocketOption (SocketOptionLevel.Socket, SocketOptionName.Error) == 0) {
							sock.connected = true;
						}
					}
					
					currentList.Add (sock);
				}
			}
		}

		// private constructor used by Accept, which already
		// has a socket handle to use
		private Socket(AddressFamily family, SocketType type,
			       ProtocolType proto, IntPtr sock)
		{
			address_family=family;
			socket_type=type;
			protocol_type=proto;
			
			socket=sock;
			connected=true;
		}
#if !TARGET_JVM
		// Creates a new system socket, returning the handle
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern IntPtr Socket_internal(AddressFamily family,
						      SocketType type,
						      ProtocolType proto,
						      out int error);
#endif		
		private void SocketDefaults ()
		{
#if NET_2_0
			try {
				if (address_family == AddressFamily.InterNetwork /* Need to test IPv6 further ||
										   address_family == AddressFamily.InterNetworkV6 */) {
					/* This is the default, but it
					 * probably has nasty side
					 * effects on Linux, as the
					 * socket option is kludged by
					 * turning on or off PMTU
					 * discovery...
					 */
					this.DontFragment = false;
				}

				//
				// Microsoft sets these to 8192, but we are going to keep them
				// both to the OS defaults as these have a big performance impact.
				// on WebClient performance.
				//
				//this.ReceiveBufferSize = 8192;
				//this.SendBufferSize = 8192;
			} catch (SocketException) {
			}
#endif
		}
		
		public Socket(AddressFamily family, SocketType type, ProtocolType proto)
		{
			address_family=family;
			socket_type=type;
			protocol_type=proto;
			
			int error;
			
			socket = Socket_internal (family, type, proto, out error);
			if (error != 0)
				throw new SocketException (error);

			SocketDefaults ();
		}

#if NET_2_0
		[MonoTODO]
		public Socket (SocketInformation socketInformation)
		{
			throw new NotImplementedException ("SocketInformation not figured out yet");

			// ifdef to avoid the warnings.
#if false
			//address_family = socketInformation.address_family;
			//socket_type = socketInformation.socket_type;
			//protocol_type = socketInformation.protocol_type;
			address_family = AddressFamily.InterNetwork;
			socket_type = SocketType.Stream;
			protocol_type = ProtocolType.IP;
			
			int error;
			socket = Socket_internal (address_family, socket_type, protocol_type, out error);
			if (error != 0)
				throw new SocketException (error);

			SocketDefaults ();
#endif
		}
#endif

		public AddressFamily AddressFamily {
			get {
				return(address_family);
			}
		}

#if !TARGET_JVM
		// Returns the amount of data waiting to be read on socket
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static int Available_internal(IntPtr socket, out int error);
#endif

		public int Available {
			get {
				if (disposed && closed)
					throw new ObjectDisposedException (GetType ().ToString ());

				int ret, error;
				
				ret = Available_internal(socket, out error);

				if (error != 0)
					throw new SocketException (error);

				return(ret);
			}
		}

#if !TARGET_JVM
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static void Blocking_internal(IntPtr socket,
							     bool block,
							     out int error);
#endif

		public bool Blocking {
			get {
				return(blocking);
			}
			set {
				if (disposed && closed)
					throw new ObjectDisposedException (GetType ().ToString ());

				int error;
				
				Blocking_internal (socket, value, out error);

				if (error != 0)
					throw new SocketException (error);
				
				blocking=value;
			}
		}

		public bool Connected {
			get {
				return(connected);
			}
		}

#if NET_2_0
		public bool DontFragment {
			get {
				if (disposed && closed) {
					throw new ObjectDisposedException (GetType ().ToString ());
				}

				bool dontfragment;
				
				if (address_family == AddressFamily.InterNetwork) {
					dontfragment = (int)(GetSocketOption (SocketOptionLevel.IP, SocketOptionName.DontFragment)) != 0;
				} else if (address_family == AddressFamily.InterNetworkV6) {
					dontfragment = (int)(GetSocketOption (SocketOptionLevel.IPv6, SocketOptionName.DontFragment)) != 0;
				} else {
					throw new NotSupportedException ("This property is only valid for InterNetwork and InterNetworkV6 sockets");
				}
				
				return(dontfragment);
			}
			set {
				if (disposed && closed) {
					throw new ObjectDisposedException (GetType ().ToString ());
				}

				if (address_family == AddressFamily.InterNetwork) {
					SetSocketOption (SocketOptionLevel.IP, SocketOptionName.DontFragment, value?1:0);
				} else if (address_family == AddressFamily.InterNetworkV6) {
					SetSocketOption (SocketOptionLevel.IPv6, SocketOptionName.DontFragment, value?1:0);
				} else {
					throw new NotSupportedException ("This property is only valid for InterNetwork and InterNetworkV6 sockets");
				}
			}
		}

		public bool EnableBroadcast {
			get {
				if (disposed && closed) {
					throw new ObjectDisposedException (GetType ().ToString ());
				}

				if (protocol_type != ProtocolType.Udp) {
					throw new SocketException ((int)SocketError.ProtocolOption);
				}
				
				return((int)(GetSocketOption (SocketOptionLevel.Socket, SocketOptionName.Broadcast)) != 0);
			}
			set {
				if (disposed && closed) {
					throw new ObjectDisposedException (GetType ().ToString ());
				}

				if (protocol_type != ProtocolType.Udp) {
					throw new SocketException ((int)SocketError.ProtocolOption);
				}

				SetSocketOption (SocketOptionLevel.Socket, SocketOptionName.Broadcast, value?1:0);
			}
		}
		
		public bool ExclusiveAddressUse {
			get {
				if (disposed && closed) {
					throw new ObjectDisposedException (GetType ().ToString ());
				}

				return((int)(GetSocketOption (SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse)) != 0);
			}
			set {
				if (disposed && closed) {
					throw new ObjectDisposedException (GetType ().ToString ());
				}
				if (isbound) {
					throw new InvalidOperationException ("Bind has already been called for this socket");
				}
				
				SetSocketOption (SocketOptionLevel.Socket, SocketOptionName.ExclusiveAddressUse, value?1:0);
			}
		}
		
		public bool IsBound {
			get {
				return(isbound);
			}
		}
		
		public LingerOption LingerState {
			get {
				if (disposed && closed) {
					throw new ObjectDisposedException (GetType ().ToString ());
				}

				return((LingerOption)GetSocketOption (SocketOptionLevel.Socket, SocketOptionName.Linger));
			}
			set {
				if (disposed && closed) {
					throw new ObjectDisposedException (GetType ().ToString ());
				}
				
				SetSocketOption (SocketOptionLevel.Socket,
						 SocketOptionName.Linger,
						 value);
			}
		}
		
		public bool MulticastLoopback {
			get {
				if (disposed && closed) {
					throw new ObjectDisposedException (GetType ().ToString ());
				}

				/* Even though this option can be set
				 * for TCP sockets on Linux, throw
				 * this exception anyway to be
				 * compatible (the MSDN docs say
				 * "Setting this property on a
				 * Transmission Control Protocol (TCP)
				 * socket will have no effect." but
				 * the MS runtime throws the
				 * exception...)
				 */
				if (protocol_type == ProtocolType.Tcp) {
					throw new SocketException ((int)SocketError.ProtocolOption);
				}
				
				bool multicastloopback;
				
				if (address_family == AddressFamily.InterNetwork) {
					multicastloopback = (int)(GetSocketOption (SocketOptionLevel.IP, SocketOptionName.MulticastLoopback)) != 0;
				} else if (address_family == AddressFamily.InterNetworkV6) {
					multicastloopback = (int)(GetSocketOption (SocketOptionLevel.IPv6, SocketOptionName.MulticastLoopback)) != 0;
				} else {
					throw new NotSupportedException ("This property is only valid for InterNetwork and InterNetworkV6 sockets");
				}
				
				return(multicastloopback);
			}
			set {
				if (disposed && closed) {
					throw new ObjectDisposedException (GetType ().ToString ());
				}

				/* Even though this option can be set
				 * for TCP sockets on Linux, throw
				 * this exception anyway to be
				 * compatible (the MSDN docs say
				 * "Setting this property on a
				 * Transmission Control Protocol (TCP)
				 * socket will have no effect." but
				 * the MS runtime throws the
				 * exception...)
				 */
				if (protocol_type == ProtocolType.Tcp) {
					throw new SocketException ((int)SocketError.ProtocolOption);
				}
				
				if (address_family == AddressFamily.InterNetwork) {
					SetSocketOption (SocketOptionLevel.IP, SocketOptionName.MulticastLoopback, value?1:0);
				} else if (address_family == AddressFamily.InterNetworkV6) {
					SetSocketOption (SocketOptionLevel.IPv6, SocketOptionName.MulticastLoopback, value?1:0);
				} else {
					throw new NotSupportedException ("This property is only valid for InterNetwork and InterNetworkV6 sockets");
				}
			}
		}
		
		public static bool OSSupportsIPv6 {
			get {
				NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces ();
				
				foreach(NetworkInterface adapter in nics) {
					if (adapter.Supports (NetworkInterfaceComponent.IPv6) == true) {
						return(true);
					} else {
						continue;
					}
				}
				return(false);
			}
		}
		
		public int ReceiveBufferSize {
			get {
				if (disposed && closed) {
					throw new ObjectDisposedException (GetType ().ToString ());
				}
				return((int)GetSocketOption (SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer));
			}
			set {
				if (disposed && closed) {
					throw new ObjectDisposedException (GetType ().ToString ());
				}
				if (value < 0) {
					throw new ArgumentOutOfRangeException ("value", "The value specified for a set operation is less than zero");
				}
				
				SetSocketOption (SocketOptionLevel.Socket, SocketOptionName.ReceiveBuffer, value);
			}
		}

		public int SendBufferSize {
			get {
				if (disposed && closed) {
					throw new ObjectDisposedException (GetType ().ToString ());
				}
				return((int)GetSocketOption (SocketOptionLevel.Socket, SocketOptionName.SendBuffer));
			}
			set {
				if (disposed && closed) {
					throw new ObjectDisposedException (GetType ().ToString ());
				}
				if (value < 0) {
					throw new ArgumentOutOfRangeException ("value", "The value specified for a set operation is less than zero");
				}
				
				SetSocketOption (SocketOptionLevel.Socket,
						 SocketOptionName.SendBuffer,
						 value);
			}
		}
		
		public short Ttl {
			get {
				if (disposed && closed) {
					throw new ObjectDisposedException (GetType ().ToString ());
				}
				
				short ttl_val;
				
				if (address_family == AddressFamily.InterNetwork) {
					ttl_val = (short)((int)GetSocketOption (SocketOptionLevel.IP, SocketOptionName.IpTimeToLive));
				} else if (address_family == AddressFamily.InterNetworkV6) {
					ttl_val = (short)((int)GetSocketOption (SocketOptionLevel.IPv6, SocketOptionName.HopLimit));
				} else {
					throw new NotSupportedException ("This property is only valid for InterNetwork and InterNetworkV6 sockets");
				}
				
				return(ttl_val);
			}
			set {
				if (disposed && closed) {
					throw new ObjectDisposedException (GetType ().ToString ());
				}
				
				if (address_family == AddressFamily.InterNetwork) {
					SetSocketOption (SocketOptionLevel.IP, SocketOptionName.IpTimeToLive, value);
				} else if (address_family == AddressFamily.InterNetworkV6) {
					SetSocketOption (SocketOptionLevel.IPv6, SocketOptionName.HopLimit, value);
				} else {
					throw new NotSupportedException ("This property is only valid for InterNetwork and InterNetworkV6 sockets");
				}
			}
		}
		
		[MonoTODO ("This doesn't do anything on Mono yet")]
		public bool UseOnlyOverlappedIO {
			get {
				return(useoverlappedIO);
			}
			set {
				useoverlappedIO = value;
			}
		}
#endif

		public IntPtr Handle {
			get {
				return(socket);
			}
		}

#if !TARGET_JVM
		// Returns the local endpoint details in addr and port
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static SocketAddress LocalEndPoint_internal(IntPtr socket, out int error);
#endif

		// Wish:  support non-IP endpoints.
		public EndPoint LocalEndPoint {
			get {
				if (disposed && closed)
					throw new ObjectDisposedException (GetType ().ToString ());
				
				/*
				 * If the seed EndPoint is null, Connect, Bind,
				 * etc has not yet been called. MS returns null
				 * in this case.
				 */
				if (seed_endpoint == null)
					return null;
				
				SocketAddress sa;
				int error;
				
				sa=LocalEndPoint_internal(socket, out error);

				if (error != 0)
					throw new SocketException (error);

				return seed_endpoint.Create (sa);
			}
		}

		public ProtocolType ProtocolType {
			get {
				return(protocol_type);
			}
		}

		// Returns the remote endpoint details in addr and port
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static SocketAddress RemoteEndPoint_internal(IntPtr socket, out int error);

		public EndPoint RemoteEndPoint {
			get {
				if (disposed && closed)
					throw new ObjectDisposedException (GetType ().ToString ());
				
				/*
				 * If the seed EndPoint is null, Connect, Bind,
				 * etc has not yet been called. MS returns null
				 * in this case.
				 */
				if (seed_endpoint == null)
					return null;
				
				SocketAddress sa;
				int error;
				
				sa=RemoteEndPoint_internal(socket, out error);

				if (error != 0)
					throw new SocketException (error);

				return seed_endpoint.Create (sa);
			}
		}

		public SocketType SocketType {
			get {
				return(socket_type);
			}
		}

		public static bool SupportsIPv4 {
			get {
				CheckProtocolSupport();
				return ipv4Supported == 1;
			}
		}

#if NET_2_0
		[ObsoleteAttribute ("Use OSSupportsIPv6 instead")]
#endif
		public static bool SupportsIPv6 {
			get {
				CheckProtocolSupport();
				return ipv6Supported == 1;
			}
		}

#if NET_2_0
		public int SendTimeout {
			get {
				if (disposed && closed)
					throw new ObjectDisposedException (GetType ().ToString ());

				return (int)GetSocketOption(
					SocketOptionLevel.Socket,
					SocketOptionName.SendTimeout);
			}
			set {
				if (disposed && closed)
					throw new ObjectDisposedException (GetType ().ToString ());

				if (value < -1)
					throw new ArgumentOutOfRangeException ("value", "The value specified for a set operation is less than -1");

				/* According to the MSDN docs we
				 * should adjust values between 1 and
				 * 499 to 500, but the MS runtime
				 * doesn't do this.
				 */
				if (value == -1)
					value = 0;

				SetSocketOption(
					SocketOptionLevel.Socket,
					SocketOptionName.SendTimeout, value);
			}
		}

		public int ReceiveTimeout {
			get {
				if (disposed && closed)
					throw new ObjectDisposedException (GetType ().ToString ());

				return (int)GetSocketOption(
					SocketOptionLevel.Socket,
					SocketOptionName.ReceiveTimeout);
			}
			set {
				if (disposed && closed)
					throw new ObjectDisposedException (GetType ().ToString ());

				if (value < -1)
					throw new ArgumentOutOfRangeException ("value", "The value specified for a set operation is less than -1");

				if (value == -1) {
					value = 0;
				}
				
				SetSocketOption(
					SocketOptionLevel.Socket,
					SocketOptionName.ReceiveTimeout, value);
			}
		}

		public bool NoDelay {
			get {
				if (disposed && closed)
					throw new ObjectDisposedException (GetType ().ToString ());

				if (protocol_type == ProtocolType.Udp)
					throw new SocketException ((int)SocketError.ProtocolOption);

				return (int)(GetSocketOption (
					SocketOptionLevel.Tcp,
					SocketOptionName.NoDelay)) != 0;
			}

			set {
				if (disposed && closed)
					throw new ObjectDisposedException (GetType ().ToString ());

				if (protocol_type == ProtocolType.Udp)
					throw new SocketException ((int)SocketError.ProtocolOption);

				SetSocketOption (
					SocketOptionLevel.Tcp,
					SocketOptionName.NoDelay, value ? 1 : 0);
			}
		}
#endif

		internal static void CheckProtocolSupport ()
		{
			if(ipv4Supported == -1) {
				try {
					Socket tmp = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
					tmp.Close();

					ipv4Supported = 1;
				} catch {
					ipv4Supported = 0;
				}
			}

			if (ipv6Supported == -1) {
#if NET_2_0 && CONFIGURATION_DEP
				SettingsSection config;
				config = (SettingsSection) System.Configuration.ConfigurationManager.GetSection ("system.net/settings");
				if (config != null)
					ipv6Supported = config.Ipv6.Enabled ? -1 : 0;
#else
				NetConfig config = (NetConfig)System.Configuration.ConfigurationSettings.GetConfig("system.net/settings");
				if (config != null)
					ipv6Supported = config.ipv6Enabled ? -1 : 0;
#endif
				if (ipv6Supported != 0) {
					try {
						Socket tmp = new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp);
						tmp.Close();

						ipv6Supported = 1;
					} catch { }
				}
			}
		}

		// Creates a new system socket, returning the handle
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static IntPtr Accept_internal(IntPtr sock, out int error);

		Thread blocking_thread;
		public Socket Accept() {
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			int error = 0;
			IntPtr sock = (IntPtr) (-1);
			blocking_thread = Thread.CurrentThread;
			try {
				sock = Accept_internal(socket, out error);
			} catch (ThreadAbortException) {
				if (disposed) {
					Thread.ResetAbort ();
					error = (int) SocketError.Interrupted;
				}
			} finally {
				blocking_thread = null;
			}

			if (error != 0)
				throw new SocketException (error);
			
			Socket accepted = new Socket(this.AddressFamily, this.SocketType,
				this.ProtocolType, sock);

			accepted.seed_endpoint = this.seed_endpoint;
			accepted.Blocking = this.Blocking;
			return(accepted);
		}

		private void Accept (Socket acceptSocket)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());
			
			int error = 0;
			IntPtr sock = (IntPtr)(-1);
			blocking_thread = Thread.CurrentThread;
			
			try {
				sock = Accept_internal (socket, out error);
			} catch (ThreadAbortException) {
				if (disposed) {
					Thread.ResetAbort ();
					error = (int)SocketError.Interrupted;
				}
			} finally {
				blocking_thread = null;
			}
			
			if (error != 0)
				throw new SocketException (error);
			
			acceptSocket.address_family = this.AddressFamily;
			acceptSocket.socket_type = this.SocketType;
			acceptSocket.protocol_type = this.ProtocolType;
			acceptSocket.socket = sock;
			acceptSocket.connected = true;
			acceptSocket.seed_endpoint = this.seed_endpoint;
			acceptSocket.Blocking = this.Blocking;

			/* FIXME: figure out what if anything else
			 * needs to be reset
			 */
		}

		public IAsyncResult BeginAccept(AsyncCallback callback,
						object state)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

#if NET_2_0
			/* FIXME: check the 1.1 docs for this too */
			if (!isbound || !islistening)
				throw new InvalidOperationException ();
#endif

			SocketAsyncResult req = new SocketAsyncResult (this, state, callback, SocketOperation.Accept);
			Worker worker = new Worker (req);
			SocketAsyncCall sac = new SocketAsyncCall (worker.Accept);
			sac.BeginInvoke (null, req);
			return(req);
		}

#if NET_2_0
		public IAsyncResult BeginAccept (int receiveSize,
						 AsyncCallback callback,
						 object state)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (receiveSize < 0)
				throw new ArgumentOutOfRangeException ("receiveSize", "receiveSize is less than zero");

			SocketAsyncResult req = new SocketAsyncResult (this, state, callback, SocketOperation.AcceptReceive);
			Worker worker = new Worker (req);
			SocketAsyncCall sac = new SocketAsyncCall (worker.AcceptReceive);
			
			req.Buffer = new byte[receiveSize];
			req.Offset = 0;
			req.Size = receiveSize;
			req.SockFlags = SocketFlags.None;

			sac.BeginInvoke (null, req);
			return(req);
		}

		public IAsyncResult BeginAccept (Socket acceptSocket,
						 int receiveSize,
						 AsyncCallback callback,
						 object state)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (receiveSize < 0)
				throw new ArgumentOutOfRangeException ("receiveSize", "receiveSize is less than zero");

			if (acceptSocket != null) {
				if (acceptSocket.disposed && acceptSocket.closed)
					throw new ObjectDisposedException (acceptSocket.GetType ().ToString ());

				if (acceptSocket.IsBound)
					throw new InvalidOperationException ();

				/* For some reason the MS runtime
				 * barfs if the new socket is not TCP,
				 * even though it's just about to blow
				 * away all those parameters
				 */
				if (acceptSocket.ProtocolType != ProtocolType.Tcp)
					throw new SocketException ((int)SocketError.InvalidArgument);
			}
			
			SocketAsyncResult req = new SocketAsyncResult (this, state, callback, SocketOperation.AcceptReceive);
			Worker worker = new Worker (req);
			SocketAsyncCall sac = new SocketAsyncCall (worker.AcceptReceive);
			
			req.Buffer = new byte[receiveSize];
			req.Offset = 0;
			req.Size = receiveSize;
			req.SockFlags = SocketFlags.None;
			req.AcceptSocket = acceptSocket;

			sac.BeginInvoke (null, req);
			return(req);
		}
#endif

		public IAsyncResult BeginConnect(EndPoint end_point,
						 AsyncCallback callback,
						 object state) {

			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (end_point == null)
				throw new ArgumentNullException ("end_point");

			SocketAsyncResult req = new SocketAsyncResult (this, state, callback, SocketOperation.Connect);
			req.EndPoint = end_point;

			// Bug #75154: Connect() should not succeed for .Any addresses.
			if (end_point is IPEndPoint) {
				IPEndPoint ep = (IPEndPoint) end_point;
				if (ep.Address.Equals (IPAddress.Any) || ep.Address.Equals (IPAddress.IPv6Any)) {
					req.Complete (new SocketException ((int) SocketError.AddressNotAvailable), true);
					return req;
				}
			}

			int error = 0;
			if (!blocking) {
				SocketAddress serial = end_point.Serialize ();
				Connect_internal (socket, serial, out error);
				if (error == 0) {
					// succeeded synch
					connected = true;
					req.Complete (true);
				} else if (error != (int) SocketError.InProgress && error != (int) SocketError.WouldBlock) {
					// error synch
					connected = false;
					req.Complete (new SocketException (error), true);
				}
			}

			if (blocking || error == (int) SocketError.InProgress || error == (int) SocketError.WouldBlock) {
				// continue asynch
				connected = false;
				Worker worker = new Worker (req);
				SocketAsyncCall sac = new SocketAsyncCall (worker.Connect);
				sac.BeginInvoke (null, req);
			}

			return(req);
		}

#if NET_2_0
		public IAsyncResult BeginConnect (IPAddress address, int port,
						  AsyncCallback callback,
						  object state)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (address == null)
				throw new ArgumentNullException ("address");

			if (address.ToString ().Length == 0)
				throw new ArgumentException ("The length of the IP address is zero");

			if (islistening)
				throw new InvalidOperationException ();

			IPEndPoint iep = new IPEndPoint (address, port);
			return(BeginConnect (iep, callback, state));
		}

		public IAsyncResult BeginConnect (IPAddress[] addresses,
						  int port,
						  AsyncCallback callback,
						  object state)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (addresses == null)
				throw new ArgumentNullException ("addresses");

			if (this.AddressFamily != AddressFamily.InterNetwork &&
				this.AddressFamily != AddressFamily.InterNetworkV6)
				throw new NotSupportedException ("This method is only valid for addresses in the InterNetwork or InterNetworkV6 families");

			if (islistening)
				throw new InvalidOperationException ();

			SocketAsyncResult req = new SocketAsyncResult (this, state, callback, SocketOperation.Connect);
			req.Addresses = addresses;
			req.Port = port;
			
			connected = false;
			Worker worker = new Worker (req);
			SocketAsyncCall sac = new SocketAsyncCall (worker.Connect);
			sac.BeginInvoke (null, req);
			
			return(req);
		}

		public IAsyncResult BeginConnect (string host, int port,
						  AsyncCallback callback,
						  object state)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (host == null)
				throw new ArgumentNullException ("host");

			if (address_family != AddressFamily.InterNetwork &&
				address_family != AddressFamily.InterNetworkV6)
				throw new NotSupportedException ("This method is valid only for sockets in the InterNetwork and InterNetworkV6 families");

			if (islistening)
				throw new InvalidOperationException ();

			IPHostEntry hostent = Dns.GetHostEntry (host);
			return (BeginConnect (hostent.AddressList, port, callback, state));
		}

		public IAsyncResult BeginDisconnect (bool reuseSocket,
						     AsyncCallback callback,
						     object state)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			SocketAsyncResult req = new SocketAsyncResult (this, state, callback, SocketOperation.Disconnect);
			req.ReuseSocket = reuseSocket;
			
			Worker worker = new Worker (req);
			SocketAsyncCall sac = new SocketAsyncCall (worker.Disconnect);
			sac.BeginInvoke (null, req);
			
			return(req);
		}
#endif
		
		public IAsyncResult BeginReceive(byte[] buffer, int offset,
						 int size,
						 SocketFlags socket_flags,
						 AsyncCallback callback,
						 object state) {

			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buffer == null)
				throw new ArgumentNullException ("buffer");

			if (offset < 0 || offset > buffer.Length)
				throw new ArgumentOutOfRangeException ("offset");

			if (size < 0 || offset + size > buffer.Length)
				throw new ArgumentOutOfRangeException ("size");

			SocketAsyncResult req;
			lock (readQ) {
				req = new SocketAsyncResult (this, state, callback, SocketOperation.Receive);
				req.Buffer = buffer;
				req.Offset = offset;
				req.Size = size;
				req.SockFlags = socket_flags;
				readQ.Enqueue (req);
				if (readQ.Count == 1) {
					Worker worker = new Worker (req);
					SocketAsyncCall sac = new SocketAsyncCall (worker.Receive);
					sac.BeginInvoke (null, req);
				}
			}

			return req;
		}
#if NET_2_0
		public IAsyncResult BeginReceive (byte[] buffer, int offset,
						  int size, SocketFlags flags,
						  out SocketError error,
						  AsyncCallback callback,
						  object state)
		{
			/* As far as I can tell from the docs and from
			 * experimentation, a pointer to the
			 * SocketError parameter is not supposed to be
			 * saved for the async parts.  And as we don't
			 * set any socket errors in the setup code, we
			 * just have to set it to Success.
			 */
			error = SocketError.Success;
			return (BeginReceive (buffer, offset, size, flags, callback, state));
		}

		[CLSCompliant (false)]
		public IAsyncResult BeginReceive (IList<ArraySegment<byte>> buffers,
						  SocketFlags socketFlags,
						  AsyncCallback callback,
						  object state)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buffers == null)
				throw new ArgumentNullException ("buffers");

			SocketAsyncResult req;
			lock(readQ) {
				req = new SocketAsyncResult (this, state, callback, SocketOperation.Receive);
				req.Buffers = buffers;
				req.SockFlags = socketFlags;
				readQ.Enqueue (req);
				if (readQ.Count == 1) {
					Worker worker = new Worker (req);
					SocketAsyncCall sac = new SocketAsyncCall (worker.Receive);
					sac.BeginInvoke (null, req);
				}
			}
			
			return(req);
		}
		
		[CLSCompliant (false)]
		public IAsyncResult BeginReceive (IList<ArraySegment<byte>> buffers,
						  SocketFlags socketFlags,
						  out SocketError errorCode,
						  AsyncCallback callback,
						  object state)
		{
			/* I assume the same SocketError semantics as
			 * above
			 */
			errorCode = SocketError.Success;
			return (BeginReceive (buffers, socketFlags, callback, state));
		}
#endif

		public IAsyncResult BeginReceiveFrom(byte[] buffer, int offset,
						     int size,
						     SocketFlags socket_flags,
						     ref EndPoint remote_end,
						     AsyncCallback callback,
						     object state) {
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buffer == null)
				throw new ArgumentNullException ("buffer");

			if (offset < 0)
				throw new ArgumentOutOfRangeException ("offset", "offset must be >= 0");

			if (size < 0)
				throw new ArgumentOutOfRangeException ("size", "size must be >= 0");

			if (offset + size > buffer.Length)
				throw new ArgumentOutOfRangeException ("offset, size", "offset + size exceeds the buffer length");

			SocketAsyncResult req;
			lock (readQ) {
				req = new SocketAsyncResult (this, state, callback, SocketOperation.ReceiveFrom);
				req.Buffer = buffer;
				req.Offset = offset;
				req.Size = size;
				req.SockFlags = socket_flags;
				req.EndPoint = remote_end;
				readQ.Enqueue (req);
				if (readQ.Count == 1) {
					Worker worker = new Worker (req);
					SocketAsyncCall sac = new SocketAsyncCall (worker.ReceiveFrom);
					sac.BeginInvoke (null, req);
				}
			}
			return req;
		}

#if NET_2_0
		[MonoTODO]
		public IAsyncResult BeginReceiveMessageFrom (
			byte[] buffer, int offset, int size,
			SocketFlags socketFlags, ref EndPoint remoteEP,
			AsyncCallback callback, object state)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buffer == null)
				throw new ArgumentNullException ("buffer");

			if (remoteEP == null)
				throw new ArgumentNullException ("remoteEP");

			if (offset < 0 || offset > buffer.Length)
				throw new ArgumentOutOfRangeException ("offset");

			if (size < 0 || offset + size > buffer.Length)
				throw new ArgumentOutOfRangeException ("size");

			throw new NotImplementedException ();
		}
#endif

		public IAsyncResult BeginSend (byte[] buffer, int offset, int size, SocketFlags socket_flags,
					       AsyncCallback callback, object state)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buffer == null)
				throw new ArgumentNullException ("buffer");

			if (offset < 0)
				throw new ArgumentOutOfRangeException ("offset", "offset must be >= 0");

			if (size < 0)
				throw new ArgumentOutOfRangeException ("size", "size must be >= 0");

			if (offset + size > buffer.Length)
				throw new ArgumentOutOfRangeException ("offset, size", "offset + size exceeds the buffer length");

#if NET_2_0
			/* TODO: Check this exception in the 1.1 profile */
			if (!connected)
				throw new SocketException ((int)SocketError.NotConnected);
#endif

			SocketAsyncResult req;
			lock (writeQ) {
				req = new SocketAsyncResult (this, state, callback, SocketOperation.Send);
				req.Buffer = buffer;
				req.Offset = offset;
				req.Size = size;
				req.SockFlags = socket_flags;
				writeQ.Enqueue (req);
				if (writeQ.Count == 1) {
					Worker worker = new Worker (req);
					SocketAsyncCall sac = new SocketAsyncCall (worker.Send);
					sac.BeginInvoke (null, req);
				}
			}
			return req;
		}

#if NET_2_0
		public IAsyncResult BeginSend (byte[] buffer, int offset,
					       int size,
					       SocketFlags socketFlags,
					       out SocketError errorCode,
					       AsyncCallback callback,
					       object state)
		{
			if (!connected) {
				errorCode = SocketError.NotConnected;
				throw new SocketException ((int)errorCode);
			}
			
			errorCode = SocketError.Success;
			
			return (BeginSend (buffer, offset, size, socketFlags, callback,
				state));
		}

		public IAsyncResult BeginSend (IList<ArraySegment<byte>> buffers,
					       SocketFlags socketFlags,
					       AsyncCallback callback,
					       object state)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buffers == null)
				throw new ArgumentNullException ("buffers");

			if (!connected)
				throw new SocketException ((int)SocketError.NotConnected);

			SocketAsyncResult req;
			lock (writeQ) {
				req = new SocketAsyncResult (this, state, callback, SocketOperation.Send);
				req.Buffers = buffers;
				req.SockFlags = socketFlags;
				writeQ.Enqueue (req);
				if (writeQ.Count == 1) {
					Worker worker = new Worker (req);
					SocketAsyncCall sac = new SocketAsyncCall (worker.Send);
					sac.BeginInvoke (null, req);
				}
			}
			
			return(req);
		}

		[CLSCompliant (false)]
		public IAsyncResult BeginSend (IList<ArraySegment<byte>> buffers,
					       SocketFlags socketFlags,
					       out SocketError errorCode,
					       AsyncCallback callback,
					       object state)
		{
			if (!connected) {
				errorCode = SocketError.NotConnected;
				throw new SocketException ((int)errorCode);
			}
			
			errorCode = SocketError.Success;
			return (BeginSend (buffers, socketFlags, callback, state));
		}

		[MonoTODO ("Not implemented")]
		public IAsyncResult BeginSendFile (string fileName,
						   AsyncCallback callback,
						   object state)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (!connected)
				throw new NotSupportedException ();

			if (!File.Exists (fileName))
				throw new FileNotFoundException ();

			throw new NotImplementedException ();
		}

		[MonoTODO ("Not implemented")]
		public IAsyncResult BeginSendFile (string fileName,
						   byte[] preBuffer,
						   byte[] postBuffer,
						   TransmitFileOptions flags,
						   AsyncCallback callback,
						   object state)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (!connected)
				throw new NotSupportedException ();

			if (!File.Exists (fileName))
				throw new FileNotFoundException ();

			throw new NotImplementedException ();
		}
#endif

		public IAsyncResult BeginSendTo(byte[] buffer, int offset,
						int size,
						SocketFlags socket_flags,
						EndPoint remote_end,
						AsyncCallback callback,
						object state) {
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buffer == null)
				throw new ArgumentNullException ("buffer");

			if (offset < 0)
				throw new ArgumentOutOfRangeException ("offset", "offset must be >= 0");

			if (size < 0)
				throw new ArgumentOutOfRangeException ("size", "size must be >= 0");

			if (offset + size > buffer.Length)
				throw new ArgumentOutOfRangeException ("offset, size", "offset + size exceeds the buffer length");

			SocketAsyncResult req;
			lock (writeQ) {
				req = new SocketAsyncResult (this, state, callback, SocketOperation.SendTo);
				req.Buffer = buffer;
				req.Offset = offset;
				req.Size = size;
				req.SockFlags = socket_flags;
				req.EndPoint = remote_end;
				writeQ.Enqueue (req);
				if (writeQ.Count == 1) {
					Worker worker = new Worker (req);
					SocketAsyncCall sac = new SocketAsyncCall (worker.SendTo);
					sac.BeginInvoke (null, req);
				}
			}
			return req;
		}

		// Creates a new system socket, returning the handle
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static void Bind_internal(IntPtr sock,
							 SocketAddress sa,
							 out int error);

		public void Bind(EndPoint local_end) {
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (local_end == null)
				throw new ArgumentNullException("local_end");
			
			int error;
			
			Bind_internal(socket, local_end.Serialize(), out error);
			if (error != 0)
				throw new SocketException (error);
#if NET_2_0
			if (error == 0)
				isbound = true;
#endif
			
			seed_endpoint = local_end;
		}

		// Closes the socket
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static void Close_internal(IntPtr socket, out int error);
		
		public void Close()
		{
			((IDisposable) this).Dispose ();
		}

#if NET_2_0
		public void Close (int timeout) 
		{
			System.Timers.Timer close_timer = new System.Timers.Timer ();
			close_timer.Elapsed += new ElapsedEventHandler (OnTimeoutClose);
			close_timer.Interval = timeout * 1000;
			close_timer.AutoReset = false;
			close_timer.Enabled = true;
		}

		private void OnTimeoutClose (object source, ElapsedEventArgs e)
		{
			this.Close ();
		}
#endif

		// Connects to the remote address
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static void Connect_internal(IntPtr sock,
							    SocketAddress sa,
							    out int error);

		public void Connect(EndPoint remote_end) {
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (remote_end == null)
				throw new ArgumentNullException("remote_end");

			if (remote_end is IPEndPoint) {
				IPEndPoint ep = (IPEndPoint) remote_end;
				if (ep.Address.Equals (IPAddress.Any) || ep.Address.Equals (IPAddress.IPv6Any))
					throw new SocketException ((int) SocketError.AddressNotAvailable);
			}

#if NET_2_0
			/* TODO: check this for the 1.1 profile too */
			if (islistening)
				throw new InvalidOperationException ();
#endif

			SocketAddress serial = remote_end.Serialize ();
			int error = 0;

			blocking_thread = Thread.CurrentThread;
			try {
				Connect_internal (socket, serial, out error);
			} catch (ThreadAbortException) {
				if (disposed) {
					Thread.ResetAbort ();
					error = (int) SocketError.Interrupted;
				}
			} finally {
				blocking_thread = null;
			}

			if (error != 0)
				throw new SocketException (error);

			connected=true;

#if NET_2_0
			isbound = true;
#endif
			
			seed_endpoint = remote_end;
		}

#if NET_2_0
		public void Connect (IPAddress address, int port)
		{
			Connect (new IPEndPoint (address, port));
		}
		
		public void Connect (IPAddress[] addresses, int port)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (addresses == null)
				throw new ArgumentNullException ("addresses");

			if (this.AddressFamily != AddressFamily.InterNetwork &&
				this.AddressFamily != AddressFamily.InterNetworkV6)
				throw new NotSupportedException ("This method is only valid for addresses in the InterNetwork or InterNetworkV6 families");

			if (islistening)
				throw new InvalidOperationException ();

			/* FIXME: do non-blocking sockets Poll here? */
			foreach (IPAddress address in addresses) {
				IPEndPoint iep = new IPEndPoint (address,
								 port);
				SocketAddress serial = iep.Serialize ();
				int error = 0;
				
				Connect_internal (socket, serial, out error);
				if (error == 0) {
					connected = true;
					seed_endpoint = iep;
					return;
				} else if (error != (int)SocketError.InProgress &&
					   error != (int)SocketError.WouldBlock) {
					continue;
				}
				
				if (!blocking) {
					Poll (-1, SelectMode.SelectWrite);
					int success = (int)GetSocketOption (SocketOptionLevel.Socket, SocketOptionName.Error);
					if (success == 0) {
						connected = true;
						seed_endpoint = iep;
						return;
					}
				}
			}
		}

		public void Connect (string host, int port)
		{
			IPHostEntry hostent = Dns.GetHostEntry (host);
			Connect (hostent.AddressList, port);
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static void Disconnect_internal(IntPtr sock,
							       bool reuse,
							       out int error);

		/* According to the docs, the MS runtime will throw
		 * PlatformNotSupportedException if the platform is
		 * newer than w2k.  We should be able to cope...
		 */
		public void Disconnect (bool reuseSocket)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			int error = 0;
			
			Disconnect_internal (socket, reuseSocket, out error);

			if (error != 0) {
				if (error == 50) {
					/* ERROR_NOT_SUPPORTED */
					throw new PlatformNotSupportedException ();
				} else {
					throw new SocketException (error);
				}
			}

			connected = false;
			
			if (reuseSocket) {
				/* Do managed housekeeping here... */
			}
		}

		[MonoTODO ("Not implemented")]
		public SocketInformation DuplicateAndClose (int targetProcessId)
		{
			/* Need to serialize this socket into a
			 * SocketInformation struct, but must study
			 * the MS implementation harder to figure out
			 * behaviour as documentation is lacking
			 */
			throw new NotImplementedException ();
		}
#endif
		
		public Socket EndAccept (IAsyncResult result)
		{
			int bytes;
			byte[] buffer;
			
			return(EndAccept (out buffer, out bytes, result));
		}

#if NET_2_0
		public Socket EndAccept (out byte[] buffer,
					 IAsyncResult asyncResult)
		{
			int bytes;
			
			return(EndAccept (out buffer, out bytes, asyncResult));
		}
#endif

#if NET_2_0
		public
#else
		private
#endif
		Socket EndAccept (out byte[] buffer, out int bytesTransferred,
				  IAsyncResult asyncResult)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (asyncResult == null)
				throw new ArgumentNullException ("asyncResult");
			
			SocketAsyncResult req = asyncResult as SocketAsyncResult;
			if (req == null)
				throw new ArgumentException ("Invalid IAsyncResult", "asyncResult");

			if (!asyncResult.IsCompleted)
				asyncResult.AsyncWaitHandle.WaitOne ();

			req.CheckIfThrowDelayedException ();
			
			buffer = req.Buffer;
			bytesTransferred = req.Total;
			
			return(req.Socket);
		}

		public void EndConnect (IAsyncResult result)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (result == null)
				throw new ArgumentNullException ("result");

			SocketAsyncResult req = result as SocketAsyncResult;
			if (req == null)
				throw new ArgumentException ("Invalid IAsyncResult", "result");

			if (!result.IsCompleted)
				result.AsyncWaitHandle.WaitOne();

			req.CheckIfThrowDelayedException();
		}

#if NET_2_0
		public void EndDisconnect (IAsyncResult asyncResult)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (asyncResult == null)
				throw new ArgumentNullException ("asyncResult");

			SocketAsyncResult req = asyncResult as SocketAsyncResult;
			if (req == null)
				throw new ArgumentException ("Invalid IAsyncResult", "asyncResult");

			if (!asyncResult.IsCompleted)
				asyncResult.AsyncWaitHandle.WaitOne ();

			req.CheckIfThrowDelayedException ();
		}
#endif

		public int EndReceive (IAsyncResult result)
		{
			SocketError error;
			
			return (EndReceive (result, out error));
		}

#if NET_2_0
		public
#else
		private
#endif
		int EndReceive (IAsyncResult asyncResult, out SocketError errorCode)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (asyncResult == null)
				throw new ArgumentNullException ("asyncResult");

			SocketAsyncResult req = asyncResult as SocketAsyncResult;
			if (req == null)
				throw new ArgumentException ("Invalid IAsyncResult", "asyncResult");

			if (!asyncResult.IsCompleted)
				asyncResult.AsyncWaitHandle.WaitOne ();

			errorCode = req.ErrorCode;
			req.CheckIfThrowDelayedException ();
			
			return(req.Total);
		}

		public int EndReceiveFrom(IAsyncResult result, ref EndPoint end_point)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (result == null)
				throw new ArgumentNullException ("result");

			SocketAsyncResult req = result as SocketAsyncResult;
			if (req == null)
				throw new ArgumentException ("Invalid IAsyncResult", "result");

			if (!result.IsCompleted)
				result.AsyncWaitHandle.WaitOne();

 			req.CheckIfThrowDelayedException();
			end_point = req.EndPoint;
			return req.Total;
		}

#if NET_2_0
		[MonoTODO]
		public int EndReceiveMessageFrom (IAsyncResult asyncResult,
						  ref SocketFlags socketFlags,
						  ref EndPoint endPoint,
						  out IPPacketInformation ipPacketInformation)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (asyncResult == null)
				throw new ArgumentNullException ("asyncResult");

			if (endPoint == null)
				throw new ArgumentNullException ("endPoint");

			SocketAsyncResult req = asyncResult as SocketAsyncResult;
			if (req == null)
				throw new ArgumentException ("Invalid IAsyncResult", "asyncResult");

			throw new NotImplementedException ();
		}
#endif

		public int EndSend (IAsyncResult result)
		{
			SocketError error;
			
			return(EndSend (result, out error));
		}

#if NET_2_0
		public
#else
		private
#endif
		int EndSend (IAsyncResult asyncResult, out SocketError errorCode)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (asyncResult == null)
				throw new ArgumentNullException ("asyncResult");
			
			SocketAsyncResult req = asyncResult as SocketAsyncResult;
			if (req == null)
				throw new ArgumentException ("Invalid IAsyncResult", "result");

			if (!asyncResult.IsCompleted)
				asyncResult.AsyncWaitHandle.WaitOne ();

			errorCode = req.ErrorCode;
			req.CheckIfThrowDelayedException ();
			
			return(req.Total);
		}

#if NET_2_0
		[MonoTODO]
		public void EndSendFile (IAsyncResult asyncResult)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (asyncResult == null)
				throw new ArgumentNullException ("asyncResult");

			SocketAsyncResult req = asyncResult as SocketAsyncResult;
			if (req == null)
				throw new ArgumentException ("Invalid IAsyncResult", "asyncResult");

			throw new NotImplementedException ();
		}
#endif

		public int EndSendTo (IAsyncResult result)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (result == null)
				throw new ArgumentNullException ("result");

			SocketAsyncResult req = result as SocketAsyncResult;
			if (req == null)
				throw new ArgumentException ("Invalid IAsyncResult", "result");

			if (!result.IsCompleted)
				result.AsyncWaitHandle.WaitOne();

			req.CheckIfThrowDelayedException();
			return req.Total;
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static void GetSocketOption_obj_internal(IntPtr socket,
			SocketOptionLevel level, SocketOptionName name, out object obj_val,
			out int error);

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static void GetSocketOption_arr_internal(IntPtr socket,
			SocketOptionLevel level, SocketOptionName name, ref byte[] byte_val,
			out int error);

		public object GetSocketOption (SocketOptionLevel level, SocketOptionName name)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			object obj_val;
			int error;
			
			GetSocketOption_obj_internal(socket, level, name, out obj_val,
				out error);
			if (error != 0)
				throw new SocketException (error);
			
			if (name == SocketOptionName.Linger) {
				return((LingerOption)obj_val);
			} else if (name==SocketOptionName.AddMembership ||
				   name==SocketOptionName.DropMembership) {
				return((MulticastOption)obj_val);
			} else if (obj_val is int) {
				return((int)obj_val);
			} else {
				return(obj_val);
			}
		}

		public void GetSocketOption (SocketOptionLevel level, SocketOptionName name, byte [] opt_value)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			int error;
			
			GetSocketOption_arr_internal(socket, level, name, ref opt_value,
				out error);
			if (error != 0)
				throw new SocketException (error);
		}

		public byte [] GetSocketOption (SocketOptionLevel level, SocketOptionName name, int length)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			byte[] byte_val=new byte[length];
			int error;
			
			GetSocketOption_arr_internal(socket, level, name, ref byte_val,
				out error);
			if (error != 0)
				throw new SocketException (error);

			return(byte_val);
		}

		// See Socket.IOControl, WSAIoctl documentation in MSDN. The
		// common options between UNIX and Winsock are FIONREAD,
		// FIONBIO and SIOCATMARK. Anything else will depend on the
		// system.
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern static int WSAIoctl (IntPtr sock, int ioctl_code, byte [] input,
			byte [] output, out int error);

		public int IOControl (int ioctl_code, byte [] in_value, byte [] out_value)
		{
			if (disposed)
				throw new ObjectDisposedException (GetType ().ToString ());

			int error;
			int result = WSAIoctl (socket, ioctl_code, in_value, out_value,
				out error);

			if (error != 0)
				throw new SocketException (error);
			
			if (result == -1)
				throw new InvalidOperationException ("Must use Blocking property instead.");

			return result;
		}

#if NET_2_0
		[MonoTODO]
		public int IOControl (IOControlCode ioControlCode,
				      byte[] optionInValue,
				      byte[] optionOutValue)
		{
			/* Probably just needs to mirror the int
			 * overload, but more investigation needed.
			 */
			throw new NotImplementedException ();
		}
#endif

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static void Listen_internal(IntPtr sock, int backlog,
			out int error);

		public void Listen (int backlog)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

#if NET_2_0
			/* TODO: check if this should be thrown in the
			 * 1.1 profile too
			 */
			if (!isbound)
				throw new SocketException ((int)SocketError.InvalidArgument);
#endif

			int error;
			
			Listen_internal(socket, backlog, out error);

			if (error != 0)
				throw new SocketException (error);

#if NET_2_0
			islistening = true;
#endif
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		extern static bool Poll_internal (IntPtr socket, SelectMode mode, int timeout, out int error);

		public bool Poll (int time_us, SelectMode mode)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (mode != SelectMode.SelectRead &&
			    mode != SelectMode.SelectWrite &&
			    mode != SelectMode.SelectError)
				throw new NotSupportedException ("'mode' parameter is not valid.");

			int error;
			bool result = Poll_internal (socket, mode, time_us, out error);
			if (error != 0)
				throw new SocketException (error);

			if (mode == SelectMode.SelectWrite && result && !connected) {
				/* Update the connected state; for
				 * non-blocking Connect()s this is
				 * when we can find out that the
				 * connect succeeded.
				 */
				if ((int)GetSocketOption (SocketOptionLevel.Socket, SocketOptionName.Error) == 0) {
					connected = true;
				}
			}
			
			return result;
		}

		/* This overload is needed as the async Connect method
		 * also needs to check the socket error status, but
		 * getsockopt(..., SO_ERROR) clears the error.
		 */
		private bool Poll (int time_us, SelectMode mode, out int socket_error)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (mode != SelectMode.SelectRead &&
			    mode != SelectMode.SelectWrite &&
			    mode != SelectMode.SelectError)
				throw new NotSupportedException ("'mode' parameter is not valid.");

			int error;
			bool result = Poll_internal (socket, mode, time_us, out error);
			if (error != 0)
				throw new SocketException (error);

			socket_error = (int)GetSocketOption (SocketOptionLevel.Socket, SocketOptionName.Error);
			
			if (mode == SelectMode.SelectWrite && result) {
				/* Update the connected state; for
				 * non-blocking Connect()s this is
				 * when we can find out that the
				 * connect succeeded.
				 */
				if (socket_error == 0) {
					connected = true;
				}
			}
			
			return result;
		}
		
		public int Receive (byte [] buf)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buf == null)
				throw new ArgumentNullException ("buf");

			SocketError error;

			int ret = Receive_nochecks (buf, 0, buf.Length, SocketFlags.None, out error);
			
			if (error != SocketError.Success)
				throw new SocketException ((int) error);

			return ret;
		}

		public int Receive (byte [] buf, SocketFlags flags)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buf == null)
				throw new ArgumentNullException ("buf");

			SocketError error;

			int ret = Receive_nochecks (buf, 0, buf.Length, flags, out error);
			
			if (error != SocketError.Success) {
				if (error == SocketError.WouldBlock && blocking) // This might happen when ReceiveTimeout is set
					throw new SocketException ((int) error, "Operation timed out.");
				throw new SocketException ((int) error);
			}

			return ret;
		}

		public int Receive (byte [] buf, int size, SocketFlags flags)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buf == null)
				throw new ArgumentNullException ("buf");

			if (size < 0 || size > buf.Length)
				throw new ArgumentOutOfRangeException ("size");

			SocketError error;

			int ret = Receive_nochecks (buf, 0, size, flags, out error);
			
			if (error != SocketError.Success) {
				if (error == SocketError.WouldBlock && blocking) // This might happen when ReceiveTimeout is set
					throw new SocketException ((int) error, "Operation timed out.");
				throw new SocketException ((int) error);
			}

			return ret;
		}

		public int Receive (byte [] buf, int offset, int size, SocketFlags flags)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buf == null)
				throw new ArgumentNullException ("buf");

			if (offset < 0 || offset > buf.Length)
				throw new ArgumentOutOfRangeException ("offset");

			if (size < 0 || offset + size > buf.Length)
				throw new ArgumentOutOfRangeException ("size");
			
			SocketError error;

			int ret = Receive_nochecks (buf, offset, size, flags, out error);
			
			if (error != SocketError.Success) {
				if (error == SocketError.WouldBlock && blocking) // This might happen when ReceiveTimeout is set
					throw new SocketException ((int) error, "Operation timed out.");
				throw new SocketException ((int) error);
			}

			return ret;
		}

#if NET_2_0
		public int Receive (byte [] buf, int offset, int size, SocketFlags flags, out SocketError error)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buf == null)
				throw new ArgumentNullException ("buf");

			if (offset < 0 || offset > buf.Length)
				throw new ArgumentOutOfRangeException ("offset");

			if (size < 0 || offset + size > buf.Length)
				throw new ArgumentOutOfRangeException ("size");
			
			return Receive_nochecks (buf, offset, size, flags, out error);
		}

		[MonoTODO]
		public int Receive (IList<ArraySegment<byte>> buffers)
		{
			/* For these generic IList overloads I need to
			 * implement WSARecv in the runtime
			 */
			throw new NotImplementedException ();
		}
		
		[CLSCompliant (false)]
		[MonoTODO]
		public int Receive (IList<ArraySegment<byte>> buffers,
				    SocketFlags socketFlags)
		{
			throw new NotImplementedException ();
		}

		[CLSCompliant (false)]
		[MonoTODO]
		public int Receive (IList<ArraySegment<byte>> buffers,
				    SocketFlags socketFlags,
				    out SocketError errorCode)
		{
			throw new NotImplementedException ();
		}
#endif

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static int Receive_internal(IntPtr sock,
							   byte[] buffer,
							   int offset,
							   int count,
							   SocketFlags flags,
							   out int error);

		int Receive_nochecks (byte [] buf, int offset, int size, SocketFlags flags, out SocketError error)
		{
			int nativeError;
			int ret = Receive_internal (socket, buf, offset, size, flags, out nativeError);
			error = (SocketError) nativeError;
			if (error != SocketError.Success && error != SocketError.WouldBlock && error != SocketError.InProgress)
				connected = false;
			else
				connected = true;
			
			return ret;
		}
		
		public int ReceiveFrom (byte [] buf, ref EndPoint remote_end)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buf == null)
				throw new ArgumentNullException ("buf");

			if (remote_end == null)
				throw new ArgumentNullException ("remote_end");

			return ReceiveFrom_nochecks (buf, 0, buf.Length, SocketFlags.None, ref remote_end);
		}

		public int ReceiveFrom (byte [] buf, SocketFlags flags, ref EndPoint remote_end)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buf == null)
				throw new ArgumentNullException ("buf");

			if (remote_end == null)
				throw new ArgumentNullException ("remote_end");

			return ReceiveFrom_nochecks (buf, 0, buf.Length, flags, ref remote_end);
		}

		public int ReceiveFrom (byte [] buf, int size, SocketFlags flags,
					ref EndPoint remote_end)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buf == null)
				throw new ArgumentNullException ("buf");

			if (remote_end == null)
				throw new ArgumentNullException ("remote_end");

			if (size < 0 || size > buf.Length)
				throw new ArgumentOutOfRangeException ("size");

			return ReceiveFrom_nochecks (buf, 0, size, flags, ref remote_end);
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static int RecvFrom_internal(IntPtr sock,
							    byte[] buffer,
							    int offset,
							    int count,
							    SocketFlags flags,
							    ref SocketAddress sockaddr,
							    out int error);

		public int ReceiveFrom (byte [] buf, int offset, int size, SocketFlags flags,
					ref EndPoint remote_end)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buf == null)
				throw new ArgumentNullException ("buf");

			if (remote_end == null)
				throw new ArgumentNullException ("remote_end");

			if (offset < 0 || offset > buf.Length)
				throw new ArgumentOutOfRangeException ("offset");

			if (size < 0 || offset + size > buf.Length)
				throw new ArgumentOutOfRangeException ("size");

			return ReceiveFrom_nochecks (buf, offset, size, flags, ref remote_end);
		}

		int ReceiveFrom_nochecks (byte [] buf, int offset, int size, SocketFlags flags,
					  ref EndPoint remote_end)
		{
			SocketAddress sockaddr = remote_end.Serialize();
			int cnt, error;

			cnt = RecvFrom_internal (socket, buf, offset, size, flags, ref sockaddr, out error);

			SocketError err = (SocketError) error;
			if (err != 0) {
				if (err != SocketError.WouldBlock && err != SocketError.InProgress)
					connected = false;
				else if (err == SocketError.WouldBlock && blocking) // This might happen when ReceiveTimeout is set
					throw new SocketException (error, "Operation timed out.");

				throw new SocketException (error);
			}

			connected = true;

#if NET_2_0
			isbound = true;
#endif

			// If sockaddr is null then we're a connection
			// oriented protocol and should ignore the
			// remote_end parameter (see MSDN
			// documentation for Socket.ReceiveFrom(...) )
			
			if ( sockaddr != null ) {
				// Stupidly, EndPoint.Create() is an
				// instance method
				remote_end = remote_end.Create (sockaddr);
			}
			
			seed_endpoint = remote_end;
			
			return cnt;
		}

#if NET_2_0
		[MonoTODO ("Not implemented")]
		public int ReceiveMessageFrom (byte[] buffer, int offset,
					       int size,
					       ref SocketFlags socketFlags,
					       ref EndPoint remoteEP,
					       out IPPacketInformation ipPacketInformation)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buffer == null)
				throw new ArgumentNullException ("buffer");

			if (remoteEP == null)
				throw new ArgumentNullException ("remoteEP");

			if (offset < 0 || offset > buffer.Length)
				throw new ArgumentOutOfRangeException ("offset");

			if (size < 0 || offset + size > buffer.Length)
				throw new ArgumentOutOfRangeException ("size");

			/* FIXME: figure out how we get hold of the
			 * IPPacketInformation
			 */
			throw new NotImplementedException ();
		}
#endif

		public int Send (byte [] buf)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buf == null)
				throw new ArgumentNullException ("buf");

			SocketError error;

			int ret = Send_nochecks (buf, 0, buf.Length, SocketFlags.None, out error);

			if (error != SocketError.Success)
				throw new SocketException ((int) error);

			return ret;
		}

		public int Send (byte [] buf, SocketFlags flags)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buf == null)
				throw new ArgumentNullException ("buf");

			SocketError error;

			int ret = Send_nochecks (buf, 0, buf.Length, flags, out error);

			if (error != SocketError.Success)
				throw new SocketException ((int) error);

			return ret;
		}

		public int Send (byte [] buf, int size, SocketFlags flags)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buf == null)
				throw new ArgumentNullException ("buf");

			if (size < 0 || size > buf.Length)
				throw new ArgumentOutOfRangeException ("size");

			SocketError error;

			int ret = Send_nochecks (buf, 0, size, flags, out error);

			if (error != SocketError.Success)
				throw new SocketException ((int) error);

			return ret;
		}

		public int Send (byte [] buf, int offset, int size, SocketFlags flags)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buf == null)
				throw new ArgumentNullException ("buffer");

			if (offset < 0 || offset > buf.Length)
				throw new ArgumentOutOfRangeException ("offset");

			if (size < 0 || offset + size > buf.Length)
				throw new ArgumentOutOfRangeException ("size");

			SocketError error;

			int ret = Send_nochecks (buf, offset, size, flags, out error);

			if (error != SocketError.Success)
				throw new SocketException ((int) error);

			return ret;
		}

#if NET_2_0
		public int Send (byte [] buf, int offset, int size, SocketFlags flags, out SocketError error)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buf == null)
				throw new ArgumentNullException ("buffer");

			if (offset < 0 || offset > buf.Length)
				throw new ArgumentOutOfRangeException ("offset");

			if (size < 0 || offset + size > buf.Length)
				throw new ArgumentOutOfRangeException ("size");

			return Send_nochecks (buf, offset, size, flags, out error);
		}

		[MonoTODO]
		public int Send (IList<ArraySegment<byte>> buffers)
		{
			/* For these generic IList overloads I need to
			 * implement WSASend in the runtime
			 */
			throw new NotImplementedException ();
		}

		[MonoTODO]
		public int Send (IList<ArraySegment<byte>> buffers,
				 SocketFlags socketFlags)
		{
			throw new NotImplementedException ();
		}

		[CLSCompliant (false)]
		[MonoTODO]
		public int Send (IList<ArraySegment<byte>> buffers,
				 SocketFlags socketFlags,
				 out SocketError errorCode)
		{
			throw new NotImplementedException ();
		}
#endif

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static int Send_internal(IntPtr sock,
							byte[] buf, int offset,
							int count,
							SocketFlags flags,
							out int error);

		int Send_nochecks (byte [] buf, int offset, int size, SocketFlags flags, out SocketError error)
		{
			if (size == 0) {
				error = SocketError.Success;
				return 0;
			}

			int nativeError;

			int ret = Send_internal (socket, buf, offset, size, flags, out nativeError);

			error = (SocketError)nativeError;

			if (error != SocketError.Success && error != SocketError.WouldBlock && error != SocketError.InProgress)
				connected = false;
			else
				connected = true;

			return ret;
		}

#if NET_2_0
		[MonoTODO ("Not implemented")]
		public void SendFile (string fileName)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (!connected)
				throw new NotSupportedException ();

			if (!blocking)
				throw new InvalidOperationException ();

			if (!File.Exists (fileName))
				throw new FileNotFoundException ();

			/* FIXME: Implement TransmitFile */
			throw new NotImplementedException ();
		}

		[MonoTODO ("Not implemented")]
		public void SendFile (string fileName, byte[] preBuffer, byte[] postBuffer, TransmitFileOptions flags)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (!connected)
				throw new NotSupportedException ();

			if (!blocking)
				throw new InvalidOperationException ();

			if (!File.Exists (fileName))
				throw new FileNotFoundException ();

			/* FIXME: Implement TransmitFile */
			throw new NotImplementedException ();
		}
#endif

		public int SendTo (byte [] buffer, EndPoint remote_end)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buffer == null)
				throw new ArgumentNullException ("buffer");

			if (remote_end == null)
				throw new ArgumentNullException ("remote_end");

			return SendTo_nochecks (buffer, 0, buffer.Length, SocketFlags.None, remote_end);
		}

		public int SendTo (byte [] buffer, SocketFlags flags, EndPoint remote_end)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buffer == null)
				throw new ArgumentNullException ("buffer");

			if (remote_end == null)
				throw new ArgumentNullException ("remote_end");
				
			return SendTo_nochecks (buffer, 0, buffer.Length, flags, remote_end);
		}

		public int SendTo (byte [] buffer, int size, SocketFlags flags, EndPoint remote_end)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buffer == null)
				throw new ArgumentNullException ("buffer");

			if (remote_end == null)
				throw new ArgumentNullException ("remote_end");

			if (size < 0 || size > buffer.Length)
				throw new ArgumentOutOfRangeException ("size");

			return SendTo_nochecks (buffer, 0, size, flags, remote_end);
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static int SendTo_internal(IntPtr sock,
							  byte[] buffer,
							  int offset,
							  int count,
							  SocketFlags flags,
							  SocketAddress sa,
							  out int error);

		public int SendTo (byte [] buffer, int offset, int size, SocketFlags flags,
				   EndPoint remote_end)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (buffer == null)
				throw new ArgumentNullException ("buffer");

			if (remote_end == null)
				throw new ArgumentNullException("remote_end");

			if (offset < 0 || offset > buffer.Length)
				throw new ArgumentOutOfRangeException ("offset");

			if (size < 0 || offset + size > buffer.Length)
				throw new ArgumentOutOfRangeException ("size");

			return SendTo_nochecks (buffer, offset, size, flags, remote_end);
		}

		int SendTo_nochecks (byte [] buffer, int offset, int size, SocketFlags flags,
				   EndPoint remote_end)
		{
			SocketAddress sockaddr = remote_end.Serialize ();

			int ret, error;

			ret = SendTo_internal (socket, buffer, offset, size, flags, sockaddr, out error);

			SocketError err = (SocketError) error;
			if (err != 0) {
				if (err != SocketError.WouldBlock && err != SocketError.InProgress)
					connected = false;

				throw new SocketException (error);
			}

			connected = true;

#if NET_2_0
			isbound = true;
#endif
			
			seed_endpoint = remote_end;
			
			return ret;
		}

		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static void SetSocketOption_internal (IntPtr socket, SocketOptionLevel level,
								     SocketOptionName name, object obj_val,
								     byte [] byte_val, int int_val,
								     out int error);

		public void SetSocketOption (SocketOptionLevel level, SocketOptionName name, byte[] opt_value)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			int error;
			
			SetSocketOption_internal(socket, level, name, null,
						 opt_value, 0, out error);

			if (error != 0)
				throw new SocketException (error);
		}

		public void SetSocketOption (SocketOptionLevel level, SocketOptionName name, int opt_value)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			int error;
			
			SetSocketOption_internal(socket, level, name, null,
						 null, opt_value, out error);

			if (error != 0)
				throw new SocketException (error);
		}

		public void SetSocketOption (SocketOptionLevel level, SocketOptionName name, object opt_value)
		{

			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			if (opt_value == null)
				throw new ArgumentNullException("opt_value");
			
			int error;
			/* From MS documentation on SetSocketOption: "For an
			 * option with a Boolean data type, specify a nonzero
			 * value to enable the option, and a zero value to
			 * disable the option."
			 * Booleans are only handled in 2.0
			 */

			if (opt_value is System.Boolean) {
#if NET_2_0
				bool bool_val = (bool) opt_value;
				int int_val = (bool_val) ? 1 : 0;

				SetSocketOption_internal (socket, level, name, null, null, int_val, out error);
#else
				throw new ArgumentException ("Use an integer 1 (true) or 0 (false) instead of a boolean.", "opt_value");
#endif
			} else {
				SetSocketOption_internal (socket, level, name, opt_value, null, 0, out error);
			}

			if (error != 0)
				throw new SocketException (error);
		}

#if NET_2_0
		public void SetSocketOption (SocketOptionLevel level, SocketOptionName name, bool optionValue)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			int error;
			int int_val = (optionValue) ? 1 : 0;
			SetSocketOption_internal (socket, level, name, null, null, int_val, out error);
			if (error != 0)
				throw new SocketException (error);
		}
#endif
		[MethodImplAttribute(MethodImplOptions.InternalCall)]
		private extern static void Shutdown_internal(IntPtr socket, SocketShutdown how, out int error);
		
		public void Shutdown (SocketShutdown how)
		{
			if (disposed && closed)
				throw new ObjectDisposedException (GetType ().ToString ());

			int error;
			
			Shutdown_internal (socket, how, out error);

			if (error != 0)
				throw new SocketException (error);
		}

#if ONLY_1_1
		public override int GetHashCode ()
		{
			// LAMESPEC:
			// The socket is not suitable to serve as a hash code,
			// because it will change during its lifetime, but
			// this is how MS.NET 1.1 implemented this method.
			return (int) socket; 
		}
#endif

		protected virtual void Dispose (bool explicitDisposing)
		{
			if (disposed)
				return;

			disposed = true;
			connected = false;
			if ((int) socket != -1) {
				int error;
				closed = true;
				IntPtr x = socket;
				socket = (IntPtr) (-1);
				Close_internal (x, out error);
				if (blocking_thread != null) {
					blocking_thread.Abort ();
					blocking_thread = null;
				}

				if (error != 0)
					throw new SocketException (error);
			}
		}

		void IDisposable.Dispose ()
		{
			Dispose (true);
			GC.SuppressFinalize (this);
		}
		
		~Socket () {
			Dispose (false);
		}
	}
}
