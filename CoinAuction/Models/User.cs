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

        [Required(ErrorMessage = "Email is a Required field.")]
        [EmailAddress]    
        public string Email { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 5, ErrorMessage = "Password length must be greater than or equal 5 characters.")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Required(ErrorMessage = "Mobile no. is required")]
        public string Cellphone { get; set; }

        public bool IsAdmin { get; set; }
    }
}
