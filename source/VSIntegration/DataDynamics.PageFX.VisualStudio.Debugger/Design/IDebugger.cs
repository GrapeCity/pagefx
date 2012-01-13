using Microsoft.VisualStudio.Debugger.Interop;

namespace DataDynamics.PageFX.VisualStudio.Debugger
{
    interface IDebugger
    {
        int Start(string apppath);

        void Stop();

        void Pause();

        void Continue();

        int Step(enum_STEPKIND kind, enum_STEPUNIT unit);

        bool BindBreakpoint(PendingBreakpoint bp);

        bool RemoveBreakpoint(PendingBreakpoint bp);

        bool EnableBreakpoint(PendingBreakpoint bp, bool value);

        void EvalChildProperties(Property prop);

        Property EvalExpression(StackFrame frame, string code);

        bool SetCondition(PendingBreakpoint bp);
    }
}