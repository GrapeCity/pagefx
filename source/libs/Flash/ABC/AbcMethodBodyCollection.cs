namespace DataDynamics.PageFX.FlashLand.Abc
{
	public sealed class AbcMethodBodyCollection : AbcAtomList<AbcMethodBody>
	{
		protected override string DumpElementName
		{
			get { return "method-bodies"; }
		}
	}
}