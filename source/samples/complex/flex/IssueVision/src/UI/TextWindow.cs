using flash.events;
using mx.containers;
using mx.controls;
using mx.core;
using mx.managers;

namespace IssueVision
{
    public static class TextWindow
    {
        public static void Show(string title, string text)
        {
            TitleWindow form = new TitleWindow();
            form.title = title;
            form.width = 600;
            form.height = 600;

            Canvas canvas = new Canvas();
            canvas.percentWidth = 100;
            canvas.percentHeight = 100;

            TextArea area = new TextArea();
            Style.SetConstraints(area, Constraint.LRT, 10);
            Style.SetConstraints(area, Constraint.Bottom, 40);
            area.text = text;
            canvas.addChild(area);

            Button ok = new Button();
            ok.label = "OK";
            Style.SetConstraints(ok, Constraint.HorrizontalCenter, 0);
            Style.SetConstraints(ok, Constraint.Bottom, 10);

            PopUpRemover r = new PopUpRemover();
            r.popUp = form;
            ok.click += r.OnOK;
            canvas.addChild(ok);

            form.addChild(canvas);

            PopUpManager.addPopUp(form, App.Instance);
            PopUpManager.centerPopUp(form);
        }

        private class PopUpRemover
        {
            public IFlexDisplayObject popUp;

            public void OnOK(MouseEvent e)
            {
                if (popUp != null)
                    PopUpManager.removePopUp(popUp);
            }
        }
    }
}