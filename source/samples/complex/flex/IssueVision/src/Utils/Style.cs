using System;
using mx.styles;

namespace IssueVision
{
    public class Style
    {
        private static string ToString(Constraint c)
        {
            switch (c)
            {
                case Constraint.Left:
                    return "left";
                case Constraint.Right:
                    return "right";
                case Constraint.Top:
                    return "top";
                case Constraint.Bottom:
                    return "bottom";
                case Constraint.HorrizontalCenter:
                    return "horizontalCenter";
                case Constraint.VerticalCenter:
                    return "verticalCenter";
                default:
                    return "";
            }
        }

        private static void SetConstraint(IStyleClient target, Constraint c, Constraint v, int value)
        {
            if ((c & v) != 0)
                target.setStyle(ToString(v), value);
        }

        public static void SetConstraints(IStyleClient target, Constraint c, int value)
        {
            SetConstraint(target, c, Constraint.Left, value);
            SetConstraint(target, c, Constraint.Right, value);
            SetConstraint(target, c, Constraint.Top, value);
            SetConstraint(target, c, Constraint.Bottom, value);
            SetConstraint(target, c, Constraint.HorrizontalCenter, value);
            SetConstraint(target, c, Constraint.VerticalCenter, value);
        }
    }

    [Flags]
    public enum Constraint
    {
        Left = 0x01,
        Right = 0x02,
        Top = 0x04,
        Bottom = 0x08,
        HorrizontalCenter = 0x10,
        VerticalCenter = 0x20,

        //Combinations
        LR = Left | Right,
        TB = Top | Bottom,
        LRTB = Left | Right | Top | Bottom,
        LRT = Left | Right | Top,
        LRB = Left | Right | Bottom,
    }
}