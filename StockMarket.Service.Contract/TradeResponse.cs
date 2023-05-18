namespace StockMarket.API.Controllers
{
    public class TradeResponse
    {
        public long Id { get; set; }
        public long SellOrderId { get; set; }
        public long BuyOrderId { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }

    }
}