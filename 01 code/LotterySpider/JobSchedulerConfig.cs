using LotterySpider.Core.Jobs.Impls;
using LotterySpider.Core.Jobs.Impls.JingWaiCai;
using Quartz;
using Quartz.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LotterySpider
{
    public class JobSchedulerConfig
    {
        private static readonly IScheduler scheduler = StdSchedulerFactory.GetDefaultScheduler();

        public static void RegisteJobScheduler()
        {
            RegisteAzxy5();
            RegisteAzxy8();
            RegisteAzxy10();
            RegisteAzxy20();
            RegisteTwbg();
        }

        private static void RegisteAzxy5()
        {
            IJobDetail job = JobBuilder.Create<Azxy5Job>()
                .WithIdentity("Azxy5Job")
                .WithDescription("Azxy5Job")
                .Build();

            ITrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .StartAt(DateBuilder.DateOf(0, 3, 40))
                .WithIdentity("Azxy5Trigger")
                .WithSimpleSchedule(p => p
                .WithIntervalInMinutes(5)
                .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        private static void RegisteAzxy8()
        {
            IJobDetail job = JobBuilder.Create<Azxy8Job>()
                .WithIdentity("Azxy8Job")
                .WithDescription("Azxy8Job")
                .Build();

            ITrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .StartAt(DateBuilder.DateOf(0, 3, 40))
                .WithIdentity("Azxy8Trigger")
                .WithSimpleSchedule(p => p
                .WithIntervalInMinutes(5)
                .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        private static void RegisteAzxy10()
        {
            IJobDetail job = JobBuilder.Create<Azxy8Job>()
                .WithIdentity("Azxy10Job")
                .WithDescription("Azxy10Job")
                .Build();

            ITrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .StartAt(DateBuilder.DateOf(0, 3, 40))
                .WithIdentity("Azxy10Trigger")
                .WithSimpleSchedule(p => p
                .WithIntervalInMinutes(5)
                .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        private static void RegisteAzxy20()
        {
            IJobDetail job = JobBuilder.Create<Azxy20Job>()
                .WithIdentity("Azxy20Job")
                .WithDescription("Azxy20Job")
                .Build();

            ITrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .StartAt(DateBuilder.DateOf(0, 3, 46))
                .WithIdentity("Azxy20Trigger")
                .WithSimpleSchedule(p => p
                .WithIntervalInMinutes(5)
                .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }

        private static void RegisteTwbg()
        {
            IJobDetail job = JobBuilder.Create<TwbgJob>()
                .WithIdentity("TwbgJob")
                .WithDescription("TwbgJob")
                .Build();

            ITrigger trigger = (ISimpleTrigger)TriggerBuilder.Create()
                .StartAt(DateBuilder.DateOf(0, 7, 53))
                .WithIdentity("TwbgTrigger")
                .WithSimpleSchedule(p => p
                .WithIntervalInMinutes(5)
                .RepeatForever())
                .Build();

            scheduler.ScheduleJob(job, trigger);
        }
    }
}