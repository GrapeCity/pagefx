namespace DataDynamics.PageFX.Common.TypeSystem
{
	public static class EventExtensions
	{
		public static bool IsFlashEvent(this IEvent e)
		{
			return e.HasAttribute("PageFX.EventAttribute");
		}
	}
}