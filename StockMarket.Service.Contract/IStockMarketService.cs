namespace StockMarket.Service.Contract
{
    public interface IStockMarketService
    {
        Task<long> AddOrderAsync(AddOrderRequest order);
        Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
        Task<OrderResponse?> GetOrderAsync(long id);
    }
}