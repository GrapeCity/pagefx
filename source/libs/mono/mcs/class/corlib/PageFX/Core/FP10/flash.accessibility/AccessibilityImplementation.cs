using System;
using System.Runtime.CompilerServices;

namespace flash.accessibility
{
    [PageFX.ABC]
    [PageFX.FP9]
    public class AccessibilityImplementation : Avm.Object
    {
        [PageFX.ABC]
        [PageFX.FP9]
        public uint errno;

        [PageFX.ABC]
        [PageFX.FP9]
        public bool stub;

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern AccessibilityImplementation();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool isLabeledBy(flash.geom.Rectangle arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String get_accName(uint arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint get_accFocus();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String get_accValue(uint arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void accDoDefaultAction(uint arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Array get_accSelection();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint get_accRole(uint arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object accLocation(uint arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.Array getChildIDArray();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual uint get_accState(uint arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void accSelect(uint arg0, uint arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual Avm.String get_accDefaultAction(uint arg0);
    }
}
