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
                if (auction.Status == EnumTypes.AuctionStatus.Pending.ToString())
                {
                    if(DateTime.Now.Hour == 9 || DateTime.Now.Hour == 17)
                        ActivateAuction(auction);
                }
                else if (auction.Status == EnumTypes.AuctionStatus.Active.ToString())
                {
                    if (DateTime.Now.Hour == 10 || DateTime.Now.Hour == 18)
                        TerminateAuction(auction);
                }
                else if(auction.Status == EnumTypes.AuctionStatus.Stopped.ToString())
                {
                    AddNewAuction();
                }
                await _context.SaveChangesAsync();
            }
            else
            {
                await StartAuction();
            }
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

        void CreateFirstAuction()
        {
            DateTime now = DateTime.Now;
            
            Auction auction = new Auction
            {
                StartTime = now,
                EndTime = now.AddHours(1),
                TotalPoolCoins = GetPoolCoins(),
                TotalSoldCoins = 0,
                Status = EnumTypes.AuctionStatus.Active.ToString()
            };
            _context.Add(auction);
        }

        void TerminateAuction(Auction auction)
        {
            auction.Status = EnumTypes.AuctionStatus.Completed.ToString();
            auction.TotalSoldCoins = 546;

            _context.Update(auction);

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
            bool morningSession = now.Hour == 9;

            Auction auction = new Auction
            {
                StartTime = morningSession ? now : now.AddHours(8),
                EndTime = morningSession ? now.AddHours(1) : now.AddHours(9),
                TotalPoolCoins = GetPoolCoins(),
                TotalSoldCoins = 0,
                Status = EnumTypes.AuctionStatus.Pending.ToString()
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
