using StockMarket.Domain;

namespace StockMarket.Service
{
    public interface IOrderReadRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderAsync(long id);
    }
}