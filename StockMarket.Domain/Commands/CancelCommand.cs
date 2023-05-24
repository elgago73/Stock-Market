namespace StockMarket.Domain.Commands
{
    internal class CancelCommand : BaseCommand<long>
    {
        private readonly StockMarketProcessor stockMarket;
        private readonly long orderId;
        private readonly Guid? refId;

        internal CancelCommand(StockMarketProcessor stockMarket, long orderId, Guid? refId = null)
        {
            this.stockMarket = stockMarket;
            this.orderId = orderId;
            this.refId = refId;
        }

        protected override long SpecificExecute()
        {
            return stockMarket.CancelOrder(orderId, refId);
        }
    }
}