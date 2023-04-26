namespace StockMarket.Domain.States
{
    internal class PreOpenState : StockMarketState
    {
        internal PreOpenState(StockMarketProcessorWithState stockMarket) : base(stockMarket)
        {
        }
    }
}