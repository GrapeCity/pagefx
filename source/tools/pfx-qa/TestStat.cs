using System.Xml;

namespace DataDynamics.PageFX
{
    class TestStat
    {
        public TestStat(Runtime runtime)
        {
            Runtime = runtime;
        }

        public Runtime Runtime;
        public bool Success = true;

        public int TotalPassed;
        public int TotalFailed;
        
        public double Time;

        public string RuntimeImage
        {
            get { return Images.Get(Runtime); }
        }

        public int Total
        {
            get { return TotalPassed + TotalFailed; }
        }

        public string StatusImage
        {
            get 
            {
                if (Success) return Images.Passed;
                return Images.Failed;
            }
        }

        public void Update(TestStat stat)
        {
            if (stat == null) return;
            Time += stat.Time;
            if (stat.Success)
            {
                TotalPassed++;
            }
            else
            {
                TotalFailed++;
                Success = false;
            }
        }

        public void Update(TestNode test)
        {
            Update(test.GetStat(Runtime));
        }

        public void Write(XmlWriter writer, bool runtime, bool isSuite, bool withTime)
        {
            if (runtime && (!isSuite || Total != 0))
                Html.IMG(writer, RuntimeImage, null);

            if (isSuite)
            {
                if (TotalPassed != 0)
                {
                    Html.IMG(writer, Images.PassedSmall, "Total Passed");
                    writer.WriteString(": " + TotalPassed);
                }
                if (TotalFailed != 0)
                {
                    Html.IMG(writer, Images.FailedSmall, "Total Failed");
                    writer.WriteString(": " + TotalFailed);
                }
            }
            else
            {
                if (Success) Html.IMG(writer, Images.PassedSmall, "Passed");
                else Html.IMG(writer, Images.FailedSmall, "Failed");
            }
            if (withTime)
            {
                Html.IMG(writer, Images.Time, "Total Duration");
                writer.WriteString(string.Format(": {0:0.##}", Time));
            }
        }
    }
}