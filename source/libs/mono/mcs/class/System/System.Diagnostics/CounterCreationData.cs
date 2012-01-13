//
// System.Diagnostics.CounterCreationData.cs
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

namespace System.Diagnostics {

	[Serializable]
	[TypeConverter ("System.Diagnostics.Design.CounterCreationDataConverter, " + Consts.AssemblySystem_Design)]
	public class CounterCreationData 
	{

		private string help;
		private string name;
		private PerformanceCounterType type;

		public CounterCreationData ()
		{
		}

		public CounterCreationData (string counterName, 
			string counterHelp, 
			PerformanceCounterType counterType)
		{
			name = counterName;
			help = counterHelp;
			type = counterType;
		}

		[DefaultValue ("")]
		[MonitoringDescription ("Description of this counter.")]
		public string CounterHelp {
			get {return help;}
			set {help = value;}
		}

		[DefaultValue ("")]
		[MonitoringDescription ("Name of this counter.")]
		[TypeConverter ("System.Diagnostics.Design.StringValueConverter, " + Consts.AssemblySystem_Design)]
		public string CounterName 
		{
			get {return name;}
			set {name = value;}
		}

		// may throw InvalidEnumArgumentException
		[DefaultValue (typeof (PerformanceCounterType), "NumberOfItems32")]
		[MonitoringDescription ("Type of this counter.")]
		public PerformanceCounterType CounterType {
			get {return type;}
			set {type = value;}
		}
	}
}

