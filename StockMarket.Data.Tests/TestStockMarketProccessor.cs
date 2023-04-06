using StockMarket.Domain;

namespace StockMarket.Data.Tests
{
    public class TestStockMarketProccessor : StockMarketProcessor
    {
        public TestStockMarketProccessor(List<Order>? orders = null, long lastOrderId = 0, long lastTradeId = 0)
            : base(orders, lastOrderId, lastTradeId)
        {
        }
    }
}