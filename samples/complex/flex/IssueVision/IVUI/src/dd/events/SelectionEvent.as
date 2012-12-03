// ActionScript file
package dd.events
{
	import flash.events.Event;
	
	public class SelectionEvent extends Event
	{
		public static const AfterSelect:String = "AfterSelect";
		
		private var _selection:Object;
		
		public function SelectionEvent(selection:Object, type:String = AfterSelect):void
		{
			super(type);
			_selection = selection;
		}
		
		public function get Selection():Object
		{
			return _selection;
		}
	}
}
