using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CoinAuction.Data;
using CoinAuction.Models;
using CoinAuction.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoinAuction.Controllers
{
    public class DashboardController : Controller
    {
        private readonly CoinAuctionContext _context;
        
        public DashboardController(CoinAuctionContext context)
        {
            _context = context;
        }

        public IActionResult Dashboard()
        {
            var userId = TempData["LoggedInUserId"]?.ToString();
            var user = _context.Users.FirstOrDefault(u => u.UserId.ToString() == userId);
            
            UserViewModel userVM;
            if (user!=null && user.UserId != 0)
            {
                userVM = new UserViewModel
                {
                    User = user,
                    Bank = _context.Banks.FirstOrDefault(b => b.UserId == user.UserId),
                    Wallet = _context.Wallets.FirstOrDefault(w => w.UserId == user.UserId)
                };
                return View(userVM);
            }
            else
            {
                //to be confirgured......
                //1st user assumed to be logged in (if no login attempted)
                if (_context.Users.Count() != 0)
                {
                    userVM = new UserViewModel
                    {
                        User = _context.Users.FirstOrDefault(),
                        Bank = _context.Banks.FirstOrDefault(),
                        Wallet = _context.Wallets.FirstOrDefault()
                    };
                    return View(userVM);
                }
            }
            return NotFound();
        }
    }
}
