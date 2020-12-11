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

        private bool auctionTimerStarted = false;
        public HomeController(ILogger<HomeController> logger, IScheduler scheduler)
        {
            _logger = logger;
            _context = new CoinAuctionContext();
            _scheduler = scheduler;
        }

        public IActionResult Index()
        {
            ViewData["role"] = HttpContext.Session.GetString("role");
            ViewData["userId"] = HttpContext.Session.GetString("userId");

            if (!auctionTimerStarted)
            {
                StartAuctionTimer();
                auctionTimerStarted = true;
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        private void StartAuctionTimer()
        {
            AuctionsTask auction = new AuctionsTask(_scheduler);
            auction.RunContinuosJobs();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
