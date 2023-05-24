namespace StockMarket.Domain.Repositories
{
    public interface ITradeReadRepository
    {

        Task<IEnumerable<Trade>> GetAllAsync();
        Task<Trade?> GetAsync(long id);
        Task<long> GetLastIdAsync();
    }
}