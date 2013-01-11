using DataDynamics.PageFX.FlashLand.Swf;

namespace DataDynamics.PageFX.FlashLand.Abc
{
	public sealed class AbcClassCollection : AbcAtomList<AbcClass>
	{
		protected override void WriteCount(SwfWriter writer)
		{
		}

		protected override bool DumpDisabled
		{
			get { return !AbcDumpService.DumpInstances; }
		}

		protected override string DumpElementName
		{
			get { return "classes"; }
		}
	}
}