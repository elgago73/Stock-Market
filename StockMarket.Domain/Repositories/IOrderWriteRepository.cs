namespace StockMarket.Domain.Repositories
{
    public interface IOrderWriteRepository
    {
        Task AddAsync(Order createdOrder);
        Task SaveChangesAsync();
        Task UpdateAsync(IEnumerable<Order> updatedOrders);
    }
}