using StockMarket.Domain;

namespace StockMarket.Domain.Reposities
{
    public interface IOrderReadRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderAsync(long id);
    }
}