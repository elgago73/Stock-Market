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

        public override async Task<long> EnqueueOrderAsync(TradeSide side, decimal price, decimal quantity)
        {
            return await stockMarket.enqueueOrderAsync(side, price, quantity);
        }

        public override async Task<long> CancelOrderAsync(long orderId)
        {
            return await stockMarket.cancelOrderAsync(orderId);
        }

        public override async Task<long> ModifyOrderAsync(long orderId, decimal price, decimal quantity)
        {
            return await stockMarket.modifyOrderAsync(orderId, price, quantity);
        }
    }
}