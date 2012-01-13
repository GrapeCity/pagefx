using System;
using System.Runtime.CompilerServices;

namespace flash.display
{
    /// <summary>
    /// AVM1Movie is a simple class that represents AVM1 movie clips, which use ActionScript 1.0 or 2.0.
    /// (AVM1 is the ActionScript virtual machine used to run ActionScript 1.0 and 2.0.
    /// AVM2 is the ActionScript virtual machine used to run ActionScript 3.0.)
    /// When a Flash Player 8, or older, SWF file is loaded by a Loader object, an AVM1Movie
    /// object is created. The AVM1Movie object can use methods and properties inherited from the
    /// DisplayObject class (such as x , y , width , and so on).
    /// However, no interoperability (such as calling methods or using parameters) between the AVM1Movie object
    /// and AVM2 objects is allowed.
    /// </summary>
    [PageFX.ABC]
    [PageFX.FP9]
    public class AVM1Movie : DisplayObject
    {
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern AVM1Movie();

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual void addCallback(Avm.String arg0, Avm.Function arg1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern virtual object call(Avm.String arg0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call(Avm.String arg0, object rest0);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call(Avm.String arg0, object rest0, object rest1);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call(Avm.String arg0, object rest0, object rest1, object rest2);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call(Avm.String arg0, object rest0, object rest1, object rest2, object rest3);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call(Avm.String arg0, object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call(Avm.String arg0, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call(Avm.String arg0, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call(Avm.String arg0, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call(Avm.String arg0, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern object call(Avm.String arg0, object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);
    }
}
