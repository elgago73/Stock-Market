﻿namespace StockMarket.Domain
{
    public interface IStockMarketProcessorWithQueue
    {
        Task<long> EnqueueOrderAsync(TradeSide side, decimal price, decimal quantity);
        Task<long> CancelOrderAsync(long orderId);
        Task<long> ModifyOrderAsync(long orderId, decimal price, decimal quantity);
    }
}
