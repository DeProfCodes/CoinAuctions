using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using CoinAuction.Models;
using Microsoft.AspNetCore.Http;
using CoinAuction.Data;
using Quartz;
using CoinAuction.Tasks;

namespace CoinAuction.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CoinAuctionContext _context;
        private readonly IScheduler _scheduler;

        public HomeController(ILogger<HomeController> logger, CoinAuctionContext context, IScheduler scheduler)
        {
            _logger = logger;
            _context = context;
            _scheduler = scheduler;
        }

        public IActionResult Index()
        {
            CheckAvailability();
            return View();
        }

        public void CheckAvailability()
        {
            ITrigger trigger = TriggerBuilder.Create()
             .WithIdentity($"Check Availability-{DateTime.Now}")
             .StartNow()
             .WithPriority(1)
             .WithSimpleSchedule(x => x.WithIntervalInSeconds(1000).RepeatForever())
             .Build();

            IJobDetail job = JobBuilder.Create<MyJob>()
                         .WithIdentity("Some Unique ID")
                         .Build();

            _scheduler.ScheduleJob(job, trigger);
        }

        public IActionResult Privacy()
        {
            return View();
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
