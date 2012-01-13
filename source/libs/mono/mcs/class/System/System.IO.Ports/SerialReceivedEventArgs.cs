/* -*- Mode: Csharp; tab-width: 8; indent-tabs-mode: t; c-basic-offset: 8 -*- */

#if NET_2_0

namespace System.IO.Ports 
{
	public class SerialDataReceivedEventArgs : EventArgs
	{
		internal SerialDataReceivedEventArgs (SerialData eventType)
		{
			this.eventType = eventType;
		}

		// properties

		public SerialData EventType {
			get {
				return eventType;
			}
		}

		SerialData eventType;
	}
}

#endif
