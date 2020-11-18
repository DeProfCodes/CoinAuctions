using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static CoinAuction.Controllers.HomeController;

namespace CoinAuction.Tasks
{
    public class MyJob : IJob
    {
        public Task Execute(IJobExecutionContext context)
        {
            // do something here
            Console.WriteLine("Job done");
            SetTimer();
            return Task.FromResult(0);
        }

        private void SetTimer()
        {
            // Get today's date and time
            var countDownDate = new DateTime(2020,11,18,22,0,0);
            var now = DateTime.Now;

            TimeSpan t = countDownDate - now;
            //sViewData["d"] = "";      
        }
    }
}
