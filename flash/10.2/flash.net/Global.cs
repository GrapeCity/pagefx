using System;
using System.Runtime.CompilerServices;

namespace flash.net
{
    [PageFX.GlobalFunctions]
    public partial class Global
    {
        [PageFX.AbcScript(4)]
        [PageFX.AbcScriptTrait(0)]
        [PageFX.ABC]
        [PageFX.QName("registerClassAlias", "flash.net", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void registerClassAlias(Avm.String aliasName, Avm.Class classObject);

        [PageFX.AbcScript(4)]
        [PageFX.AbcScriptTrait(1)]
        [PageFX.ABC]
        [PageFX.QName("getClassByAlias", "flash.net", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.Class getClassByAlias(Avm.String aliasName);

        [PageFX.AbcScript(4)]
        [PageFX.AbcScriptTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("navigateToURL", "flash.net", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void navigateToURL(flash.net.URLRequest request, Avm.String window);

        [PageFX.AbcScript(4)]
        [PageFX.AbcScriptTrait(2)]
        [PageFX.ABC]
        [PageFX.QName("navigateToURL", "flash.net", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void navigateToURL(flash.net.URLRequest request);

        [PageFX.AbcScript(4)]
        [PageFX.AbcScriptTrait(3)]
        [PageFX.ABC]
        [PageFX.QName("sendToURL", "flash.net", "package")]
        [PageFX.FP9]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static void sendToURL(flash.net.URLRequest request);
    }
}
