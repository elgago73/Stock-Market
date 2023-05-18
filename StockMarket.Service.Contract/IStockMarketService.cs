using StockMarket.API.Controllers;

namespace StockMarket.Service.Contract
{
    public interface IStockMarketService
    {
        Task<long> AddOrderAsync(AddOrderRequest order);
        Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
        Task<IEnumerable<TradeResponse>> GetAllTradesAsync();
        Task<OrderResponse?> GetOrderAsync(long id);

    }
}