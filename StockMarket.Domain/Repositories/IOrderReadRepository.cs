using StockMarket.Domain;
using System.Linq.Expressions;

namespace StockMarket.Domain.Reposities
{
    public interface IOrderReadRepository
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order?> GetOrderAsync(long id);
        Task<List<Order>> GetAllOrdersAsync(Expression<Func<Order, bool>> prerdicate);
        Task<long> GetLastOrderIdAsync();
    }
}