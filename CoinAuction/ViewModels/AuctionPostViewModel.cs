using CoinAuction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinAuction.ViewModels
{
    public class AuctionPostViewModel
    {
        public Auction Auction { get; set; }
        public string StartTimeError { get; set; }
        public string EndTimeError { get; set; }
    }
}
