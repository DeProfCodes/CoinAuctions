using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoinAuction.Data;
using CoinAuction.Models;
using CoinAuction.Helpers;
using CoinAuction.Tasks;
using CoinAuction.ViewModels;
using Microsoft.AspNetCore.Http;

namespace CoinAuction.Controllers
{
    public class AuctionsController : Controller
    {
        private readonly CoinAuctionContext _context;

        public AuctionsController()
        {
            _context = new CoinAuctionContext();
        }

        void SetSessionValues()
        {
            ViewData["role"] = HttpContext.Session.GetString("role");
            ViewData["userId"] = HttpContext.Session.GetString("userId");
        }

        // GET: Auctions
        public async Task<IActionResult> Index()
        {
            SetSessionValues();
            return View(await _context.Auctions.ToListAsync());
        }

        // GET: Auctions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var auction = await _context.Auctions.FirstOrDefaultAsync(m => m.Id == id);
            if (auction == null)
            {
                return NotFound();
            }

            return View(auction);
        }

        // GET: Auctions/Create
        public IActionResult Create()
        {
            SetSessionValues();
            Auction newAuc = new Auction 
            {
                StartTime = DateTime.Now.AddHours(1),
                EndTime = DateTime.Now.AddHours(2),
                TotalPoolCoins = new AuctionExecution().GetPoolCoins() 
            };
            var newAucVm = new AuctionPostViewModel { Auction = newAuc };
            return View(newAucVm);
        }

        // POST: Auctions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AuctionPostViewModel auctionVM)
        {
            SetSessionValues();
            if (ModelState.IsValid)
            {
                DateTime start = Convert.ToDateTime(auctionVM.Auction.StartTime);
                DateTime end = Convert.ToDateTime(auctionVM.Auction.EndTime);

                auctionVM.Auction.TotalPoolCoins = new AuctionExecution().GetPoolCoins();
                
                if (end.CompareTo(start) < 0)
                {
                    auctionVM.EndTimeError = "End time cannot be earlier than start time";
                    return View(auctionVM);
                }

                if (start.CompareTo(DateTime.Now) < 0)
                {
                    auctionVM.StartTimeError = "Choose start time later than now!";
                    return View(auctionVM);
                }

                if(end.CompareTo(DateTime.Now) < 0)
                {
                    auctionVM.EndTimeError = "Choose End time later than now!";
                    return View(auctionVM);
                }
                auctionVM.Auction.Status = EnumTypes.AuctionStatus.Pending.ToString();
                _context.Add(auctionVM.Auction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(auctionVM.Auction);
        }

        // GET: Auctions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            SetSessionValues();
            if (id == null)
            {
                return NotFound();
            }

            var auction = await _context.Auctions.FindAsync(id);
            if (auction == null)
            {
                return NotFound();
            }
            return View(auction);
        }

        // POST: Auctions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,EndTime,TotalPoolCoins,TotalSoldCoins, Status")] Auction auction)
        {
            SetSessionValues();
            if (id != auction.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(auction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AuctionExists(auction.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(auction);
        }

        // GET: Auctions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            SetSessionValues();
            if (id == null)
            {
                return NotFound();
            }

            var auction = await _context.Auctions
                .FirstOrDefaultAsync(m => m.Id == id);
            if (auction == null)
            {
                return NotFound();
            }

            return View(auction);
        }

        // POST: Auctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            _context.Auctions.Remove(auction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AuctionExists(int id)
        {
            return _context.Auctions.Any(e => e.Id == id);
        }

        public async Task<IActionResult> StopAuction(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            auction.Status = EnumTypes.AuctionStatus.Stopped.ToString();

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> StartAuction(int id)
        {
            await new AuctionExecution().ActivateAuction(id);
            return RedirectToAction(nameof(Index));
        }

        public  async Task<IActionResult> StartAuctionNow()
        {
            var activeAuction = await _context.Auctions.FirstOrDefaultAsync(x => x.Status == EnumTypes.AuctionStatus.Active.ToString());
            
            if (activeAuction != null)
                await StopAuction(activeAuction.Id);

            new AuctionExecution().Run();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteAuction(int id)
        {
            var auction = await _context.Auctions.FindAsync(id);
            _context.Auctions.Remove(auction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
