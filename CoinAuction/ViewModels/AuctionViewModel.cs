using CoinAuction.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinAuction.ViewModels
{
    public class AuctionViewModel
    {
        public UserViewModel UserVM { get; set; }

        public List<Auction> Auctions { get; set; }
    }
}
