namespace System.Diagnostics
{
    internal class AvmTraceListener : TraceListener
    {
        public override void Write(string message)
        {
            if (message == null || message.Length == 0)
                avm.trace(avm.EmptyString);
            else
                avm.trace(message);
        }

        public override void WriteLine(string message)
        {
            if (message == null || message.Length == 0)
                avm.trace(avm.EmptyString);
            else
                avm.trace(message);
        }
    }
}