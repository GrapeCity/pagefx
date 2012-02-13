using System;
using System.Linq;

namespace DataDynamics.PageFX
{
    internal class StatNode : TreeNode
    {
        public readonly TestStat[] Stats = new[]
                                               {
                                                   new TestStat(Runtime.FP10),
                                                   //new TestStat(Runtime.AVM)
                                               };

        public TestStat GetStat(Runtime runtime)
        {
			return Stats.FirstOrDefault(s => s.Runtime == runtime);
        }

        public bool Success
        {
            get { return Stats.All(s => s.Success); }
            set { Array.ForEach(Stats, s => s.Success = value); }
        }

        public void UpdateStats(StatNode node)
        {
            foreach (var stat in Stats)
            {
                var s = node.GetStat(stat.Runtime);
                if (s != null)
                    stat.Update(s);
            }
        }
    }
}