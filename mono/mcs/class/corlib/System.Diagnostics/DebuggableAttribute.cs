//CHANGED

//
// System.Collections.DebuggableAttribute.cs
//
// Author:
//   Nick Drochak II (ndrochak@gol.com)
//   Ben S. Stahlhood II (bstahlhood@gmail.com)
//
// (C) 2001 Nick Drochak II
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

namespace System.Diagnostics
{
    [AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Module)]
    public sealed class DebuggableAttribute : Attribute
    {
        private readonly bool JITTrackingEnabledFlag;
        private readonly bool JITOptimizerDisabledFlag;

#if NET_2_0
		[Flags]
		public enum DebuggingModes
        {
			// Fields
			None = 0,
			Default = 1,
			IgnoreSymbolStoreSequencePoints = 2,
			EnableEditAndContinue = 4,
			DisableOptimizations = 256
		}

		private readonly DebuggingModes debuggingModes = DebuggingModes.None;

		public DebuggingModes DebuggingFlags 
		{
			get {
				throw new NotImplementedException ();
			}
		}
#endif

        // Public Instance Constructors
        public DebuggableAttribute(bool isJITTrackingEnabled, bool isJITOptimizerDisabled)
        {
            JITTrackingEnabledFlag = isJITTrackingEnabled;
            JITOptimizerDisabledFlag = isJITOptimizerDisabled;

#if NET_2_0
			if (isJITTrackingEnabled) 
				debuggingModes |= DebuggingModes.Default;
			
			if (isJITOptimizerDisabled) 
                               debuggingModes |= DebuggingModes.DisableOptimizations;
#endif
        }

#if NET_2_0
		public DebuggableAttribute(DebuggingModes modes) 
		{
			debuggingModes = modes;
			JITTrackingEnabledFlag = (debuggingModes & DebuggingModes.Default) != 0;
			JITOptimizerDisabledFlag = (debuggingModes & DebuggingModes.DisableOptimizations) != 0;
		}
#endif

        // Public Instance Properties
        internal bool IsJITTrackingEnabled
        {
            get { return JITTrackingEnabledFlag; }
        }

        internal bool IsJITOptimizerDisabled
        {
            get { return JITOptimizerDisabledFlag; }
        }
    }
}
