using CoinAuction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinAuction.ViewModels
{
    public class UserViewModel
    {
        public User User { get; set; }

        public Bank Bank { get; set; }

        public Wallet Wallet { get; set; }

        public List<User> Users { get; set; }

        public List<Bank> Banks { get; set; }

        public List<Wallet> Wallets { get; set; }

        public string NextAuctionTime { get; set; }
        public List<Coins> PendingCoins { get; set; }
        public int TotalPendingCoins { get; set; }
        public int TotalCoinsInAuction { get; set; }

        public string FilterString{ get; set; }
        public string UsernameError { get; set; }
        public string EmailError { get; set; }

    }
}
