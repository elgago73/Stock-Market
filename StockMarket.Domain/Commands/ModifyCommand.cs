namespace StockMarket.Domain.Commands
{
    internal class ModifyCommand : BaseCommand<long>
    {
        private readonly StockMarketProcessor stockMarketProcessor;
        private readonly long orderId;
        private readonly decimal price;
        private readonly decimal quantity;
        private readonly Guid? refId;

        internal ModifyCommand(StockMarketProcessor stockMarketProcessor, long orderId, decimal price, decimal quantity, Guid? refId = null)
        {
            this.stockMarketProcessor = stockMarketProcessor;
            this.orderId = orderId;
            this.price = price;
            this.quantity = quantity;
            this.refId = refId;
        }

        protected override long SpecificExecute()
        {
            return stockMarketProcessor.ModifyOrder(orderId, price, quantity, refId);
        }
    }
}