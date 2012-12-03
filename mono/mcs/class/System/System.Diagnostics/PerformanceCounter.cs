//
// System.Diagnostics.PerformanceCounter.cs
//
// Authors:
//   Jonathan Pryor (jonpryor@vt.edu)
//   Andreas Nahr (ClassDevelopment@A-SoftTech.com)
//
// (C) 2002
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
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Diagnostics;
#if NET_2_0
using System.Runtime.ConstrainedExecution;
#endif

namespace System.Diagnostics {

	// must be safe for multithreaded operations
#if !NET_2_0
	[Designer ("Microsoft.VisualStudio.Install.PerformanceCounterDesigner, " + Consts.AssemblyMicrosoft_VisualStudio)]
#endif
	[InstallerType (typeof (PerformanceCounterInstaller))]
	public sealed class PerformanceCounter : Component, ISupportInitialize 
	{

		private string categoryName;
		private string counterName;
		private string instanceName;
		private string machineName;
		private bool readOnly;
#if NET_2_0
		private PerformanceCounterInstanceLifetime lifetime;
#endif

		public static int DefaultFileMappingSize = 524288;

		// set catname, countname, instname to "", machname to "."
		public PerformanceCounter ()
		{
			categoryName = counterName = instanceName = "";
			machineName = ".";
		}

		// throws: InvalidOperationException (if catName or countName
		// is ""); ArgumentNullException if either is null
		// sets instName to "", machname to "."
		public PerformanceCounter (String categoryName, 
			string counterName)
			: this (categoryName, counterName, false)
		{
		}

		public PerformanceCounter (string categoryName, 
			string counterName,
			bool readOnly)
			: this (categoryName, counterName, "", readOnly)
		{
		}

		public PerformanceCounter (string categoryName,
			string counterName,
			string instanceName)
			: this (categoryName, counterName, instanceName, false)
		{
		}

		public PerformanceCounter (string categoryName,
			string counterName,
			string instanceName,
			bool readOnly)
		{

			CategoryName = categoryName;
			CounterName = counterName;

			if (categoryName == "" || counterName == "")
				throw new InvalidOperationException ();

			InstanceName = instanceName;
			this.instanceName = instanceName;
			this.machineName = ".";
			this.readOnly = readOnly;
		}

		public PerformanceCounter (string categoryName,
			string counterName,
			string instanceName,
			string machineName)
			: this (categoryName, counterName, instanceName, false)
		{
			this.machineName = machineName;
		}

		// may throw ArgumentNullException
		[DefaultValue (""), ReadOnly (true), RecommendedAsConfigurable (true)]
		[TypeConverter ("System.Diagnostics.Design.CategoryValueConverter, " + Consts.AssemblySystem_Design)]
		[SRDescription ("The category name for this performance counter.")]
		public string CategoryName {
			get {return categoryName;}
			set {
				if (value == null)
					throw new ArgumentNullException ("categoryName");
				categoryName = value;
			}
		}

		// may throw InvalidOperationException
		[MonoTODO]
		[ReadOnly (true), DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription ("A description describing the counter.")]
		public string CounterHelp {
			get {return "";}
		}

		// may throw ArgumentNullException
		[DefaultValue (""), ReadOnly (true), RecommendedAsConfigurable (true)]
		[TypeConverter ("System.Diagnostics.Design.CounterNameConverter, " + Consts.AssemblySystem_Design)]
		[SRDescription ("The name of this performance counter.")]
		public string CounterName 
			{
			get {return counterName;}
			set {
				if (value == null)
					throw new ArgumentNullException ("counterName");
				counterName = value;
			}
		}

		// may throw InvalidOperationException
		[MonoTODO]
		[DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription ("The type of the counter.")]
		public PerformanceCounterType CounterType {
			get {return 0;}
		}

#if NET_2_0
		[MonoTODO]
		[DefaultValue (PerformanceCounterInstanceLifetime.Global)]
		public PerformanceCounterInstanceLifetime InstanceLifetime {
			get { return lifetime; }
			set { lifetime = value; }
		}
#endif

		[DefaultValue (""), ReadOnly (true), RecommendedAsConfigurable (true)]
		[TypeConverter ("System.Diagnostics.Design.InstanceNameConverter, " + Consts.AssemblySystem_Design)]
		[SRDescription ("The instance name for this performance counter.")]
		public string InstanceName 
			{
			get {return instanceName;}
			set {instanceName = value;}
		}

		// may throw ArgumentException if machine name format is wrong
		[MonoTODO("What's the machine name format?")]
		[DefaultValue ("."), Browsable (false), RecommendedAsConfigurable (true)]
		[SRDescription ("The machine where this performance counter resides.")]
		public string MachineName {
			get {return machineName;}
			set {machineName = value;}
		}

		// may throw InvalidOperationException, Win32Exception
		[MonoTODO]
		[Browsable (false), DesignerSerializationVisibility (DesignerSerializationVisibility.Hidden)]
		[MonitoringDescription ("The raw value of the counter.")]
		public long RawValue {
			get {return 0;}
			set {
				throw new NotImplementedException ();
			}
		}

		[Browsable (false), DefaultValue (true)]
		[MonitoringDescription ("The accessability level of the counter.")]
		public bool ReadOnly {
			get {return readOnly;}
			set {readOnly = value;}
		}

		[MonoTODO]
		public void BeginInit ()
		{
			throw new NotImplementedException ();
		}

		[MonoTODO]
		public void Close ()
		{
			throw new NotImplementedException ();
		}

		[MonoTODO]
		public static void CloseSharedResources ()
		{
			throw new NotImplementedException ();
		}

		// may throw InvalidOperationException, Win32Exception
		[MonoTODO]
		public long Decrement ()
		{
			throw new NotImplementedException ();
		}

		[MonoTODO]
		protected override void Dispose (bool disposing)
		{
			throw new NotImplementedException ();
		}

		[MonoTODO]
		public void EndInit ()
		{
			throw new NotImplementedException ();
		}

		// may throw InvalidOperationException, Win32Exception
		[MonoTODO]
		public long Increment ()
		{
			throw new NotImplementedException ();
		}

		// may throw InvalidOperationException, Win32Exception
		[MonoTODO]
#if NET_2_0
		[ReliabilityContract (Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
		public long IncrementBy (long value)
		{
			throw new NotImplementedException ();
		}

		// may throw InvalidOperationException, Win32Exception
		[MonoTODO]
		public CounterSample NextSample ()
		{
			throw new NotImplementedException ();
		}

		// may throw InvalidOperationException, Win32Exception
		[MonoTODO]
		public float NextValue ()
		{
			throw new NotImplementedException ();
		}

		// may throw InvalidOperationException, Win32Exception
		[MonoTODO]
#if NET_2_0
		[ReliabilityContract (Consistency.WillNotCorruptState, Cer.MayFail)]
#endif
		public void RemoveInstance ()
		{
			throw new NotImplementedException ();
		}
	}
}

