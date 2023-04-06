using StockMarket.API;
using StockMarket.API.Controllers;

internal class StockMarketService : IStockMarketService
{
    public IEnumerable<Order> GetAllOrderes()
    {

        return new List<Order>
            {
                new Order { Id = 1, Side = "Buy", Price = 1500, Quantity = 1 },
                new Order { Id = 2, Side = "Sell", Price = 1400, Quantity = 1 },
            };
    }
}
