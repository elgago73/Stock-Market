namespace StockMarket.Domain.Commands
{
    internal class EnqueueCommand : BaseCommand<long>
    {
        private readonly StockMarketProcessor stockMarket;
        private readonly TradeSide side;
        private readonly decimal price;
        private readonly decimal quantity;
        private readonly Guid? refId;

        internal EnqueueCommand(StockMarketProcessor stockMarket, TradeSide side, decimal price, decimal quantity, Guid? refId = null)
        {
            this.stockMarket = stockMarket;
            this.side = side;
            this.price = price;
            this.quantity = quantity;
            this.refId = refId;
        }

        protected override long SpecificExecute()
        {
            return stockMarket.EnqueueOrder(side, price, quantity, refId);
        }
    }
}