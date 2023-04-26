using Microsoft.EntityFrameworkCore;
using StockMarket.Domain;
using StockMarket.Domain.Reposities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Data.Repositories
{
    public class OrderReadRepository : IOrderReadRepository
    {
        private readonly StockMarketDbContext dbContext;

        public OrderReadRepository(StockMarketDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Order?> GetOrderAsync(long id)
        {
            return await dbContext.Orders.AsNoTracking().SingleOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await dbContext.Orders.AsNoTracking().ToListAsync();
        }
    }
}
