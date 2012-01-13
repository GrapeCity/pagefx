using System.Diagnostics;
using DataDynamics.PageFX.CodeModel;

namespace DataDynamics.PageFX.FLI
{
    internal partial class AbcGenerator
    {
        public void CheckEmbedAsset(IField field)
        {
            Debug.Assert(AvmHelper.HasEmbeddedAsset(field));

            if (!field.IsStatic)
                throw Errors.EmbedAsset.FieldIsNotStatic.CreateException(field.Name);

            if (!IsSwf)
                throw Errors.EmbedAsset.NotFlashRuntime.CreateException();
        }

        int PlayerVersion
        {
            get 
            {
                if (sfc != null)
                    return sfc.PlayerVersion;
                return -1;
            }
        }

        public void CheckApiCompatibility(ITypeMember m)
        {
            if (m == null) return;
            if (!IsSwf) return;

            int v = MethodHelper.GetPlayerVersion(m);
            if (v < 0) return;

            if (v > PlayerVersion)
            {
                var method = m as IMethod;
                if (method != null)
                {
                    CompilerReport.Add(Errors.ABC.IncompatibleCall, NameUtil.GetFullName(method), v);
                    return;
                }

                var f = m as IField;
                if (f != null)
                {
                    CompilerReport.Add(Errors.ABC.IncompatibleField, NameUtil.GetFullName(f), v);
                    return;
                }
            }
        }
    }
}