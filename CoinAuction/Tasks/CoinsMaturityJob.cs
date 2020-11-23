using CoinAuction.Data;
using CoinAuction.Helpers;
using CoinAuction.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinAuction.Tasks
{
    public class CoinsMaturityJob:IJob
    {
        private CoinAuctionContext _context = new CoinAuctionContext();

        public Task Execute(IJobExecutionContext context)
        {
            MatureCoins();
            return Task.FromResult(0);
        }

        private void MatureCoins()
        {
            var users = _context.Users.ToList();
            foreach(var usr in users)
            {
                var coins = _context.Coins.Where(c => c.UserId == usr.UserId && c.MaturityStatus == EnumTypes.CoinsMaturityStatus.Pending.ToString());
                foreach(var c in coins)
                {
                    if(true)//DateTime.Now.CompareTo(c.MaturityDate) >= 0)
                    {
                        c.MaturityStatus = EnumTypes.CoinsMaturityStatus.Matured.ToString();

                        var wallet = _context.Wallets.FirstOrDefault(u => u.UserId == c.UserId);
                        wallet.TotalCoins += c.TotalCoins;

                        _context.Update(wallet);
                        _context.Update(c);
                    }
                }
            }
            _context.SaveChangesAsync();
        }

        double GetMaturation(Coins coin)
        {
            double total = coin.TotalCoins;

            if (coin.MaturationType == EnumTypes.CoinsMaturityType.HalfMaturation.ToString())
                total += (total * 0.50);

            else if (coin.MaturationType == EnumTypes.CoinsMaturityType.FullMaturation.ToString())
                total *= 2;

            return total;
        }
    }
}
