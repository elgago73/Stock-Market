namespace StockMarket.Domain.Repositories
{
    public interface ITradeReadRepository
    {

        Task<IEnumerable<Trade>> GetAllTradesAsync();
        Task<long> GetLastTradeIdAsync();
    }
}