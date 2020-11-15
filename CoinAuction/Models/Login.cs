using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinAuction.Models
{
    public class Login
    {
        [Required]
        public string  Username { get; set; }
        public string UsernameError { get; set; }
        
        [Required]
        public string Password { get; set; }
        public string PasswordError { get; set; }

    }
}
