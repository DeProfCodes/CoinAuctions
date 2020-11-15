using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CoinAuction.Models
{
    public class User
    {
        public int UserId { get; set; }
        
        [Required]
        public string Username { get; set; }
        
        [Required]
        public string Firstname { get; set; }
        
        [Required]
        public string Lastname { get; set; }

        [Required]
        [DataType(DataType.EmailAddress, ErrorMessage = "Enter a valid email address")]
        public string Email { get; set; }
        
        [Required(ErrorMessage = "Password is required")]
        [StringLength(5, ErrorMessage = "Minimum password length is 5")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Mobile no. is required")]
        public string Cellphone { get; set; }

        public bool IsAdmin { get; set; }
    }
}
