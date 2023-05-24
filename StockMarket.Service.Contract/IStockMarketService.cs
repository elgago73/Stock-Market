using StockMarket.API.Controllers;

namespace StockMarket.Service.Contract
{
    public interface IStockMarketService
    {
        Task<long> AddOrderAsync(AddOrderRequest order);
        Task<long> ModifyOrderAsync(long id, ModifyOrderRequest order);
        Task<long> CancleOrderAsync(long id);
        Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
        Task<IEnumerable<TradeResponse>> GetAllTradesAsync();
        Task<OrderResponse?> GetOrderAsync(long id);
        Task<TradeResponse?> GetTradeAsync(long id);
    }
}