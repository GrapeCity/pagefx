namespace DataDynamics.PageFX.Ecma335.Translation.ControlFlow
{
    internal static class FlowGraphExtensions
    {
        //   C
        //  / \
        // T   F
        //  \ /
        //   V
        public static bool IsDiamond(this Node condition, Node falseTarget, out Node trueTarget)
        {
            trueTarget = null;
            if (condition == null) return false;
            if (falseTarget == null) return false;

            if (!falseTarget.HasOneOut) return false;

            var e1 = condition.FirstOut;
            if (e1 == null) return false;

            var e2 = e1.NextOut;
            if (e2 == null) return false;

            if (e2.NextOut != null) return false;

            trueTarget = e1.To == falseTarget ? e2.To : e1.To;
            if (!trueTarget.HasOneOut) return false;

            return falseTarget.FirstSuccessor == trueTarget.FirstSuccessor;
        }

        public static bool IsBranchOfTernaryExpression(this Node falseTarget, out Node trueTarget, out Node condition)
        {
            trueTarget = null;
            condition = null;

            if (falseTarget == null) return false;
            if (!falseTarget.HasOneOut) return false;

            foreach (var CE in falseTarget.InEdges)
            {
                condition = CE.From;
                if (condition.IsDiamond(falseTarget, out trueTarget))
                    return true;
            }

            condition = null;
            trueTarget = null;

            return false;
        }
    }
}