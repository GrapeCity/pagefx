namespace DataDynamics.PageFX.FlashLand.Abc
{
	public sealed class AbcMemberName
	{
		public AbcMemberName(AbcMultiname type, AbcMultiname name)
		{
			Type = type;
			Name = name;
		}

		public AbcMultiname Type { get; private set; }

		public AbcMultiname Name { get; private set; }
	}
}