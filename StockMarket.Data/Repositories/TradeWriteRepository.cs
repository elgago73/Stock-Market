using StockMarket.Domain;
using StockMarket.Domain.Repositories;

namespace StockMarket.Data.Repositories
{
    public class TradeWriteRepository : ITradeWriteRepository
    {
        private readonly StockMarketDbContext dbContext;

        public TradeWriteRepository(StockMarketDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task AddAsync(IEnumerable<Trade> createdTrades)
        {

            foreach (var trade in createdTrades)
            {
                await dbContext.Trades.AddAsync(trade);

            }
        }
    }
}