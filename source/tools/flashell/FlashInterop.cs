using System;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using stdole;
using DISPPARAMS=System.Runtime.InteropServices.ComTypes.DISPPARAMS;
using EXCEPINFO=System.Runtime.InteropServices.ComTypes.EXCEPINFO;

namespace Interop.Flash
{
	/// <summary><para><c>IFlashEvents</c> interface.  </para><para>Event interface for Shockwave Flash</para></summary>
	[Guid("D27CDB6D-AE6D-11CF-96B8-444553540000")]
	[ComImport]
	[TypeLibType(TypeLibTypeFlags.FDispatchable | TypeLibTypeFlags.FHidden)]
	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	public interface IFlashEvents
	{
		/// <summary><para><c>OnReadyStateChange</c> method of <c>_IShockwaveFlashEvents</c> interface.</para></summary>
		/// <remarks><para>An original IDL definition of <c>OnReadyStateChange</c> method was the following:  <c>HRESULT OnReadyStateChange (long newState)</c>;</para></remarks>
		[DispId(-609)]
		void OnReadyStateChange (int newState);

		/// <summary><para><c>OnProgress</c> method of <c>_IShockwaveFlashEvents</c> interface.</para></summary>
		/// <remarks><para>An original IDL definition of <c>OnProgress</c> method was the following:  <c>HRESULT OnProgress (long percentDone)</c>;</para></remarks>
		[DispId(1958)]
		void OnProgress (int percentDone);

		/// <summary><para><c>FSCommand</c> method of <c>_IShockwaveFlashEvents</c> interface.</para></summary>
		/// <remarks><para>An original IDL definition of <c>FSCommand</c> method was the following:  <c>HRESULT FSCommand (BSTR command, BSTR args)</c>;</para></remarks>
		[DispId(150)]
		void FSCommand ([MarshalAs(UnmanagedType.BStr)] string command, [MarshalAs(UnmanagedType.BStr)] string args);

		// Flash8 Only
		[DispId(0x000000c5)]
		void FlashCall([MarshalAs(UnmanagedType.BStr)] string request);
	}

	/// <summary><para>Delegate for handling <c>OnReadyStateChange</c> event of <c>IFlashEvents</c> interface.</para></summary>
	/// <remarks><para>An original IDL definition of <c>OnReadyStateChange</c> event was the following:  <c>HRESULT OnReadyStateChangeEventHandler (long newState)</c>;</para></remarks>
	public delegate void OnReadyStateChangeEventHandler (int newState);

	/// <summary><para>Delegate for handling <c>OnProgress</c> event of <c>_IShockwaveFlashEvents</c> interface.</para></summary>
	/// <remarks><para>An original IDL definition of <c>OnProgress</c> event was the following:  <c>HRESULT _IShockwaveFlashEvents_OnProgressEventHandler (long percentDone)</c>;</para></remarks>
	public delegate void OnProgressEventHandler (int percentDone);

	/// <summary><para>Delegate for handling <c>FSCommand</c> event of <c>_IShockwaveFlashEvents</c> interface.</para></summary>
	/// <remarks><para>An original IDL definition of <c>FSCommand</c> event was the following:  <c>HRESULT _IShockwaveFlashEvents_FSCommandEventHandler (BSTR command, BSTR args)</c>;</para></remarks>
	public delegate void FSCommandEventHandler ([MarshalAs(UnmanagedType.BStr)] string command, [MarshalAs(UnmanagedType.BStr)] string args);

	// Flash8 Only
	public delegate void FlashCallEventHandler([MarshalAs(UnmanagedType.BStr)] string request);

	/// <summary><para>Declaration of events of <c>_IShockwaveFlashEvents</c> source interface.  </para><para>Event interface for Shockwave Flash</para></summary>
	[ComEventInterface(typeof(IFlashEvents),typeof(EventProvider))]
	[ComVisible(false)]
	public interface IFlashEvent
	{
		/// <summary><para><c>OnReadyStateChange</c> event of <c>_IShockwaveFlashEvents</c> interface.</para></summary>
		event OnReadyStateChangeEventHandler OnReadyStateChange;

		/// <summary><para><c>OnProgress</c> event of <c>_IShockwaveFlashEvents</c> interface.</para></summary>
		event OnProgressEventHandler OnProgress;

		/// <summary><para><c>FSCommand</c> event of <c>_IShockwaveFlashEvents</c> interface.</para></summary>
		event FSCommandEventHandler FSCommand;

		event FlashCallEventHandler FlashCall;
	}

	[ClassInterface(ClassInterfaceType.None)]
	internal class SinkHelper: IFlashEvents
	{
		public int Cookie;

		public event OnReadyStateChangeEventHandler OnReadyStateChangeDelegate = null;
		public void Set_OnReadyStateChangeDelegate(OnReadyStateChangeEventHandler deleg)
		{
			OnReadyStateChangeDelegate = deleg;
		}
		public bool Is_OnReadyStateChangeDelegate(OnReadyStateChangeEventHandler deleg)
		{
			return (OnReadyStateChangeDelegate == deleg);
		}
		public void Clear_OnReadyStateChangeDelegate()
		{
			OnReadyStateChangeDelegate = null;
		}
		void IFlashEvents.OnReadyStateChange (int newState)
		{
			if (OnReadyStateChangeDelegate!=null)
				OnReadyStateChangeDelegate(newState);
		}

		public event OnProgressEventHandler OnProgressDelegate = null;
		public void Set_OnProgressDelegate(OnProgressEventHandler deleg)
		{
			OnProgressDelegate = deleg;
		}
		public bool Is_OnProgressDelegate(OnProgressEventHandler deleg)
		{
			return (OnProgressDelegate == deleg);
		}
		public void Clear_OnProgressDelegate()
		{
			OnProgressDelegate = null;
		}
		void IFlashEvents.OnProgress (int percentDone)
		{
			if (OnProgressDelegate!=null)
				OnProgressDelegate(percentDone);
		}

		public event FSCommandEventHandler FSCommandDelegate = null;
		public void Set_FSCommandDelegate(FSCommandEventHandler deleg)
		{
			FSCommandDelegate = deleg;
		}
		public bool Is_FSCommandDelegate(FSCommandEventHandler deleg)
		{
			return (FSCommandDelegate == deleg);
		}
		public void Clear_FSCommandDelegate()
		{
			FSCommandDelegate = null;
		}
		void IFlashEvents.FSCommand (string command, string args)
		{
			if (FSCommandDelegate!=null)
				FSCommandDelegate(command, args);
		}

		// Flash8 Only
		public event FlashCallEventHandler FlashCallDelegate = null;
		public void Set_FlashCallDelegate(FlashCallEventHandler deleg)
		{
			FlashCallDelegate = deleg;
		}
		public bool Is_FSCommandDelegate(FlashCallEventHandler deleg)
		{
			return (FlashCallDelegate == deleg);
		}
		public void Clear_FlashCallDelegate()
		{
			FlashCallDelegate = null;
		}
		void IFlashEvents.FlashCall (string request)
		{
			if (FlashCallDelegate!=null)
				FlashCallDelegate(request);
		}
	}

	internal class EventProvider: IDisposable, IFlashEvent
	{
		IConnectionPointContainer ConnectionPointContainer;
        IConnectionPoint ConnectionPoint;
		SinkHelper EventSinkHelper;
		int ConnectionCount;

		// Constructor: remember ConnectionPointContainer
		EventProvider(object CPContainer)
		{
            ConnectionPointContainer = (IConnectionPointContainer)CPContainer;
		}

		// Force disconnection from ActiveX event source
		~EventProvider()
		{
			Disconnect();
			ConnectionPointContainer = null;
		}

		// Aletnative to destructor
		void IDisposable.Dispose()
		{
			Disconnect();
			ConnectionPointContainer = null;
			GC.SuppressFinalize(this);
		}

		// Connect to ActiveX event source
		void Connect()
		{
			if (ConnectionPoint == null)
			{
				ConnectionCount = 0;
				var g = new Guid("D27CDB6D-AE6D-11CF-96B8-444553540000");
				ConnectionPointContainer.FindConnectionPoint(ref g, out ConnectionPoint);
				EventSinkHelper = new SinkHelper();
				ConnectionPoint.Advise(EventSinkHelper, out EventSinkHelper.Cookie);
			}
		}

		// Disconnect from ActiveX event source
		void Disconnect()
		{
			Monitor.Enter(this);
			try 
			{
				if (EventSinkHelper != null)
					ConnectionPoint.Unadvise(EventSinkHelper.Cookie);
				ConnectionPoint = null;
				EventSinkHelper = null;
			} 
			catch { }
			Monitor.Exit(this);
		}

		// If no event handler present then disconnect from ActiveX event source
		void CheckDisconnect()
		{
			ConnectionCount--;
			if (ConnectionCount <= 0)
				Disconnect();
		}

		event OnReadyStateChangeEventHandler IFlashEvent.OnReadyStateChange
		{
			add
			{
				Monitor.Enter(this);
				try 
				{
					Connect();
					EventSinkHelper.OnReadyStateChangeDelegate += value;
					ConnectionCount++;
				} 
				catch { }
				Monitor.Exit(this);
			}
			remove
			{
				if (EventSinkHelper != null)
				{
					Monitor.Enter(this);
					try 
					{
						EventSinkHelper.OnReadyStateChangeDelegate -= value;
						CheckDisconnect();
					} 
					catch { }
					Monitor.Exit(this);
				}
			}
		}

		event OnProgressEventHandler IFlashEvent.OnProgress
		{
			add
			{
				Monitor.Enter(this);
				try 
				{
					Connect();
					EventSinkHelper.OnProgressDelegate += value;
					ConnectionCount++;
				} 
				catch { }
				Monitor.Exit(this);
			}
			remove
			{
				if (EventSinkHelper != null)
				{
					Monitor.Enter(this);
					try 
					{
						EventSinkHelper.OnProgressDelegate -= value;
						CheckDisconnect();
					} 
					catch { }
					Monitor.Exit(this);
				}
			}
		}

		event FSCommandEventHandler IFlashEvent.FSCommand
		{
			add
			{
				Monitor.Enter(this);
				try 
				{
					Connect();
					EventSinkHelper.FSCommandDelegate += value;
					ConnectionCount++;
				} 
				catch { }
				Monitor.Exit(this);
			}
			remove
			{
				if (EventSinkHelper != null)
				{
					Monitor.Enter(this);
					try 
					{
						EventSinkHelper.FSCommandDelegate -= value;
						CheckDisconnect();
					} 
					catch { }
					Monitor.Exit(this);
				}
			}
		}

		event FlashCallEventHandler IFlashEvent.FlashCall
		{
			add
			{
				Monitor.Enter(this);
				try 
				{
					Connect();
					EventSinkHelper.FlashCallDelegate += value;
					ConnectionCount++;
				} 
				catch { }
				Monitor.Exit(this);
			}
			remove
			{
				if (EventSinkHelper != null)
				{
					Monitor.Enter(this);
					try 
					{
						EventSinkHelper.FlashCallDelegate -= value;
						CheckDisconnect();
					} 
					catch { }
					Monitor.Exit(this);
				}
			}
		}
	}

	/// <summary><para><c>IShockwaveFlash</c> interface.  </para><para>Shockwave Flash</para></summary>
	[Guid("D27CDB6C-AE6D-11CF-96B8-444553540000")]
	[ComImport]
	[TypeLibType(TypeLibTypeFlags.FDispatchable | TypeLibTypeFlags.FDual)]
	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	public interface IFlashControl
	{
		/// <summary><para><c>SetZoomRect</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method SetZoomRect</para></summary>
		/// <remarks><para>An original IDL definition of <c>SetZoomRect</c> method was the following:  <c>HRESULT SetZoomRect (long left, long top, long right, long bottom)</c>;</para></remarks>
		[DispId(109)]
		void SetZoomRect (int left, int top, int right, int bottom);

		/// <summary><para><c>Zoom</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method Zoom</para></summary>
		/// <remarks><para>An original IDL definition of <c>Zoom</c> method was the following:  <c>HRESULT Zoom (int factor)</c>;</para></remarks>
		[DispId(118)]
		void Zoom (int factor);

		/// <summary><para><c>Pan</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method Pan</para></summary>
		/// <remarks><para>An original IDL definition of <c>Pan</c> method was the following:  <c>HRESULT Pan (long x, long y, int mode)</c>;</para></remarks>
		[DispId(119)]
		void Pan (int x, int y, int mode);

		/// <summary><para><c>Play</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method Play</para></summary>
		/// <remarks><para>An original IDL definition of <c>Play</c> method was the following:  <c>HRESULT Play (void)</c>;</para></remarks>
		[DispId(112)]
		void Play ();

		/// <summary><para><c>Stop</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method Stop</para></summary>
		/// <remarks><para>An original IDL definition of <c>Stop</c> method was the following:  <c>HRESULT Stop (void)</c>;</para></remarks>
		[DispId(113)]
		void Stop ();

		/// <summary><para><c>Back</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method Back</para></summary>
		/// <remarks><para>An original IDL definition of <c>Back</c> method was the following:  <c>HRESULT Back (void)</c>;</para></remarks>
		[DispId(114)]
		void Back ();

		/// <summary><para><c>Forward</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method Forward</para></summary>
		/// <remarks><para>An original IDL definition of <c>Forward</c> method was the following:  <c>HRESULT Forward (void)</c>;</para></remarks>
		[DispId(115)]
		void Forward ();

		/// <summary><para><c>Rewind</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method Rewind</para></summary>
		/// <remarks><para>An original IDL definition of <c>Rewind</c> method was the following:  <c>HRESULT Rewind (void)</c>;</para></remarks>
		[DispId(116)]
		void Rewind ();

		/// <summary><para><c>StopPlay</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method StopPlay</para></summary>
		/// <remarks><para>An original IDL definition of <c>StopPlay</c> method was the following:  <c>HRESULT StopPlay (void)</c>;</para></remarks>
		[DispId(126)]
		void StopPlay ();

		/// <summary><para><c>GotoFrame</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method GotoFrame</para></summary>
		/// <remarks><para>An original IDL definition of <c>GotoFrame</c> method was the following:  <c>HRESULT GotoFrame (long FrameNum)</c>;</para></remarks>
		[DispId(127)]
		void GotoFrame (int FrameNum);

		/// <summary><para><c>CurrentFrame</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method CurrentFrame</para></summary>
		/// <remarks><para>An original IDL definition of <c>CurrentFrame</c> method was the following:  <c>HRESULT CurrentFrame ([out, retval] long* ReturnValue)</c>;</para></remarks>
		[DispId(128)]
		int CurrentFrame ();

		/// <summary><para><c>IsPlaying</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method IsPlaying</para></summary>
		/// <remarks><para>An original IDL definition of <c>IsPlaying</c> method was the following:  <c>HRESULT IsPlaying ([out, retval] VARIANT_BOOL* ReturnValue)</c>;</para></remarks>
		[DispId(129)]
		[return: MarshalAs(UnmanagedType.VariantBool)]
		bool IsPlaying ();

		/// <summary><para><c>PercentLoaded</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method PercentLoaded</para></summary>
		/// <remarks><para>An original IDL definition of <c>PercentLoaded</c> method was the following:  <c>HRESULT PercentLoaded ([out, retval] long* ReturnValue)</c>;</para></remarks>
		[DispId(130)]
		int PercentLoaded ();

		/// <summary><para><c>FrameLoaded</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method FrameLoaded</para></summary>
		/// <remarks><para>An original IDL definition of <c>FrameLoaded</c> method was the following:  <c>HRESULT FrameLoaded (long FrameNum, [out, retval] VARIANT_BOOL* ReturnValue)</c>;</para></remarks>
		[DispId(131)]
		[return: MarshalAs(UnmanagedType.VariantBool)]
		bool FrameLoaded (int FrameNum);

		/// <summary><para><c>FlashVersion</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method FlashVersion</para></summary>
		/// <remarks><para>An original IDL definition of <c>FlashVersion</c> method was the following:  <c>HRESULT FlashVersion ([out, retval] long* ReturnValue)</c>;</para></remarks>
		[DispId(132)]
		int FlashVersion ();

		/// <summary><para><c>LoadMovie</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method LoadMovie</para></summary>
		/// <remarks><para>An original IDL definition of <c>LoadMovie</c> method was the following:  <c>HRESULT LoadMovie (int layer, BSTR url)</c>;</para></remarks>
		[DispId(142)]
		void LoadMovie (int layer, [MarshalAs(UnmanagedType.BStr)] string url);

		/// <summary><para><c>TGotoFrame</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method TGotoFrame</para></summary>
		/// <remarks><para>An original IDL definition of <c>TGotoFrame</c> method was the following:  <c>HRESULT TGotoFrame (BSTR target, long FrameNum)</c>;</para></remarks>
		[DispId(143)]
		void TGotoFrame ([MarshalAs(UnmanagedType.BStr)] string target, int FrameNum);

		/// <summary><para><c>TGotoLabel</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method TGotoLabel</para></summary>
		/// <remarks><para>An original IDL definition of <c>TGotoLabel</c> method was the following:  <c>HRESULT TGotoLabel (BSTR target, BSTR label)</c>;</para></remarks>
		[DispId(144)]
		void TGotoLabel ([MarshalAs(UnmanagedType.BStr)] string target, [MarshalAs(UnmanagedType.BStr)] string label);

		/// <summary><para><c>TCurrentFrame</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method TCurrentFrame</para></summary>
		/// <remarks><para>An original IDL definition of <c>TCurrentFrame</c> method was the following:  <c>HRESULT TCurrentFrame (BSTR target, [out, retval] long* ReturnValue)</c>;</para></remarks>
		[DispId(145)]
		int TCurrentFrame ([MarshalAs(UnmanagedType.BStr)] string target);

		/// <summary><para><c>TCurrentLabel</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method TCurrentLabel</para></summary>
		/// <remarks><para>An original IDL definition of <c>TCurrentLabel</c> method was the following:  <c>HRESULT TCurrentLabel (BSTR target, [out, retval] BSTR* ReturnValue)</c>;</para></remarks>
		[DispId(146)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string TCurrentLabel ([MarshalAs(UnmanagedType.BStr)] string target);

		/// <summary><para><c>TPlay</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method TPlay</para></summary>
		/// <remarks><para>An original IDL definition of <c>TPlay</c> method was the following:  <c>HRESULT TPlay (BSTR target)</c>;</para></remarks>
		[DispId(147)]
		void TPlay ([MarshalAs(UnmanagedType.BStr)] string target);

		/// <summary><para><c>TStopPlay</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method TStopPlay</para></summary>
		/// <remarks><para>An original IDL definition of <c>TStopPlay</c> method was the following:  <c>HRESULT TStopPlay (BSTR target)</c>;</para></remarks>
		[DispId(148)]
		void TStopPlay ([MarshalAs(UnmanagedType.BStr)] string target);

		/// <summary><para><c>SetVariable</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method SetVariable</para></summary>
		/// <remarks><para>An original IDL definition of <c>SetVariable</c> method was the following:  <c>HRESULT SetVariable (BSTR name, BSTR value)</c>;</para></remarks>
		[DispId(151)]
		void SetVariable ([MarshalAs(UnmanagedType.BStr)] string name, [MarshalAs(UnmanagedType.BStr)] string value);

		/// <summary><para><c>GetVariable</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method GetVariable</para></summary>
		/// <remarks><para>An original IDL definition of <c>GetVariable</c> method was the following:  <c>HRESULT GetVariable (BSTR name, [out, retval] BSTR* ReturnValue)</c>;</para></remarks>
		[DispId(152)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string GetVariable ([MarshalAs(UnmanagedType.BStr)] string name);

		/// <summary><para><c>TSetProperty</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method TSetProperty</para></summary>
		/// <remarks><para>An original IDL definition of <c>TSetProperty</c> method was the following:  <c>HRESULT TSetProperty (BSTR target, int property, BSTR value)</c>;</para></remarks>
		[DispId(153)]
		void TSetProperty ([MarshalAs(UnmanagedType.BStr)] string target, int property, [MarshalAs(UnmanagedType.BStr)] string value);

		/// <summary><para><c>TGetProperty</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method TGetProperty</para></summary>
		/// <remarks><para>An original IDL definition of <c>TGetProperty</c> method was the following:  <c>HRESULT TGetProperty (BSTR target, int property, [out, retval] BSTR* ReturnValue)</c>;</para></remarks>
		[DispId(154)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string TGetProperty ([MarshalAs(UnmanagedType.BStr)] string target, int property);

		/// <summary><para><c>TCallFrame</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method TCallFrame</para></summary>
		/// <remarks><para>An original IDL definition of <c>TCallFrame</c> method was the following:  <c>HRESULT TCallFrame (BSTR target, int FrameNum)</c>;</para></remarks>
		[DispId(155)]
		void TCallFrame ([MarshalAs(UnmanagedType.BStr)] string target, int FrameNum);

		/// <summary><para><c>TCallLabel</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method TCallLabel</para></summary>
		/// <remarks><para>An original IDL definition of <c>TCallLabel</c> method was the following:  <c>HRESULT TCallLabel (BSTR target, BSTR label)</c>;</para></remarks>
		[DispId(156)]
		void TCallLabel ([MarshalAs(UnmanagedType.BStr)] string target, [MarshalAs(UnmanagedType.BStr)] string label);

		/// <summary><para><c>TSetPropertyNum</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method TSetPropertyNum</para></summary>
		/// <remarks><para>An original IDL definition of <c>TSetPropertyNum</c> method was the following:  <c>HRESULT TSetPropertyNum (BSTR target, int property, double value)</c>;</para></remarks>
		[DispId(157)]
		void TSetPropertyNum ([MarshalAs(UnmanagedType.BStr)] string target, int property, double value);

		/// <summary><para><c>TGetPropertyNum</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method TGetPropertyNum</para></summary>
		/// <remarks><para>An original IDL definition of <c>TGetPropertyNum</c> method was the following:  <c>HRESULT TGetPropertyNum (BSTR target, int property, [out, retval] double* ReturnValue)</c>;</para></remarks>
		[DispId(158)]
		double TGetPropertyNum ([MarshalAs(UnmanagedType.BStr)] string target, int property);

		/// <summary><para><c>TGetPropertyAsNumber</c> method of <c>IShockwaveFlash</c> interface.  </para><para>method TGetPropertyAsNumber</para></summary>
		/// <remarks><para>An original IDL definition of <c>TGetPropertyAsNumber</c> method was the following:  <c>HRESULT TGetPropertyAsNumber (BSTR target, int property, [out, retval] double* ReturnValue)</c>;</para></remarks>
		[DispId(172)]
		double TGetPropertyAsNumber ([MarshalAs(UnmanagedType.BStr)] string target, int property);

		/// <summary><para><c>AlignMode</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property AlignMode</para></summary>
		/// <remarks><para>An original IDL definition of <c>AlignMode</c> property was the following:  <c>int AlignMode</c>;</para></remarks>
		int AlignMode
		{
			[DispId(121)]
			get;
			[DispId(121)]
			set;
		}

		/// <summary><para><c>AllowScriptAccess</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property AllowScriptAccess</para></summary>
		/// <remarks><para>An original IDL definition of <c>AllowScriptAccess</c> property was the following:  <c>BSTR AllowScriptAccess</c>;</para></remarks>
		string AllowScriptAccess
		{
			[DispId(171)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[DispId(171)]
			set;
		}

		/// <summary><para><c>BackgroundColor</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property BackgroundColor</para></summary>
		/// <remarks><para>An original IDL definition of <c>BackgroundColor</c> property was the following:  <c>long BackgroundColor</c>;</para></remarks>
		int BackgroundColor
		{
			[DispId(123)]
			get;
			[DispId(123)]
			set;
		}

		/// <summary><para><c>Base</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property Base</para></summary>
		/// <remarks><para>An original IDL definition of <c>Base</c> property was the following:  <c>BSTR Base</c>;</para></remarks>
		string Base
		{
			[DispId(136)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[DispId(136)]
			set;
		}

		/// <summary><para><c>BGColor</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property BGColor</para></summary>
		/// <remarks><para>An original IDL definition of <c>BGColor</c> property was the following:  <c>BSTR BGColor</c>;</para></remarks>
		string BGColor
		{
			[DispId(140)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[DispId(140)]
			set;
		}

		/// <summary><para><c>DeviceFont</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property DeviceFont</para></summary>
		/// <remarks><para>An original IDL definition of <c>DeviceFont</c> property was the following:  <c>VARIANT_BOOL DeviceFont</c>;</para></remarks>
		bool DeviceFont
		{
			[DispId(138)]
			[return: MarshalAs(UnmanagedType.VariantBool)]
			get;
			[DispId(138)]
			set;
		}

		/// <summary><para><c>EmbedMovie</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property EmbedMovie</para></summary>
		/// <remarks><para>An original IDL definition of <c>EmbedMovie</c> property was the following:  <c>VARIANT_BOOL EmbedMovie</c>;</para></remarks>
		bool EmbedMovie
		{
			[DispId(139)]
			[return: MarshalAs(UnmanagedType.VariantBool)]
			get;
			[DispId(139)]
			set;
		}

		/// <summary><para><c>FlashVars</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property FlashVars</para></summary>
		/// <remarks><para>An original IDL definition of <c>FlashVars</c> property was the following:  <c>BSTR FlashVars</c>;</para></remarks>
		string FlashVars
		{
			[DispId(170)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[DispId(170)]
			set;
		}

		/// <summary><para><c>FrameNum</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property FrameNum</para></summary>
		/// <remarks><para>An original IDL definition of <c>FrameNum</c> property was the following:  <c>long FrameNum</c>;</para></remarks>
		int FrameNum
		{
			[DispId(107)]
			get;
			[DispId(107)]
			set;
		}

		/// <summary><para><c>InlineData</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property inline-data</para></summary>
		/// <remarks><para>An original IDL definition of <c>InlineData</c> property was the following:  <c>IUnknown* InlineData</c>;</para></remarks>
		object InlineData
		{
			[DispId(191)]
			[return: MarshalAs(UnmanagedType.IUnknown)]
			get;
			[DispId(191)]
			set;
		}

		/// <summary><para><c>Loop</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property Loop</para></summary>
		/// <remarks><para>An original IDL definition of <c>Loop</c> property was the following:  <c>VARIANT_BOOL Loop</c>;</para></remarks>
		bool Loop
		{
			[DispId(106)]
			[return: MarshalAs(UnmanagedType.VariantBool)]
			get;
			[DispId(106)]
			set;
		}

		/// <summary><para><c>Menu</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property Menu</para></summary>
		/// <remarks><para>An original IDL definition of <c>Menu</c> property was the following:  <c>VARIANT_BOOL Menu</c>;</para></remarks>
		bool Menu
		{
			[DispId(135)]
			[return: MarshalAs(UnmanagedType.VariantBool)]
			get;
			[DispId(135)]
			set;
		}

		/// <summary><para><c>Movie</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property Movie</para></summary>
		/// <remarks><para>An original IDL definition of <c>Movie</c> property was the following:  <c>BSTR Movie</c>;</para></remarks>
		string Movie
		{
			[DispId(102)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[DispId(102)]
			set;
		}

		/// <summary><para><c>MovieData</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property MovieData</para></summary>
		/// <remarks><para>An original IDL definition of <c>MovieData</c> property was the following:  <c>BSTR MovieData</c>;</para></remarks>
		string MovieData
		{
			[DispId(190)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[DispId(190)]
			set;
		}

		/// <summary><para><c>Playing</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property Playing</para></summary>
		/// <remarks><para>An original IDL definition of <c>Playing</c> property was the following:  <c>VARIANT_BOOL Playing</c>;</para></remarks>
		bool Playing
		{
			[DispId(125)]
			[return: MarshalAs(UnmanagedType.VariantBool)]
			get;
			[DispId(125)]
			set;
		}

		/// <summary><para><c>Quality</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property Quality</para></summary>
		/// <remarks><para>An original IDL definition of <c>Quality</c> property was the following:  <c>int Quality</c>;</para></remarks>
		int Quality
		{
			[DispId(105)]
			get;
			[DispId(105)]
			set;
		}

		/// <summary><para><c>Quality2</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property Quality2</para></summary>
		/// <remarks><para>An original IDL definition of <c>Quality2</c> property was the following:  <c>BSTR Quality2</c>;</para></remarks>
		string Quality2
		{
			[DispId(141)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[DispId(141)]
			set;
		}

		/// <summary><para><c>ReadyState</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property ReadyState</para></summary>
		/// <remarks><para>An original IDL definition of <c>ReadyState</c> property was the following:  <c>long ReadyState</c>;</para></remarks>
		int ReadyState
		{
			[DispId(-525)]
			get;
		}

		/// <summary><para><c>SAlign</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property SAlign</para></summary>
		/// <remarks><para>An original IDL definition of <c>SAlign</c> property was the following:  <c>BSTR SAlign</c>;</para></remarks>
		string SAlign
		{
			[DispId(134)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[DispId(134)]
			set;
		}

		/// <summary><para><c>Scale</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property Scale</para></summary>
		/// <remarks><para>An original IDL definition of <c>Scale</c> property was the following:  <c>BSTR Scale</c>;</para></remarks>
		string Scale
		{
			[DispId(137)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[DispId(137)]
			set;
		}

		/// <summary><para><c>ScaleMode</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property ScaleMode</para></summary>
		/// <remarks><para>An original IDL definition of <c>ScaleMode</c> property was the following:  <c>int ScaleMode</c>;</para></remarks>
		int ScaleMode
		{
			[DispId(120)]
			get;
			[DispId(120)]
			set;
		}

		/// <summary><para><c>SeamlessTabbing</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property SeamlessTabbing</para></summary>
		/// <remarks><para>An original IDL definition of <c>SeamlessTabbing</c> property was the following:  <c>VARIANT_BOOL SeamlessTabbing</c>;</para></remarks>
		bool SeamlessTabbing
		{
			[DispId(192)]
			[return: MarshalAs(UnmanagedType.VariantBool)]
			get;
			[DispId(192)]
			set;
		}

		/// <summary><para><c>SWRemote</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property SWRemote</para></summary>
		/// <remarks><para>An original IDL definition of <c>SWRemote</c> property was the following:  <c>BSTR SWRemote</c>;</para></remarks>
		string SWRemote
		{
			[DispId(159)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[DispId(159)]
			set;
		}

		/// <summary><para><c>TotalFrames</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property TotalFrames</para></summary>
		/// <remarks><para>An original IDL definition of <c>TotalFrames</c> property was the following:  <c>long TotalFrames</c>;</para></remarks>
		int TotalFrames
		{
			[DispId(124)]
			get;
		}

		/// <summary><para><c>WMode</c> property of <c>IShockwaveFlash</c> interface.  </para><para>property WMode</para></summary>
		/// <remarks><para>An original IDL definition of <c>WMode</c> property was the following:  <c>BSTR WMode</c>;</para></remarks>
		string WMode
		{
			[DispId(133)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[DispId(133)]
			set;
		}

		// Flash8 adds the following
		//		[id(0x000000c1), helpstring("method EnforceLocalSecurity")]
		//		HRESULT EnforceLocalSecurity();
		[DispId(0xc1)]
		void EnforceLocalSecurity();
		[DispId(0xc2)]
		//		[id(0x000000c2), propget, helpstring("property Profile")]
		//		HRESULT Profile([out, retval] VARIANT_BOOL* pVal);
		//		[id(0x000000c2), propput, helpstring("property Profile")]
		//		HRESULT Profile([in] VARIANT_BOOL pVal);
		bool Profile
		{
			[DispId(0xc2)]
			[return: MarshalAs(UnmanagedType.VariantBool)]
			get;
			[DispId(0xc2)]
			set;
		}

		//		[id(0x000000c3), propget, helpstring("property ProfileAddress")]
		//		HRESULT ProfileAddress([out, retval] BSTR* pVal);
		//		[id(0x000000c3), propput, helpstring("property ProfileAddress")]
		//		HRESULT ProfileAddress([in] BSTR pVal);
		string ProfileAddress
		{
			[DispId(0xc3)]
			[return: MarshalAs(UnmanagedType.BStr)]
			get;
			[DispId(0xc3)]
			set;
		}

		//		[id(0x000000c4), propget, helpstring("property ProfilePort")]
		//		HRESULT ProfilePort([out, retval] long* pVal);
		//		[id(0x000000c4), propput, helpstring("property ProfilePort")]
		//		HRESULT ProfilePort([in] long pVal);
		int ProfilePort
		{
			[DispId(0xc4)]
			get;
			[DispId(0xc4)]
			set;
		}

		//		[id(0x000000c6), helpstring("method Call")]
		//		HRESULT CallFunction([in] BSTR request, [out, retval] BSTR* response);
		[DispId(0xc6)]
		[return: MarshalAs(UnmanagedType.BStr)]
		string CallFunction([MarshalAs(UnmanagedType.BStr)] string request);
		
		//		[id(0x000000c7), helpstring("method SetReturnValue")]
		//		HRESULT SetReturnValue([in] BSTR returnValue);
		[DispId(0xc7)]
		void SetReturnValue([MarshalAs(UnmanagedType.BStr)] string returnValue);

		//		[id(0x000000c8), helpstring("method DisableLocalSecurity")]
		//		HRESULT DisableLocalSecurity();
		[DispId(0xc8)]
		void DisableLocalSecurity();
	}

	/// <summary><para><c>IDispatchEx</c> interface.</para></summary>
	[Guid("A6EF9860-C720-11D0-9337-00A0C90DCAA9")]
	[ComImport]
	[TypeLibType((short)4096)]
	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	public interface IDispatchEx
	{
		/// <summary><para><c>GetDispID</c> method of <c>IDispatchEx</c> interface.</para></summary>
		/// <remarks><para>An original IDL definition of <c>GetDispID</c> method was the following:  <c>HRESULT GetDispID (BSTR bstrName, unsigned long grfdex, [out] long* pid)</c>;</para></remarks>
		[DispId(1610743808)]
		void GetDispID ([MarshalAs(UnmanagedType.BStr)] string bstrName, uint grfdex, [Out] out int pid);

		/// <summary><para><c>RemoteInvokeEx</c> method of <c>IDispatchEx</c> interface.</para></summary>
		/// <remarks><para>An original IDL definition of <c>RemoteInvokeEx</c> method was the following:  <c>HRESULT RemoteInvokeEx (long id, unsigned long lcid, unsigned long dwFlags, [in] Interop.stdole.DISPPARAMS* pdp, [out] VARIANT* pvarRes, [out] Interop.stdole.EXCEPINFO* pei, IServiceProvider* pspCaller, unsigned int cvarRefArg, [in] unsigned int* rgiRefArg, [in, out] VARIANT* rgvarRefArg)</c>;</para></remarks>
		[DispId(1610743809)]
		void RemoteInvokeEx (int id, uint lcid, uint dwFlags, [In] ref DISPPARAMS pdp, [Out] out object pvarRes, [Out] out EXCEPINFO pei, IServiceProvider pspCaller, uint cvarRefArg, [In] ref uint rgiRefArg, [In, Out] ref object rgvarRefArg);

		/// <summary><para><c>DeleteMemberByName</c> method of <c>IDispatchEx</c> interface.</para></summary>
		/// <remarks><para>An original IDL definition of <c>DeleteMemberByName</c> method was the following:  <c>HRESULT DeleteMemberByName (BSTR bstrName, unsigned long grfdex)</c>;</para></remarks>
		[DispId(1610743810)]
		void DeleteMemberByName ([MarshalAs(UnmanagedType.BStr)] string bstrName, uint grfdex);

		/// <summary><para><c>DeleteMemberByDispID</c> method of <c>IDispatchEx</c> interface.</para></summary>
		/// <remarks><para>An original IDL definition of <c>DeleteMemberByDispID</c> method was the following:  <c>HRESULT DeleteMemberByDispID (long id)</c>;</para></remarks>
		[DispId(1610743811)]
		void DeleteMemberByDispID (int id);

		/// <summary><para><c>GetMemberProperties</c> method of <c>IDispatchEx</c> interface.</para></summary>
		/// <remarks><para>An original IDL definition of <c>GetMemberProperties</c> method was the following:  <c>HRESULT GetMemberProperties (long id, unsigned long grfdexFetch, [out] unsigned long* pgrfdex)</c>;</para></remarks>
		[DispId(1610743812)]
		void GetMemberProperties (int id, uint grfdexFetch, [Out] out uint pgrfdex);

		/// <summary><para><c>GetMemberName</c> method of <c>IDispatchEx</c> interface.</para></summary>
		/// <remarks><para>An original IDL definition of <c>GetMemberName</c> method was the following:  <c>HRESULT GetMemberName (long id, [out] BSTR* pbstrName)</c>;</para></remarks>
		[DispId(1610743813)]
		void GetMemberName (int id, [Out, MarshalAs(UnmanagedType.BStr)] out string pbstrName);

		/// <summary><para><c>GetNextDispID</c> method of <c>IDispatchEx</c> interface.</para></summary>
		/// <remarks><para>An original IDL definition of <c>GetNextDispID</c> method was the following:  <c>HRESULT GetNextDispID (unsigned long grfdex, long id, [out] long* pid)</c>;</para></remarks>
		[DispId(1610743814)]
		void GetNextDispID (uint grfdex, int id, [Out] out int pid);

		/// <summary><para><c>GetNameSpaceParent</c> method of <c>IDispatchEx</c> interface.</para></summary>
		/// <remarks><para>An original IDL definition of <c>GetNameSpaceParent</c> method was the following:  <c>HRESULT GetNameSpaceParent ([out] IUnknown** ppunk)</c>;</para></remarks>
		[DispId(1610743815)]
		void GetNameSpaceParent ([Out, MarshalAs(UnmanagedType.IUnknown)] out object ppunk);
	}

	/// <summary><para><c>IFlashFactory</c> interface.  </para><para>IFlashFactory Interface</para></summary>
	[Guid("D27CDB70-AE6D-11CF-96B8-444553540000")]
	[ComImport]
	[TypeLibType((short)0)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IFlashFactory
	{
	}

	/// <summary><para><c>IFlashObjectInterface</c> interface.  </para><para>IFlashObjectInterface Interface</para></summary>
	[Guid("D27CDB72-AE6D-11CF-96B8-444553540000")]
	[ComImport]
	[TypeLibType(TypeLibTypeFlags.FDispatchable)]
	[InterfaceType(ComInterfaceType.InterfaceIsIDispatch)]
	public interface IFlashObject
	{
	}

	/// <summary><para><c>IServiceProvider</c> interface.</para></summary>
	[Guid("6D5140C1-7436-11CE-8034-00AA006009FA")]
	[ComImport]
	[TypeLibType((short)0)]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	public interface IServiceProvider
	{
		/// <summary><para><c>RemoteQueryService</c> method of <c>IServiceProvider</c> interface.</para></summary>
		/// <remarks><para>An original IDL definition of <c>RemoteQueryService</c> method was the following:  <c>HRESULT RemoteQueryService ([in] Interop.stdole.GUID* guidService, [in] Interop.stdole.GUID* riid, [out] IUnknown** ppvObject)</c>;</para></remarks>
		void RemoteQueryService ([In] ref GUID guidService, [In] ref GUID riid, [Out, MarshalAs(UnmanagedType.IUnknown)] out object ppvObject);
	}

	/// <summary><para><c>FlashObjectInterface</c> interface.IFlashObjectInterface Interface</para></summary>
	[Guid("D27CDB72-AE6D-11CF-96B8-444553540000")]
	[ComImport]
	[CoClass(typeof(FlashObjectClass))]
	public interface FlashObject: IFlashObject
	{
	}

	/// <summary><para><c>FlashObjectInterfaceClass</c> class.  </para><para>IFlashObjectInterface Interface</para></summary>
	/// <remarks>The following sample shows how to use FlashObjectInterfaceClass class.  You should simply create new class instance and cast it to FlashObjectInterface interface.  After this you can call interface methods and access its properties: <code>
	/// FlashObjectInterface A = (FlashObjectInterface) new FlashObjectInterfaceClass();
	/// A.[method name]();  A.[property name] = [value]; [variable] = A.[property name];
	/// </code></remarks>
	[Guid("D27CDB71-AE6D-11CF-96B8-444553540000")]
	[ComImport]
	[TypeLibType(TypeLibTypeFlags.FCanCreate)]
	[ClassInterface(ClassInterfaceType.None)]
	public class FlashObjectClass // : IFlashObjectInterface, FlashObjectInterface
	{
	}

	/// <summary><para><c>FlashProp</c> interface.Macromedia Flash Player Properties</para></summary>
	//[CoClass(typeof(FlashPropClass))]
	//public interface FlashProp
	//{
	//}

	/// <summary><para><c>FlashPropClass</c> class.  </para><para>Macromedia Flash Player Properties</para></summary>
	/// <remarks>The following sample shows how to use FlashPropClass class.  You should simply create new class instance and cast it to FlashProp interface.  After this you can call interface methods and access its properties: <code>
	/// FlashProp A = (FlashProp) new FlashPropClass();
	/// A.[method name]();  A.[property name] = [value]; [variable] = A.[property name];
	/// </code></remarks>
	// Macromedia Flash Player Properties
	[Guid("1171A62F-05D2-11D1-83FC-00A0C9089C5A")]
	[ComImport]
	[TypeLibType(TypeLibTypeFlags.FCanCreate)]
	[ClassInterface(ClassInterfaceType.None)]
	public class FlashPropClass // : FlashProp
	{
	}

	/// <summary><para><c>ShockwaveFlash</c> interface.Shockwave Flash</para></summary>
	[Guid("D27CDB6C-AE6D-11CF-96B8-444553540000")]
	[ComImport]
	[CoClass(typeof(FlashClass))]
	public interface IFlash: IFlashControl
	{
	}

	/// <summary><para><c>ShockwaveFlashClass</c> class.  </para><para>Shockwave Flash</para></summary>
	/// <remarks>The following sample shows how to use ShockwaveFlashClass class.  You should simply create new class instance and cast it to ShockwaveFlash interface.  After this you can call interface methods and access its properties: <code>
	/// ShockwaveFlash A = (ShockwaveFlash) new ShockwaveFlashClass();
	/// A.[method name]();  A.[property name] = [value]; [variable] = A.[property name];
	/// </code></remarks>
	[Guid("D27CDB6E-AE6D-11CF-96B8-444553540000")]
	[ComImport]
	[TypeLibType(TypeLibTypeFlags.FCanCreate)]
	[ClassInterface(ClassInterfaceType.None)]
	[ComSourceInterfaces("_IShockwaveFlashEvents")]
	public class FlashClass // : IShockwaveFlash, ShockwaveFlash, _IShockwaveFlashEvents_Event
	{
	}
}