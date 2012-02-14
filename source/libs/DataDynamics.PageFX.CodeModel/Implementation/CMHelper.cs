namespace DataDynamics.PageFX.CodeModel
{
    public static class CMHelper
    {
    	public static bool AreEquals(IType a, IType b)
        {
            if (a == null) return b == null;
            if (b == null) return false;
            if (ReferenceEquals(a, b)) return true;
            if (a.TypeKind != b.TypeKind) return false;
            if (!AreEquals(a.DeclaringType, b.DeclaringType)) return false;
            if (a.DeclaringMethod != b.DeclaringMethod) return false;
            if (a.FullName != b.FullName) return false;
            return true;
        }

        public static int GetHashCode(IType type)
        {
            int h = 0;
            if (type.DeclaringType != null)
                h ^= GetHashCode(type.DeclaringType);
            h ^= type.FullName.GetHashCode();
            return h;
        }
    }
}