namespace StockMarket.Domain.States
{
    internal abstract class StockMarketState : IStockMarketProcessorWithState
    {
        protected readonly StockMarketProcessorWithState stockMarket;

        internal StockMarketState(StockMarketProcessorWithState stockMarket)
        {
            this.stockMarket = stockMarket;
        }

        public virtual void OpenMarket()
        {
            throw new NotImplementedException();
        }

        public virtual void CloseMarket()
        {
            throw new NotImplementedException();
        }

        public virtual Task<long> EnqueueOrderAsync(TradeSide side, decimal price, decimal quantity)
        {
            throw new NotImplementedException();
        }

        public virtual Task<long> CancelOrderAsync(long orderId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<long> ModifyOrderAsync(long orderId, decimal price, decimal quantity)
        {
            throw new NotImplementedException();
        }
    }
}