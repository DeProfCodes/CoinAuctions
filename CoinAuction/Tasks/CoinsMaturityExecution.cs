using CoinAuction.Data;
using CoinAuction.Helpers;
using CoinAuction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinAuction.Tasks
{
    public class CoinsMaturityExecution
    {
        private readonly CoinAuctionContext _context;
        public CoinsMaturityExecution()
        {
            _context = new CoinAuctionContext();
        }

        public void MatureCoins()
        {
            var users = _context.Users.ToList();
            foreach (var usr in users)
            {
                var coins = _context.Coins.Where(c => c.UserId == usr.UserId && c.MaturityStatus == EnumTypes.CoinsMaturityStatus.Pending.ToString());
                foreach (var c in coins)
                {
                    MatureCoinForId(c.Id, true);
                }
            }
            _context.SaveChangesAsync();
        }

        public async Task MatureCoinForId(int id, bool allCoins = false)
        {
            var coins = await _context.Coins.FindAsync(id);
            coins.MaturityStatus = EnumTypes.CoinsMaturityStatus.Matured.ToString();

            var wallet = _context.Wallets.FirstOrDefault(u => u.UserId == coins.UserId);
            wallet.TotalCoins += coins.TotalCoins;

            _context.Update(wallet);
            _context.Update(coins);

            if (!allCoins)
                await _context.SaveChangesAsync();
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
