namespace StockMarket.Domain.Commands
{
    internal class ModifyCommand : BaseCommand<long>
    {
        private readonly StockMarketProcessor stockMarketProcessor;
        private readonly long orderId;
        private readonly decimal price;
        private readonly decimal quantity;

        internal ModifyCommand(StockMarketProcessor stockMarketProcessor, long orderId, decimal price, decimal quantity)
        {
            this.stockMarketProcessor = stockMarketProcessor;
            this.orderId = orderId;
            this.price = price;
            this.quantity = quantity;
        }

        protected override long SpecificExecute()
        {
            return stockMarketProcessor.ModifyOrder(orderId, price, quantity);
        }
    }
}