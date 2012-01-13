﻿using System;
using Microsoft.VisualStudio.Debugger.Interop;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    // This class represents a succesfully parsed expression to the debugger. 
    // It is returned as a result of a successful call to IDebugExpressionContext2.ParseText
    // It allows the debugger to obtain the values of an expression in the debuggee. 
    // For the purposes of this sample, this means obtaining the values of locals and parameters from a stack frame.
    class Expression : IDebugExpression2
    {
        Property m_property;
        readonly string m_code;
        readonly StackFrame m_frame;

        public Expression(StackFrame frame, string code)
        {
            m_frame = frame;
            m_code = code;
        }

        public Expression(Property property)
        {
            m_property = property;
        }

        #region IDebugExpression2 Members
        // This method cancels asynchronous expression evaluation as started by a call to the IDebugExpression2::EvaluateAsync method.
        int IDebugExpression2.Abort()
        {
            throw new NotImplementedException();
        }

        // This method evaluates the expression asynchronously.
        // This method should return immediately after it has started the expression evaluation. 
        // When the expression is successfully evaluated, an IDebugExpressionEvaluationCompleteEvent2 
        // must be sent to the IDebugEventCallback2 event callback
        //
        // This is primarily used for the immediate window which this engine does not currently support.
        int IDebugExpression2.EvaluateAsync(uint dwFlags, IDebugEventCallback2 pExprCallback)
        {
            throw new NotImplementedException();
        }

        void Eval()
        {
            if (m_property != null) return;
            m_property = m_frame.Engine.EvalExpression(m_frame, m_code);
        }

        // This method evaluates the expression synchronously.
        int IDebugExpression2.EvaluateSync(uint dwFlags, uint dwTimeout, 
            IDebugEventCallback2 pExprCallback,
            out IDebugProperty2 ppResult)
        {
            Eval();
            ppResult = m_property;
            return Const.S_OK;
        }
        #endregion
    }
}