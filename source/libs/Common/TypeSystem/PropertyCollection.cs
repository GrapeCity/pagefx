namespace DataDynamics.PageFX.Common.TypeSystem
{
    public sealed class PropertyCollection : MultiMemberCollection<IProperty>, IPropertyCollection
    {
        public override string ToString()
        {
            return ToString(null, null);
        }
    }
}