namespace StockMarket.Domain
{
    public interface IStockMarketProcessorWithState : IStockMarketProcessorWithQueue
    {
        void OpenMarket();
        void CloseMarket();
    }
}
