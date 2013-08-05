namespace DataDynamics.PageFX.FlashLand.Abc
{
	public sealed class AbcExceptionHandlerCollection : AbcAtomList<AbcExceptionHandler>
	{
		public void Sort()
		{
			Sort((x, y)=>
				{
					int c = x.Target - y.Target;
					if (c != 0) return c;

					c = x.From - y.From;
					if (c != 0) return c;

					return 0;
				});

			for (int i = 0; i < Count; i++)
			{
				this[i].Index = i;
			}
		}

		protected override string DumpElementName
		{
			get { return "exceptions"; }
		}
	}
}