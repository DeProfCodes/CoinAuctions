using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinAuction.Models
{
    public class Wallet
    {
        public int Id { get; set; }
        public int TotalCoins { get; set; }
        public int UserId { get; set; }
    }
}
