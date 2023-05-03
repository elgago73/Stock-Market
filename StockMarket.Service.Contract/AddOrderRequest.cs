namespace StockMarket.Service.Contract
{
    public class AddOrderRequest
    {
        public string Side { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }
}