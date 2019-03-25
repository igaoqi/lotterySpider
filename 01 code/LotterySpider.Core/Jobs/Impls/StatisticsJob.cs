using LotterySpider.Common.Utils;
using LotterySpider.Core.Statistics;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace LotterySpider.Core.Jobs.Impls
{
    public sealed class StatisticsJob : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            Task.Run(() => GoAnalysis());
        }

        private void GoAnalysis()
        {
            while (true)
            {
                if (QueueHelper<IStatisticsProvider>.Instance.Count > 0)
                {
                    var t = QueueHelper<IStatisticsProvider>.Instance.Dequeue();
                    t.GoStatistics();
                }
                Thread.Sleep(5000);
            }
        }
    }
}