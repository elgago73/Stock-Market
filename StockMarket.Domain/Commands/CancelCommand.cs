namespace StockMarket.Domain.Commands
{
    internal class CancelCommand : BaseCommand<long>
    {
        private readonly StockMarketProcessor stockMarket;
        private readonly long orderId;

        internal CancelCommand(StockMarketProcessor stockMarket, long orderId)
        {
            this.stockMarket = stockMarket;
            this.orderId = orderId;
        }

        protected override long SpecificExecute()
        {
            return stockMarket.CancelOrder(orderId);
        }
    }
}