using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinAuction.Models
{
    public class BidSent
    {
        public int Id { get; set; }
        public int RequestUsersId { get; set; }
        public int UserId { get; set; }
        public string RecipientName{ get; set; }
        public string Cellphone { get; set; }
        public string BankName { get; set; }
        public string BranchCode { get; set; }
        public string AccountNumber { get; set; }
        public string AccountType { get; set; }
        public int BidCoins { get; set; }
        public string BidCoinsType { get; set; }
        public string BidStatus { get; set; }
        public DateTime BidDate { get; set; }
    }
}
