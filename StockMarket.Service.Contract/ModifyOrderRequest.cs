namespace StockMarket.Service.Contract
{
    public class ModifyOrderRequest
    {
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
    }
}