namespace StockMarket.Domain.Tests
{
    public class TestStockMarketProcessorWithQueue : StockMarketProcessorWithQueue
    {
    }

    public class StockMarketProcessorWithQueueTests
    {
        private readonly TestStockMarketProcessorWithQueue sut;

        public StockMarketProcessorWithQueueTests()
        {
            sut = new();
        }
    }
}
