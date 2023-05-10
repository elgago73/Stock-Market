using Microsoft.EntityFrameworkCore;
using StockMarket.Domain.Repositories;

namespace StockMarket.Data
{
    public class TradeReadRepository : ITradeReadRepository
    {
        private StockMarketDbContext dbContext;

        public TradeReadRepository(StockMarketDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<long> GetLastTradeIdAsync()
        {
            // MaxAsync vs LastAsync ?
            return await dbContext.Trades.MaxAsync(t => (long?)t.Id) ?? 0;
        }
    }
}