namespace StockMarket.Domain.Repositories
{
    public interface ITradeReadRepository
    {
        Task<long> GetLastTradeIdAsync();
    }
}