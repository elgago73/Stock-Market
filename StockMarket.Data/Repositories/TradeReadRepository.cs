using Microsoft.EntityFrameworkCore;
using StockMarket.Domain;
using StockMarket.Domain.Repositories;

namespace StockMarket.Data.Repositories
{
    public class TradeReadRepository : ITradeReadRepository
    {
        private readonly StockMarketDbContext dbContext;

        public TradeReadRepository(StockMarketDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<IEnumerable<Trade>> GetAllAsync()
        {
            return await dbContext.Trades.AsNoTracking().ToListAsync();

        }

        public async Task<long> GetLastIdAsync()
        {
            // MaxAsync vs LastAsync ?
            return await dbContext.Trades.MaxAsync(t => (long?)t.Id) ?? 0;
        }
    }
}