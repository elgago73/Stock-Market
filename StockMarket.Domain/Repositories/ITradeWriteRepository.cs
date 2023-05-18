namespace StockMarket.Domain.Repositories
{
    public interface ITradeWriteRepository
    {
        Task AddAsync(IEnumerable<Trade> createdTrades);
    }
}