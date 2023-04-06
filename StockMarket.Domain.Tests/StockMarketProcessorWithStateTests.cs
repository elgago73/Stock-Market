namespace StockMarket.Domain.Tests
{
    public class StockMarketProcessorWithStateTests
    {
        private readonly StockMarketProcessorWithState sut;

        public StockMarketProcessorWithStateTests()
        {
            sut = new();
        }

        [Fact]
        public void EnqueueOrder_should_not_work_when_stockMarket_is_closed_test()
        {
            // Arrange
            sut.CloseMarket();
            // Act
            async Task act() => await sut.EnqueueOrderAsync(TradeSide.Buy, 1500M, 1M);
            // Assert
            Assert.ThrowsAsync<NotImplementedException>(act);
        }
    }
}
