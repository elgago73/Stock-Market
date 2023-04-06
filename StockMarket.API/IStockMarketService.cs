namespace StockMarket.API.Controllers
{
    public interface IStockMarketService
    {
        IEnumerable<Order> GetAllOrderes();
    }
}