namespace System.Diagnostics
{
    internal class AvmTraceListener : TraceListener
    {
        public override void Write(string message)
        {
            if (string.IsNullOrEmpty(message))
                avm.trace("");
            else
                avm.trace(message);
        }

        public override void WriteLine(string message)
        {
            if (string.IsNullOrEmpty(message))
                avm.trace("");
            else
                avm.trace(message);
        }
    }
}