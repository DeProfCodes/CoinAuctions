using CoinAuction.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinAuction.Data
{
    public class CoinAuctionContext : DbContext
    {
        public CoinAuctionContext(DbContextOptions<CoinAuctionContext> options) : base(options)
        {
        }

        public DbSet<User> Users {get; set;}
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Wallet> Wallets { get; set; }

        public DbSet<Auction> Auctions { get; set; }
    }
}
