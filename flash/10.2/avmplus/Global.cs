using System;
using System.Runtime.CompilerServices;

namespace avmplus
{
    [PageFX.GlobalFunctions]
    public partial class Global
    {
        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(61)]
        [PageFX.ABC]
        [PageFX.QName("describeType", "avmplus", "package")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.XML describeType(object value, uint flags);

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(62)]
        [PageFX.ABC]
        [PageFX.QName("getQualifiedClassName", "avmplus", "package")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String getQualifiedClassName(object value);

        [PageFX.AbcScript(0)]
        [PageFX.AbcScriptTrait(63)]
        [PageFX.ABC]
        [PageFX.QName("getQualifiedSuperclassName", "avmplus", "package")]
        [PageFX.FP("10.2")]
        [MethodImpl(MethodImplOptions.InternalCall)]
        public extern static Avm.String getQualifiedSuperclassName(object value);
    }
}
