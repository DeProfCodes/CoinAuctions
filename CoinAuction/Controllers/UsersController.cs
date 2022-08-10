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
using CoinAuction.Helpers;
using static CoinAuction.Helpers.EnumTypes;

namespace CoinAuction.Controllers
{
    public class UsersController : Controller
    {
        private readonly CoinAuctionContext _context;

        public UsersController()
        {
            _context = new CoinAuctionContext();

        }

        // GET: Users
        [HttpGet]
        public async Task<IActionResult> Index(string searchString)
        {
            SetSessionValues();
            if (IsLoggedOut())
            {
                return Logout();
            }
            var users = from u in _context.Users select u;
            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.Username.Contains(searchString));
            }

            var usersVM = new UserViewModel
            {
                Users = await users.ToListAsync(),
                Banks = await _context.Banks.ToListAsync(),
                Wallets = await _context.Wallets.ToListAsync()
            };

            return View(usersVM);
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

        //For Development and demo purposes: Create dummy bids for dummy users to display in the front end
        private async Task DummyBids(string email)
        {
            int userId = (await _context.Users.FirstOrDefaultAsync(u => u.Email == email)).UserId;
            int dummyUserId1 = (await _context.Users.FirstOrDefaultAsync(u => u.Email == "dummy1@gmail.com")).UserId;
            int dummyUserId2 = (await _context.Users.FirstOrDefaultAsync(u => u.Email == "dummy2@gmail.com")).UserId;

            BidSent newBid = new BidSent
            {
                RecipientName = "DummyName",
                BidCoins = 3500,
                BankName = "Dummy Bank",
                AccountNumber = "1234",
                BidStatus = BidRequestStatus.InProgress.ToString(),
                BranchCode = "000",
                Cellphone = "0123",
                BidDate = DateTime.Now, 
                RequestUsersId = (userId == dummyUserId1) ? dummyUserId2 : dummyUserId1,
                UserId = (userId == dummyUserId1) ? dummyUserId2 : dummyUserId1
            };

            var user = _context.Users.FirstOrDefault(u => u.UserId == userId);

            newBid.BidDate = DateTime.Now;
            _context.Add(newBid);
            await _context.SaveChangesAsync();
            var lastBidReq = await _context.BidsSent.OrderByDescending(x => x.Id).FirstOrDefaultAsync();

            BidRequest bidReq = new BidRequest
            {
                UserId = userId,
                BidSentId = lastBidReq.Id,
                BidStatus = "Pending",
                BidCoins = newBid.BidCoins,
                BidType = CoinsMaturityType.FullMaturation.ToString(),
                BidderName = $"{user.Firstname} {user.Lastname}",
                BidderCellphone = user.Cellphone,
                RecipientName = newBid.RecipientName,
                BidDate = newBid.BidDate
            };

            _context.Add(bidReq);
            await _context.SaveChangesAsync();
        }

        //For Development and demo purposes: Create user with a lot of records to display in the front end
        private async Task CreateDummyUser(int dummyNumber, bool isAdmin = false)
        {
            UserViewModel userVM = new UserViewModel
            {
                User = new User
                {
                    Username = !isAdmin ? $"dummy{dummyNumber}" : "admin",
                    Firstname = !isAdmin ? $"Fdummy-{dummyNumber}" : "adminFname",
                    Lastname = !isAdmin ? $"Ldummy-{dummyNumber}" : "adminLname",
                    Cellphone = "073",
                    Email =  $"{(!isAdmin ? $"dummy{dummyNumber}" : "admin" )}@gmail.com",
                    IsAdmin = isAdmin,
                    Password = !isAdmin ? "dummy" : "admin"
                },
                Bank = new Bank
                {
                    AccountNumber = "123",
                    AccountType = $"dummy{dummyNumber} acc",
                    BankName = $"dummy{dummyNumber} bank",
                    BranchCode = "000"
                }
            };

            _context.Users.Add(userVM.User);
            await _context.SaveChangesAsync();

            User user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userVM.User.Username);

            userVM.Bank.UserId = user.UserId;
            Wallet wallet = new Wallet { UserId = user.UserId, TotalCoins = 178956 };

            _context.Banks.Add(userVM.Bank);
            _context.Wallets.Add(wallet);
            await _context.SaveChangesAsync();
        }

        public async Task CreateDummyUsersAndBids()
        {
            bool dummyAdmin = await _context.Users.FirstOrDefaultAsync(u => u.Username == "admin") != null;
            if (!dummyAdmin)
            {
                await CreateDummyUser(1, true);
            }

            bool dummiesInserted = await _context.Users.FirstOrDefaultAsync(u => u.Username == "dummy1") != null;
            if (!dummiesInserted)
            {
                await CreateDummyUser(1);
                await CreateDummyUser(2);

                await DummyBids("dummy1@gmail.com");
                await DummyBids("dummy1@gmail.com");
                await DummyBids("dummy2@gmail.com");
                await DummyBids("dummy2@gmail.com");
            }
        }
        // GET: Users/Login
        public IActionResult Login()
        {  
            //Temp Injection for dummy users.
            Task.Run(async () =>
            {
                await CreateDummyUsersAndBids();
            });
            return View();
        }

        // POST: Users/Login
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(Login login)
        {
            if (login.Username == null || login.Password == null)
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
                        var role = user.IsAdmin ? EnumTypes.Role.Admin.ToString() : EnumTypes.Role.User.ToString();
                        HttpContext.Session.SetString("userId", user.UserId.ToString());
                        HttpContext.Session.SetString("role", role);
                        ViewData["userId"] = user.UserId.ToString();
                        ViewData["role"] = role;

                        if (role == EnumTypes.Role.User.ToString())
                            return RedirectToAction("Dashboard", "Dashboard");

                        if (role == EnumTypes.Role.Admin.ToString())
                            return RedirectToAction("Admin", "Dashboard");
                    }
                }
            }
            return View(login);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.SetString("userId", "");
            HttpContext.Session.SetString("role", "");
            ViewData["userId"] = null;
            ViewData["role"] = null;

            return RedirectToAction("Index", "Home");
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            SetSessionValues();
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UserViewModel userVM)
        {
            SetSessionValues();

            if (ModelState.IsValid)
            {
                var loadByUsername = await _context.Users.FirstOrDefaultAsync(u => u.Username == userVM.User.Username);
                if (loadByUsername != null)
                {
                    userVM.UsernameError = "This username already exist!";
                    return View(userVM);
                }

                var loadByEmail = await _context.Users.FirstOrDefaultAsync(u => u.Email == userVM.User.Email);
                if (loadByEmail != null)
                {
                    userVM.EmailError = "This email already exist!";
                    return View(userVM);
                }

                if (_context.Users.Count() == 0)
                {
                    userVM.User.IsAdmin = true;
                }
                else
                {
                    userVM.User.IsAdmin = false;
                }

                _context.Users.Add(userVM.User);
                await _context.SaveChangesAsync();

                User user = await _context.Users.FirstOrDefaultAsync(u => u.Username == userVM.User.Username);



                userVM.Bank.UserId = user.UserId;
                Wallet wallet = new Wallet { UserId = user.UserId, TotalCoins = 0 };

                _context.Banks.Add(userVM.Bank);
                _context.Wallets.Add(wallet);
                await _context.SaveChangesAsync();

                if (string.IsNullOrEmpty((string)ViewData["role"]))
                {
                    HttpContext.Session.SetString("userId", user.UserId.ToString());
                    HttpContext.Session.SetString("role", user.IsAdmin ? EnumTypes.Role.Admin.ToString() : EnumTypes.Role.User.ToString());

                    return RedirectToAction("Dashboard", "Dashboard");
                }
                else if ((string)ViewData["role"] == EnumTypes.Role.Admin.ToString())
                {
                    return RedirectToAction("Index", "Users");
                }
            }
            return View();
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            SetSessionValues();
            if (IsLoggedOut())
            {
                return Logout();
            }
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
            SetSessionValues();
            if (IsLoggedOut())
            {
                return Logout();
            }
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
        public async Task<IActionResult> Delete2(int? id)
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

        public async Task<IActionResult> Delete(int id)
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

        public void SetSessionValues()
        {
            ViewData["role"] = HttpContext.Session.GetString("role");
            ViewData["userId"] = HttpContext.Session.GetString("userId");
        }

        public bool IsLoggedOut()
        {
            return (string.IsNullOrEmpty(HttpContext.Session.GetString("userId")));
        }

    }
}
