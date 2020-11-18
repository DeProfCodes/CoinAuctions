using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CoinAuction.Data;
using CoinAuction.Models;
using CoinAuction.ViewModels;
using Microsoft.AspNetCore.Http;

namespace CoinAuction.Controllers
{
    public class UsersController : Controller
    {
        private readonly CoinAuctionContext _context;

        public UsersController(CoinAuctionContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var usersVM = new UserViewModel
            {
                Users = await _context.Users.ToListAsync(),
                Banks = await _context.Banks.ToListAsync(),
                Wallets = await _context.Wallets.ToListAsync()
            };

            var auctionVM = new AuctionViewModel
            {
                UserVM = usersVM,
                Auctions = await _context.Auctions.ToListAsync()
            };

            return View(auctionVM);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
            var bank = await _context.Banks.FirstOrDefaultAsync(b => b.UserId == id);
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == id);
            
            if (user == null || bank == null || wallet == null)
            {
                return NotFound();
            }

            var usersVM = new UserViewModel
            {
                User = user,
                Bank = bank,
                Wallet = wallet
            };

            return View(usersVM);
        }

        // GET: Users/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: Users/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if(login.Username == null || login.Password == null)
            {
                login.UsernameError = login.Username == null ? "Enter username" : login.UsernameError;
                login.PasswordError = login.Password == null ? "Enter password" : login.PasswordError;
                return View(login);
            }
            if (ModelState.IsValid)
            {
                User user = await _context.Users.FirstOrDefaultAsync(u => (u.Username.ToLower() == login.Username.ToLower()) || (u.Email.ToLower() == login.Username.ToLower()));
                if (user == null)
                {
                    login.UsernameError = "This username does not exist!";
                    return View(login);
                }
                else
                {
                    if (user.Password != login.Password)
                    {
                        login.PasswordError = "Wrong password!";
                        return View(login);
                    }
                    else
                    {
                        TempData["LoggedInUserId"] = user.UserId;
                        HttpContext.Session.SetString("userId", user.UserId.ToString());
                        return RedirectToAction("Dashboard", "Dashboard");
                    }
                }
            }
            return View(login);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel userVM)
        {
            if (ModelState.IsValid)
            {
                _context.Users.Add(userVM.User);
                await _context.SaveChangesAsync();

                User user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userVM.User.Username);
                
                userVM.Bank.UserId = user.UserId;
                Wallet wallet = new Wallet { UserId = user.UserId, TotalCoins = 0 };

                _context.Banks.Add(userVM.Bank);
                _context.Wallets.Add(wallet);
                await _context.SaveChangesAsync();

                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
            var bank = await _context.Banks.FirstOrDefaultAsync(b => b.UserId == id);
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == id);

            if (user == null || bank == null || wallet == null)
            {
                return NotFound();
            }

            var usersVM = new UserViewModel
            {
                User = user,
                Bank = bank,
                Wallet = wallet
            };

            return View(usersVM);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UserViewModel userVM)
        {
            if (id != userVM.User.UserId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userVM.User);
                    _context.Update(userVM.Wallet);
                    _context.Update(userVM.Bank);
                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserExists(userVM.User.UserId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(userVM);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
            var bank = await _context.Banks.FirstOrDefaultAsync(b => b.UserId == id);
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == id);

            if (user == null || bank == null || wallet == null)
            {
                return NotFound();
            }

            var usersVM = new UserViewModel
            {
                User = user,
                Bank = bank,
                Wallet = wallet
            };

            return View(usersVM);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.Users.FindAsync(id);
            var bank = await _context.Banks.FirstOrDefaultAsync(b => b.UserId == id);
            var wallet = await _context.Wallets.FirstOrDefaultAsync(w => w.UserId == id);

            _context.Users.Remove(user);
            _context.Banks.Remove(bank);
            _context.Wallets.Remove(wallet);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.UserId == id);
        }
    }
}
