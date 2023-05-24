namespace StockMarket.Domain
{
    public interface IStockMarketProcessor
    {
        IEnumerable<Order> Orders { get; }
        IEnumerable<Trade> Trades { get; }
        MatchContext? TakeContextBy(Guid refId);

    }
}