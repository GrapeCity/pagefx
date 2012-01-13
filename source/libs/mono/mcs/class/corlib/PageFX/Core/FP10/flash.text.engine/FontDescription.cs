using System;
using System.Runtime.CompilerServices;

namespace flash.text.engine
{
    [PageFX.ABC]
    [PageFX.FP10]
    public class FontDescription : Avm.Object
    {
        public extern virtual Avm.String fontLookup
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String fontWeight
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual bool locked
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String renderingMode
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String cffHinting
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String fontPosture
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        public extern virtual Avm.String fontName
        {
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            get;
            [PageFX.ABC]
            [PageFX.FP10]
            [MethodImpl(MethodImplOptions.InternalCall)]
            set;
        }

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontDescription(Avm.String arg0, Avm.String arg1, Avm.String arg2, Avm.String arg3, Avm.String arg4, Avm.String arg5);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontDescription(Avm.String arg0, Avm.String arg1, Avm.String arg2, Avm.String arg3, Avm.String arg4);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontDescription(Avm.String arg0, Avm.String arg1, Avm.String arg2, Avm.String arg3);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontDescription(Avm.String arg0, Avm.String arg1, Avm.String arg2);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontDescription(Avm.String arg0, Avm.String arg1);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontDescription(Avm.String arg0);

        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern FontDescription();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual FontDescription clone();

        [PageFX.ABC]
        [PageFX.FP10]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static bool isFontCompatible(Avm.String arg0, Avm.String arg1, Avm.String arg2);
    }
}
