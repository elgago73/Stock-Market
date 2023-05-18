using Microsoft.EntityFrameworkCore.Infrastructure.Internal;
using StockMarket.Domain;
using StockMarket.Domain.Repositories;

namespace StockMarket.Data.Repositories
{
    public class OrderWriteRepository : IOrderWriteRepository
    {
        private readonly StockMarketDbContext dbContext;

        public OrderWriteRepository(StockMarketDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task AddAsync(Order createdOrder)
        {
            await dbContext.Orders.AddAsync(createdOrder);
        }

        public async Task SaveChangesAsync()
        {
            await dbContext.SaveChangesAsync();
        }

        public async Task UpdateAsync(IEnumerable<Order> updatedOrders)
        {
            var orderList = new List<Order?>();
            foreach(var order in updatedOrders)
            {
                orderList.Add(await FindAsync(order.Id));
            }
            var query = from newItem in updatedOrders
                        join oldItem in orderList
                        on newItem.Id equals oldItem.Id
                        select new
                        {
                            oldItem,
                            newItem
                        };  
            foreach(var item in query)
            {
                item.oldItem.UpdateBy(item.newItem);
            }
        }

        private async Task<Order?> FindAsync(long id)
        {
            return await dbContext.Orders.FindAsync(id);
        }
    }
}
