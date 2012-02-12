using System;

namespace DataDynamics
{
    public delegate bool CancelCallback();

    public class CancelException : Exception
    {
        public CancelException() : base("Operation was cancelled")
        {
        }

        public CancelException(string message) : base(message)
        {
        }
    }

    public delegate TResult Func<TResult>();
    public delegate TResult Func<TResult, T>(T arg);
    public delegate TResult Func<TResult, T1, T2>(T1 v1, T2 v2);
    public delegate TResult Func<TResult, T1, T2, T3>(T1 v1, T2 v2, T3 v3);
    public delegate TResult Func<TResult, T1, T2, T3, T4>(T1 v1, T2 v2, T3 v3, T4 v4);
    public delegate TResult Func<TResult, T1, T2, T3, T4, T5>(T1 v1, T2 v2, T3 v3, T4 v4, T5 v5);
}