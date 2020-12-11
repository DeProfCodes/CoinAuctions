using CoinAuction.Data;
using CoinAuction.Helpers;
using CoinAuction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinAuction.Tasks
{
    public class AuctionExecution
    {
        private CoinAuctionContext _context;
        public AuctionExecution()
        {
            _context = new CoinAuctionContext();
        }

        public async void Run()
        {
            var auction = _context.Auctions.OrderByDescending(x => x.Id).FirstOrDefault();
            if (auction != null)
            {
                var hour = DateTime.Now.Hour;
                if (auction.Status == EnumTypes.AuctionStatus.Pending.ToString())
                {
                    if((auction.IsManualScheduled && auction.StartTime.Hour==hour) || hour == 9 || hour == 17)
                        ActivateAuction(auction);
                }
                else if (auction.Status == EnumTypes.AuctionStatus.Active.ToString())
                {
                    if ((auction.IsManualScheduled && auction.EndTime.Hour == hour) || hour == 10 || hour == 18)
                        TerminateAuction(auction);
                }
                else if(auction.Status == EnumTypes.AuctionStatus.Stopped.ToString() || auction.Status == EnumTypes.AuctionStatus.Completed.ToString())
                {
                    if (hour == 9 || hour == 17)
                    {
                        CreateFirstAuction();
                    }
                }
            }
            else
            {
                var now = DateTime.Now.Hour;
                if (now == 9 || now == 17)
                    await StartAuction();
            }
            await _context.SaveChangesAsync();
        }

        public async Task StartAuction()
        {
            if (_context.Auctions.Count() == 0)
            {
                CreateFirstAuction();
            }
            else
            {
                await ActivateAuction();
            }
        }

        public void CreateFirstAuction(bool external=false)
        {
            DateTime now = DateTime.Now;

            Auction auction = new Auction
            {
                StartTime = now,
                EndTime = now.AddHours(1),
                TotalPoolCoins = GetPoolCoins(),
                TotalSoldCoins = 0,
                Status = EnumTypes.AuctionStatus.Active.ToString(),
                IsManualScheduled = false
            };
            _context.Add(auction);

            if (external)
                _context.SaveChangesAsync();
        }

        void TerminateAuction(Auction auction)
        {
            auction.Status = EnumTypes.AuctionStatus.Completed.ToString();
            auction.TotalSoldCoins = 546;

            _context.Update(auction);

            if(!auction.IsManualScheduled)
                AddNewAuction();
        }

        public void ActivateAuction(Auction auction)
        {
            auction.TotalPoolCoins = GetPoolCoins();
            auction.Status = EnumTypes.AuctionStatus.Active.ToString();

            _context.Update(auction);
        }

        public async Task ActivateAuction(int? id = null)
        {
            var auction = id != null ? await _context.Auctions.FindAsync(id) : _context.Auctions.FirstOrDefault(u => u.Status == EnumTypes.AuctionStatus.Pending.ToString());

            auction.TotalPoolCoins = GetPoolCoins();
            auction.Status = EnumTypes.AuctionStatus.Active.ToString();

            _context.Update(auction);
            await _context.SaveChangesAsync();
        }

        void AddNewAuction()
        {
            DateTime now = DateTime.Now;

            bool morningSession = now.Hour < 11;

            Auction auction = new Auction
            {
                StartTime = morningSession ? now.AddHours(7) : now.AddHours(15),
                EndTime = morningSession ? now.AddHours(8) : now.AddHours(16),
                TotalPoolCoins = GetPoolCoins(),
                TotalSoldCoins = 0,
                Status = EnumTypes.AuctionStatus.Pending.ToString(),
                IsManualScheduled = false
            };
            _context.Add(auction);
        }

        public int GetPoolCoins()
        {
            int total = 0;
            var coins = _context.Wallets.ToList();

            foreach (var c in coins)
            {
                total += c.TotalCoins;
            }

            return total;
        }
    }
}
