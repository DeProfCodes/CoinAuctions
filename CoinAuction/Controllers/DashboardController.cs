using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinAuction.Data;
using CoinAuction.Helpers;
using CoinAuction.Models;
using CoinAuction.Tasks;
using CoinAuction.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace CoinAuction.Controllers
{
    public class DashboardController : Controller
    {
        private readonly CoinAuctionContext _context;

        public DashboardController()
        {
            _context = new CoinAuctionContext();
        }

        void SetSessionValues()
        {
            ViewData["role"] = HttpContext.Session.GetString("role");
            ViewData["userId"] = HttpContext.Session.GetString("userId");
        }

        private async Task<List<UserViewModel>> GetAuctionAccounts()
        {
            List<UserViewModel> accounts = new List<UserViewModel>();
            var userId = HttpContext.Session.GetString("userId");
            var users = _context.Users.Where(u => u.UserId.ToString() != userId);
            var wallets = _context.Wallets.Where(u => u.UserId.ToString() != userId);
            var banks = _context.Banks.Where(u => u.UserId.ToString() != userId);

            foreach (var usr in users)
            {
                var wlt = await wallets.FirstOrDefaultAsync(u => u.UserId == usr.UserId && u.TotalCoins > 0);
                if (wlt != null)
                {
                    var bidReq = _context.BidsRequest.Where(s => s.UserId == usr.UserId && s.BidStatus == EnumTypes.BidRequestStatus.InProgress.ToString());
                    var bidReqCoins = bidReq.Sum(x => x.BidCoins);
                    var userVM = new UserViewModel
                    {
                        User = usr,
                        Wallet = wlt,
                        Bank = banks.FirstOrDefault(b => b.UserId == usr.UserId),
                        TotalCoinsInAuction = bidReq != null ? bidReqCoins : 0
                    };
                    accounts.Add(userVM);
                }
            }
            return accounts;
        }

        private async Task<AuctionViewModel> GetMainViewModel()
        {
            var userId = HttpContext.Session.GetString("userId");
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserId.ToString() == userId);
            var auction = await _context.Auctions.OrderByDescending(x => x.Id).FirstOrDefaultAsync();
            var coins = _context.Coins.Where(c => c.UserId.ToString() == userId);

            int totalPendingCoins = coins.Where(c => c.MaturityStatus == EnumTypes.CoinsMaturityStatus.Pending.ToString()).Sum(x => x.TotalCoins);


            UserViewModel userVM;
            AuctionViewModel auctionVM;

            var auctionStartTime = "n/a";
            if(auction != null)
            {
                auctionStartTime = auction.Status == EnumTypes.AuctionStatus.Pending.ToString() ? auction.StartTime.ToString() :
                                   (auction.Status == EnumTypes.AuctionStatus.Active.ToString() ? "active" : "n/a");
            }
            
            userVM = new UserViewModel
            {
                User = user,
                Bank = await _context.Banks.FirstOrDefaultAsync(b => b.UserId == user.UserId),
                Wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == user.UserId),
                NextAuctionTime = auctionStartTime,
                PendingCoins = coins.ToList(),
                TotalPendingCoins = totalPendingCoins
            };

            bool activeAuction = await _context.Auctions.OrderByDescending(x => x.Id).AnyAsync(x => x.Status == EnumTypes.AuctionStatus.Active.ToString());
            var auctionAccs = activeAuction ? await GetAuctionAccounts() : new List<UserViewModel>();
            var auctionCoins = auctionAccs.Sum(v => v.Wallet.TotalCoins);
            var bidders = auctionAccs.ConvertAll(x => new BidSent { UserId = x.User.UserId });

            auctionVM = new AuctionViewModel
            {
                UserVM = userVM,
                AuctionAccounts = auctionAccs,
                AuctionCoins = auctionCoins,
                Bidding = bidders
            };

            return auctionVM;
        }

        public async Task<IActionResult> Dashboard()
        {
            SetSessionValues();
            return View(await GetMainViewModel());
        }

        public IActionResult CoinsMaturation()
        {
            SetSessionValues();
            var userId = HttpContext.Session.GetString("userId");
            var coins = _context.Coins.Where(c => c.UserId.ToString() == userId);
            return View(coins);
        }

        public async Task<IActionResult> CoinsMaturationAdmin()
        {
            SetSessionValues();
            return View(await _context.Coins.ToListAsync());
        }

        public IActionResult BidRequests()
        {
            SetSessionValues();
            var userId = HttpContext.Session.GetString("userId");
            var openBids = _context.BidsRequest.Where(b => b.UserId.ToString() == userId);
            return View(openBids);
        }

        public async Task<IActionResult> BidRequestsAdmin()
        {
            SetSessionValues();
            return View(await _context.BidsRequest.ToListAsync());
        }
        public IActionResult SentBid()
        {
            SetSessionValues();
            var userId = HttpContext.Session.GetString("userId");
            var openBids = _context.BidsSent.Where(b => b.RequestUsersId.ToString() == userId);
            return View(openBids);
        }

        [HttpPost]
        public async Task<IActionResult> PlaceBid(BidSent newBid)
        {
            IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                var user = _context.Users.FirstOrDefault(u => u.UserId == newBid.RequestUsersId);

                newBid.BidDate = DateTime.Now;
                _context.Add(newBid);
                await _context.SaveChangesAsync();
                var lastBidReq = await _context.BidsSent.OrderByDescending(x => x.Id).FirstOrDefaultAsync();

                BidRequest bidReq = new BidRequest
                {
                    UserId = newBid.UserId,
                    BidSentId = lastBidReq.Id,
                    BidStatus = newBid.BidStatus,
                    BidCoins = newBid.BidCoins,
                    BidType = newBid.BidCoinsType,
                    BidderName = $"{user.Firstname} {user.Lastname}",
                    BidderCellphone = user.Cellphone,
                    RecipientName = newBid.RecipientName,
                    BidDate = DateTime.Now
                };

                _context.Add(bidReq);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
            return RedirectToAction(nameof(Dashboard));
        }

        public async Task<IActionResult> CancelBid(int id)
        {
            IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                var BidSent = await _context.BidsSent.FindAsync(id);
                var BidReq = await _context.BidsRequest.FirstOrDefaultAsync(x => x.BidSentId == BidSent.Id);

                BidSent.BidStatus = EnumTypes.BidRequestStatus.Cancelled.ToString();
                BidReq.BidStatus = EnumTypes.BidRequestStatus.Cancelled.ToString();

                _context.Update(BidSent);
                _context.Update(BidReq);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
            return RedirectToAction(nameof(SentBid));
        }

        public async Task<IActionResult> RejectBid(int id)
        {
            IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                var BidReq = await _context.BidsRequest.FindAsync(id);
                var BidSent = await _context.BidsSent.FirstOrDefaultAsync(x => x.Id == BidReq.BidSentId);

                BidSent.BidStatus = EnumTypes.BidRequestStatus.Rejected.ToString();
                BidReq.BidStatus = EnumTypes.BidRequestStatus.Rejected.ToString();

                _context.Update(BidSent);
                _context.Update(BidReq);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
            return RedirectToAction(nameof(BidRequests));
        }

        public async Task<IActionResult> ApproveBid(int id)
        {
            IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                var BidReq = await _context.BidsRequest.FindAsync(id);
                var BidSent = await _context.BidsSent.FirstOrDefaultAsync(x => x.Id == BidReq.BidSentId);

                BidSent.BidStatus = EnumTypes.BidRequestStatus.Approved.ToString();
                BidReq.BidStatus = EnumTypes.BidRequestStatus.Approved.ToString();

                var sellerAcc = await _context.Wallets.FirstOrDefaultAsync(u => u.UserId == BidReq.UserId);

                sellerAcc.TotalCoins -= BidReq.BidCoins;

                var profitCoins = BidSent.BidCoinsType == EnumTypes.CoinsMaturityType.HalfMaturation.ToString() ? (int)(BidSent.BidCoins * 0.5) : BidSent.BidCoins;
                var days = BidSent.BidCoinsType == EnumTypes.CoinsMaturityType.HalfMaturation.ToString() ? 3 : 5;

                var coins = new Coins
                {
                    UserId = BidSent.RequestUsersId,
                    OwnerName = BidReq.BidderName,
                    MaturationType = BidSent.BidCoinsType,
                    MaturityStatus = EnumTypes.CoinsMaturityStatus.Pending.ToString(),
                    OpeningCoins = BidSent.BidCoins,
                    ProfitCoins = profitCoins,
                    TotalCoins = BidSent.BidCoins + profitCoins,
                    MaturityDate = DateTime.Now.AddDays(days)
                };

                _context.Update(BidSent);
                _context.Update(BidReq);

                _context.Update(sellerAcc);
                _context.Add(coins);

                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
            }
            return RedirectToAction(nameof(BidRequests));
        }

        public async Task<IActionResult> MatureCoins(int id)
        {
            IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                await new CoinsMaturityExecution().MatureCoinForId(id);
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
            return RedirectToAction(nameof(CoinsMaturation));
        }

        public async Task<IActionResult> DeleteCoinMaturation(int id)
        {
            IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                var coins = await _context.Coins.FindAsync(id);
                _context.Remove(coins);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
            return RedirectToAction(nameof(CoinsMaturation));
        }

        public async Task<IActionResult> DeleteBidSent(int id)
        {
            IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                var sentBid = await _context.BidsSent.FindAsync(id);
                _context.Remove(sentBid);
                await _context.SaveChangesAsync();
            }
            catch
            {
                transaction.Rollback();
            }
            return RedirectToAction(nameof(SentBid));
        }

        public async Task<IActionResult> DeleteBidRequest(int id)
        {
            IDbContextTransaction transaction = _context.Database.BeginTransaction();
            try
            {
                var bidReq = await _context.BidsRequest.FindAsync(id);
                _context.Remove(bidReq);
                await _context.SaveChangesAsync();
                transaction.Commit();
            }
            catch
            {
                transaction.Rollback();
            }
            return RedirectToAction(nameof(BidRequests));
        }

        public async Task<IActionResult> Admin()
        {
            SetSessionValues();
            return View();
        }
    }
}
