namespace DataDynamics.PageFX.Flash.Abc
{
	public sealed class AbcMethodBodyCollection : AbcAtomList<AbcMethodBody>
	{
		protected override string DumpElementName
		{
			get { return "method-bodies"; }
		}
	}
}