namespace StockMarket.Domain.States
{
    internal class OpenState : StockMarketState
    {
        internal OpenState(StockMarketProcessorWithState stockMarket) : base(stockMarket)
        {
        }

        public override void OpenMarket()
        {
        }

        public override void CloseMarket()
        {
            stockMarket.closeMarket();
        }

        public override async Task<long> EnqueueOrderAsync(TradeSide side, decimal price, decimal quantity, Guid? refId = null)
        {
            return await stockMarket.enqueueOrderAsync(side, price, quantity, refId);
        }

        public override async Task<long> CancelOrderAsync(long orderId, Guid? refId = null)
        {
            return await stockMarket.cancelOrderAsync(orderId, refId);
        }

        public override async Task<long> ModifyOrderAsync(long orderId, decimal price, decimal quantity, Guid? refId = null)
        {
            return await stockMarket.modifyOrderAsync(orderId, price, quantity, refId);
        }
    }
}