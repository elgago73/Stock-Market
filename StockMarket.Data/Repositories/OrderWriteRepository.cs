using StockMarket.Domain.Reposities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockMarket.Data.Repositories
{
    public class OrderWriteRepository : IOrderWriteRepository
    {
        private readonly StockMarketDbContext dbContext;

    }
}
