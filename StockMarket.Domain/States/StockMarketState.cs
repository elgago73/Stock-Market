namespace StockMarket.Domain.States
{
    internal abstract class StockMarketState : IStockMarketProcessorWithState
    {
        protected readonly StockMarketProcessorWithState stockMarket;

        public IEnumerable<Order> Orders => throw new NotImplementedException();

        public IEnumerable<Trade> Trades => throw new NotImplementedException();

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

        public virtual Task<long> EnqueueOrderAsync(TradeSide side, decimal price, decimal quantity, Guid? refId = null)
        {
            throw new NotImplementedException();
        }

        public virtual Task<long> CancelOrderAsync(long orderId, Guid? refId = null)
        {
            throw new NotImplementedException();
        }

        public virtual Task<long> ModifyOrderAsync(long orderId, decimal price, decimal quantity, Guid? refId = null)
        {
            throw new NotImplementedException();
        }

        public MatchContext? TakeContextBy(Guid refId)
        {
            throw new NotImplementedException();
        }

    }
}