using StockMarket.Domain.Commands;

namespace StockMarket.Domain
{
    public abstract class StockMarketProcessorWithQueue : StockMarketProcessor, IStockMarketProcessorWithQueue
    {
        private MarketQueue marketQueue;

        internal StockMarketProcessorWithQueue(List<Order>? allOrders = null, long lastOrderNumber = 0, long lastTradeNumber = 0)
            : base(allOrders, lastOrderNumber, lastTradeNumber)
        {
            marketQueue = new();
        }

        public virtual async Task<long> EnqueueOrderAsync(TradeSide side, decimal price, decimal quantity, Guid? refId = null)
        {
            return await marketQueue.ExecuteAsync(new EnqueueCommand(this, side, price, quantity, refId));
        }

        public virtual async Task<long> CancelOrderAsync(long orderId, Guid? refId = null)
        {
            return await marketQueue.ExecuteAsync(new CancelCommand(this, orderId, refId));
        }

        public virtual async Task<long> ModifyOrderAsync(long orderId, decimal price, decimal quantity, Guid? refId = null)
        {
            return await marketQueue.ExecuteAsync(new ModifyCommand(this, orderId, price, quantity, refId));
        }
    }
}
