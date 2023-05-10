namespace StockMarket.Domain
{
    public interface IStockMarketProcessor
    {
        MatchContext? ResultContext { get; }

        IEnumerable<Order> Orders { get; }
        IEnumerable<Trade> Trades { get; }
    }
}