using StockMarket.Domain;
using System.Linq.Expressions;

namespace StockMarket.Domain.Repositories
{
    public interface IOrderReadRepository
    {
        Task<IEnumerable<Order>> GetAllAsync();
        Task<Order?> GetAsync(long id);
        Task<List<Order>> GetAllAsync(Expression<Func<Order, bool>> prerdicate);
        Task<long> GetLastIdAsync();
    }
}