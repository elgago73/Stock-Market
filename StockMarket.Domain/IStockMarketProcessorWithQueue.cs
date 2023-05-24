namespace StockMarket.Domain
{
    public interface IStockMarketProcessorWithQueue: IStockMarketProcessor
    {
        Task<long> EnqueueOrderAsync(TradeSide side, decimal price, decimal quantity, Guid? refId);
        Task<long> CancelOrderAsync(long orderId, Guid? refId);
        Task<long> ModifyOrderAsync(long orderId, decimal price, decimal quantity, Guid? refId);
    }
}
