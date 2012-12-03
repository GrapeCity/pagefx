using mx.core;
using DataDynamics.UI;

class App : Application
{
	public App()
	{
        TestToolbar tb = new TestToolbar();
        tb.percentWidth = 100;
        addChild(tb);

		//OneIssuePane pane = new OneIssuePane();
		//pane.setStyle("left", 10);
		//pane.setStyle("right", 10);
		//pane.setStyle("top", 10);
		//pane.setStyle("bottom", 10);
		//addChild(pane);
	}
}