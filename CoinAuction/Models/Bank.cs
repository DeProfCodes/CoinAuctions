using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinAuction.Models
{
    public class Bank
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        
        [Required]
        public string BankName { get; set; }
        
        [Required]
        public string BranchCode { get; set; }

        [Required]
        public string AccountType { get; set; }
        
        [Required]
        public string AccountNumber { get; set; }
    }
}
