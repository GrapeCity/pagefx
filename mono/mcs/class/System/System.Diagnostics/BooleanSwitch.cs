//
// System.Diagnostics.BooleanSwitch.cs
//
// Author:
//      John R. Hicks (angryjohn69@nc.rr.com)
//      Jonathan Pryor (jonpryor@vt.edu)
//
// (C) 2001-2002
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
using System.Globalization;

namespace System.Diagnostics
{
	/// <summary>
	/// Provides a simple on/off switch that controls debugging
	/// and tracing output
	/// </summary>
#if NET_2_0
	[SwitchLevel (typeof (bool))]
#endif
	public class BooleanSwitch : Switch
	{
		/// <summary>
		/// Initializes a new instance
		/// </summary>
		public BooleanSwitch(string displayName, string description)
			: base(displayName, description)
		{
		}

#if NET_2_0
		/// <summary>
		/// Initializes a new instance
		/// </summary>
		public BooleanSwitch(string displayName, string description, string defaultSwitchValue)
			: base(displayName, description, defaultSwitchValue)
		{
		}
#endif

		/// <summary>
		/// Specifies whether the switch is enabled or disabled
		/// </summary>
		public bool Enabled {
			// On .NET, any non-zero value is true.  Only 0 is false.
			get {return SwitchSetting != 0;}
			set {
				SwitchSetting = Convert.ToInt32(value);
			}
		}

#if NET_2_0
		protected override void OnValueChanged ()
		{
			int i;
			if (int.TryParse (Value, out i))
				Enabled = i != 0;
			else
				Enabled = Convert.ToBoolean (Value);
		}
#endif
	}
}

