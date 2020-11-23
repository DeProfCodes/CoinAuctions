using CoinAuction.Data;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinAuction.Tasks
{
    public class AuctionsTask
    {
        private readonly IScheduler _scheduler;
        
        public AuctionsTask(IScheduler scheduler)
        {
            _scheduler = scheduler;
        }

        public void RunAuctionJobs()
        {
            ITrigger trigger = TriggerBuilder.Create()
             .WithIdentity($"Check Availability-{DateTime.Now}")
             .StartNow()
             .WithPriority(1)
             .WithCronSchedule("0 0 8,9,17,18 ? * MON-FRI *")
             .Build();

            IJobDetail job = JobBuilder.Create<AuctionsJob>()
                         .WithIdentity("Some Unique ID")
                         .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

        public void RunCoinsMaturityJobs()
        {
            ITrigger trigger = TriggerBuilder.Create()
             .WithIdentity($"Check Availability-{DateTime.Now}")
             .StartNow()
             .WithPriority(1)
             .WithSimpleSchedule(x => x.WithIntervalInHours(4).RepeatForever())
             .Build();

            IJobDetail job = JobBuilder.Create<CoinsMaturityJob>()
                         .WithIdentity("Some Unique ID")
                         .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

        public void RunContinuosJobs()
        {
            ITrigger trigger = TriggerBuilder.Create()
             .WithIdentity($"Check Availability-{DateTime.Now}")
             .StartNow()
             .WithPriority(1)
             .WithSimpleSchedule(x => x.WithIntervalInSeconds(500).RepeatForever())
             .Build();

            IJobDetail job = JobBuilder.Create<AuctionsJob>()
                         .WithIdentity("Some Unique ID")
                         .Build();

            _scheduler.ScheduleJob(job, trigger);
        }
    }
}
