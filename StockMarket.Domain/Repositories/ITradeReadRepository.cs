namespace StockMarket.Domain.Repositories
{
    public interface ITradeReadRepository
    {

        Task<IEnumerable<Trade>> GetAllAsync();
        Task<long> GetLastIdAsync();
    }
}