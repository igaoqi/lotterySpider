using Quartz;
using System;

namespace LotterySpider.Core.Jobs
{
    public interface IJobProvider<T> : IJob
    {
        DateTime JobDate { get; }
        string CacheKey { get; }
        string Url { get; }

        void Insert(T t);

        void Filter(ref T t);
    }
}