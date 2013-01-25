using System;
using System.Runtime.CompilerServices;

namespace flash.automation
{
    [PageFX.AbcInstance(278)]
    [PageFX.ABC]
    [PageFX.FP("10.2")]
    public partial class MouseAutomationAction : flash.automation.AutomationAction
    {
        [PageFX.AbcClassTrait(0)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String MOUSE_DOWN;

        [PageFX.AbcClassTrait(1)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String MOUSE_MOVE;

        [PageFX.AbcClassTrait(2)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String MOUSE_UP;

        [PageFX.AbcClassTrait(3)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String MOUSE_WHEEL;

        [PageFX.AbcClassTrait(4)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String MIDDLE_MOUSE_DOWN;

        [PageFX.AbcClassTrait(5)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String MIDDLE_MOUSE_UP;

        [PageFX.AbcClassTrait(6)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String RIGHT_MOUSE_DOWN;

        [PageFX.AbcClassTrait(7)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        public static Avm.String RIGHT_MOUSE_UP;

        public extern virtual double stageX
        {
            [PageFX.AbcInstanceTrait(1)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(2)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual double stageY
        {
            [PageFX.AbcInstanceTrait(4)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(5)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual int delta
        {
            [PageFX.AbcInstanceTrait(7)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.AbcInstanceTrait(8)]
            [PageFX.ABC]
            [PageFX.FP("10.2")]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern MouseAutomationAction(Avm.String type, double stageX, double stageY, int delta);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern MouseAutomationAction(Avm.String type, double stageX, double stageY);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern MouseAutomationAction(Avm.String type, double stageX);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern MouseAutomationAction(Avm.String type);


    }
}
