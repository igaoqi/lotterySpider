using LotterySpider.Core.Service;
using Topshelf;

namespace LotterySpider
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            AutoMapperConfig.RegisteMaps();
            //JobSchedulerConfig.RegisteJobScheduler();

            HostFactory.Run(x =>
             {
                 x.Service<ServiceRunner>();

                 x.RunAsLocalSystem();

                 x.SetDescription("168彩票网数据采集服务");
                 x.SetDisplayName("168LotterySpider");
                 x.SetServiceName("168LotterySpider");
                 x.EnablePauseAndContinue();
             });
        }
    }
}