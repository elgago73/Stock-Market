namespace StockMarket.Domain.States
{
    internal class CloseState : StockMarketState
    {
        internal CloseState(StockMarketProcessorWithState stockMarket) : base(stockMarket)
        {
        }

        public override void OpenMarket()
        {
            stockMarket.openMarket();
        }

        public override void CloseMarket()
        {
        }
    }
}