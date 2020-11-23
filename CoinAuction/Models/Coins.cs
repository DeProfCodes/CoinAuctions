using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinAuction.Models
{
    public class Coins
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string OwnerName { get; set; }
        public int OpeningCoins { get; set; }
        public int ProfitCoins { get; set; }
        public int TotalCoins { get; set; }
        public string MaturationType { get; set; }
        public DateTime MaturityDate { get; set; }
        public string MaturityStatus { get; set; }
    }
}
