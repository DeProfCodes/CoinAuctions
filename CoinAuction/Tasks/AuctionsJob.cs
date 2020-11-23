using CoinAuction.Data;
using CoinAuction.Helpers;
using CoinAuction.Models;
using Quartz;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;

namespace CoinAuction.Tasks
{
    public class AuctionsJob : IJob
    {
  
        private AuctionExecution auction = new AuctionExecution();
        private CoinsMaturityExecution coinsMaturity = new CoinsMaturityExecution();

        public Task Execute(IJobExecutionContext context)
        {
            auction.Run();
            coinsMaturity.MatureCoins();
            return Task.FromResult(0);
        }
        
        
    }
}
