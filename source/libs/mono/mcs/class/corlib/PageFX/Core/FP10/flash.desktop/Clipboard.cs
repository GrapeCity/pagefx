using System;
using System.Runtime.CompilerServices;

namespace flash.desktop
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class Clipboard : Avm.Object
    {
        public extern virtual Avm.Array formats
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        public extern static Clipboard generalClipboard
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern Clipboard();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void clear();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void clearData(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object getData(Avm.String arg0, Avm.String arg1);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object getData(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool setData(Avm.String arg0, object arg1, bool arg2);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool setData(Avm.String arg0, object arg1);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool hasFormat(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual bool setDataHandler(Avm.String arg0, Avm.Function arg1, bool arg2);

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern bool setDataHandler(Avm.String arg0, Avm.Function arg1);


    }
}
