namespace StockMarket.Service.Contract
{
    public interface IStockMarketService
    {
        Task<IEnumerable<OrderResponse>> GetAllOrdersAsync();
        Task<OrderResponse?> GetOrderAsync(long id);
    }
}