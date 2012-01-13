//
// ExecutionScope.cs
//
// Authors:
//	Marek Safar  <marek.safar@gmail.com>
//	Jb Evain  <jbevain@novell.com>
//
// Copyright (C) 2007 - 2008, Novell, Inc (http://www.novell.com)
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
using System.Linq.Expressions;

namespace System.Runtime.CompilerServices {

	public class ExecutionScope {

		public object [] Globals;
		public object [] Locals;
		public ExecutionScope Parent;

		internal CompilationContext context;

		internal ExecutionScope (CompilationContext context)
		{
			this.context = context;
			this.Globals = context.GetGlobals ();
		}

		internal ExecutionScope (CompilationContext context, ExecutionScope parent, object [] locals)
			: this (context)
		{
			this.Parent = parent;
			this.Locals = locals;
		}

#if NOT_PFX
		public Delegate CreateDelegate (int indexLambda, object [] locals)
		{
			return context.CreateDelegate (indexLambda, new ExecutionScope (context, this, locals));
		}
#endif

		public object [] CreateHoistedLocals ()
		{
			throw new NotSupportedException ();
		}

		public Expression IsolateExpression (Expression expression, object [] locals)
		{
			throw new NotSupportedException ();
		}
	}
}
