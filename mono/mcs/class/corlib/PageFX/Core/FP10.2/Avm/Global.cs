using System;
using System.Runtime.CompilerServices;

namespace Avm
{
    [PageFX.GlobalFunctions]
    public class Global
    {
        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(12)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static bool bugzilla(int n);

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(13)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String decodeURI(Avm.String uri);

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(13)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String decodeURI();

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(14)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String decodeURIComponent(Avm.String uri);

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(14)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String decodeURIComponent();

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(15)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String encodeURI(Avm.String uri);

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(15)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String encodeURI();

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(16)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String encodeURIComponent(Avm.String uri);

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(16)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String encodeURIComponent();

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(17)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static bool isNaN(double n);

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(17)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static bool isNaN();

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(18)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static bool isFinite(double n);

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(18)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static bool isFinite();

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(19)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double parseInt(Avm.String s, int radix);

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(19)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double parseInt(Avm.String s);

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(19)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double parseInt();

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(20)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double parseFloat(Avm.String str);

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(20)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static double parseFloat();

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(21)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String escape(Avm.String s);

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(21)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String escape();

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(22)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String unescape(Avm.String s);

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(22)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String unescape();

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(23)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static bool isXMLName(object str);

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(23)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static bool isXMLName();

        [PageFX.AbcScript(115)]
        [PageFX.AbcScriptTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void trace();

        [PageFX.AbcScript(115)]
        [PageFX.AbcScriptTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void trace(object rest0);

        [PageFX.AbcScript(115)]
        [PageFX.AbcScriptTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void trace(object rest0, object rest1);

        [PageFX.AbcScript(115)]
        [PageFX.AbcScriptTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void trace(object rest0, object rest1, object rest2);

        [PageFX.AbcScript(115)]
        [PageFX.AbcScriptTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void trace(object rest0, object rest1, object rest2, object rest3);

        [PageFX.AbcScript(115)]
        [PageFX.AbcScriptTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void trace(object rest0, object rest1, object rest2, object rest3, object rest4);

        [PageFX.AbcScript(115)]
        [PageFX.AbcScriptTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void trace(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5);

        [PageFX.AbcScript(115)]
        [PageFX.AbcScriptTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void trace(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6);

        [PageFX.AbcScript(115)]
        [PageFX.AbcScriptTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void trace(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7);

        [PageFX.AbcScript(115)]
        [PageFX.AbcScriptTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void trace(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8);

        [PageFX.AbcScript(115)]
        [PageFX.AbcScriptTrait(8)]
        [PageFX.ABC]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void trace(object rest0, object rest1, object rest2, object rest3, object rest4, object rest5, object rest6, object rest7, object rest8, object rest9);

        [PageFX.AbcScript(115)]
        [PageFX.AbcScriptTrait(9)]
        [PageFX.ABC]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static bool watson(int n);
    }
}
