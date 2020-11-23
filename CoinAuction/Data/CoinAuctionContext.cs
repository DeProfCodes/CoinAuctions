using CoinAuction.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoinAuction.Data
{
    public class CoinAuctionContext : DbContext
    {
        /*
        public CoinAuctionContext(DbContextOptions<CoinAuctionContext> options) : base(options)
        {
        }
        */
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var configuration = new ConfigurationBuilder()
                  .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                  .AddJsonFile("appsettings.json")
                  .Build();

            optionsBuilder.UseSqlServer(configuration.GetConnectionString("CoinAuctionContext"));
        }

        public DbSet<User> Users {get; set;}
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Wallet> Wallets { get; set; }
        public DbSet<Auction> Auctions { get; set; }
        public DbSet<Coins> Coins{ get; set; }
        public DbSet<BidRequest> BidsRequest { get; set; }
        public DbSet<BidSent> BidsSent { get; set; }
    }
}
