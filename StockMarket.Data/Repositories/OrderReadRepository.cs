﻿using Microsoft.EntityFrameworkCore;
using StockMarket.Domain;
using StockMarket.Domain.Repositories;
using System.Linq.Expressions;

namespace StockMarket.Data.Repositories
{
    public class OrderReadRepository : IOrderReadRepository
    {
        private readonly StockMarketDbContext dbContext;

        public OrderReadRepository(StockMarketDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Order?> GetAsync(long id)
        {
            return await dbContext.Orders.AsNoTracking().SingleOrDefaultAsync(o => o.Id == id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await dbContext.Orders.AsNoTracking().ToListAsync();
        }

        public async Task<List<Order>> GetAllAsync(Expression<Func<Order, bool>> prerdicate)
        {
               return await dbContext.Orders.AsNoTracking().Where(prerdicate).ToListAsync();
        }

        public async Task<long> GetLastIdAsync()
        {
            return await dbContext.Orders.MaxAsync(t => (long?)t.Id) ?? 0;
        }
    }
}
