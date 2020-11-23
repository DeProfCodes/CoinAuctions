using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinAuction.Models
{
    public class BidRequest
    {
        public int Id { get; set; }
        public int BidSentId { get; set; }
        public int UserId { get; set; }
        public string BidderName { get; set; }
        public string RecipientName { get; set; }
        public string BidderCellphone { get; set; }
        public int BidCoins { get; set; }
        public string BidStatus { get; set; }
        public string BidType { get; set; }
        public DateTime BidDate { get; set; }
    }
}
