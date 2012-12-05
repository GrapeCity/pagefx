using System.Drawing;
using System.Windows.Forms;

namespace DataDynamics.UI
{
    public class ThumbnailFlowLayoutPanel : FlowLayoutPanel
    {
        protected override Point ScrollToControl(Control activeControl)
        {
            return AutoScrollPosition;
        }
    }
}
